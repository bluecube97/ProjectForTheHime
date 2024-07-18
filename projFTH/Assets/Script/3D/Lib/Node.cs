using Script._3D.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Script._3D.Lib
{
    public class Node
    {
        public Vector2Int Position { get; set; }
        public PositionComponentVo PositionComponent { get; set; }
        public int Distance { get; set; }
        public bool Visited { get; set; }
        public Node Previous { get; set; }
        public List<Node> Neighbors { get; set; }

        public Node(Vector2Int position, PositionComponentVo positionComponent = null)
        {
            Position = position;
            PositionComponent = positionComponent;
            Distance = int.MaxValue;
            Visited = false;
            Neighbors = new List<Node>();
        }
    }
}