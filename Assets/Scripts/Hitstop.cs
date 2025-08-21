using UnityEngine;
using System.Collections;

public class Hitstop : MonoBehaviour
{
    public void Stop(float duration, float slowScale)
    {
        StartCoroutine(CoStop(duration, slowScale));
    }

    IEnumerator CoStop(float duration, float slowScale)
    {
        float oldScale = Time.timeScale;
        float oldFixed = Time.fixedDeltaTime;

        Time.timeScale = slowScale;
        Time.fixedDeltaTime = oldFixed * slowScale;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = oldScale;
        Time.fixedDeltaTime = oldFixed;
    }
}
