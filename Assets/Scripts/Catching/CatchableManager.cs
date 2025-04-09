using System.Collections.Generic;
using UnityEngine;

public class CatchableManager : MonoBehaviour {

    public static CatchableManager instance;
    void Awake() { instance = this; }
    
    [Header("Spawning parameters")]
    [SerializeField] private float spawnRadius = 20.0f;
    [SerializeField] private uint maxCatchables = 100;
    [SerializeField] private uint maxCatchableAmount = 3;

    [Header("Data")]
    [SerializeField] private uint catchables = 0;

    [Header("Waves")]
    [SerializeField] private float waveStrength = 1.0f;

    [Header("Static")]
    [SerializeField] private GameObject catchablePrefab;
    [SerializeField] private List<Item> catchableItems = new List<Item>();

    private List<GameObject> catchablePool = new List<GameObject>();

    void Start() {
        
        for(int i = 0; i < maxCatchables; i++) {

            GameObject catchable = Instantiate(catchablePrefab, new Vector3(Random.Range(-spawnRadius, spawnRadius), 0.0f, Random.Range(-spawnRadius, spawnRadius)), Quaternion.identity, transform);
            catchablePool.Add(catchable);

            catchable.GetComponent<Catchable>().Setup(catchableItems[Random.Range(0, catchableItems.Count)], (uint) Random.Range(1, maxCatchableAmount + 1));
            catchable.GetComponent<Collider>().enabled = true;
            catchable.SetActive(true);
            catchables++;

        }

    }

    void Update() {

        while(catchables < maxCatchables) {

            GameObject catchable = null;
            foreach (GameObject tmp in catchablePool) {

                if (tmp.activeSelf) continue;
                catchable = tmp;
                break;

            }

            catchable.transform.position = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0.0f, Random.Range(-spawnRadius, spawnRadius / 4.0f));
            catchable.GetComponent<Catchable>().Setup(catchableItems[Random.Range(0, catchableItems.Count)], (uint) Random.Range(1, maxCatchableAmount + 1));
            catchable.GetComponent<Collider>().enabled = true;
            catchable.SetActive(true);
            catchables++;

        }

    }

    public void RemoveCatchable(Catchable catchable) {

        catchable.gameObject.SetActive(false);
        catchables--;

    }

    void OnTriggerEnter(Collider other) {
        
        if (other.tag != "Catchable") return;
        other.gameObject.SetActive(false);
        catchables--;

    }

    public float WaveStrength { get { return waveStrength; } }

}