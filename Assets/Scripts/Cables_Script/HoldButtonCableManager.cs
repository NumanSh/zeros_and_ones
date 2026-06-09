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
    [SerializeField] public List<Cable16bitTruthTable> truthTable = new List<Cable16bitTruthTable>(new Cable16bitTruthTable[16]);
    private Vector3 panelOffset;
    public int bitIndex;
   

    


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
        if (spawnedCables != null)
        {
            Debug.Log($"i have a cable on button");
        }
        else
        {
            if (prefabToSpawnCable != null)
            {
                
                GameObject newCable = Instantiate(prefabToSpawnCable, transform.position, transform.rotation, transform.parent);
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
                        if (!cableScript.CanConnect() )
                        {
                            Debug.Log("it is full");
                        }
                        else
                        {
                            ButtonController_CableManager buttonController =positionToSpawnCable.GetComponent<ButtonController_CableManager>();
                            if (buttonController == null)
                            {
                                Debug.LogError("ButtonController_CableManager component not found on positionToSpawnCable.");
                                return;
                            }
                            Cable selectedCable = buttonController.GetSelectedCable();
                            if(selectedCable != null)
                            {
                                cableScript.ConnectCable(selectedCable);
                                truthTable[bitIndex].truthTable = new List<bool>(selectedCable.truthTable);
                                // selectedCable.SetButton_cableManager( bitButtons[bitIndex]);
                                // selectedCable.SetTargetConnector(cableScript);
                                // index++;
                            
                                // printALLTruthTables();
                                Debug.Log("selectedCable  wall3at");
                            }
                            else
                            {
                                Debug.Log("selectedCable is null wall3at");
                            }
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
        // Creat_A_Cable(); 
        Debug.Log("Hold button is being pressed.");
    }

    // تتنفذ تلقائياً أول ما يرفع اللاعب إصبعه عن الزر
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
