using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] int sceneToCharge = 0; //Primer nivel para el boton de new game, o un valor que se obtenga para el continue

    private float timer = 0; //Timer para las animaciones del cubo
    [SerializeField] private float[] waitTime = new float[3]; //Lista de wait time para las animaciones

    private int buttonPressed = -1;

    private bool isOnSettings = false; //variable para saber si se esta en las opciones

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
        switch(buttonPressed){
            case 0:
                NewGame();
                break;
            case 1:
                ContinueButton();
                break;
            case 2:
               Settings();
                break;
            case 3:
                ExitGame();
                break;
        }
        if(buttonPressed >= 0){
            timer += Time.deltaTime;
        }
    }
    public void GetButtonPressed(int buttonIndex){
        buttonPressed = buttonIndex;
        principalMenu.SetActive(false); //Desactivo el panel de menu
        settingsPanel.SetActive(false); //Desactivo el panel de opcionesprincipalMenu
        if(buttonIndex == 2){
            isOnSettings = !isOnSettings;
        }
    }

    public void NewGame(){ //Al apretar el boton de nuevo juego:
        backgroundAnimator.Play("GoToNewGame"); //Activo la animacion de juego nuevo
        if(timer >= waitTime[0]){
           SceneManager.LoadScene(sceneToCharge); //Se carga la primera escena que este en build settings
        }
    }
    public void ContinueButton(){
        backgroundAnimator.Play("ContinueAnimation");
        if(timer >= waitTime[1]){
           SceneManager.LoadScene(sceneToCharge); //Se carga la primera escena que este en build settings
        }
    }
    

    public void Settings(){
        if (isOnSettings)
        {
            backgroundAnimator.Play("GoToSettings");
            if(timer >= waitTime[2]){
                buttonPressed = -1;
                timer = 0;
                principalMenu.SetActive(false);
                settingsPanel.SetActive(true);
            }
            
        }
        else
        {
            backgroundAnimator.Play("ReturnFromSettings");
            if(timer >= waitTime[2]){
                buttonPressed = -1;
                timer = 0;
                principalMenu.SetActive(true);
                settingsPanel.SetActive(false);
            }
            
        }
    }

    public void ExitGame(){ //Al apretar el boton de salir del juego:
        backgroundAnimator.Play("ExitAnimation");
        if(timer >= waitTime[3]){
            Application.Quit(); //Cierra el juego (Solo disponible cuando el juego tenga un formato .exe)
        }
    }
}
