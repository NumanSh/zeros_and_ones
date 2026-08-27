using UnityEngine;
using System.Collections.Generic;


public class Mux4Way16_Gate : MonoBehaviour
{
    public int ID_Gate { get; private set; } 

    public void SetID(int id)
    {
        ID_Gate = id;
    }
    [SerializeField] private ButtonController_CableManager inputA; 
    [SerializeField] private ButtonController_CableManager inputB; 
    [SerializeField] private ButtonController_CableManager inputC; 
    [SerializeField] private ButtonController_CableManager inputD; 
    [SerializeField] private ButtonController_CableManager inputSel; 
    public ButtonController16bit Out;
    private bool isConnected =false;
    
    public ButtonController16bit getCable()
    {
        return Out;
    }
    public ButtonController_CableManager getInputA()
    {
        return inputA;
    }
    public ButtonController_CableManager getInputB()
    {
        return inputB;
    }
    public ButtonController_CableManager getInputC()
    {
        return inputC;
    }
    public ButtonController_CableManager getInputD()
    {
        return inputD;
    }
    void Update()
    {
        if(inputA != null && inputB != null && inputSel!= null)
        {
            if(inputA.GetCable16Saved() == null)
            {
                isConnected=true;
            }
            if(inputB.GetCable16Saved() == null)
            {
                isConnected=true;
            }
            if(inputC.GetCable16Saved() == null)
            {
                isConnected=true;
            }
            if(inputD.GetCable16Saved() == null)
            {
                isConnected=true;
            }
            if(inputSel.GetCable16Saved() == null)
            {
                isConnected=true;
            }
        
            if ((inputA.GetCable16Saved() != null || inputA.GetsizeCableManagers() ==inputA.getSizeTruthTable() )
            && (inputB.GetCable16Saved() != null || inputB.GetsizeCableManagers() ==inputB.getSizeTruthTable())
            && (inputC.GetCable16Saved() != null || inputC.GetsizeCableManagers() ==inputC.getSizeTruthTable() )
            && (inputD.GetCable16Saved() != null || inputD.GetsizeCableManagers() ==inputD.getSizeTruthTable())
            && (inputSel.GetCable16Saved() != null || inputSel.GetsizeCableManagers() ==inputSel.getSizeTruthTable())
            && isConnected)
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
                else if (!truthTableSel[0].truthTable[i] && truthTableSel[1].truthTable[i])
                {
                    newTruthTable[i].truthTable[j]=truthTableB[i].truthTable[j];
                }
                else if (truthTableSel[0].truthTable[i] && !truthTableSel[1].truthTable[i])
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
        Out.SetTruthTable(Evaluate(inputA.GetTruthTable(),inputB.GetTruthTable(),inputC.GetTruthTable(),inputD.GetTruthTable(),inputSel.GetTruthTable()));
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
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 3
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 4
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));

        

        return result;
    }
    

    public static List<Cable16bitTruthTable> TestIN_B()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 3
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 4
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));

        return result;
    }
    public static List<Cable16bitTruthTable> TestIN_C()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();

        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 3
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 4
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));


        return result;
    }
    public static List<Cable16bitTruthTable> TestIN_D()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();

        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 3
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 4
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,false ,false ,false ,false}));
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,false ,false ,false ,true ,true ,true ,true}));

        return result;
    }

    public static List<Cable16bitTruthTable> TestIN_SEL()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();

        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, true, false, false, true, true,false, false, true, true, false, false, true, true }));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false, true, false, true,false, true, false, true, false, true, false, true }));

        return result;
    }

    public static List<Cable16bitTruthTable> TestOUT()
    {
        List<Cable16bitTruthTable> originala = TestIN_A();
        List<Cable16bitTruthTable> originalb = TestIN_B();
        List<Cable16bitTruthTable> originalc = TestIN_C();
        List<Cable16bitTruthTable> originald = TestIN_D();
        List<Cable16bitTruthTable> selList = TestIN_SEL();
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();

        for (int i = 0; i < originala.Count; i++)
        {
            bool sel0 = selList[0].truthTable[i];
            bool sel1 = selList[1].truthTable[i];
            List<bool> muxedTruth = new List<bool>();
            if (!sel0  && !sel1)
            {
                muxedTruth =  originala[i].truthTable;
            }
            else if (!sel0  && sel1)
            {
                muxedTruth =  originalb[i].truthTable;
            }
            else if (sel0  && !sel1)
            {
                muxedTruth =  originalc[i].truthTable;
            }
            else
            {
                muxedTruth =  originald[i].truthTable;
            }
            result.Add(new Cable16bitTruthTable(muxedTruth));
        }

        return result;
    }


}