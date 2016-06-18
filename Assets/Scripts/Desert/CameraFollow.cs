using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float distanceToTarget;
    public GameObject targetObject;

    void Awake()
    {
        distanceToTarget = transform.position.x - targetObject.transform.position.x;
    }

    void Update()
    {
        float targetObjectX = targetObject.transform.position.x;

        Vector3 newCameraPosition = transform.position;
        newCameraPosition.x = targetObjectX + distanceToTarget;
        transform.position = newCameraPosition;
    }
}
