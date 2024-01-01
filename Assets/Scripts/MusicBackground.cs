using System;
using UnityEngine;

public class MusicBackground : MonoBehaviour {

    private AudioSource _audioSource;
    
    private void Start() {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if(PlayerPrefs.GetString("music") == "Yes" && !_audioSource.isPlaying)
            _audioSource.Play();
        else if(PlayerPrefs.GetString("music") == "No" && _audioSource.isPlaying)
            _audioSource.Stop();
    }
}
