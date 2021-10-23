using TMPro;
using UnityEngine;

namespace UI {
    public class LevelUI : Window {
        [SerializeField] private UnityEngine.UI.Button _pauseButton;
        [SerializeField] private GameObject _tutorial;
        [SerializeField] private TextMeshProUGUI _tutorialText;

        private void Start() {
            _pauseButton.onClick.AddListener(() => UIController.Instance.ShowPause());
        }

        public void SetTutorial(string tutorial) {
            if (string.IsNullOrEmpty(tutorial)) {
                _tutorial.SetActive(false);
                return;
            }

            _tutorial.SetActive(true);
            _tutorialText.text = tutorial;
        }
    }
}
