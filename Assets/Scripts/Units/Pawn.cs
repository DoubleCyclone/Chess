using System.Linq;
using UnityEngine;

public class Pawn : BaseUnit
{
    private int verticalMovement = 0;

    public void Awake()
    {
        if (Faction == Faction.White) verticalMovement = 1;
        else verticalMovement = -1; // doesn't work well ??? for black
    }

    public override void getPossibleLocationTiles()
    {
        possibleLocationTiles.Clear();

        validityCheck(new Vector2(OccupiedTile.transform.position.x, OccupiedTile.transform.position.y + verticalMovement), false);
        if (firstTurn) validityCheck(new Vector2(OccupiedTile.transform.position.x, OccupiedTile.transform.position.y + 2 * verticalMovement), false);
        validityCheck(new Vector2(OccupiedTile.transform.position.x - 1, OccupiedTile.transform.position.y + verticalMovement), true);
        validityCheck(new Vector2(OccupiedTile.transform.position.x + 1, OccupiedTile.transform.position.y + verticalMovement), true);

        removeBlockedTiles();
    }

    public override void removeBlockedTiles()
    {
        foreach (Tile tile in possibleLocationTiles.Values.ToList<Tile>())
        {      
            if (GridManager.Instance.GetTileAtPosition(new Vector2(this.transform.position.x, this.transform.position.y + verticalMovement)).OccupiedUnit != null)
            {
                possibleLocationTiles.Remove(new Vector2(OccupiedTile.transform.position.x, OccupiedTile.transform.position.y + 2 * verticalMovement));
            }
        }
    }

    public override void validityCheck(Vector2 vector, bool isTakingUnit)
    {
        if ((vector.x >= 0 && vector.x <= 7) && (vector.y >= 0 && vector.y <= 7))
        {
            if (isTakingUnit)
            {
                if (GridManager.Instance.getTiles()[vector].OccupiedUnit != null && GridManager.Instance.getTiles()[vector].OccupiedUnit.Faction != this.Faction)
                {
                    possibleLocationTiles.Add(vector, GridManager.Instance.getTiles()[vector]); // add the tile to the possible tiles dictionary
                }
            }
            else
            {
                if (GridManager.Instance.getTiles()[vector].OccupiedUnit == null)
                {
                    possibleLocationTiles.Add(vector, GridManager.Instance.getTiles()[vector]); 
                }
            }
        }
    }
}

