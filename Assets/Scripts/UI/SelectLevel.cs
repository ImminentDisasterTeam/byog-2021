using System;
using DG.Tweening;
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

            var levelController = LevelController.Instance;

            RectTransform row = null;
            for (var i = 0; i < levelController.LevelCount; i++) {
                if (i % _levelsPerRow == 0)
                    row = Instantiate(_rowPrefab, _levelRoot);

                var button = Instantiate(_buttonPrefab, row);
                button.Label = $"{i + 1}";
                button.Button.interactable = i <= levelController.MaxLevelAvailable;

                var index = i;
                button.Button.onClick.AddListener(() => StartLevel(index));
            }

            const float showTime = 0.5f;

            transform.localScale = Vector3.zero;
            transform
                .DOScale(Vector3.one, showTime)
                .SetEase(Ease.OutBack)
                .OnComplete(() => onDone?.Invoke());
        }

        private void StartLevel(int index) {
            UIController.Instance.ShowLevelUI(() => 
                LevelController.Instance.StartLevel(index));
        }

        protected override void PerformHide(Action onDone) {
            const float hideTime = 0.3f;

            transform.localScale = Vector3.one;
            transform
                .DOScale(Vector3.zero, hideTime)
                .SetEase(Ease.InSine)
                .OnComplete(() => onDone?.Invoke());
        }
    }
}
