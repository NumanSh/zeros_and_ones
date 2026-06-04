using UnityEngine;
using System.Collections.Generic;
public class CableManager16bit : MonoBehaviour
{
    private Cable16bit connectedCable = null; 
    public ConnectorType Name;

    public Cable16bit getConnectedCable()
    {
        return connectedCable;
    }
    public bool CanConnect()
    {
        return connectedCable == null ;
    }

    public void ConnectCable(Cable16bit cable)
    {
        connectedCable=cable;
        List<Cable16bitTruthTable> newTruthTable=cable.GetTruthTable();
        Debug.Log("Input A MuxGate: ");
        for(int i=0;i<cable.GetTruthTable().Count;i++)
        {
            Debug.Log("  "+i +" "+ string.Join(", ", cable.GetTruthTable()[i].truthTable));
        }
    }
    

    public void DisconnectCable()
    {
        connectedCable = null;
    }
}