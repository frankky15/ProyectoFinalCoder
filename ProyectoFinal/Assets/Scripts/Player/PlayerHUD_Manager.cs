using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHUD_Manager : MonoBehaviour
{
    public static PlayerHUD_Manager Instance;

    [SerializeField] private Slider healthBar;
    [SerializeField] private Image fill;
    [SerializeField] private Color32 normalHpColor;
    [SerializeField] private Color32 midHpColor;
    [SerializeField] private Color32 lowHpColor;
    [SerializeField] private UnityEvent normalHp;
    [SerializeField] private UnityEvent midHp;
    [SerializeField] private UnityEvent lowHp;
    [SerializeField] private TMPro.TMP_Text highScoreText;

    private void Awake()
    {
        Instance = this;
        highScoreText.text =  $"HighScore  {PlayerPrefs.GetInt("highscore", 0)}";
    }
    public void UpdateHealthBar(float healthValue)
    {
        healthBar.value = healthValue;

        if (healthBar.value * 100 / (healthBar.maxValue - healthBar.minValue) > 60)
        {
            normalHp?.Invoke();
            fill.color = normalHpColor;
        }
        else if (IsInBetween(healthBar.value * 100 / (healthBar.maxValue - healthBar.minValue), 20, 60))
        {
            midHp?.Invoke();
            fill.color = midHpColor;
        }
        else if (healthBar.value * 100 / (healthBar.maxValue - healthBar.minValue) < 20)
        {
            lowHp?.Invoke();
            fill.color = lowHpColor;
        }
    }
    public void HealthBarInit(float maxVal, float minVal = 0) // inicializacion del HealthBar
    {
        healthBar.minValue = minVal; // por si por algun motivo hay que poner mas de o menos de 0 como valor minimo..
        healthBar.maxValue = maxVal;
    }
    private bool IsInBetween(float value, float a, float b)
    {
        if (value > a && value < b) return true;
        else return false;
    }
}
