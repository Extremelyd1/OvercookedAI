
using System;
using UnityEngine;

namespace AI {
    class ComponentUtil {

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

        public static Component GetClosestComponent(Vector3 position, Type componentType) {
            UnityEngine.Object[] objects = GameObject.FindObjectsOfType(componentType);
            
            float dist = float.MaxValue;
            Component closestComponent = null;

            for (int i = 0; i < objects.Length; i++) {
                if (objects[i] is Component component) {
                    Vector3 objectPos = component.transform.position;
                    float newDist =
                        new Vector3(position.x - objectPos.x, position.y - objectPos.y, position.z - objectPos.z)
                            .sqrMagnitude;
                    if (newDist < dist) {
                        dist = newDist;
                        closestComponent = component;
                    }
                }
            }

            if (closestComponent != null) {
                return closestComponent;
            }

            return null;
        }

    }
}
