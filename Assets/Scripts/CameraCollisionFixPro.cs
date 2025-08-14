using UnityEngine;

public class CameraCollisionFixPro : MonoBehaviour
{
    public Transform target;
    public Transform cameraRig;

    // 거리 설정을 위한 변수.
    public float defaultDistance = 4.0f;
    public float minDistance = 0.6f;
    public float maxDistance = 6.0f;

    // 충돌 검사를 위한 변수.
    public float sphereRadius = 0.25f;
    public LayerMask obstacleMask;

    // 시선 높이.
    public Vector3 targetHeightOffset = new Vector3(0.0f, 1.2f, 0.0f);

    // 거리 변화의 부드러움.
    public float distSmoothTime = 0.05f;

    // 현재 거리.
    float currentDistance;
    // 거리 변화량.
    float distVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 거리를 최소 거리, 최대 거리 사이의 값으로 제한을 두겠다.
        currentDistance = Mathf.Clamp(defaultDistance, minDistance, maxDistance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if(target == null || cameraRig == null)
        {
            return;
        }

        // 원하는 위치 계산.
        Vector3 rigBack = -cameraRig.forward;   // cameraRig의 반대 방향.
        Vector3 rigPos = cameraRig.position;
        Vector3 desired = rigPos + rigBack * defaultDistance;   // 원하는 위치 계산.

        // 타겟 시선 높이에서 원하는 위치까지 경로.
        Vector3 rayStart = target.position + targetHeightOffset;
        Vector3 toDesired = desired - rayStart;
        float maxDist = toDesired.magnitude;
        Vector3 dir = maxDist > 0.0001f ? toDesired / maxDist : rigBack;

        // SphereCast로 충돌 검사.
        float targetDistance = defaultDistance;
        if(Physics.SphereCast(rayStart, sphereRadius, dir, out RaycastHit hit, maxDist, obstacleMask) == true)
        {
            // 충돌 지점 직전까지 카메라를 당겨주는 처리.
            targetDistance = Mathf.Clamp(hit.distance - 0.05f, minDistance, defaultDistance);
        }
        else
        {
            // 기본 거리로 복구.
            targetDistance = Mathf.Clamp(defaultDistance, minDistance, maxDistance);
        }

        // 현재 거리를 부드럽게.
        currentDistance = Mathf.SmoothDamp(currentDistance, targetDistance, ref distVelocity, distSmoothTime);

        // 최종 카메라 위치.
        Vector3 finalPos = rigPos + rigBack * currentDistance;
        transform.position = finalPos;

        // 카메라가 타겟을 바라봄.
        transform.LookAt(target.position + targetHeightOffset);
    }
}
