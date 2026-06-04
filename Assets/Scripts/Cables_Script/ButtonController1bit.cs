using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class ButtonController1bit : MonoBehaviour
{
    public  ConnectorType Name;
    public GameObject bitSelectionPanel;
    public Vector3 panelOffset ;
    [SerializeField] public List<bool> truthTable = new List<bool>(){};
    [SerializeField] public Button AddButton;
    [SerializeField] private GameObject prefabToSpawnCable;
    private List<Cable> spawnedCables = new List<Cable>(new Cable[16]);
    [SerializeField] private List<GameObject> bitButtons = new List<GameObject>(){};
    private int index=0;
    // public HoldButton myHoldButton;
    
    void Start()
    {
        
        bitSelectionPanel.SetActive(false);
        HoldButton holdButton = bitButtons[0].GetComponent<HoldButton>();
        if (holdButton != null)         {
            holdButton.SetTruthTable(truthTable);
            holdButton.SetPanelOffset(panelOffset);
            holdButton.SetPrefabToSpawnCable(prefabToSpawnCable);
        }
        else
        {
            Debug.LogError("HoldButton component not found on the first bit button.");
        }
        for (int i=1;i<bitButtons.Count;i++)
        {
            if (bitButtons[i] != null)
            {
                HoldButton holdButton1 = bitButtons[i].GetComponent<HoldButton>();
                if (holdButton1 != null)         {
                    holdButton1.SetTruthTable(truthTable);
                    holdButton1.SetPanelOffset(panelOffset);
                    holdButton1.SetPrefabToSpawnCable(prefabToSpawnCable);
                }
                else
                {
                    Debug.LogError("HoldButton component not found on the first bit button.");
                }
                bitButtons[i].SetActive(false);
            }
            else
            {
                Debug.Log($"the button in index {i} dose not exist");
            }
        }
        AddButton.onClick.AddListener(AddButtonListener);
    }

    void Update_VariablesButton()
    {
        for (int i = 0; i < bitButtons.Count; i++)
        {
            if (bitButtons[i] != null)
            {
                bitButtons[i].GetComponent<HoldButton>().SetTruthTable(truthTable);
            }
        }
    }

    // void OnbitButtonClicked(int bitIndex)
    // {
    //     OnBitSelected(bitIndex);
    // }
    public List<bool> GetTruthTable()
    {
        return truthTable;
    }

    public void SetTruthTable(List<bool> newTruthTable)
    {
        
        if (truthTable.Count != newTruthTable.Count)
        {
            truthTable = new List<bool>(newTruthTable); 
            Update_VariablesButton();
        }
        else
        {
            for (int i = 0; i < newTruthTable.Count; i++)
            {
                truthTable[i] = newTruthTable[i];
            }
            Update_VariablesButton();
        }
    }


    // void ShowBitSelectionUI()
    // {
    //     bitSelectionPanel.SetActive(true);

    //     float targetX = transform.position.x + 2f; 
    //     float targetY = transform.position.y - 1f; 

    //     bitSelectionPanel.transform.position = new Vector3(targetX, targetY, 0f);

    //     Debug.Log("تم إظهار اللوحة بنجاح في الموقع الصحيح!");
    // }
    

    // public void Creat_A_Cable(int bitIndex)
    // {
    //     if (spawnedCables[bitIndex] != null)
    //     {
    //         // Debug.Log($"i have a cable on {bitIndex} button");
    //         Cable existingCable = spawnedCables[bitIndex];
    //         existingCable.SetDragging(true);
    //         existingCable.SetIsSelected(true);
    //         // Debug.Log("dragging");
    //     }
    //     else
    //     {
    //         if (prefabToSpawnCable != null)
    //         {
                
    //             GameObject newCable = Instantiate(prefabToSpawnCable, transform.position, transform.rotation, transform.parent);
    //             newCable.transform.localScale = transform.localScale;
    //             Transform cable1Transform = newCable.transform.Find("cable1");

    //             if (cable1Transform != null)
    //             {
    //                 Cable cableScript = cable1Transform.GetComponent<Cable>();
    //                 if (cableScript != null)
    //                 {
    //                     spawnedCables[bitIndex] = cableScript;
    //                     cableScript.SetTruthTable(truthTable);
    //                     // if (cableScript.getCableManager() != null )
    //                     // {
    //                     //     bitButtons[0].GetComponent<Button>().interactable = false;
    //                     // }
    //                     // Debug.Log($"sameh {spawnedCables[bitIndex] == null}");
    //                     cableScript.SetDragging(true);
    //                     cableScript.SetIsSelected(true);
    //                 }
    //                 else
    //                 {
    //                     Debug.LogError("i did not find a cable1");
    //                 }
    //             }
    //             else
    //             {
    //                 Debug.LogError("i cant find a script of cable");
    //             }
    //         }
    //     }
    //     bitSelectionPanel.SetActive(false);
    // }

    public void AddButtonListener()
    {
        // Debug.Log("Selected add button");
        if(index <10)
        {
            index++;
            bitButtons[index].SetActive(true);
        } 
        if(index == 10)
        {
            AddButton.interactable = false;
        }
        else
        {
        //    Debug.Log("you can not add button any more"); 
        } 
        
    }

    // public void OnBitSelected(int bitIndex)
    // {
    //     bitButtons[bitIndex].SetActive(false);
    //     Creat_A_Cable(bitIndex);
    // }

    void OnMouseDown()
    {
        // ShowBitSelectionUI();
        bitSelectionPanel.SetActive(true);
    }

    
    // void Update()
    // {
        
    //     if (bitSelectionPanel.activeSelf)
    //     {
    //         if (Input.GetKeyDown(KeyCode.Alpha1) && bitButtons[1] != null && bitButtons[1].GetComponent<Button>().interactable && bitButtons[1].activeSelf)
    //         {
    //             bitSelectionPanel.SetActive(false);
    //             OnBitSelected(1);
    //         }
    //         else if (Input.GetKeyDown(KeyCode.Alpha2) && bitButtons[2] != null && bitButtons[2].GetComponent<Button>().interactable && bitButtons[2].activeSelf)
    //         {
    //             bitSelectionPanel.SetActive(false);
    //             OnBitSelected(2);
    //         }
    //         else if (Input.GetKeyDown(KeyCode.Alpha3) && bitButtons[3] != null && bitButtons[3].GetComponent<Button>().interactable && bitButtons[3].activeSelf)
    //         {
    //             bitSelectionPanel.SetActive(false);
    //             OnBitSelected(3);
    //         }
    //         else if (Input.GetKeyDown(KeyCode.Alpha4) && bitButtons[4] != null && bitButtons[4].GetComponent<Button>().interactable && bitButtons[4].activeSelf)
    //         {
    //             bitSelectionPanel.SetActive(false);
    //             OnBitSelected(4);
    //         }
    //         else if (Input.GetKeyDown(KeyCode.Alpha5) && bitButtons[5] != null && bitButtons[5].GetComponent<Button>().interactable && bitButtons[5].activeSelf)
    //         {
    //             bitSelectionPanel.SetActive(false);
    //             OnBitSelected(5);
    //         }
    //         else if (Input.GetKeyDown(KeyCode.Alpha6) && bitButtons[6] != null && bitButtons[6].GetComponent<Button>().interactable && bitButtons[6].activeSelf)
    //         {
    //             bitSelectionPanel.SetActive(false);
    //             OnBitSelected(6);
    //         }
    //         else if (Input.GetKeyDown(KeyCode.Alpha7) && bitButtons[7] != null && bitButtons[7].GetComponent<Button>().interactable && bitButtons[7].activeSelf)
    //         {
    //             bitSelectionPanel.SetActive(false);
    //             OnBitSelected(7);
    //         }
    //         else if (Input.GetKeyDown(KeyCode.Alpha8) && bitButtons[8] != null && bitButtons[8].GetComponent<Button>().interactable && bitButtons[8].activeSelf)
    //         {
    //             bitSelectionPanel.SetActive(false);
    //             OnBitSelected(8);

    //         }
    //         else if (Input.GetKeyDown(KeyCode.Alpha9) && bitButtons[9] != null && bitButtons[9].GetComponent<Button>().interactable && bitButtons[9].activeSelf)
    //         {
    //             bitSelectionPanel.SetActive(false);
    //             OnBitSelected(9);
    //         }
            
    //         else if (Input.GetKeyDown(KeyCode.F) && AddButton != null && AddButton.interactable)
    //         {
    //             AddButton.onClick.Invoke();
    //         }
    //     }



    // }

}