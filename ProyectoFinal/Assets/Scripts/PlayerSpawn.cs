using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    private void Awake()
    {
        Player = SessionDataContainer.Instance.selectedClass;
    }
    public void SpawnPlayer()
    {
        Instantiate(Player, Vector3.up, Quaternion.identity, gameObject.transform);
    }
}
