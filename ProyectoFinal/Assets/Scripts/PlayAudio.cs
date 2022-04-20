using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAudio : MonoBehaviour
{
    [SerializeField] private AudioClipContainer audioContainer;
    private AudioSource audioData;
    private void Awake() {
        audioData = GetComponent<AudioSource>();
    }
    private void Start()
    {
        audioData.spatialBlend = 1f;
    }
    public void PlayAudioOneShot(int index)
    {
        switch (index)
        {
            case 0:
                audioData.PlayOneShot(audioContainer.audio0);
                audioData.volume = audioContainer.audioVol0;
                break;
            case 1:
                audioData.PlayOneShot(audioContainer.audio1);
                audioData.volume = audioContainer.audioVol1;
                break;
            case 2:
                audioData.PlayOneShot(audioContainer.audio2);
                audioData.volume = audioContainer.audioVol2;
                break;
            case 3:
                audioData.PlayOneShot(audioContainer.audio3);
                audioData.volume = audioContainer.audioVol3;
                break;
            case 4:
                audioData.PlayOneShot(audioContainer.audio4);
                audioData.volume = audioContainer.audioVol4;
                break;
            case 5:
                audioData.PlayOneShot(audioContainer.audio5);
                audioData.volume = audioContainer.audioVol5;
                break;
            case 6:
                audioData.PlayOneShot(audioContainer.audio6);
                audioData.volume = audioContainer.audioVol6;
                break;
            case 7:
                audioData.PlayOneShot(audioContainer.audio7);
                audioData.volume = audioContainer.audioVol7;
                break;
            case 8:
                audioData.PlayOneShot(audioContainer.audio8);
                audioData.volume = audioContainer.audioVol8;
                break;
            case 9:
                audioData.PlayOneShot(audioContainer.audio9);
                audioData.volume = audioContainer.audioVol9;
                break;
        }
    }
}
