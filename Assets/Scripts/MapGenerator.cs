using System.Runtime.InteropServices;
using System.Diagnostics;
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
    private List<Vector3> path;

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

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            GenerateNewMap();
        }
        PublicVars.timePassed += Time.deltaTime;
        //UnityEngine.Debug.Log((int)PublicVars.timePassed);
        
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Level Passed: " + PublicVars.levelPassed);
        GUI.Label(new Rect(10, 25, 100, 20), "Time Passed: " + (int)PublicVars.timePassed);
    }

    public void SpawnCar()
    {
        Transform start = GameObject.FindGameObjectWithTag("Start").transform;
        //Vector3 startPos = start.position + start.forward * 0.1f;
        //UnityEngine.Debug.Log(start.position);
        
        Vector3 startPos = path[1] + start.forward * 0.3f;
        //UnityEngine.Debug.Log(startPos);
        if (startPos.x > 14) 
        {
            startPos.x = 14;
        }
        if (startPos.x < 1) 
        {
            startPos.x = 1;
        }
        if (startPos.z > 14) 
        {
            startPos.z = 14;
        }
        if (startPos.z < 1) 
        {
            startPos.z = 1;
        }
        UnityEngine.Debug.Log(startPos);
        startPos.y += 1;
        startPos.x += 0.5f;
        Vector3 relativePos = path[1] - path[0];
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        currCar = Instantiate(cars[Random.Range(0, cars.Length)], startPos, rotation);
        
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
            path = map.ReturnMapData().path;
        } while (path.Count < 18);
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
