using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; 
using System.Collections.Generic;

public class Reset : MonoBehaviour, IPointerDownHandler
{
    

    public void OnPointerDown(PointerEventData eventData)
    {

        
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        List<PlacedItemData> mySceneItems = SaveManager.GetItemsBySceneName(currentScene);

        
        // foreach (PlacedItemData item in mySceneItems)
        // {
            
        //     GameObject prefab = Resources.Load<GameObject>("Logic_Gates_prefab/" + item.itemName+"/"+item.itemName);
        //     Debug.Log($"Logic_Gates_prefab/" + item.itemName+"/"+item.itemName);

        //     if (prefab != null)
        //     {
        //         Debug.Log($"Found prefab for {item.itemName} in Resources. Instantiating at saved position.");   
        //         Vector3 savedPosition = new Vector3(item.posX, item.posY, item.posZ);
        //         Instantiate(prefab, savedPosition, Quaternion.identity);
        //     }
        //     else
        //     {
        //         Debug.LogError($" we did not find {item.itemName} in Resources!");
        //     }
        // }
        SaveManager.ClearSave();
    }
}
