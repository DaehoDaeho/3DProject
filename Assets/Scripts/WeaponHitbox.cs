using UnityEngine;

/// <summary>
/// 무기 히트박스: 활성 중일 때만 Trigger 충돌을 받아 대상에 피해 전달.
/// </summary>
[RequireComponent(typeof(Collider))]
public class WeaponHitbox : MonoBehaviour
{
    public string ownerTag = "Player"; // 자기 자신/아군 타격 방지용
    public int damage = 12;

    Collider col;
    bool active; // 현재 활성 상태

    void Awake()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;
        col.enabled = false; // 기본 비활성
        active = false;
    }

    public void SetActive(bool on)
    {
        active = on;
        col.enabled = on; // 간단히 콜라이더 자체를 On/Off
    }

    void OnTriggerEnter(Collider other)
    {
        if (!active) return;
        if (other.CompareTag(ownerTag)) return; // 자기 자신/아군은 무시

        IDamageable dmg = other.GetComponentInParent<IDamageable>();
        if (dmg != null)
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            dmg.TakeDamage(damage, hitPoint);
        }
    }
}

/// <summary>
/// 애니메이션 이벤트 리시버: Attack 클립 타임라인에서 OnAttackStart/End를 호출.
/// </summary>
public class AttackEventRelay : MonoBehaviour
{
    public WeaponHitbox hitbox;

    // 공격 시작 프레임 이벤트
    public void OnAttackStart()
    {
        if (hitbox) hitbox.SetActive(true);
    }

    // 공격 종료 프레임 이벤트
    public void OnAttackEnd()
    {
        if (hitbox) hitbox.SetActive(false);
    }
}
