using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class NavNode
    {
        public Vector2 Location;
        public List<NavNode> nearestNodes;
        public bool nearWall;
        public NavNode(float x, float y)
        {
            Location = new Vector2(x, y);
            nearestNodes = new List<NavNode>();
            nearWall = true;
        }

        public void AddNode(NavNode nn) //Adds a node to nearest nodes if it is close enough
        {
            if (this.Location.X < nn.Location.X+51 && this.Location.X > nn.Location.X - 51)
            {
                if (this.Location.Y < nn.Location.Y + 51 && this.Location.Y > nn.Location.Y - 51)
                {
                    if (this.Location.Y != nn.Location.Y || this.Location.X != nn.Location.X)
                    {
                        nearestNodes.Add(nn);
                    }
                }
            }
            if (nearestNodes.Count >= 8) // if the node has 8 near nodes than there no walls near by
            {
                nearWall = false;
            }
        }

        public void ClearNearestNodes()
        {
            nearestNodes = new List<NavNode>();
        }
    }
}
