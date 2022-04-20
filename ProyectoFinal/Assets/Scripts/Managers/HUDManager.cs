using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    private int currentPoints;
    [SerializeField] private TMP_Text textPoints;
    [SerializeField] private Slider healthBar;

    private PlayerHandler player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        OnHealthChange();
    }

    public void morePoints(int points){
        Debug.Log("morePoints del UI recibio la llamada al evento");
        currentPoints += points;
        textPoints.text = currentPoints.ToString();
    }
    public void OnHealthChange(){
        healthBar.value = player.health/player.maxHealth;
    }
}
