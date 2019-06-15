using UnityEngine;

namespace AI {

    public class Loader {

        private static GameObject _load;

        public static void Init() {
            _load = new GameObject();
            _load.AddComponent<Main>();
            GameObject.DontDestroyOnLoad(_load);
        }
        public static void Unload() {
            _Unload();
        }
        private static void _Unload() {
            GameObject.Destroy(_load);
        }


    }
}
