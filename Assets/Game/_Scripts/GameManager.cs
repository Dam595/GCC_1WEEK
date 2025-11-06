using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnGameWin;
    public event Action OnGameLose;

    public enum GameState { Playing, Win, Lose, Paused }
    public GameState CurrentState { get; private set; } = GameState.Playing;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void WinGame()
    {
        if (CurrentState != GameState.Playing) return;
        CurrentState = GameState.Win;
        OnGameWin?.Invoke();
        Debug.Log("wins!");
    }

    public void LoseGame()
    {
        if (CurrentState != GameState.Playing) return;
        CurrentState = GameState.Lose;
        OnGameLose?.Invoke();
        Debug.Log("loses!");
    }

    public void ResetGame()
    {
        CurrentState = GameState.Playing;
        Debug.Log("Game restart");
    }
}
