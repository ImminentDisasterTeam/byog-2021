using System;
using System.Linq;
using UI;
using UnityEngine;

public class LevelController : MonoBehaviour {
    public static LevelController Instance { get; private set; }

    public const string LEVEL_KEY = "LEVEL";

    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private CameraController _cameraController;
    
    private readonly Map _map;
    private readonly GameLogic _gameLogic;
    private string _currentLevel;
    private int _currentLevelIndex;

    public LevelController() {
        if (Instance != null)
            throw new ApplicationException("ONLY ONE UICONTROLLER ALLOWED");
        Instance = this;

        _map = new Map();
        _gameLogic = new GameLogic(_map);
        _map.OnButtonsUpdate = _gameLogic.CheckButtons;
    }

    public void StartLevel(string levelText, int levelIndex) {
        _currentLevel = levelText;
        _currentLevelIndex = levelIndex;
        var (level, buttons, player, exit) = _levelLoader.LoadLevel(levelText);

        var buttonList = (from row in buttons from button in row where button != null select button).ToList();
        _gameLogic.SetEntities(player, exit, buttonList, this);
        _map.SetMap(level, buttons);

        var levelSize = new Vector2Int(level.Count, level[0].Count);
        _cameraController.Set(levelSize);

        player.OnPlayerMove += direction => {
            var log = "";
            if (direction == Vector2Int.up) {
                log = "W";
            } else if (direction == Vector2Int.down) {
                log = "S";
            } else if (direction == Vector2Int.right) {
                log = "D";
            } else if (direction == Vector2Int.left) {
                log = "A";
            }

            Debug.LogWarning(log);
        };
        player.OnPlayerReset += () => Debug.LogWarning("R");
        _gameLogic.OnLevelWin = () => FinishLevel(true);
    }

    public void RestartLevel() {
        StartLevel(_currentLevel, _currentLevelIndex);
    }

    public void FinishLevel(bool success) {
        _map.Clear();
        if (success) {
            var currentMaxLevel = PlayerPrefs.GetInt(LEVEL_KEY, 1);
            if (currentMaxLevel == _currentLevelIndex) {
                PlayerPrefs.SetInt(LEVEL_KEY, currentMaxLevel + 1);
                PlayerPrefs.Save();
            }
        }

        UIController.Instance.ShowMainMenu();
    }
}
