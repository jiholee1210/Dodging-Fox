using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum EAudioMixerType{Master,BGM,SFX}
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set;}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] public AudioClip jumpClip;

    [SerializeField] private TMP_Text[] volText;
    [SerializeField] private Slider[] volSliders;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        SetDefaultVolume();
    }

    public void SetAudioVolume(EAudioMixerType audioMixerType, float volume) {
        string volTag = audioMixerType.ToString();
        int uiVolume = (int)Mathf.Clamp(volume * 100f, 0, 100);
        int value = (int)audioMixerType;
        volText[value].text = uiVolume.ToString();

        PlayerPrefs.SetFloat(volTag, volume);
        audioMixer.SetFloat(audioMixerType.ToString(), Mathf.Log10(volume) * 20);
    }

    public void ChangeBGMVolume(float volume) {
        SetAudioVolume(EAudioMixerType.BGM, volume);
    }

    public void ChangeMasterVolume(float volume) {
        SetAudioVolume(EAudioMixerType.Master, volume);
    }

    public void ChangeSFXVolume(float volume) {
        SetAudioVolume(EAudioMixerType.SFX, volume);
    }

    private void SetDefaultVolume() {
        for(int i = 0; i < 3; i++) {
            EAudioMixerType audioMixerType = (EAudioMixerType)Enum.ToObject(typeof(EAudioMixerType), i);

            string tag = audioMixerType.ToString();
            Debug.Log(tag);
            float vol = PlayerPrefs.GetFloat(tag);

            audioMixer.SetFloat(tag, Mathf.Log10(vol) * 20);
            volSliders[i].value = PlayerPrefs.GetFloat(tag);
            volText[i].text = ((int)Mathf.Clamp(vol * 100f, 0, 100)).ToString();
        }
    }
}
