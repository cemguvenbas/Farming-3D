using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    public static CashManager instance;

    [Header("Settings")]
    private int coins;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LoadData();
        UpdateCoinContainers();
    }
    public void UseCoins(int amount)
    {
        AddCoins(-amount);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinContainers();

        SaveData();
    }

    private void UpdateCoinContainers()
    {
        GameObject[] coinContainers = GameObject.FindGameObjectsWithTag("CoinAmount");
        foreach (var coinContainer in coinContainers)
            coinContainer.GetComponent<TextMeshProUGUI>().text = coins.ToString();
    }

    public int GetCoins()
    {
        return coins;
    }

    [NaughtyAttributes.Button]
    private void Add500Coins()
    {
        AddCoins(500);
    }

    private void LoadData()
    {
        coins = PlayerPrefs.GetInt("Coins");
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("Coins", coins);
    }


}
