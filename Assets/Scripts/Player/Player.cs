using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    public static Player instance;
    void Awake() { instance = this; }

    public bool gameStarted = false;
    public bool gamePaused = false;
    public Transform head;

    [SerializeField] private GameObject hook;
    [SerializeField] private GameObject hammer;

    bool tmp = false;

    void Update() {
        
        if (tmp) { gameStarted = true; }
        if (!gameStarted && Input.GetKeyDown(KeyCode.Mouse0)) { tmp = true; return; }
        if (!gameStarted) return;

        if (Input.GetKeyDown(KeyCode.X)) {

            hook.SetActive(true);
            hammer.SetActive(false);

        } else if(Input.GetKeyDown(KeyCode.Y) && Inventory.instance.HasItem(3, 1)) {

            hook.SetActive(false);
            hammer.SetActive(true);

        }

    }

}