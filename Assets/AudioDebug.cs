using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioDebug : MonoBehaviour
{
    public AudioMixer myMixer;
    public AudioSource sfxAudioSource;
    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider SFXSlider;

    private void Start()
    {
        Debug.Log("🔍 Debugging Audio System...");
        DebugMixerAssignment();
        DebugPlayerPrefs();
    }

    public void DebugMixerAssignment()
    {
        if (sfxAudioSource != null)
        {
            if (sfxAudioSource.outputAudioMixerGroup == null)
            {
                Debug.LogError("⚠ SFX AudioSource is NOT assigned to an Audio Mixer Group!");
            }
            else
            {
                Debug.Log("✅ SFX AudioSource is correctly assigned to: " + sfxAudioSource.outputAudioMixerGroup.name);
            }
        }
        else
        {
            Debug.LogError("❌ SFX AudioSource is missing!");
        }
    }

    public void DebugPlayerPrefs()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

        Debug.Log($"📀 Loaded Volumes - Master: {master}, Music: {music}, SFX: {sfx}");

        MasterSlider.value = master;
        MusicSlider.value = music;
        SFXSlider.value = sfx;
    }

    public void TestMuteSFX()
    {
        myMixer.SetFloat("SFX", -80f);
        Debug.Log("🔇 SFX should now be muted.");
    }

    public void TestUnmuteSFX()
    {
        myMixer.SetFloat("SFX", 0f);
        Debug.Log("🔊 SFX should now be at normal volume.");
    }

    public void TestPlaySFX()
    {
        if (sfxAudioSource != null && sfxAudioSource.clip != null)
        {
            sfxAudioSource.PlayOneShot(sfxAudioSource.clip);
            Debug.Log("▶ Playing SFX...");
        }
        else
        {
            Debug.LogError("❌ No SFX clip assigned to the AudioSource!");
        }
    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();

        float debugVolume;
        myMixer.GetFloat("SFX", out debugVolume);
        Debug.Log($"SFX Slider: {volume}, Applied dB: {debugVolume}");
    }
}
