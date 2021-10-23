using System;
using UnityEngine;

namespace UI {
    public class SelectLevel : Window {
        [SerializeField] private int _levelsPerRow = 6;
        [SerializeField] private RectTransform _levelRoot;
        [SerializeField] private UnityEngine.UI.Button _backButton;
        [SerializeField] private RectTransform _rowPrefab;
        [SerializeField] private LevelButton _buttonPrefab;

        private void Start() {
            _backButton.onClick.AddListener(() => Hide(null));
        }

        protected override void PerformShow(Action onDone) {
            for (var i = 0; i < _levelRoot.childCount; i++) {
                Destroy(_levelRoot.GetChild(i).gameObject);
            }

            var currentMaxLevel = PlayerPrefs.GetInt(LevelController.LEVEL_KEY, 0);
            var levelController = LevelController.Instance;

            RectTransform row = null;
            for (var i = 0; i < levelController.LevelCount; i++) {
                if (i % _levelsPerRow == 0)
                    row = Instantiate(_rowPrefab, _levelRoot);

                var button = Instantiate(_buttonPrefab, row);
                button.Label = $"{i + 1}";
                button.enabled = i <= currentMaxLevel || levelController.AllLevelsAvilable;

                var index = i;
                button.Button.onClick.AddListener(() => StartLevel(index));
            }

            onDone?.Invoke();
        }

        private void StartLevel(int index) {
            UIController.Instance.ShowLevelUI(this, () => 
                LevelController.Instance.StartLevel(index));
        }
    }
}
