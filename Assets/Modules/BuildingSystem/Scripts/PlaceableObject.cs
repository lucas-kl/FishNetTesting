using System.Collections.Generic;
using UnityEngine;

namespace TheKingdomMetaverse.BuildingSystem
{
    [CreateAssetMenu(menuName = "BuildingSystem/PlacableObject")]
    public class PlaceableObject : ScriptableObject
    {
        public enum Facing
        {
            Down,
            Left,
            Up,
            Right,
        }

        public static Facing Next(Facing curFacing)
        {
            return (curFacing == Facing.Right) ? Facing.Down : curFacing++;
        }

        public GameObject model;
        public GameObject baseGround;
        public string name;
        public int width = 1;
        public int height = 1;

        public int GetRotationAngle(Facing facing)
        {
            return (int)facing * 90;
        }

        public Vector2Int GetRotationOffset(Facing facing)
        {
            switch (facing)
            {
                default:
                case Facing.Down: return new Vector2Int(0, 0);
                case Facing.Left: return new Vector2Int(0, width);
                case Facing.Up: return new Vector2Int(width, height);
                case Facing.Right: return new Vector2Int(height, 0);
            }
        }

        public List<Vector2Int> GetGridPositionList(Vector2Int startPoint, Facing facing)
        {
            List<Vector2Int> gridPositionList = new List<Vector2Int>();
            switch (facing)
            {
                default:
                case Facing.Down:
                case Facing.Up:
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            gridPositionList.Add(startPoint + new Vector2Int(x, y));
                        }
                    }
                    break;
                case Facing.Left:
                case Facing.Right:
                    for (int x = 0; x < height; x++)
                    {
                        for (int y = 0; y < width; y++)
                        {
                            gridPositionList.Add(startPoint + new Vector2Int(x, y));
                        }
                    }
                    break;
            }
            return gridPositionList;
        }

    }

}
