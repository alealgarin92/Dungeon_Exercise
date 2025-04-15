using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator
{
    private Vector2 dungeonSize;
    private int startPos;
    private int maxIterations;
    private List<Cell> board;

    public MazeGenerator(Vector2 dungeonSize, int startPos, int maxIterations = 1000)
    {
        this.dungeonSize = dungeonSize;
        this.startPos = startPos;
        this.maxIterations = maxIterations;
        board = new List<Cell>();
    }

    public List<Cell> GenerateMaze()
    {
        int boardLength = (int)(dungeonSize.x * dungeonSize.y);
        for (int i = 0; i < boardLength; i++)
        {
            board.Add(new Cell());
        }

        int currentCell = startPos;
        Stack<int> path = new Stack<int>();
        int iterations = 0;

        while (iterations < maxIterations)
        {
            iterations++;
            board[currentCell].visited = true;

            // Condición de término: se llega a la última celda.
            if (currentCell == board.Count - 1)
            {
                break;
            }

            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);
                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                // Se actualiza el estado de las paredes/puertas entre celdas.
                if (newCell > currentCell)
                {
                    // Puede ser down o right.
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    // Puede ser up o left.
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }
        }

        return board;
    }

    private List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();
        int width = (int)dungeonSize.x;

        // Check Up
        if (cell - width >= 0 && !board[cell - width].visited)
        {
            neighbors.Add(cell - width);
        }
        // Check Down
        if (cell + width < board.Count && !board[cell + width].visited)
        {
            neighbors.Add(cell + width);
        }
        // Check Right
        if ((cell + 1) % width != 0 && !board[cell + 1].visited)
        {
            neighbors.Add(cell + 1);
        }
        // Check Left
        if (cell % width != 0 && !board[cell - 1].visited)
        {
            neighbors.Add(cell - 1);
        }

        return neighbors;
    }
}
