using UnityEngine;

namespace UI {
    public class PauseMenu : MonoBehaviour {
        private void Awake() {
            DontDestroyOnLoad(this);
        }
    }
}
