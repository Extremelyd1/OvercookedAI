using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AI {
    class PlayerUtil {

        public static bool IsCarrying(PlayerControls playerControls) {
            return GetCarrying(playerControls) != "";
        }

        public static String GetCarrying(PlayerControls playerControls) {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                    | BindingFlags.Static;
            FieldInfo clientCarrierField = playerControls.GetType().GetField("m_clientCarrier", bindFlags);
            ClientPlayerAttachmentCarrier clientCarrier = (ClientPlayerAttachmentCarrier)clientCarrierField.GetValue(playerControls);

            FieldInfo carriedObjectsField = clientCarrier.GetType().GetField("m_carriedObjects", bindFlags);
            IClientAttachment[] carriedObjects = (IClientAttachment[])carriedObjectsField.GetValue(clientCarrier);

            //Logger.Get().Log(String.Format("Number of carried objects: {0}", carriedObjects.Length));
            for (int i = 0; i < carriedObjects.Length; i++) {

                //Logger.Get().Log(String.Format("Carried object is null: {0}", carriedObjects[i] == null));
                //Logger.Get().Log(String.Format("CarriedObject Type: {0}", carriedObjects[i].GetType()));
                //Logger.Get().Log(String.Format("Carried game object is null: {0}", carriedObjects[i].AccessGameObject() == null));

                if (carriedObjects[i] != null) {
                    if (carriedObjects[i].AccessGameObject() != null) {
                        return carriedObjects[i].AccessGameObject().name;
                    }
                }
            }

            return "";
        }

        public static Vector3 GetChefPosition(PlayerControls playerControls) {
            return playerControls.transform.position;
        }
        
        public static Vector3 GetChefPosition(int chef) {
            PlayerControls[] playerControls = GameObject.FindObjectsOfType<PlayerControls>();
            
            return playerControls[chef].transform.position;
        }

        public static float GetAngleFacingDiff(PlayerControls player, Component componentToFace) {
            Vector3 playerPos = player.transform.position;
            Vector3 compPos = componentToFace.transform.position;

            float rot = player.transform.rotation.eulerAngles.y;

            float xDif = Math.Abs(playerPos.x - compPos.x);
            float zDif = Math.Abs(playerPos.z - compPos.z);
            
            Logger.Log($"AngleFacingDif method: rot={rot}, xDif={xDif}, zDif={zDif}");

            if (xDif > zDif) {
                if (playerPos.x > compPos.x) {
                    // Should be 270
                    if (rot < 90) {
                        rot += 360;
                    }
                    return Math.Abs(270 - rot);
                }

                // Should be 90
                if (rot > 270) {
                    rot -= 360;
                }
                
                return Math.Abs(90 - rot);
            }
            if (playerPos.z < compPos.z) {
                // Should be 0
                if (rot > 180) {
                    rot -= 360;
                }
                
                return Math.Abs(rot);
            }
            
            // Should be 180 
            return Math.Abs(180 - rot);
        }

        public static Keyboard.Input GetInputFacing(PlayerControls player, Component componentToFace) {
            Vector3 playerPos = player.transform.position;
            Vector3 compPos = componentToFace.transform.position;

            float xDif = Math.Abs(playerPos.x - compPos.x);
            float zDif = Math.Abs(playerPos.z - compPos.z);

            if (xDif > zDif) {
                if (playerPos.x > compPos.x) {
                    return Keyboard.Input.LEFT;
                }

                return Keyboard.Input.RIGHT;
            }
            if (playerPos.z > compPos.z) {
                return Keyboard.Input.DOWN;
            }

            return Keyboard.Input.UP;
        }

    }
}
