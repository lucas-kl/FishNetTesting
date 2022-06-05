using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int z;
    }

    private int width; // Single Grid Width
    private int height; // Single Grid Height
    private float size; // Size Of Grid Map 

    private Vector3 originPosition;
    private TGridObject[,] gridArray;
    private Transform debugObjectParent;

    public Grid(int width, int height, float size, Vector3 ogPos, Func<Grid<TGridObject>, int, int, TGridObject> onGridCreated)
    {
        this.width = width;
        this.height = height;
        this.size = size;
        this.originPosition = ogPos;

        gridArray = new TGridObject[width, height];


        if (onGridCreated != null)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int z = 0; z < gridArray.GetLength(1); z++)
                {
                    gridArray[x, z] = onGridCreated(this, x, z);
                }
            }
        }


        bool showDebug = true;
        if (showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int z = 0; z < gridArray.GetLength(1); z++)
                {
                    //debugTextArray[x, z] = UtilsClass.CreateWorldText(gridArray[x, z]?.ToString(), null, GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, 15, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);

                    DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white);
                    DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white);

                }
            }
            DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white);
            DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white);


            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
                debugTextArray[eventArgs.x, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.z]?.ToString();
            };
        }
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetSize()
    {
        return size;
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * size + originPosition;
    }

    public TGridObject GetGridObject(int x, int z)
    {
        return gridArray[x, z];
    }

    public void GetXZCoordinate(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / size);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / size);
    }

    public bool IsTooCloseToGridLine(Vector3 worldPosition)
    {
        float x = (worldPosition - originPosition).x / size % 1;
        float z = (worldPosition - originPosition).z / size % 1;
        if ( x < 0.1f || z < 0.1f )
        {
            return true;
        }
        return false;
    }

    public void TriggerOnGridObjectChanged(int x, int z)
    {
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, z = z });
    }

    // Debug
    private void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        if(!debugObjectParent)
        {
            debugObjectParent = new GameObject().transform;
        }
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.transform.SetParent(debugObjectParent.transform);
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.5f;
        lr.endWidth = 0.5f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
