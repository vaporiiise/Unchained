using UnityEngine;

public class HeartPulsing : MonoBehaviour
{
     public GameManager gameManager;
    public AudioSource audioSource; 

    [Header("Health Settings")]
    public int maxHealth; // set based on game manager
    public int warningHealth = 50; // Slow heartbeat threshold
    public int criticalHealth = 20; // Fast heartbeat threshold

    [Header("Heartbeat Sounds")]
    public AudioClip slowHeartbeatClip; 
    public AudioClip fastHeartbeatClip; 

    private bool isSlowPlaying = false;
    private bool isFastPlaying = false;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found!");
        }
    }

    private void Update()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager reference is missing!");
            return;
        }

        int currentHealth = gameManager.savedPlayerHealth; // Get current health from GameManager

        if (currentHealth > warningHealth)
        {
            StopHeartbeat();
        }
        else if (currentHealth > criticalHealth) 
        {
            PlayHeartbeat(slowHeartbeatClip, ref isSlowPlaying, ref isFastPlaying);
        }
        else if (currentHealth > 0)
        {
            PlayHeartbeat(fastHeartbeatClip, ref isFastPlaying, ref isSlowPlaying);
        }
        else
        {
            StopHeartbeat();
        }
    }

    private void PlayHeartbeat(AudioClip clip, ref bool isPlaying, ref bool stopOther)
    {
        if (!isPlaying || audioSource.clip != clip)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
            isPlaying = true;
            stopOther = false;
        }
    }

    private void StopHeartbeat()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            isSlowPlaying = false;
            isFastPlaying = false;
        }
    }
}
