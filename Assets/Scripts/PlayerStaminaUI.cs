using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaUI : MonoBehaviour
{
    public PlayerStamina stamina;
    public Image staminaFill;
    public TextMeshProUGUI staminaText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(stamina == null || staminaFill == null)
        {
            return;
        }

        float ratio = (float)stamina.current / stamina.max;
        staminaFill.fillAmount = Mathf.Clamp01(ratio);

        if(staminaText != null)
        {
            staminaText.text = (int)stamina.current + " / " + (int)stamina.max;
        }
    }
}
