using UnityEngine;

public class CameraZoomToMouse : MonoBehaviour
{
    private Camera cam;

    
    public float zoomSpeed = 3f;
    public float minSize = 2f;
    public float maxSize = 15f;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            
            Vector3 mouseWorldPosBeforeZoom = cam.ScreenToWorldPoint(Input.mousePosition);

            
            float newSize = cam.orthographicSize - (scroll * zoomSpeed);
            cam.orthographicSize = Mathf.Clamp(newSize, minSize, maxSize);

            
            Vector3 mouseWorldPosAfterZoom = cam.ScreenToWorldPoint(Input.mousePosition);

            
            Vector3 difference = mouseWorldPosBeforeZoom - mouseWorldPosAfterZoom;
            Vector3 newCameraPosition = transform.position + difference;

            
            newCameraPosition.z = transform.position.z;
            transform.position = newCameraPosition;
        }
    }
}
