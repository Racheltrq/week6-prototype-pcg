using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GridVisualizer gridVisualizer;
    public MapVisualizer mapVisualizer;
    public Direction startEdge, exitEdge;
    public bool randomPlacement;
    [Range(1,10)]
    public int numberOfPieces;
    private Vector3 startPosition, exitPosition;

    [Range(3,20)]
    public int width, length = 11;
    private MapGrid grid;

    // Start is called before the first frame update
    private void Start()
    {
        MapGrid grid = new MapGrid(width, length);
        gridVisualizer.VisualizeGrid(width, length);
        MapHelper.RandomlyChooseAndSetStartAndExit(grid, ref startPosition, ref exitPosition, randomPlacement, startEdge, exitEdge);
        CandidateMap map = new CandidateMap(grid, numberOfPieces);
        map.CreateMap(startPosition, exitPosition);
        mapVisualizer.VisualizeMap(grid, map.ReturnMapData(), false);

    }

}
