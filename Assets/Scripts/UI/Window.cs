using System;
using UnityEngine;

namespace UI {
    public abstract class Window : MonoBehaviour {
        protected Action OnClose;
        public Action OnStartHiding;
        
        public void Show(Action onDone, Action onClose = null) {
            OnClose = onClose;

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

                var onClose = OnClose;
                OnClose = null;
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
