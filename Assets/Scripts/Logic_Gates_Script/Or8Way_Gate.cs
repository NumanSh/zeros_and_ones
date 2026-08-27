using System.Collections.Generic;
using UnityEngine;

public class Or8Way_Gate  : MonoBehaviour
{
   public int ID_Gate { get; private set; } 

    public void SetID(int id)
    {
        ID_Gate = id;
    }
    [SerializeField] private ButtonController_CableManager input; 
    public ButtonController1bit Out;
    public ButtonController1bit getCable()
    {
        return Out;
    }
    public ButtonController_CableManager getInput()
    {
        return input;
    }
    private bool isConnected = false;
    public bool Evaluate(bool value)
    {
        return !value;
    }
    void Update()
    {
        if (input != null  )
        {
            if((input.GetCable16Saved() != null || input.GetsizeCableManagers() ==input.getSizeTruthTable() )
             && isConnected)
            {
                UpdateTruthTable();
                isConnected =false;
            }
            if(input.GetCable16Saved() == null )
            {
                isConnected =true;
            }
            
            
        }
    }
    private List<bool> Evaluate(List<Cable16bitTruthTable> truthTable)
    {
        int size= truthTable[0].truthTable.Count;
        List<bool> newTruthTable = new List<bool>(size);
        for (int i = 0; i < size; i++)
        {
            bool value=false;
            for (int j=0;j< truthTable.Count ;j++)
            {
                value |= truthTable[j].truthTable[i];
            }
            newTruthTable.Add(value);
        }
        return newTruthTable;
    }

    private void UpdateTruthTable()
    {
        // if (isConnected)
        // {
            Out.SetTruthTable(Evaluate(input.GetTruthTable()));
            Debug.Log("Input A notGate: " + string.Join(", ", Out.GetTruthTable()));
            
        // }
        
    }
    public static List<Cable16bitTruthTable> TestIN()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, false, false}));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, false, false}));
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, false, true}));
        // Row 3
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, false, false}));
        // Row 4
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, false, false}));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, false, true}));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, false, true}));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false}));

        return result;
    }
    public static List<bool> TestOUT()
    {
        return new List<bool> { false, true, true, true ,true};

        
    }
}