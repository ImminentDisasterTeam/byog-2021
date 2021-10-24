using System;
using System.Linq;
using UI;
using UnityEngine;

public class LevelController : MonoBehaviour {
    public static LevelController Instance { get; private set; }

    public const string LEVEL_KEY = "LEVEL";
    [SerializeField] private Level[] _levels;
    [SerializeField] private bool _allLevelsAvailable;

    public int LevelCount => _levels.Length;
    private int CurrentMaxLevel {
        get => PlayerPrefs.GetInt(LEVEL_KEY, 0);
        set {
            PlayerPrefs.SetInt(LEVEL_KEY, value);
            PlayerPrefs.Save();
        }
    }

    public int MaxLevelAvailable => _allLevelsAvailable ? _levels.Length - 1 : CurrentMaxLevel;

    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Transform _mapRoot;

    private readonly Map _map;
    private readonly GameLogic _gameLogic;
    private Level _currentLevel;
    private int _currentLevelIndex;

    public LevelController() {
        if (Instance != null)
            throw new ApplicationException("ONLY ONE UICONTROLLER ALLOWED");
        Instance = this;

        _map = new Map();
        _gameLogic = new GameLogic(_map);
        _map.OnButtonsUpdate = _gameLogic.CheckButtons;
    }

    public void StartLevel(int levelIndex) {
        _currentLevelIndex = levelIndex;
        var levelData = _levels[levelIndex];
        var (level, buttons, floors, player, exit) = _levelLoader.LoadLevel(_mapRoot, levelData.levelText.text, levelData.decorationType);

        UIController.Instance.SetTutorial(levelData.tutorial);

        var buttonList = (from row in buttons from button in row where button != null select button).ToList();
        _gameLogic.SetEntities(player, exit, buttonList, this);
        _map.SetMap(level, buttons, floors);

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
        StartLevel(_currentLevelIndex);
    }

    public void FinishLevel(bool success) {
        if (success) {
            if (CurrentMaxLevel == _currentLevelIndex)
                CurrentMaxLevel++;
            UIController.Instance.ShowLevelWin(() => _map.Clear(), _currentLevelIndex);
            return;
        }

        UIController.Instance.ShowMainMenu(() => _map.Clear());
    }

    [Serializable]
    public struct Level {
        public TextAsset levelText;
        public DecorationType decorationType;
        public string tutorial;
    }

    public enum DecorationType {
        Green,
        Yellow,
        Orange,
        Red,
        Ice
    }
}
