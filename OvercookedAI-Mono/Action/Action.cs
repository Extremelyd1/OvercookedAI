namespace AI {

    abstract class Action {

        /**
         * Returns true when completed, false when still needs updating
         */
        public abstract bool Update();

        public abstract void End();

    }
}
