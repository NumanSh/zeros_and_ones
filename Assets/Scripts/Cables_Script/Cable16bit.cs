using UnityEngine;
using System.Collections.Generic;



public class Cable16bit : MonoBehaviour
{
    public  ConnectorType Name;
    
    
    [SerializeField] public List<Cable16bitTruthTable> truthTable = new List<Cable16bitTruthTable>(){};
    private LineRenderer lineRenderer;
    private bool isDragging = false; 
    private CableManager16bit CableManager1 = null; 
    // private List<CableManager> connectedConnectors = new List<CableManager>();
    private CableManager16bit targetConnector = null; 
    public Transform startPoint; 
    public Transform endPoint;   
    private bool isSelected =false;

    public CableManager16bit getCableManager()
    {
        return CableManager1;
    }

    public void SetDragging(bool value)
    {
        isDragging = value;
    }
    public void SetIsSelected(bool value)
    {
        isSelected = value;
    }

    public List<Cable16bitTruthTable> GetTruthTable()
    {
        return truthTable;
    }
    public void SetTruthTable(List<Cable16bitTruthTable> newTruthTable)
    {

        truthTable.Clear();
        foreach (var item in newTruthTable)
        {
            truthTable.Add(new Cable16bitTruthTable(new List<bool>(item.truthTable))); 
        }

    }
    public Transform getendPoint()
    {
        return endPoint;
    }
    void Start()
    {
       
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint.position); 
        lineRenderer.SetPosition(1, endPoint.position); 
    }

    void Update()
    {
        // if (Input.GetMouseButton(0))
        // {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
        //     if (Physics.Raycast(ray, out RaycastHit hitInfo))
        //     {
        //         if (hitInfo.collider.gameObject == endPoint.gameObject)
        //         {
        //             // CableInteraction.showme();
        //             isDragging = true; 
        //         }

        //         if (isDragging)
        //         {
        //             lineRenderer.SetPosition(1, hitInfo.point);
        //             CableManager16bit connector = hitInfo.collider.GetComponent<CableManager16bit>();
                    
        //             if (connector != null && connector.CanConnect())
        //             {
                        
        //                 targetConnector = connector;
        //             }
        //             else
        //             {
        //                 targetConnector = null; 
        //             }
        //         }
                
        //     }
        // }
        // if (Input.GetMouseButtonUp(0))
        // {
        //     if (isDragging && targetConnector != null)
        //     {
        //         if (CableManager1 != null)
        //         {
        //             CableManager1.DisconnectCable();
        //             CableManager1=null;
        //         }
        //         lineRenderer.SetPosition(1, targetConnector.transform.position);
        //         targetConnector.ConnectCable(this);
        //         CableManager1 = targetConnector; 
        //     }
        //     isDragging = false;
        // }
        // if (endPoint != null && !isDragging && targetConnector == null)
        // {
        //     if (CableManager1 != null)
        //     {
        //         CableManager1.DisconnectCable();
        //         CableManager1=null;
        //     }
        //     lineRenderer.SetPosition(1, endPoint.position);
        // }
    
    }
}