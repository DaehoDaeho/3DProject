using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이어의 Health 스크립트를 참조해서 HP 게이지와 HP 숫자 텍스트를 갱신한다.
/// </summary>
public class PlayerHealthUI : MonoBehaviour
{
    public Health playerHealth; // 플레이어의 붙어 있는 Health 컴포넌트.
    public Image hpFill;    // Image
    public TextMeshProUGUI hpText;  // Text

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealth == null || hpFill == null)
        {
            return;
        }

        float ratio = (float)playerHealth.hp / playerHealth.maxHP;
        hpFill.fillAmount = Mathf.Clamp01(ratio);

        if(hpText != null)
        {
            hpText.text = playerHealth.hp.ToString() + " / " + playerHealth.maxHP.ToString();
        }
    }
}
