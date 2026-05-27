using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using ZerosAndOnes.Gameplay;

public class MapGenerator : EditorWindow
{
    [MenuItem("ZerosAndOnes/Fix Build Settings")]
    public static void FixBuildSettings()
    {
        string menuScenePath = "Assets/Scenes/MainMenu.unity";
        string mapScenePath = "Assets/Scenes/firstMap.unity";

        var scenes = new System.Collections.Generic.List<EditorBuildSettingsScene>();
        
        if (System.IO.File.Exists(menuScenePath))
        {
            scenes.Add(new EditorBuildSettingsScene(menuScenePath, true));
        }
        else
        {
            Debug.LogError("Could not find MainMenu scene at " + menuScenePath);
        }

        if (System.IO.File.Exists(mapScenePath))
        {
            scenes.Add(new EditorBuildSettingsScene(mapScenePath, true));
        }
        else
        {
            Debug.LogError("Could not find firstMap scene at " + mapScenePath);
        }

        EditorBuildSettings.scenes = scenes.ToArray();
        Debug.Log("Build Settings successfully fixed! MainMenu (Index 0), firstMap (Index 1).");
    }

    [MenuItem("ZerosAndOnes/Generate Base Map")]
    public static void GenerateMap()
    {
        // 1. Load Spring Grass Tiles
        string basePath = "Assets/BayatGames/Free Platform Game Assets/Tiles/2D Tiles ( Update 1.9 )/Spring/128x128/";
        TileBase grassMid = AssetDatabase.LoadAssetAtPath<TileBase>(basePath + "GrassMid.asset");
        TileBase grassLeft = AssetDatabase.LoadAssetAtPath<TileBase>(basePath + "GrassLeft.asset");
        TileBase grassRight = AssetDatabase.LoadAssetAtPath<TileBase>(basePath + "GrassRight.asset");

        // Load 1-1x.asset for solid walls
        TileBase solidWall = AssetDatabase.LoadAssetAtPath<TileBase>(basePath + "1-1x.asset");
        if (solidWall == null) solidWall = grassMid; // Fallback

        // Setup fallbacks if they are missing
        if (grassMid == null) grassMid = CreateDefaultTile("DefaultGrassMid", Color.green);
        if (grassLeft == null) grassLeft = CreateDefaultTile("DefaultGrassLeft", Color.green);
        if (grassRight == null) grassRight = CreateDefaultTile("DefaultGrassRight", Color.green);

        // 2. Load and create Water Tile
        string waterSpritePath = "Assets/BayatGames/Free Platform Game Assets/Water Animation/2D view ( Update 1.9 )/128x128/1-1x.png";
        Sprite waterSprite = AssetDatabase.LoadAssetAtPath<Sprite>(waterSpritePath);
        TileBase waterTile = null;

        if (waterSprite != null)
        {
            Tile wTile = ScriptableObject.CreateInstance<Tile>();
            wTile.sprite = waterSprite;
            wTile.name = "WaterTile";
            
            if (!AssetDatabase.IsValidFolder("Assets/Tiles"))
            {
                AssetDatabase.CreateFolder("Assets", "Tiles");
            }
            AssetDatabase.CreateAsset(wTile, "Assets/Tiles/WaterTile.asset");
            waterTile = wTile;
        }
        else
        {
            waterTile = CreateDefaultTile("DefaultWater", new Color(0, 0.5f, 1f, 0.7f));
        }

        // 3. Setup Grid and Tilemaps
        // Destroy existing Environment_Grid to avoid duplicates
        GameObject existingGrid = GameObject.Find("Environment_Grid");
        if (existingGrid != null)
        {
            DestroyImmediate(existingGrid);
        }

        GameObject gridGO = new GameObject("Environment_Grid");
        Grid grid = gridGO.AddComponent<Grid>();

        // Create main ground tilemap
        GameObject tilemapGO = new GameObject("Ground");
        tilemapGO.transform.SetParent(gridGO.transform);
        Tilemap tilemap = tilemapGO.AddComponent<Tilemap>();
        TilemapRenderer renderer = tilemapGO.AddComponent<TilemapRenderer>();

        // Add colliders for Ground
        TilemapCollider2D tilemapCollider = tilemapGO.AddComponent<TilemapCollider2D>();
        tilemapCollider.usedByComposite = true;
        Rigidbody2D rb = tilemapGO.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        CompositeCollider2D compositeCollider = tilemapGO.AddComponent<CompositeCollider2D>();

        // Create separate water tilemap (without rigidbodies so player falls in, but could have trigger colliders)
        GameObject waterMapGO = new GameObject("Water");
        waterMapGO.transform.SetParent(gridGO.transform);
        Tilemap waterMap = waterMapGO.AddComponent<Tilemap>();
        TilemapRenderer waterRenderer = waterMapGO.AddComponent<TilemapRenderer>();
        waterRenderer.sortingOrder = 1; // Render in front of ground

        // 4. Draw Layout Coordinates
        // Ground is at Y = -5, outer walls ceiling is at Y = 15

        // Central Start Lobby: X = -8 to 8
        // Left Wing: X = -28 to -9
        // Right Wing: X = 9 to 28

        // Draw general floor at Y = -5 and Y = -6
        for (int x = -28; x <= 28; x++)
        {
            // Bottom solid foundation
            tilemap.SetTile(new Vector3Int(x, -6, 0), solidWall);

            // Floor at Y = -5
            if (x >= -2 && x <= 2)
            {
                // Central Water Pit (X = -2 to 2)
                // Dig out Y = -5 for water
                waterMap.SetTile(new Vector3Int(x, -5, 0), waterTile);
                waterMap.SetTile(new Vector3Int(x, -6, 0), waterTile);
                tilemap.SetTile(new Vector3Int(x, -7, 0), solidWall); // floor under water
            }
            else if (x == -3)
            {
                tilemap.SetTile(new Vector3Int(x, -5, 0), grassRight); // Edge of pit
            }
            else if (x == 3)
            {
                tilemap.SetTile(new Vector3Int(x, -5, 0), grassLeft); // Edge of pit
            }
            else
            {
                tilemap.SetTile(new Vector3Int(x, -5, 0), grassMid);
            }
        }

        // Draw Left Wing outer wall (X = -29)
        for (int y = -5; y <= 15; y++)
        {
            // Door at Y = 2 to 4 (Green "door" at far left in sketch)
            bool isFarLeftDoor = (y >= 2 && y <= 4);
            if (!isFarLeftDoor)
            {
                tilemap.SetTile(new Vector3Int(-29, y, 0), solidWall);
            }
        }

        // Draw Inner Left Wall (between lobby and Left Wing) at X = -9
        for (int y = -5; y <= 15; y++)
        {
            // Lower door: Y = -4 to -2
            // Upper door: Y = 0 to 1
            bool isLowerDoor = (y >= -4 && y <= -2);
            bool isUpperDoor = (y >= 0 && y <= 1);

            if (!isLowerDoor && !isUpperDoor)
            {
                tilemap.SetTile(new Vector3Int(-9, y, 0), solidWall);
            }
        }

        // Draw Left Wing "Half" puzzle platforms
        // Platform at Y = -2, from X = -20 to -14
        tilemap.SetTile(new Vector3Int(-20, -2, 0), grassLeft);
        for (int x = -19; x <= -15; x++) tilemap.SetTile(new Vector3Int(x, -2, 0), grassMid);
        tilemap.SetTile(new Vector3Int(-14, -2, 0), grassRight);

        // Ceiling/Floor above Start Lobby & Left Wing at Y = 2
        // Start lobby ceiling (X = -8 to 8) and Left Wing ceiling (X = -28 to -9)
        // Left Wing ceiling is Y = 5. Central ceiling is Y = 2.
        for (int x = -28; x <= 8; x++)
        {
            if (x >= -8 && x <= 8)
            {
                // Central ceiling (XOR floor above)
                tilemap.SetTile(new Vector3Int(x, 2, 0), grassMid);
            }
            else if (x >= -28 && x <= -10)
            {
                // Left Wing ceiling
                tilemap.SetTile(new Vector3Int(x, 5, 0), solidWall);
            }
        }

        // Draw Inner Right Wall (between lobby and Right Wing) at X = 9
        for (int y = -5; y <= 15; y++)
        {
            // Lower door: Y = -4 to -2
            bool isLowerDoor = (y >= -4 && y <= -2);
            if (!isLowerDoor)
            {
                tilemap.SetTile(new Vector3Int(9, y, 0), solidWall);
            }
        }

        // Right Wing Divider Wall at X = 17
        for (int y = -5; y <= 15; y++)
        {
            // Door at Y = 1 to 3
            bool isDividerDoor = (y >= 1 && y <= 3);
            if (!isDividerDoor)
            {
                tilemap.SetTile(new Vector3Int(17, y, 0), solidWall);
            }
        }

        // Right Wing Outer Wall at X = 29
        for (int y = -5; y <= 15; y++)
        {
            // Lower door: Y = -4 to -2
            // Upper door: Y = 7 to 9
            bool isLowerDoor = (y >= -4 && y <= -2);
            bool isUpperDoor = (y >= 7 && y <= 9);

            if (!isLowerDoor && !isUpperDoor)
            {
                tilemap.SetTile(new Vector3Int(29, y, 0), solidWall);
            }
        }

        // Right Wing Platforms
        // Platform 1: Y = -2, X = 10 to 13
        tilemap.SetTile(new Vector3Int(10, -2, 0), grassLeft);
        tilemap.SetTile(new Vector3Int(11, -2, 0), grassMid);
        tilemap.SetTile(new Vector3Int(12, -2, 0), grassMid);
        tilemap.SetTile(new Vector3Int(13, -2, 0), grassRight);

        // Platform 2: Y = 1, X = 20 to 24
        tilemap.SetTile(new Vector3Int(20, 1, 0), grassLeft);
        for (int x = 21; x <= 23; x++) tilemap.SetTile(new Vector3Int(x, 1, 0), grassMid);
        tilemap.SetTile(new Vector3Int(24, 1, 0), grassRight);

        // Platform 3: Y = 4, X = 11 to 15
        tilemap.SetTile(new Vector3Int(11, 4, 0), grassLeft);
        for (int x = 12; x <= 14; x++) tilemap.SetTile(new Vector3Int(x, 4, 0), grassMid);
        tilemap.SetTile(new Vector3Int(15, 4, 0), grassRight);

        // Platform 4: Y = 7, X = 19 to 24
        tilemap.SetTile(new Vector3Int(19, 7, 0), grassLeft);
        for (int x = 20; x <= 23; x++) tilemap.SetTile(new Vector3Int(x, 7, 0), grassMid);
        tilemap.SetTile(new Vector3Int(24, 7, 0), grassRight);

        // Right Wing Ceiling at Y = 12
        for (int x = 9; x <= 28; x++)
        {
            tilemap.SetTile(new Vector3Int(x, 12, 0), solidWall);
        }

        // Save & Undos
        Undo.RegisterCreatedObjectUndo(gridGO, "Generate Sketch Map");
        Selection.activeGameObject = gridGO;

        // Try to position the Player if a PlayerController exists in the scene
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            // Spawn player at bottom-left in front of the door (X = -7, Y = -4)
            player.transform.position = new Vector3(-7f, -4f, 0f);
            Undo.RecordObject(player.transform, "Position Player");
            Debug.Log("Player found and positioned at the starting door (-7, -4)!");
        }

        Debug.Log("Base Map from Sketch Generated Successfully! Water pit and Spawning Door setup.");
    }

    private static TileBase CreateDefaultTile(string name, Color color)
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        
        Texture2D tex = new Texture2D(128, 128);
        Color[] colors = new Color[128 * 128];
        for (int i = 0; i < colors.Length; i++) colors[i] = color;
        tex.SetPixels(colors);
        tex.Apply();
        
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f), 128);
        sprite.name = name + "_Sprite";
        tile.sprite = sprite;

        if (!AssetDatabase.IsValidFolder("Assets/Tiles"))
        {
            AssetDatabase.CreateFolder("Assets", "Tiles");
        }
        AssetDatabase.CreateAsset(tile, "Assets/Tiles/" + name + ".asset");
        return tile;
    }
}
