using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // جلب نظام المدخلات الجديد
using System.Collections.Generic;
public class Cable : MonoBehaviour
{

    [Header("Line Renderer")]
    private LineRenderer lineRenderer;

    [Header("Connections (2D)")]
    public Transform startPoint; 
    public Transform endPoint;   
    private CableManager CableManager1 = null; 
    private CableManager targetConnector = null; 

    [Header("State Flags")]
    public bool isDragging = false; 
    private bool isSelected =false;

    public  ConnectorType Name;
    public List<bool> truthTable = new List<bool>();
    private List<CableManager> connectedConnectors = new List<CableManager>();
    private Button button_cableManager =null;
    private ButtonController_CableManager ButtonController;
    public void SetTargetConnector(CableManager value)
    {
        targetConnector = value;
    }

    public void SetButton_cableManager(Button value)
    {
        button_cableManager = value;
    }
    
    public void setCableManager(CableManager connector)
    {
        CableManager1 = connector;
    }
    public CableManager getCableManager()
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

    public List<bool> GetTruthTable()
    {
        return truthTable;
    }
    public void SetTruthTable(List<bool> newTruthTable)
    {
        
        if (truthTable.Count != newTruthTable.Count)
        {
            truthTable = new List<bool>(newTruthTable); 
        }
        else
        {
            for (int i = 0; i < newTruthTable.Count; i++)
            {
                truthTable[i] = newTruthTable[i];
            }
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
        
        if (isDragging )
        {
            Debug.Log("dragging");
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            
            lineRenderer.SetPosition(1, new Vector3(mousePosition.x, mousePosition.y, 0f));

            RaycastHit2D hitInfo = Physics2D.Raycast(mousePosition, Vector2.zero);
            
            if (hitInfo.collider != null)
            {
                CableManager connector = hitInfo.collider.GetComponentInParent<CableManager>();
                ButtonController_CableManager connectorCableManager = hitInfo.collider.GetComponentInParent<ButtonController_CableManager>();
                
                if (connector != null && connector.CanConnect())
                {
                    targetConnector = connector;
                }
                else if (connectorCableManager != null)
                {
                    connectorCableManager.SetIsSelected(true);
                    connectorCableManager.SetSelectedCable(this);
                    connectorCableManager.ShowBitSelectionUI();
                    ButtonController = connectorCableManager;
                }
                else
                {
                    if (ButtonController != null)
                    {
                        ButtonController.CloseBitSelectionUI();
                    }
                    targetConnector = null;
                }
            }
            else
            {
                if (ButtonController != null)
                {
                    ButtonController.CloseBitSelectionUI();
                }
                targetConnector = null;
            }
        }

    
        if (Input.GetMouseButtonDown(0))
        {
            if (isDragging && targetConnector != null)
            {
                if (CableManager1 != null)
                {
                    CableManager1.DisconnectCable();
                    CableManager1 = null;
                }
                if (ButtonController != null)
                {
                    ButtonController.CloseBitSelectionUI();
                }

                lineRenderer.SetPosition(1, new Vector3(targetConnector.transform.position.x, targetConnector.transform.position.y, 0f));
                targetConnector.ConnectCable(this);
                CableManager1 = targetConnector; 
                Debug.Log("save cable");
                isDragging = false;
            }
            
        }
        if (Input.GetMouseButtonDown(1))
        {
            isDragging = false;
        }

        // 4. إذا لم يكن هناك سحب والكابل غير موصول
        if (endPoint != null && !isDragging && targetConnector == null)
        {
            if (CableManager1 != null)
            {
                if (button_cableManager != null)
                {
                    button_cableManager.interactable = true;
                }
                CableManager1.DisconnectCable();
                CableManager1 = null;
            }
            lineRenderer.SetPosition(1, new Vector3(endPoint.position.x, endPoint.position.y, 0f));
        }
    }


}