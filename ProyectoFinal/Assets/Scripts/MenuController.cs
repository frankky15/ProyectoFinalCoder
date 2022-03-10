using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] int firstSceneInBuild = 0; //Primer nivel para el boton de new game

    private float timer = 0; //Timer para las animaciones del cubo

    private bool isOnSettings = false; //variable para saber si se esta en las opciones
    private bool buttonGamePressed = false;

    private GameObject principalMenu; //panel del menu principal
    private GameObject settingsPanel; //panel de opciones

    private Animator backgroundAnimator; //Animacion del cubo de fondo
    

    private void Start() {
        principalMenu = GameObject.Find("PrincipalPanel"); //Asigno el menu principal
        settingsPanel = GameObject.Find("SettingsPanel"); //Asigno el panel de opciones
        backgroundAnimator = GameObject.Find("BackgroundMenuCube").GetComponent<Animator>(); //Asigno el animador del cubo de fondo

        principalMenu.SetActive(true); //Activo el panel de menu
        settingsPanel.SetActive(false); //Desactivo el panel de opciones
    }
    private void Update() {
        if (buttonGamePressed)
        {
            timer += Time.deltaTime;
            Debug.Log("buttonGamePressed");
        }
        if(timer >= 1){
           SceneManager.LoadScene(firstSceneInBuild); //Se carga la primera escena que este en build settings
        }
    }

    public void NewGame(){ //Al apretar el boton de nuevo juego:
        backgroundAnimator.Play("GoToNewGame"); //Activo la animacion de juego nuevo
        buttonGamePressed = true;
    }
    public void ContinueButton(){
        backgroundAnimator.Play("ContinueAnimation");
    }
    

    public void Settings(){
        isOnSettings = !isOnSettings;
        if (isOnSettings)
        {
            backgroundAnimator.Play("GoToSettings");
            principalMenu.SetActive(false);
            settingsPanel.SetActive(true);
        }
        else
        {
            backgroundAnimator.Play("ReturnFromSettings");
            principalMenu.SetActive(true);
            settingsPanel.SetActive(false);
        }
    }

    public void ExitGame(){ //Al apretar el boton de salir del juego:
        backgroundAnimator.Play("ExitAnimation");
        Application.Quit(); //Cierra el juego (Solo disponible cuando el juego tenga un formato .exe)
    }

    //Funciones del menu de opciones:
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
    }
    public void ChangeBrightnessValue(Scrollbar scrollBrightness){
        Screen.brightness = scrollBrightness.value;
        //ya que no me funciono, podemos guardar el valor que se obtenga y cambiar la intencidad de la luz global en base al valor guardado
    }
}
