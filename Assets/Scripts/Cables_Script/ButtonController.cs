using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class ButtonController : MonoBehaviour
{
    public  ConnectorType Name;
    public GameObject bitSelectionPanel;
    public Vector3 panelOffset = new Vector3(100f, 0f, 0f);
    [SerializeField] public List<Cable16bitTruthTable> truthTable = new List<Cable16bitTruthTable>(){};
    [SerializeField] public Button myButton;
    // [SerializeField] public Button myButton1;
    // [SerializeField] public Button myButton2;
    [SerializeField] private GameObject prefabToSpawnCable;
    [SerializeField] private GameObject prefabToSpawnCable16;
    private List<Cable> spawnedCables = new List<Cable>(new Cable[16]);
    [SerializeField] private List<Button> bitButtons = new List<Button>(){};
    private Cable16bit spawnedCable16 = null;
    
    
    void Start()
    {
        // bitSelectionPanel.SetActive(false);
        for (int i=0;i<bitButtons.Count;i++)
        {
            if (bitButtons[i] != null)
            {
                bitButtons[i].onClick.AddListener(() => OnBitSelected2(i));
            }
            else
            {
                Debug.Log($"the button in index {i} dose not exist");
            }
        }
        myButton.onClick.AddListener(OnBitSelectedAll);
    }

    public void ShowBitSelectionUI()
    {
        Debug.Log($"sameh16 hon bas ");
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        bitSelectionPanel.SetActive(true);
        Debug.Log($"New cable active: {bitSelectionPanel.activeSelf}");
        bitSelectionPanel.transform.position = screenPos + panelOffset;
    }

    public void Creat_A_Cable(int bitIndex)
    {
        if (spawnedCables[bitIndex] != null)
        {
            Debug.Log($"i have a cable on {bitIndex} button");
            Cable existingCable = spawnedCables[bitIndex];
            existingCable.SetDragging(true);
            existingCable.SetIsSelected(true);
            // if (existingCable.getCableManager() != null)
            // {
            //     bitButtons[bitIndex].interactable = false;
            // }
            // Debug.Log("dragging");
        }
        else
        {
            if (prefabToSpawnCable != null)
            {
                
                GameObject newCable = Instantiate(prefabToSpawnCable, transform.position, transform.rotation, transform.parent);
                // Debug.Log(newCable.transform.position);
                // Debug.Log("EndA Pos: " + transform.position);
                newCable.transform.localScale = transform.localScale;
                Transform cable1Transform = newCable.transform.Find("cable1");
                
                // Debug.Log("added");

                if (cable1Transform != null)
                {
                    Cable cableScript = cable1Transform.GetComponent<Cable>();
                    if (cableScript != null)
                    {
                        spawnedCables[bitIndex] = cableScript;
                        // cableScript.SetTruthTable(truthTable[bitIndex].truthTable);
                        // Debug.Log($"sameh {spawnedCables[bitIndex] == null}");
                        spawnedCables[bitIndex].SetTruthTable(truthTable[bitIndex].truthTable);
                        Debug.Log("  "+bitIndex +" "+ string.Join(", ", truthTable[bitIndex].truthTable));
                        cableScript.SetDragging(true);
                        cableScript.SetIsSelected(true);
                        // if (cableScript.getCableManager() != null )
                        // {
                        //     bitButtons[bitIndex].interactable = false;
                        //     cableScript.SetButton_cable( bitButtons[bitIndex]);
                        // }
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

    public void SetTruthTable(List<Cable16bitTruthTable> newTruthTable)
    {

        truthTable.Clear();
        foreach (var item in newTruthTable)
        {
            truthTable.Add(new Cable16bitTruthTable(new List<bool>(item.truthTable))); 
        }

    }

    public List<Cable16bitTruthTable> GetTruthTable()
    {
        return truthTable;
    }

    public void OnBitSelectedAll()
    {
        Debug.Log("Selected bit: 16");
        if (spawnedCable16 != null)
        {
            Debug.Log($"i have a cable on all bits button");
            Cable16bit existingCable = spawnedCable16;
            existingCable.SetDragging(true);
            existingCable.SetIsSelected(true);
            if (existingCable.getCableManager() != null)
            {
                myButton.interactable = false;
            }
            // Debug.Log("dragging");
        }
        else
        {
            if (prefabToSpawnCable16 != null)
            {
                GameObject newCable = Instantiate(prefabToSpawnCable16, transform.position, transform.rotation, transform);
                Debug.Log("added");

                Transform cable1Transform = newCable.transform.Find("cable1");
                if (cable1Transform != null)
                {
                    Cable16bit cableScript = cable1Transform.GetComponent<Cable16bit>();
                    if (cableScript != null)
                    {
                        spawnedCable16 = cableScript;
                        Debug.Log($"sameh {myButton == null}");
                        cableScript.SetDragging(true);
                        cableScript.SetTruthTable(truthTable);
                        cableScript.SetIsSelected(true);
                        if (cableScript.getCableManager() != null )
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
        if( bitIndex >=0 && bitIndex <=15)
        {
            Creat_A_Cable(bitIndex);
        }
    }

    void OnMouseDown()
    {
        ShowBitSelectionUI();
    }

    void Update()
    {
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
            if (Input.GetKeyDown(KeyCode.Alpha0) && bitButtons[0] != null && bitButtons[0].interactable )
            {
                // bitButtons[0].onClick.Invoke();
                // Debug.Log("الكائن ظهر! 0");
                bitSelectionPanel.SetActive(false);
                bitButtons[0].interactable=false;
                OnBitSelected(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1) && bitButtons[1] != null && bitButtons[1].interactable)
            {
                // bitButtons[1].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                bitButtons[1].interactable=false;
                OnBitSelected(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && bitButtons[2] != null && bitButtons[2].interactable)
            {
                // bitButtons[2].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                bitButtons[2].interactable=false;
                OnBitSelected(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && bitButtons[3] != null && bitButtons[3].interactable)
            {
                // bitButtons[3].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                bitButtons[3].interactable=false;
                OnBitSelected(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) && bitButtons[4] != null && bitButtons[4].interactable)
            {
                // bitButtons[4].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                bitButtons[4].interactable=false;
                OnBitSelected(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5) && bitButtons[5] != null && bitButtons[5].interactable)
            {
                // bitButtons[5].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                bitButtons[5].interactable=false;
                OnBitSelected(5);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6) && bitButtons[6] != null && bitButtons[6].interactable)
            {
                // bitButtons[6].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                bitButtons[6].interactable=false;
                OnBitSelected(6);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7) && bitButtons[7] != null && bitButtons[7].interactable)
            {
                // bitButtons[7].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                bitButtons[7].interactable=false;
                OnBitSelected(7);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8) && bitButtons[8] != null && bitButtons[8].interactable)
            {
                // bitButtons[8].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                bitButtons[8].interactable=false;
                OnBitSelected(8);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9) && bitButtons[9] != null && bitButtons[9].interactable)
            {
                // bitButtons[9].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                bitButtons[9].interactable=false;
                OnBitSelected(9);
            }
            // مفاتيح بديلة لـ 10 إلى 15 (لأن KeyCode.10 إلى KeyCode.15 مش موجودة)
            else if (Input.GetKeyDown(KeyCode.Z) && bitButtons[10] != null && bitButtons[10].interactable)
            {
                // bitButtons[10].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                bitButtons[10].interactable=false;
                OnBitSelected(10);
            }
            else if (Input.GetKeyDown(KeyCode.X) && bitButtons[11] != null && bitButtons[11].interactable)
            {
                // bitButtons[11].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                bitButtons[11].interactable=false;
                OnBitSelected(11);
            }
            else if (Input.GetKeyDown(KeyCode.C) && bitButtons[12] != null && bitButtons[12].interactable)
            {
                // bitButtons[12].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                bitButtons[12].interactable=false;
                OnBitSelected(12);
            }
            else if (Input.GetKeyDown(KeyCode.V) && bitButtons[13] != null && bitButtons[13].interactable)
            {
                // bitButtons[13].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                bitButtons[13].interactable=false;
                OnBitSelected(13);
            }
            else if (Input.GetKeyDown(KeyCode.B) && bitButtons[14] != null && bitButtons[14].interactable)
            {
                // bitButtons[14].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                bitButtons[14].interactable=false;
                OnBitSelected(14);
            }
            else if (Input.GetKeyDown(KeyCode.N) && bitButtons[15] != null && bitButtons[15].interactable)
            {
                // bitButtons[15].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                bitButtons[15].interactable=false;
                OnBitSelected(15);
            }
            
        }



    }

}