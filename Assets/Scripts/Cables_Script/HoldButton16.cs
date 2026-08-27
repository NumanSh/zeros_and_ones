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
    // no longer used to place the spawned cable - the cable is now positioned from the
    // socket's own hierarchy, which stays correct however the workspace is scaled.
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
                // Same placement rule as HoldButton: the cable's start_point/end_point children
                // are authored in the same local frame as the socket, so the cable lines up
                // exactly when the cable root takes over the socket parent's transform.
                Transform socket = positionToSpawnCable.transform;
                Transform cableParent = socket.parent != null ? socket.parent : socket;
                GameObject newCable = Instantiate(prefabToSpawnCable, cableParent);
                newCable.transform.localPosition = Vector3.zero;
                newCable.transform.localRotation = Quaternion.identity;
                newCable.transform.localScale = Vector3.one;

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
