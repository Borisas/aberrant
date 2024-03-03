
using UnityEngine;

namespace SimpleUi {
    public class Director : MonoBehaviour {
        [Sirenix.OdinInspector.ReadOnly] [SerializeField] private View[] _views = null;

        protected virtual void Awake() {
            _views = GetComponentsInChildren<View>();
        }

        protected virtual void Start() { }

        public void CloseAllViews() {
            for (int i = 0; i < _views.Length; i++) {
                _views[i].gameObject.SetActive(false);
            }
        }

        public void OpenView<T>(bool animate = true) where T : View {
            var view = GetView<T>();
            if (view == null) return;
            CloseAllViews();
            view.gameObject.SetActive(true);
        }

        public void OpenViewAdditive<T>(bool animate = true) where T : View {
            var view = GetView<T>();
            if (view == null) return;
            view.gameObject.SetActive(true);
        }

        public T GetView<T>() where T : View {

            for (int i = 0; i < _views.Length; i++) {
                if (_views[i].GetType() == typeof(T))
                    return _views[i] as T;
            }

            return null;
        }
    }
}
