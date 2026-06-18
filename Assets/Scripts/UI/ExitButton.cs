using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; 


public class ExitButton : MonoBehaviour, IPointerDownHandler 
{
    public static string LastSceneName = "";

   
    public void OnPointerDown(PointerEventData eventData)
    {
  
        LastSceneName = SceneManager.GetActiveScene().name;

        
        SceneManager.LoadScene("firstMap");
    }
}
