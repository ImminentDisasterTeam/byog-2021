using System;
using UnityEngine;

namespace UI {
    public abstract class Window : MonoBehaviour {
        private Action _onClose;
        public Action OnStartHiding;
        
        public void Show(Action onDone, Action onClose = null) {
            _onClose = onClose;

            if (gameObject.activeSelf) {
                onDone?.Invoke();
                return;
            }
            
            gameObject.SetActive(true);
            PerformShow(onDone);
        }

        public void Hide(Action onDone) {
            OnStartHiding?.Invoke();
            OnStartHiding = null;

            if (!gameObject.activeSelf) {
                onDone?.Invoke();
                return;
            }
            
            PerformHide(() => {
                gameObject.SetActive(false);
                onDone?.Invoke();

                var onClose = _onClose;
                _onClose = null;
                onClose?.Invoke();
            });
        }

        protected virtual void PerformShow(Action onDone) {
            onDone?.Invoke();
        }
        
        protected virtual void PerformHide(Action onDone) {
            onDone?.Invoke();
        }
    }
}
