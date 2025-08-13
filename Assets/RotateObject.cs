using UnityEngine;
using UnityEngine.AI;

public class RotateObject : MonoBehaviour
{
    public NavMeshAgent nma;
    public Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nma.SetDestination(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(new Vector3(0.0f, 5.0f, 0.0f));
    }
}
