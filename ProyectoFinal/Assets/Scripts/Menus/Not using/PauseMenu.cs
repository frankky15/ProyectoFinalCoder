using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // public static PauseMenu Instance;
    private bool menuToggle;
    [SerializeField] private GameObject menu;

    private void Awake()
    {
        // Instance = this;
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
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLevel(int index)
    {
        LoadingScreenScript.Instance.LoadLevel(index);
    }

    public void ResumeTime()
    {
        Time.timeScale = GameManager.Instance.defaultTimescale;
    }
}