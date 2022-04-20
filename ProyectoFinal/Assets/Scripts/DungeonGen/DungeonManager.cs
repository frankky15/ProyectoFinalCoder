using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;
    public bool GenerateOnStart = true;
    [Range(3, 100)] public int RoomCount = 9;
    public LayerMask CellLayer;
    public Cell MainRoom;
    public GameObject InsteadDoor; // Placa que se pone en las salidas sin utilizar.
    public GameObject[] DoorPrefabs; // Array con diferentes prefabs de puertas.
    public Cell[] CellPrefabs; // Array con diferentes prefabs de salas.
    [SerializeField] private GameObject GenLoadScreen;
    [SerializeField] private Slider SliderProgress;
    [SerializeField] private float timeToBreak = 10f;
    private bool breakOn;
    private int RoomsBuilt;
    [HideInInspector] public bool finished;
    private float timeNow;
    
    [SerializeField] private UnityEvent onAwake;
    // [SerializeField] private UnityEvent onBreak;
    [SerializeField] private UnityEvent onFinish;


    private void Awake() 
    {
        Instance = this;

        onAwake?.Invoke();

        SliderProgress.maxValue = RoomCount;
        SliderProgress.minValue = 3;
    }
    private void Start()
    {
        if (GenerateOnStart) StartCoroutine(StartGeneration());
        timeNow = Time.time + timeToBreak;
    }
    private void Update() 
    {
        LoadingScreen();
        Breaker();
    }

    private void LoadingScreen()
    {
        SliderProgress.value = RoomsBuilt;
    }

    private IEnumerator StartGeneration()
    {
        GenLoadScreen.SetActive(true); // Cortina de carga!

        List<Transform> CreatedExits = new List<Transform>();
        Cell StartRoom = Instantiate(MainRoom, Vector3.zero, Quaternion.identity); // Primero se crea la celda inicial.
        for (int i = 0; i < StartRoom.Exits.Length; i++) CreatedExits.Add(StartRoom.Exits[i].transform);
        StartRoom.TriggerBox.enabled = true;

        RoomsBuilt++;

        int limit = 1000, roomsLeft = RoomCount - 1;
        while (limit > 0 && roomsLeft > 0) // Segundo, se generan las celdas hijas una por una...
        {
            limit--;

            Cell selectedPrefab = Instantiate(CellPrefabs[Random.Range(0, CellPrefabs.Length)], Vector3.zero, Quaternion.identity);

            int lim = 100;
            bool collided;
            Transform selectedExit; // El transform de la una salida aleatoria de la nueva celda... (recomiendo ver prefabs de celdas y Cell.cs para entender como funcionan las salidas)
            Transform createdExit; // El transform de una salida aleatorea dentro de CreatedExists[] (todas las del mapa que esten desocupadas)...

            selectedPrefab.TriggerBox.enabled = false; // se deshabilita el triggerbox para que en la siguiente etapa de checking no se tenga en cuenta el propio collider de la celda.

            do
            {
                lim--;

                createdExit = CreatedExits[Random.Range(0, CreatedExits.Count)];
                selectedExit = selectedPrefab.Exits[Random.Range(0, selectedPrefab.Exits.Length)].transform;

                // rotation
                float shiftAngle = createdExit.eulerAngles.y + 180 - selectedExit.eulerAngles.y;
                selectedPrefab.transform.Rotate(new Vector3(0, shiftAngle, 0)); // Rota la createdExit (y en consecuente la celda) enfrentada a la selectedExit...

                // position
                Vector3 shiftPosition = createdExit.position - selectedExit.position;
                selectedPrefab.transform.position += shiftPosition; // Superposiciona las salidas..

                // check // * Checkea con un Physics.CheckBox que la nueva celda no este superpuesta con otra celda...
                Vector3 center = selectedPrefab.transform.position + selectedPrefab.TriggerBox.center.z * selectedPrefab.transform.forward
                    + selectedPrefab.TriggerBox.center.y * selectedPrefab.transform.up
                    + selectedPrefab.TriggerBox.center.x * selectedPrefab.transform.right; // selectedPrefab.TriggerBox.center
                Vector3 size = selectedPrefab.TriggerBox.size / 2f; // half size
                Quaternion rot = selectedPrefab.transform.localRotation;
                collided = Physics.CheckBox(center, size, rot, CellLayer, QueryTriggerInteraction.Collide);

                yield return new WaitForEndOfFrame();

            } while (collided && lim > 0); // Si no se paso el check repite con las salidas disponibles.. #1

            selectedPrefab.TriggerBox.enabled = true; // Se vuelve a activar la Triggerbox...

            if (lim > 0) // Se coloca una puerta en el lugar de el created/selectedExit (y a su vez estos se eliminan)..
            {
                roomsLeft--;
                RoomsBuilt++;

                for (int j = 0; j < selectedPrefab.Exits.Length; j++) CreatedExits.Add(selectedPrefab.Exits[j].transform);

                CreatedExits.Remove(createdExit);
                CreatedExits.Remove(selectedExit);

                Instantiate(DoorPrefabs[Random.Range(0, DoorPrefabs.Length)], createdExit.transform.position, createdExit.transform.rotation);
                DestroyImmediate(createdExit.gameObject);
                DestroyImmediate(selectedExit.gameObject);
            }
            else DestroyImmediate(selectedPrefab.gameObject); // En el caso de que la sala genere colisiones en todas las salidas disponibles  (que pase el lim) se descarta la sala...

            yield return new WaitForEndOfFrame();
        }

        // instead doors // * pone una pared/tapa en las salidas no utilizadas al momento de que el RoomCount sea alcanzado...
        for (int i = 0; i < CreatedExits.Count; i++)
        {
            Instantiate(InsteadDoor, CreatedExits[i].position, CreatedExits[i].rotation);
            DestroyImmediate(CreatedExits[i].gameObject);
        }

        Debug.Log("Finished " + Time.time);

        GenLoadScreen.SetActive(false);

        finished = true;
        onFinish?.Invoke();
    }
    
    private void Breaker()
    {
        if (!finished && (Time.time > timeNow) && !breakOn)
        {
            breakOn = true;
            StartCoroutine(BreakerCoroutine());
        }
    }
    private IEnumerator BreakerCoroutine()
    {
            // onBreak?.Invoke();

        ScoreManager.Instance.SaveScore();
        
        yield return new WaitForSeconds(0.5f);

        LoadingScreenScript.Instance.ReloadLevel();
    }
}

 // ** Creditos a "Art Notes" por el script base del generador..