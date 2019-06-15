
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AI {
    class StationUtil {

        public static bool HasFinishedChopping(ClientWorkstation workstation) {
            const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                           | BindingFlags.Static;
            FieldInfo itemField = workstation.GetType().GetField("m_item", bindFlags);
            ClientWorkableItem workableItem = (ClientWorkableItem)itemField.GetValue(workstation);

            return workableItem.HasFinished();
        }

        public static CookedCompositeOrderNode.CookingProgress GetCookingProgress(GameObject gameObject) {
            ClientCookingHandler cookingHandler = gameObject.GetComponent<ClientCookingHandler>();

            return cookingHandler.GetCookedOrderState();
        }

        public static ArrayList GetIngredientContainerContents(GameObject gameObject) {

            ServerIngredientContainer serverIngredientContainer = gameObject.GetComponent<ServerIngredientContainer>();
            
            AssembledDefinitionNode[] plateContents = serverIngredientContainer.GetContents();

            Logger.Log($"Plate contents size: {plateContents.Length}");

            ArrayList contents = new ArrayList();

            foreach (AssembledDefinitionNode node in plateContents) {

                ItemUtil.GetIngredientsInNode(node, contents);

            }

            return contents;
        }

        public static PickupItemSpawner GetSpawnerForItem(string ingredientName) {
            PickupItemSpawner[] itemSpawners = GameObject.FindObjectsOfType<PickupItemSpawner>();

            return itemSpawners.FirstOrDefault(itemSpawner => itemSpawner.m_itemPrefab.name.Equals(ingredientName));
        }

    }
}
