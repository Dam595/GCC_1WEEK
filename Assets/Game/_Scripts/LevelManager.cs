using UnityEngine;
using System.IO;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelEditor levelEditor;
    [SerializeField] private float nextLevelDelay = 2f;
    [SerializeField] private StrokeUI strokeUI;


    private float timer = 0f;
    private bool isWaitingNextLevel = false;

    private void OnDisable()
    {
            GameManager.Instance.OnGameWin -= OnGameWin;
    }

    private void OnGameWin()
    {
        isWaitingNextLevel = true;
        timer = 0f;
    }
    private void Start()
    {
        GameManager.Instance.OnGameWin += OnGameWin;
    }
    private void Update()
    {
        if (!isWaitingNextLevel) return;

        timer += Time.deltaTime;
        if (timer >= nextLevelDelay)
        {
            isWaitingNextLevel = false;
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        int nextLevel = levelEditor != null ? levelEditor.LevelNumber + 1 : 1;
        string folder = Path.Combine(Application.streamingAssetsPath, "Levels");
        string nextPath = Path.Combine(folder, $"level_{nextLevel}.json");

        if (File.Exists(nextPath))
        {
            levelEditor.SetLevelNumber(nextLevel);
            levelEditor.LoadLevel();
            GameManager.Instance.ResetGame();
        }

        if (strokeUI != null)
            strokeUI.ResetStroke();
    }
}
