using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using ZerosAndOnes.Gameplay;
using ZerosAndOnes.Managers;

namespace ZerosAndOnes.EditorScripts
{
    public static class SetupExplorationMap
    {
        [MenuItem("ZerosAndOnes/Setup Exploration Map Scene")]
        public static void SetupScene()
        {
            string scenePath = "Assets/Scenes/ExplorationMap.unity";
            
            Scene scene;
            if (!System.IO.File.Exists(scenePath))
            {
                scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            }
            else
            {
                scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            }

            // 1. Setup GameManager (persistent across scenes)
            if (Object.FindAnyObjectByType<GameManager>() == null)
            {
                GameObject gmObj = new GameObject("GameManager");
                gmObj.AddComponent<GameManager>();
            }

            // 2. Setup Grid & Tilemaps
            Grid grid = Object.FindAnyObjectByType<Grid>();
            if (grid == null)
            {
                GameObject gridObj = new GameObject("Grid");
                grid = gridObj.AddComponent<Grid>();
            }

            // Configure Layers
            // Ensure tag exists (or handle dynamically if possible)
            // Note: tag "Ladder" and "Player" are standard, but we'll print warnings if not found.

            // Background Tilemap
            Tilemap bgTilemap = GetOrCreateTilemap(grid.transform, "Background", 0);
            
            // Platforms Tilemap
            Tilemap platformsTilemap = GetOrCreateTilemap(grid.transform, "Platforms", 1);
            SetupPlatformsColliders(platformsTilemap.gameObject);

            // Ladders Tilemap
            Tilemap laddersTilemap = GetOrCreateTilemap(grid.transform, "Ladders", 2);
            SetupLaddersColliders(laddersTilemap.gameObject);

            // 3. Setup Player
            PlayerController player = Object.FindAnyObjectByType<PlayerController>();
            GameObject playerObj;
            if (player == null)
            {
                playerObj = new GameObject("Player");
                playerObj.tag = "Player";
                
                // Visuals
                GameObject visualChild = new GameObject("Visual");
                visualChild.transform.SetParent(playerObj.transform);
                visualChild.transform.localPosition = Vector3.zero;
                visualChild.AddComponent<SpriteRenderer>();

                // Physics
                Rigidbody2D rb = playerObj.AddComponent<Rigidbody2D>();
                rb.freezeRotation = true;
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

                BoxCollider2D col = playerObj.AddComponent<BoxCollider2D>();
                col.size = new Vector2(0.8f, 1.8f);

                // Ground Check Point
                GameObject groundCheck = new GameObject("GroundCheckPoint");
                groundCheck.transform.SetParent(playerObj.transform);
                groundCheck.transform.localPosition = new Vector3(0f, -0.95f, 0f);

                player = playerObj.AddComponent<PlayerController>();
                
                // Configure fields
                var serializedPlayer = new SerializedObject(player);
                serializedPlayer.FindProperty("groundCheckPoint").objectReferenceValue = groundCheck.transform;
                serializedPlayer.FindProperty("spriteRenderer").objectReferenceValue = visualChild.GetComponent<SpriteRenderer>();
                serializedPlayer.ApplyModifiedProperties();

                playerObj.transform.position = new Vector3(0f, 2f, 0f);
                Debug.Log("[SetupExplorationMap] Created Player GameObject.");
            }
            else
            {
                playerObj = player.gameObject;
            }

            // 4. Setup Camera
            Camera cam = Object.FindAnyObjectByType<Camera>();
            if (cam == null)
            {
                GameObject camObj = new GameObject("Main Camera");
                camObj.tag = "MainCamera";
                cam = camObj.AddComponent<Camera>();
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = new Color(0.04f, 0.04f, 0.04f);
            }

            CameraController2D camCtrl = cam.GetComponent<CameraController2D>();
            if (camCtrl == null)
            {
                camCtrl = cam.gameObject.AddComponent<CameraController2D>();
            }
            
            // Link target to Player
            var serializedCam = new SerializedObject(camCtrl);
            serializedCam.FindProperty("target").objectReferenceValue = playerObj.transform;
            serializedCam.ApplyModifiedProperties();

            // 5. Setup Portals (NAND, AND, OR)
            CreatePortalIfMissing("Portal_NAND", new Vector3(-5f, 1f, 0f), "NAND_Puzzle");
            CreatePortalIfMissing("Portal_AND", new Vector3(5f, 1f, 0f), "AND_Puzzle");
            CreatePortalIfMissing("Portal_OR", new Vector3(0f, 6f, 0f), "OR_Puzzle");

            // Save the scene
            EditorSceneManager.SaveScene(scene, scenePath);
            Debug.Log("[SetupExplorationMap] ExplorationMap scene setup complete and saved.");

            // 6. Add to Build Settings
            AddSceneToBuildSettings(scenePath);
        }

        private static Tilemap GetOrCreateTilemap(Transform parent, string name, int sortingOrder)
        {
            Transform t = parent.Find(name);
            GameObject go;
            if (t == null)
            {
                go = new GameObject(name);
                go.transform.SetParent(parent);
            }
            else
            {
                go = t.gameObject;
            }

            Tilemap tilemap = go.GetComponent<Tilemap>();
            if (tilemap == null) tilemap = go.AddComponent<Tilemap>();

            TilemapRenderer renderer = go.GetComponent<TilemapRenderer>();
            if (renderer == null) renderer = go.AddComponent<TilemapRenderer>();
            renderer.sortingOrder = sortingOrder;

            return tilemap;
        }

        private static void SetupPlatformsColliders(GameObject go)
        {
            TilemapCollider2D col = go.GetComponent<TilemapCollider2D>();
            if (col == null) col = go.AddComponent<TilemapCollider2D>();

            CompositeCollider2D comp = go.GetComponent<CompositeCollider2D>();
            if (comp == null) comp = go.AddComponent<CompositeCollider2D>();

            col.usedByComposite = true;

            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Static;
            }

            // Try to set Ground layer if it exists
            int groundLayer = LayerMask.NameToLayer("Ground");
            if (groundLayer != -1)
            {
                go.layer = groundLayer;
            }
        }

        private static void SetupLaddersColliders(GameObject go)
        {
            TilemapCollider2D col = go.GetComponent<TilemapCollider2D>();
            if (col == null) col = go.AddComponent<TilemapCollider2D>();
            col.isTrigger = true;

            // Set tag to Ladder if registered
            try
            {
                go.tag = "Ladder";
            }
            catch
            {
                Debug.LogWarning("[SetupExplorationMap] Tag 'Ladder' is not defined in project tags. Please define it.");
            }
        }

        private static void CreatePortalIfMissing(string name, Vector3 position, string sceneTarget)
        {
            GameObject portalObj = GameObject.Find(name);
            if (portalObj == null)
            {
                portalObj = new GameObject(name);
                portalObj.transform.position = position;

                BoxCollider2D col = portalObj.AddComponent<BoxCollider2D>();
                col.isTrigger = true;
                col.size = new Vector2(1.5f, 2.5f);

                PortalController ctrl = portalObj.AddComponent<PortalController>();
                
                var serializedCtrl = new SerializedObject(ctrl);
                serializedCtrl.FindProperty("portalID").stringValue = name;
                serializedCtrl.FindProperty("targetSceneName").stringValue = sceneTarget;
                serializedCtrl.ApplyModifiedProperties();

                Debug.Log($"[SetupExplorationMap] Created portal: {name} targeting scene {sceneTarget}");
            }
        }

        private static void AddSceneToBuildSettings(string scenePath)
        {
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
                
                EditorBuildSettings.scenes = newSettings;
                Debug.Log($"[SetupExplorationMap] Added '{scenePath}' to Build Settings at index {originalBuildSettings.Length}.");
            }
        }
    }
}
