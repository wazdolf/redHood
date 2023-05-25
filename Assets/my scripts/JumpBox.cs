using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JumpBox : MonoBehaviour
{
    public string unlockMessage;

    private bool isPlayerColliding;
    public TextMeshProUGUI textDisplay;

    private void Start()
    {
        // Hide the text initially
        textDisplay.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerColliding = true;
            DisplayUnlockMessage();
            Destroy(gameObject, 3f);
        }
    }

    private void OnCollsionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerColliding = false;
            HideUnlockMessage();
        }
    }

    private void DisplayUnlockMessage()
    {
        if (isPlayerColliding)
        {
            // Display the unlock message
            textDisplay.text = unlockMessage;
            textDisplay.enabled = true;
            // Hide the message after 3 seconds
            Invoke(nameof(HideUnlockMessage), 3f);
        }
    }

    private void HideUnlockMessage()
    {
        textDisplay.enabled = false;
    }
}
