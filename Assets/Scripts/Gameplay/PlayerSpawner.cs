using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject player;
    void Start()
    {
       
        string lastScene = ExitButton.LastSceneName;

        
        if (!string.IsNullOrEmpty(lastScene))
        {
         
            GameObject spawnPoint = GameObject.Find(lastScene);

            if (spawnPoint != null)
            {
              
                MonoBehaviour cc = GetComponent("PlayerController") as MonoBehaviour;
                if (cc != null) cc.enabled = false;

             
                transform.position = spawnPoint.transform.position;
                transform.rotation = spawnPoint.transform.rotation;

                if (cc != null) cc.enabled = true;
            }
        }
    }
}
