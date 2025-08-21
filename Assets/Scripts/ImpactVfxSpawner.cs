using UnityEngine;

public class ImpactVfxSpawner : MonoBehaviour
{
    public GameObject vfxPrefab;
    public float life = 1.0f;

    public void Spawn(Vector3 position, Vector3 normal)
    {
        if (vfxPrefab == null) return;

        Quaternion rot = Quaternion.LookRotation(normal);
        GameObject go = GameObject.Instantiate(vfxPrefab, position, rot);
        GameObject.Destroy(go, life);
    }
}
