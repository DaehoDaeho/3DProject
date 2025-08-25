using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/// <summary>
/// [역할] 미니보스: 추적 + 근접 공격(윈드업 후 타격) + 쿨다운.
/// 단순/확실: if/코루틴만 사용.
/// </summary>
[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class MiniBossAI : MonoBehaviour
{
    [Header("타깃/기본")]
    public Transform target;           // 보통 Player
    public float repathInterval = 0.2f;
    public float attackRange = 2.0f;

    [Header("공격 수치")]
    public float windupTime = 0.45f;   // 공격 전 준비시간
    public float attackCooldown = 1.2f;
    public int damage = 20;

    [Header("애니메이터")]
    public string tAttack = "Attack";

    NavMeshAgent agent;
    Animator anim;
    float pathTimer;
    float lastAttackTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        if (target == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) target = p.transform;
        }
        lastAttackTime = -999f;
    }

    void Update()
    {
        if (target == null) return;

        // 추적 경로 갱신(주기적으로)
        pathTimer += Time.deltaTime;
        if (pathTimer >= repathInterval)
        {
            agent.SetDestination(target.position);
            pathTimer = 0f;
        }

        // 사정거리 체크 + 쿨다운 체크
        float dist = Vector3.Distance(transform.position, target.position);
        if (dist <= attackRange)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                StartCoroutine(CoAttack());
            }
        }
    }

    IEnumerator CoAttack()
    {
        lastAttackTime = Time.time;

        // 공격 애니메이션 트리거
        anim.SetTrigger(tAttack);

        // 윈드업 대기(플레이어가 피할 수 있는 시간)
        yield return new WaitForSeconds(windupTime);

        // 여전히 사정거리면 데미지 적용
        if (target != null)
        {
            float dist = Vector3.Distance(transform.position, target.position);
            if (dist <= attackRange + 0.2f)
            {
                // 플레이어 Health 찾아서 데미지
                Health hp = target.GetComponent<Health>();
                if (hp != null)
                {
                    Vector3 hitPoint = target.position + Vector3.up * 1.0f;
                    hp.TakeDamage(damage, hitPoint);
                }
            }
        }
    }
}
