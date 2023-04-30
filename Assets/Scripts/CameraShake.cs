using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private bool isShaking = false;

    IEnumerator ShakeRoutine(float duration, float intensity)
    {
        isShaking = true;
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
        isShaking = false;
    }
    public void Shake(float duration, float intensity)
    {
        if (!isShaking)
            StartCoroutine(ShakeRoutine(duration, intensity));
    }
}