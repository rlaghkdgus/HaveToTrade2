using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private GameState curState;

    public GameObject LoanClearPrefab;
    public GameObject BadEndPrefab;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        curState = GameState.Start;
    }

    public void GameStateChange(GameState next)
    {
        curState = next;
    }

    private void OnEnable()
    {
        EventManager.LoanClear += LoanClearEnding;
        EventManager.TownMoveMax += TimeOutBadEnding;
    }

    private void OnDisable()
    {
        EventManager.LoanClear -= LoanClearEnding;
        EventManager.TownMoveMax -= TimeOutBadEnding;
    } 

    private void LoanClearEnding()
    {
        curState = GameState.LoanEnding;
        GameObject endui = Instantiate(LoanClearPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
        // 돈 다 갚았을 때 엔딩
    }

    private void TimeOutBadEnding()
    {
        curState = GameState.BadEnding;
        GameObject endui = Instantiate(BadEndPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
        // 마을 횟수 제한 엔딩
    }
}

public enum GameState
{
    Start,
    Tutorial,
    MainIdle,
    BadEnding,
    LoanEnding
}