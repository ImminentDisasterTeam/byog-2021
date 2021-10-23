using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestSceneController : MonoBehaviour {
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private TextAsset _levelText;

    private void Start() {
        var (level, buttons, player, exit) = _levelLoader.LoadLevel(_levelText.text);

        var buttonList = (from row in buttons from button in row where button != null select button).ToList();
        GameLogic.Instance.SetEntities(player, exit, buttonList, this);
        Map.Instance.SetMap(level, buttons);

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
        GameLogic.Instance.OnLevelWin += () => Debug.LogWarning("YOU WON");
    }
}
