namespace SokobanGUI;

public class Box() : TileObject
{
    public bool OnGoal;
    
    public Box(bool onGoal) : this()
    {
        OnGoal = onGoal;
    }
}