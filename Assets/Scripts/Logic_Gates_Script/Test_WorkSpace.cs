// using DoorScript;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

[System.Serializable]
public class Cable16bitTruthTable
{
    public List<bool> truthTable; 
    
    public Cable16bitTruthTable(List<bool> values)
    {
        this.truthTable = new List<bool>(values);
    }
}


[System.Serializable]
public class CableManagerTruthTableT
{
    public CableManager cableManager; 
    public List<Cable16bitTruthTable> truthTable = new List<Cable16bitTruthTable>(){}; 
}

[System.Serializable]
public class CableTruthTable
{
    public Cable cable; 
    public ButtonController1bit cables;
    // public Cable16bit cable16bit;
    public List<Cable16bitTruthTable> truthTable = new List<Cable16bitTruthTable>(){}; 
}


[System.Serializable]
public class CableManagerTruthTable16bit
{
    public CableManager16bit cableManager; 
    public ButtonController_CableManager cableManagers;
    // public CableManager16bit cableManager16bit;
    public List<Cable16bitTruthTable> truthTable = new List<Cable16bitTruthTable>(){}; 
}
[System.Serializable]
public class CableTruthTable16bit
{
    public Cable16bit cable; 
    public ButtonController16bit cables;
    // public Cable16bit cable16bit;
    public List<Cable16bitTruthTable> truthTable = new List<Cable16bitTruthTable>(){}; 
}

public class Test_WorkSpace : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private LogicGates_Type Name; 
    // [SerializeField] private Micanical_door door; 
    [SerializeField] private List<CableTruthTable> cableTruthTables = new List<CableTruthTable>(){};
    [SerializeField] private List<CableManagerTruthTableT> cableManagaerTruthTables = new List<CableManagerTruthTableT>(){};
    [SerializeField] private List<CableTruthTable16bit> cable16TruthTables = new List<CableTruthTable16bit>(){};
    [SerializeField] private List<CableManagerTruthTable16bit> cableManagaer16TruthTables = new List<CableManagerTruthTable16bit>(){};
    
    [SerializeField] public GameObject challenge;

    [SerializeField] public GameObject challengeUI_win;
    [SerializeField] public GameObject challengeUI_fail;
    [SerializeField] public GameObject challengeUI_pass_gate;

    private bool isSolves =false;
    private bool isSolves16 =false;
    private bool isAllCorrect16 =false;
    private bool isAllCorrect =false;

    private void Not_Gate_tests()
    {
        SearchAndPrintCable(ConnectorType.In,Not_Gate.TestIN(),CableTypes.c1);
    }
    private void Not16_Gate_tests()
    {
        SearchAndPrintCable(ConnectorType.In,Not16_Gate.TestIN(),CableTypes.c16);
    }
    private void And_Gate_tests()
    {
        SearchAndPrintCable(ConnectorType.InA,AND_Gate.TestIN_A(),CableTypes.c1);
        SearchAndPrintCable(ConnectorType.InB,AND_Gate.TestIN_B(),CableTypes.c1);
    }
    private void And16_Gate_tests()
    {
        SearchAndPrintCable(ConnectorType.InA,AND16_Gate.TestIN_A(),CableTypes.c16);
        SearchAndPrintCable(ConnectorType.InB,AND16_Gate.TestIN_B(),CableTypes.c16);
    }

    private void Xor_Gate_tests()
    {
        SearchAndPrintCable(ConnectorType.InA,Xor_Gate.TestIN_A(),CableTypes.c1);
        SearchAndPrintCable(ConnectorType.InB,Xor_Gate.TestIN_B(),CableTypes.c1);
    }
    private void Mux_Gate_tests()
    {
        SearchAndPrintCable(ConnectorType.InA,Mux_Gate.TestIN_A(),CableTypes.c1);
        SearchAndPrintCable(ConnectorType.InB,Mux_Gate.TestIN_B(),CableTypes.c1);
        SearchAndPrintCable(ConnectorType.InSel,Mux_Gate.TestIN_SEL(),CableTypes.c1);
    }
    private void Dmux_Gate_tests()
    {
        SearchAndPrintCable(ConnectorType.InA,Dmux_Gate.TestIN_IN(),CableTypes.c1);
        SearchAndPrintCable(ConnectorType.InB,Dmux_Gate.TestIN_SEL(),CableTypes.c1);
    }
    private void Or_Gate_tests()
    {
        SearchAndPrintCable(ConnectorType.InA,OR_Gate.TestIN_A(),CableTypes.c1);
        SearchAndPrintCable(ConnectorType.InB,OR_Gate.TestIN_B(),CableTypes.c1);
    }
    private void Or16_Gate_tests()
    {
        SearchAndPrintCable(ConnectorType.InA,OR16_Gate.TestIN_A(),CableTypes.c16);
        SearchAndPrintCable(ConnectorType.InB,OR16_Gate.TestIN_B(),CableTypes.c16);
    }

    private void Mux16_Gate_tests()
    {
        SearchAndPrintCable(ConnectorType.InA,Mux16_Gate.TestIN_A(),CableTypes.c16);
        SearchAndPrintCable(ConnectorType.InB,Mux16_Gate.TestIN_B(),CableTypes.c16);
        SearchAndPrintCable(ConnectorType.InSel,Mux16_Gate.TestIN_SEL(),CableTypes.c1);
    }

    private void Mux4Way16_Gate_tests()
    {
        SearchAndPrintCable(ConnectorType.InA,Mux4Way16_Gate.TestIN_A(),CableTypes.c16);
        SearchAndPrintCable(ConnectorType.InB,Mux4Way16_Gate.TestIN_B(),CableTypes.c16);
        SearchAndPrintCable(ConnectorType.InC,Mux4Way16_Gate.TestIN_C(),CableTypes.c16);
        SearchAndPrintCable(ConnectorType.InD,Mux4Way16_Gate.TestIN_D(),CableTypes.c16);
        SearchAndPrintCable(ConnectorType.InSel,Mux4Way16_Gate.TestIN_SEL(),CableTypes.c16);
    }

    private void Mux8Way16_Gate_tests()
    {
        SearchAndPrintCable(ConnectorType.InA,Mux8Way16_Gate.TestIN_A(),CableTypes.c16);
        SearchAndPrintCable(ConnectorType.InB,Mux8Way16_Gate.TestIN_B(),CableTypes.c16);
        SearchAndPrintCable(ConnectorType.InC,Mux8Way16_Gate.TestIN_C(),CableTypes.c16);
        SearchAndPrintCable(ConnectorType.InD,Mux8Way16_Gate.TestIN_D(),CableTypes.c16);
        SearchAndPrintCable(ConnectorType.InE,Mux8Way16_Gate.TestIN_E(),CableTypes.c16);
        SearchAndPrintCable(ConnectorType.InF,Mux8Way16_Gate.TestIN_F(),CableTypes.c16);
        SearchAndPrintCable(ConnectorType.InG,Mux8Way16_Gate.TestIN_G(),CableTypes.c16);
        SearchAndPrintCable(ConnectorType.InH,Mux8Way16_Gate.TestIN_H(),CableTypes.c16);
        SearchAndPrintCable(ConnectorType.InSel,Mux8Way16_Gate.TestIN_SEL(),CableTypes.c16);
        // SearchAndPrintCable(ConnectorType.InSeln2,Mux8Way16_Gate.TestIN_SEL(),CableTypes.c16);
        // SearchAndPrintCable(ConnectorType.InSel0n1,Mux8Way16_Gate.TestIN_SEL0(),CableTypes.c1);
        // SearchAndPrintCable(ConnectorType.InSel0n2,Mux8Way16_Gate.TestIN_SEL0(),CableTypes.c1);
        // SearchAndPrintCable(ConnectorType.InSel1n1,Mux8Way16_Gate.TestIN_SEL1(),CableTypes.c1);
        // SearchAndPrintCable(ConnectorType.InSel1n2,Mux8Way16_Gate.TestIN_SEL1(),CableTypes.c1);
        // SearchAndPrintCable(ConnectorType.InSel2n1,Mux8Way16_Gate.TestIN_SEL2(),CableTypes.c1);
        // SearchAndPrintCable(ConnectorType.InSel2n2,Mux8Way16_Gate.TestIN_SEL2(),CableTypes.c1);
        // SearchAndPrintCable(ConnectorType.InSel1n0_1,Mux8Way16_Gate.TestIN_SEL0_1(),CableTypes.c16);
        // SearchAndPrintCable(ConnectorType.InSel2n0_1,Mux8Way16_Gate.TestIN_SEL0_1(),CableTypes.c16);
        // SearchAndPrintCable(ConnectorType.InSel1n1_2,Mux8Way16_Gate.TestIN_SEL1_2(),CableTypes.c16);
        // SearchAndPrintCable(ConnectorType.InSel2n1_2,Mux8Way16_Gate.TestIN_SEL1_2(),CableTypes.c16);
    }

    private void Dmux4Way_Gate_tests()
    {
        
        SearchAndPrintCable(ConnectorType.In,Dmux4Way_Gate.TestIN(),CableTypes.c1);
        SearchAndPrintCable(ConnectorType.InSel,Dmux4Way_Gate.TestIN_SEL(),CableTypes.c16);
        
    }
    private void HalfAdder_Gate_tests()
    {
        SearchAndPrintCable(ConnectorType.InA,HalfAdder_Gate.TestA(),CableTypes.c1);
        SearchAndPrintCable(ConnectorType.InB,HalfAdder_Gate.TestB(),CableTypes.c1);
        
    }
    private void FullAdder_Gate_tests()
    {
        SearchAndPrintCable(ConnectorType.InA,FullAdder_Gate.TestA(),CableTypes.c1);
        SearchAndPrintCable(ConnectorType.InB,FullAdder_Gate.TestB(),CableTypes.c1);
        SearchAndPrintCable(ConnectorType.InC,FullAdder_Gate.TestC(),CableTypes.c1);
    }
    private void Add16_Gate_tests()
    {
        SearchAndPrintCable(ConnectorType.InA,Add16_Gate.TestA(),CableTypes.c16);
        SearchAndPrintCable(ConnectorType.InB,Add16_Gate.TestB(),CableTypes.c16);
    }
    private void InC16_Gate_tests()
    {
        SearchAndPrintCable(ConnectorType.InA,Inc16_Gate.TestA(),CableTypes.c16);
        // SearchAndPrintCable(ConnectorType.InB,Inc16_Gate.TestB(),CableTypes.c16);
        // SearchAndPrintCable(ConnectorType.InC,Inc16_Gate.TestC(),CableTypes.c16);
    }
    private void Or4Way_Gate_tests()
    {
        SearchAndPrintCable(ConnectorType.In,Or4Way_Gate.TestIN(),CableTypes.c16);
    }
    void Start()
    {
        if(Name == LogicGates_Type.Not)
        {
            Not_Gate_tests();
        }
        else if(Name == LogicGates_Type.Not16)
        {
            Not16_Gate_tests();
        }
        else if(Name == LogicGates_Type.And)
        {
            And_Gate_tests();
        }
        else if(Name == LogicGates_Type.Xor)
        {
            Xor_Gate_tests();
        }
        else if(Name == LogicGates_Type.Mux)
        {
            Mux_Gate_tests();
        }
        else if(Name == LogicGates_Type.Dmux)
        {
            Dmux_Gate_tests();
        }
        else if(Name == LogicGates_Type.And16)
        {
            And16_Gate_tests();
        }
        else if(Name == LogicGates_Type.Or)
        {
            Or_Gate_tests();
        }
        else if(Name == LogicGates_Type.Or16)
        {
            Or16_Gate_tests();
        }
        else if(Name == LogicGates_Type.Or4Way)
        {
            Or4Way_Gate_tests();
        }
        else if (Name == LogicGates_Type.Mux4Way16)
        {
            Mux4Way16_Gate_tests();
        }
        else if (Name == LogicGates_Type.Mux8Way16)
        {
            Mux8Way16_Gate_tests();
        }
        else if (Name == LogicGates_Type.Dmux4Way)
        {
            Dmux4Way_Gate_tests();
        }
        else if (Name == LogicGates_Type.HalfAdder)
        {

            HalfAdder_Gate_tests();
        }
        else if (Name == LogicGates_Type.FullAdder)
        {

            FullAdder_Gate_tests();
        }
        else if (Name == LogicGates_Type.Add16)
        {

            Add16_Gate_tests();
        }
        else if (Name == LogicGates_Type.Inc16)
        {
            InC16_Gate_tests();
        }
        else 
        {

        }
        // Debug.Log(" ana mosa bafham fe alhaker");
        // foreach (var entry in cable16TruthTables)
        // {
        //     List<Cable16bitTruthTable> new11TruthTable=entry.cable.GetTruthTable();
        //     Debug.Log($"Input {entry.cable.Name}: ");
        //     for(int i=0;i<new11TruthTable.Count;i++)
        //     {
        //         Debug.Log("  "+i +" "+ string.Join(", ", new11TruthTable[i].truthTable));
        //     }
        // }

    }

    void Update()
    {
        
        if(isSolves16 && isAllCorrect16)
        {
            if (cableManagaerTruthTables.Count == 0)
            {
                return;
            }
        }
        if (isSolves && isAllCorrect)
        {
            if (cableManagaer16TruthTables.Count == 0 )
            {
               return; 
            }
        }
        if (isSolves16 && isSolves && isAllCorrect && isAllCorrect16)
        {
            return;
        }
        int count16=0;
        int count =0;
        
        for (int i=0; i<cableManagaer16TruthTables.Count ;i++)
        {
            if (cableManagaer16TruthTables[i] != null  && cableManagaer16TruthTables[i].cableManagers.getSizeTruthTable() == 16)
            {
                var actualTruthTable = cableManagaer16TruthTables[i].cableManagers.GetTruthTable();
                var cableManagerName= cableManagaer16TruthTables[i].cableManagers.Name;
                if(Name == LogicGates_Type.Mux4Way16 )
                {
                    if(cableManagerName == ConnectorType.Out)
                    {
                        if (!CheckTruth16Table(actualTruthTable, Mux4Way16_Gate.TestOUT()))
                        {
                            isSolves16=false;
                            break;
                        }
                        count16++;
                        isSolves16=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                }
                else if (Name == LogicGates_Type.Mux8Way16)
                {
                    if(cableManagerName == ConnectorType.Out)
                    {
                        if (!CheckTruth16Table(actualTruthTable, Mux8Way16_Gate.TestOUT()))
                        {
                            isSolves16=false;
                            break;
                        }
                        count16++;
                        isSolves16=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                }
                else if (Name == LogicGates_Type.Not16)
                {
                    if(cableManagerName == ConnectorType.Out)
                    {
                        if (!CheckTruth16Table(actualTruthTable, Not16_Gate.TestOUT()))
                        {
                            isSolves16=false;
                            break;
                        }
                        count16++;
                        isSolves16=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                }
                else if (Name == LogicGates_Type.And16)
                {
                    if(cableManagerName == ConnectorType.Out)
                    {
                        if (!CheckTruth16Table(actualTruthTable, AND16_Gate.TestOUT()))
                        {
                            isSolves16=false;
                            break;
                        }
                        count16++;
                        isSolves16=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                }
                else if (Name == LogicGates_Type.Inc16)
                {
                    if(cableManagerName == ConnectorType.Out)
                    {
                        if (!CheckTruth16Table(actualTruthTable, Inc16_Gate.TestOUT()))
                        {
                            isSolves16=false;
                            break;
                        }
                        count16++;
                        isSolves16=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                }
                else if (Name == LogicGates_Type.Or16)
                {
                    if(cableManagerName == ConnectorType.Out)
                    {
                        if (!CheckTruth16Table(actualTruthTable, OR16_Gate.TestOUT()))
                        {
                            isSolves16=false;
                            break;
                        }
                        count16++;
                        isSolves16=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                }
                else if (Name == LogicGates_Type.Add16)
                {
                    if(cableManagerName == ConnectorType.Out)
                    {
                        if (!CheckTruth16Table(actualTruthTable, Add16_Gate.TestOUT()))
                        {
                            isSolves16=false;
                            break;
                        }
                        count16++;
                        isSolves16=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                }
                else
                {
                    Debug.Log($"i dont find {Name}  work space.");
                } 
            }
        }
        for (int i=0; i<cableManagaer16TruthTables.Count ;i++)
        {
            if (cableManagaer16TruthTables[i] != null  && cableManagaer16TruthTables[i].cableManagers.getSizeTruthTable() == 8)
            {
                var actualTruthTable = cableManagaer16TruthTables[i].cableManagers.GetTruthTable();
                var cableManagerName= cableManagaer16TruthTables[i].cableManagers.Name;
                if(Name == LogicGates_Type.Or4Way )
                {
                    if(cableManagerName == ConnectorType.Out)
                    {
                        if (!CheckTruth16Table(actualTruthTable, Or4Way_Gate.TestOUT()))
                        {
                            isSolves16=false;
                            break;
                        }
                        count16++;
                        isSolves16=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                }
                
                else
                {
                    Debug.Log($"i dont find {Name}  work space.");
                } 
            }
        }

        for (int i=0; i<cableManagaerTruthTables.Count ;i++)
        {
            if (cableManagaerTruthTables[i] != null && cableManagaerTruthTables[i].cableManager.getConnectedCable() != null )
            {
                var actualTruthTable = cableManagaerTruthTables[i].cableManager.getConnectedCable().GetTruthTable();
                var cableManagerName= cableManagaerTruthTables[i].cableManager.Name;
                if (Name == LogicGates_Type.Dmux4Way)
                {
                    if(cableManagerName == ConnectorType.InA)
                    {
                        if (!CheckTruthTable(actualTruthTable, Dmux4Way_Gate.TestA()))
                        {
                            isSolves=false;
                            break;
                        }
                        count++;
                        isSolves=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                    else if(cableManagerName == ConnectorType.InB)
                    {
                        if (!CheckTruthTable(actualTruthTable, Dmux4Way_Gate.TestB()))
                        {
                            isSolves=false;
                            break;
                        }
                        count++;
                        isSolves=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                    else if(cableManagerName == ConnectorType.InC)
                    {
                        if (!CheckTruthTable(actualTruthTable, Dmux4Way_Gate.TestC()))
                        {
                            isSolves=false;
                            break;
                        }
                        count++;
                        isSolves=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                    else if(cableManagerName == ConnectorType.InD)
                    {
                        if (!CheckTruthTable(actualTruthTable, Dmux4Way_Gate.TestD()))
                        {
                            isSolves=false;
                            break;
                        }
                        count++;
                        isSolves=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                    else
                    {
                        Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                }
                else if (Name == LogicGates_Type.HalfAdder)
                {
                    if(cableManagerName == ConnectorType.sum)
                    {
                        if (!CheckTruthTable(actualTruthTable, HalfAdder_Gate.TestSum()))
                        {
                            isSolves=false;
                            break;
                        }
                        count++;
                        isSolves=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                    else if(cableManagerName == ConnectorType.carry)
                    {
                        if (!CheckTruthTable(actualTruthTable, HalfAdder_Gate.TestCarry()))
                        {
                            isSolves=false;
                            break;
                        }
                        count++;
                        isSolves=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                    else
                    {
                        Debug.Log($"i do not find {cableManagerName}  cableTruthTables.");
                    }
                }
                else if (Name == LogicGates_Type.FullAdder)
                {
                    if(cableManagerName == ConnectorType.sum)
                    {
                        if (!CheckTruthTable(actualTruthTable, FullAdder_Gate.TestSum()))
                        {
                            isSolves=false;
                            break;
                        }
                        count++;
                        isSolves=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                    else if(cableManagerName == ConnectorType.carry)
                    {
                        if (!CheckTruthTable(actualTruthTable, FullAdder_Gate.TestCarry()))
                        {
                            isSolves=false;
                            break;
                        }
                        count++;
                        isSolves=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                    else
                    {
                        Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                }
                else if (Name == LogicGates_Type.Not)
                {
                    if(cableManagerName == ConnectorType.Out)
                    {
                        if (!CheckTruthTable(actualTruthTable, Not_Gate.TestOut()[0].truthTable))
                        {
                            isSolves=false;
                            break;
                        }
                        count++;
                        isSolves=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                    else
                    {
                        Debug.Log($"i do not find {cableManagerName}  cableTruthTables.");
                    }
                }
                else if (Name == LogicGates_Type.And)
                {
                    if(cableManagerName == ConnectorType.Out)
                    {
                        if (!CheckTruthTable(actualTruthTable, AND_Gate.TestOut()[0].truthTable))
                        {
                            isSolves=false;
                            break;
                        }
                        count++;
                        isSolves=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                    else
                    {
                        Debug.Log($"i do not find {cableManagerName}  cableTruthTables.");
                    }
                }
                else if (Name == LogicGates_Type.Or)
                {
                    if(cableManagerName == ConnectorType.Out)
                    {
                        if (!CheckTruthTable(actualTruthTable, OR_Gate.TestOut()[0].truthTable))
                        {
                            isSolves=false;
                            break;
                        }
                        count++;
                        isSolves=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                    else
                    {
                        Debug.Log($"i do not find {cableManagerName}  cableTruthTables.");
                    }
                }
                else if (Name == LogicGates_Type.Xor)
                {
                    if(cableManagerName == ConnectorType.Out)
                    {
                        if (!CheckTruthTable(actualTruthTable, Xor_Gate.TestOut()[0].truthTable))
                        {
                            isSolves=false;
                            challengeUI_fail.SetActive(true);
                            break;
                        }
                        count++;
                        isSolves=true;
                        // Debug.Log($"i find {cableManagerName}  cableTruthTables.");
                    }
                    else
                    {
                        Debug.Log($"i do not find {cableManagerName}  cableTruthTables.");
                    }
                }
                else
                {
                    Debug.Log($"i dont find {Name}  work space.");
                } 
            }
        }


        if(isSolves16 && count16 == cableManagaer16TruthTables.Count)
        {
            isAllCorrect16=true;
            if (cableManagaerTruthTables.Count == 0)
            {
                Debug.Log("All tests passed for 16-bit gate!");
                // door.OpenDoor();
            }
        }
        if (isSolves && count == cableManagaerTruthTables.Count)
        {
            isAllCorrect=true;
            if (cableManagaer16TruthTables.Count == 0 )
            {
                Debug.Log("All tests passed for 1-bit gate!");
            //    door.OpenDoor(); 
            }
        }
        if (isSolves16 && isSolves && isAllCorrect && isAllCorrect16)
        {
            Debug.Log("All tests passed for both 1-bit and 16-bit gates!");
            if (challenge.GetComponent<Timer>().GetIsComplete())
            {
                challengeUI_win.SetActive(true);
                
            }
            else
            {
                challengeUI_pass_gate.SetActive(true);
            }
            // door.OpenDoor();
        }

    }

    private bool CheckTruth16Table(List<Cable16bitTruthTable> truthTable,List<Cable16bitTruthTable> actualTruthTable)
    {
        if(truthTable.Count == actualTruthTable.Count)
        {   
            for(int i=0;i<actualTruthTable.Count;i++)
            {
                for (int j=0;j< actualTruthTable[i].truthTable.Count ;j++)
                {
                    if(actualTruthTable[i].truthTable[j] != truthTable[i].truthTable[j])
                    {
                        return false;
                    }  
                }
            }
            return true;  
        }
        return false;
    }
    private bool CheckTruthTable(List<bool> truthTable,List<bool> actualTruthTable)
    {
        if(truthTable.Count == actualTruthTable.Count)
        {
            for(int i=0;i<actualTruthTable.Count;i++)
            {
                if(actualTruthTable[i] != truthTable[i])
                {
                    return false;
                }  
            }
            return true;  
        }
        return false;
    }


    void SearchAndPrintCable(ConnectorType Name,List<Cable16bitTruthTable> truthTable,CableTypes type)
    {
        // Debug.Log($"i find  cableTruthTables. {truthTable[0].truthTable}");
        if (type == CableTypes.c1)
        {
            for (int i=0; i<cableTruthTables.Count ;i++)
            {
                if (cableTruthTables[i].cables != null)
                {
                    if (Name == cableTruthTables[i].cables.Name)
                    {
                        Debug.Log("Input A notGate: " + string.Join(", ", truthTable[0].truthTable));
                        Debug.Log($"i find {cableTruthTables[i].cable}  cableTruthTables. {truthTable[0].truthTable}");
                        // cableTruthTables[i].cable.SetTruthTable(truthTable[0].truthTable); 
                        cableTruthTables[i].cables.SetTruthTable(truthTable[0].truthTable);
                        Debug.Log($"i find {Name}  cableTruthTables.");
                        break;
                    }        
                }
            }

        }
        
        else if (type == CableTypes.c16)
        {
            for (int i=0; i<cable16TruthTables.Count ;i++)
            {
                if (cable16TruthTables[i].cables != null)
                {
                    if (Name == cable16TruthTables[i].cables.Name)
                    {
                        cable16TruthTables[i].cables.SetTruthTable(truthTable); 
                        break;
                    }
                }
            }
        }
        else
        {
            //it is for ather cables for future
        }
    }



}