using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelLoader : MonoBehaviour {
    [SerializeField] private Transform _levelRoot;
    [SerializeField] private WallEntity _wallPrefab;
    [SerializeField] private BlockEntity _blockPrefab;
    [SerializeField] private PlayerEntity _playerPrefab;
    [SerializeField] private ExitEntity _exitPrefab;
    [SerializeField] private Button _buttonPrefab;

    public const string LevelDirectory = "Assets/Levels/";

    private const string WALL = "W";
    private const string BLOCK = "B";
    private const string BUTTON = "D";
    private const string ENABLED_BUTTON = "E";
    private const string PLAYER = "P";
    private const string EXIT = "Q";
    private const string EMPTY = "";

    public (List<List<Entity>>, List<List<Button>>, PlayerEntity, ExitEntity) LoadLevel(string csvLevel) {
        var strLevel = csvLevel.Split('\n').Select(row => row.Split(',').ToArray()).ToArray();

        PlayerEntity player = null;
        ExitEntity exit = null;
        var level = new List<List<Entity>>();
        var buttons = new List<List<Button>>();
        for (var i = 0; i < strLevel[0].Length; i++) {
            buttons.Add(new List<Button>());
            level.Add(new List<Entity>());
            for (var j = 0; j < strLevel.Length; j++) {
                buttons[i].Add(null);
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
                        prefab = _wallPrefab;
                        break;
                    case BLOCK:
                        prefab = _blockPrefab;
                        break;
                    case PLAYER:
                        prefab = _playerPrefab;
                        break;
                    case EXIT:
                        prefab = _exitPrefab;
                        break;
                    case EMPTY:
                        prefab = null;
                        break;
                    case BUTTON:
                    case ENABLED_BUTTON:
                        prefab = token == ENABLED_BUTTON ? _blockPrefab : null;
                        button = Instantiate(_buttonPrefab, _levelRoot);
                        break; // TODO
                    default:
                        throw new ApplicationException($"UNKNOWN TOKEN \"{token}\"; {i} {j}");
                }

                buttons[j][strLevel.Length - i - 1] = button;

                if (prefab == null) {
                    continue;
                }

                var toAdd = Instantiate(prefab, _levelRoot);
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

        return (level, buttons, player, exit);
    }
}
