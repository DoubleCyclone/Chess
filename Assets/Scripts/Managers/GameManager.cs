using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public GameState GameState;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }

    private void Update()
    {
        MenuManager.Instance.ShowTurnInfo();
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch (newState)
        {                
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnWhites:
                UnitManager.Instance.SpawnWhites();
                break;
            case GameState.SpawnBlacks:
                UnitManager.Instance.SpawnBlacks();
                break;
            case GameState.WhitesTurn:
                break;
            case GameState.BlacksTurn:
                break;
            case GameState.GameOver:            
                break;
            default:
                break;
        }
    }
}



    public enum GameState
{
    GenerateGrid = 0,
    SpawnWhites = 1,
    SpawnBlacks = 2,
    WhitesTurn= 3,
    BlacksTurn = 4,
    GameOver = 5,
}
