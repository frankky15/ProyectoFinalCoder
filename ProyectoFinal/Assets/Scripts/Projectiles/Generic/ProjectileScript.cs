using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    protected void Movement(float Speed)
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }
    protected void OnImpact(GameObject _object, LayerMask mask, float despawnTime = 5f, float scaleMult = 1f)  // la deteccion con onCollision funciona mucho mejor que con onTrigger, pero igualmente se siguen perdiendo un par de balas por algun motivo, supongo que la deteccion de colisiones de los rigidbody no es perfecta.
    {
        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out RaycastHit rayhit, 100f, mask))
        {
            GameObject impact = Instantiate(_object, rayhit.point, Quaternion.LookRotation(rayhit.normal)) as GameObject;
            impact.transform.rotation *= Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            impact.transform.localScale *= scaleMult;
            Destroy(impact, despawnTime);
            Destroy(this.gameObject);
        }
    }
}
