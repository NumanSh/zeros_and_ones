using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class InstructionBook : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [Header("UI Elements")]
    [Tooltip("The panel to show/hide. Its GameObject stays active at all times so its VideoPlayers can warm up in advance; visibility is controlled via CanvasGroup instead.")]
    public GameObject bookPanel;

    [Header("Pages")]
    [Tooltip("Each entry is a full page (e.g. the video display for 'drag', 'zoom', 'connect'). Only one is active at a time.")]
    public GameObject[] pages;
    private int currentIndex = 0;

    [Header("Navigation Buttons")]
    public Button RightButton;
    public Button LeftButton;
    public Button CancelButton;
    public Button OpenBookButton;

    [Header("State")]
    [Tooltip("All of these are hidden while the book is open, and restored when it closes.")]
    public GameObject[] objectsToHide;

    /// <summary>True while the book is visually open. Unlike bookPanel.activeSelf, this is accurate
    /// even though the panel's GameObject stays active at all times for video pre-warming.</summary>
    public bool IsOpen { get; private set; }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Start()
    {
        // Start visually hidden but active, so VideoPlayers can pre-warm on scene load!
        SetBookPanelVisible(false);

        if (RightButton != null) RightButton.onClick.AddListener(NextPage);
        if (LeftButton != null) LeftButton.onClick.AddListener(PreviousPage);
        if (CancelButton != null) CancelButton.onClick.AddListener(CloseBook);
        if (OpenBookButton != null) OpenBookButton.onClick.AddListener(OpenBook);
    }

    public void OpenBook()
    {
        if (pages == null || pages.Length == 0) return;

        SetBookPanelVisible(true);
        currentIndex = 0;
        UpdatePage();

        SetHiddenObjectsActive(false);
    }

    public void CloseBook()
    {
        SetBookPanelVisible(false);
        SetHiddenObjectsActive(true);
    }

    private void SetBookPanelVisible(bool visible)
    {
        IsOpen = visible;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = visible ? 1f : 0f;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
        else if (bookPanel != null)
        {
            bookPanel.SetActive(visible);
        }
    }

    private void SetHiddenObjectsActive(bool active)
    {
        if (objectsToHide == null) return;
        foreach (var obj in objectsToHide)
        {
            if (obj != null) obj.SetActive(active);
        }
    }

    public void NextPage()
    {
        if (pages == null || currentIndex >= pages.Length - 1) return;

        if (pages[currentIndex] != null) pages[currentIndex].SetActive(false);
        currentIndex++;
        UpdatePage();
    }

    public void PreviousPage()
    {
        if (pages == null || currentIndex <= 0) return;

        if (pages[currentIndex] != null) pages[currentIndex].SetActive(false);
        currentIndex--;
        UpdatePage();
    }

    private void UpdatePage()
    {
        if (pages == null || currentIndex < 0 || currentIndex >= pages.Length) return;
        if (pages[currentIndex] != null) pages[currentIndex].SetActive(true);
    }
}
