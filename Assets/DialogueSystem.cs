using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialogueBox;
    
    [Header("Dialogue Settings")]
    [SerializeField] private List<string> dialogueLines;
    [SerializeField] private float textSpeed = 0.05f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip typingSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float fadeOutDuration = 0.3f;

    [Header("Script to Disable")]
    [SerializeField] private Tilemovement scriptToDisable; // 🔹 Script to disable when dialogue is active

    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private Coroutine fadeOutCoroutine;
    

    
    private void Start()
    {
        dialogueBox.SetActive(false);

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }
    }

    private void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space)) 
        {
            NextLine();
        }
        if (scriptToDisable != null)
        {
            scriptToDisable.enabled = !isDialogueActive;
        }
    }

    public void BeginDialogue()
    {
        if (dialogueLines.Count == 0) return;

        dialogueBox.SetActive(true);
        currentLineIndex = 0;
        isDialogueActive = true;

        

        StartTypingCurrentLine();
        BossMusicManager.Instance.OnDialogueStart();
    }

    private void StartTypingCurrentLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
    }

    private IEnumerator TypeText(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        if (typingSound != null && audioSource != null)
        {
            StartCoroutine(PlayTypingSoundLoop());
        }

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
        StartCoroutine(FadeOutAudio());
    }

    private IEnumerator PlayTypingSoundLoop()
    {
        while (isTyping)
        {
            if (typingSound != null && audioSource != null)
            {
                audioSource.clip = typingSound;
                audioSource.volume = 1f;
                audioSource.Play();
                yield return new WaitForSeconds(typingSound.length);
            }
        }
    }

    private IEnumerator FadeOutAudio()
    {
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }

        fadeOutCoroutine = StartCoroutine(FadeOutCoroutine());
        yield return fadeOutCoroutine;
    }

    private IEnumerator FadeOutCoroutine()
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeOutDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeOutDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }

    public void NextLine()
    {
        if (isTyping) 
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = dialogueLines[currentLineIndex];
            isTyping = false;
            StartCoroutine(FadeOutAudio());
            return;
        }

        currentLineIndex++;

        if (currentLineIndex < dialogueLines.Count)
        {
            StartTypingCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialogueBox.SetActive(false);
        isDialogueActive = false;

        GameManager2.ResumeGame(); // Unfreeze player input

        StartCoroutine(FadeOutAudio());
        BossMusicManager.Instance.OnDialogueEnd();
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}