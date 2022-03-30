using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentHandler : MonoBehaviour
{
    public static EquipmentHandler Instance;
    #region Variables
    // [SerializeField] private WeaponScriptableObject[] loadout; // El armamento en un array, para agregar armas desde el editor simplemente agregar un nuevo WeaponScriptableObject con stats del arma.
    [SerializeField] private List<WeaponScriptableObject> loadout = new List<WeaponScriptableObject>(); // lo pase a lista.
    [SerializeField] private Transform weaponParent; // El parent donde se va a instanciar el modelo del arma.
    [SerializeField] private Transform hitscanTransform;
    [SerializeField] private LayerMask aimColliderMask;
    [SerializeField] private GameObject bulletHole;
    private GameObject currentWeapon; // El arma en uso.
    private int currentIndex; // El index del WeaponScriptableObject en uso.
    private Transform cameraCenter;
    private RaycastHit hitscanRaycast;
    private float fireCooldown;
    private float pulloutCooldown; // Un cooldown para cuando sacas el arma y podes empezar a disparar.

    // *PlaceHolders* //
    private GameObject gObjPlaceHolder0; // PlaceHolders para almacenar variables temporalmente. Lo hice asi porque no quiero tener variables unicas para cada metodo y las variables locales no me sirven.
    private GameObject gObjPlaceHolder1;
    private float floatPlaceHolder0;
    private float floatPlaceHolder1;
    private bool boolPlaceHolder0;
    private bool boolPlaceHolder1;
    public bool isShooting {get; private set;}

    #endregion

    #region Unity Calls
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        cameraCenter = transform.Find("Look Pivot/Main Camera");
    }
    private void Update()
    {
        if (!GameManager.Instance.gameIsPaused) // esto no se ejecuta mientras el juego esta pausado.
        {
            WeaponSwitch();
        }
        Hitscan();
    }
    #endregion

    #region Methods
    private void Hitscan() // Crea un raycast desde el centro de la camara y un transform con el punto de colision, esto se usa luego para corregir el angulo del proyectil.
    {
        if (Physics.Raycast(cameraCenter.transform.position, cameraCenter.transform.forward, out hitscanRaycast, 999f, aimColliderMask))
        {
            hitscanTransform.position = hitscanRaycast.point;
        }
    }
    private void WeaponSwitch() // Cambio de armas.
    {
        // *Inputs* //
        if (Input.GetKeyDown(KeyCode.Alpha1)) equipWeapon(0); // Para agregar mas slots simplemente agregar una linea con una nueva tecla y un nuevo index en equipWeapon(index).
        if (Input.GetKeyDown(KeyCode.Alpha2)) equipWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) equipWeapon(2);

        // *Weapon Behaviour* //
        if (currentWeapon != null && (loadout[currentIndex].name == "Staff")) Staff(); // Si el arma en uso se llama "staff" utilizar el siguiente metodo de comportamiento.
        if (currentWeapon != null && (loadout[currentIndex].name == "PyroHand")) Pyronamcer();
    }
    private void equipWeapon(int _index)
    {
        if (_index < loadout.Count)
        {
            if (currentWeapon != null) Destroy(currentWeapon); // Si ya existe un arma equipada borrarla e instanciar lo siguiente.

            GameObject newWeapon = Instantiate(loadout[_index].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
            newWeapon.transform.localPosition = loadout[_index].prefab.transform.position; // Cambio a la pos y rot local del arma a la pos y rot del prefab por compatibilidad y facilidad.
            newWeapon.transform.localRotation = loadout[_index].prefab.transform.rotation;

            pulloutCooldown = loadout[_index].pulloutTime + Time.time;

            currentWeapon = newWeapon;
            currentIndex = _index;

            // placeholders to default //
            boolPlaceHolder0 = false;
            boolPlaceHolder1 = false;
            floatPlaceHolder0 = 0;
            floatPlaceHolder1 = 0;
            gObjPlaceHolder0 = null;
            gObjPlaceHolder1 = null;
        }
    }

    // *Player Classes* //

    // ? Mage Class ? //
    private void Staff() // Arma inicial de la clase mago.
    {
        // Onload // se ejecuta la primera vez que se carga el metodo / en este caso como condicion uso el pullout time por lo que se ejecuta por un tiempo reducido.
        if (pulloutCooldown > Time.time)
        {
            gObjPlaceHolder0 = GameObject.Find("Look Pivot/Weapons/staff(Clone)/Staff Gunpoint Glow"); // el dummy del proyectil durante la carga.
        }
        // Primary //

        if (Input.GetMouseButtonDown(0) && fireCooldown < Time.time && pulloutCooldown < Time.time)
        {
            gObjPlaceHolder0.SetActive(true);
            
            boolPlaceHolder0 = true; // condicional para que el siguiente if no se ejecute sin el primero.
            isShooting = true;
        }
        if (Input.GetMouseButton(0) && fireCooldown < Time.time && pulloutCooldown < Time.time && boolPlaceHolder0) // tuve que poner la condicion de null porque aveces en el primer frame no tiene la referencia del gObj..
        {
            floatPlaceHolder0 = Mathf.Clamp((floatPlaceHolder0 += (loadout[currentIndex].p_castTime / loadout[currentIndex].prefab.transform.localScale.y) * Time.deltaTime),
            loadout[currentIndex].p_projectileMinSize / loadout[currentIndex].prefab.transform.localScale.y,
            loadout[currentIndex].p_projectileMaxSize / loadout[currentIndex].prefab.transform.localScale.y); // tengo que dividirlo para compenzar la escala global.
        }
        if (Input.GetMouseButtonUp(0) && fireCooldown < Time.time && pulloutCooldown < Time.time && boolPlaceHolder0)
        {
            gObjPlaceHolder0.SetActive(false);

            Vector3 aimDir = (hitscanTransform.position - gObjPlaceHolder0.transform.position).normalized;

            GameObject newProjectile = Instantiate(loadout[currentIndex].p_projectile, gObjPlaceHolder0.transform.position, Quaternion.LookRotation(aimDir, Vector3.up)) as GameObject;
            newProjectile.transform.localScale = gObjPlaceHolder0.transform.lossyScale;
            floatPlaceHolder0 = loadout[currentIndex].p_projectileMinSize / loadout[currentIndex].prefab.transform.localScale.y; // le asigno el valor de tamaño del dummy al proyectil.
            Destroy(newProjectile, 6f);

            fireCooldown = loadout[currentIndex].p_fireRate + Time.time;

            boolPlaceHolder0 = false;
            isShooting = false;
        }
        if (gObjPlaceHolder0 != null) gObjPlaceHolder0.transform.localScale = new Vector3(floatPlaceHolder0, floatPlaceHolder0, floatPlaceHolder0);

        // Secondary //

        if (Input.GetMouseButton(1) && fireCooldown < Time.time && pulloutCooldown < Time.time)
        {
            Vector3 aimDir = (hitscanTransform.position - gObjPlaceHolder0.transform.position).normalized;

            GameObject newProjectile = Instantiate(loadout[currentIndex].s_projectile, gObjPlaceHolder0.transform.position, Quaternion.LookRotation(aimDir, Vector3.up)) as GameObject;
            Destroy(newProjectile, 5f);

            fireCooldown = loadout[currentIndex].s_fireRate + Time.time;
            isShooting = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isShooting = false;
        }
    }
    private void Electromancer() // Libro con habilidades electricas.
    {

    }
    private void Pyronamcer() // Libro con habilidades de fuego.
    {
        // Onload // se ejecuta la primera vez que se carga el metodo / en este caso como condicion uso el pullout time por lo que se ejecuta por un tiempo reducido.
        if (pulloutCooldown > Time.time)
        {
            gObjPlaceHolder0 = GameObject.Find("Look Pivot/Weapons/Hand(Clone)/Model/Sphere"); // el dummy del proyectil durante la carga.
        }

        // primary // es como un arpegiador va aumentando el size y cuando llega al maximo empieza a bajar, asi sucesivamente...
        if (Input.GetMouseButton(0) && fireCooldown < Time.time && pulloutCooldown < Time.time)
        {
            Vector3 aimDir = (hitscanTransform.position - gObjPlaceHolder0.transform.position).normalized;

            GameObject newProjectile = Instantiate(loadout[currentIndex].p_projectile, gObjPlaceHolder0.transform.position, Quaternion.LookRotation(aimDir, Vector3.up)) as GameObject;
            Destroy(newProjectile, 2f);
            fireCooldown = loadout[currentIndex].p_fireRate + Time.time;
            isShooting = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isShooting = false;
        }
    }
    #endregion
}