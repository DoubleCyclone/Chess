using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private int width, height;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform camera;

    private Dictionary<Vector2, Tile> tiles;

    public Dictionary<Vector2,Tile> getTiles()
    {
        return tiles;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        camera.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);

        GameManager.Instance.ChangeState(GameState.SpawnWhites);
    }

    public Tile GetHeroSpawnTile()
    {
        return tiles.Where(t => t.Key.y <= 1 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    public Tile GetEnemySpawnTile()
    {
        return tiles.Where(t => t.Key.y >= 6 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
        //return tiles.Where(t => t.Key.y > height / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    public Tile GetTileAtPosition(Vector2 pos) //probably unused
    {
        if(tiles.TryGetValue(pos,out var Tile))
        {
            return Tile;
        }
        return null;
    }
}
