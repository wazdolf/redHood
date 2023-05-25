using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartToken : MonoBehaviour
{
    // set the speed of the floating movement
    public float speed;
    
    // set the direction of the floating movement
    Vector3 direction; 
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
        
        // set initial direction of the floating movement
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        startButton.SetActive(false);
        square.SetActive(false);
    }

    void Update () 
    {
        speed = Random.Range(0,0.07f);
      // move the token in the set direction
        transform.position += direction * speed; 
        
        // if the token has reached the edge of the screen, change the direction of the floating movement
        if (transform.position.x < -8.5f || transform.position.x > 8.5f || transform.position.y < -4.6 || transform.position.y > 4.6)
        {
            direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        }
    }
   
}
