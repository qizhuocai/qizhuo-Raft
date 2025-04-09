using UnityEngine;

public class RespawnTrigger : MonoBehaviour {
    
    void OnTriggerEnter(Collider other) {
        
        if (other.tag != "Player") return;

        other.transform.position = new Vector3(0.0f, 2.0f, 0.0f);

    }

}