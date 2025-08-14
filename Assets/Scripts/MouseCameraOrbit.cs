using UnityEngine;
using UnityEngine.Rendering;

public class MouseCameraOrbit : MonoBehaviour
{
    public Transform target;
    public Transform cam;
    public float distance = 4.0f;   // 대상과의 기본 거리.
    public float minDistance = 2.0f;    // 최소 줌 거리.
    public float maxDistance = 6.0f;    // 최소 줌 거리.

    public float yawSpeed = 180.0f;
    public float pitchSpeed = 120.0f;
    public float minPitch = -30.0f;
    public float maxPitch = 60.0f;
    public bool invertY = false;

    public KeyCode toggleCursorKey = KeyCode.LeftAlt;   // 커서 고정 토글.

    private float yaw;
    private float pitch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("target 미지정!!!!!");
        }

        if (cam == null)
        {
            Debug.LogWarning("카메라 미지정!!!!");
        }

        LockCursor(true);

        // yaw와 pitch 설정.
        Vector3 e = transform.eulerAngles;
        yaw = e.y;
        pitch = e.x > 180.0f ? e.x - 360.0f : e.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(toggleCursorKey) == true)
        {
            LockCursor(Cursor.lockState != CursorLockMode.Locked);
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            yaw += mouseX * yawSpeed * Time.deltaTime;

            float yInput = invertY ? mouseY : -mouseY;
            pitch += yInput * pitchSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0.0f);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance = Mathf.Clamp(distance - scroll * 3.0f, minDistance, maxDistance);
    }

    private void LateUpdate()
    {
        if (target == null || cam == null)
        {
            return;
        }

        transform.position = target.position;

        //Vector3 desired = transform.position - transform.forward * distance + Vector3.up * 0.0f;
        //cam.position = desired;
        //cam.LookAt(target.position + Vector3.up * 1.2f);
    }

    void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
