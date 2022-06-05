using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKingdomMetaverse.BuildingSystem
{
    public class BuildingGrid
    {

        private Grid<BuildingGrid> owner;
        private int x;
        private int y;
        private PlaceableObject placedObject;

        public BuildingGrid(Grid<BuildingGrid> grid, int x, int y)
        {
            this.owner = grid;
            this.x = x;
            this.y = y;
        }

        public void SetPlacedObject(PlaceableObject newObj)
        {
            this.placedObject = newObj;
        }


        public override string ToString()
        {
            return x + ", " + y + "\n";
        }

        public bool CanBuild()
        {
            return placedObject == null;
        }

    }
}