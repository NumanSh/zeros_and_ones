using UnityEngine;

/// <summary>
/// Shows the Instructions/Hints tab buttons only while one of the two book panels is open.
/// Attach to an always-active object (NOT the tabs container itself, since that would stop
/// this script from running once it's hidden).
/// </summary>
public class BookTabsVisibility : MonoBehaviour
{
    [Tooltip("Parent GameObject containing the Instructions/Hints tab buttons.")]
    [SerializeField] private GameObject tabsContainer;

    [SerializeField] private HintBook hintBook;
    [SerializeField] private InstructionBook instructionBook;

    private void Update()
    {
        if (tabsContainer == null) return;

        bool eitherBookOpen =
            (hintBook != null && hintBook.IsOpen) ||
            (instructionBook != null && instructionBook.IsOpen);

        if (tabsContainer.activeSelf != eitherBookOpen)
        {
            tabsContainer.SetActive(eitherBookOpen);
        }
    }
}
