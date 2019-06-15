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

        public static bool HasFinishedChopping(ClientWorkstation workstation) {
            return workstation.IsBeingUsed();
        }

    }
}
