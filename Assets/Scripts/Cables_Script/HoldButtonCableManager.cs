using UnityEngine;
using UnityEngine.EventSystems; // ضروري لجلب نظام اللمس والضغط
using System.Collections.Generic;



public class HoldButtonCableManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isPressed = false;
    [SerializeField] private GameObject prefabToSpawnCable;
    private CableManager spawnedCables ;
    public GameObject bitSelectionPanel;
    public GameObject positionToSpawnCable;
    private ButtonController16bit buttonController16bit;
    private int sizeTruthTable=16;
    [SerializeField] public List<bool> truthTable ;
    private Vector3 panelOffset;
    private int bitIndex ;
   
    

    public void SetBitIndex(int index)
    {
        bitIndex=index;
    }
    void Start()
    {
        ButtonController_CableManager buttonController =positionToSpawnCable.GetComponent<ButtonController_CableManager>();
        sizeTruthTable = buttonController.GetsizeCableManagers();
        // truthTable = new List<bool>(sizeTruthTable);
    }
    


    public void SetPanelOffset(Vector3 offset)
    {
        panelOffset = offset;
    }
    public void SetPrefabToSpawnCable(GameObject prefab)
    {
        prefabToSpawnCable = prefab;
    }
   
    public void Creat_A_Cable()
    {
        ButtonController_CableManager buttonController =positionToSpawnCable.GetComponent<ButtonController_CableManager>();
        if (buttonController == null)
        {
            Debug.LogError("ButtonController_CableManager component not found on positionToSpawnCable.");
            return;
        }
        Debug.Log($"added cable manager {bitIndex}");
        
        
        if (spawnedCables != null)
        {
            Debug.Log($"i have a cable on button");
            
            // buttonController.GetSelectedCable().Disconnect();
            buttonController.CloseBitSelectionUI();
            
        }
        else
        {
            if (prefabToSpawnCable != null)
            {
                
                GameObject newCable = Instantiate(prefabToSpawnCable, transform.position, transform.rotation, positionToSpawnCable.transform.parent);
                // Debug.Log(newCable.transform.position);
                // Debug.Log("EndA Pos: " + transform.position);
                newCable.transform.localScale = transform.localScale;
                // Transform cable1Transform = newCable.transform.Find("cable1");
                
                Debug.Log($"added cable manager {bitIndex}");

                if (newCable != null)
                {
                    CableManager cableScript = newCable.GetComponent<CableManager>();
                    if (cableScript != null)
                    {
                        spawnedCables = cableScript;
                        // Debug.Log($"sameh {spawnedCables[bitIndex] == null} cable manager");
                        
                            
                        Cable selectedCable = buttonController.GetSelectedCable();
                        if(selectedCable != null && bitIndex >=0 && bitIndex<sizeTruthTable)
                        {
                            cableScript.ConnectCable(selectedCable);
                            Debug.Log($"sameh {selectedCable} cable manager");
                            Debug.Log($"sameh {bitIndex} bit manager");
                            // buttonController.truthTable[bitIndex].truthTable = new List<bool>(selectedCable.truthTable);
                            // truthTable = new List<bool>(selectedCable.truthTable);
                            buttonController.spawnedCableManagers[bitIndex]= spawnedCables;
                            // selectedCable.SetButton_cableManager( bitButtons[bitIndex]);
                            selectedCable.SetTargetConnector(cableScript);
                            // index++;
                        
                            // printALLTruthTables();
                            Debug.Log("selectedCable  wall3at");
                        }
                        else
                        {
                            Debug.Log("selectedCable is null wall3at");
                        }
                        
                        // Debug.Log("dragging");
                    }
                    else
                    {
                        Debug.LogError("i did not find a cable1");
                    }
                }
                else
                {
                    Debug.LogError("i cant find a script of cable");
                }
            }
        }
        Debug.Log("selectedCable  wall3at0000000000000000000000000000000000000000");
        // bitButtons[bitIndex].interactable = false;
        bitSelectionPanel.SetActive(false);
        // iselected=false;
    }

   
    public void OnPointerDown(PointerEventData eventData)
    {
        
        isPressed = true;
        Debug.Log($"{bitIndex}");
        Creat_A_Cable(); 
        Debug.Log("Hold button is being pressed.");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
