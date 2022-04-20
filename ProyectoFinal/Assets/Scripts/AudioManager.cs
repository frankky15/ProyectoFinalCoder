using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioSource audioData;
    [SerializeField] private AudioClipContainer audioContainer;
    [SerializeField] private bool playOnAwake;
    [SerializeField] private int audioToPLay;
    private void Start()
    {
        audioData = GetComponent<AudioSource>();
        if (playOnAwake) PlayAudio(audioToPLay);
    }
    public void PlayAudio(int index)
    {
        switch (index)
        {
            case 0:
                audioData.clip = audioContainer.audio0;
                audioData.volume = audioContainer.audioVol0;
                break;
            case 1:
                audioData.clip = audioContainer.audio1;
                audioData.volume = audioContainer.audioVol1;
                break;
            case 2:
                audioData.clip = audioContainer.audio2;
                audioData.volume = audioContainer.audioVol2;
                break;
            case 3:
                audioData.clip = audioContainer.audio3;
                audioData.volume = audioContainer.audioVol3;
                break;
            case 4:
                audioData.clip = audioContainer.audio4;
                audioData.volume = audioContainer.audioVol4;
                break;
            case 5:
               audioData.clip = audioContainer.audio5;
                audioData.volume = audioContainer.audioVol5;
                break;
            case 6:
                audioData.clip = audioContainer.audio6;
                audioData.volume = audioContainer.audioVol6;
                break;
            case 7:
                audioData.clip = audioContainer.audio7;
                audioData.volume = audioContainer.audioVol7;
                break;
            case 8:
                audioData.clip = audioContainer.audio8;
                audioData.volume = audioContainer.audioVol8;
                break;
            case 9:
                audioData.clip = audioContainer.audio9;
                audioData.volume = audioContainer.audioVol9;
                break;
        }
        audioData.Play();
    }
}
