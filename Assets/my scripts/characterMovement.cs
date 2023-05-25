using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Animation")]
    public Animator animator;
    public string idleAnimation = "idle_z";
    public string runAnimation = "run_2";
    public string jumpAnimation = "jump_z";
    public string bowAttackAnimation = "bow attack";
    public string swordAttackAnimation = "light attack";
    public string hurtAnimation = "hurt_z";
    public string deathAnimation = "death";

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float moveDirection = Input.GetAxis("Horizontal");

        // Check if the character is on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Handle movement
        HandleMovement(moveDirection);

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // Handle attacking
        if (Input.GetButtonDown("Fire1"))
        {
            // Bow attack
            if (Input.GetKey(KeyCode.LeftShift))
            {
                BowAttack();
            }
            // Sword attack
            else
            {
                SwordAttack();
            }
        }

        // Update animations
        UpdateAnimations(moveDirection);
    }

    private void HandleMovement(float moveDirection)
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        // Flip character sprite based on movement direction
        if (moveDirection > 0 && !isFacingRight)
        {
            FlipCharacter();
        }
        else if (moveDirection < 0 && isFacingRight)
        {
            FlipCharacter();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void BowAttack()
    {
        // Play bow attack animation
        animator.Play(bowAttackAnimation);
    }

    private void SwordAttack()
    {
        // Play sword attack animation
        animator.Play(swordAttackAnimation);
    }

    private void UpdateAnimations(float moveDirection)
    {
        // Update idle/run animations
        if (moveDirection == 0)
        {
            animator.Play(idleAnimation);
        }
        else
        {
            animator.Play(runAnimation);
        }

        // Update jump animation
        if (!isGrounded)
        {
            Debug.Log("not grounded");
            animator.Play(jumpAnimation);
        }
    }

    private void FlipCharacter()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(Vector3.up, 180f);
    }

    public void TakeDamage()
    {
        // Play hurt animation
        animator.Play(hurtAnimation);
    }

    public void Die()
    {
        // Play death animation
        animator.Play(deathAnimation);
    }
}
