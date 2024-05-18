using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private GameObject selectedHeroObject, tileObject, tileUnitObject, turnObject;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowTileInfo(Tile tile)
    {
        if (tile == null) // no tile is selected
        {
            tileObject.SetActive(false); 
            tileUnitObject.SetActive(false);
            return;
        }
        tileObject.GetComponentInChildren<Text>().text = tile.tileName;
        tileObject.SetActive(true);

        if (tile.OccupiedUnit) // there is a unit on the tile
        {
            tileUnitObject.GetComponentInChildren<Text>().text = tile.OccupiedUnit.unitName;
            tileUnitObject.SetActive(true);
        }
    }

    public void ShowSelectedHero(BaseUnit hero)
    {
        if (hero == null)
        {
            selectedHeroObject.SetActive(false);
            return;
        }
        selectedHeroObject.GetComponentInChildren<Text>().text = hero.unitName;
        selectedHeroObject.SetActive(true);
    }

    public void ShowTurnInfo()
    {
        if (GameManager.Instance.GameState != GameState.WhitesTurn && GameManager.Instance.GameState != GameState.BlacksTurn)
        {
            turnObject.SetActive(false);
            return;
        }
        else
        {           
            turnObject.GetComponentInChildren<Text>().text = (GameManager.Instance.GameState == GameState.WhitesTurn ? "White's Turn" : "Black's Turn");
            turnObject.SetActive(true);
        }
    }
}
