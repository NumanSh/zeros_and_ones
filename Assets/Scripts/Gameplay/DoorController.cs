using UnityEngine;
using UnityEngine.SceneManagement;
using ZerosAndOnes.Gameplay;
using ZerosAndOnes.Managers;

namespace ZerosAndOnes.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class DoorController : MonoBehaviour
    {
        [Header("Door Settings")]
        public LogicGates_Type gateType;
        
        private Collider2D _collider;
        private bool _isPlayerInside = false;
        private bool _isTransitioning = false;
        private bool _wasLastInteraction = false;
        public GameObject door; // Reference to the door GameObject
        private void Awake()
        {
            // Auto-assign tag "door"
            try
            {
                gameObject.tag = "door";
            }
            catch (System.Exception)
            {
                Debug.LogWarning("[DoorController] Tag 'door' is not defined in the Unity project. Please add the 'door' tag in Tag Manager.");
            }

            _collider = GetComponent<Collider2D>();
            _collider.isTrigger = true;
        }

        private void Start()
        {
            // if (ComponentBarManager.SolvedComponents.Count != 0)
            // {
            //     foreach (LogicGates_Type gate in System.Enum.GetValues(typeof(LogicGates_Type)))
            //     {
            //         if (gate == gateType && ComponentBarManager.SolvedComponents[gate])
            //         {
            //             if (door != null)
            //             {
            //                 door.SetActive(false);
            //             }
            //             else
            //             {
            //                 Debug.LogWarning("[DoorController] Door GameObject reference is not set. Please assign it in the inspector.");
            //             }
            //             door.SetActive(false);
            //         }
            //     }
            // }
            // استخدام TryValue يتحقق من وجود المفتاح ومن قيمته في نفس الوقت وبأمان تام
            if (ComponentBarManager.SolvedComponents.TryGetValue(gateType, out bool isSolved) && isSolved)
            {
                if (door != null)
                {
                    door.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("[DoorController] Door GameObject reference is not set. Please assign it in the inspector.");
                }
            }
            // Check if we just came back from the scene this door loads
            string lastScene = ExitButton.LastSceneName;
            string thisGateScene = GetSceneNameForGate(gateType);
            if (!string.IsNullOrEmpty(lastScene) && lastScene == thisGateScene)
            {
                _wasLastInteraction = true;
            }
        }

        private void Update()
        {
            // This triggers on proximity alone, so pausing inside a door trigger would otherwise
            // load the gate scene on the very next frame.
            if (UI.PauseMenuController.IsPaused) return;

            // Option B: Check collision status during the Update loop
            if (_isPlayerInside && !_isTransitioning && !_wasLastInteraction)
            {
                string sName = SceneManager.GetActiveScene().name;
                _isTransitioning = true;
                string sceneName = GetSceneNameForGate(gateType);
                if (gateType == LogicGates_Type.Or4Way && sName == "firstMap")
                {
                    sceneName = "SecondMap";
                    Debug.Log($"[DoorController] Player touched door of type {gateType}. Transitioning to scene: {sceneName}");
                }
                if (gateType == LogicGates_Type.Or4Way && sName == "SecondMap")
                {
                    sceneName = "firstMap";
                    Debug.Log($"[DoorController] Player touched door of type {gateType}. Transitioning to scene: {sceneName}");
                }
                else
                {
                    Debug.Log($"[DoorController] Player touched door of type {gateType}. Transitioning to scene: {sceneName}");
                }
                ExitButton.LastExplorationMap = SceneManager.GetActiveScene().name;

                Debug.Log($"[DoorController] Player touched door of type {gateType}. Transitioning to scene: {sceneName}");

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.LoadScene(sceneName);
                }
                else
                {
                    SceneManager.LoadScene(sceneName);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") || other.GetComponent<PlayerController>() != null)
            {
                _isPlayerInside = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player") || other.GetComponent<PlayerController>() != null)
            {
                _isPlayerInside = false;
                _wasLastInteraction = false; // Reset protection once the player walks away from the door
            }
        }

        private string GetSceneNameForGate(LogicGates_Type type) => GetSceneNameForGateType(type);

        public static string GetSceneNameForGateType(LogicGates_Type type)
        {
            switch (type)
            {
                case LogicGates_Type.Xor: 
                    return "Xor_Scene";
                case LogicGates_Type.And: 
                    return "And_Scene";
                case LogicGates_Type.Or: 
                    return "Or_Scene";
                case LogicGates_Type.And16:
                    return "And16_Scene";
                case LogicGates_Type.Not16:
                    return "Not16_Scene";
                case LogicGates_Type.Or16:
                    return "Or16_Scene";
                case LogicGates_Type.HalfAdder:
                    return "HalfAdder_Scene";
                case LogicGates_Type.FullAdder:
                    return "FullAdder_Scene";
                case LogicGates_Type.Add16:
                    return "Add16_Scene";
                case LogicGates_Type.Inc16:
                    return "Inc16_Scene";
                case LogicGates_Type.Or4Way:
                    return "Or8Way_Scene";
                case LogicGates_Type.Mux:
                    return "Mux_Scene";
                case LogicGates_Type.Mux16:
                    return "Mux16_Scene";
                case LogicGates_Type.Mux4Way16:
                    return "Mux16_4Way_Scene";
                case LogicGates_Type.Mux8Way16:
                    return "Mux16_8Way_Scene";
                case LogicGates_Type.Dmux:
                    return "Dmux_Scene";
                case LogicGates_Type.Dmux4Way:
                    return "Dmux4Way_Scene";
                case LogicGates_Type.Dmux8Way:
                    return "Dmux8Way_Scene";
                default:
                    return type.ToString(); // Fallback
            }
        }
    }
}
