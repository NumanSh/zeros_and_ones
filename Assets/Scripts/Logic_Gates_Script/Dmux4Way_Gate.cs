using UnityEngine;
using System.Collections.Generic;


public class Dmux4Way_Gate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int ID_Gate { get; private set; } 
   
    public void SetID(int id)
    {
        ID_Gate = id;
    }
    [SerializeField] private CableManager inputIn; 

    [SerializeField] private ButtonController_CableManager inputSel; 
    public ButtonController1bit OutA;
    public ButtonController1bit OutB;
    public ButtonController1bit OutC;
    public ButtonController1bit OutD;
    private bool isConnected =false;
    
    public ButtonController1bit getCableA()
    {
        return OutA;
    }
    public ButtonController1bit getCableB()
    {
        return OutB;
    }
    public ButtonController1bit getCableC()
    {
        return OutC;
    }
    public ButtonController1bit getCableD()
    {
        return OutD;
    }
    public CableManager getInputIn()
    {
        return inputIn;
    }
    public ButtonController_CableManager getInputSel()
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
            if(inputSel.GetCable16Saved() == null)
            {
                isConnected=true;
            }
        
            if (inputIn.getConnectedCable() != null   && isConnected
            && (inputSel.GetCable16Saved() != null || inputSel.GetsizeCableManagers() ==inputSel.getSizeTruthTable() ))
            {
                print("Both inputs are now connected!");
                isConnected=false;
                UpdateTruthTable();
            }
        }
    }
    private List<bool> EvaluateA(List<bool> truthTableIn,List<Cable16bitTruthTable> truthTableSel)
    {
        List<bool> newTruthTable = new List<bool>(truthTableIn);
        for (int i = 0; i < truthTableIn.Count; i++)
        {
            if(!truthTableSel[0].truthTable[i] && !truthTableSel[1].truthTable[i])
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
    private List<bool> EvaluateB(List<bool> truthTableIn,List<Cable16bitTruthTable> truthTableSel)
    {
        List<bool> newTruthTable = new List<bool>(truthTableIn);
        for (int i = 0; i < truthTableIn.Count; i++)
        {
            if(!truthTableSel[0].truthTable[i] && truthTableSel[1].truthTable[i])
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
    private List<bool> EvaluateC(List<bool> truthTableIn,List<Cable16bitTruthTable> truthTableSel)
    {
        List<bool> newTruthTable = new List<bool>(truthTableIn);
        for (int i = 0; i < truthTableIn.Count; i++)
        {
            if(truthTableSel[0].truthTable[i] && !truthTableSel[1].truthTable[i])
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
    private List<bool> EvaluateD(List<bool> truthTableIn,List<Cable16bitTruthTable> truthTableSel)
    {
        List<bool> newTruthTable = new List<bool>(truthTableIn);
        for (int i = 0; i < truthTableIn.Count; i++)
        {
            if(truthTableSel[0].truthTable[i] && truthTableSel[1].truthTable[i])
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
        
        OutA.SetTruthTable(EvaluateA(inputIn.getConnectedCable().GetTruthTable(),inputSel.GetTruthTable()));
        Debug.Log("OUT a MuxGate: " + string.Join(", ", OutA.GetTruthTable()));
        OutB.SetTruthTable(EvaluateB(inputIn.getConnectedCable().GetTruthTable(),inputSel.GetTruthTable()));
        Debug.Log("OUT B MuxGate: " + string.Join(", ", OutB.GetTruthTable()));
        OutC.SetTruthTable(EvaluateC(inputIn.getConnectedCable().GetTruthTable(),inputSel.GetTruthTable()));
        Debug.Log("OUT C MuxGate: " + string.Join(", ", OutC.GetTruthTable()));
        OutD.SetTruthTable(EvaluateD(inputIn.getConnectedCable().GetTruthTable(),inputSel.GetTruthTable()));
        Debug.Log("OUT D MuxGate: " + string.Join(", ", OutD.GetTruthTable()));
        
        
    }
  
    public static List<Cable16bitTruthTable> TestIN()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();

        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, true, true, true, true }));

        return result;
    }

    public static List<Cable16bitTruthTable> TestIN_SEL()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, true, false, false, true, true }));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false, true, false, true }));

        return result;
    }

    public static List<bool> TestA()
    {
        return new List<bool> { false, false, false, false, true, false, false, false };
    }

    public static List<bool> TestB()
    {
        return new List<bool> { false, false, false, false, false, true, false, false };
    }

    public static List<bool> TestC()
    {
        return new List<bool> { false, false, false, false, false, false, true, false };
    }

    public static List<bool> TestD()
    {
        return new List<bool> { false, false, false, false, false, false, false, true };
    }









}