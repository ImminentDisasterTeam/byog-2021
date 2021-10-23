using UI;
using UnityEngine;

public class TestSceneController : MonoBehaviour {
    [SerializeField] private TextAsset _levelText;

    private void Start() {
        LevelController.Instance.StartLevel(_levelText.text, 1);
        UIController.Instance.ShowLevelUI();
    }
}
