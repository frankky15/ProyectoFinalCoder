using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private KeyCode toggleCursorKey;
    [SerializeField] private KeyCode pauseKey;
    [SerializeField] private float slowmoMult = 0.5f;
    private bool cursorIsLocked = true;
    public bool gameIsPaused {get; private set;}
    private float defaultTimescale;
    private float defaultFixedDeltaTime;
    private float slowmoTimescale;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        defaultTimescale = Time.timeScale;
        defaultFixedDeltaTime = Time.fixedDeltaTime;
        slowmoTimescale = Time.timeScale * slowmoMult;
    }
    private void Start()
    {
        // PauseMenu.Instance.ToggleMenu(false);
    }
    private void Update()
    {
        ToggleCursor();
        PauseMenu();

        // Debug.Log("timescale is at :" + Time.timeScale);
    }

    private void ToggleCursor()
    {
        if (Input.GetKeyDown(toggleCursorKey)) cursorIsLocked = !cursorIsLocked;

        if (cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (!cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void TimeManager(string state)
    {
        if (state == "pause")
        {
            Time.timeScale = 0f;
            // Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
        }

        else if (state == "slowmo")
        {
            Time.timeScale = slowmoTimescale;
            // Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
        }

        else if (state == "default")
        {
            Time.timeScale = defaultTimescale;
            // Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
        }

        else
        {
            Time.timeScale = defaultTimescale;
            // Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
        }
    }
    private void PauseMenu()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            gameIsPaused = !gameIsPaused;

            if (gameIsPaused)
            {
                Pause();
            }
            if (!gameIsPaused)
            {
                Resume();
            }
        }
    }
    public void Pause()
    {
        cursorIsLocked = false;
        TimeManager("pause");
        gameIsPaused = true;
    }
    public void Resume()
    {
        cursorIsLocked = true;
        TimeManager("default");
        gameIsPaused = false;
    }
}
