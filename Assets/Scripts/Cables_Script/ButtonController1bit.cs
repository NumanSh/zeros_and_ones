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
            // holdButton.SetTruthTable(truthTable);
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
                    // holdButton1.SetTruthTable(truthTable);
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