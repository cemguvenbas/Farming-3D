using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerToolSelector))]
public class PlayerHarvestAbility : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform harvestSphere;
    private PlayerAnimator playerAnimator;
    private PlayerToolSelector playerToolSelector;

    [Header("Settings")]
    private CropField currentCropField;
    private bool canHarvest;

    private void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerToolSelector = GetComponent<PlayerToolSelector>();

        //WaterParticles.onWaterCollided += WaterCollidedCallback;
        CropField.onFullyHarvested += CropFieldFullyHarvestedCallback;
        playerToolSelector.onToolSelected += ToolSelectedCallback;
    }

    private void OnDestroy()
    {
        //WaterParticles.onWaterCollided -= WaterCollidedCallback;
        CropField.onFullyHarvested -= CropFieldFullyHarvestedCallback;
        playerToolSelector.onToolSelected -= ToolSelectedCallback;
    }

    private void ToolSelectedCallback(PlayerToolSelector.Tool selectedTool)
    {
        if (!playerToolSelector.CanHarvest())
            playerAnimator.StopHarvestAnimation();
    }

    private void CropFieldFullyHarvestedCallback(CropField cropField)
    {
        if (cropField == currentCropField)
            playerAnimator.StopHarvestAnimation();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsWatered())
        {
            currentCropField = other.GetComponent<CropField>();
            EnteredCropField(currentCropField);
        }
    }

    private void EnteredCropField(CropField cropField)
    {
        if (playerToolSelector.CanHarvest())
        {
            if (currentCropField == null)
                currentCropField = cropField;

            playerAnimator.PlayHarvestAnimation();

            if (canHarvest)
                currentCropField.Harvest(harvestSphere);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsWatered())
            EnteredCropField(other.GetComponent<CropField>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            playerAnimator.StopHarvestAnimation();
            currentCropField = null;
        }
    }

    public void HarvestingStartedCallback()
    {
        canHarvest = true;
    }

    public void HarvestingStoppedCallback()
    {
        canHarvest = false;
    }
}
