using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class collect : MonoBehaviour
{
    private int count;
    public int maxCount;
    public Animator anim;
    public TMP_Text countText;
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(count >= maxCount)
        {
            Invoke("LoadNext", 2f);
        }
        countText.text = count.ToString() + "-" + maxCount.ToString();
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("token"))
        {
            count++;
            Destroy(other.gameObject);
        }
        else if(other.gameObject.CompareTag("spike"))
        {
           Die();
        }
        //  if(other.gameObject.CompareTag("enemy"))
        // {
        //     Debug.Log("enemy collided");
        //    Die();
        // }
    }
    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.CompareTag("enemy"))
        {
            Debug.Log("enemy triggered");
           Die();
        }
    }
    private void LoadNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    public void Die()
    {
        anim.SetTrigger("death");
        Invoke("ReloadScene", 1f);
    }
    
}
