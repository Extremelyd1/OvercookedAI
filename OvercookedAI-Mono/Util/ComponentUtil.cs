
using System;
using System.Collections.Generic;
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

            ClientPlateReturnStation[] plateReturnStations = Object.FindObjectsOfType<ClientPlateReturnStation>();
            
            foreach (ClientPlateReturnStation plateReturnStation in plateReturnStations) {
                ClientPlateStackBase plateStackBase =
                    (ClientPlateStackBase) ReflectionUtil.GetValue(plateReturnStation, "m_stack");

                ClientStack clientStack = (ClientStack) ReflectionUtil.GetValue(plateStackBase, "m_stack");

                List<GameObject> stackItems = (List<GameObject>) ReflectionUtil.GetValue(clientStack, "m_stackItems");

                if (stackItems.Contains(plate.gameObject)) {
                    return plateReturnStation;
                }
            }

            return null;
        }

        /**
         * Return whether the attachstation given is not a bin/chopping-board/etc.
         */
        public static bool IsCleanAttachStation(Component targetStation) {
            if (!(targetStation is ClientAttachStation)) {
                return false;
            }
            
            // TODO: fill with all station types
            Dictionary<Type, String> stationMapping = new Dictionary<Type, string> {
                { typeof(ClientWorkstation), "m_attachStation" },
                { typeof(ClientRubbishBin), "m_clientAttachStation" },
                { typeof(ClientCookingStation), "m_attachStation" },
                { typeof(ClientPlateReturnStation), "m_attachStation" },
                { typeof(ClientPlateStation), "m_ClientAttachStation" },
            };

            foreach (Type type in stationMapping.Keys) {
                var stations = Object.FindObjectsOfType(type);
                foreach (var station in stations) {
                    ClientAttachStation attachStation = (ClientAttachStation) ReflectionUtil.GetValue(station, type, stationMapping[type]);
                    if (attachStation.Equals(targetStation)) {
                        return false;
                    }
                }
            }
            
            // Different test for item containers, since they have no associated attach station
            Vector3 targetPos = targetStation.transform.position;
            foreach (ClientPickupItemSpawner spawner in Object.FindObjectsOfType<ClientPickupItemSpawner>()) {
                Vector3 spawnerPos = spawner.transform.position;
                if (new Vector2(spawnerPos.x - targetPos.x, spawnerPos.z - targetPos.z).sqrMagnitude < 0.1) {
                    return false;
                }
            }

            return true;
        }

        public static bool IsPlateOnComponent(ClientPlate plate) {
            return GetPlateLocationComponent(plate) != null;
        }

    }
}
