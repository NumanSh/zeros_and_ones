using UnityEngine;
using UnityEngine.EventSystems; // ضروري لجلب نظام اللمس والضغط
using System.Collections.Generic;
using System.Collections;
public class HoldButton16 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // متغير عام لمعرفة حالة الضغط (يمكن قراءته من أي سكريبت آخر)
    public bool isPressed = false;
    [SerializeField] private GameObject prefabToSpawnCable;
    private Cable16bit spawnedCables ;
    public GameObject bitSelectionPanel;
    public GameObject positionToSpawnCable;
    [SerializeField] public List<Cable16bitTruthTable> truthTable = new List<Cable16bitTruthTable>(){};
    private Vector3 panelOffset;
    

    public void SetTruthTable(List<Cable16bitTruthTable> newTruthTable)
    {

        truthTable.Clear();
        foreach (var item in newTruthTable)
        {
            truthTable.Add(new Cable16bitTruthTable(new List<bool>(item.truthTable))); 
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
    public void Creat_A_Cable16bit()
    {
        if (spawnedCables != null)
        {
            CableManager16bit cableManager = spawnedCables.getCableManager();
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
                    Cable16bit cableScript = newCable.GetComponent<Cable16bit>();
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

    
    
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        Creat_A_Cable16bit(); 
        Debug.Log("Hold button is being pressed.");
    }

      public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
