using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapEditor : EditorBase
{
    private const string S_LBL_MAP_NAME = "Map Name";
    private const string S_LBL_CHUNK_DIRECTORY = "Chunk Directory";
    private const string S_LBL_ENEMY_LIST = "EnemyList";
            
    Vector2 scrollView;

    MapData activeMapData;
    TextAsset obj;
    string mapName = string.Empty;
    string chunkDirectory = string.Empty;
    public List<TextAsset> EnemyList = new List<TextAsset>();

    [MenuItem("Window/Editor/Maps")]
    private static void ShowWindow()
    {
        GetWindow(typeof(MapEditor));
    }

    private void Awake()
    {
        so = new SerializedObject(this);
        activeEntity = new EntityReplacement();
    }

    protected override void MainWindow()
    {
        scrollView = EditorGUILayout.BeginScrollView(scrollView, false, true, GUILayout.Width(position.width));
        {
            obj = PaintXMLLookup(obj, "Resource File", true);
            if (PaintButton("Save"))
            {
                Save();
            }
                        
            PaintTextField(ref mapName, S_LBL_MAP_NAME);
            PaintTextField(ref chunkDirectory, S_LBL_CHUNK_DIRECTORY);
            PaintList(S_LBL_ENEMY_LIST);
        }
        EditorGUILayout.EndScrollView();
    }

    protected override void ResetProperties()
    {

    }

    protected override void LoadProperties(TextAsset textAsset)
    {
        activeMapData = XMLUtility.Load<MapData>(textAsset);
    }
    
    private void Save()
    {
        string filePath = string.Empty;
        
        foreach( TextAsset enemy in EnemyList ) {
            filePath = AssetDatabase.GetAssetPath(enemy).Substring(S_RESOURCE_DIR_LENGTH);
            filePath = filePath.Substring(0, filePath.Length - S_XML_EXTENSION_LENGTH);
            activeMapData.EnemyList.Add(filePath);
        }
        
        XMLUtility.Save<MapData>(activeMapData, S_MAPDATA_DIR, activeMapData.MapName);
    }
}
