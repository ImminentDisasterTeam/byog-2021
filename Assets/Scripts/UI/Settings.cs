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
    }
}
