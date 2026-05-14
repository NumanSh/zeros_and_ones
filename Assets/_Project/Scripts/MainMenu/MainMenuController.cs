using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using ZerosAndOnes.Core;

namespace ZerosAndOnes.MainMenu
{
    [RequireComponent(typeof(UIDocument))]
    public class MainMenuController : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private Label _lblComingSoon;
        private Coroutine _comingSoonCoroutine;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            var root = _uiDocument.rootVisualElement;

            if (root == null)
            {
                Debug.LogError("[MainMenuController] UIDocument has no root VisualElement. Is the UXML attached?");
                return;
            }

            var btnNewGame = root.Q<Button>("BtnNewGame");
            var btnLoadGame = root.Q<Button>("BtnLoadGame");
            var btnSettings = root.Q<Button>("BtnSettings");
            var btnExit = root.Q<Button>("BtnExit");
            
            _lblComingSoon = root.Q<Label>("LblComingSoon");

            if (btnNewGame != null) btnNewGame.clicked += OnNewGameClicked;
            if (btnLoadGame != null) btnLoadGame.clicked += OnLoadGameClicked;
            if (btnSettings != null) btnSettings.clicked += OnSettingsClicked;
            if (btnExit != null) btnExit.clicked += OnExitClicked;
        }

        private void OnNewGameClicked()
        {
            Debug.Log("[MainMenuController] New Game clicked");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadGameplayScene();
            }
            else
            {
                Debug.LogWarning("[MainMenuController] GameManager not found!");
            }
        }

        private void OnLoadGameClicked()
        {
            Debug.Log("[MainMenuController] Load Game clicked");
            ShowComingSoonMessage();
        }

        private void OnSettingsClicked()
        {
            Debug.Log("[MainMenuController] Settings clicked");
            ShowComingSoonMessage();
        }

        private void OnExitClicked()
        {
            Debug.Log("[MainMenuController] Exit clicked");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.QuitGame();
            }
            else
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }

        private void ShowComingSoonMessage()
        {
            if (_lblComingSoon == null) return;

            if (_comingSoonCoroutine != null)
            {
                StopCoroutine(_comingSoonCoroutine);
            }
            
            _comingSoonCoroutine = StartCoroutine(ShowComingSoonRoutine());
        }

        private IEnumerator ShowComingSoonRoutine()
        {
            _lblComingSoon.AddToClassList("visible");
            yield return new WaitForSeconds(2.0f);
            _lblComingSoon.RemoveFromClassList("visible");
        }
    }
}
