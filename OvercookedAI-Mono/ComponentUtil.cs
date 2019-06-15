
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace AI {
    class ComponentUtil {

        //private static ComponentUtil INSTANCE;

        //private ComponentUtil() {
        //}

        //public static ComponentUtil Get() {
        //    if (INSTANCE == null) {
        //        INSTANCE = new ComponentUtil();
        //    }
        //
        //    return INSTANCE;
        //}

        public static T GetClosestComponent<T>(Vector3 position) where T : Component {
            T[] objects = GameObject.FindObjectsOfType<T>();

            float dist = float.MaxValue;
            int closestIndex = -1;

            for (int i = 0; i < objects.Length; i++) {
                Vector3 objectPos = objects[i].transform.position;
                float newDist = new Vector3(position.x - objectPos.x, position.y - objectPos.y, position.z - objectPos.z).sqrMagnitude;
                if (newDist < dist) {
                    dist = newDist;
                    closestIndex = i;
                }
            }

            if (closestIndex != -1) {
                return objects[closestIndex];
            }

            return null;
        }

    }
}
