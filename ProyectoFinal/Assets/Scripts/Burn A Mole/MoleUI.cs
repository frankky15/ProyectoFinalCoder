using UnityEngine;
using UnityEngine.UI;

public class MoleUI : MonoBehaviour
{
    public static MoleUI Instance;
    [SerializeField] private Text molesKilled;
    [SerializeField] private Text molesLeft;
    [SerializeField] public GameObject gameStart;
    [SerializeField] public GameObject youWon;
    [SerializeField] public GameObject toggleUI;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        molesKilled.text = BurnAMoleScript.Instance.molesKilled.ToString();
        molesLeft.text = BurnAMoleScript.Instance.molesLeft.ToString();
    }
}
