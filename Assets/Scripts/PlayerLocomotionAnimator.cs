using UnityEngine;

/// <summary>
/// CharacterController 기반 이동/점프/중력 + Animator 파라미터 연동 + Attack 트리거 발화.
/// '코드 이동' 방식(In-Place 애니메이션 가정).
/// </summary>
[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class PlayerLocomotionAnimator : MonoBehaviour
{
    [Header("이동/점프")]
    public float walkSpeed = 4f;       // 걷기 속도
    public float runSpeed = 7f;       // 달리기 속도(Shift로 토글 가능)
    public float jumpHeight = 1.6f;    // 점프 높이
    public float gravity = -9.81f;     // 중력 가속도(음수)

    [Header("카메라 기준 이동")]
    public Transform cam;              // 메인 카메라 Transform

    [Header("접지(Raycast)")]
    public Transform groundCheck;      // 발밑 기준점(빈 오브젝트)
    public float groundRayLen = 0.25f; // 레이 길이
    public LayerMask groundLayer;      // 접지로 인정할 레이어

    [Header("회전 보간")]
    public float turnSmooth = 10f;     // 회전 속도(보간 계수)

    [Header("Animator 파라미터명")]
    public string pSpeed = "Speed";        // 0~1 정규화 속도
    public string pIsGrounded = "IsGrounded";
    public string pYVel = "YVel";
    public string tJump = "Jump";          // 트리거
    public string tAttack = "Attack";      // 트리거
    public float speedDampTime = 0.1f;     // Speed 감쇠 시간

    // 내부 상태
    CharacterController cc;
    Animator anim;
    Vector3 velocity;       // 현재 속도(특히 y 사용)
    bool isGrounded;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        if (!cam && Camera.main) cam = Camera.main.transform;
        if (!groundCheck) Debug.LogWarning("groundCheck 미지정");
    }

    void Update()
    {
        // 1) 접지 판정(우선 Raycast, 없으면 cc.isGrounded 보조)
        isGrounded = false;
        if (groundCheck)
        {
            if (Physics.Raycast(groundCheck.position, Vector3.down, groundRayLen, groundLayer))
                isGrounded = true;
        }
        else
        {
            isGrounded = cc.isGrounded;
        }
        if (isGrounded && velocity.y < 0f) velocity.y = -2f; // 바닥에 붙여 안정화

        // 2) 입력
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool run = Input.GetKey(KeyCode.LeftShift);

        // 3) 카메라 기준 수평 이동 벡터
        Vector3 f = cam ? cam.forward : Vector3.forward;
        Vector3 r = cam ? cam.right : Vector3.right;
        f.y = 0f; r.y = 0f; f.Normalize(); r.Normalize();
        Vector3 moveDir = (r * h + f * v);
        if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

        // 4) 속도 선택/이동
        float spd = run ? runSpeed : walkSpeed;
        cc.Move(moveDir * spd * Time.deltaTime);

        // 5) 점프
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // sqrt(2 g h)
            anim.SetTrigger(tJump);
        }

        // 6) 중력
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        // 7) 회전(이동 중일 때만)
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion t = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, t, Time.deltaTime * turnSmooth);
        }

        // 8) 애니메이터 파라미터
        float horizontalSpeed = new Vector3(cc.velocity.x, 0f, cc.velocity.z).magnitude;
        float max = Mathf.Max(walkSpeed, runSpeed);
        float speed01 = Mathf.InverseLerp(0f, max, horizontalSpeed);
        anim.SetFloat(pSpeed, speed01, speedDampTime, Time.deltaTime);
        anim.SetBool(pIsGrounded, isGrounded);
        anim.SetFloat(pYVel, velocity.y);

        // 9) 공격 입력
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J))
            anim.SetTrigger(tAttack);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundRayLen);
        }
    }
}
