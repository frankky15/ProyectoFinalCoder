using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject[] sheets;
    private Canvas canvas;
    private int index;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("tutorial", 0) == 1) gameObject.SetActive(false);
        
        canvas = GetComponent<Canvas>();
    }
    private void Start()
    {
        sheets[index].SetActive(true);
    }
    private void Update()
    {
        if (DungeonManager.Instance.finished && !GameManager.Instance.gameIsPaused) 
        {
            canvas.worldCamera = FindObjectOfType<Camera>();
            TutorialMode();
        }
    }
    private void TutorialMode()
    {
        if (index < sheets.Length - 1 && Input.anyKeyDown)
        {
            Debug.Log(sheets.Length);
            sheets[index].SetActive(false);
            index++;
            sheets[index].SetActive(true);
        }
        else if (index == sheets.Length - 1 && Input.anyKeyDown)
        {
            PlayerPrefs.SetInt("tutorial", 1);
            gameObject.SetActive(false);
        }
    }
}
