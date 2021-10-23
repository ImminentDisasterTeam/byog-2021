using UnityEngine;

public class TestSceneController : MonoBehaviour {
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private TextAsset _levelText;

    private void Start() {
        var (level, player, exit) = _levelLoader.LoadLevel(_levelText.text);
        Map.Instance.SetMap(level);
        GameLogic.Instance.SetEntities(player, exit, this);

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
        player.OnPlayerRewind += () => Debug.LogWarning("R");
        GameLogic.Instance.OnLevelWin += () => Debug.LogWarning("YOU WON");
    }
}
