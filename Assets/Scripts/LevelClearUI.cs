using UnityEngine;
using TMPro;

/// <summary>
/// [역할] "STAGE CLEAR" 같은 텍스트를 보여준다.
/// </summary>
public class LevelClearUI : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text text;

    void Awake()
    {
        if (panel != null) panel.SetActive(false);
    }

    public void Show()
    {
        if (panel != null) panel.SetActive(true);
        if (text != null) text.text = "Welcome To Hell";
    }
}
