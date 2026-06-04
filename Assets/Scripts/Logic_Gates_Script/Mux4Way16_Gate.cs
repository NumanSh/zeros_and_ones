using UnityEngine;
using System.Collections.Generic;


public class Mux4Way16_Gate : MonoBehaviour
{
    public int ID_Gate { get; private set; } 

    public void SetID(int id)
    {
        ID_Gate = id;
    }
    [SerializeField] private CableManager16bit inputA; 
    [SerializeField] private CableManager16bit inputB; 
    [SerializeField] private CableManager16bit inputC; 
    [SerializeField] private CableManager16bit inputD; 
    [SerializeField] private CableManager16bit inputSel; 
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
    public CableManager16bit getInputC()
    {
        return inputC;
    }
    public CableManager16bit getInputD()
    {
        return inputD;
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
            if(inputC.getConnectedCable() == null)
            {
                isConnected=true;
            }
            if(inputD.getConnectedCable() == null)
            {
                isConnected=true;
            }
            if(inputSel.getConnectedCable() == null)
            {
                isConnected=true;
            }
        
            if (inputA.getConnectedCable() != null && inputB.getConnectedCable() && inputC.getConnectedCable() != null && inputD.getConnectedCable() != null && inputSel.getConnectedCable() != null && isConnected)
            {
                print("Both inputs are now connected!");
                isConnected=false;
                UpdateTruthTable();
            }
        }
    }
    private List<Cable16bitTruthTable> Evaluate(List<Cable16bitTruthTable> truthTableA,List<Cable16bitTruthTable> truthTableB,List<Cable16bitTruthTable> truthTableC,List<Cable16bitTruthTable> truthTableD,List<Cable16bitTruthTable> truthTableSel)
    {
        List<Cable16bitTruthTable> newTruthTable = new List<Cable16bitTruthTable>(truthTableA);
        for (int i = 0; i < truthTableA.Count; i++)
        {
            for (int j=0;j< truthTableA[i].truthTable.Count ;j++)
            {
                if( !truthTableSel[0].truthTable[i] && !truthTableSel[1].truthTable[i])
                {
                    newTruthTable[i].truthTable[j]=truthTableA[i].truthTable[j];
                }
                else if (truthTableSel[0].truthTable[i] && !truthTableSel[1].truthTable[i])
                {
                    newTruthTable[i].truthTable[j]=truthTableB[i].truthTable[j];
                }
                else if (!truthTableSel[0].truthTable[i] && truthTableSel[1].truthTable[i])
                {
                    newTruthTable[i].truthTable[j]=truthTableC[i].truthTable[j];
                }
                else
                {
                    newTruthTable[i].truthTable[j]=truthTableD[i].truthTable[j];          
                }
            }
        }
        return newTruthTable;
    }
    private void UpdateTruthTable()
    {
        Out.SetTruthTable(Evaluate(inputA.getConnectedCable().GetTruthTable(),inputB.getConnectedCable().GetTruthTable(),inputC.getConnectedCable().GetTruthTable(),inputD.getConnectedCable().GetTruthTable(),inputSel.getConnectedCable().GetTruthTable()));
        List<Cable16bitTruthTable> newTruthTable=Out.GetTruthTable();
        Debug.Log("Input A Mux4Way16Gate: ");
        for(int i=0;i<newTruthTable.Count;i++)
        {
            Debug.Log("  "+i +" "+ string.Join(", ", newTruthTable[i].truthTable));
        }
        
    }

    public static List<Cable16bitTruthTable> TestIN_A()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();

        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 3
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 4
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, true, false, false, true, false ,false, false, true, true, false, true, false, false }));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, true, false, false, true, false ,false, false, true, true, false, true, false, false }));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, true, false, false, true, false ,false, false, true, true, false, true, false, false }));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, true, false, false, true, false ,false, false, true, true, false, true, false, false }));

        return result;
    }
    

    public static List<Cable16bitTruthTable> TestIN_B()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 3
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 4
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false, false, true, true, false, false, false ,false, true, true, true, false, true, true, false }));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false, false, true, true, false, false, false ,false, true, true, true, false, true, true, false }));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false, false, true, true, false, false, false ,false, true, true, true, false, true, true, false }));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false, false, true, true, false, false, false ,false, true, true, true, false, true, true, false }));

        return result;
    }
    public static List<Cable16bitTruthTable> TestIN_C()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();

        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 3
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 4
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false, true, false, true, false, true, false ,true, false, true, false, true, false, true, false }));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false, true, false, true, false, true, false ,true, false, true, false, true, false, true, false }));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false, true, false, true, false, true, false ,true, false, true, false, true, false, true, false }));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false, true, false, true, false, true, false ,true, false, true, false, true, false, true, false }));

        return result;
    }
    public static List<Cable16bitTruthTable> TestIN_D()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();

        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 3
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 4
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false, true, false, true ,false, true, false, true, false, true, false, true }));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false, true, false, true ,false, true, false, true, false, true, false, true }));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false, true, false, true ,false, true, false, true, false, true, false, true }));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false, true, false, true ,false, true, false, true, false, true, false, true }));

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

    public static List<Cable16bitTruthTable> TestOUT()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();

        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 3
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 4
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, true, false, false, true, false ,false, false, true, true, false, true, false, false }));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false, false, true, true, false, false, false ,false, true, true, true, false, true, true, false }));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false, true, false, true, false, true, false ,true, false, true, false, true, false, true, false }));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false, true, false, true ,false, true, false, true, false, true, false, true }));
     

        return result;
    }


}