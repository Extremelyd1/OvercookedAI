namespace AI {
    internal class Bot {

        private bool executing;

        private string currentOrder;
        private Action currentAction;
        
        public void Init() {
            executing = false;

            currentOrder = "";
            currentAction = null;
        }

        public void Start() {
            executing = true;
        }

        public void End() {
            executing = false;
            
            currentOrder = "";
            currentAction = null;
        }

        public void ToggleExecution() {
            if (IsExecuting()) {
                End();
            } else {
                Start();
            }
        }

        public void Update() {
            if (!executing) {
                return;
            }
            
            if (currentOrder.Equals("")) {
                currentOrder = OrderUtil.GetNewOrder(ObjectUtil.GetFlowController());
                Logger.Log("Getting new order");
            } else {
                bool orderExists = false;
                foreach (string order in OrderUtil.GetOrders(ObjectUtil.GetFlowController())) {
                    if (order.Equals(currentOrder)) {
                        orderExists = true;
                        break;
                    }
                }

                if (!orderExists) {
                    if (currentAction is IPausableAction cancellableCurrentAction) {
                        cancellableCurrentAction.Pause();
                    }

                    currentAction = null;

                    currentOrder = OrderUtil.GetNewOrder(ObjectUtil.GetFlowController());
                    Logger.Log("Getting new order");
                }
            }
            // We should have an order to work on
            if (currentAction == null) {
                currentAction = new HandleOrderAction(ObjectUtil.GetBotControls(), currentOrder);                    
            }

            if (currentAction.Update()) {
                currentAction.End();
                currentAction = null;
                currentOrder = "";
            }
        }

        public bool IsExecuting() {
            return executing;
        }
        
    }
}