using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private KeyCode toggleCursorKey;
    [SerializeField] private KeyCode pauseKey;
    [SerializeField] private float slowmoMult = 0.5f;
    public bool cursorIsLocked = true;
    public bool gameIsPaused {get; private set;}
    [HideInInspector] public float defaultTimescale;
    private float defaultFixedDeltaTime;
    private float slowmoTimescale;
    [SerializeField] private UnityEvent onPause;
    [SerializeField] private UnityEvent onResume;
    public bool isEnabled;

    private void Awake()
    {
        Instance = this;

        defaultTimescale = Time.timeScale;
        defaultFixedDeltaTime = Time.fixedDeltaTime;
        slowmoTimescale = Time.timeScale * slowmoMult;
    }

    private void Update()
    {
        if (isEnabled)
        {
            ToggleCursor();
            PauseMenu();
        }
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
    public void EnableUpdate(bool state)
    {
        isEnabled = state;
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
        onPause?.Invoke();

        cursorIsLocked = false;
        TimeManager("pause");
        gameIsPaused = true;

        Debug.Log("llamada recivida desde Pause()");
    }
    public void Resume()
    {
        onResume?.Invoke();

        cursorIsLocked = true;
        TimeManager("default");
        gameIsPaused = false;

        Debug.Log("llamada recivida desde Resume()");
    }
}
