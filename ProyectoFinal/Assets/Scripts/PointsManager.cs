using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;

public class PointsManager : MonoBehaviour
{
    private int currentPoints;
    [SerializeField] private TMP_Text textPoints;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void morePoints(int points){
        Debug.Log("morePoints del UI recibio la llamada al evento");
        currentPoints += points;
        textPoints.text = currentPoints.ToString();
    }
}
