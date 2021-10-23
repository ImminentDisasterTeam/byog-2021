using System;
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
        [SerializeField] private Credits _credits;
        [SerializeField] private Image _hider;

        public void ShowMainMenu() {
            _pause.Hide(() => 
                _levelUI.Hide(() => 
                    _mainMenu.Show(null)));
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

        public void ShowLevelUI(MainMenu invoker = null) {
            invoker ??= _mainMenu;
            invoker.Hide(() => {
                _levelUI.Show(null);
            });
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
