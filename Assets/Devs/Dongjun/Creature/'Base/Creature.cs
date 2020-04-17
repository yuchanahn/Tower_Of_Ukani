using UnityEngine;

public enum CreatureType
{
    Player,
    Wildlife,
    ChildOfUkani,
    Mutant
}

public interface Creature
{
    CreatureType CreatureType { get; }
    StatusID StatusID { get; }
}
