using System;
using UnityEngine;

namespace UI {
    public class LevelWin : Window {
        [SerializeField] private UnityEngine.UI.Button _nextLevelButton;
        [SerializeField] private UnityEngine.UI.Button _exitButton;
        private int _finishedLevelIndex;

        private void Start() {
            _nextLevelButton.onClick.AddListener(StartNextLevel);
            _exitButton.onClick.AddListener(() => {
                UIController.Instance.ShowMainMenu();
            });
        }

        public void SetFinishedLevel(int index) {
            _finishedLevelIndex = index;
        }

        protected override void PerformShow(Action onDone) {
            _nextLevelButton.gameObject.SetActive(_finishedLevelIndex < LevelController.Instance.LevelCount - 1);
            onDone?.Invoke();
        }

        private void StartNextLevel() {
            UIController.Instance.ShowLevelUI(() => 
                LevelController.Instance.StartLevel(_finishedLevelIndex + 1));
        }
    }
}
