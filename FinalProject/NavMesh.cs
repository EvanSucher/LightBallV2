using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class NavMesh
    {
        public List<NavNode> Mesh;

        public NavMesh()
        {
            Mesh = new List<NavNode>();
        }

        public void AddNode(float x, float y)
        {
            Mesh.Add(new NavNode(x, y));
        }

        public void Clear()
        {
            Mesh = new List<NavNode>();
        }

        public void UpdateNearbyNodes() //updates node's list of other nearby nodes
        {
            foreach (NavNode baseNode in Mesh)
            {
                foreach(NavNode n in Mesh)
                {
                    baseNode.AddNode(n);
                }
            }
        }

        public NavNode nodeClosestToPoint(float x, float y) //returns NavNode closest to given location
        {
            float shortestDist = 200;
            NavNode passNode = new NavNode(0,0);
            foreach (NavNode n in Mesh)
            {
                float xDif = x - n.Location.X;
                float yDif = y - n.Location.Y;
                float currentDist = (float)Math.Sqrt((xDif * xDif) + (yDif * yDif));
                if (currentDist < shortestDist)
                {
                    passNode = n;
                    shortestDist = currentDist;
                }
            }
            return passNode;
        }

        public NavNode nodeClosestToPointNextToNode(float x, float y, NavNode originNode) //returns NavNode next to input Node that is closest to point
        {
            float shortestDist = 10000;
            NavNode passNode = new NavNode(0, 0);
            foreach (NavNode n in originNode.nearestNodes)
            {
                if (!n.nearWall)
                {
                    float xDif = x - n.Location.X;
                    float yDif = y - n.Location.Y;
                    float currentDist = (float)Math.Sqrt((xDif * xDif) + (yDif * yDif));
                    if (currentDist < shortestDist)
                    {
                        passNode = n;
                        shortestDist = currentDist;
                    }
                }
            }
            return passNode;
        }
    }
}
