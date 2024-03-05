using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitReceiveFlashEffect : MonoBehaviour
{
    [SerializeField] private Material _flashMaterial;
    private Material _defaultMaterial;


    public void GetFlashEffect(SpriteRenderer spriteRenderer)
    {
        if (_defaultMaterial == null)
        {
            _defaultMaterial = spriteRenderer.material;
        }
        StartCoroutine(FlashEffect(spriteRenderer));
    }

    private IEnumerator FlashEffect(SpriteRenderer spriteRenderer)
    {

        spriteRenderer.material = _flashMaterial;
        yield return new WaitForSeconds(0.2f);
        if (spriteRenderer != null)
        {
            spriteRenderer.material = _defaultMaterial;
        }

    }
}
