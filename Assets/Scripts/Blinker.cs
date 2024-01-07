using System;
using System.Collections;
using UnityEngine;

public class Blinker : MonoBehaviour {

    private MeshRenderer _mesh;
    
    private void Start() {
        _mesh = GetComponent<MeshRenderer>();
        StartCoroutine(BlinkObject());
    }

    IEnumerator BlinkObject() {
        while (true) {
            yield return new WaitForSeconds(0.5f);
            _mesh.enabled = !_mesh.enabled;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player") && other.transform.localPosition.x == -15) {
            GameObject.Find("GameController").GetComponent<GameController>().needToTurnCarRight = true;
        }
    }
}
