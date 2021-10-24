using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI {
    public class LevelUI : Window {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private UnityEngine.UI.Button _pauseButton;
        [SerializeField] private GameObject _tutorial;
        [SerializeField] private TextMeshProUGUI _tutorialText;
        [SerializeField] private RectTransform _leftRewind;
        [SerializeField] private RectTransform _rightRewind;

        private RectTransform _buttonTransform;
        private RectTransform _tutorialTransform;
        private Vector2 _buttonShownPos;
        private Vector2 _tutorialShownPos;
        private Vector2 _buttonHidedPos;
        private Vector2 _tutorialHidedPos;
        private Vector2 _leftRewindShownPos;
        private Vector2 _rightRewindShownPos;
        private Vector2 _leftRewindHidedPos;
        private Vector2 _rightRewindHidedPos;

        private void Awake() {
            _buttonTransform = (RectTransform) _pauseButton.transform;
            _tutorialTransform = (RectTransform) _tutorial.transform;
            _buttonShownPos = _buttonTransform.anchoredPosition;
            _tutorialShownPos = _tutorialTransform.anchoredPosition;
            _buttonHidedPos = new Vector2(_buttonShownPos.x, -_buttonShownPos.y);
            _tutorialHidedPos = new Vector2(_tutorialShownPos.x, -_tutorialShownPos.y);
            _leftRewindShownPos = _leftRewind.anchoredPosition;
            _rightRewindShownPos = _rightRewind.anchoredPosition;
            _leftRewindHidedPos = new Vector2(-_leftRewindShownPos.x, _leftRewindShownPos.y);
            _rightRewindHidedPos = new Vector2(-_rightRewindShownPos.x, _rightRewindShownPos.y);
        }

        private void Start() {
            _pauseButton.onClick.AddListener(() => UIController.Instance.ShowPause());
            _leftRewind.anchoredPosition = _leftRewindHidedPos;
            _rightRewind.anchoredPosition = _rightRewindHidedPos;
        }

        public void SetTutorial(string tutorial) {
            if (string.IsNullOrEmpty(tutorial)) {
                _tutorial.SetActive(false);
                return;
            }

            _tutorial.SetActive(true);
            _tutorialText.text = tutorial;
        }

        public void StartRewind() {
            _canvasGroup.interactable = false;
            const float showTime = 0.4f;

            _leftRewind.anchoredPosition = _leftRewindHidedPos;
            _rightRewind.anchoredPosition = _rightRewindHidedPos;
            DOTween.Sequence()
                .Insert(0, _leftRewind.DOAnchorPos(_leftRewindShownPos, showTime).SetEase(Ease.OutBack))
                .Insert(0, _rightRewind.DOAnchorPos(_rightRewindShownPos, showTime).SetEase(Ease.OutBack));
        }

        public void StopRewind() {
            const float hideTime = 0.4f;

            _leftRewind.anchoredPosition = _leftRewindShownPos;
            _rightRewind.anchoredPosition = _rightRewindShownPos;
            DOTween.Sequence()
                .Insert(0, _leftRewind.DOAnchorPos(_leftRewindHidedPos, hideTime).SetEase(Ease.InBack))
                .Insert(0, _rightRewind.DOAnchorPos(_rightRewindHidedPos, hideTime).SetEase(Ease.InBack))
                .AppendCallback(() => _canvasGroup.interactable = true);
        }

        protected override void PerformShow(Action onDone) {
            const float showTime = 0.5f;

            _tutorialTransform.anchoredPosition = _tutorialHidedPos;
            _buttonTransform.anchoredPosition = _buttonHidedPos;
            DOTween.Sequence()
                .Insert(0, _tutorialTransform.DOAnchorPos(_tutorialShownPos, showTime).SetEase(Ease.OutBack))
                .Insert(0, _buttonTransform.DOAnchorPos(_buttonShownPos, showTime).SetEase(Ease.OutBack))
                .AppendCallback(() => onDone?.Invoke());
        }

        protected override void PerformHide(Action onDone) {
            const float hideTime = 0.3f;

            _tutorialTransform.anchoredPosition = _tutorialShownPos;
            _buttonTransform.anchoredPosition = _buttonShownPos;
            DOTween.Sequence()
                .Insert(0, _tutorialTransform.DOAnchorPos(_tutorialHidedPos, hideTime).SetEase(Ease.InSine))
                .Insert(0, _buttonTransform.DOAnchorPos(_buttonHidedPos, hideTime).SetEase(Ease.InSine))
                .AppendCallback(() => onDone?.Invoke());
        }
    }
}
