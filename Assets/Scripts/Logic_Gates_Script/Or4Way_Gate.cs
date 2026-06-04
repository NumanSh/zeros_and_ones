using System.Collections.Generic;
using UnityEngine;

public class Or4Way_Gate  : MonoBehaviour
{
   public int ID_Gate { get; private set; } 

    public void SetID(int id)
    {
        ID_Gate = id;
    }
    [SerializeField] private CableManager16bit input; 
    public ButtonController1bit Out;
    public ButtonController1bit getCable()
    {
        return Out;
    }
    public CableManager16bit getInput()
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
            if(input.getConnectedCable() != null && isConnected)
            {
                UpdateTruthTable();
                isConnected =false;
            }
            if(input.getConnectedCable() == null )
            {
                isConnected =true;
            }
            
            
        }
    }
    private List<Cable16bitTruthTable> Evaluate(List<Cable16bitTruthTable> truthTable)
    {
        List<Cable16bitTruthTable> newTruthTable = new List<Cable16bitTruthTable>(truthTable);
        for (int i = 0; i < truthTable.Count; i++)
        {
            for (int j=0;j< truthTable[i].truthTable.Count ;j++)
            {
                newTruthTable[i].truthTable[j] |= truthTable[i].truthTable[j];
            }
        }
        return newTruthTable;
    }

    private void UpdateTruthTable()
    {
        // if (isConnected)
        // {
            // Out.SetTruthTable(Evaluate(input.getConnectedCable().GetTruthTable()));
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
    public static List<Cable16bitTruthTable> TestOUT()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, true, true}));

        return result;
    }
}