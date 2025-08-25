using UnityEngine;

/// <summary>
/// [역할] 콤보 입력/버퍼/창 제어. 어려운 문법 없이 정수/불린만으로 관리.
/// Animator 트리거: Attack1 → Attack2 → Attack3.
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerComboController : MonoBehaviour
{
    [Header("Animator 참조/트리거명")]
    public Animator anim;
    public string tAttack1 = "Attack1";
    public string tAttack2 = "Attack2";
    public string tAttack3 = "Attack3";

    [Header("입력 키")]
    public KeyCode attackKey = KeyCode.J; // 또는 마우스 왼쪽

    // 상태 변수(간단!)
    int comboIndex = 0;   // 0=대기, 1/2/3 = 몇 타 재생 중인지
    bool windowOpen = false;   // 콤보 창 열림 여부
    bool queuedNext = false;   // 창 열리기 전 입력 저장
    bool attackPressed = false; // 이번 프레임에 입력이 있었는지

    void Awake()
    {
        if (anim == null) anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 1) 입력 감지(버퍼) - 버튼 다운만 기록
        if (Input.GetKeyDown(attackKey) || Input.GetMouseButtonDown(0))
        {
            attackPressed = true;
        }

        // 2) 대기 상태면 1타 시작 시도
        if (comboIndex == 0 && attackPressed)
        {
            StartAttack1();
            attackPressed = false; // 소비
            return;
        }

        // 3) 재생 중이면 입력을 큐에 쌓아두기
        if (attackPressed)
        {
            queuedNext = true;   // "다음 창 열리면 바로 써줘!"
            attackPressed = false;
        }

        // 4) 창이 열려 있고 큐가 있으면 다음 타 발사
        if (windowOpen && queuedNext)
        {
            TriggerNext();
            queuedNext = false;
        }
    }

    // ---- 공격 트리거 함수들 ----
    void StartAttack1()
    {
        anim.SetTrigger(tAttack1);
        comboIndex = 1;
        windowOpen = false;
        queuedNext = false;
    }

    void TriggerNext()
    {
        if (comboIndex == 1)
        {
            anim.SetTrigger(tAttack2);
            comboIndex = 2;
            windowOpen = false;
        }
        else if (comboIndex == 2)
        {
            anim.SetTrigger(tAttack3);
            comboIndex = 3;
            windowOpen = false;
        }
        // comboIndex==3이면 더 갈 곳 없음
    }

    // ---- 애니메이션 이벤트에서 호출할 메서드들 ----
    public void OnComboWindowOpen()
    {
        windowOpen = true;

        // 창이 열리는 순간 이미 큐되어 있으면 즉시 발사
        if (queuedNext)
        {
            TriggerNext();
            queuedNext = false;
            windowOpen = false; // 다음 상태의 창까지 잠시 닫힘
        }
    }

    public void OnComboWindowClose()
    {
        windowOpen = false;
    }

    public void OnComboEnd()
    {
        // 모든 공격 종료(Idle/Move로 복귀)
        comboIndex = 0;
        windowOpen = false;
        queuedNext = false;
        attackPressed = false;
    }
}
