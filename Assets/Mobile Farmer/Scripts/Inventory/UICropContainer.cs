using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICropContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;

    public void Configure(Sprite icon, int amount)
    {
        iconImage.sprite = icon;
        amountText.text = amount.ToString();
    }

    public void UpdateDisplay(int amount)
    {
        amountText.text = amount.ToString();
    }
}
