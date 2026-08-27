using UnityEngine;
using System.Collections.Generic;
public class Inc16_Gate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private ButtonController_CableManager input; 
    public ButtonController16bit Out;
    public ButtonController16bit getCable()
    {
        return Out;
    }
    public ButtonController_CableManager getInput()
    {
        return input;
    }
    private bool isConnected = false;
    
    void Update()
    {
        if (input != null  )
        {
            if ((input.GetCable16Saved() != null || input.GetsizeCableManagers() ==input.getSizeTruthTable() )
            && isConnected)
            {
                UpdateTruthTable();
                isConnected =false;
            }
            if(input.GetCable16Saved() == null )
            {
                isConnected =true;
            }
            
            
        }
    }
    private List<Cable16bitTruthTable> Evaluate(List<Cable16bitTruthTable> truthTable)
    {
        List<Cable16bitTruthTable> newTruthTable = new List<Cable16bitTruthTable>(truthTable);
        for (int i = 0; i < truthTable.Count; i++)
        {
            for (int j=0;j< truthTable[i].truthTable.Count ;j++)
            {
                newTruthTable[i].truthTable[j] = !(truthTable[i].truthTable[j]);
            }
        }
        return newTruthTable;
    }
    private void UpdateTruthTable()
    {
        
        Out.SetTruthTable(Evaluate(input.GetTruthTable()));
        Debug.Log("Input A notGate: " + string.Join(", ", Out.GetTruthTable()));
        List<Cable16bitTruthTable> newTruthTable=Out.GetTruthTable();
        Debug.Log("Input A ORGate: ");
        for(int i=0;i<newTruthTable.Count;i++)
        {
            Debug.Log("  "+i +" "+ string.Join(", ", newTruthTable[i].truthTable));
        }
        
        
    }
    
    public static List<Cable16bitTruthTable> TestA()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,false, true}));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,false, true}));
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,false, true}));
        // Row 3
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,false, true}));
        // Row 4
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,false, true}));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,false, true}));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,false, true}));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,false, true}));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,false, true}));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,false, true}));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,false, true}));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,false, true}));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,false, true}));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,true, false}));
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,false, true}));
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true,true, true}));
        return result;
    }


    public static List<Cable16bitTruthTable> TestB()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,false, false}));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,false, false}));
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,false, false}));
        // Row 3
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,false, false}));
        // Row 4
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,false, false}));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,false, false}));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,false, false}));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,false, false}));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,false, false}));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,false, false}));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,false, false}));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,false, false}));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,false, false}));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,false, false}));
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,false, false}));
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false,false, false}));

        return result;
    }

    public static List<Cable16bitTruthTable> TestC()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { true, true,true, true}));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { true, true,true, true})); 
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { true, true,true, true})); 
        // Row 3
        result.Add(new Cable16bitTruthTable(new List<bool> { true, true,true, true})); 
        // Row 4
        result.Add(new Cable16bitTruthTable(new List<bool> { true, true,true, true})); 
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { true, true,true, true}));  
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { true, true,true, true})); 
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { true, true,true, true})); 
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { true, true,true, true})); 
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { true, true,true, true})); 
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { true, true,true, true})); 
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { true, true,true, true})); 
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { true, true,true, true})); 
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { true, true,true, true})); 
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { true, true,true, true}));
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { true, true,true, true})); 

        return result;
    }

    public static List<Cable16bitTruthTable>  TestOUT()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 0
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, false,false, true}));
        // Row 1
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, false,false, true}));
        // Row 2
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, false,false, true}));
        // Row 3
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, false,false, true}));
        // Row 4
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, false,false, true}));
        // Row 5
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, false,false, true}));
        // Row 6
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, false,false, true}));
        // Row 7
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, false,false, true}));
        // Row 8
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, false,false, true}));
        // Row 9
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, false,false, true}));
        // Row 10
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, false,false, true}));
        // Row 11
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, false,false, true}));
        // Row 12
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, false,false, true}));
        // Row 13
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, false,true, true}));
        // Row 14
        result.Add(new Cable16bitTruthTable(new  List<bool> { false, false,true, false}));
        // Row 15
        result.Add(new Cable16bitTruthTable(new  List<bool> { true, false,false, false}));

        return result;
    }

}