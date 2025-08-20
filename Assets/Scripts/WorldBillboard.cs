using UnityEngine;

/// <summary>
/// UI 오브젝트가 항상 메인 카메라를 바라보도록 맞춘다 (빌보드)
/// </summary>
public class WorldBillboard : MonoBehaviour
{
    Transform cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if(cam == null)
        {
            return;
        }

        Vector3 dir = (transform.position - cam.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
}
