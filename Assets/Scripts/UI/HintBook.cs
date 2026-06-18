using UnityEngine;
using UnityEngine.UI; // مهم جداً للتعامل مع الـ UI

public class HintBook : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject bookPanel;      
        
    public GameObject imageDisplay;    

    [Header("Images list")]
    public GameObject[] hintImages;        
    private int currentIndex = 0;     

    public Button RightButton;

    public Button LeftButton;

    public Button CancelButton;

    public Button OpenBookButton;


   
    public void Start()
    {
        bookPanel.SetActive(false); 
        
        RightButton.onClick.AddListener(NextPage);
        LeftButton.onClick.AddListener(PreviousPage);
        CancelButton.onClick.AddListener(CloseBook);
        OpenBookButton.onClick.AddListener(OpenBook);
        
    }

    public void OpenBook()
    {
        if (hintImages.Length > 0)
        {
            bookPanel.SetActive(true);
            Debug.Log("open");
            currentIndex = 0;        
            UpdatePage();
        }
    }

  
    public void CloseBook()
    {
        bookPanel.SetActive(false);
    }

  
    public void NextPage()
    {
        if (currentIndex < hintImages.Length - 1)
        {
            currentIndex++;
            imageDisplay.SetActive(false);
            UpdatePage();
        }
    }

  
    public void PreviousPage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            imageDisplay.SetActive(false);
            UpdatePage();
        }
    }

   
    void UpdatePage()
    {
        imageDisplay = hintImages[currentIndex];
        imageDisplay.SetActive(true);
    }
}
