using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenScript : MonoBehaviour
{
    public static LoadingScreenScript Instance;

    [SerializeField] private Slider sliderProgress;
    [SerializeField] private GameObject LoadingScreen;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel(int sceneIndex)
    {
        PlayerPrefs.Save();
        StartCoroutine(LoadAsynchronously(sceneIndex));
        // Debug.Log("llamada recivida desde LoadLevel()");
    }
    public void ReloadLevel()
    {
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex));
    }
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log(progress);
            sliderProgress.value = progress;

            yield return null;
        }
        if (operation.isDone)
        {
            LoadingScreen.SetActive(false);
            DestroyImmediate(gameObject);
        }
    }
}