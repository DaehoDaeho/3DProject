using UnityEngine;
using System.Collections;

public class DamageFlash : MonoBehaviour
{
    public Renderer[] renderers;     // 반짝일 대상
    public Color flashColor = Color.white;
    public float flashAmount = 0.6f; // 원래색과 섞는 비율
    public float duration = 0.12f;   // 유지
    public float fadeTime = 0.15f;   // 복귀

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(CoFlash());
    }

    IEnumerator CoFlash()
    {
        // 원래 색 저장
        int len = renderers.Length;
        Color[] originals = new Color[len];
        int i = 0;
        while (i < len)
        {
            if (renderers[i] != null && renderers[i].material.HasProperty("_Color"))
            {
                originals[i] = renderers[i].material.color;
            }
            i = i + 1;
        }

        // 즉시 밝게
        i = 0;
        while (i < len)
        {
            if (renderers[i] != null && renderers[i].material.HasProperty("_Color"))
            {
                Color target = Color.Lerp(originals[i], flashColor, flashAmount);
                renderers[i].material.color = target;
            }
            i = i + 1;
        }

        yield return new WaitForSeconds(duration);

        // 서서히 원복
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float k = t / fadeTime;
            if (k > 1f) k = 1f;

            i = 0;
            while (i < len)
            {
                if (renderers[i] != null && renderers[i].material.HasProperty("_Color"))
                {
                    Color cur = renderers[i].material.color;
                    Color back = Color.Lerp(cur, originals[i], k);
                    renderers[i].material.color = back;
                }
                i = i + 1;
            }
            yield return null;
        }

        // 최종 원복
        i = 0;
        while (i < len)
        {
            if (renderers[i] != null && renderers[i].material.HasProperty("_Color"))
            {
                renderers[i].material.color = originals[i];
            }
            i = i + 1;
        }
    }
}
