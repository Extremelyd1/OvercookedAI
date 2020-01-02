using UnityEngine;

namespace AI {
    internal class Main : MonoBehaviour {

        private Bot bot;
        private Menu menu;

        public void Start() {
            Logger.Clear();
            Logger.Log("Loaded.");

            bot = new Bot();
            menu = new Menu();
            menu.Start();
        }
        public void Update() {
            bot.Update();
            menu.Update();
            
            Debug.Update();
        }

        public void OnGUI() {
            menu.OnGUI();
        }
    }
}