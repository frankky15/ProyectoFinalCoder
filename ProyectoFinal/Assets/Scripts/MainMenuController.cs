using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text highScoreText;
    private int highScore;

    private void Awake()
    {
        highScore = PlayerPrefs.GetInt("highscore", 0);
        highScoreText.text = $"HighScore  {highScore.ToString()}";
    }
    public void QuitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }
}
