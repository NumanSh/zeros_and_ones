using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class ButtonController16bit : MonoBehaviour
{
    public  ConnectorType Name;
    public GameObject bitSelectionPanel;
    public Vector3 panelOffset ;
    [SerializeField] public List<Cable16bitTruthTable> truthTable = new List<Cable16bitTruthTable>();
    [SerializeField] public Button All16bits;
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
            // holdButton.SetTruthTable(truthTable[0].truthTable);
            holdButton.SetPanelOffset(panelOffset);
            if (holdButton.type == CableTypes.c16)
            {
                holdButton.SetTruthTable16(truthTable);
            }
            else
            {
                if (truthTable.Count > 0)
                {
                    holdButton.SetTruthTable(truthTable[0].truthTable);
                }
            }
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
                    // holdButton1.SetTruthTable(truthTable[i].truthTable);
                    if (holdButton1.type == CableTypes.c16)
                    {
                        holdButton1.SetTruthTable16(truthTable);
                    }
                    else
                    {
                        if (truthTable.Count > 0)
                        {
                            holdButton1.SetTruthTable(truthTable[i].truthTable);
                        }
                    }
                    holdButton1.SetPanelOffset(panelOffset);
                    holdButton1.SetPrefabToSpawnCable(prefabToSpawnCable);
                }
                else
                {
                    Debug.LogError("HoldButton component not found on the first bit button.");
                }
                // bitButtons[i].SetActive(false);
            }
            else
            {
                Debug.Log($"the button in index {i} dose not exist");
            }
        }
        if (AddButton != null)
        {
            AddButton.onClick.AddListener(AddButtonListener);
        }
        // All16bits.onClick.AddListener(All16bitsListener);
    }

    void Update_VariablesButton()
    {
        for (int i = 0; i < bitButtons.Count; i++)
        {
            if (bitButtons[i] != null)
            {
                HoldButton holdButton1 = bitButtons[i].GetComponent<HoldButton>();
                if (holdButton1 != null)
                {
                    if (holdButton1.type == CableTypes.c16)
                    {
                        holdButton1.SetTruthTable16(truthTable);
                    }
                    else
                    {
                        if (truthTable.Count > 0)
                        {
                            holdButton1.SetTruthTable(truthTable[0].truthTable);
                        }
                    }
                }
                // bitButtons[i].GetComponent<HoldButton>().SetTruthTable(truthTable[i].truthTable);
            }
        }
        if (All16bits != null)
        {
            All16bits.GetComponent<HoldButton16>().SetTruthTable(truthTable);
        }
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
        Update_VariablesButton();
        
    }


   

    // public void All16bitsListener()
    // {
    //     Debug.Log("Selected all 16 bits button");
        
        
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

    

    void OnMouseDown()
    {
        // ShowBitSelectionUI();
        bitSelectionPanel.SetActive(true);
    }

   

}