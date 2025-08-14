using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseAgentPro : MonoBehaviour
{
    public Transform target;            // 추적 대상(보통 Player, 태그 "Player")
    private NavMeshAgent agent;

    public float repathInterval = 0.2f;
    float timer;

    public bool useVisionCheck = false; // true면 아래 조건 만족시에만 추적
    public float viewDistance = 12f;    // 최대 감지 거리
    public float viewAngle = 80f; // 좌우 시야각 절반(총 160도)
    public LayerMask obstacleMask;      // 시야를 가리는 레이어(벽/지형)

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // target 미지정 시, "Player" 태그 자동 탐색
        if (target == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                target = p.transform;
            }
        }
    }

    void Update()
    {
        if (agent == null || agent.enabled == false || target == null)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer < repathInterval)
        {
            return; // 주기가 안 됐으면 대기
        }
        
        timer = 0f;

        // 시야 체크 옵션: 보일 때만 추적
        if (useVisionCheck == true && HasLineOfSight(target) == false)
        {
            // 보이지 않으면 목적지 설정 안 함(제자리/이전 경로 유지)
            return;
        }

        // 목적지를 플레이어 현재 위치로 갱신
        agent.SetDestination(target.position);
    }

    /// <summary>
    /// 플레이어가 '시야 거리/각도' 내에 있고, 장애물에 가려지지 않으면 true
    /// </summary>
    bool HasLineOfSight(Transform t)
    {
        Vector3 toTarget = t.position - transform.position;
        float dist = toTarget.magnitude;
        if (dist > viewDistance)
        {
            return false; // 거리 초과
        }

        // 수평면 기준으로 각도 판단(수직각은 단순화)
        Vector3 dir = toTarget.normalized;
        float angle = Vector3.Angle(transform.forward, dir);
        if (angle > viewAngle)
        {
            return false;   // 시야각 밖
        }

        // 장애물 레이캐스트: 맞으면 가려진 것
        if (Physics.Raycast(transform.position + Vector3.up * 1.2f, dir, out RaycastHit hit, dist, obstacleMask) == true)
        {
            // 레이가 플레이어에 도달하기 전에 다른 것에 부딪혔다 = 가려짐
            if (hit.transform != t)
            {
                return false;
            }
        }

        return true;
    }

    void OnDrawGizmosSelected()
    {
        // Scene 뷰에서 시야 디버그(거리/각도)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        // 시야각 시각화
        Vector3 leftDir = Quaternion.Euler(0, -viewAngle, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, viewAngle, 0) * transform.forward;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + leftDir * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * viewDistance);
    }
}
