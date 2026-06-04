using UnityEngine;
using System.Collections.Generic;


public class Mux16_Gate : MonoBehaviour
{
    public int ID_Gate { get; private set; } 

    public void SetID(int id)
    {
        ID_Gate = id;
    }
    [SerializeField] private CableManager16bit inputA; 
    [SerializeField] private CableManager16bit inputB; 
    [SerializeField] private CableManager inputSel; 
    public ButtonController16bit Out;
    private bool isConnected =false;
    
    public ButtonController16bit getCable()
    {
        return Out;
    }
    public CableManager16bit getInputA()
    {
        return inputA;
    }
    public CableManager16bit getInputB()
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
    private List<Cable16bitTruthTable> Evaluate(List<Cable16bitTruthTable> truthTableA,List<Cable16bitTruthTable> truthTableB,List<bool> truthTableSel)
    {
        List<Cable16bitTruthTable> newTruthTable = new List<Cable16bitTruthTable>(truthTableA);
        for (int i = 0; i < truthTableA.Count; i++)
        {
            for (int j=0;j< truthTableA[i].truthTable.Count ;j++)
            {
                if(! truthTableSel[i])
                {
                    newTruthTable[i].truthTable[j]=truthTableA[i].truthTable[j];
                }
                else
                {
                    newTruthTable[i].truthTable[j]=truthTableB[i].truthTable[j];
                }
                // newTruthTable[i].truthTable[j] = ((truthTableB[i].truthTable[j] && truthTableSel[i].truthTable[j]) || (truthTableA[i].truthTable[j] && !(truthTableSel[i].truthTable[j])));
            }
        }
        return newTruthTable;
    }
    private void UpdateTruthTable()
    {
        Out.SetTruthTable(Evaluate(inputA.getConnectedCable().GetTruthTable(),inputB.getConnectedCable().GetTruthTable(),inputSel.getConnectedCable().GetTruthTable()));
        List<Cable16bitTruthTable> newTruthTable=Out.GetTruthTable();
        Debug.Log("Input A MuxGate: ");
        for(int i=0;i<newTruthTable.Count;i++)
        {
            Debug.Log("  "+i +" "+ string.Join(", ", newTruthTable[i].truthTable));
        }
        
    }

    public static List<Cable16bitTruthTable> TestIN_A()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,true ,true}));
        // Row 3
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,false ,false}));
        // Row 4
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,true ,true}));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,true ,true}));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,false ,false}));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,false ,false}));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,true ,true}));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,false ,false}));
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));

        return result;
    }
    public static List<Cable16bitTruthTable> TestIN_B()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,true ,true}));
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 3
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,true ,true ,false ,false ,true ,true}));
        // Row 4
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,true ,true}));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,true ,true ,false ,false ,false ,false}));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,true ,true}));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,true ,true}));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,true ,true ,false ,false ,false ,false}));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,true ,true ,false ,false ,true ,true}));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,true ,true ,false ,false ,true ,true}));
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,true ,true}));

        return result;
    }
    public static List<Cable16bitTruthTable> TestIN_SEL()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,true ,false ,true ,false ,true ,false ,true}));

        return result;
    }

public static List<Cable16bitTruthTable> TestOUT()
{
    List<Cable16bitTruthTable> originala = TestIN_A();
    List<Cable16bitTruthTable> originalb = TestIN_B();
    List<Cable16bitTruthTable> selList = TestIN_SEL();
    List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();

    for (int i = 0; i < originala.Count; i++)
    {
        List<bool> muxedTruth = new List<bool>();

        for (int j = 0; j < originala[i].truthTable.Count; j++)
        {
            bool sel = selList[0].truthTable[j];
            bool output = sel ? originalb[i].truthTable[j] : originala[i].truthTable[j];
            muxedTruth.Add(output);
        }

        result.Add(new Cable16bitTruthTable(muxedTruth));
    }

    return result;
}






}