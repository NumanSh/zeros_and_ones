using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using ZerosAndOnes.Core;
using ZerosAndOnes.MainMenu;

namespace ZerosAndOnes.EditorScripts
{
    public static class SetupMainMenu
    {
        [MenuItem("Zeros & Ones/Setup Main Menu Scene")]
        public static void SetupScene()
        {
            string scenePath = "Assets/_Project/Scenes/MainMenu.unity";
            
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
            if (Object.FindAnyObjectByType<MainMenuController>() == null)
            {
                GameObject uiObj = new GameObject("MainMenuUI");
                var uiDoc = uiObj.AddComponent<UIDocument>();
                
                var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Project/UI/MainMenu.uxml");
                if (visualTree != null)
                {
                    uiDoc.visualTreeAsset = visualTree;
                }
                else
                {
                    Debug.LogWarning("MainMenu.uxml not found. Make sure it exists at Assets/_Project/UI/MainMenu.uxml");
                }
                
                uiObj.AddComponent<MainMenuController>();
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
