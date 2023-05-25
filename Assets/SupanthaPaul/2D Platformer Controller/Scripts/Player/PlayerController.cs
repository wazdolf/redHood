﻿using UnityEngine;

namespace SupanthaPaul
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField] private float speed;
		[Header("Jumping")]
		[SerializeField] private float jumpForce;
		[SerializeField] private float fallMultiplier;
		[SerializeField] private Transform groundCheck;
		[SerializeField] private float groundCheckRadius;
		[SerializeField] private LayerMask whatIsGround;
		[SerializeField] private int extraJumpCount = 1;
		[SerializeField] private GameObject jumpEffect;
		[Header("Dashing")]
		[SerializeField] private float dashSpeed = 30f;
		[Tooltip("Amount of time (in seconds) the player will be in the dashing speed")]
		[SerializeField] private float startDashTime = 0.1f;
		[Tooltip("Time (in seconds) between dashes")]
		[SerializeField] private float dashCooldown = 0.2f;
		[SerializeField] private GameObject dashEffect;

		// Access needed for handling animation in Player script and other uses
		[HideInInspector] public bool isGrounded;
		[HideInInspector] public float moveInput;
		[HideInInspector] public bool canMove = true;
		[HideInInspector] public bool isDashing = false;
		[HideInInspector] public bool isAttacking = false;
		// controls whether this instance is currently playable or not
		[HideInInspector] public bool isCurrentlyPlayable = false;

		private Rigidbody2D m_rb;
		private ParticleSystem m_dustParticle;
		private bool m_facingRight = true;
		private readonly float m_groundedRememberTime = 0.25f;
		private float m_groundedRemember = 0f;
		private int m_extraJumps;
		private float m_extraJumpForce;
		private float m_dashTime;
		private bool m_hasDashedInAir = false;
		private float m_dashCooldown;
		private bool m_canMoveLeft = false;
        private bool m_canJump = false;
        private bool m_canDash = false;
		private bool m_canAttack = false;

		void Start()
		{
			// create pools for particles
			PoolManager.instance.CreatePool(dashEffect, 2);
			PoolManager.instance.CreatePool(jumpEffect, 2);

			// if it's the player, make this instance currently playable
			if (transform.CompareTag("Player"))
				isCurrentlyPlayable = true;

			m_extraJumps = extraJumpCount;
			m_dashTime = startDashTime;
			m_dashCooldown = dashCooldown;
			m_extraJumpForce = jumpForce * 0.7f;

			m_rb = GetComponent<Rigidbody2D>();
			m_dustParticle = GetComponentInChildren<ParticleSystem>();
		}

		private void FixedUpdate()
		{
			// check if grounded
			isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
			var position = transform.position;
			// if this instance is currently playable
			if (isCurrentlyPlayable)
			{
				// horizontal movement
					if(canMove && m_canMoveLeft)
						m_rb.velocity = new Vector2(moveInput * speed, m_rb.velocity.y);
					else if(!canMove && !m_canMoveLeft)
						m_rb.velocity = new Vector2(0f, m_rb.velocity.y);
				
				// better jump physics
				if (m_rb.velocity.y < 0f)
				{
					m_rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
				}

				// Flipping
				if (!m_facingRight && moveInput > 0f && m_canMoveLeft)
					Flip();
				else if (m_facingRight && moveInput < 0f && m_canMoveLeft)
					Flip();

				// Dashing logic
				if (isDashing && m_canDash)
				{
					if (m_dashTime <= 0f)
					{
						isDashing = false;
						m_dashCooldown = dashCooldown;
						m_dashTime = startDashTime;
						m_rb.velocity = Vector2.zero;
					}
					else
					{
						m_dashTime -= Time.deltaTime;
						if(m_facingRight)
							m_rb.velocity = Vector2.right * dashSpeed;
						else
							m_rb.velocity = Vector2.left * dashSpeed;
					}
				}

				// enable/disable dust particles
				float playerVelocityMag = m_rb.velocity.sqrMagnitude;
				if(m_dustParticle.isPlaying && playerVelocityMag == 0f)
				{
					m_dustParticle.Stop();
				}
				else if(!m_dustParticle.isPlaying && playerVelocityMag > 0f)
				{
					m_dustParticle.Play();
				}

			}
		}

		private void Update()
		{
			// horizontal input
			moveInput = InputSystem.HorizontalRaw();

			if (isGrounded)
			{
				m_extraJumps = extraJumpCount;
			}

			// grounded remember offset (for more responsive jump)
			m_groundedRemember -= Time.deltaTime;
			if (isGrounded)
				m_groundedRemember = m_groundedRememberTime;

			if (!isCurrentlyPlayable) return;
			// if not currently dashing and hasn't already dashed in air once
			if (!isDashing && !m_hasDashedInAir && m_dashCooldown <= 0f && m_canDash)
			{
				// dash input (left shift)
				if (InputSystem.Dash())
				{
					isDashing = true;
					// dash effect
					PoolManager.instance.ReuseObject(dashEffect, transform.position, Quaternion.identity);
					// if player in air while dashing
					if(!isGrounded)
					{
						m_hasDashedInAir = true;
					}
					// dash logic is in FixedUpdate
				}
			}
			
				if(InputSystem.Attack() && m_canAttack)
				{
					isAttacking = true;
				}
				else
				{
					isAttacking = false;
				}
				

			m_dashCooldown -= Time.deltaTime;
			
			// if has dashed in air once but now grounded
			if (m_hasDashedInAir && isGrounded)
				m_hasDashedInAir = false;
			
			// Jumping
			if(InputSystem.Jump() && m_extraJumps > 0 && !isGrounded && m_canJump)	// extra jumping
			{
				m_rb.velocity = new Vector2(m_rb.velocity.x, m_extraJumpForce); ;
				m_extraJumps--;
				// jumpEffect
				PoolManager.instance.ReuseObject(jumpEffect, groundCheck.position, Quaternion.identity);
			}
			else if(InputSystem.Jump() && (isGrounded || m_groundedRemember > 0f) && m_canJump)	// normal single jumping
			{
				m_rb.velocity = new Vector2(m_rb.velocity.x, jumpForce);
				// jumpEffect
				PoolManager.instance.ReuseObject(jumpEffect, groundCheck.position, Quaternion.identity);
			}
			

		}

		void Flip()
		{
			m_facingRight = !m_facingRight;
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
		}
		private void OnCollisionEnter2D(Collision2D other)
		{
			if(other.gameObject.CompareTag("LeftButton"))
			{
				m_canMoveLeft = true;
			}
			if(other.gameObject.CompareTag("JumpButton"))
			{
				m_canJump = true;
			}
			if(other.gameObject.CompareTag("DashButton"))
			{
				m_canDash = true;
			}
			if(other.gameObject.CompareTag("AttackButton"))
			{
				m_canAttack = true;
			}
		}
		// private void OnTriggerEnter2D(Collider2D other) 
		// {
		// 	if(other.gameObject.CompareTag("LeftButton"))
		// 	{
		// 		m_canMoveLeft = true;
		// 	}
		// 	if(other.gameObject.CompareTag("JumpButton"))
		// 	{
		// 		m_canJump = true;
		// 	}
		// 	if(other.gameObject.CompareTag("DashButton"))
		// 	{
		// 		m_canDash = true;
		// 	}
		// 	if(other.gameObject.CompareTag("AttackButton"))
		// 	{
		// 		m_canAttack = true;
		// 	}
		// }

	}
}