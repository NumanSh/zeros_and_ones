using System.Collections.Generic;
using UnityEngine;


public class FullAdder_Gate : MonoBehaviour
{
    public int ID_Gate { get; private set; } 

    public void SetID(int id)
    {
        ID_Gate = id;
    }
    [SerializeField] private CableManager inputA; 
    [SerializeField] private CableManager inputB; 
    [SerializeField] private CableManager inputC; 
    public ButtonController1bit sum;
    public ButtonController1bit carry;
    private bool isConnected =false;
    
    public ButtonController1bit getCarry()
    {
        return carry;
    }
    public ButtonController1bit getSum()
    {
        return sum;
    }
    public CableManager getInputA()
    {
        return inputA;
    }
    public CableManager getInputB()
    {
        return inputB;
    }
    public CableManager getInputC()
    {
        return inputC;
    }
    void Update()
    {
        if(inputA != null && inputB != null && inputC != null)
        {
            if(inputA.getConnectedCable() == null)
            {
                isConnected=true;
            }
            if(inputB.getConnectedCable() == null)
            {
                isConnected=true;
            }
            if(inputC.getConnectedCable() == null)
            {
                isConnected=true;
            }
        
            if (inputA.getConnectedCable() != null && inputB.getConnectedCable() != null && inputC.getConnectedCable() != null && isConnected)
            {
                print("Both inputs are now connected!");
                isConnected=false;
                UpdateTruthTable();
            }
        }
    }
    private List<bool> EvaluateCarry(List<bool> truthTableA,List<bool> truthTableB,List<bool> truthTableC)
    {
        List<bool> newTruthTable = new List<bool>(truthTableA);
        for (int i = 0; i < truthTableA.Count; i++)
        {
            bool andR =(truthTableB[i] && truthTableC[i]);
            bool xorR =(truthTableB[i] ^ truthTableC[i]);
            bool andr2= (truthTableA[i] && xorR);
            newTruthTable[i] = ( andr2 || andR );
        }
        return newTruthTable;
    }
    private List<bool> EvaluateSum(List<bool> truthTableA,List<bool> truthTableB,List<bool> truthTableC)
    {
        List<bool> newTruthTable = new List<bool>(truthTableA);
        for (int i = 0; i < truthTableA.Count; i++)
        {
            bool xorR=(truthTableA[i] ^ truthTableC[i]);
            newTruthTable[i] = (xorR ^ truthTableB[i]);
        }
        return newTruthTable;
    }
    private void UpdateTruthTable()
    {
        sum.SetTruthTable(EvaluateSum(inputA.getConnectedCable().GetTruthTable(),inputB.getConnectedCable().GetTruthTable(),inputC.getConnectedCable().GetTruthTable()));
        Debug.Log("Input sum: " + string.Join(", ", sum.GetTruthTable()));
        carry.SetTruthTable(EvaluateCarry(inputA.getConnectedCable().GetTruthTable(),inputB.getConnectedCable().GetTruthTable(),inputC.getConnectedCable().GetTruthTable()));
        Debug.Log("Input carry: " + string.Join(", ", carry.GetTruthTable()));
        
        
    }

    public static List<Cable16bitTruthTable> TestA()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,false, false,  true,  true,  true,  true }));

        return result;
    }

    public static List<Cable16bitTruthTable> TestB()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, true,false, false, true, true }));

        return result;
    }
    public static List<Cable16bitTruthTable> TestC()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true,false, true, false, true }));

        return result;
    }

    public static List<bool> TestSum()
    {
        return new List<bool> { false, true, true, false,true, false, false, true };
    }

    public static List<bool> TestCarry()
    {
        return new List<bool> { false, false, false, true ,false, true, true, true };
    }

}