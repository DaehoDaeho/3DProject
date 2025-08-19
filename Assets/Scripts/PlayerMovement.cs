using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    private Transform cam;
    public Transform groundCheck;
    public float groundRayLength = 0.2f;
    public LayerMask groundLayer;

    private CharacterController controller;
    private Vector3 velocity;   // 현재 속도.
    private bool isGrounded;

    public Animator animator;
    public BoxCollider hitBox;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = false;

        if(groundCheck != null)
        {
            if(Physics.Raycast(groundCheck.position, Vector3.down, groundRayLength, groundLayer) == true)
            {
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = controller.isGrounded;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;
        forward.y = 0.0f;
        right.y = 0.0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = (right * h + forward * v);
        moveDir.Normalize();

        controller.Move(moveDir * moveSpeed * Time.deltaTime);

        if(isGrounded == true && Input.GetKeyDown(KeyCode.Space) == true)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            animator.SetTrigger("Jump");
        }
        else if(isGrounded == true && Input.GetKeyDown(KeyCode.F) == true)
        {
            animator.SetTrigger("Attack");
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if(moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10.0f);
        }

        bool move = false;
        if(h != 0.0f || v != 0.0f)
        {
            move = true;            
        }

        animator.SetBool("Move", move);
    }

    void OnAttackStart()
    {
        if (hitBox != null)
        {
            hitBox.enabled = true;
        }
    }

    void OnAttackEnd()
    {
        if (hitBox != null)
        {
            hitBox.enabled = false;
        }
    }
}
