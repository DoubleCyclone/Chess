using System.Linq;
using UnityEngine;

class Knight : BaseUnit
{
    public override void getPossibleLocationTiles()
    {
        possibleLocationTiles.Clear();

        validityCheck(new Vector2(OccupiedTile.transform.position.x + 2, OccupiedTile.transform.position.y + 1), false);
        validityCheck(new Vector2(OccupiedTile.transform.position.x + 2, OccupiedTile.transform.position.y - 1), false);
        validityCheck(new Vector2(OccupiedTile.transform.position.x + 1, OccupiedTile.transform.position.y + 2), false);
        validityCheck(new Vector2(OccupiedTile.transform.position.x - 1, OccupiedTile.transform.position.y + 2), false);
        validityCheck(new Vector2(OccupiedTile.transform.position.x - 2, OccupiedTile.transform.position.y + 1), false);
        validityCheck(new Vector2(OccupiedTile.transform.position.x - 2, OccupiedTile.transform.position.y - 1), false);
        validityCheck(new Vector2(OccupiedTile.transform.position.x + 1, OccupiedTile.transform.position.y - 2), false);
        validityCheck(new Vector2(OccupiedTile.transform.position.x - 1, OccupiedTile.transform.position.y - 2), false);

        removeBlockedTiles();
    }

    public override void removeBlockedTiles()
    {
        foreach (Tile tile in possibleLocationTiles.Values.ToList<Tile>())
        {
            if (tile.OccupiedUnit != null && tile.OccupiedUnit.Faction == this.Faction)
            {
                possibleLocationTiles.Remove(tile.transform.position);
            }
        }
    }

    public override void validityCheck(Vector2 vector, bool pawn)
    {
        if ((vector.x >= 0 && vector.x <= 7) && (vector.y >= 0 && vector.y <= 7))
        {
            possibleLocationTiles.Add(vector, GridManager.Instance.getTiles()[vector]); // add the tile to the possible tiles dictionary             
        }
    }
}

