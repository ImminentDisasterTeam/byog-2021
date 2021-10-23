using UnityEngine;

namespace UI {
    public class Credits : Window {
        [SerializeField] private UnityEngine.UI.Button _button;

        private void Start() {
            _button.onClick.AddListener(() => Hide(null));
        }
    }
}
