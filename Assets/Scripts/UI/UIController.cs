using System;
using DG.Tweening;
using UnityEngine;

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
        [SerializeField] private CanvasGroup _hider;
        [SerializeField] private CanvasGroup _curtain;

        private const float MusicTransitionTime = 0.6f;
        private AudioSource _musicSource;

        private void SetMusic(AudioClip music) {
            var soundController = SoundController.Instance;

            if (_musicSource != null) 
                TransitionVolume(soundController.MusicVolume, 0, UpdateClip);
            else
                UpdateClip();

            void UpdateClip() {
                _musicSource = soundController.PlayMusic(music);
                TransitionVolume(0, soundController.MusicVolume);
            }

            void TransitionVolume(float from, float to, Action onDone = null) {
                _musicSource.volume = from;
                _musicSource.DOFade(to, MusicTransitionTime).OnComplete(() => onDone?.Invoke());
            }
        }

        public void ShowMainMenu(Action onCurtainMiddle = null, bool fromStart = false) {
            _levelWin.Hide(() =>
                _pause.Hide(() => {
                    HideHider();
                    _levelUI.Hide(() => 
                        ShowCurtain(() => {
                            SetMusic(SoundController.Instance.MenuClip);
                            _mainMenu.Show(null);
                            onCurtainMiddle?.Invoke();
                        }, null, fromStart));
                }));
        }

        public void ShowSelectLevel() {
            ShowHider();
            _selectLevel.transform.SetAsLastSibling();
            _selectLevel.OnStartHiding += () => HideHider();
            _selectLevel.Show(null);
        }

        public void ShowSettings(Window invoker) {
            ShowHider();
            _settings.transform.SetAsLastSibling();
            _settings.OnStartHiding += () => {
                if (invoker is MainMenu)
                    HideHider();
            };
            _settings.Show(null, () => {
                invoker.transform.SetAsLastSibling();
            });
        }

        public void ShowCredits(MainMenu invoker) {
            ShowHider();
            _credits.transform.SetAsLastSibling();
            _credits.OnStartHiding += () => HideHider();
            _credits.Show(null);
        }

        public void ShowLevelUI(Action onCurtainMiddle, bool fromLevel = false) {
            _selectLevel.Hide(() =>
                _levelWin.Hide(() =>
                    ShowCurtain(() => {
                            if (!fromLevel)
                                SetMusic(SoundController.Instance.LevelClip);
                            _mainMenu.Hide(null);
                            onCurtainMiddle?.Invoke();
                        }, () => _levelUI.Show(null))));
        }

        public void SetTutorial(string tutorial) {
            _levelUI.SetTutorial(tutorial);
        }

        public void StartRewind() {
            _levelUI.StartRewind();
        }

        public void StopRewind() {
            _levelUI.StopRewind();
        }

        public void ShowLevelWin(Action onClose, int finishedLevelIndex) {
            ShowHider();
            _levelWin.transform.SetAsLastSibling();
            _levelWin.OnStartHiding += () => HideHider();
            _levelWin.SetFinishedLevel(finishedLevelIndex);
            _levelWin.Show(null, onClose);
        }

        private void ShowCurtain(Action onCurtainMiddle, Action onCurtainEnd = null, bool startFromMiddle = false) {
            const float fadeTime = 0.25f;
            const float waitTime = 0.3f;

            _curtain.alpha = 0f;

            DOTween.Sequence()
                .AppendCallback(() => _curtain.gameObject.SetActive(true))
                .Append(_curtain.DOFade(1f, startFromMiddle ? 0.0001f : fadeTime).SetEase(Ease.InOutSine))
                .AppendInterval(startFromMiddle ? 0.0001f : waitTime / 2)
                .AppendCallback(() => onCurtainMiddle?.Invoke())
                .AppendInterval(startFromMiddle ? waitTime : waitTime / 2)
                .Append(_curtain.DOFade(0f, fadeTime).SetEase(Ease.InOutSine))
                .AppendCallback(() => _curtain.gameObject.SetActive(false))
                .AppendCallback(() => onCurtainEnd?.Invoke());
        }

        private void ShowHider(Action onDone = null) {
            const float showTime = 0.5f;
            const float shownAlpha = 0.72f;

            _hider.transform.SetAsLastSibling();
            if (_hider.gameObject.activeSelf) {
                onDone?.Invoke();
                return;
            }
            
            _hider.gameObject.SetActive(true);
            _hider.alpha = 0f;
            _hider
                .DOFade(shownAlpha, showTime)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => onDone?.Invoke());
        }

        private void HideHider(Action onDone = null) {
            const float hideTime = 0.3f;

            if (!_hider.gameObject.activeSelf) {
                onDone?.Invoke();
                return;
            }

            _hider
                .DOFade(0f, hideTime)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => {
                    _hider.gameObject.SetActive(false);
                    onDone?.Invoke();
                });
        }

        public void ShowPause() {
            ShowHider();
            _pause.transform.SetAsLastSibling();
            _pause.OnStartHiding += () => HideHider();
            _pause.Show(null);
        }
        
        public void CloseGame() {
            Application.Quit();
        }
    }
}
