using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningFlash : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private Material hitMat;
    private Material originMaterial;

    [SerializeField] private float flashDuratrion;
    private float flashTimer;
    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originMaterial = sr.material;
        flashTimer = flashDuratrion;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        yield return new WaitForSeconds(.15f);
        sr.material = originMaterial;
    }

    private void Update()
    {
        flashTimer -= Time.deltaTime;
        if (flashTimer < 0)
        {
            flashTimer = flashDuratrion;
            StartCoroutine(FlashFX());
        }
    }
}
