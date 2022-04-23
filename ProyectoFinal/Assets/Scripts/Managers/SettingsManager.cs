using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private TMPro.TMP_Dropdown displayMode;
    [SerializeField] private TMPro.TMP_Dropdown qualityMode;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensSlider;
    [SerializeField] private Slider renderDistanceSlider;
    // [SerializeField] private Slider fovSlider;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private bool playerInScene = true;

    private void Start()
    {
        InitSettings();
    }


    public void SetScreenMode(int value)
    {
        if (value == 0)
        {
            Screen.fullScreen = true;
            PlayerPrefs.SetInt("screenmode", value);
        }
        else if (value == 1)
        {
            Screen.fullScreen = false;
            PlayerPrefs.SetInt("screenmode", value);
        }
    }

    public void SetQualityMode(int value)
    {
        QualitySettings.SetQualityLevel(value);
        PlayerPrefs.SetInt("qualitymode", value);
    }

    public void SetVolume(float volume)
    {
        // Debug.Log(volume);
        audioMixer.SetFloat("volume", Mathf.Log10(volume) *20);
        PlayerPrefs.SetFloat("volume", Mathf.Log10(volume) *20);
    }

    public void SetSensitivity(float sensitivity)
    {
        // Debug.Log(sensitivity);
        PlayerPrefs.SetFloat("sensitivity", sensitivity);
        // PlayerHandler.Instance.sensMultiplier = sensitivity;
        if (playerInScene) PlayerHandler.Instance.sensMultiplier = sensitivity;
    }

    public void SetRenderDistance(float renderDistance)
    {
        // Debug.Log(renderDistance);
        PlayerPrefs.SetFloat("rendist", renderDistance);
    }

    public void ToggleMusic(bool toggleMusic) // prove de varias maneras pero no se porque el mixer no se actualiza con el toggle.. hay que darle dos veces, te la devo.
    {
        if (toggleMusic)
        {
            musicMixer.SetFloat("volume", 0);
            PlayerPrefs.SetInt("music", 1);
        }
        else
        {
            musicMixer.SetFloat("volume", -80);
            PlayerPrefs.SetInt("music", 0);
        }
    }

    public void SetFov(float fov)
    {
        Debug.Log(fov);
    }

    public void SaveConfig()
    {
        PlayerPrefs.Save();
    }

    private bool IntToBool(int value)
    {
        if (value == 1) return true;
        else return false;
    }
    private int BoolToInt(bool boolean)
    {
        if (boolean) return 1;
        else return 0;
    }

    public void InitSettings()
    { // Inicializacion de los sliders..
        volumeSlider.minValue = 0.0001f;
        volumeSlider.maxValue = 1f;

        sensSlider.minValue = 1f;
        sensSlider.maxValue = 10f;

      // Inicializacion de los valores...
        displayMode.value = PlayerPrefs.GetInt("screenmode", 1);
        qualityMode.value = PlayerPrefs.GetInt("qualitymode", 1);
        musicToggle.isOn = IntToBool(PlayerPrefs.GetInt("music", 1));
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 0.8f);
        sensSlider.value = PlayerPrefs.GetFloat("sensitivity", 4f);
        renderDistanceSlider.value = PlayerPrefs.GetFloat("rendist", 20f);

      // Asignacion de valores..
        if (playerInScene) PlayerHandler.Instance.sensMultiplier = sensSlider.value;
    }
}
