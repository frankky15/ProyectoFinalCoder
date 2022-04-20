using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance;
    [SerializeField] private GameObject gameOverScreen;
    private UnityEvent onGameOver;
    private bool isDead;

    private void Awake()
    {
        Instance = this;
        gameOverScreen.SetActive(false);
    }
    public void PlayerDied()
    {
        if (!isDead) StartCoroutine(_PlayerDied());
        isDead = true;
    }
    private IEnumerator _PlayerDied()
    {
        gameOverScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        yield return new WaitForSeconds(10);

        LoadingScreenScript.Instance.LoadLevel(0);
    }
}
