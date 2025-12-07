namespace AdventOfCode2025.Days.Day04Group;

public class PaperRoll
{
    public bool Removed { get; set; }
    public bool CanRemove => !Removed && AdjacentRolls.Count(roll => !roll.Removed) < 4;

    public List<PaperRoll> AdjacentRolls { get; } = [];

    public void RemoveIfAble()
    {
        if (CanRemove)
        {
            Removed = true;
            foreach (var roll in AdjacentRolls)
            {
                roll.RemoveIfAble();
            }
        }
    }
}
