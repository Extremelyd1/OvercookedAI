using System;
using UnityEngine;

namespace AI {

    class MoveTargetAction : MoveAction {

        private Component target;
        
        public MoveTargetAction(PlayerControls player, PickupItemSpawner spawner) : 
            base(player, spawner.transform.position, false) {

            target = spawner;
            
            Logger.Log($"MoveTargetAction instantiated to {target.name}");
        }
        
        public MoveTargetAction(PlayerControls player, ClientPlateStation deliverStation) : 
            base(player, deliverStation.transform.position, false) {

            target = deliverStation;
            
            Logger.Log($"MoveTargetAction instantiated to {target.name}");
        }

        public MoveTargetAction(PlayerControls player, ClientWorkstation workstation) :
            base(player, workstation.transform.position, false) {

            target = workstation;

            Logger.Log($"MoveTargetAction instantiated to {target.name}");
        }

        public MoveTargetAction(PlayerControls player, Component component) :
            base(player, component.transform.position, false) {

            target = component;
            
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
//            float facingDif = PlayerUtil.GetAngleFacingDiff(player, target);
//
////            Logger.Log($"Facing diff: {facingDif}");
//
//            if (facingDif > 40) {
//                Keyboard.Get().SendDown(PlayerUtil.GetInputFacing(player, target));
//
//                return false;
//            }
//
//            Keyboard.Get().StopXMovement();
//            Keyboard.Get().StopZMovement();

            Logger.Log("MoveTargetAction done");

            return true;

        }

    }

}
