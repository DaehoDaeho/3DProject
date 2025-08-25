using UnityEngine;

/// <summary>
/// [역할] 문 열기(애니메이터 트리거 또는 오브젝트 비활성).
/// </summary>
public class DoorUnlock : MonoBehaviour
{
    public Animator doorAnimator;        // 있으면 사용
    public string openTrigger = "Open";
    public GameObject doorObject;        // 애니메이터 없을 때 비활성 처리

    public void Open()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(openTrigger);
        }
        else
        {
            if (doorObject != null) doorObject.SetActive(false);
        }
    }
}
