using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{

    public bool firstTurn;
    public string unitName;
    public Tile OccupiedTile;
    public Faction Faction;
    public int spawnLimit;
    public Dictionary<Vector2, Tile> possibleLocationTiles = new Dictionary<Vector2, Tile>();

    private void Start()
    {
        firstTurn = true;
        getPossibleLocationTiles();
    }

    public abstract void getPossibleLocationTiles();

    public abstract void validityCheck(Vector2 vector, bool pawn);

    public abstract void removeBlockedTiles();

    public Dictionary<Vector2, Tile> getTiles()
    {
        return possibleLocationTiles;
    }

    
}
