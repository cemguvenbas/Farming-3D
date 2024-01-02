using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform cropRenderer;
    [SerializeField] private ParticleSystem harvestedParticles;
    public void ScaleUp()
    {
        //cropRenderer.localScale = Vector3.one;
        cropRenderer.gameObject.LeanScale(Vector3.one, 1).setEase(LeanTweenType.easeOutBack);
    }

    public void ScaleDown()
    {
        //cropRenderer.localScale = Vector3.one;
        cropRenderer.gameObject.LeanScale(Vector3.zero, 1).
            setEase(LeanTweenType.easeOutBack).setOnComplete(() => Destroy(gameObject));

        harvestedParticles.transform.parent = null;
        harvestedParticles.gameObject.SetActive(true);
        harvestedParticles.Play();
    }

}
