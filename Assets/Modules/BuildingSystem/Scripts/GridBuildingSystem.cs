using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheKingdomMetaverse.BuildingSystem
{

    public class GridBuildingSystem : Singleton<GridBuildingSystem>
    {
        public PlayerBuildingControlInputs input;
        public PlaceableObject[] buildObjectList;

        public event EventHandler OnSelectionChanged;
        public event EventHandler OnGridChanged;

        // TheGrid 
        private Grid<BuildingGrid> grid;

        public PlaceableObject selectedPlacedObject;
        private PlaceableObject.Facing facing = PlaceableObject.Facing.Down;

        private int[] lastCoordinate = new int[2] { -1, -1};

        [Header("Grid Settings")]
        public int gridWidth = 10;
        public int gridHeight = 10;
        public int gridSize = 10; 

        private void Awake()
        {
            grid = new Grid<BuildingGrid>(
                gridWidth,
                gridHeight,
                gridSize,
                Vector3.zero,
                (Grid<BuildingGrid> g, int x, int y) => new BuildingGrid(g, x, y)
            );
        }

        // Start is called before the first frame update
        void Start()
        {
            ChangeBuildingSelection(0);
        }

        // Update is called once per frame
        void Update()
        {
            if (input.interact)
            {
                input.interact = false;
                ChangeBuildingSelection(1);
            }

            if (input.confirm)
            {
                input.confirm = false;
                Ray r = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (!Physics.Raycast(r, out RaycastHit hit))
                {
                    return;
                }

                grid.GetXZCoordinate(hit.point, out int x, out int z);

                // Coor Not Updated
                if (lastCoordinate[0] == x && lastCoordinate[1] == z)
                    return;

                if (x < 0 || z < 0)
                    return;

                Vector2Int objectCoordinate = new Vector2Int(x, z);
                List<Vector2Int> gridPositionList = selectedPlacedObject.GetGridPositionList(objectCoordinate, facing);
                if (CheckAvailability(gridPositionList))
                {
                    GenerateBuilding(x, z, gridPositionList);
                }

                lastCoordinate[0] = x;
                lastCoordinate[1] = z;
            }
        }

        bool CheckAvailability(List<Vector2Int> gridPositionList)
        {
            if (!CanBuild(gridPositionList))
            {
                print("No space");
                return false;
            }

            return true;
        }

        void GenerateBuilding(int x, int z, List<Vector2Int> gridPositionList)
        {
            Vector3 origin = grid.GetWorldPosition(x, z);
            Vector2Int rotationOffset = selectedPlacedObject.GetRotationOffset(facing);
            Vector3 position = origin + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetSize();

            GameObject go = new GameObject();
            Building building = go.AddComponent<Building>();
            building.Init(selectedPlacedObject, position, facing, true);
            grid.GetGridObject(x, z).SetPlacedObject(selectedPlacedObject);
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(selectedPlacedObject);
            }
        }

        private bool CanBuild(List<Vector2Int> gridPositionList)
        {
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                {
                    return false;
                }
            }
            return true;
        }

        public void ChangeBuildingSelection(int newSelection) {
        
            if(newSelection >= buildObjectList.Length) 
            {
                return;
            }
            selectedPlacedObject = buildObjectList[newSelection];
            OnSelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool GetMouseSnappedWorldPosition(out Vector3 position)
        {
            Ray r = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(r, out RaycastHit hit))
            {
                position = Vector3.zero;
                return false;
            }

            Vector3 mousePosition = hit.point;


            grid.GetXZCoordinate(mousePosition, out int x, out int z);


            if (grid.IsTooCloseToGridLine(mousePosition))
            {
                position = mousePosition;
                return false;
            }

            if (selectedPlacedObject != null)
            {
                Vector2Int rotationOffset = selectedPlacedObject.GetRotationOffset(facing);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetSize();
                position = placedObjectWorldPosition;
                return true;

            }
            else
            {
                position = mousePosition;
                return false;
            }
        }

        public Quaternion GetPlacedObjectRotation()
        {
            if (selectedPlacedObject != null)
            {
                return Quaternion.Euler(0, selectedPlacedObject.GetRotationAngle(facing), 0);
            }
            else
            {
                return Quaternion.identity;
            }
        }

        public PlaceableObject.Facing GetSystemFacing
        {
            get { return facing; }
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
                        new Vector3(x * gridSize + gridSize * 0.5f, 0, y * gridSize + gridSize * 0.5f),
                        new Vector3(gridSize, 0.1f, gridSize)
                    );
                    Handles.Label(
                        new Vector3(x * gridSize + gridSize * 0.5f, 5, y * gridSize + gridSize * 0.5f),
                        "[" + x + "," + y + "]"
                    );
                }
            }
        }
#endif
    }

}
