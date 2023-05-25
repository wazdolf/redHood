using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public float delay = 1f; // Delay before the enemy starts mimicking player movement

    private GameObject player;
    public Animator playerAnimator;
    public Animator enemyAnimator;
    private Vector3 targetPosition;
    private bool isDelayOver = false;
    // private bool facingRight;
    // private float isRunning;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // playerAnimator = player.GetComponent<Animator>();
        // enemyAnimator = GetComponent<Animator>();

        // Start the delay coroutine
        StartCoroutine(StartDelay());
    }

    private void Update()
    {
        if (isDelayOver)
        {
            // Move towards the player's position
            targetPosition = player.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 4);

            // Mimic player's animations
            float isRunning = playerAnimator.GetFloat("Move");
            enemyAnimator.SetFloat("Move", isRunning);

            bool isJumping = playerAnimator.GetBool("IsJumping");
            enemyAnimator.SetBool("IsJumping", isJumping);
        }
        Vector3 scale = player.transform.localScale;
        transform.localScale = scale;
        
    }

    private System.Collections.IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(delay);

        isDelayOver = true;
    }
    // public void Flip()
    // {
    //     facingRight = !facingRight;
    //     Vector3 scale = transform.localScale;
    //     scale.x *= -1;
    //     transform.localScale = scale;
    // }
}
