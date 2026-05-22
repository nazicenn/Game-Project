using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothSpeed = 8f;
    public float baseFOV = 60f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);

        if (cam != null)
        {
            float targetFOV = baseFOV + (GroundSpawner.moveSpeed * 1.2f);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, 2f * Time.deltaTime);
        }
    }
}