using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GazerSoundEffects : MonoBehaviour
{
    
    [Header("Sound Settings")]
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private AudioClip _gazerEnviHit;
    [SerializeField] private AudioClip _gazerplayerHit;
    [SerializeField] private AudioClip _gazerCharge;
    [SerializeField] private AudioClip _gazerSpawn;
    [SerializeField] private AudioClip _gazerGiggle;
    [SerializeField] private AudioClip _gazerLaugh;
    [SerializeField] private AudioClip _gazerCloseCall;
    [SerializeField] private AudioClip _gazerSob;
    [SerializeField] private AudioClip _gazerWaffling;

    private bool _wafflingPlaying = false;
    private AudioSource _wafflingAudioSource;


    // private void Start() {
    //     _wafflingAudioSource = GetComponent<AudioSource>();
    //     _wafflingAudioSource.clip = _gazerWaffling;
    //     _wafflingAudioSource.outputAudioMixerGroup = _audioMixerGroup;
    //     _wafflingAudioSource.loop = true;
    //     _wafflingAudioSource.Play();
    // }


    public void gazerEnviHit() {
        GameObject audioObj = new GameObject("Gazer Envi Hit");
        AudioSource audioSource = audioObj.AddComponent<AudioSource>();
        audioObj.transform.SetParent(transform);
        audioSource.outputAudioMixerGroup = _audioMixerGroup;
        audioSource.clip = _gazerEnviHit;
        audioSource.Play();
        Destroy(audioObj, audioSource.clip.length);
    }

    public void gazerPlayerHit() {
        GameObject audioObj = new GameObject("Gazer Player Hit");
        AudioSource audioSource = audioObj.AddComponent<AudioSource>();
        audioObj.transform.SetParent(transform);
        audioSource.outputAudioMixerGroup = _audioMixerGroup;
        audioSource.clip = _gazerplayerHit;
        audioSource.Play();
        Destroy(audioObj, audioSource.clip.length);
    }

    public void gazerCharge() {
        GameObject audioObj = new GameObject("Gazer Charge");
        AudioSource audioSource = audioObj.AddComponent<AudioSource>();
        audioObj.transform.SetParent(transform);
        audioSource.outputAudioMixerGroup = _audioMixerGroup;
        audioSource.clip = _gazerCharge;
        audioSource.Play();
        Destroy(audioObj, audioSource.clip.length);
    }

    public void gazerSpawn() {
        GameObject audioObj = new GameObject("Gazer Spawn");
        AudioSource audioSource = audioObj.AddComponent<AudioSource>();
        audioObj.transform.SetParent(transform);
        audioSource.outputAudioMixerGroup = _audioMixerGroup;
        audioSource.clip = _gazerSpawn;
        audioSource.Play();
        Destroy(audioObj, audioSource.clip.length);
    }

    public void gazerGiggle() {
        GameObject audioObj = new GameObject("Gazer Giggle");
        AudioSource audioSource = audioObj.AddComponent<AudioSource>();
        audioObj.transform.SetParent(transform);
        audioSource.outputAudioMixerGroup = _audioMixerGroup;
        audioSource.clip = _gazerGiggle;
        audioSource.Play();
        Destroy(audioObj, audioSource.clip.length);
    }

    public void gazerLaugh() {
        GameObject audioObj = new GameObject("Gazer Laugh");
        AudioSource audioSource = audioObj.AddComponent<AudioSource>();
        audioObj.transform.SetParent(transform);
        audioSource.outputAudioMixerGroup = _audioMixerGroup;
        audioSource.clip = _gazerLaugh;
        audioSource.Play();
        Destroy(audioObj, audioSource.clip.length);
    }

    public void gazerCloseCall() {
        GameObject audioObj = new GameObject("Gazer Close Call");
        AudioSource audioSource = audioObj.AddComponent<AudioSource>();
        audioObj.transform.SetParent(transform);
        audioSource.outputAudioMixerGroup = _audioMixerGroup;
        audioSource.clip = _gazerCloseCall;
        audioSource.Play();
        Destroy(audioObj, audioSource.clip.length);
    }

    public void gazerSob() {
        GameObject audioObj = new GameObject("Gazer Sob");
        AudioSource audioSource = audioObj.AddComponent<AudioSource>();
        audioObj.transform.SetParent(transform);
        audioSource.outputAudioMixerGroup = _audioMixerGroup;
        audioSource.clip = _gazerSob;
        audioSource.Play();
        Destroy(audioObj, audioSource.clip.length);
    }
}
