using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Camera cam;

    [Header("Zoom Settings")]
    public float minSize = 2f;
    public float maxSize = 5f;
    public float zoomSpeed = 2f;

    private Vector3 dragOrigin;
    private float targetZoom;
    private Vector3 initialPosition;

    void Start()
    {
        cam = GetComponent<Camera>();
        targetZoom = cam.orthographicSize;
        initialPosition = transform.position;
        maxSize = cam.orthographicSize; // الحجم الأصلي عند البداية هو الحد الأقصى
    }

    void Update()
    {
        // Don't zoom or pan the camera behind an open pause menu.
        if (ZerosAndOnes.UI.PauseMenuController.IsPaused) return;

        HandleZoom();
        HandlePan();
    }

    // زوم لمنتصف الشاشة
    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            targetZoom -= scroll * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minSize, maxSize);
        }

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * 10f);
        ClampCameraPosition();
    }

    // سحب بزر الفأرة الأيمن
    void HandlePan()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position += difference;
            ClampCameraPosition();
        }
    }

    // منع الكاميرا من الخروج عن الحدود الأصلية للشاشة
    void ClampCameraPosition()
    {
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float maxHeight = maxSize;
        float maxWidth = maxHeight * cam.aspect;

        float minX = initialPosition.x - (maxWidth - camWidth);
        float maxX = initialPosition.x + (maxWidth - camWidth);
        float minY = initialPosition.y - (maxHeight - camHeight);
        float maxY = initialPosition.y + (maxHeight - camHeight);

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
