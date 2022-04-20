using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [HideInInspector] public int score;
    [HideInInspector] public int highScore;
    [SerializeField] private TMPro.TMP_Text highScoreText;
    [SerializeField] private TMPro.TMP_Text scoreText;
    [SerializeField] private bool scoreSystemToggle = true;

    private void Awake()
    {
        Instance = this;

        highScore = PlayerPrefs.GetInt("highscore", 0);
        score = PlayerPrefs.GetInt("score", 0);
        PlayerPrefs.SetInt("score", 0);
        // highScoreText.text = $"HighScore  {highScore.ToString()}";
        if (scoreSystemToggle) ScoreSystem();
    }
    private void Update()
    {
        // if (scoreSystemToggle) ScoreSystem();
        // Test();
    }
    private void ScoreSystem()
    {
        scoreText.text = $"Score  {score.ToString()}";

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highscore", highScore);
            highScoreText.text = $"HighScore  {highScore.ToString()}";
        }
    }
    public void AddScore(int scoreAmm = 1)
    {
        score += scoreAmm;
    }
    public void SaveScore()
    {
        PlayerPrefs.SetInt("score", score);
    }
    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.Space)) score++;
    }
}
