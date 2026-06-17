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

        private void Update()
        {
            // Option B: Check collision status during the Update loop
            if (_isPlayerInside && !_isTransitioning)
            {
                _isTransitioning = true;
                string sceneName = GetSceneNameForGate(gateType);
                
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
            }
        }

        private string GetSceneNameForGate(LogicGates_Type type)
        {
            switch (type)
            {
                case LogicGates_Type.Xor: 
                    return "Xor_scene";
                case LogicGates_Type.And: 
                    return "and_scene";
                case LogicGates_Type.Or: 
                    return "Or_scenes";
                case LogicGates_Type.And16:
                    return "And16";
                case LogicGates_Type.Not16:
                    return "Not16";
                case LogicGates_Type.Or16:
                    return "Or16";
                case LogicGates_Type.HalfAdder:
                    return "HalfAdder";
                case LogicGates_Type.FullAdder:
                    return "FullAdder";
                case LogicGates_Type.Add16:
                    return "Add16";
                case LogicGates_Type.Inc16:
                    return "Inc16";
                default: 
                    return type.ToString(); // Fallback
            }
        }
    }
}
