using UnityEngine;
using System.Collections.Generic;


public class Mux8Way16_Gate : MonoBehaviour
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
    [SerializeField] private CableManager16bit inputE; 
    [SerializeField] private CableManager16bit inputF; 
    [SerializeField] private CableManager16bit inputG; 
    [SerializeField] private CableManager16bit inputH; 
    [SerializeField] private CableManager16bit inputSel; 
    public Cable16bit Out;
    private bool isConnected =false;
    
    public Cable16bit getCable()
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
    public CableManager16bit getInputE()
    {
        return inputE;
    }
    public CableManager16bit getInputF()
    {
        return inputF;
    }
    public CableManager16bit getInputG()
    {
        return inputG;
    }
    public CableManager16bit getInputH()
    {
        return inputH;
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
            if(inputE.getConnectedCable() == null)
            {
                isConnected=true;
            }
            if(inputF.getConnectedCable() == null)
            {
                isConnected=true;
            }
            if(inputG.getConnectedCable() == null)
            {
                isConnected=true;
            }
            if(inputH.getConnectedCable() == null)
            {
                isConnected=true;
            }
            if(inputSel.getConnectedCable() == null)
            {
                isConnected=true;
            }
        
            if (inputA.getConnectedCable() != null && inputB.getConnectedCable() && inputC.getConnectedCable() != null && inputD.getConnectedCable() != null && inputSel.getConnectedCable() != null 
            && inputE.getConnectedCable() != null && inputF.getConnectedCable() && inputG.getConnectedCable() != null && inputH.getConnectedCable() != null && isConnected)
            {
                print("Both inputs are now connected!");
                isConnected=false;
                UpdateTruthTable();
            }
        }
    }
    private List<Cable16bitTruthTable> Evaluate(List<Cable16bitTruthTable> truthTableA,List<Cable16bitTruthTable> truthTableB,List<Cable16bitTruthTable> truthTableC,List<Cable16bitTruthTable> truthTableD
    ,List<Cable16bitTruthTable> truthTableE,List<Cable16bitTruthTable> truthTableF,List<Cable16bitTruthTable> truthTableG,List<Cable16bitTruthTable> truthTableH,List<Cable16bitTruthTable> truthTableSel)
    {
        List<Cable16bitTruthTable> newTruthTable = new List<Cable16bitTruthTable>(truthTableA);
        for (int i = 0; i < truthTableA.Count; i++)
        {
            for (int j=0;j< truthTableA[i].truthTable.Count ;j++)
            {
                if( !truthTableSel[i].truthTable[0] && !truthTableSel[i].truthTable[1] && !truthTableSel[i].truthTable[2])
                {
                    newTruthTable[i].truthTable[j]=truthTableA[i].truthTable[j];
                }
                else if (!truthTableSel[i].truthTable[0] && !truthTableSel[i].truthTable[1] && truthTableSel[i].truthTable[2])
                {
                    newTruthTable[i].truthTable[j]=truthTableB[i].truthTable[j];
                }
                else if (!truthTableSel[i].truthTable[0] && truthTableSel[i].truthTable[1] && !truthTableSel[i].truthTable[2])
                {
                    newTruthTable[i].truthTable[j]=truthTableC[i].truthTable[j];
                }
                else if (!truthTableSel[i].truthTable[0] && truthTableSel[i].truthTable[1] && truthTableSel[i].truthTable[2])
                {
                    newTruthTable[i].truthTable[j]=truthTableD[i].truthTable[j];
                }
                else if (truthTableSel[i].truthTable[0] && !truthTableSel[i].truthTable[1] && !truthTableSel[i].truthTable[2])
                {
                    newTruthTable[i].truthTable[j]=truthTableE[i].truthTable[j];
                }
                else if (truthTableSel[i].truthTable[0] && !truthTableSel[i].truthTable[1] && truthTableSel[i].truthTable[2])
                {
                    newTruthTable[i].truthTable[j]=truthTableF[i].truthTable[j];
                }
                else if (truthTableSel[i].truthTable[0] && truthTableSel[i].truthTable[1] && !truthTableSel[i].truthTable[2])
                {
                    newTruthTable[i].truthTable[j]=truthTableG[i].truthTable[j];
                }
                else
                {
                    newTruthTable[i].truthTable[j]=truthTableH[i].truthTable[j];          
                }
            }
        }
        return newTruthTable;
    }
    private void UpdateTruthTable()
    {
        Out.SetTruthTable(Evaluate(inputA.getConnectedCable().GetTruthTable(),inputB.getConnectedCable().GetTruthTable(),inputC.getConnectedCable().GetTruthTable(),inputD.getConnectedCable().GetTruthTable()
        ,inputE.getConnectedCable().GetTruthTable(),inputF.getConnectedCable().GetTruthTable(),inputG.getConnectedCable().GetTruthTable(),inputH.getConnectedCable().GetTruthTable(),inputSel.getConnectedCable().GetTruthTable()));
        List<Cable16bitTruthTable> newTruthTable=Out.GetTruthTable();
        Debug.Log("Input A Mux8Way16Gate: ");
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
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, true, false, false, true, false ,false, false, true, true, false, true, false, false }));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, true, false, false, true, false ,false, false, true, true, false, true, false, false }));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, true, false, false, true, false ,false, false, true, true, false, true, false, false }));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, true, false, false, true, false ,false, false, true, true, false, true, false, false }));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, true, false, false, true, false ,false, false, true, true, false, true, false, false }));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, true, false, false, true, false ,false, false, true, true, false, true, false, false }));
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, true, false, false, true, false ,false, false, true, true, false, true, false, false }));
        // Row 15
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
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, false, false, false, true, true ,false, true, false, false, false, true, false, true }));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, false, false, false, true, true ,false, true, false, false, false, true, false, true }));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, false, false, false, true, true ,false, true, false, false, false, true, false, true }));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, false, false, false, true, true ,false, true, false, false, false, true, false, true }));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, false, false, false, true, true ,false, true, false, false, false, true, false, true }));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, false, false, false, true, true ,false, true, false, false, false, true, false, true }));
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, false, false, false, true, true ,false, true, false, false, false, true, false, true }));
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, false, false, false, true, true ,false, true, false, false, false, true, false, true }));

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
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, true, false, true, false, false ,false, true, false, true, false, true, true, false }));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, true, false, true, false, false ,false, true, false, true, false, true, true, false }));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, true, false, true, false, false ,false, true, false, true, false, true, true, false }));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, true, false, true, false, false ,false, true, false, true, false, true, true, false }));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, true, false, true, false, false ,false, true, false, true, false, true, true, false }));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, true, false, true, false, false ,false, true, false, true, false, true, true, false }));
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, true, false, true, false, false ,false, true, false, true, false, true, true, false }));
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, true, false, true, false, false ,false, true, false, true, false, true, true, false }));

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
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, false, false, true, false, true ,false, true, true, false, false, true, true, true }));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, false, false, true, false, true ,false, true, true, false, false, true, true, true }));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, false, false, true, false, true ,false, true, true, false, false, true, true, true }));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, false, false, true, false, true ,false, true, true, false, false, true, true, true }));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, false, false, true, false, true ,false, true, true, false, false, true, true, true }));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, false, false, true, false, true ,false, true, true, false, false, true, true, true }));
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, false, false, true, false, true ,false, true, true, false, false, true, true, true }));
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, false, false, true, false, true ,false, true, true, false, false, true, true, true }));

        return result;
    }

    public static List<Cable16bitTruthTable> TestIN_E()
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
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false, true, true, false ,false, true, true, true, true, false, false, false }));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false, true, true, false ,false, true, true, true, true, false, false, false }));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false, true, true, false ,false, true, true, true, true, false, false, false }));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false, true, true, false ,false, true, true, true, true, false, false, false }));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false, true, true, false ,false, true, true, true, true, false, false, false }));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false, true, true, false ,false, true, true, true, true, false, false, false }));
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false, true, true, false ,false, true, true, true, true, false, false, false }));
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false, true, true, false ,false, true, true, true, true, false, false, false }));

        return result;
    }

    public static List<Cable16bitTruthTable> TestIN_F()
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
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, false, false, true, true, true ,true, false, false, false, true, false, false, true }));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, false, false, true, true, true ,true, false, false, false, true, false, false, true }));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, false, false, true, true, true ,true, false, false, false, true, false, false, true }));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, false, false, true, true, true ,true, false, false, false, true, false, false, true }));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, false, false, true, true, true ,true, false, false, false, true, false, false, true }));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, false, false, true, true, true ,true, false, false, false, true, false, false, true }));
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, false, false, true, true, true ,true, false, false, false, true, false, false, true }));
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, false, false, true, true, true ,true, false, false, false, true, false, false, true }));

        return result;
    }

    public static List<Cable16bitTruthTable> TestIN_G()
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
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, true, true, false, false, false ,true, false, false, true, true, false, true, false }));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, true, true, false, false, false ,true, false, false, true, true, false, true, false }));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, true, true, false, false, false ,true, false, false, true, true, false, true, false }));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, true, true, false, false, false ,true, false, false, true, true, false, true, false }));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, true, true, false, false, false ,true, false, false, true, true, false, true, false }));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, true, true, false, false, false ,true, false, false, true, true, false, true, false }));
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, true, true, false, false, false ,true, false, false, true, true, false, true, false }));
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, true, true, false, false, false ,true, false, false, true, true, false, true, false }));

        return result;
    }

    public static List<Cable16bitTruthTable> TestIN_H()
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
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false, false, false, true, false, false, true ,true, false, true, false, true, false, true, true }));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false, false, false, true, false, false, true ,true, false, true, false, true, false, true, true }));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false, false, false, true, false, false, true ,true, false, true, false, true, false, true, true }));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false, false, false, true, false, false, true ,true, false, true, false, true, false, true, true }));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false, false, false, true, false, false, true ,true, false, true, false, true, false, true, true }));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false, false, false, true, false, false, true ,true, false, true, false, true, false, true, true }));
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false, false, false, true, false, false, true ,true, false, true, false, true, false, true, true }));
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false, false, false, true, false, false, true ,true, false, true, false, true, false, true, true }));

        return result;
    }


    public static List<Cable16bitTruthTable> TestIN_SEL()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();

        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false, true, false, true,false, true, false, true, false, true, false, true }));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, true, false, false, true, true,false, false, true, true, false, false, true, true }));
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, true, true, true, true,false, false, false, false, true, true, true, true }));
        return result;
    }
    public static List<Cable16bitTruthTable> TestIN_SEL0()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();

        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false, true, false, true,false, true, false, true, false, true, false, true }));;
        return result;
    }
    public static List<Cable16bitTruthTable> TestIN_SEL1()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, true, false, false, true, true,false, false, true, true, false, false, true, true }));

        return result;
    }
    public static List<Cable16bitTruthTable> TestIN_SEL2()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, true, true, true, true,false, false, false, false, true, true, true, true }));

        return result;
    }
    public static List<Cable16bitTruthTable> TestIN_SEL0_1()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 0
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false, true, false, true,false, true, false, true, false, true, false, true }));
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, true, false, false, true, true,false, false, true, true, false, false, true, true }));

        return result;
    }
    public static List<Cable16bitTruthTable> TestIN_SEL1_2()
    {
        List<Cable16bitTruthTable> result = new List<Cable16bitTruthTable>();
        // Row 1
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, true, false, false, true, true,false, false, true, true, false, false, true, true }));
        // Row 2
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, true, true, true, true,false, false, false, false, true, true, true, true }));

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
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 5
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 6
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 7
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, false, false, false, false, false ,false, false, false, false, false, false, false, false }));
        // Row 8
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, false, true, false, false, true, false ,false, false, true, true, false, true, false, false }));
        // Row 9
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, false, false, false, true, true ,false, true, false, false, false, true, false, true }));
        // Row 10
        result.Add(new Cable16bitTruthTable(new List<bool> { false, false, true, true, false, true, false, false ,false, true, false, true, false, true, true, false }));
        // Row 11
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, false, false, true, false, true ,false, true, true, false, false, true, true, true }));
        // Row 12
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, false, true, false, true, true, false ,false, true, true, true, true, false, false, false }));
        // Row 13
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, false, false, true, true, true ,true, false, false, false, true, false, false, true }));
        // Row 14
        result.Add(new Cable16bitTruthTable(new List<bool> { false, true, true, true, true, false, false, false ,true, false, false, true, true, false, true, false }));
        // Row 15
        result.Add(new Cable16bitTruthTable(new List<bool> { true, false, false, false, true, false, false, true ,true, false, true, false, true, false, true, true }));

        return result;
    }


}