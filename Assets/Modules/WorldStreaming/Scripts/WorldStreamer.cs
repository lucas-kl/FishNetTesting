using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace TheKingdomMetaverse.WorldStreaming
{
    public class WorldStreamer : MonoBehaviour
    {
        [Header("World Settings")]
        public int gridSize; // The length of each block
        public int gridWidth;
        public int gridHeight;

        public Vector3 worldOffset;

        [Header("Player")]
        public Transform player;

        private Vector2Int playerLastPos;

        private Dictionary<Vector2Int, string> sceneList = new Dictionary<Vector2Int, string>();

        // Current rendered List
        private List<Vector2Int> lastRenderedScenePosList = new List<Vector2Int>();
        private List<Vector2Int> unusedScenePosList = new List<Vector2Int>();

        public float unusedSceneClearMaxTime = 10f;
        private float unusedSceneClearTimer;

        private Grid<WorldSceneGrid> grid;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("World Streamer Start");
            Init();
            RenderRegionalSpace(true);
        }

        // Update is called once per frame
        void Update()
        {
            if (player == null)
                return;
            RenderRegionalSpace();
            ClearUnusedScenes();
        }
        
        void Init()
        {

            GetAllSceneTiles();
            Debug.Log("END");

            //GetAllSceneTiles()
            unusedSceneClearTimer = unusedSceneClearMaxTime;
            Debug.Log("Build Grid");
            grid = new Grid<WorldSceneGrid>(
                gridWidth,
                gridHeight,
                gridSize,
                worldOffset
            );
            Debug.Log("END2");

        }

        void GetAllSceneTiles()
        {
            Debug.Log("GG");
            int sceneCount = 17;
            string[] scenes = new string[sceneCount];
            for (int i = 0; i < sceneCount; i++)
            {
                Debug.Log("this" + SceneUtility.GetScenePathByBuildIndex(i));
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                //string sceneName = SceneUtility.GetScenePathByBuildIndex(i).Substring(0,  SceneUtility.GetScenePathByBuildIndex(i).Length - 6 ).Substring(SceneUtility.GetScenePathByBuildIndex(i).LastIndexOf('/') + 1);

                Debug.Log(sceneName);
                scenes[i] = sceneName;
                if (!sceneName.Contains("Tiles_"))
                {
                    continue;
                }

                try
                {
                    string[] splitedString = sceneName.Split('_');
                    int x = int.Parse(splitedString[1]);
                    int y = int.Parse(splitedString[2]);
                    sceneList.Add(new Vector2Int(x, y), sceneName);
                    Debug.Log(new Vector2Int(x, y));
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

            }
        }

        void RenderRegionalSpace(bool isFirstTime = false)
        {
            if (player == null)
                return;
            // Get player current position
            int playerPosX = (int)((player.position.x - worldOffset.x) / gridSize);
            int playerPosZ = (int)((player.position.z - worldOffset.y) / gridSize);
            Vector2Int curPos = new Vector2Int(playerPosX, playerPosZ);

            if(!isFirstTime)
            {
                if(playerLastPos == curPos)
                {
                    return;
                }
            }
            playerLastPos = curPos;

            List<Vector2Int> tempLastRenderedPosList = new List<Vector2Int>(lastRenderedScenePosList);
            for (int i = -1; i < 2; i++)
            {
                for(int j = -1; j < 2; j++)
                {
                    if(!IsValidGrid(playerPosX + i, playerPosZ + j))
                    {
                        continue;
                    }

                    Vector2Int validPos = curPos + new Vector2Int(i, j);

                    // Remove in deletion list
                    if (unusedScenePosList.Contains(validPos))
                    {
                        unusedScenePosList.Remove(validPos);
                    }

                    // Render new blocks
                    if (!lastRenderedScenePosList.Contains(validPos))
                    {
                        LoadScene(validPos);
                        lastRenderedScenePosList.Add(validPos);
                    }

                    // Remove in temp last rendered list
                    tempLastRenderedPosList.Remove(validPos);
                }
            }
            
            // Add the remaining unused scene from last rendered list
            foreach(Vector2Int pos in tempLastRenderedPosList)
            {
                if (!unusedScenePosList.Contains(pos))
                {
                    unusedScenePosList.Add(pos);
                }
            }

            unusedSceneClearTimer = unusedSceneClearMaxTime;

            Debug.Log("END RENDER");
        }

        void ClearUnusedScenes()
        {
            if(unusedScenePosList.Count == 0)
            {
                return;
            }

            unusedSceneClearTimer -= Time.deltaTime;
            if(unusedSceneClearTimer >= 0)
            {
                return;
            } 

            foreach (Vector2Int unusedPos in unusedScenePosList)
            {
                UnloadScene(unusedPos);
                lastRenderedScenePosList.Remove(unusedPos);
            }
            unusedScenePosList.Clear();

            unusedSceneClearTimer = unusedSceneClearMaxTime;
        }

        bool IsValidGrid(int x, int z)
        {
            if(x < 0 || z < 0)
            {
                return false;
            }

            if (x >= gridWidth || z >= gridHeight)
            {
                return false;
            }

            return true;
        }

        void LoadScene(Vector2Int pos)
        {
            string sceneName = sceneList[pos];
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            // yield return null;
            /*
            while (op.isDone == false)
            {
                yield return null;
            }
            */
        }

        void UnloadScene(Vector2Int pos)
        {
            string sceneName = sceneList[pos];
            AsyncOperation op = SceneManager.UnloadSceneAsync(sceneName);
        }


#if UNITY_EDITOR
        // Draw tiles in scene view
        void OnDrawGizmos()
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Gizmos.DrawWireCube(
                        new Vector3(worldOffset.x + x * gridSize + gridSize * 0.5f, 0, worldOffset.y + y * gridSize + gridSize * 0.5f),
                        new Vector3(gridSize, 0.1f, gridSize)
                    );
                    Handles.Label(
                        new Vector3(worldOffset.x + x * gridSize + gridSize * 0.5f, 10, worldOffset.y + y * gridSize + gridSize * 0.5f),
                        "[" + x + "," + y + "]"
                    );
                }
            }
        }
#endif
    }
}
