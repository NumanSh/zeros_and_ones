using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using ZerosAndOnes.Managers;
using ZerosAndOnes.UI;

namespace ZerosAndOnes.EditorScripts
{
    public static class SetupMainMenu
    {
        [MenuItem("Zeros & Ones/Setup Main Menu Scene")]
        public static void SetupScene()
        {
            string scenePath = "Assets/Scenes/MainMenu.unity";
            
            Scene scene;
            if (!System.IO.File.Exists(scenePath))
            {
                scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            }
            else
            {
                scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            }

            // Setup Camera if not exists
            if (Object.FindAnyObjectByType<Camera>() == null)
            {
                GameObject cameraObj = new GameObject("Main Camera");
                Camera cam = cameraObj.AddComponent<Camera>();
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = new Color(0.04f, 0.04f, 0.04f); // #0a0a0a
            }

            // Setup GameManager
            if (Object.FindAnyObjectByType<GameManager>() == null)
            {
                GameObject gmObj = new GameObject("GameManager");
                gmObj.AddComponent<GameManager>();
            }
            
            // Setup UI Document & Controller
            MainMenuController controller = Object.FindAnyObjectByType<MainMenuController>();
            UIDocument uiDoc;
            GameObject uiObj;

            if (controller == null)
            {
                uiObj = new GameObject("MainMenuUI");
                uiDoc = uiObj.AddComponent<UIDocument>();
                uiObj.AddComponent<MainMenuController>();
            }
            else
            {
                uiObj = controller.gameObject;
                uiDoc = uiObj.GetComponent<UIDocument>();
                if (uiDoc == null) uiDoc = uiObj.AddComponent<UIDocument>();
            }

            // Setup Panel Settings
            string settingsPath = "Assets/UI/MainPanelSettings.asset";
            PanelSettings panelSettings = AssetDatabase.LoadAssetAtPath<PanelSettings>(settingsPath);
            
            if (panelSettings == null)
            {
                panelSettings = ScriptableObject.CreateInstance<PanelSettings>();
                panelSettings.scaleMode = PanelScaleMode.ScaleWithScreenSize;
                panelSettings.referenceResolution = new Vector2Int(1920, 1080);
                panelSettings.screenMatchMode = PanelScreenMatchMode.MatchWidthOrHeight;
                panelSettings.match = 0.5f;
                
                AssetDatabase.CreateAsset(panelSettings, settingsPath);
                AssetDatabase.SaveAssets();
                Debug.Log("[Zeros & Ones] Created default Panel Settings.");
            }
            
            uiDoc.panelSettings = panelSettings;
            
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/MainMenu.uxml");
            if (visualTree != null)
            {
                uiDoc.visualTreeAsset = visualTree;
            }
            else
            {
                Debug.LogWarning("MainMenu.uxml not found at Assets/UI/MainMenu.uxml");
            }

            EditorSceneManager.SaveScene(scene, scenePath);
            Debug.Log("[Zeros & Ones] MainMenu scene setup complete and saved.");
            
            // Add to build settings
            EditorBuildSettingsScene[] originalBuildSettings = EditorBuildSettings.scenes;
            bool found = false;
            foreach (var s in originalBuildSettings)
            {
                if (s.path == scenePath) found = true;
            }
            if (!found)
            {
                var newSettings = new EditorBuildSettingsScene[originalBuildSettings.Length + 1];
                System.Array.Copy(originalBuildSettings, newSettings, originalBuildSettings.Length);
                newSettings[originalBuildSettings.Length] = new EditorBuildSettingsScene(scenePath, true);
                
                // Move it to index 0
                for(int i = newSettings.Length - 1; i > 0; i--)
                {
                    var temp = newSettings[i];
                    newSettings[i] = newSettings[i-1];
                    newSettings[i-1] = temp;
                }
                
                EditorBuildSettings.scenes = newSettings;
                Debug.Log("[Zeros & Ones] Added MainMenu scene to Build Settings at index 0.");
            }
        }
    }
}
