using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ButtonController3bit : MonoBehaviour
{
    public  ConnectorType Name;
    public GameObject bitSelectionPanel;
    public Vector3 panelOffset = new Vector3(100f, 0f, 0f);
    [SerializeField] public List<Cable16bitTruthTable> truthTable = new List<Cable16bitTruthTable>(){};
    [SerializeField] public Button myButton;
    // [SerializeField] public Button myButton1;
    // [SerializeField] public Button myButton2;
    [SerializeField] private GameObject prefabToSpawnCable;
    [SerializeField] private GameObject prefabToSpawnCable16;
    private List<Cable> spawnedCables = new List<Cable>(new Cable[3]);
    [SerializeField] private List<Button> bitButtons = new List<Button>(){};
    private Cable16bit spawnedCable16 = null;
    public GameState currentState;
    
    public void SetcurrentState( GameState state)
    {
        currentState=state;
        Debug.Log($"the button in currentState {currentState} dose not exist");
    }
    void Start()
    {
        // currentState = GameState.AddingCable;
        // bitSelectionPanel.SetActive(false);
        for (int i=0;i<bitButtons.Count;i++)
        {
            if (bitButtons[i] != null)
            {
                bitButtons[i].onClick.AddListener(() => OnBitSelected2(i));
            }
            else
            {
                Debug.Log($"the button in index {i} dose not exist");
            }
        }
        myButton.onClick.AddListener(OnBitSelectedAll);
    }

    public void ShowBitSelectionUI()
    {
        Debug.Log($"sameh16 hon bas ");
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        bitSelectionPanel.SetActive(true);
        Debug.Log($"New cable active: {bitSelectionPanel.activeSelf}");
        bitSelectionPanel.transform.position = screenPos + panelOffset;
    }

    public void Creat_A_Cable(int bitIndex)
    {
        if (spawnedCables[bitIndex] != null)
        {
            Debug.Log($"i have a cable on {bitIndex} button");
            Cable existingCable = spawnedCables[bitIndex];
            existingCable.SetDragging(true);
            existingCable.SetIsSelected(true);
            // if (existingCable.getCableManager() != null)
            // {
            //     bitButtons[bitIndex].interactable = false;
            // }
            // Debug.Log("dragging");
        }
        else
        {
            if (prefabToSpawnCable != null)
            {
                
                GameObject newCable = Instantiate(prefabToSpawnCable, transform.position, transform.rotation, transform.parent);
                // Debug.Log(newCable.transform.position);
                // Debug.Log("EndA Pos: " + transform.position);
                newCable.transform.localScale = transform.localScale;
                Transform cable1Transform = newCable.transform.Find("cable1");
                
                // Debug.Log("added");

                if (cable1Transform != null)
                {
                    Cable cableScript = cable1Transform.GetComponent<Cable>();
                    if (cableScript != null)
                    {
                        spawnedCables[bitIndex] = cableScript;
                        // cableScript.SetTruthTable(truthTable[bitIndex].truthTable);
                        // Debug.Log($"sameh {spawnedCables[bitIndex] == null}");
                        // spawnedCables[bitIndex].SetTruthTable(truthTable[bitIndex].truthTable);
                        // Debug.Log("  "+bitIndex +" "+ string.Join(", ", truthTable[bitIndex].truthTable));
                        cableScript.SetDragging(true);
                        cableScript.SetIsSelected(true);
                        // if (cableScript.getCableManager() != null )
                        // {
                        //     bitButtons[bitIndex].interactable = false;
                        //     cableScript.SetButton_cable( bitButtons[bitIndex]);
                        // }
                        // Debug.Log("dragging");
                    }
                    else
                    {
                        Debug.LogError("i did not find a cable1");
                    }
                }
                else
                {
                    Debug.LogError("i cant find a script of cable");
                }
            }
        }
        bitSelectionPanel.SetActive(false);
    }

    public void SetTruthTable(List<Cable16bitTruthTable> newTruthTable)
    {

        truthTable.Clear();
        foreach (var item in newTruthTable)
        {
            truthTable.Add(new Cable16bitTruthTable(new List<bool>(item.truthTable))); 
        }

    }

    public List<Cable16bitTruthTable> GetTruthTable()
    {
        return truthTable;
    }

    public void OnBitSelectedAll()
    {
        Debug.Log("Selected bit: 16");
        if (spawnedCable16 != null)
        {
            Debug.Log($"i have a cable on all bits button");
            Cable16bit existingCable = spawnedCable16;
            existingCable.SetDragging(true);
            existingCable.SetIsSelected(true);
            if (existingCable.getCableManager() != null)
            {
                myButton.interactable = false;
            }
            // Debug.Log("dragging");
        }
        else
        {
            if (prefabToSpawnCable16 != null)
            {
                GameObject newCable = Instantiate(prefabToSpawnCable16, transform.position, transform.rotation, transform);
                Debug.Log("added");

                Transform cable1Transform = newCable.transform.Find("cable1");
                if (cable1Transform != null)
                {
                    Cable16bit cableScript = cable1Transform.GetComponent<Cable16bit>();
                    if (cableScript != null)
                    {
                        spawnedCable16 = cableScript;
                        Debug.Log($"sameh {myButton == null}");
                        cableScript.SetDragging(true);
                        cableScript.SetTruthTable(truthTable);
                        cableScript.SetIsSelected(true);
                        if (cableScript.getCableManager() != null )
                        {
                            myButton.interactable = false;
                        }
                        // Debug.Log("dragging");
                    }
                    else
                    {
                        Debug.LogError("i did not find a cable1");
                    }
                }
                else
                {
                    Debug.LogError("i cant find a script of cable");
                }
            }
        }
        bitSelectionPanel.SetActive(false);
        
    }

    public void OnBitSelected2(int bitIndex)
    {
        // Debug.Log($"Selected bit: {bitIndex}");
        // Creat_A_Cable(2);
    }
    public void OnBitSelected(int bitIndex)
    {
        Debug.Log($"GameState {currentState}");
        if( bitIndex >=0 && bitIndex <=2 && currentState == GameState.SelectingBit)
        {
            Debug.Log($"Selected bit: {bitIndex}");
            Creat_A_Cable(bitIndex);
            SetcurrentState(GameState.AddingCable);
        }
    }

    void OnMouseDown()
    {
        ShowBitSelectionUI();
    }

    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     if (Physics.Raycast(ray, out RaycastHit hitInfo))
        //     {
        //         Canvas hitCanvas = hitInfo.collider.gameObject.GetComponentInParent<Canvas>();
        //         if (hitCanvas != null && hitCanvas == canvas)
        //         {
        //             ShowBitSelectionUI();
        //         }
        //     }
        // }
        if (bitSelectionPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0) && bitButtons[0] != null && bitButtons[0].interactable )
            {
                // bitButtons[0].onClick.Invoke();
                Debug.Log("الكائن ظهر! 0");
                bitSelectionPanel.SetActive(false);
                bitButtons[0].interactable=false;
                OnBitSelected(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1) && bitButtons[1] != null && bitButtons[1].interactable)
            {
                // bitButtons[1].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                bitButtons[1].interactable=false;
                OnBitSelected(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && bitButtons[2] != null && bitButtons[2].interactable)
            {
                // bitButtons[1].onClick.Invoke();
                bitSelectionPanel.SetActive(false);
                bitButtons[2].interactable=false;
                OnBitSelected(2);
            }
            
        }



    }

}