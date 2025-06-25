using System;
using UnityEngine;

public class Player : Singleton<Player>
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;
    private Vector3 velocity;
    private float gravityValue = -9.81f;
    public float moveSpeed;
    public float jumpSpeed;
    public float rotationSpeed;
    const float k_Half = 0.5f;
    float m_ForwardAmount;
    [SerializeField] float m_RunCycleLegOffset = 0.2f;
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector3 groundCheckSize;
    [SerializeField] private LayerMask isGround;
    void FixedUpdate()
    {
        Move();
        GroundCheck();
        if (isGrounded)
        {
            velocity.y = 0;
            if (Input.GetKeyDown(KeyCode.Space)) Jump();
        }
        if (!isGrounded)
        {
            velocity.y += gravityValue * Time.deltaTime;
        }
        if (transform.position.y < -2)
        {
            transform.position = new Vector3(30, 2, 6);
        }
        SetAnimatorParameter();
    }
    void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 movementInput = Quaternion.Euler(0, ThirdPersonCamera.Instance.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);
        Vector3 movementDirection = movementInput.normalized;
        m_ForwardAmount = movementDirection.magnitude;
        controller.Move(movementDirection * moveSpeed * Time.deltaTime);

        if (movementDirection != Vector3.zero)
        {
            Quaternion nextRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, nextRotation, rotationSpeed * Time.deltaTime);
        }
        controller.Move(velocity * Time.deltaTime);
    }
    void Jump()
    {
        velocity.y += Mathf.Sqrt(jumpSpeed * gravityValue * -1f);
    }
    private void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        Collider[] colliders = Physics.OverlapBox(groundCheck.position, groundCheckSize, Quaternion.Euler(0, 0, 0), isGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                return;
            }
        }
    }
    private void SetAnimatorParameter()
    {
        animator.SetFloat("Forward", m_ForwardAmount);
        animator.SetBool("OnGround", isGrounded);
        if (!isGrounded)
        {
            animator.SetFloat("Jump", velocity.y);
        }

        float runCycle = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
        float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
        if (isGrounded)
        {
            animator.SetFloat("JumpLeg", jumpLeg);
        }
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(groundCheck.position, groundCheckSize);
    }
}
