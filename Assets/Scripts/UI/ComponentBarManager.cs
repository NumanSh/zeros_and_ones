using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ComponentBarManager : MonoBehaviour
{
    public static Dictionary<LogicGates_Type, bool> SolvedComponents = new Dictionary<LogicGates_Type, bool>();

    [System.Serializable]
    public struct ComponentUI
    {
        public LogicGates_Type componentType; 
        public GameObject uiElement;         
    }

    [Header("logic gates list")]
    public List<ComponentUI> allComponentsUI;

    [Header("buttons")]
    public Button nextButton;
    public Button prevButton; 

    public List<GameObject> activeSolvedElements = new List<GameObject>(); 
    private int currentPage = 0; 
    private const int ITEMS_PER_PAGE = 3; 

    void Start()
    {

        InitializeComponents();

        SolvedComponents = SaveManager.GetSavedLogicGates();
        SolvedComponents[LogicGates_Type.And] = true;
        SolvedComponents[LogicGates_Type.Not] = true;
        // SolvedComponents[LogicGates_Type.Or] = true;
        // SaveManager.SaveLogicGates(SolvedComponents);
        // SolvedComponents[LogicGates_Type.Xor] = true;
        // SolvedComponents[LogicGates_Type.Not16] = true;
        // SolvedComponents[LogicGates_Type.Add16] = true;
        // SolvedComponents[LogicGates_Type.And16] = true;
        // SolvedComponents[LogicGates_Type.Inc16] = true;
        // SolvedComponents[LogicGates_Type.HalfAdder] = true;
        // SolvedComponents[LogicGates_Type.Or16] = true;
        // SolvedComponents[LogicGates_Type.FullAdder] = true;
        // SolvedComponents[LogicGates_Type.Mux] = true;
        // SolvedComponents[LogicGates_Type.Mux16] = true;
        // SolvedComponents[LogicGates_Type.Mux4Way16] = true;
        // SolvedComponents[LogicGates_Type.Mux8Way16] = true;
        // SolvedComponents[LogicGates_Type.Dmux] = true;
        // SolvedComponents[LogicGates_Type.Dmux4Way] = true;
        // SolvedComponents[LogicGates_Type.Dmux8Way] = true;
        // SolvedComponents[LogicGates_Type.Or4Way] = true;
        // SaveManager.SaveLogicGates(SolvedComponents);
        if (nextButton != null) nextButton.onClick.AddListener(NextPage);
        if (prevButton != null) prevButton.onClick.AddListener(PreviousPage);
        
        UpdateBarPages();
    }

    void InitializeComponents()
    {
        if (SolvedComponents.Count == 0)
        {
            foreach (LogicGates_Type gate in System.Enum.GetValues(typeof(LogicGates_Type)))
            {
                SolvedComponents.Add(gate, false);
            }
        }
    }

    public void UpdateBarPages()
    {
        activeSolvedElements.Clear();
        foreach (var comp in allComponentsUI)
        {
            comp.uiElement.SetActive(false); 

            if (SolvedComponents.ContainsKey(comp.componentType) && SolvedComponents[comp.componentType])
            {
                activeSolvedElements.Add(comp.uiElement);
            }
        }

        int startIndex = currentPage * ITEMS_PER_PAGE;
        int endIndex = startIndex + ITEMS_PER_PAGE;

        for (int i = startIndex; i < endIndex; i++)
        {
            if (i < activeSolvedElements.Count)
            {
                activeSolvedElements[i].SetActive(true);
            }
        }

        if (prevButton != null) prevButton.gameObject.SetActive(currentPage > 0);
        if (nextButton != null) nextButton.gameObject.SetActive(endIndex < activeSolvedElements.Count);
    }

    public void NextPage()
    {
        if ((currentPage + 1) * ITEMS_PER_PAGE < activeSolvedElements.Count)
        {
            currentPage++;
            UpdateBarPages();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateBarPages();
        }
    }

    public static void MarkComponentAsSolved(LogicGates_Type gateType)
    {
        if (SolvedComponents.ContainsKey(gateType))
        {
            SolvedComponents[gateType] = true;
        }
    }
}
