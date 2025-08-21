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

    public ImpactVfxSpawner impactVfx;
    public CameraShake cameraShake;
    public Hitstop hitStop;
    public SfxPlayer sfxPlayer;

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
        //if (!active) return;
        if (other.CompareTag(ownerTag)) return; // 자기 자신/아군은 무시

        //IDamageable dmg = other.GetComponentInParent<IDamageable>();
        IDamageable dmg = other.GetComponent<IDamageable>();
        if (dmg != null)
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            dmg.TakeDamage(damage, hitPoint);

            if (impactVfx != null)
            {
                Vector3 normal = (transform.position - hitPoint.normalized);
                impactVfx.Spawn(hitPoint, normal);
            }

            DamageTextSpawner spawner = FindObjectOfType<DamageTextSpawner>();
            if(spawner != null)
            {
                spawner.Spawn(damage, hitPoint);
            }

            if(cameraShake != null)
            {
                cameraShake.Shake(0.15f, 0.5f);
            }

            if(hitStop != null)
            {
                hitStop.Stop(0.1f, 0.1f);
            }

            if(sfxPlayer != null)
            {
                sfxPlayer.Play(0);
            }
        }
    }
}
