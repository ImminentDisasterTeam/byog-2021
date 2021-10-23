using System;
using DG.Tweening;
using UnityEngine;

namespace UI {
    public class Settings : Window {
        [SerializeField] private UnityEngine.UI.Slider _soundsSlider;
        [SerializeField] private UnityEngine.UI.Slider _musicSlider;
        [SerializeField] private UnityEngine.UI.Button _backButton;

        private const string SOUNDS = "SOUNDS";
        private const string MUSIC = "MUSIC";

        private void Start() {
            _soundsSlider.value = PlayerPrefs.GetFloat(SOUNDS, 1f);
            _musicSlider.value = PlayerPrefs.GetFloat(MUSIC, 1f);

            _soundsSlider.onValueChanged.AddListener(val => OnValueChanged(val, SOUNDS));
            _musicSlider.onValueChanged.AddListener(val => OnValueChanged(val, MUSIC));

            _backButton.onClick.AddListener(() => Hide(null));
        }

        private void OnValueChanged(float value, string type) {
            PlayerPrefs.SetFloat(type, value);
            PlayerPrefs.Save();
        }

        protected override void PerformShow(Action onDone) {
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
