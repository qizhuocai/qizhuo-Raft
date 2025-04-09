using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour {

    [Header("Ghost Foundation")]
    [SerializeField] private float maxDistace = 10.0f;
    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private Transform ghostFoundation;

    [Header("Actually building the foundation")]
    [SerializeField] private GameObject foundation;
    [SerializeField] private Transform foundationParent;

    [Header("Misc.")]
    [SerializeField] private List<GameObject> defaultFoundations = new List<GameObject>();

    private Dictionary<Vector3, GameObject> foundations = new Dictionary<Vector3, GameObject>();

    void Start() {
        
        foreach (GameObject defaultFoundation in defaultFoundations) {

            foundations.Add(defaultFoundation.transform.localPosition, defaultFoundation);

        }

    }
    
    void Update() {
        
        if (!Player.instance.gameStarted || Player.instance.gamePaused) return;

        HandleGhostFoundation();

        if (!ghostFoundation.gameObject.activeSelf || foundations.ContainsKey(ghostFoundation.localPosition) || !Input.GetKeyDown(KeyCode.Mouse0) || !Inventory.instance.HasItem(4, 1)) return;

        ghostFoundation.gameObject.SetActive(false);
        foundations.Add(ghostFoundation.localPosition, Instantiate(foundation, ghostFoundation.position, ghostFoundation.rotation, foundationParent));
        Inventory.instance.RemoveItem(4, 1);

    }

    void OnDisable() {
        
        ghostFoundation.gameObject.SetActive(false);

    }

    private void HandleGhostFoundation() {

        RaycastHit hit;
        if (!Physics.Raycast(Player.instance.head.position, Player.instance.head.forward, out hit, maxDistace, raycastMask)) {

            ghostFoundation.gameObject.SetActive(false);
            return;

        }

        Vector3 position = new Vector3(Mathf.RoundToInt(hit.point.x / 4.0f) * 4.0f, 0.0f, Mathf.RoundToInt(hit.point.z / 4.0f) * 4.0f);
        if(!foundations.ContainsKey(new Vector3(position.x + 4.0f, 0.0f, position.z)) && !foundations.ContainsKey(new Vector3(position.x - 4.0f, 0.0f, position.z)) &&
           !foundations.ContainsKey(new Vector3(position.x, 0.0f, position.z + 4.0f)) && !foundations.ContainsKey(new Vector3(position.x, 0.0f, position.z - 4.0f))) {

            ghostFoundation.gameObject.SetActive(false);
            return;

        }

        ghostFoundation.localPosition = position;
        if (!ghostFoundation.gameObject.activeSelf) ghostFoundation.gameObject.SetActive(true);

    }

}