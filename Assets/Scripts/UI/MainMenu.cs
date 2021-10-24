using UnityEngine;

namespace UI {
    public class MainMenu : Window {
        [SerializeField] private UnityEngine.UI.Button _selectLevelButton;
        [SerializeField] private UnityEngine.UI.Button _settingsButton;
        [SerializeField] private UnityEngine.UI.Button _creditsButton;
        [SerializeField] private UnityEngine.UI.Button _exitButton;

        private void Start() {
            _selectLevelButton.onClick.AddListener(() => UIController.Instance.ShowSelectLevel());
            _settingsButton.onClick.AddListener(() => UIController.Instance.ShowSettings(this));
            _creditsButton.onClick.AddListener(() => UIController.Instance.ShowCredits(this));
            _exitButton.onClick.AddListener(() => UIController.Instance.CloseGame());

            #if UNITY_WEBGL
            _exitButton.gameObject.SetActive(false);
            #endif
        }
    }
}
