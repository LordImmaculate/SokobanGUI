using System;

namespace SokobanGUI.Models;

public class GameState
{
    public readonly TileObject?[,,] Grid = new TileObject[10, 10, 2];

    private int PlayerX { get; set; }
    private int PlayerY { get; set; }

    public int Rows => Grid.GetLength(0);
    public int Cols => Grid.GetLength(1);

    public bool HasWon;

    public GameState()
    {
        Grid[2, 2, 1] = new Player();
        
        Grid[3, 3, 0] = new Goal();
        Grid[2, 3, 1] = new Box();
        
        Grid[7, 7, 0] = new Goal();
        Grid[6, 6, 1] = new Box();
        
        Grid[8, 8, 0] = new Void();

        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Cols; x++)
            {
                if (x == 0 || y == 0 || x == Cols - 1 || y == Rows - 1)
                {
                    Grid[y, x, 0] = new Wall();
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
        
        switch (Grid[newY, newX, 1])
        {
            case Box:
                int boxX = newX;
                int boxY = newY;
            
                int newBoxX = boxX + dx;
                int newBoxY = boxY + dy;
            
                Goal goal = Grid[newBoxY, newBoxX, 0] as Goal;
            
                if (Grid[newBoxY, newBoxX, 0] != null || Grid[newBoxY, newBoxX, 1] != null) if(goal == null) return;
                
                Grid[boxY, boxX, 1] = null;
                Grid[newBoxY, newBoxX, 1] = new Box();
            
                Grid[PlayerY, PlayerX, 1] = null;
                PlayerX = newX;
                PlayerY = newY;
                Grid[PlayerY, PlayerX, 1] = new Player();
                break;
        }

        switch (Grid[newY, newX, 0])
        {
            case null or Goal:
                Grid[PlayerY, PlayerX, 1] = null;
                PlayerX = newX;
                PlayerY = newY;
                Grid[PlayerY, PlayerX, 1] = new Player();
                break;
        }

        HasWon = CheckGoals();
        
    }

    private bool CheckGoals()
    {
        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Cols; x++)
            {
                if (Grid[y, x, 0] is Goal)
                {
                    if (Grid[y, x, 1] is not Box)
                        return false;
                }
            }
        }
        
        return true;
    }
}