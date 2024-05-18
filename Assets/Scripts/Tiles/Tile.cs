using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tile : MonoBehaviour
{

    public string tileName;
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject highlight, highlight2, highlight3;

    public BaseUnit OccupiedUnit;
    public bool Walkable => OccupiedUnit == null;

    public void Init(bool isOffset)
    {
        spriteRenderer.color = isOffset ? offsetColor : baseColor;
    }

    private void OnMouseEnter()
    {
        highlight.SetActive(true);
        MenuManager.Instance.ShowTileInfo(this);
    }

    private void OnMouseExit()
    {
        if (UnitManager.Instance.SelectedHero != null) // if there is a piece selected
        {
            highlight.SetActive(false); //set the highlight to inactive
            if (!UnitManager.Instance.SelectedHero.possibleLocationTiles.ContainsValue(this)) // if the tile pointed at is in the range of motion (of the piece selected)
            {
                highlight2.SetActive(false); // set the movement highlight to inactive
                MenuManager.Instance.ShowTileInfo(null);
            }
        }
        else
        {
            highlight.SetActive(false);
            highlight2.SetActive(false);
            highlight3.SetActive(false);
            MenuManager.Instance.ShowTileInfo(null);
        }
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.GameState == GameState.WhitesTurn)
        {
            TakeTurn(Faction.White);
        }
        else if (GameManager.Instance.GameState == GameState.BlacksTurn)
        {
            TakeTurn(Faction.Black);
        }
    }

    public void TakeTurn(Faction faction)
    {
        if (OccupiedUnit != null)
        {
            if (OccupiedUnit.Faction == faction) // if the unit which is clicked on a white piece
            {
                setHighlightsInactive();

                if (faction == Faction.White) // check faction
                {
                    UnitManager.Instance.SetSelectedHero(OccupiedUnit); // set selected piece to the piece on the clicked tile    
                }
                else if (faction == Faction.Black)
                {
                    UnitManager.Instance.SetSelectedHero(OccupiedUnit); // set selected piece to the piece on the clicked tile (was baseblack)
                }

                UnitManager.Instance.SelectedHero.getPossibleLocationTiles(); // clear and reinitialize possible location tiles
                foreach (var tile in UnitManager.Instance.SelectedHero.possibleLocationTiles)
                {
                    tile.Value.highlight2.SetActive(true);

                    bool isOpposingKing = tile.Value.OccupiedUnit != null && tile.Value.OccupiedUnit.GetType().IsInstanceOfType(new GameObject().AddComponent<King>());

                    if (isOpposingKing) // highlight the king red if it is in the range of motion
                    {
                        tile.Value.highlight2.SetActive(false);
                        tile.Value.highlight3.SetActive(true);
                    }
                }
            }
            else
            {
                if (UnitManager.Instance.SelectedHero != null) // white is already selected and wants to destroy a black piece
                {
                    if (UnitManager.Instance.SelectedHero.possibleLocationTiles.ContainsKey(this.transform.position))
                    {
                        BaseUnit enemy = null;
                        enemy = OccupiedUnit; //new

                        if (enemy.GetType().IsInstanceOfType(new GameObject().AddComponent<King>())) // when a king is eaten
                        {
                            GameManager.Instance.ChangeState(GameState.GameOver);
                            SceneManager.LoadScene(2); // go to game over screen
                        }

                        Destroy(enemy.gameObject); // idk if this deletes them from unitmanager-list
                        SetUnit(UnitManager.Instance.SelectedHero);
                        UnitManager.Instance.SelectedHero.firstTurn = false;

                        UnitManager.Instance.SetSelectedHero(null);

                        if (faction == Faction.White) // check faction
                        {
                            GameManager.Instance.ChangeState(GameState.BlacksTurn);
                        }
                        else if (faction == Faction.Black)
                        {
                            GameManager.Instance.ChangeState(GameState.WhitesTurn);
                        }

                        setHighlightsInactive();

                    }
                }
            }
        }
        else
        {
            if (UnitManager.Instance.SelectedHero != null) // white is already selected and wants to go to an empty tile
            {
                if (UnitManager.Instance.SelectedHero.possibleLocationTiles.ContainsKey(this.transform.position))
                {
                    SetUnit(UnitManager.Instance.SelectedHero);
                    UnitManager.Instance.SelectedHero.firstTurn = false;

                    UnitManager.Instance.SetSelectedHero(null);
                    if (faction == Faction.White) // check faction
                    {
                        GameManager.Instance.ChangeState(GameState.BlacksTurn);
                    }
                    else if (faction == Faction.Black)
                    {
                        GameManager.Instance.ChangeState(GameState.WhitesTurn);
                    }
                    setHighlightsInactive();

                }
            }
        }
    }

    public void SetUnit(BaseUnit unit)
    {
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }

    public void setHighlightsInactive()
    {
        foreach (var tile in GridManager.Instance.getTiles()) // solves movement highlights not being removed after choosing a white piece after another
        {
            tile.Value.highlight.SetActive(false);
            tile.Value.highlight2.SetActive(false);
            tile.Value.highlight3.SetActive(false);
        }
    }

}
