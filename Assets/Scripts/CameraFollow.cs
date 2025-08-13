using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0.0f, 2.5f, 5.0f);
    public float smooth = 0.15f;

    private void LateUpdate()
    {
        if(target != null)
        {
            Vector3 desiredPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPos, smooth);

            transform.LookAt(target);
        }
    }
}
