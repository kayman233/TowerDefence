﻿using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace Field
{
    public class Node
    {
        public Vector3 Position;
        public List<EnemyData> EnemyDatas = new List<EnemyData>();

        public Node(Vector3 position)
        {
            Position = position;
        }

        public Node NextNode;
        public bool IsOccupied;

        public OccupationAvailability OccupationAvailability;
        
        public float PathWeight;

        public void ResetWeight()
        {
            PathWeight = float.MaxValue;
        }
    }
    
    public enum OccupationAvailability
    {
        CanOccupy,
        CanNotOccupy,
        Undefined
    }
}