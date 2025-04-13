// Clase responsable de generar la lógica del laberinto (SRP)
using System.Collections.Generic;
using UnityEngine;
using static DungeonGenerator;

public class MazeGenerator
{
    private Vector2 size;
    private int startPos;
    private List<Cell> board;

    public MazeGenerator(Vector2 dungeonSize, int startCell)
    {
        size = dungeonSize;
        startPos = startCell;
        CreateBoard();
    }

    public List<Cell> Generate()
    {
        int currentCell = startPos;
        Stack<int> path = new Stack<int>();

        int steps = 0;
        while (steps < 1000)
        {
            steps++;
            board[currentCell].visited = true;
            if (currentCell == board.Count - 1) break;

            List<int> neighbors = GetUnvisitedNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0) break;
                currentCell = path.Pop();
            }
            else
            {
                path.Push(currentCell);
                int newCell = neighbors[Random.Range(0, neighbors.Count)];
                SetPassage(currentCell, newCell);
                currentCell = newCell;
            }
        }

        return board;
    }

    private void CreateBoard()
    {
        board = new List<Cell>();
        int total = Mathf.FloorToInt(size.x * size.y);
        for (int i = 0; i < total; i++) board.Add(new Cell());
    }

    private List<int> GetUnvisitedNeighbors(int index)
    {
        List<int> neighbors = new List<int>();

        int up = Mathf.FloorToInt(index - size.x);
        if (up >= 0 && !board[up].visited) neighbors.Add(up);

        int down = Mathf.FloorToInt(index + size.x);
        if (down < board.Count && !board[down].visited) neighbors.Add(down);

        if ((index + 1) % size.x != 0 && !board[index + 1].visited) neighbors.Add(index + 1);
        if (index % size.x != 0 && !board[index - 1].visited) neighbors.Add(index - 1);

        return neighbors;
    }

    private void SetPassage(int current, int next)
    {
        if (next > current)
        {
            if (next - 1 == current)
            {
                board[current].status[2] = true;
                board[next].status[3] = true;
            }
            else
            {
                board[current].status[1] = true;
                board[next].status[0] = true;
            }
        }
        else
        {
            if (next + 1 == current)
            {
                board[current].status[3] = true;
                board[next].status[2] = true;
            }
            else
            {
                board[current].status[0] = true;
                board[next].status[1] = true;
            }
        }
    }
}