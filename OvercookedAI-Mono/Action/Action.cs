namespace AI {

    abstract class Action {

        public abstract void Initialize();

        /**
         * Returns true when completed, false when still needs updating
         */
        public abstract bool Update();

        public abstract void End();

    }
}
