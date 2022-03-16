using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnAMoleScript : MonoBehaviour
{
    public static BurnAMoleScript Instance;
    [SerializeField] private GameObject Mole;
    public int moleHealth;
    [SerializeField] int moleAmount = 10;
    [SerializeField] int marginOfError = 1;
    [SerializeField] private Transform[] Spawns;
    [SerializeField] private AudioClip bellSound;
    [SerializeField] private AudioClip winSound;
    public float stayTime;
    private float stayCooldown;
    private float startUICooldown;
    private float winUICooldown;
    public int molesLeft;
    public int molesKilled;
    private bool gameIsOn = false;
    private float debounceCooldown;
    private AudioSource bamAudio;

    private void Awake()
    {
        Instance = this;
        molesLeft = moleAmount;
        bamAudio = GetComponent<AudioSource>();
        // gameIsOn = true;
    }
    private void Update()
    {
        if (gameIsOn) SpawnLoop();

        MoleUI.Instance.toggleUI.SetActive(gameIsOn);

        if (molesLeft == 0) // cuando no hay mas topos y perdes
        {
            molesKilled = 0;
            gameIsOn = false;
        }
        if (molesKilled == (moleAmount - marginOfError)) // cuando ganas
        {
            bamAudio.PlayOneShot(winSound);
            molesKilled = 0;
            gameIsOn = false;
            MoleUI.Instance.youWon.SetActive(true);
            winUICooldown = 2f + Time.time;
        }
        if (startUICooldown < Time.time)
        {
            MoleUI.Instance.gameStart.SetActive(false);
        }
        if (winUICooldown < Time.time)
        {
            MoleUI.Instance.youWon.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PyroHandPrimary(Clone)" && gameIsOn && debounceCooldown < Time.time) // cuando reseteas
        {
            gameIsOn = false;
            debounceCooldown = 0.1f + Time.time;
            molesKilled = 0;
        }
        if (other.name == "PyroHandPrimary(Clone)" && !gameIsOn && debounceCooldown < Time.time) // cuando empezas
        {
            molesLeft = moleAmount;
            gameIsOn = true;
            debounceCooldown = 0.1f + Time.time;

            MoleUI.Instance.gameStart.SetActive(true);
            startUICooldown = 1f + Time.time;
        }
    }
    private void SpawnLoop()
    {
        if (stayCooldown < Time.time && molesLeft > 0)
        {
            GameObject NewMole = Instantiate(Mole, Spawns[RandomSpawn()].position, Quaternion.LookRotation(Vector3.left));
            Destroy(NewMole, stayTime);
            stayCooldown = stayTime + Time.time;
            molesLeft--;
        }
    }
    private int RandomSpawn()
    {
        return Random.Range(0, Spawns.Length);
    }
    public void RingBell()
    {
        bamAudio.PlayOneShot(bellSound);
        Debug.Log("played the bell");
    }
}
