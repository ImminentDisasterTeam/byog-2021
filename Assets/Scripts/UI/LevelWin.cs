using System;
using DG.Tweening;
using UnityEngine;

namespace UI {
    public class LevelWin : Window {
        [SerializeField] private UnityEngine.UI.Button _nextLevelButton;
        [SerializeField] private UnityEngine.UI.Button _exitButton;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _hidedPosition;
        private int _finishedLevelIndex;
        private Vector3 _shownPos;

        private void Awake() {
            _shownPos = _rectTransform.localPosition;
        }

        private void Start() {
            _nextLevelButton.onClick.AddListener(StartNextLevel);
            _exitButton.onClick.AddListener(() => {
                var onCurtainMiddle = OnClose;
                OnClose = null;
                UIController.Instance.ShowMainMenu(onCurtainMiddle);
            });
        }

        public void SetFinishedLevel(int index) {
            _finishedLevelIndex = index;
        }

        protected override void PerformShow(Action onDone) {
            _nextLevelButton.gameObject.SetActive(_finishedLevelIndex < LevelController.Instance.LevelCount - 1);

            const float showTime = 0.5f;

            _rectTransform.localPosition = _hidedPosition.localPosition;
            _rectTransform
                .DOLocalMove(_shownPos, showTime)
                .SetEase(Ease.OutBack)
                .OnComplete(() => onDone?.Invoke());
        }

        protected override void PerformHide(Action onDone) {
            const float hideTime = 0.3f;

            _rectTransform.localPosition = _shownPos;
            _rectTransform
                .DOLocalMove(_hidedPosition.localPosition, hideTime)
                .SetEase(Ease.InSine)
                .OnComplete(() => onDone?.Invoke());
        }

        private void StartNextLevel() {
            UIController.Instance.ShowLevelUI(() => 
                LevelController.Instance.StartLevel(_finishedLevelIndex + 1), true);
        }
    }
}
