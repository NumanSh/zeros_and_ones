using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class ButtonController_CableManager : MonoBehaviour
{
    public  ConnectorType Name;
    public GameObject bitSelectionPanel;
    public Vector3 panelOffset = new Vector3(100f, 0f, 0f);
    [SerializeField] public List<Cable16bitTruthTable> truthTable = new List<Cable16bitTruthTable>(new Cable16bitTruthTable[16]);
    [SerializeField] public Button myButton;
    // [SerializeField] public Button myButton1;
    // [SerializeField] public Button myButton2;
    [SerializeField] private GameObject prefabToSpawnCable_Mangar;
    [SerializeField] private GameObject prefabToSpawnCable16_Mangar;
    private List<CableManager> spawnedCables = new List<CableManager>(new CableManager[16]);
    [SerializeField] private List<Button> bitButtons = new List<Button>(){};
    private CableManager16bit spawnedCable16 = null;
    public bool iselected=false;
    private Cable selectedCable= null;
    private int index=0;

    public int getSizeTruthTable()
    {
        return index;
    }

    public List<CableManager> GetSpawnedCable_managers()
    {
        return spawnedCables;
    }

    public void SetSelectedCable(Cable cable)
    {
        selectedCable=cable;
    }
    public void SetIsSelected(bool value)
    {
        iselected=value;
    }
    
    public bool GetIsSelected()
    {
        return iselected;
    }
    public List<Cable16bitTruthTable> GetTruthTable()
    {
        return truthTable;
    }
    void Awake()
    {
        for (int i = 0; i < 16; i++)
        {
            truthTable.Add(new Cable16bitTruthTable(new List<bool>(new bool[16])));
        }
    }
    void Start()
    {
        bitSelectionPanel.SetActive(false);
        
        myButton.onClick.AddListener(OnBitSelectedAll);
    }

    public void ShowBitSelectionUI()
    {
        // Debug.Log($"sameh16 hon bas ");
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        bitSelectionPanel.SetActive(true);
        // Debug.Log($"New cable active: {bitSelectionPanel.activeSelf}");
        bitSelectionPanel.transform.position = screenPos + panelOffset;
    }
    public void CloseBitSelectionUI()
    {
        
        bitSelectionPanel.SetActive(false);
        
    }
    public void printALLTruthTables()
    {
        for(int i=0;i<spawnedCables.Count;i++)
        {
            if(spawnedCables[i] != null)
            {
                Debug.Log("  "+i +" "+ string.Join(", ", spawnedCables[i].getConnectedCable().GetTruthTable()));
                Debug.Log("  "+i +" "+ string.Join(", ", truthTable[i].truthTable));
            }
        }
    }


    public void Creat_A_Cable(int bitIndex)
    {
        if (spawnedCables[bitIndex] != null)
        {
            Debug.Log($"i have a cable on {bitIndex} button");
            CableManager existingCable = spawnedCables[bitIndex];
            
            if (!existingCable.CanConnect() )
            {
                // bitButtons[bitIndex].interactable = false;
                Debug.Log("it is full");
            }
            else
            {
                if(selectedCable != null)
                {
                    existingCable.ConnectCable(selectedCable);
                    
                    selectedCable.SetButton_cableManager( bitButtons[bitIndex]);
                    selectedCable.SetTargetConnector(existingCable);
                    index++;
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
            if (prefabToSpawnCable_Mangar != null)
            {
                
                GameObject newCable = Instantiate(prefabToSpawnCable_Mangar, transform.position, transform.rotation, transform.parent);
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
                        spawnedCables[bitIndex] = cableScript;
                        Debug.Log($"sameh {spawnedCables[bitIndex] == null} cable manager");
                        if (!cableScript.CanConnect() )
                        {
                            Debug.Log("it is full");
                        }
                        else
                        {
                            if(selectedCable != null)
                            {
                                cableScript.ConnectCable(selectedCable);
                                truthTable[bitIndex].truthTable = new List<bool>(selectedCable.truthTable);
                                selectedCable.SetButton_cableManager( bitButtons[bitIndex]);
                                selectedCable.SetTargetConnector(cableScript);
                                index++;
                            
                                printALLTruthTables();
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
        bitButtons[bitIndex].interactable = false;
        bitSelectionPanel.SetActive(false);
        iselected=false;
    }

    public void SetTruthTable(List<Cable16bitTruthTable> newTruthTable)
    {

        truthTable.Clear();
        foreach (var item in newTruthTable)
        {
            truthTable.Add(new Cable16bitTruthTable(new List<bool>(item.truthTable))); 
        }

    }

    public void OnBitSelectedAll()
    {
        Debug.Log("Selected bit: 16");
        if (spawnedCable16 != null)
        {
            Debug.Log($"i have a cable on all bits button");
            CableManager16bit existingCable = spawnedCable16;
            
            if (existingCable.CanConnect() )
            {
                myButton.interactable = false;
            }
            // Debug.Log("dragging");
        }
        else
        {
            if (prefabToSpawnCable16_Mangar != null)
            {
                GameObject newCable = Instantiate(prefabToSpawnCable16_Mangar, transform.position, transform.rotation, transform);
                Debug.Log("added");

                // Transform cable1Transform = newCable.transform.Find("cable1");
                if (newCable != null)
                {
                    CableManager16bit cableScript = newCable.GetComponent<CableManager16bit>();
                    if (cableScript != null)
                    {
                        spawnedCable16 = cableScript;
                        Debug.Log($"sameh {myButton == null}");
                        
                        if (cableScript.CanConnect() )
                        {
                            myButton.interactable = false;
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
        bitSelectionPanel.SetActive(false);
        
    }

    public void OnBitSelected2(int bitIndex)
    {
        // Debug.Log($"Selected bit: {bitIndex}");
        // Creat_A_Cable(2);
    }
    public void OnBitSelected(int bitIndex)
    {
        Creat_A_Cable(bitIndex);
    }

    // void OnMouseDown()
    // {
    //     ShowBitSelectionUI();
    // }

    void Update()
    {
        // if( iselected )
        // {
            
        //     bitSelectionPanel.SetActive(true);
        // }
        // if(!iselected )
        // {
            
        //     bitSelectionPanel.SetActive();
        // }
        // for (int i=0;i<index;i++)
        // {
        //     if(spawnedCables[i].CanConnect())
        //     {
        //         Debug.Log($"Selected loop : {i}");
        //         bitButtons[i].interactable =true;
        //     }
        // }
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     if (Physics.Raycast(ray, out RaycastHit hitInfo))
        //     {
        //         Canvas hitCanvas = hitInfo.collider.gameObject.GetComponentInParent<Canvas>();
        //         if (hitCanvas != null && hitCanvas == canvas)
        //         {
        //             ShowBitSelectionUI();
        //         }
        //     }
        // }
        if (bitSelectionPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0) && bitButtons[0] != null && bitButtons[0].interactable && iselected)
            {
                // bitButtons[0].onClick.Invoke();
                // Debug.Log("الكائن ظهر!00000");
                bitButtons[0].interactable = false;
                // bitSelectionPanel.SetActive(false);
                OnBitSelected(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1) && bitButtons[1] != null && bitButtons[1].interactable && iselected)
            {
                // bitButtons[1].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                OnBitSelected(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && bitButtons[2] != null && bitButtons[2].interactable && iselected)
            {
                // bitButtons[2].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                OnBitSelected(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && bitButtons[3] != null && bitButtons[3].interactable && iselected)
            {
                // bitButtons[3].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                OnBitSelected(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) && bitButtons[4] != null && bitButtons[4].interactable && iselected)
            {
                // bitButtons[4].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                OnBitSelected(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5) && bitButtons[5] != null && bitButtons[5].interactable && iselected)
            {
                // bitButtons[5].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                OnBitSelected(5);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6) && bitButtons[6] != null && bitButtons[6].interactable && iselected)
            {
                // bitButtons[6].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                OnBitSelected(6);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7) && bitButtons[7] != null && bitButtons[7].interactable && iselected)
            {
                // bitButtons[7].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                OnBitSelected(7);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8) && bitButtons[8] != null && bitButtons[8].interactable && iselected)
            {
                // bitButtons[8].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                OnBitSelected(8);

            }
            else if (Input.GetKeyDown(KeyCode.Alpha9) && bitButtons[9] != null && bitButtons[9].interactable && iselected)
            {
                // bitButtons[9].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                OnBitSelected(9);
            }
            // مفاتيح بديلة لـ 10 إلى 15 (لأن KeyCode.10 إلى KeyCode.15 مش موجودة)
            else if (Input.GetKeyDown(KeyCode.Z) && bitButtons[10] != null && bitButtons[10].interactable && iselected)
            {
                // bitButtons[10].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                OnBitSelected(10);
            }
            else if (Input.GetKeyDown(KeyCode.X) && bitButtons[11] != null && bitButtons[11].interactable && iselected)
            {
                // bitButtons[11].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                OnBitSelected(11);
            }
            else if (Input.GetKeyDown(KeyCode.C) && bitButtons[12] != null && bitButtons[12].interactable && iselected)
            {
                // bitButtons[12].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                OnBitSelected(12);
            }
            else if (Input.GetKeyDown(KeyCode.V) && bitButtons[13] != null && bitButtons[13].interactable && iselected)
            {
                // bitButtons[13].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                OnBitSelected(13);
            }
            else if (Input.GetKeyDown(KeyCode.B) && bitButtons[14] != null && bitButtons[14].interactable && iselected)
            {
                // bitButtons[14].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                OnBitSelected(14);
            }
            else if (Input.GetKeyDown(KeyCode.N) && bitButtons[15] != null && bitButtons[15].interactable && iselected)
            {
                // bitButtons[15].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                OnBitSelected(15);
            }
            
        }



    }

}