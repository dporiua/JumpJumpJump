using UnityEngine;

public class RotatingPlatform : MonoBehaviour {
    public float rotationSpeed = 20.0f;
    public bool clockwiseRotation = true;

    void Update() {
        float direction = clockwiseRotation ? -1f : 1f;
        transform.Rotate(0, 0, direction * rotationSpeed * Time.deltaTime);
    }
}