using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileScriptableObject", menuName = "ScriptableObjects/Projectile")]
public class ProjectileScriptableObject : ScriptableObject
{
    public LayerMask aimColliderMask;
    public LayerMask enemyColliderMask;
    public float speed;
    public float damage;
    public float damageMult;
    public float explosionRadius;
    public GameObject enemyImpact;
    public GameObject defaultImpact;
    public float enemyImpactTime;
    public float defaultImpactTime;
    public AudioClip impactSound;
    public AudioClip shotSound;
}
