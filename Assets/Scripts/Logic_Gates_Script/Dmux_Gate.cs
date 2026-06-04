using UnityEngine;
using System.Collections.Generic;


public class Dmux_Gate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int ID_Gate { get; private set; } 
   
    public void SetID(int id)
    {
        ID_Gate = id;
    }
    [SerializeField] private CableManager inputIn; 

    [SerializeField] private CableManager inputSel; 
    public ButtonController1bit OutB;
    public ButtonController1bit OutA;
    private bool isConnected =false;
    
    public ButtonController1bit getCableA()
    {
        return OutA;
    }
    public ButtonController1bit getCableB()
    {
        return OutB;
    }
    public CableManager getInputA()
    {
        return inputIn;
    }
    public CableManager getInputB()
    {
        return inputSel;
    }
    void Update()
    {
        if(inputIn != null && inputSel != null )
        {
            if(inputIn.getConnectedCable() == null)
            {
                isConnected=true;
            }
            if(inputSel.getConnectedCable() == null)
            {
                isConnected=true;
            }
        
            if (inputIn.getConnectedCable() != null  && inputSel.getConnectedCable() != null && isConnected)
            {
                print("Both inputs are now connected!");
                isConnected=false;
                UpdateTruthTable();
            }
        }
    }
    private List<bool> EvaluateB(List<bool> truthTableIn,List<bool> truthTableSel)
    {
        List<bool> newTruthTable = new List<bool>(truthTableIn);
        for (int i = 0; i < truthTableIn.Count; i++)
        {
            if( truthTableSel[i])
            {
                newTruthTable[i] = truthTableIn[i] ;
            }
            else
            {
                newTruthTable[i] = false ;
            }
        }
        return newTruthTable;
    }
    private List<bool> EvaluateA(List<bool> truthTableIn,List<bool> truthTableSel)
    {
        List<bool> newTruthTable = new List<bool>(truthTableIn);
        for (int i = 0; i < truthTableIn.Count; i++)
        {
            if( !truthTableSel[i])
            {
                newTruthTable[i] = truthTableIn[i] ;
            }
            else
            {
                newTruthTable[i] = false ;
            }
        }
        return newTruthTable;
    }
    private void UpdateTruthTable()
    {
        OutA.SetTruthTable(EvaluateA(inputIn.getConnectedCable().GetTruthTable(),inputSel.getConnectedCable().GetTruthTable()));
        Debug.Log("OUT A MuxGate: " + string.Join(", ", OutA.GetTruthTable()));
        OutB.SetTruthTable(EvaluateB(inputIn.getConnectedCable().GetTruthTable(),inputSel.getConnectedCable().GetTruthTable()));
        Debug.Log("OUT B MuxGate: " + string.Join(", ", OutA.GetTruthTable()));
        
        
    }
    public static List<Cable16bitTruthTable> TestIN_IN()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false ,true,true}));

        return result;
    }
    public static List<Cable16bitTruthTable> TestIN_SEL()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,false,true}));

        return result;
    }
    public static List<Cable16bitTruthTable> TestOut_A()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,true,false}));

        return result;
    }
    public static List<Cable16bitTruthTable> TestOut_B()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,false,true}));

        return result;
    }
  
}