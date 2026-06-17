using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using ZerosAndOnes.Gameplay;

namespace ZerosAndOnes.EditorScripts
{
    public static class SetupHeroAnimations
    {
        [MenuItem("ZerosAndOnes/Configure Hero (firstMap)")]
        public static void ConfigureHero()
        {
            Debug.Log("[SetupHeroAnimations] Starting Hero configuration...");

            // 1. Define paths
            string scenePath = "Assets/Scenes/firstMap.unity";
            string animationsDir = "Assets/Animations/Hero";
            string controllerPath = Path.Combine(animationsDir, "HeroController.controller");
            
            string idleSpritesPath = "Assets/Assits_imported/BayatGames/Free Platform Game Assets/Character/Character Animation ( Update 1.8 )/Idle/1x.png";
            string runSpritesPath = "Assets/Assits_imported/BayatGames/Free Platform Game Assets/Character/Character Animation ( Update 1.8 )/Run/1x.png";
            string jumpSpritesPath = "Assets/Assits_imported/BayatGames/Free Platform Game Assets/Character/Character Animation ( Update 1.8 )/Jump/1x.png";

            // 2. Open firstMap scene (only if not already open to preserve active unsaved edits)
            if (!File.Exists(scenePath))
            {
                Debug.LogError($"[SetupHeroAnimations] Scene not found at: {scenePath}");
                return;
            }

            var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (activeScene.path != scenePath)
            {
                activeScene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                Debug.Log($"[SetupHeroAnimations] Opened scene from disk: {activeScene.name}");
            }
            else
            {
                Debug.Log($"[SetupHeroAnimations] Using already active scene: {activeScene.name}");
            }

            // 3. Ensure animation directories exist
            if (!AssetDatabase.IsValidFolder("Assets/Animations"))
            {
                AssetDatabase.CreateFolder("Assets", "Animations");
            }
            if (!AssetDatabase.IsValidFolder(animationsDir))
            {
                AssetDatabase.CreateFolder("Assets/Animations", "Hero");
            }

            // 4. Load sprites from Sheets
            Sprite[] idleSprites = LoadSpritesFromSheet(idleSpritesPath);
            Sprite[] runSprites = LoadSpritesFromSheet(runSpritesPath);
            Sprite[] jumpSprites = LoadSpritesFromSheet(jumpSpritesPath);

            if (idleSprites == null || idleSprites.Length == 0)
            {
                Debug.LogError($"[SetupHeroAnimations] No sprites found at: {idleSpritesPath}");
                return;
            }
            if (runSprites == null || runSprites.Length == 0)
            {
                Debug.LogError($"[SetupHeroAnimations] No sprites found at: {runSpritesPath}");
                return;
            }
            if (jumpSprites == null || jumpSprites.Length == 0)
            {
                Debug.LogError($"[SetupHeroAnimations] No sprites found at: {jumpSpritesPath}");
                return;
            }

            Debug.Log($"[SetupHeroAnimations] Loaded {idleSprites.Length} Idle sprites, {runSprites.Length} Run sprites, {jumpSprites.Length} Jump sprites.");

            // 5. Create Animation Clips
            // Idle Clip (Using Blink sprites or Idle 1x.png)
            AnimationClip idleClip = CreateSpriteAnimationClip(Path.Combine(animationsDir, "Hero_Idle.anim"), idleSprites, 12f, true);
            
            // Run Clip
            AnimationClip runClip = CreateSpriteAnimationClip(Path.Combine(animationsDir, "Hero_Run.anim"), runSprites, 18f, true);
            
            // Jump Up Clip (using first frame of Jump sheet)
            AnimationClip jumpUpClip = CreateSpriteAnimationClip(Path.Combine(animationsDir, "Hero_JumpUp.anim"), new[] { jumpSprites[0] }, 1f, false);
            
            // Fall Down Clip (using second frame of Jump sheet, if available, else first frame)
            Sprite fallSprite = jumpSprites.Length > 1 ? jumpSprites[1] : jumpSprites[0];
            AnimationClip fallDownClip = CreateSpriteAnimationClip(Path.Combine(animationsDir, "Hero_FallDown.anim"), new[] { fallSprite }, 1f, false);

            // 6. Create or setup Animator Controller
            AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
            AnimatorStateMachine stateMachine = controller.layers[0].stateMachine;

            // Add parameters
            controller.AddParameter("isRunning", AnimatorControllerParameterType.Bool);
            controller.AddParameter("isGrounded", AnimatorControllerParameterType.Bool);
            controller.AddParameter("isClimbing", AnimatorControllerParameterType.Bool);
            controller.AddParameter("verticalVelocity", AnimatorControllerParameterType.Float);

            // Add States
            AnimatorState idleState = stateMachine.AddState("Idle");
            idleState.motion = idleClip;

            AnimatorState runState = stateMachine.AddState("Run");
            runState.motion = runClip;

            // Air blend tree state
            AnimatorState airState = stateMachine.AddState("Air");
            BlendTree blendTree = new BlendTree();
            blendTree.name = "AirBlendTree";
            blendTree.blendParameter = "verticalVelocity";
            blendTree.blendType = BlendTreeType.Simple1D;
            blendTree.AddChild(fallDownClip, -0.1f);
            blendTree.AddChild(jumpUpClip, 0.1f);

            AssetDatabase.AddObjectToAsset(blendTree, controller);
            airState.motion = blendTree;

            // Transitions
            // Idle -> Run (isRunning = true)
            var tIdleToRun = idleState.AddTransition(runState);
            tIdleToRun.AddCondition(AnimatorConditionMode.If, 0, "isRunning");
            tIdleToRun.hasExitTime = false;
            tIdleToRun.duration = 0f;

            // Run -> Idle (isRunning = false)
            var tRunToIdle = runState.AddTransition(idleState);
            tRunToIdle.AddCondition(AnimatorConditionMode.IfNot, 0, "isRunning");
            tRunToIdle.hasExitTime = false;
            tRunToIdle.duration = 0f;

            // Idle -> Air (isGrounded = false)
            var tIdleToAir = idleState.AddTransition(airState);
            tIdleToAir.AddCondition(AnimatorConditionMode.IfNot, 0, "isGrounded");
            tIdleToAir.hasExitTime = false;
            tIdleToAir.duration = 0f;

            // Run -> Air (isGrounded = false)
            var tRunToAir = runState.AddTransition(airState);
            tRunToAir.AddCondition(AnimatorConditionMode.IfNot, 0, "isGrounded");
            tRunToAir.hasExitTime = false;
            tRunToAir.duration = 0f;

            // Air -> Idle (isGrounded = true, isRunning = false)
            var tAirToIdle = airState.AddTransition(idleState);
            tAirToIdle.AddCondition(AnimatorConditionMode.If, 0, "isGrounded");
            tAirToIdle.AddCondition(AnimatorConditionMode.IfNot, 0, "isRunning");
            tAirToIdle.hasExitTime = false;
            tAirToIdle.duration = 0f;

            // Air -> Run (isGrounded = true, isRunning = true)
            var tAirToRun = airState.AddTransition(runState);
            tAirToRun.AddCondition(AnimatorConditionMode.If, 0, "isGrounded");
            tAirToRun.AddCondition(AnimatorConditionMode.If, 0, "isRunning");
            tAirToRun.hasExitTime = false;
            tAirToRun.duration = 0f;

            AssetDatabase.SaveAssets();
            Debug.Log($"[SetupHeroAnimations] Animator Controller created at: {controllerPath}");

            // 7. Find and delete duplicate/leftover static character GameObjects named "1x_0"
            int deletedCount = 0;
            foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (go.scene.name == "firstMap" && go.name.Equals("1x_0", System.StringComparison.OrdinalIgnoreCase))
                {
                    Undo.DestroyObjectImmediate(go);
                    deletedCount++;
                }
            }
            if (deletedCount > 0)
            {
                Debug.Log($"[SetupHeroAnimations] Deleted {deletedCount} duplicate static character GameObject(s) named '1x_0'.");
            }

            // Find Player GameObject in the scene
            GameObject playerObj = null;
            foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (go.scene.name == "firstMap" && go.name.Equals("player", System.StringComparison.OrdinalIgnoreCase))
                {
                    playerObj = go;
                    break;
                }
            }

            if (playerObj == null)
            {
                Debug.LogError("[SetupHeroAnimations] Player GameObject with name 'player' was not found in the scene.");
                return;
            }

            Undo.RecordObject(playerObj, "Configure Hero Components");

            // Rigidbody2D
            Rigidbody2D rb = playerObj.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = playerObj.AddComponent<Rigidbody2D>();
            }
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 3f;

            // BoxCollider2D (adjust for player size)
            BoxCollider2D col = playerObj.GetComponent<BoxCollider2D>();
            if (col == null)
            {
                col = playerObj.AddComponent<BoxCollider2D>();
            }
            col.size = new Vector2(0.7f, 0.9f);
            col.offset = new Vector2(0f, 0f);

            // GroundCheckPoint child
            Transform groundCheck = playerObj.transform.Find("GroundCheckPoint");
            if (groundCheck == null)
            {
                GameObject gcObj = new GameObject("GroundCheckPoint");
                gcObj.transform.SetParent(playerObj.transform);
                groundCheck = gcObj.transform;
            }
            groundCheck.localPosition = new Vector3(0f, -0.46f, 0f);

            // Animator
            Animator animator = playerObj.GetComponent<Animator>();
            if (animator == null)
            {
                animator = playerObj.AddComponent<Animator>();
            }
            animator.runtimeAnimatorController = controller;

            // PlayerController Script
            PlayerController playerScript = playerObj.GetComponent<PlayerController>();
            if (playerScript == null)
            {
                playerScript = playerObj.AddComponent<PlayerController>();
            }

            // Configure fields via SerializedObject
            var serializedPlayer = new SerializedObject(playerScript);
            serializedPlayer.FindProperty("groundCheckPoint").objectReferenceValue = groundCheck;
            
            SpriteRenderer sr = playerObj.GetComponent<SpriteRenderer>();
            if (sr == null)
            {
                sr = playerObj.GetComponentInChildren<SpriteRenderer>();
            }
            serializedPlayer.FindProperty("spriteRenderer").objectReferenceValue = sr;
            serializedPlayer.FindProperty("jumpForce").floatValue = 11f;

            // Set Ground layer mask
            int groundLayerIndex = LayerMask.NameToLayer("Ground");
            if (groundLayerIndex != -1)
            {
                serializedPlayer.FindProperty("groundLayer").intValue = 1 << groundLayerIndex;
            }
            else
            {
                // Fallback to Everything/Default if Ground layer isn't defined yet
                serializedPlayer.FindProperty("groundLayer").intValue = 1; // Default layer
                Debug.LogWarning("[SetupHeroAnimations] 'Ground' layer not found. Please create 'Ground' layer in Tag Manager and set it on your platforms.");
            }

            serializedPlayer.ApplyModifiedProperties();

            // Camera Setup
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                mainCamera = Object.FindAnyObjectByType<Camera>();
            }

            if (mainCamera != null)
            {
                Undo.RecordObject(mainCamera.gameObject, "Configure Camera Controller");
                CameraController2D camCtrl = mainCamera.GetComponent<CameraController2D>();
                if (camCtrl == null)
                {
                    camCtrl = mainCamera.gameObject.AddComponent<CameraController2D>();
                }

                var serializedCam = new SerializedObject(camCtrl);
                serializedCam.FindProperty("target").objectReferenceValue = playerObj.transform;
                serializedCam.FindProperty("useBounds").boolValue = false; // Disable bounds limit so camera tracks player anywhere on firstMap
                serializedCam.ApplyModifiedProperties();
                EditorUtility.SetDirty(mainCamera.gameObject);
                Debug.Log("[SetupHeroAnimations] CameraController2D linked to Player on Main Camera.");
            }
            else
            {
                Debug.LogWarning("[SetupHeroAnimations] Main Camera not found in scene. Could not link CameraController2D.");
            }

            // Audio Setup for "thunders"
            GameObject thunderObj = GameObject.Find("thunders");
            if (thunderObj != null)
            {
                AudioClip thunderClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/sounds/freesound_community-electric-90746.mp3");
                if (thunderClip != null)
                {
                    Undo.RecordObject(thunderObj, "Configure Thunder Audio");
                    AudioSource audioSource = thunderObj.GetComponent<AudioSource>();
                    if (audioSource == null)
                    {
                        audioSource = thunderObj.AddComponent<AudioSource>();
                    }

                    audioSource.clip = thunderClip;
                    audioSource.spatialBlend = 1f; // Make it 3D (Spatialized)
                    audioSource.loop = true;
                    audioSource.playOnAwake = true;
                    audioSource.volume = 0.1f;     // Set a much lower overall volume
                    audioSource.minDistance = 0.5f; // Starts falling off very close
                    audioSource.maxDistance = 2.5f; // Fades out completely within a short range
                    audioSource.rolloffMode = AudioRolloffMode.Linear;

                    EditorUtility.SetDirty(thunderObj);
                    Debug.Log("[SetupHeroAnimations] Configured 3D Spatial AudioSource on 'thunders' GameObject.");
                }
                else
                {
                    Debug.LogWarning("[SetupHeroAnimations] Thunder audio clip not found at: Assets/sounds/freesound_community-electric-90746.mp3");
                }
            }
            else
            {
                Debug.LogWarning("[SetupHeroAnimations] GameObject 'thunders' not found in scene. Could not configure audio.");
            }

            // 8. Automatically add all scenes in the project to the Build Settings
            AddAllScenesToBuildSettings();

            // Save and Mark Scene as Dirty
            EditorUtility.SetDirty(playerObj);
            EditorSceneManager.MarkSceneDirty(activeScene);
            EditorSceneManager.SaveScene(activeScene);

            Debug.Log("[SetupHeroAnimations] Configuration completed successfully! Please check the character in the firstMap scene.");
        }

        private static Sprite[] LoadSpritesFromSheet(string path)
        {
            Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
            if (assets == null || assets.Length == 0) return null;

            List<Sprite> sprites = new List<Sprite>();
            foreach (var asset in assets)
            {
                if (asset is Sprite sprite)
                {
                    sprites.Add(sprite);
                }
            }

            // Sort sprites numerically based on suffix (e.g. 1x_12 -> 12)
            sprites.Sort((a, b) => GetSpriteIndex(a.name).CompareTo(GetSpriteIndex(b.name)));
            return sprites.ToArray();
        }

        private static int GetSpriteIndex(string name)
        {
            int underscore = name.LastIndexOf('_');
            if (underscore != -1 && int.TryParse(name.Substring(underscore + 1), out int index))
            {
                return index;
            }
            return 0;
        }

        private static AnimationClip CreateSpriteAnimationClip(string path, Sprite[] sprites, float frameRate, bool loop)
        {
            AnimationClip clip = new AnimationClip();
            clip.frameRate = frameRate;

            // Set loop setting
            var settings = AnimationUtility.GetAnimationClipSettings(clip);
            settings.loopTime = loop;
            AnimationUtility.SetAnimationClipSettings(clip, settings);

            var binding = new EditorCurveBinding();
            binding.type = typeof(SpriteRenderer);
            binding.path = "";
            binding.propertyName = "m_Sprite";

            ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[sprites.Length];
            for (int i = 0; i < sprites.Length; i++)
            {
                keyframes[i] = new ObjectReferenceKeyframe();
                keyframes[i].time = i * (1f / frameRate);
                keyframes[i].value = sprites[i];
            }

            AnimationUtility.SetObjectReferenceCurve(clip, binding, keyframes);
            AssetDatabase.CreateAsset(clip, path);
            return clip;
        }

        private static void AddAllScenesToBuildSettings()
        {
            // Find all scene files in the Assets folder
            string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets" });
            List<EditorBuildSettingsScene> buildScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

            bool changed = false;
            foreach (string guid in sceneGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (string.IsNullOrEmpty(path)) continue;

                // Check if this scene is already in build settings
                bool alreadyInBuild = false;
                foreach (var s in buildScenes)
                {
                    if (s.path == path)
                    {
                        alreadyInBuild = true;
                        break;
                    }
                }

                if (!alreadyInBuild)
                {
                    buildScenes.Add(new EditorBuildSettingsScene(path, true));
                    Debug.Log($"[SetupHeroAnimations] Added scene to Build Settings: {path}");
                    changed = true;
                }
            }

            if (changed)
            {
                EditorBuildSettings.scenes = buildScenes.ToArray();
            }
        }
    }
}
