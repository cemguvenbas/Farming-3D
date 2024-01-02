using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform world;
    Chunk[,] grid;

    [Header("Settings")]
    [SerializeField] private int gridSize;
    [SerializeField] private int gridScale;

    [Header("Data")]
    private WorldData worldData;
    private string dataPath;
    private bool shouldSave;
    private void Awake()
    {
        Chunk.onUnlocked += ChunkUnlockedCallback;
        Chunk.onPriceChanged += ChunkPriceChangedCallback;
    }
    private void Start()
    {
        dataPath = Application.dataPath + "/worldData.txt";

        LoadWorld();
        Initialize();

        InvokeRepeating("TrySaveGame", 1, 1);
    }
    private void OnDestroy()
    {
        Chunk.onUnlocked -= ChunkUnlockedCallback;
        Chunk.onPriceChanged -= ChunkPriceChangedCallback;

    }

    private void Initialize()
    {
        for (int i = 0; i < world.childCount; i++)       
            world.GetChild(i).GetComponent<Chunk>().Initialize(worldData.chunkPrices[i]);

        InitializeGrid();
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if(grid[x,y] != null)
                    Debug.Log(grid[x, y].name);
            }
        }
    }

    private void InitializeGrid()
    {
        grid = new Chunk[gridSize, gridSize];
        for (int i = 0; i < world.childCount; i++)
        {
            Chunk chunk = world.GetChild(i).GetComponent<Chunk>();

            Vector2Int chunkGridPosition = new Vector2Int((int)chunk.transform.position.x / gridScale,
                (int)chunk.transform.position.z / gridScale);

            chunkGridPosition += new Vector2Int(gridSize / 2, gridSize / 2);
            grid[chunkGridPosition.x, chunkGridPosition.y] = chunk;
        }
    }

    private void TrySaveGame()
    {
        Debug.Log("Trying to save");

        if (shouldSave)
        {
            SaveWorld();
            shouldSave = false;
        }
    }

    private void ChunkUnlockedCallback()
    {
        Debug.Log("Chunk Unlocked!");
        SaveWorld();
    }

    private void ChunkPriceChangedCallback()
    {
        shouldSave = true;
    }

    private void LoadWorld()
    {
        string data = "";
        if (!File.Exists(dataPath))
        {
            FileStream fs = new FileStream(dataPath, FileMode.Create);
            worldData = new WorldData();

            for (int i = 0; i < world.childCount; i++)
            {
                int chunkInitialPrice = world.GetChild(i).GetComponent<Chunk>().GetInitialPrice();
                worldData.chunkPrices.Add(chunkInitialPrice);

            }

            string worldDataString = JsonUtility.ToJson(worldData, true);

            byte[] worldDataBytes = Encoding.UTF8.GetBytes(worldDataString);

            fs.Write(worldDataBytes);

            fs.Close();
        }
        else
        {
            data = File.ReadAllText(dataPath);
            worldData = JsonUtility.FromJson<WorldData>(data);

            if (worldData.chunkPrices.Count < world.childCount)
                UpdateData();
        }
    }

    private void UpdateData()
    {
        //How many chunks are missing in our data
        int missingData = world.childCount - worldData.chunkPrices.Count;
        for (int i = 0; i < missingData; i++)
        {
            int chunkIndex = world.childCount - missingData + i;
            int chunkPrice = world.GetChild(chunkIndex).GetComponent<Chunk>().GetInitialPrice();
            worldData.chunkPrices.Add(chunkPrice);
        }
    }

    private void SaveWorld()
    {
        if (worldData.chunkPrices.Count != world.childCount)
            worldData = new WorldData();

        for (int i = 0; i < world.childCount; i++)
        {
            int chunkCurrentPrice = world.GetChild(i).GetComponent<Chunk>().GetCurrentPrice();

            if (worldData.chunkPrices.Count > i)
                worldData.chunkPrices[i] = chunkCurrentPrice;
            else
                worldData.chunkPrices.Add(chunkCurrentPrice);
        }

        string data = JsonUtility.ToJson(worldData, true);

        File.WriteAllText(dataPath, data);

        Debug.LogWarning("Data Saved! ");
    }
}
