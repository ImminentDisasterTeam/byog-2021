using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelLoader : MonoBehaviour {
    [SerializeField] private WallEntity _wallPrefab;
    [SerializeField] private BlockEntity _blockPrefab;
    [SerializeField] private BlockEntity _sliderPrefab;
    [SerializeField] private PlayerEntity _playerPrefab;
    [SerializeField] private ExitEntity _exitPrefab;
    [SerializeField] private Button _buttonPrefab;
    [SerializeField] private Floor _floorPrefab;
    [SerializeField] private AntiButton _antiButtonPrefab;
    [SerializeField] private Sprite _greenWall;
    [SerializeField] private Sprite _yellowWall;
    [SerializeField] private Sprite _orangeWall;
    [SerializeField] private Sprite _redWall;
    [SerializeField] private Sprite _iceWall;
    [SerializeField] private Sprite _normalFloor;
    [SerializeField] private Sprite _iceFloor;
    [SerializeField] private Sprite _normalDoor;
    [SerializeField] private Sprite _iceDoor;

    public const string LevelDirectory = "Assets/Levels/";

    private const string WALL = "W";
    private const string BLOCK = "B";
    private const string SLIDER = "I";
    private const string BUTTON = "D";
    private const string PRESSED_BUTTON = "E";
    private const string ANTIBUTTON = "Y";
    private const string PRESSED_ANTIBUTTON = "N";
    private const string PLAYER = "P";
    private const string EXIT = "Q";
    private const string EMPTY = "";
    private const string SPACE = " ";

    public (List<List<Entity>>, List<List<Button>>, List<List<Floor>>, PlayerEntity, ExitEntity) LoadLevel(Transform levelRoot, string csvLevel,
        LevelController.DecorationType decorationType) {
        var strLevel = csvLevel.Split('\n').Select(row => row.Split(',').ToArray()).ToArray();

        PlayerEntity player = null;
        ExitEntity exit = null;
        var level = new List<List<Entity>>();
        var buttons = new List<List<Button>>();
        var floors = new List<List<Floor>>();
        for (var i = 0; i < strLevel[0].Length; i++) {
            buttons.Add(new List<Button>());
            floors.Add(new List<Floor>());
            level.Add(new List<Entity>());
            for (var j = 0; j < strLevel.Length; j++) {
                buttons[i].Add(null);
                floors[i].Add(null);
                level[i].Add(null);
            }
        }

        for (var i = 0; i < strLevel.Length; i++) {
            for (var j = 0; j < strLevel[i].Length; j++) {
                var token = strLevel[i][j].Trim('\r');
                Button button = null;
                Entity prefab;
                switch (token) {
                    case WALL:
                        _wallPrefab.Sprite = decorationType switch {
                            LevelController.DecorationType.Green => _greenWall,
                            LevelController.DecorationType.Yellow => _yellowWall,
                            LevelController.DecorationType.Orange => _orangeWall,
                            LevelController.DecorationType.Red => _redWall,
                            LevelController.DecorationType.Ice => _iceWall,
                            _ => throw new ArgumentOutOfRangeException(nameof(decorationType), decorationType, null)
                        };
                        prefab = _wallPrefab;
                        break;
                    case BLOCK:
                        prefab = _blockPrefab;
                        break;
                    case SLIDER:
                        prefab = _sliderPrefab;
                        break;
                    case PLAYER:
                        prefab = _playerPrefab;
                        break;
                    case EXIT:
                        _exitPrefab.ActiveSprite = decorationType switch {
                            LevelController.DecorationType.Ice => _iceDoor,
                            _ => _normalDoor
                        };
                        prefab = _exitPrefab;
                        break;
                    case EMPTY:
                    case SPACE:
                        prefab = null;
                        break;
                    case BUTTON:
                    case PRESSED_BUTTON:
                        prefab = token == PRESSED_BUTTON ? _blockPrefab : null;
                        button = Instantiate(_buttonPrefab, levelRoot);
                        break;
                    case ANTIBUTTON:
                    case PRESSED_ANTIBUTTON:
                        prefab = token == PRESSED_ANTIBUTTON ? _blockPrefab : null;
                        button = Instantiate(_antiButtonPrefab, levelRoot);
                        break;
                    default:
                        throw new ApplicationException($"UNKNOWN TOKEN \"{token}\"; {i} {j}");
                }

                buttons[j][strLevel.Length - i - 1] = button;

                _floorPrefab.Sprite = decorationType switch {
                    LevelController.DecorationType.Ice => _iceFloor,
                    _ => _normalFloor
                };
                floors[j][strLevel.Length - i - 1] = Instantiate(_floorPrefab, levelRoot);

                if (prefab == null) {
                    continue;
                }

                var toAdd = Instantiate(prefab, levelRoot);
                switch (toAdd) {
                    case PlayerEntity p:
                        if (player == null)
                            player = p;
                        else
                            throw new ApplicationException("TOO MANY PLAYERS");
                        break;
                    case ExitEntity e:
                        if (exit == null)
                            exit = e;
                        else
                            throw new ApplicationException("TOO MANY EXITS");
                        break;
                }

                level[j][strLevel.Length - i - 1] = toAdd;
            }
        }
        
        if (player == null)
            throw new ApplicationException("0 PLAYERS");
        if (exit == null)
            throw new ApplicationException("0 EXITS");

        return (level, buttons, floors, player, exit);
    }
}
