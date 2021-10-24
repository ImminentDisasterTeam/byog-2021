using System;
using DG.Tweening;
using UnityEngine;

namespace UI {
    public class Settings : Window {
        [SerializeField] private UnityEngine.UI.Slider _soundsSlider;
        [SerializeField] private UnityEngine.UI.Slider _musicSlider;
        [SerializeField] private UnityEngine.UI.Button _backButton;

        private void Start() {
            var soundController = SoundController.Instance;
            _soundsSlider.value = soundController.SoundVolume;
            _musicSlider.value = soundController.MusicVolume;
            _soundsSlider.onValueChanged.AddListener(val => soundController.SoundVolume = val);
            _musicSlider.onValueChanged.AddListener(val => soundController.MusicVolume = val);

            _backButton.onClick.AddListener(() => Hide(null));
        }

        public void OnEndDrag() {
            var soundController = SoundController.Instance;
            soundController.PlaySound(soundController.ButtonPressClip);
        }

        protected override void PerformShow(Action onDone) {
            _soundsSlider.value = SoundController.Instance.SoundVolume;
            _musicSlider.value = SoundController.Instance.MusicVolume;
            const float showTime = 0.5f;

            transform.localScale = Vector3.zero;
            transform
                .DOScale(Vector3.one, showTime)
                .SetEase(Ease.OutBack)
                .OnComplete(() => onDone?.Invoke());
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
