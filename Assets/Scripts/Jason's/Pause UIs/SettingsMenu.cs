using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameplayVolumeManager : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;

    private const string MasterKey = "MasterVolume";
    private const string MusicKey = "MusicVolume";
    private const string SFXKey = "SFXVolume";

    private void Start()
    {
        // Load saved values or default to 1
        MasterSlider.value = PlayerPrefs.GetFloat(MasterKey, 1f);
        MusicSlider.value = PlayerPrefs.GetFloat(MusicKey, 1f);
        SFXSlider.value = PlayerPrefs.GetFloat(SFXKey, 1f);

        ApplyVolumes();

        // Add listeners to sliders
        MasterSlider.onValueChanged.AddListener(delegate { SetMasterVolume(); });
        MusicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
        SFXSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
    }

    public void SetMasterVolume()
    {
        float volume = MasterSlider.value;
        myMixer.SetFloat("Master", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat(MasterKey, volume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume()
    {
        float volume = MusicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat(MusicKey, volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat(SFXKey, volume);
        PlayerPrefs.Save();
    }

    private void ApplyVolumes()
    {
        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }
}
