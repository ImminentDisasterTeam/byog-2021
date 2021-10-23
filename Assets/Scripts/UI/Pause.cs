using System;
using DG.Tweening;
using UnityEngine;

namespace UI {
    public class Pause : Window {
        [SerializeField] private UnityEngine.UI.Button _continueButton;
        [SerializeField] private UnityEngine.UI.Button _restartButton;
        [SerializeField] private UnityEngine.UI.Button _settingsButton;
        [SerializeField] private UnityEngine.UI.Button _exitButton;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _hidedPosition;
        private Vector3 _shownPos;

        private void Awake() {
            _shownPos = _rectTransform.localPosition;
        }

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

        protected override void PerformShow(Action onDone) {

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

    }
}
