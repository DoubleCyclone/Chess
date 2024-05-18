using System.Linq;
using UnityEngine;

class Queen : BaseUnit
{
    public override void getPossibleLocationTiles()
    {
        possibleLocationTiles.Clear();

        for (int i = 1; i < 8; i++)
        {
            validityCheck(new Vector2(OccupiedTile.transform.position.x + i, OccupiedTile.transform.position.y), false); // rook
            validityCheck(new Vector2(OccupiedTile.transform.position.x - i, OccupiedTile.transform.position.y), false);
            validityCheck(new Vector2(OccupiedTile.transform.position.x, OccupiedTile.transform.position.y + i), false);
            validityCheck(new Vector2(OccupiedTile.transform.position.x, OccupiedTile.transform.position.y - i), false);

            validityCheck(new Vector2(OccupiedTile.transform.position.x + i, OccupiedTile.transform.position.y + i), false); // bishop
            validityCheck(new Vector2(OccupiedTile.transform.position.x - i, OccupiedTile.transform.position.y + i), false);
            validityCheck(new Vector2(OccupiedTile.transform.position.x + i, OccupiedTile.transform.position.y - i), false);
            validityCheck(new Vector2(OccupiedTile.transform.position.x - i, OccupiedTile.transform.position.y - i), false);
        }

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
            if (tile.OccupiedUnit != null)
            {
                if(tile.OccupiedUnit.Faction != this.Faction && tile.OccupiedUnit.GetType().IsInstanceOfType(new GameObject().AddComponent<King>()))
                {
                    continue;
                }
                float horizontalDistance = tile.transform.position.x - this.transform.position.x; // positive if the target is to the right of the selected piece, negative otherwise
                float verticalDistance = tile.transform.position.y - this.transform.position.y; // positive if the target is to the north(up) of the selected piece, negative otherwise 
                for (int i = 1; i < 7; i++)
                {// x+3,y-3, (2,-2)
                    possibleLocationTiles.Remove(new Vector2(OccupiedTile.transform.position.x + (horizontalDistance) + i * (horizontalDistance > 0 ? 1 : -1), OccupiedTile.transform.position.y + (verticalDistance) + i * (verticalDistance > 0 ? 1 : -1)));
                    if (verticalDistance > 0 && horizontalDistance == 0)
                    {
                        possibleLocationTiles.Remove(new Vector2(OccupiedTile.transform.position.x, OccupiedTile.transform.position.y + i + verticalDistance));
                    }
                    else if (verticalDistance < 0 && horizontalDistance == 0)
                    {
                        possibleLocationTiles.Remove(new Vector2(OccupiedTile.transform.position.x, OccupiedTile.transform.position.y - i + verticalDistance));
                    }
                    if (horizontalDistance > 0 && verticalDistance == 0)
                    {
                        possibleLocationTiles.Remove(new Vector2(OccupiedTile.transform.position.x + i + horizontalDistance, OccupiedTile.transform.position.y));
                    }
                    else if (horizontalDistance < 0 && verticalDistance == 0)
                    {
                        possibleLocationTiles.Remove(new Vector2(OccupiedTile.transform.position.x - i + horizontalDistance, OccupiedTile.transform.position.y));
                    }
                }

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
