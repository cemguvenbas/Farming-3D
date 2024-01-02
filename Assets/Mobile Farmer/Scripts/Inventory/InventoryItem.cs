using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem 
{
    public CropType cropType;
    public int amount;

    public InventoryItem(CropType cropType, int amount)
    {
        this.cropType = cropType;
        this.amount = amount;
    }
}
