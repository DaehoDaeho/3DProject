using UnityEngine;

/// <summary>
/// 대미지 텍스트를 생성하는 역할.
/// </summary>
public class DamageTextSpawner : MonoBehaviour
{
    public DamageText textPrefab;

    public void Spawn(int amount, Vector3 worldPos)
    {
        if(textPrefab == null)
        {
            return;
        }

        var t = Instantiate(textPrefab, worldPos, Quaternion.identity);
        t.SetText(amount.ToString());
    }
}
