using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class ButtonController_CableManager : MonoBehaviour
{
    public  ConnectorType Name;
    public GameObject bitSelectionPanel;
    public Vector3 panelOffset ;
    [SerializeField] public List<Cable16bitTruthTable> truthTable ;
    [SerializeField] public Button myButton;
    // [SerializeField] public Button myButton1;
    // [SerializeField] public Button myButton2;
    [SerializeField] private GameObject prefabToSpawnCable_Mangar;
    [SerializeField] private GameObject prefabToSpawnCable16_Mangar;
    
    [SerializeField] private List<Button> bitButtons = new List<Button>(){};
    private CableManager16bit spawnedCable16 = null;
    public bool iselected=false;
    private Cable selectedCable= null;
    private Cable16bit selectedCable16=null;

    private Cable16bit Cable16Saved=null;
    public List<CableManager> spawnedCableManagers;
    private int index=0;
    public int sizeCableManagers=16;

    public int getSizeTruthTable()
    {
        return index;
    }

    public int GetsizeCableManagers()
    {
        return sizeCableManagers;
    }

    public CableManager16bit GetspawnedCable16()
    {
        return spawnedCable16;
    }
    public List<CableManager> GetSpawnedCable_managers()
    {
        return spawnedCableManagers;
    }

    public Cable GetSelectedCable()
    {
        return selectedCable;
    }
    public Cable16bit GetSelectedCable16()
    {
        return selectedCable16;
    }
    public Cable16bit GetCable16Saved()
    {
        return Cable16Saved;
    }

    public void SetCable16Saved(Cable16bit cable)
    {
        Cable16Saved=cable;
    }
    public void SetSelectedCable(Cable cable)
    {
        selectedCable=cable;
    }
    public void SetSelectedCable16(Cable16bit cable)
    {
        selectedCable16=cable;
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
    
    void Start()
    {
        bitSelectionPanel.SetActive(false);
        truthTable = new List<Cable16bitTruthTable>(new Cable16bitTruthTable[sizeCableManagers]);
        spawnedCableManagers = new List<CableManager>(new CableManager[sizeCableManagers]);
        for (int i=0;i<bitButtons.Count;i++)
        {
            if (bitButtons[i] != null)
            {
                int tempIndex = i;
                HoldButtonCableManager holdButton1 = bitButtons[i].GetComponent<HoldButtonCableManager>();
                if (holdButton1 != null)
                {
                    holdButton1.SetBitIndex(tempIndex);
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
    }

    void Update()
    {
        int x=0;
        for (int i =0;i<spawnedCableManagers.Count;i++)
        {
            if (spawnedCableManagers[i] != null)
            {
                x++;
                if (i < truthTable.Count)
                {
                    if (truthTable[i] != null)
                    {
                        truthTable[i].truthTable = spawnedCableManagers[i].getConnectedCable().GetTruthTable();
                        
                    }
                    // truthTable[i].truthTable = spawnedCableManagers[i].getConnectedCable().GetTruthTable();
                }
                else
                {
                    Debug.LogWarning("this index is out of range truth table " + i);
                }
                // truthTable[i].truthTable=spawnedCableManagers[i].getConnectedCable().GetTruthTable();
            }
        }
        if (x ==sizeCableManagers )
        {
            index = x;
        }
        if (index == sizeCableManagers)
        {
            return;;
        }
    }

    public void ShowBitSelectionUI()
    {
        bitSelectionPanel.SetActive(true);
        
    }
    public void CloseBitSelectionUI()
    {
        
        bitSelectionPanel.SetActive(false);
        
    }
    public void printALLTruthTables()
    {
        for(int i=0;i<spawnedCableManagers.Count;i++)
        {
            if(spawnedCableManagers[i] != null)
            {
                Debug.Log("  "+i +" "+ string.Join(", ", spawnedCableManagers[i].getConnectedCable().GetTruthTable()));
                Debug.Log("  "+i +" "+ string.Join(", ", truthTable[i].truthTable));
            }
        }
    }


 

    public void SetTruthTable(List<Cable16bitTruthTable> newTruthTable)
    {

        truthTable.Clear();
        foreach (var item in newTruthTable)
        {
            truthTable.Add(new Cable16bitTruthTable(new List<bool>(item.truthTable))); 
        }

    }


}