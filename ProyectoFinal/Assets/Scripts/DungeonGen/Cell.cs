using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider))]
public class Cell : MonoBehaviour
{
    [HideInInspector] public BoxCollider TriggerBox;
    public GameObject[] Exits;

    private void Awake()
    {
        TriggerBox = GetComponent<BoxCollider>();
        TriggerBox.isTrigger = true;
    }
}
 // El script es super simple, simplemente agrega un BoxCollider (que por lo general se ajusta automaticamente) y te permite desde el editor agregar las salidas a la lista de salidas...