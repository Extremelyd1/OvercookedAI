namespace AI {

    internal interface Action {

        /**
         * Returns true when completed, false when still needs updating
         */
        bool Update();

        void End();

    }
}
