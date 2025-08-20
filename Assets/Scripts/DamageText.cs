using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public float life = 0.8f;
    public float floatUp = 1.0f;

    Transform cam;
    float timer;
    Vector3 start;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main.transform;
        start = transform.position;
    }

    public void SetText(string s)
    {
        tmp.text = s;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // 위로 떠오르게.
        float t = Mathf.Clamp01(timer / life);
        transform.position = start + Vector3.up * (floatUp * t);

        // 빌보드 처리.
        if(cam != null)
        {
            Vector3 dir = (transform.position - cam.position).normalized;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }

        // 페이드 처리.
        if(tmp != null)
        {
            var c = tmp.color;
            c.a = 1.0f - t;
            tmp.color = c;
        }

        if(timer >= life)
        {
            Destroy(gameObject);
        }
    }
}
