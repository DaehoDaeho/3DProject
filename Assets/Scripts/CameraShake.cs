using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    Vector3 originalPos;
    Coroutine co;

    void Awake()
    {
        originalPos = transform.localPosition;
    }

    public void Shake(float duration, float amplitude)
    {
        if (co != null) StopCoroutine(co);
        co = StartCoroutine(CoShake(duration, amplitude));
    }

    IEnumerator CoShake(float duration, float amplitude)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float rx = (Random.value - 0.5f) * 2f * amplitude;
            float ry = (Random.value - 0.5f) * 2f * amplitude;
            transform.localPosition = originalPos + new Vector3(rx, ry, 0f);
            yield return null;
        }
        transform.localPosition = originalPos;
        co = null;
    }
}
