using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartToken : MonoBehaviour
{
    public float movementSpeed = 5f; // Adjust the movement speed as desired
    public float fleeDistance = 9f; // Distance at which the token flees

    private Vector3 targetPosition;
    private bool isFleeing = false;
   
    public GameObject startButton;
    public GameObject square;
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            startButton.SetActive(true);
            square.SetActive(true);
        }
    }

    void Start () 
    {
        SetRandomTargetPosition();
        startButton.SetActive(false);
        square.SetActive(false);
    }

    void Update () 
    {
        Check();
        if (!isFleeing)
        {
            MoveTowardsTarget();
        }
        else
        {
            FleeFromPlayer();
        }
    }
    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetRandomTargetPosition();
        }
    }

    void FleeFromPlayer()
    {
         // Generate a new random position to flee to
        Vector3 fleePosition = GetRandomFleePosition();

        // Move the token towards the flee position
        transform.position = Vector3.MoveTowards(transform.position, fleePosition, movementSpeed * Time.deltaTime);

        // Check if the token has reached the flee position
        if (Vector3.Distance(transform.position, fleePosition) < 0.1f)
        {
        // Reset the target position and stop fleeing
        SetRandomTargetPosition();
        isFleeing = false;
        }
    }
    void SetRandomTargetPosition()
    {
        float randomX = Random.Range(-7f, 7f); // Adjust the range as per your screen size
        float randomY = Random.Range(-5f, 5f); // Adjust the range as per your screen size

        targetPosition = new Vector3(randomX, randomY, 0f);
    }
    Vector3 GetRandomFleePosition()
    {
        // Generate a new random position away from the player
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 direction = (transform.position - playerPosition).normalized;
        Vector3 fleePosition = transform.position + direction * fleeDistance;

        // Clamp the flee position within the screen boundaries
        // float halfScreenWidth = Screen.width / 2;
        // float halfScreenHeight = Screen.height / 2;
        // fleePosition.x = Mathf.Clamp(fleePosition.x, -halfScreenWidth, halfScreenWidth);
        // fleePosition.y = Mathf.Clamp(fleePosition.y, -halfScreenHeight, halfScreenHeight);

        return fleePosition;
    }
    private void Check()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < fleeDistance)
        {
        isFleeing = true;
        }
        else 
        {
            isFleeing = false;
        }
    }
   
}
