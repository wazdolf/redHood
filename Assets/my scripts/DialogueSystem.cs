using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueSystem : MonoBehaviour
{
    public TMP_Text dialogueText;
    public float typingSpeed = 0.05f;
    
    private Conversation conversation;
    private Coroutine typingCoroutine;
    private bool canAdvanceConversation = false;
    public Animator dialogueAnim;
    public Image blackScreen;
    public float fadeDuration = 1f;
    private void DelayedLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator FadeToBlackCoroutine()
    {
        // Start with alpha value of 0 (fully transparent)
        Color targetColor = blackScreen.color;
        targetColor.a = 0f;
        blackScreen.color = targetColor;

        // Enable the black screen image
        blackScreen.enabled = true;

        float timer = 0f;

        while (timer < fadeDuration)
        {
            // Increase the timer based on the elapsed time
            timer += Time.deltaTime;

            // Calculate the new alpha value based on the fade duration
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);

            // Update the alpha value of the black screen image
            targetColor.a = alpha;
            blackScreen.color = targetColor;

            yield return null;
        }

        // Ensure the final alpha value is fully opaque
        targetColor.a = 1f;
        blackScreen.color = targetColor;
    }
    public void FadeToBlack()
    {
        StartCoroutine(FadeToBlackCoroutine());
    }
    
    private void Start()
    {
        // // Create a new conversation
        // conversation = new Conversation();
        
        // Start the conversation
        conversation = new Conversation();
        Invoke("StartConversation", 4f);
       
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        dialogueAnim.SetBool("IsOpen", true);
        
    }
    
    private void StartConversation()
    {
        // Clear the dialogue text
        dialogueText.text = string.Empty;
        
        // Generate the first message from the conversation
        Message initialMessage = conversation.GenerateMessage();
        
        // Start displaying the message with typewriter effect
        typingCoroutine = StartCoroutine(TypeText(initialMessage));
    }
    
    private IEnumerator TypeText(Message message)
    {
        dialogueText.text = string.Empty;
        
        // Display the name of the speaker
        dialogueText.text += message.speakerName + ": ";
        
        foreach (char c in message.text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        canAdvanceConversation = true;
    }
    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space) && canAdvanceConversation)
        {
            ContinueConversation();
        }
    }
    
    private void DisplayMessage(Message message)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        
        typingCoroutine = StartCoroutine(TypeText(message));
    }
    
    public void ContinueConversation()
    {
          // Prevent advancing the conversation while text is still typing
        if (!canAdvanceConversation)
            return;

        // Disable advancing the conversation until text typing completes
        canAdvanceConversation = false;
        
        // Generate the next message from the conversation
        Message nextMessage = conversation.GenerateMessage();
        // If the conversation has ended, do not display any message
        if (nextMessage == null)
        {
            dialogueAnim.SetBool("IsOpen", false);
            Invoke("FadeToBlack", 1f);
            Invoke("DelayedLoad",3f);
            return;
        }
           
        // Display the message with typewriter effect
        DisplayMessage(nextMessage);
    }
}

[System.Serializable]
public class Message
{
    public string speakerName;
    public string text;
}

public class Conversation
{
    private int messageIndex = 0;
    private Message[] messages = new Message[]
    {
        new Message { speakerName = "Player", text = "Wh-Where am I? What happened?" },
        new Message { speakerName = "Unknown Voice", text = "Welcome, traveler. You have arrived in a realm of forgotten memories." },
        new Message { speakerName = "Player", text = "Who... who are you? What is this place?" },
        new Message { speakerName = "Unknown Voice", text = "I am the Guardian of Lost Souls. You have lost your memories, but fear not, for I will guide you through this journey." },
        new Message { speakerName = "Player", text = "How can I regain my memories?" },
        new Message { speakerName = "Unknown Voice", text = "You must embark on a quest to reclaim what was lost. But first, you must learn to walk again. Feel the ground beneath you. Take your time." },
        new Message { speakerName = "Player", text = "[Tries to move but feels immobile] I... I can't move. I can't feel anything." },
        new Message { speakerName = "Unknown Voice", text = "Patience, dear traveler. The path to recovery begins with stillness. Listen to the whispers of your soul. Find solace in the silence." },
        new Message { speakerName = "Unknown Voice", text = "With time, movement shall be restored, and the fragments of your identity shall be revealed. Embrace the journey that lies ahead." }
    };
    
    public Message GenerateMessage()
    {
        if (messageIndex >= messages.Length)
        {
            // Conversation ended
            return null;
        }
        
        Message message = messages[messageIndex];
        messageIndex++;
        
        return message;
    }
    
    
}
