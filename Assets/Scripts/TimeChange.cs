using UnityEngine;

public class TimeChange : MonoBehaviour {

    public float scrollSpeed = 0.2f;
    public Material material;



    private void Start() {
        material.mainTextureOffset = new Vector2(Random.Range(0f, 0.2f),0);
    }

    private void Update() {
        float offset = scrollSpeed * Time.deltaTime;
        material.mainTextureOffset += new Vector2(offset, 0);
    }
}
