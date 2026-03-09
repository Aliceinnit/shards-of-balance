using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float parallaxSpeed = 0.5f;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(delta.x * parallaxSpeed, 0, 0);
        lastCameraPosition = cameraTransform.position;
    }
}