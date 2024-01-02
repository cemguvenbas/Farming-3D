using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform cropContainersParent;
    [SerializeField] private UICropContainer uiCropContainerPrefab;

    public void Configure(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();
        for (int i = 0; i < items.Length; i++)
        {
            UICropContainer cropContainerInstance = Instantiate(uiCropContainerPrefab, cropContainersParent);

            Sprite cropIcon = DataManager.instance.GetCropSpriteFromCropType(items[i].cropType);

            cropContainerInstance.Configure(cropIcon, items[i].amount);
        }
    }

    //public void UpdateDisplay(Inventory inventory)
    //{
    //    InventoryItem[] items = inventory.GetInventoryItems();

    //    while (cropContainersParent.childCount > 0)
    //    {
    //        Transform container = cropContainersParent.GetChild(0);
    //        container.SetParent(null);
    //        Destroy(container.gameObject);
    //    }

    //    Configure(inventory);

    //    //for (int i = 0; i < items.Length; i++)
    //    //{
    //    //    UICropContainer cropContainerInstance = Instantiate(uiCropContainerPrefab, cropContainersParent);

    //    //    Sprite cropIcon = DataManager.instance.GetCropSpriteFromCropType(items[i].cropType);

    //    //    cropContainerInstance.Configure(cropIcon, items[i].amount);
    //    //}
    //}

    public void UpdateDisplay(Inventory inventory) 
    {
        InventoryItem[] items = inventory.GetInventoryItems();

        for (int i = 0; i < items.Length; i++)
        {
            UICropContainer containerInstance;

            if (i < cropContainersParent.childCount)
            {
                containerInstance = cropContainersParent.GetChild(i).GetComponent<UICropContainer>();
                containerInstance.gameObject.SetActive(true);
            }
            else 
                containerInstance = Instantiate(uiCropContainerPrefab, cropContainersParent);

            Sprite cropIcon = DataManager.instance.GetCropSpriteFromCropType(items[i].cropType);
            containerInstance.Configure(cropIcon, items[i].amount);
        }

        int remainingContainers = cropContainersParent.childCount - items.Length;
        if (remainingContainers <= 0)
            return;

        for (int i = 0; i < remainingContainers; i++)
            cropContainersParent.GetChild(items.Length + i).gameObject.SetActive(false);

    }
}
