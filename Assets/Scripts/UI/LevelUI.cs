using UnityEngine;

namespace UI {
    public class LevelUI : Window {
        [SerializeField] private UnityEngine.UI.Button _pauseButton;

        private void Start() {
            _pauseButton.onClick.AddListener(() => UIController.Instance.ShowPause());
        }
    }
}
