using UnityEngine;

/// <summary>
/// [역할] 스태미나 관리: 소비, 지연 후 회복.
/// 어려운 문법 없이 필드 + 메서드 + Update만 사용.
/// </summary>
public class PlayerStamina : MonoBehaviour
{
    public float max = 100f;        // 최대 스태미나
    public float current = 100f;    // 현재 스태미나
    public float regenRate = 20f;   // 초당 회복량
    public float regenDelay = 1.0f; // 소비 후 회복 대기 시간(초)

    float regenResumeTime;          // 언제부터 회복 재개할지(월드 시간)

    void Awake()
    {
        current = max;
        regenResumeTime = 0f;
    }

    public bool Has(float amount)
    {
        return current >= amount;
    }

    public void Use(float amount)
    {
        current -= amount;
        if (current < 0f)
        {
            current = 0f;
        }

        // 회복은 잠깐 멈췄다가 나중에 다시 시작
        regenResumeTime = Time.time + regenDelay;
    }

    void Update()
    {
        // 회복 재개 시간 지났으면 회복
        if (Time.time >= regenResumeTime)
        {
            current += regenRate * Time.deltaTime;
            if (current > max)
            {
                current = max;
            }
        }
    }
}
