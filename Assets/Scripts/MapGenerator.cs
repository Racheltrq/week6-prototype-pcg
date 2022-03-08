using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        MapGrid grid = new MapGrid(5, 5);
        grid.CheckCoordinates();
    }

}
