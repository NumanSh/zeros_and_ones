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
    // no longer used to place the spawned cable - the cable is now positioned from the
    // socket's own hierarchy, which stays correct however the workspace is scaled.
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
        ButtonController16bit manager1bit = positionToSpawnCable.GetComponent<ButtonController16bit>();
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
                // The cable's start_point/end_point children are authored in the same local
                // frame as the socket itself, so the cable lines up exactly when the cable root
                // takes over the socket parent's transform - drop it in as a sibling of the
                // socket at the parent's origin with an identity local transform.
                // (The old panelOffset was a hand-measured stand-in for that offset. It was both
                // slightly off and wrong as soon as the workspace was scaled.)
                Transform socket = positionToSpawnCable.transform;
                Transform cableParent = socket.parent != null ? socket.parent : socket;
                GameObject newCable = Instantiate(prefabToSpawnCable, cableParent);
                newCable.transform.localPosition = Vector3.zero;
                newCable.transform.localRotation = Quaternion.identity;
                newCable.transform.localScale = Vector3.one;

                Cable cableScript = newCable.GetComponent<Cable>();
                if (cableScript != null)
                {
                    spawnedCables = cableScript;
                    cableScript.SetDragging(true);
                    // manager1bit.spawnedCables.Add(cableScript);
                    cableScript.SetTruthTable(truthTable);
                    cableScript.SetIsSelected(true);
                    Debug.Log("dragging");
                }
                else
                {
                    Debug.LogError("i did not find a cable1");
                }
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
