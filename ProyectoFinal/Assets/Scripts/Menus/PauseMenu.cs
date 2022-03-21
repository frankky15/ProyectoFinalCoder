using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    private bool menuToggle;
    private GameObject menu;
    private void Awake()
    {
        Instance = this;
        menu = GameObject.Find("Menu");
    }
    private void Update()
    {
        menuToggle = GameManager.Instance.gameIsPaused;
        menu.SetActive(menuToggle);
    }
    public void ToggleMenu(bool state)
    {
        menuToggle = state;
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}