using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class UIController : MonoBehaviour {
        public static UIController Instance { get; private set; }

        public UIController() {
            if (Instance != null)
                throw new ApplicationException("ONLY ONE UICONTROLLER ALLOWED");
            Instance = this;
        }

        [SerializeField] private MainMenu _mainMenu;
        [SerializeField] private SelectLevel _selectLevel;
        [SerializeField] private Settings _settings;
        [SerializeField] private Pause _pause;
        [SerializeField] private LevelUI _levelUI;
        [SerializeField] private LevelWin _levelWin;
        [SerializeField] private Credits _credits;
        [SerializeField] private Image _hider;
        [SerializeField] private CanvasGroup _curtain;

        public void ShowMainMenu(Action onCurtainMiddle = null, bool fromStart = false) {
            ShowCurtain(() => {
                _pause.Hide(null);
                _levelUI.Hide(null);
                _levelWin.Hide(null);
                _hider.gameObject.SetActive(false);

                _mainMenu.Show(null);

                onCurtainMiddle?.Invoke();
            }, fromStart);
        }

        public void ShowSelectLevel(MainMenu invoker) {
            _hider.transform.SetAsLastSibling();
            _selectLevel.transform.SetAsLastSibling();
            _selectLevel.Show(null, () => {
                invoker.transform.SetAsLastSibling();
            });
        }

        public void ShowSettings(Window invoker) {
            _hider.gameObject.SetActive(true);
            _hider.transform.SetAsLastSibling();
            _settings.transform.SetAsLastSibling();
            _settings.Show(null, () => {
                invoker.transform.SetAsLastSibling();
                if (invoker is MainMenu)
                    _hider.gameObject.SetActive(false);
            });
        }

        public void ShowCredits(MainMenu invoker) {
            _hider.transform.SetAsLastSibling();
            _credits.transform.SetAsLastSibling();
            _credits.Show(null, () => {
                invoker.transform.SetAsLastSibling();
            });
        }

        public void ShowLevelUI(Window invoker, Action onCurtainMiddle) {
            ShowCurtain(() => {
                _levelWin.Hide(null);
                _mainMenu.Hide(null);
                invoker.Hide(null);
                _hider.gameObject.SetActive(false);

                _levelUI.Show(null);

                onCurtainMiddle?.Invoke();
            });
        }

        public void ShowLevelWin(Action onClose, int finishedLevelIndex) {
            _hider.transform.SetAsLastSibling();
            _levelWin.transform.SetAsLastSibling();
            _levelWin.SetFinishedLevel(finishedLevelIndex);
            _levelWin.Show(null, onClose);
        }

        private void ShowCurtain(Action onCurtainMiddle, bool startFromMiddle = false) {
            const float fadeTime = 0.5f;
            const float waitTime = 0.6f;

            _curtain.alpha = 0f;

            DOTween.Sequence()
                .AppendCallback(() => _curtain.gameObject.SetActive(true))
                .Append(_curtain.DOFade(1f, startFromMiddle ? 0.0001f : fadeTime).SetEase(Ease.InOutSine))
                .AppendInterval(startFromMiddle ? 0.0001f : waitTime / 2)
                .AppendCallback(() => onCurtainMiddle?.Invoke())
                .AppendInterval(startFromMiddle ? waitTime : waitTime / 2)
                .Append(_curtain.DOFade(0f, fadeTime).SetEase(Ease.InOutSine))
                .AppendCallback(() => _curtain.gameObject.SetActive(false));
        }

        public void ShowPause() {
            _hider.gameObject.SetActive(true);
            _hider.transform.SetAsLastSibling();
            _pause.transform.SetAsLastSibling();
            _pause.Show(null, () => {
                _hider.gameObject.SetActive(false);
            });
        }
        
        public void CloseGame() {
            Application.Quit();
        }
    }
}
