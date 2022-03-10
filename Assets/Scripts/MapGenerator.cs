using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GridVisualizer gridVisualizer;
    public MapVisualizer mapVisualizer;
    public Direction startEdge, exitEdge;
    public bool randomPlacement;
    [Range(1, 10)]
    public int numberOfPieces;
    private Vector3 startPosition, exitPosition;
    public bool visualizeUsingPrefabs = false;
    public bool autoRepair = true;

    public GameObject[] cars;

    //[Range(3,20)]
    public int width, length = 11;
    private MapGrid grid;
    private CandidateMap map;

    private GameObject currCar;

    // Start is called before the first frame update
    private void Start()
    {

        gridVisualizer.VisualizeGrid(width, length);
        GenerateNewMap();

    }

    public void SpawnCar()
    {
        Transform start = GameObject.FindGameObjectWithTag("Start").transform;
        Vector3 startPos = start.position + start.forward * 0.1f;
        Debug.Log(start.position);
        Debug.Log(start.forward);
        currCar = Instantiate(cars[Random.Range(0, cars.Length)], startPos, start.rotation);
    }

    public void GenerateNewMap()
    {
        Destroy(currCar);
        do
        {
            mapVisualizer.ClearMap(); // clear map
            grid = new MapGrid(width, length); // generate a new grid
            MapHelper.RandomlyChooseAndSetStartAndExit(grid, ref startPosition, ref exitPosition, randomPlacement, startEdge, exitEdge); // randomly select start and exit points
            map = new CandidateMap(grid, numberOfPieces);
            map.CreateMap(startPosition, exitPosition, autoRepair);
            mapVisualizer.VisualizeMap(grid, map.ReturnMapData(), visualizeUsingPrefabs);
            UnityEngine.Debug.Log(map.ReturnMapData().path.Count);
        } while (map.ReturnMapData().path.Count < 18);
        SpawnCar();

    }

    public void TryRepair()
    {
        if (map != null)
        {
            var listOfObstaclesToRemove = map.Repair();
            if (listOfObstaclesToRemove.Count > 0)
            {
                mapVisualizer.ClearMap();
                mapVisualizer.VisualizeMap(grid, map.ReturnMapData(), visualizeUsingPrefabs);
            }
        }
    }

}
