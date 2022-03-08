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
    private CandidateMap map;

    // Start is called before the first frame update
    private void Start()
    {
        
        gridVisualizer.VisualizeGrid(width, length);
        GenerateNewMap();
        

    }

    public void GenerateNewMap()
    {   
        mapVisualizer.ClearMap(); // clear map
        grid = new MapGrid(width, length); // generate a new grid
        MapHelper.RandomlyChooseAndSetStartAndExit(grid, ref startPosition, ref exitPosition, randomPlacement, startEdge, exitEdge); // randomly select start and exit points
        map = new CandidateMap(grid, numberOfPieces);
        map.CreateMap(startPosition, exitPosition);
        mapVisualizer.VisualizeMap(grid, map.ReturnMapData(), false);
    }

    public void TryRepair()
    {
        if (map != null)
        {
            var listOfObstaclesToRemove = map.Repair();
            if (listOfObstaclesToRemove.Count > 0)
            {
                mapVisualizer.ClearMap();
                mapVisualizer.VisualizeMap(grid, map.ReturnMapData(), false);
            }
        }
    }

}
