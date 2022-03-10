using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffPrimary : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float despawnTime = 10f;
    [SerializeField] LayerMask aimColliderMask;
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
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & aimColliderMask) != 0) // esto es para poder usar un layer mask en el ontrigger method. parece que funciona bien pero no lo entiendo.
        {
            Destroy(this.gameObject);
        }
    }
}
