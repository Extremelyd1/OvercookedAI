using UnityEngine;

namespace AI {

    internal class ChopIngredientAction : IPausableAction {

        private readonly PlayerControls player;

        private int state;
        private Action currentAction;

        private readonly ClientWorkstation workstation;

        public ChopIngredientAction(PlayerControls player) {
            this.player = player;
            state = 0;

            workstation = ComponentUtil.GetClosestMatchingComponent<ClientWorkstation>(
                player.transform.position, IsChoppingNotBlocked
            );
            
            currentAction = 
                new PathFindAction(
                    player, 
                    workstation
                );
            
            Logger.Log("ChopIngredientAction instantiated");
        }

        public bool Update() {
            switch (state) {
                case 0:
                    if (currentAction.Update()) {
                        currentAction.End();
                        state = 1;
                        currentAction = new PickDropAction(player);
                    }

                    return false;
                case 1:
                    if (currentAction.Update()) {
                        currentAction.End();
                        state = 2;
                        currentAction = new ChopAction(player, workstation);
                    }
                    
                    return false;
                case 2:
                    if (currentAction.Update()) {
                        currentAction.End();
                        state = 3;
                        currentAction = new PickDropAction(player);
                    }

                    return false;
                case 3:
                    if (currentAction.Update()) {
                        currentAction.End();

                        return true;
                    }

                    return false;
                default:
                    return false;
            }
        }

        public void End() {
            currentAction.End();
        }

        public bool IsChoppingNotBlocked(Component component) {
            if (component is ClientWorkstation clientWorkstation) {
                ClientAttachStation clientAttachStation = (ClientAttachStation) ReflectionUtil.GetValue(clientWorkstation, "m_attachStation");
                return clientAttachStation.InspectItem() == null;
            }

            return false;
        }

        public bool Pause() {
            if (state == 1) {
                return false;
            }
            
            if (currentAction is IPausableAction pausableAction) {
                return pausableAction.Pause();
            }

            return false;
        }
    }

}
