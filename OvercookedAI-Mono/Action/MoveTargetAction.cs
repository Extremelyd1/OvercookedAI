using System;
using UnityEngine;

namespace AI {

    internal class MoveTargetAction : MoveAction {

        private Component target;

        public MoveTargetAction(PlayerControls player, Component component) :
            base(player, component.transform.position, false) {

            Component locationComponent = ComponentUtil.GetObjectLocationComponent(component);
            if (locationComponent != null) {
                target = locationComponent;
            } else {
                target = component;
            }

            Logger.Log($"MoveTargetAction instantiated to {target.name}");
        }

        public override bool Update() {
            bool baseResult = base.Update();

            // Check whether the player is currently highlighting its target
            bool highlightedTarget = false;
            if (PlayerUtil.HasHighlighted(player)) {
                highlightedTarget = PlayerUtil.GetHighlightedObject(player).Equals(target.gameObject);
            }
            
            if (!baseResult && !highlightedTarget) {
                return false;
            }

            // TODO: face target component before ending
            float facingDif = PlayerUtil.GetAngleFacingDiff(player, target);

            // Logger.Log($"Facing diff: {facingDif}");

            if (facingDif > 40) {
                Keyboard.Input input = PlayerUtil.GetInputFacing(player, target);
                // Logger.Log($"Input: {input}");
                Keyboard.Get().SendDown(input);

                return false;
            }

            Keyboard.Get().StopXMovement();
            Keyboard.Get().StopZMovement();

            Logger.Log("MoveTargetAction done");

            return true;

        }

    }

}
