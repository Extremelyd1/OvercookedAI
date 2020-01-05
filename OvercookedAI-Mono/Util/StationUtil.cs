
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AI {
    internal static class StationUtil {

        public static bool HasFinishedChopping(ClientWorkstation workstation) {
            ClientWorkableItem workableItem = (ClientWorkableItem) ReflectionUtil.GetValue(workstation, "m_item");

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

            foreach (PickupItemSpawner spawner in itemSpawners) {
                if (spawner.m_itemPrefab.name.Equals(ingredientName)) {
                    return spawner;
                }
            }

            return null;
        }

        public static Type GetStationTypeForIngredient(string ingredient) {
            switch (ingredient) {
                case "SushiPrawn":
                case "SushiFish":
                    return typeof(ClientWorkstation);
            }

            return null;
        }

    }
}
