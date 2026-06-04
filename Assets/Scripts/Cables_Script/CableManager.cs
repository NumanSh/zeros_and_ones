using UnityEngine;
using System.Collections.Generic;
public class CableManager : MonoBehaviour
{
    private Cable connectedCable = null; 
    public ConnectorType Name;

    public Cable getConnectedCable()
    {
        return connectedCable;
    }
    public bool CanConnect()
    {
        return connectedCable == null ;
    }

    public void ConnectCable(Cable cable)
    {
        connectedCable = cable;
        Debug.Log("Input"+cable.Name + ":" + string.Join(", ", cable.GetTruthTable()));
    }
    

    public void DisconnectCable()
    {
        connectedCable = null;
    }
}