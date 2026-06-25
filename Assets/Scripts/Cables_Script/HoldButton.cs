using UnityEngine;
using UnityEngine.EventSystems; // ضروري لجلب نظام اللمس والضغط
using System.Collections.Generic;
using System.Collections;


public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isPressed = false;
    [SerializeField] private GameObject prefabToSpawnCable;
    private Cable spawnedCables ;
    public GameObject bitSelectionPanel;
    public GameObject positionToSpawnCable;
    private ButtonController16bit buttonController16bit;
    [SerializeField] public List<bool> truthTable = new List<bool>(){};
    [SerializeField] public List<Cable16bitTruthTable> truthTable16 = new List<Cable16bitTruthTable>();
    private Vector3 panelOffset;
    private bool turnUI=false;
    
    [SerializeField] public CableTypes type;
    [SerializeField] private GameObject canvas16bit;

    [SerializeField] public GameObject end_point;
    public GameObject GetCanvas16bit()
    {
        return canvas16bit;
    }

    public bool GetTurnUI()
    {
        return turnUI;
    }

    public void SetTurnUI(bool value)
    {
        turnUI = value;
    }

    public void SetTruthTable16(List<Cable16bitTruthTable> newTruthTable)
    {
        truthTable16.Clear();
        foreach (var item in newTruthTable)
        {
            truthTable16.Add(new Cable16bitTruthTable(new List<bool>(item.truthTable))); 
        }
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

    IEnumerator EnableSecondCanvas()
    {
        yield return null; 
        canvas16bit.SetActive(true); 
    }

    public void SetPanelOffset(Vector3 offset)
    {
        panelOffset = offset;
    }
    public void SetPrefabToSpawnCable(GameObject prefab)
    {
        prefabToSpawnCable = prefab;
    }
    IEnumerator WaitAndStartDrag()
    {
        yield return new WaitForSeconds(101f); 
    }
    public void Creat_A_Cable()
    {
        if (spawnedCables != null)
        {
            CableManager cableManager = spawnedCables.getCableManager();
            if (cableManager != null)
            {
                cableManager.DisconnectCable();
                spawnedCables.setCableManager(null);
            }
            WaitAndStartDrag();
            spawnedCables.SetDragging(true);
            spawnedCables.SetIsSelected(true);
            
        }
        else
        {
            if (prefabToSpawnCable != null)
            {
                // Vector3 panelOffset = new Vector3(0.6f, 0f, 0f);
                GameObject newCable = Instantiate(prefabToSpawnCable, positionToSpawnCable.transform.position+panelOffset, positionToSpawnCable.transform.rotation, positionToSpawnCable.transform.parent);
                //  GameObject newCable = Instantiate(prefabToSpawnCable, transform, Quaternion.identity, null);
                newCable.transform.localScale = transform.localScale;
                // Transform cable1Transform = newCable.transform.parent.Find("Cable");

                // if (cable1Transform != null)
                // {
                    Cable cableScript = newCable.GetComponent<Cable>();
                    if (cableScript != null)
                    {
                        spawnedCables = cableScript;
                        cableScript.SetDragging(true);
                        cableScript.SetTruthTable(truthTable);
                        cableScript.SetIsSelected(true);
                        Debug.Log("dragging");
                    }
                    else
                    {
                        Debug.LogError("i did not find a cable1");
                    }
                // }
                // else
                // {
                //     Debug.LogError("i cant find a script of cable");
                // }
            }
        }
        bitSelectionPanel.SetActive(false);
    }

   
    public void OnPointerDown(PointerEventData eventData)
    {
        // isPressed = true;
        // Creat_A_Cable(); 
        // Debug.Log("Hold button is being pressed.");
        if (type == CableTypes.c1)
        {
            isPressed = true;
            Creat_A_Cable(); 
            Debug.Log("Hold button is being pressed.");
        }
        else if (type == CableTypes.c16)
        {
            
            isPressed = true;
            Debug.Log("Hold button is being pressed 16.");
            // CreatUI16BitCable();
            canvas16bit.SetActive(true);
            bitSelectionPanel.SetActive(false);
            if (end_point != null)
            {
                end_point.GetComponent<ButtonController16bit>().SetTruthTable(truthTable16);
            }
            // StartCoroutine(EnableSecondCanvas());
            SetTurnUI(true);
            
            
        }
        else
        {
            Debug.LogError("Invalid cable type.");
        }
    }

       public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
