using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    [SerializeField] private int maxSizeX;
    [SerializeField] private int maxSizeY;
    [SerializeField] private int cellSize = 1;

    public int MaxSizeX => maxSizeX;
    public int MaxSizeY => maxSizeY;
    public int CellSize => cellSize;
}
