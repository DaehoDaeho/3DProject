using UnityEngine;
using System.Collections;

/// <summary>
/// [역할] 회피(구르기) 입력/조건 체크 → 방향·속도·시간으로 전진 → i-프레임 적용.
/// CharacterController 기반 "코드 이동" 방식.
/// </summary>
[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class PlayerDodgeRoll : MonoBehaviour
{
    [Header("입력")]
    public KeyCode dodgeKey = KeyCode.LeftControl;

    [Header("이동/속도")]
    public float rollSpeed = 10f;        // 구르는 동안의 수평 속도(전진 힘)
    public float rollDuration = 0.45f;   // 구르는 시간(초)

    [Header("i-프레임(무적) 구간")]
    public float iframeStart = 0.06f;    // 회피 시작 후 ~초
    public float iframeEnd = 0.36f;    // 회피 시작 후 ~초 (이전보다 커야 함)

    [Header("스태미나 소비")]
    public float rollCost = 25f;
    public PlayerStamina stamina;

    [Header("참조")]
    public Transform cam;                // 카메라 기준 이동
    public Transform groundCheck;        // 접지 확인(있으면 사용)
    public float groundRayLen = 0.25f;
    public LayerMask groundLayer;

    [Header("애니메이터 파라미터")]
    public string tDodge = "Dodge";

    CharacterController cc;
    Animator anim;
    Health health;

    bool isDodging;
    Vector3 lastMoveDir; // 최근 이동 방향(정지 시 앞방향 대용)

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();

        if (cam == null && Camera.main != null) cam = Camera.main.transform;
        if (stamina == null) stamina = GetComponent<PlayerStamina>();
    }

    void Update()
    {
        // 최근 이동 방향 업데이트(로코모션 스크립트가 있다면 같은 논리로)
        Vector3 dir = GetInputMoveDir();
        if (dir.sqrMagnitude > 0.0001f) lastMoveDir = dir;

        // 회피 입력
        if (Input.GetKeyDown(dodgeKey))
        {
            TryDodge();
        }
    }

    void TryDodge()
    {
        if (isDodging) return;
        if (!IsGrounded()) return;

        // 스태미나 체크
        if (stamina != null && !stamina.Has(rollCost))
        {
            // 부족: 그냥 무시(효과음/플래시 같은 피드백은 추후)
            return;
        }

        // 회피 시작
        StartCoroutine(CoDodge());
    }

    IEnumerator CoDodge()
    {
        isDodging = true;

        // 스태미나 차감
        if (stamina != null) stamina.Use(rollCost);

        // 애니메이션 트리거
        anim.SetTrigger(tDodge);

        // 이동 방향 결정: 입력이 있으면 그쪽, 없으면 정면
        Vector3 moveDir = lastMoveDir.sqrMagnitude > 0.0001f ? lastMoveDir : transform.forward;
        moveDir.y = 0f;
        if (moveDir.sqrMagnitude > 0f) moveDir.Normalize();

        // i-프레임 타이머
        float t = 0f;
        bool invOn = false;

        // 회피 유지
        while (t < rollDuration)
        {
            float dt = Time.deltaTime;
            t += dt;

            // i-프레임 On/Off
            if (!invOn && t >= iframeStart)
            {
                if (health != null) health.isInvincible = true;
                invOn = true;
            }
            if (invOn && t >= iframeEnd)
            {
                if (health != null) health.isInvincible = false;
                invOn = false;
            }

            // 전진(수평), 중력은 별도 로코모션에서 처리되었을 수 있음
            Vector3 move = moveDir * rollSpeed * dt;

            // 살짝 낮추어 경사 대응(선택)
            move += Vector3.down * 0.05f;

            cc.Move(move);

            yield return null;
        }

        // 안전: i-프레임 꺼주기
        if (health != null) health.isInvincible = false;

        isDodging = false;
    }

    Vector3 GetInputMoveDir()
    {
        // 로코모션과 동일한 방식: 카메라 기준 WASD
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 f = cam ? cam.forward : Vector3.forward;
        Vector3 r = cam ? cam.right : Vector3.right;
        f.y = 0f; r.y = 0f; f.Normalize(); r.Normalize();

        Vector3 m = r * h + f * v;
        if (m.sqrMagnitude > 1f) m.Normalize();
        return m;
    }

    bool IsGrounded()
    {
        if (groundCheck != null)
        {
            if (Physics.Raycast(groundCheck.position, Vector3.down, groundRayLen, groundLayer))
                return true;
        }
        // 보조: CharacterController 자체 판정
        return cc.isGrounded;
    }
}
