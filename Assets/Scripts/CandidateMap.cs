using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandidateMap
{
    public MapGrid grid;
    private int numberOfPieces = 0;
    private bool[] pbstaticArray = null;
    private Vector3 startPoint, exitPoint;

    public MapGrid Grid { get => grid; }
    public bool[] ObstaclesArray { get => obstaclesArray; }

    public CandidateMap(MapGrid grid, int numberOfPieces)
    {
        this.numberOfPieces = numberOfPieces;
        this.grid = grid;
    }

    private bool CheckIfPositionCanBeObstacle(Vector3 position)
    {
        if (position == startPoint || position == exitPoint)
        {
            return false;
        }
        int index = grid.CalculateIndexFromCoordinates(position.x, position.z);

        return obstacleArray[index] == false;
    }
}
