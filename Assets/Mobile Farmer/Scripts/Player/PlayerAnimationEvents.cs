using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerAnimationEvents : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private ParticleSystem seedParticles;
    [SerializeField] private ParticleSystem waterParticles;

    [Header("Events")]
    [SerializeField] private UnityEvent startHarvestingEvent;
    [SerializeField] private UnityEvent stopHarvestingEvent;

    private void PlaySeedParticles()
    {
        seedParticles.Play();
    }

    private void PlayWaterParticles()
    {
        waterParticles.Play();
    }

    private void StartHarvestingCallback()
    {
        startHarvestingEvent?.Invoke();
    }

    private void StopHarvestingCallback()
    {
        stopHarvestingEvent?.Invoke();
    }
}
