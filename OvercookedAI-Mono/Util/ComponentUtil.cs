
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AI {
    internal static class ComponentUtil {

        public delegate bool ComponentFunction(Component component);
        
        public static T GetClosestComponent<T>(Vector3 position) where T : Component {
            T[] objects = Object.FindObjectsOfType<T>();

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

        public static T GetClosestMatchingComponent<T>(Vector3 position, ComponentFunction matchFunction) where T : Component {
            T[] objects = Object.FindObjectsOfType<T>();

            float dist = float.MaxValue;
            int closestIndex = -1;

            for (int i = 0; i < objects.Length; i++) {
                if (!matchFunction(objects[i])) {
                    continue;
                }
                
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

        public static Component GetObjectLocationComponent(Component component) {
            ClientAttachStation[] attachStations = Object.FindObjectsOfType<ClientAttachStation>();
            foreach (ClientAttachStation attachStation in attachStations) {
                if (attachStation.InspectItem() != null) {
                    if (attachStation.InspectItem().Equals(component.gameObject)) {
                        return attachStation;
                    }
                }
            }

            return null;
        }

        public static Component GetPlateLocationComponent(ClientPlate plate) {
            Component locationComponent = GetObjectLocationComponent(plate);
            if (locationComponent != null) {
                return locationComponent;
            }

            ClientPlateReturnStation[] plateReturnStations = GameObject.FindObjectsOfType<ClientPlateReturnStation>();
            
            const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                           | BindingFlags.Static;
            
            foreach (ClientPlateReturnStation plateReturnStation in plateReturnStations) {
                Type plateReturnStationType = plateReturnStation.GetType();

                FieldInfo stackFieldInfo = plateReturnStationType.GetField("m_stack", bindFlags);

                ClientPlateStackBase plateStackBase = (ClientPlateStackBase) stackFieldInfo
                    .GetValue(plateReturnStation);

                stackFieldInfo = plateStackBase.GetType().GetField("m_stack", bindFlags);
                
                ClientStack clientStack = (ClientStack) stackFieldInfo.GetValue(plateStackBase);

                stackFieldInfo = clientStack.GetType().GetField("m_stackItems", bindFlags);
                
                List<GameObject> stackItems = (List<GameObject>) stackFieldInfo.GetValue(clientStack);

                if (stackItems.Contains(plate.gameObject)) {
                    return plateReturnStation;
                }
            }

            return null;
        }

        public static bool IsPlateOnComponent(ClientPlate plate) {
            return GetPlateLocationComponent(plate) != null;
        }

    }
}
