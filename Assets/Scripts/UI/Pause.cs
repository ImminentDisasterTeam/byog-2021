using UnityEngine;

namespace UI {
    public class Pause : Window {
        [SerializeField] private UnityEngine.UI.Button _continueButton;
        [SerializeField] private UnityEngine.UI.Button _restartButton;
        [SerializeField] private UnityEngine.UI.Button _settingsButton;
        [SerializeField] private UnityEngine.UI.Button _exitButton;

        private void Start() {
            _continueButton.onClick.AddListener(() => Hide(null));
            _restartButton.onClick.AddListener(() => {
                Hide(null);
                LevelController.Instance.RestartLevel();
            });
            _settingsButton.onClick.AddListener(() => UIController.Instance.ShowSettings(this));
            _exitButton.onClick.AddListener(() => {
                LevelController.Instance.FinishLevel(false);
            });
        }
    }
}
