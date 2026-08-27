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

            // Fallback: If exact case match fails, do a case-insensitive search
            if (spawnPoint == null)
            {
                // Try searching door-tagged objects first
                GameObject[] doors = GameObject.FindGameObjectsWithTag("door");
                foreach (GameObject door in doors)
                {
                    if (string.Equals(door.name, lastScene, System.StringComparison.OrdinalIgnoreCase))
                    {
                        spawnPoint = door;
                        break;
                    }
                }
            }

            // Fallback 2: Search all game objects in the scene if still not found
            if (spawnPoint == null)
            {
                GameObject[] allObjects = FindObjectsOfType<GameObject>();
                foreach (GameObject go in allObjects)
                {
                    if (string.Equals(go.name, lastScene, System.StringComparison.OrdinalIgnoreCase))
                    {
                        spawnPoint = go;
                        break;
                    }
                }
            }

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
