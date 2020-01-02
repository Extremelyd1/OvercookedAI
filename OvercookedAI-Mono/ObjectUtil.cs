using UnityEngine;

namespace AI {
    internal static class ObjectUtil {

        public static ClientKitchenFlowControllerBase GetFlowController() {
            return Object.FindObjectOfType<ClientKitchenFlowControllerBase>();
        }

        public static PlayerControls GetBotControls() {
            return Object.FindObjectsOfType<PlayerControls>()[1];
        }
    }
}