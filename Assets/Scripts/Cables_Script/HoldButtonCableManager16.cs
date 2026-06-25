using UnityEngine;
using UnityEngine.EventSystems; // ضروري لجلب نظام اللمس والضغط
using System.Collections.Generic;



public class HoldButtonCableManager16 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isPressed = false;
    [SerializeField] private GameObject prefabToSpawnCable;
    private CableManager16bit spawnedCables ;
    public GameObject bitSelectionPanel;
    public GameObject positionToSpawnCable;
    private ButtonController16bit buttonController16bit;
    private int sizeTruthTable=16;
    [SerializeField] public List<Cable16bitTruthTable> truthTable ;
    private Vector3 panelOffset;
    // public int bitIndex;
   
    
    void Start()
    {
        truthTable = new List<Cable16bitTruthTable>(new Cable16bitTruthTable[sizeTruthTable]);
    }
    


    public void SetPanelOffset(Vector3 offset)
    {
        panelOffset = offset;
    }
    public void SetPrefabToSpawnCable(GameObject prefab)
    {
        prefabToSpawnCable = prefab;
    }
   
    public void Creat_A_Cable16()
    {
        ButtonController_CableManager buttonController =positionToSpawnCable.GetComponent<ButtonController_CableManager>();
        if (buttonController == null)
        {
            Debug.LogError("ButtonController_CableManager component not found on positionToSpawnCable.");
            return;
        }
        
        
        
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
                
                // Debug.Log($"added cable manager {bitIndex}");

                if (newCable != null)
                {
                    CableManager16bit cableScript = newCable.GetComponent<CableManager16bit>();
                    if (cableScript != null)
                    {
                        spawnedCables = cableScript;
                        // Debug.Log($"sameh {spawnedCables[bitIndex] == null} cable manager");
                        
                            
                        Cable16bit selectedCable16 = buttonController.GetSelectedCable16();
                        if(selectedCable16 != null )
                        {
                            cableScript.ConnectCable(selectedCable16);

                            buttonController.SetTruthTable(selectedCable16.truthTable);
                            // truthTable[bitIndex].truthTable = new List<bool>(selectedCable16.truthTable);
                            // selectedCable.SetButton_cableManager( bitButtons[bitIndex]);
                            selectedCable16.SetTargetConnector(cableScript);
                            buttonController.SetCable16Saved(selectedCable16);
                            // index++;
                        
                            // printALLTruthTables();
                            Debug.Log("selectedCable16  wall3at");
                        }
                        else
                        {
                            Debug.Log("selectedCable16 is null wall3at");
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
        Debug.Log("selectedCable16  wall3at0000000000000000000000000000000000000000");
        // bitButtons[bitIndex].interactable = false;
        bitSelectionPanel.SetActive(false);
        // iselected=false;
    }

   
    public void OnPointerDown(PointerEventData eventData)
    {
        
        isPressed = true;
        Creat_A_Cable16(); 
        Debug.Log("Hold button all16 is being pressed.");
    }

    // تتنفذ تلقائياً أول ما يرفع اللاعب إصبعه عن الزر
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
