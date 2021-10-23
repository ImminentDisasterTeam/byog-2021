using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelLoader : MonoBehaviour {
    [SerializeField] private Transform _levelRoot;
    [SerializeField] private WallEntity _wallPrefab;
    [SerializeField] private BlockEntity _blockPrefab;
    // [SerializeField] private ButtonEntity _buttonPrefab;
    [SerializeField] private PlayerEntity _playerPrefab;
    [SerializeField] private ExitEntity _exitPrefab;

    public const string LevelDirectory = "Assets/Levels/";

    private const string WALL = "W";
    private const string BLOCK = "B";
    private const string BUTTON = "D";
    private const string ENABLED_BUTTON = "E";
    private const string PLAYER = "P";
    private const string EXIT = "Q";
    private const string EMPTY = "";

    public (List<List<Entity>>, PlayerEntity, ExitEntity) LoadLevel(string csvLevel) {
        var strLevel = csvLevel.Split('\n').Select(row => row.Split(',').ToArray()).ToArray();

        PlayerEntity player = null;
        ExitEntity exit = null;
        var level = new List<List<Entity>>();
        for (var i = 0; i < strLevel.Length; i++) {
            level.Add(new List<Entity>());
            for (var j = 0; j < strLevel[i].Length; j++) {
                var token = strLevel[i][j].Trim('\r');
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
                        Debug.Log($"UNSUPPORTED BUTTON TOKEN: \"{token}\"; {i} {j}");
                        prefab = null;
                        break; // TODO
                    default:
                        throw new ApplicationException($"UNKNOWN TOKEN \"{token}\"; {i} {j}");
                }

                if (prefab == null) {
                    level[i].Add(null);
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

                level[i].Add(toAdd);
            }
        }
        
        if (player == null)
            throw new ApplicationException("0 PLAYERS");
        if (exit == null)
            throw new ApplicationException("0 EXITS");

        return (level, player, exit);
    }
}
