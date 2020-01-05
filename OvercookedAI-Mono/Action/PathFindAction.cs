using System.Collections.Generic;
using UnityEngine;

namespace AI {

    internal class PathFindAction : IPausableAction {

        private Component target;
        private List<Vector3> path;
        private PlayerControls player;

        private int i;

        private Action currentAction;
        private bool hasCurrentAction;

        public PathFindAction(PlayerControls player, Component component) {
            target = component;
            this.player = player;
            
            GridNavSpace gridNavSpace = GameUtils.GetGridNavSpace();

            Point2 startPoint = gridNavSpace.GetNavPoint(player.transform.position);
            Point2 targetPoint = gridNavSpace.GetNavPoint(component.transform.position);

            path = gridNavSpace.FindPath(startPoint, targetPoint);
            i = 0;
            hasCurrentAction = false;
            
            Logger.Log($"PathFindAction instantiated to {target.name}");
        }

        public bool Update() {
            if (!hasCurrentAction) {
                if (i == path.Count - 1) {
                    currentAction = new MoveTargetAction(player, target);
                } else {
                    currentAction = new MoveAction(player, path[i]);
                }

                i++;

                hasCurrentAction = true;
            } else {
                if (currentAction.Update()) {
                    currentAction.End();
                    hasCurrentAction = false;

                    if (i == path.Count) {
                        Logger.Log($"PathFindAction to {target.name} done");
                        return true;
                    }
                }
            }

            return false;
        }

        public void End() {
            currentAction.End();
        }

        public bool Pause() {
            currentAction.End();

            return true;
        }
    }

}
