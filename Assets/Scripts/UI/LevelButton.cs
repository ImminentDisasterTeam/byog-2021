using TMPro;
using UnityEngine;

namespace UI {
    public class LevelButton : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private UnityEngine.UI.Button _button;

        public string Label {
            get => _label.text;
            set => _label.text = value;
        }
        public UnityEngine.UI.Button Button => _button;
    }
}
