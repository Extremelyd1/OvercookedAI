using System;
using UnityEngine;

namespace AI {

    class MoveTargetAction : MoveAction {

        private Component target;
        
        public MoveTargetAction(PlayerControls player, PickupItemSpawner spawner) : 
            base(player, spawner.transform.position, true) {

            target = spawner;
            
            Logger.Log("MoveTargetAction instantiated");
        }
        
        public MoveTargetAction(PlayerControls player, ClientPlateStation deliverStation) : 
            base(player, deliverStation.transform.position, true) {

            target = deliverStation;
            
            Logger.Log("MoveTargetAction instantiated");
        }

        public MoveTargetAction(PlayerControls player, Component component) :
            base(player, component.transform.position, true) {

            target = component;
            
            Logger.Log("MoveTargetAction instantiated");
        }

        public override bool Update() {
            bool baseResult = base.Update();

            if (!baseResult) {
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
