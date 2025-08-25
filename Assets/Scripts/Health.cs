using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 체력/피격 처리. 무적 시간으로 연타 방지, 사망 이벤트 제공.
/// </summary>
public class Health : MonoBehaviour, IDamageable
{
    public int maxHP = 100;
    public float invincibleTime = 0.2f;
    public UnityEvent onDeath;

    [HideInInspector] public int hp;
    float invEnd;

    public bool isInvincible = false;

    void Awake() { hp = maxHP; }

    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        Debug.Log("Apply Damage");

        if (hp <= 0)
        {
            return;
        }

        if (Time.time < invincibleTime)
        {
            return;
        }

        if (isInvincible == true)
        {
            return; // ← i-프레임 중이면 무시
        }

        if (Time.time < invEnd) return; // 무적 시간
        hp = Mathf.Max(0, hp - amount);
        invEnd = Time.time + invincibleTime;

        DamageFlash flash = GetComponent<DamageFlash>();
        if(flash != null)
        {
            flash.Flash();
        }

        if (hp <= 0)
        {
            onDeath?.Invoke();
            // TODO: 사망 애니메이션/리스폰/비활성 등
            Destroy(gameObject, 1.0f);
        }
    }
}