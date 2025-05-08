using System;

namespace SokobanGUI.Models;

public class GameState
{
    public readonly TileObject?[,] Grid = new TileObject[10, 10];

    private int PlayerX { get; set; }
    private int PlayerY { get; set; }
    
    private int IsPlayerOnGoal { get; set; }

    public int Rows => Grid.GetLength(0);
    public int Cols => Grid.GetLength(1);

    public GameState()
    {
        Grid[2, 3] = new Box(false);
        Grid[2, 2] = new Player();
        Grid[3, 3] = new Goal();

        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Cols; x++)
            {
                if (x == 0 || y == 0 || x == Cols - 1 || y == Rows - 1)
                {
                    Grid[y, x] = new Wall();
                }
            }
        }

        PlayerX = 2;
        PlayerY = 2;
    }

    public void Move(int dx, int dy)
    {
        int newX = PlayerX + dx;
        int newY = PlayerY + dy;

        switch (Grid[newY, newX])
        {
            case null:
                Grid[PlayerY, PlayerX] = null;
                PlayerX = newX;
                PlayerY = newY;
                Grid[PlayerY, PlayerX] = new Player();
                break;
            
            case Box:
                int boxX = newX;
                int boxY = newY;

                int newBoxX = boxX + dx;
                int newBoxY = boxY + dy;
                
                Box box = Grid[boxY, boxX] as Box;

                Goal goal = Grid[newBoxY, newBoxX] as Goal;

                if (Grid[newBoxY, newBoxX] != null) if(goal == null) return;

                if (box.OnGoal) Grid[boxY, boxX] = new Goal();
                Grid[boxY, boxX] = null;
                Grid[newBoxY, newBoxX] = new Box(goal != null);

                Grid[PlayerY, PlayerX] = null;
                PlayerX = newX;
                PlayerY = newY;
                Grid[PlayerY, PlayerX] = new Player();
                break;
        }
    }
}