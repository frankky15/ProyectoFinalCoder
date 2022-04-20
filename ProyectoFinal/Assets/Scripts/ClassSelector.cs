using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class ClassSelector : MonoBehaviour
{
    [SerializeField] private GameObject toggle;
    [SerializeField] private GameObject[] sheets;
    [SerializeField] private CinemachineVirtualCamera classCam;
    private int index;
    private void Awake()
    {
        toggle.SetActive(false);
        for (int i = 0; i < sheets.Length; i++) sheets[i].SetActive(false);
        sheets[0].SetActive(true);
    }
    public void next()
    {
        if (index < sheets.Length - 1)
        {
            sheets[index].SetActive(false);
            index++;
            sheets[index].SetActive(true);
        }
        else
        {
            sheets[index].SetActive(false);
            index = 0;
            sheets[index].SetActive(true);
        }
    }
    public void previous()
    {
        if (index > 0)
        {
            sheets[index].SetActive(false);
            index--;
            sheets[index].SetActive(true);
        }
        else
        {
            sheets[index].SetActive(false);
            index = sheets.Length - 1;
            sheets[index].SetActive(true);
        }
    }
    public void PickThisClass()
    {
        SessionDataContainer.Instance.SelectClass(index);
        LoadingScreenScript.Instance.LoadLevel(2);
    }
    public void StartClassPicker()
    {
        toggle.SetActive(true);
        classCam.Priority = 11;
        GameManager.Instance.isEnabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
