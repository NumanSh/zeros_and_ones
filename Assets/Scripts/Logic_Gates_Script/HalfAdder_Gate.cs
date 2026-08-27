using System.Collections.Generic;
using UnityEngine;


public class HalfAdder_Gate : MonoBehaviour
{
    public int ID_Gate { get; private set; } 

    public void SetID(int id)
    {
        ID_Gate = id;
    }
    [SerializeField] private CableManager inputA; 
    [SerializeField] private CableManager inputB; 
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
    void Update()
    {
        if(inputA != null && inputB != null)
        {
            if(inputA.getConnectedCable() == null)
            {
                isConnected=true;
            }
            if(inputB.getConnectedCable() == null)
            {
                isConnected=true;
            }
        
            if (inputA.getConnectedCable() != null && inputB.getConnectedCable() != null && isConnected)
            {
                print("Both inputs are now connected!");
                isConnected=false;
                UpdateTruthTable();
            }
        }
    }
    private List<bool> EvaluateSum(List<bool> truthTableA,List<bool> truthTableB)
    {
        List<bool> newTruthTable = new List<bool>(truthTableA);
        for (int i = 0; i < truthTableA.Count; i++)
        {
            newTruthTable[i] = ((truthTableA[i] && !truthTableB[i]) || (!truthTableA[i] && truthTableB[i]));
        }
        return newTruthTable;
    }
    private List<bool> EvaluateCarry(List<bool> truthTableA,List<bool> truthTableB)
    {
        List<bool> newTruthTable = new List<bool>(truthTableA);
        for (int i = 0; i < truthTableA.Count; i++)
        {
            newTruthTable[i] = (truthTableA[i] && truthTableB[i]);
        }
        return newTruthTable;
    }
    private void UpdateTruthTable()
    {
        sum.SetTruthTable(EvaluateSum(inputA.getConnectedCable().GetTruthTable(),inputB.getConnectedCable().GetTruthTable()));
        Debug.Log("Input A sum: " + string.Join(", ", sum.GetTruthTable()));
        carry.SetTruthTable(EvaluateCarry(inputA.getConnectedCable().GetTruthTable(),inputB.getConnectedCable().GetTruthTable()));
        Debug.Log("Input A carry: " + string.Join(", ", carry.GetTruthTable()));
        
        
    }

    public static List<Cable16bitTruthTable> TestA()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,  true,  true }));

        return result;
    }

    public static List<Cable16bitTruthTable> TestB()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true }));

        return result;
    }

    public static List<bool> TestSum()
    {
        return new List<bool> { false, true, true, false };
    }

    public static List<bool> TestCarry()
    {
        return new List<bool> { false, false, false, true };
    }

}