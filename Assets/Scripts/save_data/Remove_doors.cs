using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remove_doors : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public LogicGates_Type Name; 
    public GameObject door;
    void Start()
    {
        if (ComponentBarManager.SolvedComponents[Name])
        {
            door.SetActive(false);
        }
    }

    // Update is called once per frame
    
}
