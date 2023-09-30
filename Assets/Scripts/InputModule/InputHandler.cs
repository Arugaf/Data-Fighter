using UnityEngine;
using UnityEngine.Events;

namespace InputModule {
    public class InputHandler : MonoBehaviour {
        private const int PrimaryButton = 0;

        private void Update() {
            if (Input.GetMouseButtonDown(PrimaryButton)) {
                GotPrimaryMouseButtonDown?.Invoke();
            }

            if (Input.GetMouseButtonUp(PrimaryButton)) GotPrimaryMouseButtonUp?.Invoke();
        }

        public static event UnityAction GotPrimaryMouseButtonDown;
        public static event UnityAction GotPrimaryMouseButtonUp;
    }
}
