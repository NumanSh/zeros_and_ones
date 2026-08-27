using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using ZerosAndOnes.Gameplay;

public static class FixSecondMapMaterials
{
    private const string SharedMaterialFolder = "Assets/Assits_imported/DungeonFloorsAndWallsSamples";
    private const string OutputFolder = "Assets/Material/SecondMap";

    [MenuItem("Tools/Maps/Give SecondMap Its Own Materials")]
    public static void Run()
    {
        if (EditorSceneNotOpen("SecondMap"))
        {
            Debug.LogError("Open the SecondMap scene before running this.");
            return;
        }

        if (!AssetDatabase.IsValidFolder(OutputFolder))
            Directory.CreateDirectory(OutputFolder);

        var duplicates = new Dictionary<Material, Material>();
        int renderersFixed = 0;

        foreach (var renderer in Object.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None))
        {
            var original = renderer.sharedMaterial;
            if (original == null) continue;

            string originalPath = AssetDatabase.GetAssetPath(original);
            if (string.IsNullOrEmpty(originalPath) || !originalPath.StartsWith(SharedMaterialFolder))
                continue; // not one of the shared package materials, leave it alone

            if (!duplicates.TryGetValue(original, out var copy))
            {
                copy = new Material(original);
                string newPath = $"{OutputFolder}/{original.name}_Map2.mat";
                newPath = AssetDatabase.GenerateUniqueAssetPath(newPath);
                AssetDatabase.CreateAsset(copy, newPath);
                duplicates[original] = copy;
            }

            Undo.RecordObject(renderer, "Reassign SecondMap material");
            renderer.sharedMaterial = copy;
            renderersFixed++;
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Created {duplicates.Count} unique material copies and reassigned them to {renderersFixed} SpriteRenderers in the open scene. " +
                  "FirstMap and the shared package materials were not touched.");
    }

    private static bool EditorSceneNotOpen(string sceneName)
    {
        var scene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
        return !scene.name.Equals(sceneName, System.StringComparison.OrdinalIgnoreCase);
    }

    [MenuItem("Tools/Maps/Color Ground (1) Tiles")]
    public static void ColorGroundOne()
    {
        var ground = GameObject.Find("Ground (1)");
        if (ground == null)
        {
            Debug.LogError("Could not find a GameObject named 'Ground (1)' in the open scene.");
            return;
        }

        if (!ColorUtility.TryParseHtmlString("#B46956", out var color))
        {
            Debug.LogError("Failed to parse color B46956.");
            return;
        }

        var renderers = ground.GetComponentsInChildren<SpriteRenderer>(true);
        Undo.RecordObjects(renderers, "Color Ground (1) Tiles");
        foreach (var renderer in renderers)
            renderer.color = color;

        EditorUtility.SetDirty(ground);
        Debug.Log($"Set color #B46956 on {renderers.Length} SpriteRenderers under 'Ground (1)'.");
    }

    [MenuItem("Tools/Maps/Match Ground (1) Squares To Square (4162)")]
    public static void MatchGroundSquaresToReference()
    {
        var ground = GameObject.Find("Ground (1)");
        if (ground == null)
        {
            Debug.LogError("Could not find a GameObject named 'Ground (1)' in the open scene.");
            return;
        }

        var reference = GameObject.Find("Square (4162)");
        if (reference == null || !reference.TryGetComponent<SpriteRenderer>(out var referenceRenderer))
        {
            Debug.LogError("Could not find a 'Square (4162)' GameObject with a SpriteRenderer.");
            return;
        }

        var renderers = ground.GetComponentsInChildren<SpriteRenderer>(true);
        Undo.RecordObjects(renderers, "Match Ground (1) Squares To Square (4162)");

        foreach (var renderer in renderers)
        {
            renderer.sprite = referenceRenderer.sprite;
            renderer.sharedMaterial = referenceRenderer.sharedMaterial;
            renderer.color = referenceRenderer.color;
            renderer.flipX = referenceRenderer.flipX;
            renderer.flipY = referenceRenderer.flipY;
            renderer.drawMode = referenceRenderer.drawMode;
            renderer.size = referenceRenderer.size;
            renderer.maskInteraction = referenceRenderer.maskInteraction;
            renderer.sortingLayerID = referenceRenderer.sortingLayerID;
            renderer.sortingOrder = referenceRenderer.sortingOrder;
            renderer.spriteSortPoint = referenceRenderer.spriteSortPoint;
        }

        EditorUtility.SetDirty(ground);
        Debug.Log($"Matched {renderers.Length} SpriteRenderers under 'Ground (1)' to 'Square (4162)'.");
    }

    [MenuItem("Tools/Maps/Wire Up Player For This Scene")]
    public static void WireUpPlayer()
    {
        var player = GameObject.Find("player");
        if (player == null)
        {
            Debug.LogError("Could not find a GameObject named 'Player' in the open scene.");
            return;
        }

        var playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
            Debug.LogWarning("Player has no PlayerController component.");

        var cameraController = Object.FindAnyObjectByType<CameraController2D>();
        if (cameraController == null)
        {
            Debug.LogWarning("No CameraController2D found in the open scene; camera will not follow the player.");
        }
        else
        {
            Undo.RecordObject(cameraController, "Wire Up Player Camera Target");
            cameraController.SetTarget(player.transform);
            EditorUtility.SetDirty(cameraController);
            Debug.Log($"Pointed {cameraController.name}'s CameraController2D at '{player.name}'.");
        }
    }

    [MenuItem("Tools/Maps/Fix Camera Bounds For This Map")]
    public static void FixCameraBounds()
    {
        var cameraController = Object.FindAnyObjectByType<CameraController2D>();
        if (cameraController == null)
        {
            Debug.LogError("No CameraController2D found in the open scene.");
            return;
        }

        float minX = float.MaxValue, maxX = float.MinValue, minY = float.MaxValue, maxY = float.MinValue;
        bool any = false;
        foreach (var renderer in Object.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None))
        {
            var p = renderer.transform.position;
            if (p.x < minX) minX = p.x;
            if (p.x > maxX) maxX = p.x;
            if (p.y < minY) minY = p.y;
            if (p.y > maxY) maxY = p.y;
            any = true;
        }

        if (!any)
        {
            Debug.LogError("No SpriteRenderers found in the open scene to size the camera bounds from.");
            return;
        }

        const float margin = 2f;
        var so = new SerializedObject(cameraController);
        so.FindProperty("minX").floatValue = minX - margin;
        so.FindProperty("maxX").floatValue = maxX + margin;
        so.FindProperty("minY").floatValue = minY - margin;
        so.FindProperty("maxY").floatValue = maxY + margin;
        so.ApplyModifiedProperties();

        EditorUtility.SetDirty(cameraController);
        Debug.Log($"Set camera bounds to X:[{minX - margin:F1}, {maxX + margin:F1}] Y:[{minY - margin:F1}, {maxY + margin:F1}] based on this scene's content.");
    }

    [MenuItem("Tools/Maps/Remove DoorController From Selection")]
    public static void RemoveDoorControllerFromSelection()
    {
        var selected = Selection.gameObjects;
        if (selected.Length == 0)
        {
            Debug.LogError("Select one or more GameObjects in the Hierarchy first (the accidental door tiles).");
            return;
        }

        int cleaned = 0;
        foreach (var go in selected)
        {
            if (!go.TryGetComponent<DoorController>(out var door))
                continue;

            Undo.DestroyObjectImmediate(door);

            if (go.TryGetComponent<Collider2D>(out var collider))
            {
                Undo.RecordObject(collider, "Restore solid collider");
                collider.isTrigger = false;
            }

            if (go.CompareTag("door"))
            {
                Undo.RecordObject(go, "Reset tag");
                go.tag = "Untagged";
            }

            EditorUtility.SetDirty(go);
            cleaned++;
        }

        Debug.Log($"Removed DoorController from {cleaned} of {selected.Length} selected objects, restored solid colliders, and reset their tag.");
    }

    [MenuItem("Tools/Maps/Create Spawn Point For Selected Door")]
    public static void CreateSpawnPointForSelectedDoor()
    {
        var door = Selection.activeGameObject;
        if (door == null || !door.TryGetComponent<DoorController>(out var doorController))
        {
            Debug.LogError("Select a door GameObject (one with a DoorController component) in the Hierarchy first.");
            return;
        }

        var spawnList = GameObject.Find("Spawn_list");
        if (spawnList == null)
        {
            Debug.LogError("Could not find a 'Spawn_list' GameObject in the open scene.");
            return;
        }

        string sceneName = DoorController.GetSceneNameForGateType(doorController.gateType);

        Transform existing = null;
        foreach (Transform child in spawnList.transform)
        {
            if (child.name == sceneName)
            {
                existing = child;
                break;
            }
        }

        Vector3 spawnPos = door.transform.position + new Vector3(0f, 0.8f, 0f);

        if (existing != null)
        {
            Undo.RecordObject(existing, "Move Spawn Point To Door");
            existing.position = spawnPos;
            EditorUtility.SetDirty(existing);
            Debug.Log($"Moved existing spawn point '{sceneName}' to door '{door.name}' at {spawnPos}.");
        }
        else
        {
            var spawnPoint = new GameObject(sceneName);
            Undo.RegisterCreatedObjectUndo(spawnPoint, "Create Spawn Point For Door");
            spawnPoint.transform.SetParent(spawnList.transform);
            spawnPoint.transform.position = spawnPos;
            Debug.Log($"Created spawn point '{sceneName}' under 'Spawn_list' at {spawnPos}, next to door '{door.name}'.");
        }
    }

    [MenuItem("Tools/Maps/Fix Player Sorting Order")]
    public static void FixPlayerSortingOrder()
    {
        var player = GameObject.Find("player");
        if (player == null)
        {
            Debug.LogError("Could not find a GameObject named 'player' in the open scene.");
            return;
        }

        var renderer = player.GetComponentInChildren<SpriteRenderer>(true);
        if (renderer == null)
        {
            Debug.LogError("Player has no SpriteRenderer.");
            return;
        }

        Undo.RecordObject(renderer, "Fix Player Sorting Order");
        renderer.sortingOrder = 10;
        EditorUtility.SetDirty(renderer);
        Debug.Log($"Set '{renderer.name}' sortingOrder to 10 so the player renders above the Ground (1) tiles.");
    }
}
