using System.Collections.Generic;
using UnityEngine;


// [System.Serializable]
// public class Cable16bitTruthTable
// {
//     public List<bool> truthTable; 
// }

public class Add16_Gate : MonoBehaviour
{
    public int ID_Gate { get; private set; } 

    public void SetID(int id)
    {
        ID_Gate = id;
    }
    [SerializeField] private ButtonController_CableManager inputA; 
    [SerializeField] private ButtonController_CableManager inputB; 
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
    void Update()
    {
        if(inputA != null && inputB != null)
        {
            if(inputA.GetCable16Saved() == null )
            {
                isConnected=true;
            }
            if(inputB.GetCable16Saved() == null )
            {
                isConnected=true;
            }

            //  Debug.Log($" input a{inputA.GetSelectedCable16() != null}");
            if ((inputA.GetCable16Saved() != null || inputA.GetsizeCableManagers() ==inputA.getSizeTruthTable() )
            && (inputB.GetCable16Saved() != null || inputB.GetsizeCableManagers() ==inputB.getSizeTruthTable())
            && isConnected)
            {
                print("Both inputs are now connected!");
                isConnected=false;
                UpdateTruthTable();
            }
        }
    }
    private List<Cable16bitTruthTable> Evaluate(List<Cable16bitTruthTable> truthTableA,List<Cable16bitTruthTable> truthTableB)
    {
        List<Cable16bitTruthTable> newTruthTable = new List<Cable16bitTruthTable>(truthTableA);
        for (int i = 0; i < truthTableA.Count; i++)
        {
            bool carry = false;
            for (int j=0;j< truthTableA[i].truthTable.Count ;j++)
            {
                bool a=truthTableA[i].truthTable[j];
                bool b= truthTableB[i].truthTable[j];
                newTruthTable[i].truthTable[j] = a^b ^ carry;
                carry = (a && b) || (a &&  carry) || ( b && carry);
            }
        }
        return newTruthTable;
    }
    private void UpdateTruthTable()
    {
        Out.SetTruthTable(Evaluate(inputA.GetTruthTable(),inputB.GetTruthTable()));
        List<Cable16bitTruthTable> new11TruthTable=Out.GetTruthTable();
        Debug.Log("Input A AddGate: ");
        for(int i=0;i<new11TruthTable.Count;i++)
        {
            Debug.Log("  "+i +" "+ string.Join(", ", new11TruthTable[i].truthTable));
        }
        
        
    }
    public static List<Cable16bitTruthTable> TestA()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,true, false,  true,  false}));
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,true, true,  true,  false}));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,true, false,  false,  true}));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,true, true,  false,  false}));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,true, false,  false,  true}));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,true, true,  false,  true}));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,true, false,  true,  false}));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,true, true,  true,  false}));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,true, false,  false,  false}));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,true, true,  false,  true}));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,true, false,  true,  false}));
        // Row 4
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,true, true,  true,  false}));
        // Row 3
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,true, false,  true,  true}));
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,true, true,  true,  false}));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,true, false,  false,  false}));
        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,true, true,  false,  false}));

        return result;
    }
    // public static List<Cable16bitTruthTable> TestA()
    // {
    //     List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
    //     // Row 1
    //     result.Add(new Cable16bitTruthTable(new List<bool> {false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false}));
    //     //Row 2
    //     result.Add(new Cable16bitTruthTable(new List<bool> {false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false}));
    //     // Row 3
    //     result.Add(new Cable16bitTruthTable(new List<bool> {true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true}));
    //     // rOW 4
    //     result.Add(new Cable16bitTruthTable(new List<bool> {true,false,true,false,true,false,true,false,true,false,true,false,true,false,true,false}));
    //     // Row 5
    //     result.Add(new Cable16bitTruthTable(new List<bool> {false,false,true,true,true,true,false,false,true,true,false,false,false,false,true,true}));
    //     // Row 6
    //     result.Add(new Cable16bitTruthTable(new List<bool> {false,false,false,true,false,false,true,false,false,false,true,true,false,true,false,false}));

    //     return result;
    // }

    public static List<Cable16bitTruthTable> TestB()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,true, true,  false,  false})); 
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,true, false,  false,  true}));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,true, true,  false,  true}));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,true, false,  false,  false}));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,true, true,  true,  true}));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,true, false,  true,  true}));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,true, true,  true,  true}));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,true, false,  true,  false}));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,true, true,  true,  false}));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,true, false,  true,  false}));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,true, true,  true,  false}));
        // Row 4
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,true, false,  true,  true}));
        // Row 3
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,true, true,  false,  true}));
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,true, false,  false,  false}));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,true, true,  false,  false}));
        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,true, false,  false,  true}));

        return result;
    }
    
    // public static List<Cable16bitTruthTable> TestB()
    // {
    //     List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
    //     //Row 1
    //     result.Add(new Cable16bitTruthTable(new List<bool> {false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false}));
    //     // Row 2
    //     result.Add(new Cable16bitTruthTable(new List<bool> {true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true}));
    //     // Row 3
    //     result.Add(new Cable16bitTruthTable(new List<bool> {true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true}));
    //     // Row 4
    //     result.Add(new Cable16bitTruthTable(new List<bool> {false,true,false,true,false,true,false,true,false,true,false,true,false,true,false,true}));
    //     // Row 5
    //     result.Add(new Cable16bitTruthTable(new List<bool> {false,false,false,false,true,true,true,true,true,true,true,true,false,false,false,false}));
    //     // Row 6
    //     result.Add(new Cable16bitTruthTable(new List<bool> {true,false,false,true,true,false,false,false,false,true,true,true,false,true,true,false}));

    //     return result;
    // }
    
    // public static List<Cable16bitTruthTable>  TestOUT()
    // {
    //     List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
    //     //Row 1
    //     result.Add(new Cable16bitTruthTable(new List<bool> {false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false}));
    //     // Row 2
    //     result.Add(new Cable16bitTruthTable(new List<bool> {true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true}));
    //     // Row 3
    //     result.Add(new Cable16bitTruthTable(new List<bool> {false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true}));
    //     // Row 4
    //     result.Add(new Cable16bitTruthTable(new List<bool> {true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true}));
    //     // Row 5
    //     result.Add(new Cable16bitTruthTable(new List<bool> {false,false,true,true,false,true,false,false,true,true,false,false,true,false,true,true}));
    //     // Row 6
    //     result.Add(new Cable16bitTruthTable(new List<bool> {true,false,false,false,false,true,true,false,false,true,false,true,true,false,false,true}));
    //     return result;
    // }
    
    public static List<Cable16bitTruthTable>  TestOUT()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 15
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, true,false, true,  true,  false}));
        // Row 14
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, true,true, true,  true,  true}));
        // Row 13
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, true,true, true,  false,  false}));
        // Row 12
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, true,true, true,  false,  true}));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,true, true,  true,  false}));
        // Row 10
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, true,true, true,  true,  true}));
        // Row 9
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, true,true, true,  false,  false}));
        // Row 8
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, true,true, true,  true,  true}));
        // Row 7
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, true,true, true,  false,  false}));
        // Row 6
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, true,true, true,  false,  true}));
        // Row 5
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, true,true, true,  true,  false}));
        // Row 4
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, true,true, true,  true,  true}));
        // Row 3
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, true,true, true,  false,  false}));
        // Row 2
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, true,true, true,  false,  true}));
        // Row 1
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, true,true, true,  true,  false}));
        // Row 0
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, true,true, true,  false,  true}));

        return result;
    }

}