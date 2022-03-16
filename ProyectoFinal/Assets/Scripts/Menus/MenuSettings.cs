using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSettings : MonoBehaviour
{
    [SerializeField] private List<Light> lights;

    private List<Light> initialValueLights = new List<Light>();
    private void Start() {
        int i = 0;
        switch (PlayerPrefs.GetInt("screenMode",2))
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
            case 3:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            default:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
        }
        foreach (var Light in lights)
        {
            initialValueLights.Add(lights[i]);
            lights[i].intensity = (PlayerPrefs.GetFloat("lightBright") + 1f)/initialValueLights[i].intensity;
            i++;
        }
    }
   public void ChangeScreenMode(Dropdown screenMode){ //Debugear cuando se exporte el juego
        Debug.Log(screenMode.value);
        switch (screenMode.value)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
            case 3:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            default:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
        }
        PlayerPrefs.SetInt("screenMode",screenMode.value);
    }
    public void ChangeBrightnessValue(Scrollbar scrollBrightness){
        int i = 0;
        foreach (var Light in lights)
        {
            lights[i].intensity = (scrollBrightness.value + 1f)/initialValueLights[i].intensity;
            i++;
        }
        PlayerPrefs.SetFloat("lightBright",scrollBrightness.value);
    }
    public void DeleteAll(){
        PlayerPrefs.DeleteAll();
    }
}
