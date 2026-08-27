using UnityEngine;
using System.Collections.Generic;
public class Mux_Gate : MonoBehaviour
{
    public int ID_Gate { get; private set; } 

    public void SetID(int id)
    {
        ID_Gate = id;
    }
    [SerializeField] private CableManager inputA; 
    [SerializeField] private CableManager inputB; 
    [SerializeField] private CableManager inputSel; 
    public ButtonController1bit Out;
    private bool isConnected =false;
    
    public ButtonController1bit getCable()
    {
        return Out;
    }
    public CableManager getInputA()
    {
        return inputA;
    }
    public CableManager getInputB()
    {
        return inputB;
    }
    void Update()
    {
        if(inputA != null && inputB != null && inputSel!= null)
        {
            if(inputA.getConnectedCable() == null)
            {
                isConnected=true;
            }
            if(inputB.getConnectedCable() == null)
            {
                isConnected=true;
            }
            if(inputSel.getConnectedCable() == null)
            {
                isConnected=true;
            }
        
            if (inputA.getConnectedCable() != null && inputB.getConnectedCable() && inputSel.getConnectedCable() != null && isConnected)
            {
                print("Both inputs are now connected!");
                isConnected=false;
                UpdateTruthTable();
            }
        }
    }
    private List<bool> Evaluate(List<bool> truthTableA,List<bool> truthTableB,List<bool> truthTableSel)
    {
        List<bool> newTruthTable = new List<bool>(truthTableA);
        for (int i = 0; i < truthTableA.Count; i++)
        {
            newTruthTable[i] = ((truthTableB[i] && truthTableSel[i]) || (truthTableA[i] && !truthTableSel[i]));
        }
        return newTruthTable;
    }
    private void UpdateTruthTable()
    {
        Out.SetTruthTable(Evaluate(inputA.getConnectedCable().GetTruthTable(),inputB.getConnectedCable().GetTruthTable(),inputSel.getConnectedCable().GetTruthTable()));
        Debug.Log("Input A MuxGate: " + string.Join(", ", Out.GetTruthTable()));
        
        
    }
    public static List<Cable16bitTruthTable> TestIN_A()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false ,false,false ,true, true ,true,true}));

        return result;
    }
    public static List<Cable16bitTruthTable> TestIN_B()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false ,true,true ,false, false ,true,true}));

        return result;
    }
    public static List<Cable16bitTruthTable> TestIN_SEL()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true ,false,true ,false, true ,false,true}));

        return result;
    }
    public static List<Cable16bitTruthTable> TestOut()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false ,false,true ,true, false ,true,true}));

        return result;
    }

}