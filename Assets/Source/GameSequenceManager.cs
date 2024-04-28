using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSequenceManager : Singleton<GameSequenceManager>
{
    public enum GameState
    {
        Boot,
        Menu,
        SetUp,
        InGame,
        Result,
        Max
    }

    public GameState CurrentGameState { get; protected set; }

    public AsyncOperation GoToNextScene()
    {
        return LoadScene((GameState)Mathf.Max(1, (((int)CurrentGameState + 1) % (int)GameState.Max)));
    }

    public AsyncOperation LoadScene(GameState gameState)
    {
        CurrentGameState = gameState;
        return SceneManager.LoadSceneAsync(gameState.ToString() + "Scene");
    }
}
