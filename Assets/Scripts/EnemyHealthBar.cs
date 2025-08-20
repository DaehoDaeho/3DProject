using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 대상의 HP를 읽어서 월드 공간에 띄워진 HP 바를 갱신하고, 머리 위에 띄운다.
/// </summary>
public class EnemyHealthBar : MonoBehaviour
{
    public Health targetHealth;
    public Image fill;
    public bool hideWhenFull = true;

    public Transform target;
    public Vector3 offset = new Vector3(0, 0.5f, 0.0f);

    private void LateUpdate()
    {
        if(targetHealth == null || fill == null || target == null)
        {
            return;
        }

        float ratio = (float)targetHealth.hp / targetHealth.maxHP;
        fill.fillAmount = Mathf.Clamp01(ratio);

        transform.position = target.position + offset;

        if(hideWhenFull == true)
        {
            gameObject.SetActive(ratio < 0.999f);
        }
    }
}
