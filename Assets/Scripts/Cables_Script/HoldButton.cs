using UnityEngine;
using UnityEngine.EventSystems; // ضروري لجلب نظام اللمس والضغط
using System.Collections.Generic;
using System.Collections;
public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // متغير عام لمعرفة حالة الضغط (يمكن قراءته من أي سكريبت آخر)
    public bool isPressed = false;
    [SerializeField] private GameObject prefabToSpawnCable;
    private Cable spawnedCables ;
    public GameObject bitSelectionPanel;
    public GameObject positionToSpawnCable;
    [SerializeField] public List<bool> truthTable = new List<bool>(){};
    private Vector3 panelOffset;
    

    public void SetTruthTable(List<bool> newTruthTable)
    {
        
        if (truthTable.Count != newTruthTable.Count)
        {
            truthTable = new List<bool>(newTruthTable); 
        }
        else
        {
            for (int i = 0; i < newTruthTable.Count; i++)
            {
                truthTable[i] = newTruthTable[i];
            }
        }
    }

    public void SetPanelOffset(Vector3 offset)
    {
        panelOffset = offset;
    }
    public void SetPrefabToSpawnCable(GameObject prefab)
    {
        prefabToSpawnCable = prefab;
    }
    IEnumerator WaitAndStartDrag()
    {
        yield return new WaitForSeconds(10f); 
    }
    public void Creat_A_Cable()
    {
        if (spawnedCables != null)
        {
            CableManager cableManager = spawnedCables.getCableManager();
            if (cableManager != null)
            {
                cableManager.DisconnectCable();
                spawnedCables.setCableManager(null);
            }
            WaitAndStartDrag();
            spawnedCables.SetDragging(true);
            spawnedCables.SetIsSelected(true);
            
        }
        else
        {
            if (prefabToSpawnCable != null)
            {
                // Vector3 panelOffset = new Vector3(0.6f, 0f, 0f);
                GameObject newCable = Instantiate(prefabToSpawnCable, positionToSpawnCable.transform.position+panelOffset, positionToSpawnCable.transform.rotation, positionToSpawnCable.transform.parent);
                //  GameObject newCable = Instantiate(prefabToSpawnCable, transform, Quaternion.identity, null);
                newCable.transform.localScale = transform.localScale;
                // Transform cable1Transform = newCable.transform.parent.Find("Cable");

                // if (cable1Transform != null)
                // {
                    Cable cableScript = newCable.GetComponent<Cable>();
                    if (cableScript != null)
                    {
                        spawnedCables = cableScript;
                        cableScript.SetDragging(true);
                        cableScript.SetTruthTable(truthTable);
                        cableScript.SetIsSelected(true);
                        Debug.Log("dragging");
                    }
                    else
                    {
                        Debug.LogError("i did not find a cable1");
                    }
                // }
                // else
                // {
                //     Debug.LogError("i cant find a script of cable");
                // }
            }
        }
        bitSelectionPanel.SetActive(false);
    }

    // void Update()
    // {
    //     if (truthTable.Count == 0)
    //     {
    //         SetTruthTable(positionToSpawnCable.GetComponent<ButtonController1bit>().GetTruthTable());
    //     }
    // }

    
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        Creat_A_Cable(); // يمكنك تعديل هذا الرقم ليمثل أي زر تريد
        Debug.Log("Hold button is being pressed.");
    }

    // تتنفذ تلقائياً أول ما يرفع اللاعب إصبعه عن الزر
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
