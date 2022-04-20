using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionDataContainer : MonoBehaviour
{
    public static SessionDataContainer Instance;
    public int score;
    [SerializeField] private GameObject[] PlayerClass;
    public GameObject selectedClass;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) DestroyImmediate(gameObject);
    }

    public void SelectClass(int classIndex)
    {
        if (PlayerClass[classIndex] != null) selectedClass = PlayerClass[classIndex];
    }
}
