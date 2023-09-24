using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private bool _isMainmenu;
    [SerializeField] private AudioClip _musicCut;
    [SerializeField] private AudioClip _musicLoop;



    private AudioSource _musicAudioSource;
    private float _startTime;
    public bool _isLooping = false;


    private void Start() {
        _musicAudioSource = GetComponent<AudioSource>();
        _musicAudioSource.clip = _musicCut;
        _musicAudioSource.outputAudioMixerGroup = _audioMixerGroup;
        if (_isMainmenu) {
            _musicAudioSource.loop = true;
        }
        else {
            _musicAudioSource.loop = false;
        }
        _startTime = Time.time;
        _musicAudioSource.Play();
    }

    private void Update() {
        if (!_isLooping && Time.time - _startTime > _musicCut.length && !_isMainmenu) {
            _musicAudioSource.clip = _musicLoop;
            _musicAudioSource.loop = true;
            _musicAudioSource.Play();
            _isLooping = true;
        }
    }
}
