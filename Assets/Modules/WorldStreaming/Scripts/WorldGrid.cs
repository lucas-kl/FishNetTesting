using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheKingdomMetaverse.WorldStreaming
{
    public class WorldSceneGrid
    {

        private Grid<WorldSceneGrid> owner;
        private int x;
        private int y;

        public WorldSceneGrid(Grid<WorldSceneGrid> grid, int x, int y)
        {
            this.owner = grid;
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return x + ", " + y + "\n";
        }

    }

}
