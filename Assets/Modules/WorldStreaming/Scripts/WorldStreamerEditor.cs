#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheKingdomMetaverse.WorldStreaming
{
    [CustomEditor(typeof(WorldStreamer))]
    public class WorldStreamerEditor : Editor
    {
        string tilesSavePath = "";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUIStyle style = new GUIStyle();
            style.richText = true;
            style.normal.textColor = new Color(0.75f, 0.75f, 0.75f);
            GUILayout.Label("");
            GUILayout.Label("<b>Editor Operation</b>", style);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Tiles SavePath");
            tilesSavePath = GUILayout.TextField(tilesSavePath, 25);

            if (GUILayout.Button("Browse"))
            {
                string newSavePath = EditorUtility.OpenFolderPanel("Select the tiles folder", Application.dataPath, "");
                if(newSavePath.Contains(Application.dataPath))
                {
                    tilesSavePath = newSavePath.Replace(Application.dataPath, "");
                }
            }

            GUILayout.EndHorizontal();

            WorldStreamer worldStreamer = (WorldStreamer)target;


            if (GUILayout.Button("Transform To GridTile Scenes"))
            {
                if(tilesSavePath == "")
                {
                    EditorUtility.DisplayDialog("Error", "You have to select the save path before transformation.", "OK");
                    return;
                }
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                ApplyColliderToAllMeshes();
                SeperateObjectsToTiles(
                    worldStreamer.gridWidth, 
                    worldStreamer.gridHeight, 
                    worldStreamer.gridSize,
                    worldStreamer.worldOffset
                );

            }
        }


        // Long process, use with caution
        void ApplyColliderToAllMeshes()
        {
            MeshRenderer[] meshRenderers = FindObjectsOfType<MeshRenderer>();
            foreach (MeshRenderer renderer in meshRenderers)
            {
                if (renderer.GetComponent<Collider>() == null)
                    renderer.gameObject.AddComponent<MeshCollider>();

                Debug.Log(renderer.name);
            }
        }

        void SeperateObjectsToTiles(int gridWidth, int gridHeight, int gridSize, Vector3 offset)
        {

            if (!Directory.Exists(Application.dataPath + tilesSavePath))
            {
                Directory.CreateDirectory(Application.dataPath + tilesSavePath);
            }

            // From x = 0, y = 0 to x = 100, y = 100
            Vector3 halfExtent = new Vector3(gridSize / 2 * 0.99f, 1000, gridSize / 2 * 0.99f);
            for (int x = 0; x < gridWidth; x++)
            {
                for (int z = 0; z < gridHeight; z++)
                {
                    // Create Scene.
                    string sceneName = "Tiles_" + x + "_" + z;
                    Debug.Log(sceneName);

                    Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
                    newScene.name = sceneName;
                    // Find all meshes
                    Vector3 center = offset + new Vector3(x * gridSize + (gridSize / 2), 0, z * gridSize + (gridSize / 2));

                    Collider[] cols = Physics.OverlapBox(center, halfExtent, Quaternion.identity);
                    foreach (Collider col in cols)
                    {
                        GameObject target;
                        if (col.transform.parent != null)
                        {
                            target = col.transform.parent.gameObject;
                        }
                        else
                        {
                            target = col.gameObject;
                        }

                        try
                        {
                            SceneManager.MoveGameObjectToScene(target, newScene);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }

                    EditorSceneManager.SaveScene(
                        newScene,
                        Application.dataPath + tilesSavePath + "/" + sceneName + ".unity",
                        false
                    );

                    EditorSceneManager.CloseScene(newScene, true);
                    AssetDatabase.SaveAssets();
                }
            }
            AssetDatabase.Refresh();

            Scene viewScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
           
            for (int x = 0; x < gridWidth; x++)
            {
                for (int z = 0; z < gridHeight; z++)
                {
                    // Create Scene.
                    string sceneName = "Tiles_" + x + "_" + z;
                    EditorSceneManager.OpenScene(Application.dataPath + tilesSavePath + "/" + sceneName + ".unity", OpenSceneMode.Additive);
                }
            }
        }

    }

}

#endif