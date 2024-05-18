using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    public List<ScriptableUnit> heroUnits; //white
    public List<ScriptableUnit> enemyUnits; //black
    public BaseUnit SelectedHero;

    void Awake()
    {
        Instance = this;
        LoadUnits();

    }

    public void LoadUnits()
    {
        heroUnits = new List<ScriptableUnit>();
        enemyUnits = new List<ScriptableUnit>();

        LoadUnitsFromFiles("Units/Heroes", heroUnits);
        LoadUnitsFromFiles("Units/Enemies", enemyUnits);
    }

    private void LoadUnitsFromFiles(string path, List<ScriptableUnit> units)
    {
        List<ScriptableUnit> tempUnits = Resources.LoadAll<ScriptableUnit>(path).ToList();
        for (int i = 0; i < tempUnits.Count(); i++)
        {
            for (int j = 0; j < tempUnits[i].UnitPrefab.spawnLimit; j++)
            {
                units.Add(tempUnits[i]);
            }
        }
    }

    public void SpawnWhites()
    {
        RandomizeUnits();
        SpawnUnits(16, heroUnits);

        GameManager.Instance.ChangeState(GameState.SpawnBlacks);
    }

    public void SpawnBlacks()
    {
        SpawnUnits(16, enemyUnits);

        GameManager.Instance.ChangeState(GameState.WhitesTurn);
    }
    private void SpawnUnits(int unitCount, List<ScriptableUnit> unitList)
    {
        for (int i = 0; i < unitCount; i++)
        {
            var randomPrefab = unitList[i].UnitPrefab;
            var spawnedUnit = Instantiate(randomPrefab);
            Tile randomSpawnTile;
            if (randomPrefab.Faction == Faction.White)
            {
                randomSpawnTile = GridManager.Instance.GetHeroSpawnTile();
            }
            else
            {
                randomSpawnTile = GridManager.Instance.GetEnemySpawnTile();
            }

            randomSpawnTile.SetUnit(spawnedUnit);
        }
    }

    private void RandomizeUnits()
    {
        var rnd = new System.Random();
        heroUnits = heroUnits.OrderBy(item => rnd.Next()).ToList();
        enemyUnits = enemyUnits.OrderBy(item => rnd.Next()).ToList();
    }

    public void SetSelectedHero(BaseUnit hero)
    { 
        SelectedHero = hero;
        MenuManager.Instance.ShowSelectedHero(hero);
    }
}