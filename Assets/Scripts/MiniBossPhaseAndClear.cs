using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// [역할] 보스 페이즈2 적용(HP 50%↓에서 스펙 상향) + 사망 시 클리어 처리.
/// </summary>
public class MiniBossPhaseAndClear : MonoBehaviour
{
    public Health bossHealth;
    public MiniBossAI bossAI;
    public NavMeshAgent agent;

    [Header("Phase2 조정")]
    public float phase2HpRate = 0.5f;
    public float speedBoost = 1.5f;       // 에이전트 속도 배수
    public float cooldownFactor = 0.7f;   // 공격 쿨다운 곱(작아질수록 자주 침)

    [Header("클리어 처리")]
    public DoorUnlock door;       // 문 열기(선택)
    public LevelClearUI clearUI;  // "STAGE CLEAR" 표시(선택)

    bool phase2Done = false;
    bool cleared = false;

    void Awake()
    {
        if (bossHealth == null) bossHealth = GetComponent<Health>();
        if (bossAI == null) bossAI = GetComponent<MiniBossAI>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (bossHealth == null) return;

        // 이미 클리어면 더 볼 필요 없음
        if (!cleared && bossHealth.hp <= 0)
        {
            cleared = true;
            OnBossDead();
            return;
        }

        // Phase2 전환 체크(한 번만)
        if (!phase2Done)
        {
            float rate = (float)bossHealth.hp / (float)bossHealth.maxHP;
            if (rate <= phase2HpRate)
            {
                phase2Done = true;
                ApplyPhase2();
            }
        }
    }

    void ApplyPhase2()
    {
        // 속도/쿨다운/기타를 강화
        if (agent != null) agent.speed = agent.speed * speedBoost;
        if (bossAI != null) bossAI.attackCooldown = bossAI.attackCooldown * cooldownFactor;
    }

    void OnBossDead()
    {
        // 문 열기 + 클리어 UI
        if (door != null) door.Open();
        if (clearUI != null) clearUI.Show();
    }
}
