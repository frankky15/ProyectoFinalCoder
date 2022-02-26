using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffPrimary : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float despawnTime = 10f;
    private float despawn;
    private float _speed;
    private void Start()
    {
        _speed = speed - transform.localScale.y;
        despawn = Time.time + despawnTime;
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        if (Time.time > despawn)
        {
            Destroy(this.gameObject);
        }
    }
}
