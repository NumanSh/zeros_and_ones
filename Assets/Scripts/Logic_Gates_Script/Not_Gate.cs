using System.Collections.Generic;
using UnityEngine;

public class Not_Gate  : MonoBehaviour
{
   public int ID_Gate { get; private set; } 

    [SerializeField] private CableManager input; 
    public ButtonController1bit Out;
    private bool isConnected = false;

    public void SetID(int id)
    {
        ID_Gate = id;
    }

    public ButtonController1bit getCable()
    {
        return Out;
    }
    public CableManager getInput()
    {
        return input;
    }
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
    private List<bool> Evaluate(List<bool> truthTable)
    {
        List<bool> newTruthTable = new List<bool>(truthTable);
        for (int i = 0; i < truthTable.Count; i++)
        {
            newTruthTable[i] = !truthTable[i];
        }
        return newTruthTable;
    }
    private void UpdateTruthTable()
    {
        // if (isConnected)
        // {
            Out.SetTruthTable(Evaluate(input.getConnectedCable().GetTruthTable()));
            Debug.Log("Input A notGate: " + string.Join(", ", Out.GetTruthTable()));
            
        // }
        
    }
    public static List<Cable16bitTruthTable> TestIN()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true}));

        return result;
    }
    public static List<Cable16bitTruthTable> TestOut()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false}));

        return result;
    }
}