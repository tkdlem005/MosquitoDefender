using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NavGridData", menuName = "Grid Data", order = 1)]
public class NavGridData : ScriptableObject
{
    #region Struct
    [System.Serializable]
    public struct CustomHeight
    {
        public Vector2Int XZ;
        public float Y;
    }
    #endregion

    public int GridWidth = 20;
    public int GridHeight = 20;
    public float DefaultY = 1.1f;
    public float CellSize = 1f;

    public List<CustomHeight> CustomHeights = new();
    public List<Vector2Int> CustomNonWalkables;
}
