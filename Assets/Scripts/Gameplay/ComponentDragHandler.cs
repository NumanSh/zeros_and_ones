using UnityEngine;
using System.Collections.Generic;

public class ComponentDragHandler : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 offset;
    private Vector3 originalPosition;

    public List<CableManager> cableManagers; 

    public List<ButtonController1bit> cables; 

    public List<ButtonController_CableManager> button_CableManager; 

    public List<ButtonController16bit> button_Cable; 

    [Header("Workspace Boundaries (using Colliders)")]
  
    private Collider2D workspaceArenaCollider; 

    private int greenAreaLayerMask;

    private void Awake()
    {
     
        mainCamera = Camera.main;
        GameObject arenaObj = GameObject.Find("greenArea");
        
        if (arenaObj != null)
        {
            workspaceArenaCollider = arenaObj.GetComponent<Collider2D>();
        }
      
    }

 
    private void OnMouseDown()
    {
       
        originalPosition = transform.position;

       
        Vector3 mouseWorldPos = GetMouseWorldPosition();
        offset = transform.position - mouseWorldPos;
        offset.z = 0; 
    }

   
    private void OnMouseDrag()
    {
        Vector3 targetPosition = GetMouseWorldPosition() + offset;

       
        if (workspaceArenaCollider != null)
        {
         
            Bounds bounds = workspaceArenaCollider.bounds;
            targetPosition.x = Mathf.Clamp(targetPosition.x, bounds.min.x, bounds.max.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, bounds.min.y, bounds.max.y);
        }

       UpdateAttachedCables();
        transform.position = targetPosition;
    }

 
    
    private void OnMouseUp() 
    {
        Vector2 dropPosition = transform.position;

        if (workspaceArenaCollider != null && workspaceArenaCollider.OverlapPoint(dropPosition)) 
        {
            Debug.Log($"[Test Lab Log] {gameObject.name} placed successfully inside Green Area.");
        } 
        else 
        {
            Debug.Log($"[Test Lab Log] Placed outside! Returning to original position.");
            transform.position = originalPosition;
            UpdateAttachedCables();
        }
    }


    
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
      
        mousePoint.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

    private void UpdateAttachedCables()
    {
        if (cables == null || cables.Count == 0) return;

        for (int i = 0; i < cables.Count; i++)
        {
            if (cables[i] != null)
            {
                   
                int size=cables[i].GetComponent<ButtonController1bit>().spawnedCables.Count;
                List<Cable> real_cables=cables[i].GetComponent<ButtonController1bit>().spawnedCables;
                for(int j=0;j<size;j++)
                {
                    real_cables[j].SetlineRenderStart(cables[i].transform);
                }
            }
        }
        for (int i = 0; i < cableManagers.Count; i++)
        {
            if (cableManagers[i] != null)
            {
                if (cableManagers[i].getConnectedCable() != null)
                {
                    
                    cableManagers[i].getConnectedCable().SetlineRenderEnd(cableManagers[i].transform);
                }
                
                
            }
        }
        for (int i = 0; i < button_CableManager.Count; i++)
        {
            if (button_CableManager[i] != null)
            {
                List<CableManager> cableManagerb=button_CableManager[i].spawnedCableManagers;
                int size = cableManagerb.Count;
                for ( int j=0;j< size;j++)
                {
                    if (cableManagerb[j].getConnectedCable() != null)
                    {
                        
                        cableManagerb[j].getConnectedCable().SetlineRenderEnd(cableManagerb[j].transform);
                    }
                    
                }
                
                
            }
        }
        
    }


}
