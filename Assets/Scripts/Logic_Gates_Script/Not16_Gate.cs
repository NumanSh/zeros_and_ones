using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class Not16_Gate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int ID_Gate { get; private set; } 

    public void SetID(int id)
    {
        ID_Gate = id;
    }
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
            if((input.GetCable16Saved() != null || input.GetsizeCableManagers() ==input.getSizeTruthTable() )
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
    public static List<Cable16bitTruthTable> TestIN()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,true ,true ,false ,false}));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,true ,false ,false ,false}));
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,true ,true ,true ,false}));
        // Row 3
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,true ,false ,true ,true}));
        // Row 4
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,true ,true ,true ,false}));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,true ,false ,true ,false}));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,true ,true ,false ,true}));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,true ,false ,false ,false}));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,true ,true ,true ,false}));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,true ,false ,true ,false}));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,true ,true ,false ,true}));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,true ,false ,false ,true}));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,true ,true ,false ,false}));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,true ,false ,false ,true}));
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,true ,true ,true ,false}));
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { false ,true ,false ,true ,false}));

        return result;
    }

    public static List<Cable16bitTruthTable> TestOUT()
    {
        List<Cable16bitTruthTable> original = TestIN();
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();

        foreach (var cable in original)
        {
            List<bool> invertedTruth = cable.truthTable
                .Select(bit => !bit) 
                .ToList();

            result.Add(new Cable16bitTruthTable(invertedTruth));
        }

        return result;
    }


}