using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HintBook : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject bookPanel;      
    public GameObject imageDisplay;    

    [Header("Images lists")]
    public GameObject[] hintImages;        
    public GameObject[] instructionImages; // New: Instruction pages
    
    private GameObject[] activeImages;     // Currently active set of images
    private int currentIndex = 0;     

    [Header("Navigation Buttons")]
    public Button RightButton;
    public Button LeftButton;
    public Button CancelButton;
    public Button OpenBookButton;

    [Header("Tabs (Optional)")]
    public Button instructionsTabButton; // New: Tab button for instructions
    public Button hintsTabButton;        // New: Tab button for hints

    [Header("State")]
    [Tooltip("All of these are hidden while the book is open, and restored when it closes.")]
    public GameObject[] objectsToHide;

    public bool IsOpen => bookPanel != null && bookPanel.activeSelf;

    public void Start()
    {
        bookPanel.SetActive(false);

        RightButton.onClick.AddListener(NextPage);
        LeftButton.onClick.AddListener(PreviousPage);
        CancelButton.onClick.AddListener(CloseBook);
        OpenBookButton.onClick.AddListener(OpenBook);

        // Ensure the nav buttons render above the book's background artwork,
        // regardless of how they're ordered in the prefab hierarchy.
        RightButton.transform.SetAsLastSibling();
        LeftButton.transform.SetAsLastSibling();
        CancelButton.transform.SetAsLastSibling();

        // Set default active images list
        activeImages = hintImages;

        // Setup optional tabs
        if (instructionsTabButton != null)
        {
            instructionsTabButton.onClick.AddListener(ShowInstructions);
        }
        if (hintsTabButton != null)
        {
            hintsTabButton.onClick.AddListener(ShowHints);
        }
    }

    public void OpenBook()
    {
        // Default to Hints or first available list when opening
        if (hintImages != null && hintImages.Length > 0)
        {
            activeImages = hintImages;
        }
        else if (instructionImages != null && instructionImages.Length > 0)
        {
            activeImages = instructionImages;
        }

        if (activeImages != null && activeImages.Length > 0)
        {
            // Hide all pages in both lists first to avoid overlaps
            HideAllPages();

            bookPanel.SetActive(true);
            Debug.Log("open");
            currentIndex = 0;
            UpdatePage();
            SetHiddenObjectsActive(false);
        }
    }

    public void CloseBook()
    {
        bookPanel.SetActive(false);
        SetHiddenObjectsActive(true);
    }

    private void SetHiddenObjectsActive(bool active)
    {
        if (objectsToHide == null) return;
        foreach (var obj in objectsToHide)
        {
            if (obj != null) obj.SetActive(active);
        }
    }

    public void ShowInstructions()
    {
        if (instructionImages == null || instructionImages.Length == 0) return;
        
        HideAllPages();
        activeImages = instructionImages;
        currentIndex = 0;
        UpdatePage();
    }

    public void ShowHints()
    {
        if (hintImages == null || hintImages.Length == 0) return;

        HideAllPages();
        activeImages = hintImages;
        currentIndex = 0;
        UpdatePage();
    }

    private void HideAllPages()
    {
        if (hintImages != null)
        {
            foreach (var img in hintImages)
            {
                if (img != null) img.SetActive(false);
            }
        }
        if (instructionImages != null)
        {
            foreach (var img in instructionImages)
            {
                if (img != null) img.SetActive(false);
            }
        }
        if (imageDisplay != null)
        {
            imageDisplay.SetActive(false);
        }
    }

    public void NextPage()
    {
        if (activeImages == null) return;

        if (currentIndex < activeImages.Length - 1)
        {
            currentIndex++;
            if (imageDisplay != null) imageDisplay.SetActive(false);
            UpdatePage();
        }
    }

    public void PreviousPage()
    {
        if (activeImages == null) return;

        if (currentIndex > 0)
        {
            currentIndex--;
            if (imageDisplay != null) imageDisplay.SetActive(false);
            UpdatePage();
        }
    }

    void UpdatePage()
    {
        if (activeImages == null || currentIndex < 0 || currentIndex >= activeImages.Length) return;

        imageDisplay = activeImages[currentIndex];
        if (imageDisplay != null)
        {
            imageDisplay.SetActive(true);
        }
    }
}
