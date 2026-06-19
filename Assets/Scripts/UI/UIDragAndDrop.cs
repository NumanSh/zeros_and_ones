using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragAndDrop : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
   
    public GameObject gatePrefab; 

    private Vector3 startPosition;

    public Vector3 offsit;
    private bool isOverGreenArea = false;
    private Vector3 mouseWorldPos;

  
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position; 
        GetComponent<CanvasGroup>().blocksRaycasts = false; 
    }

  
    public void OnDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true; 
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;

     
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; 

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("greenArea"))
        {
            isOverGreenArea = true;
        }
        else
        {
            isOverGreenArea = false;
        }
    }

   
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isOverGreenArea && gatePrefab != null)
        {
            
            Instantiate(gatePrefab, mouseWorldPos+offsit, Quaternion.identity);
        }

        transform.position = startPosition;
    }
}
