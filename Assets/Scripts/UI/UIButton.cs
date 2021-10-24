using UnityEngine;

namespace UI {
    public class UIButton : MonoBehaviour {
        [SerializeField] private UnityEngine.UI.Button _button;

        private void Awake() {
            var soundController = SoundController.Instance;
            _button.onClick.AddListener(() => soundController.PlaySound(soundController.ButtonPressClip));
        }
    }
}
