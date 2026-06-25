using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class ButtonController_CableManager : MonoBehaviour
{
    public  ConnectorType Name;
    public GameObject bitSelectionPanel;
    public Vector3 panelOffset ;
    [SerializeField] private List<Cable16bitTruthTable> truthTable ;
    [SerializeField] public Button myButton;
    // [SerializeField] public Button myButton1;
    // [SerializeField] public Button myButton2;
    [SerializeField] private GameObject prefabToSpawnCable_Mangar;
    [SerializeField] private GameObject prefabToSpawnCable16_Mangar;
    private List<CableManager> spawnedCables ;
    [SerializeField] private List<Button> bitButtons = new List<Button>(){};
    private CableManager16bit spawnedCable16 = null;
    public bool iselected=false;
    private Cable selectedCable= null;
    private Cable16bit selectedCable16=null;

    private Cable16bit Cable16Saved=null;
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
        return spawnedCables;
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
        return selectedCable16;
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
        spawnedCables = new List<CableManager>(new CableManager[sizeCableManagers]);
        for (int i=0;i<bitButtons.Count;i++)
        {
            if (bitButtons[i] != null)
            {
                HoldButtonCableManager holdButton1 = bitButtons[i].GetComponent<HoldButtonCableManager>();
                if (holdButton1 != null)
                {
                    holdButton1.SetBitIndex(i);
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
        for(int i=0;i<spawnedCables.Count;i++)
        {
            if(spawnedCables[i] != null)
            {
                Debug.Log("  "+i +" "+ string.Join(", ", spawnedCables[i].getConnectedCable().GetTruthTable()));
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