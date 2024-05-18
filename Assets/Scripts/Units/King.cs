using System.Linq;
using UnityEngine;

public class King : BaseUnit
{
    private void Start()
    {
        firstTurn = true;
    }
    public override void getPossibleLocationTiles()
    {
        possibleLocationTiles.Clear();

        validityCheck(new Vector2(OccupiedTile.transform.position.x + 1, OccupiedTile.transform.position.y), false);
        validityCheck(new Vector2(OccupiedTile.transform.position.x - 1, OccupiedTile.transform.position.y), false);
        validityCheck(new Vector2(OccupiedTile.transform.position.x, OccupiedTile.transform.position.y + 1), false);
        validityCheck(new Vector2(OccupiedTile.transform.position.x, OccupiedTile.transform.position.y - 1), false);
        validityCheck(new Vector2(OccupiedTile.transform.position.x + 1, OccupiedTile.transform.position.y + 1), false);
        validityCheck(new Vector2(OccupiedTile.transform.position.x + 1, OccupiedTile.transform.position.y - 1), false);
        validityCheck(new Vector2(OccupiedTile.transform.position.x - 1, OccupiedTile.transform.position.y + 1), false);
        validityCheck(new Vector2(OccupiedTile.transform.position.x - 1, OccupiedTile.transform.position.y - 1), false);

        checkMate();

        removeBlockedTiles();

        //kinda good but checkmate doesn't really understand the range of the pieces (blocked tiles and pawn diagonal movement)
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

    public void checkMate()
    {
        BaseUnit[] unitArray = new BaseUnit[32];
        unitArray = FindObjectsOfType<BaseUnit>();

        int i = 0;

        foreach (BaseUnit unit in unitArray) // get units one by one
        {
            if (unit.Faction == this.Faction)
            {
                continue; // don't count this unit
            }
            var tiles = unit.getTiles();

            Debug.Log(unit.unitName + i++);
            foreach (Tile tile in tiles.Values)
            {
                Debug.Log(tile.name); // THEY ALL NEED TO BE CLICKED ON ONCE AT LEAST, MAKE IT SO THAT IT CONSIDERS ALL WHEN THE GAME STARTS
            }

            foreach (Vector2 key in tiles.Keys.ToList<Vector2>()) // get the tiles of the unit one by one
            {
                if (this.getTiles().ContainsKey(key))
                {
                    this.possibleLocationTiles.Remove(key);
                }
            }
        }
    }
}

