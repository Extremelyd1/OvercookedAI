using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AI {
    class OrderUtil {

        public static ArrayList GetOrders(ClientKitchenFlowControllerBase flowController) {
            ArrayList recipes = new ArrayList();

            ClientOrderControllerBase orderController = flowController.GetMonitorForTeam(TeamID.One).OrdersController;

            const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                           | BindingFlags.Static;
            FieldInfo activeOrderField = orderController.GetType().GetField("m_activeOrders", bindFlags);

            IEnumerable activeOrderFieldEnumerable = (IEnumerable) activeOrderField.GetValue(orderController);

            IEnumerator enumerator = activeOrderFieldEnumerable.GetEnumerator();
            while (enumerator.MoveNext()) {
                FieldInfo activeOrderRecipeListEntry = enumerator.Current.GetType().GetField("RecipeListEntry", bindFlags);
                RecipeList.Entry entry = (RecipeList.Entry) activeOrderRecipeListEntry.GetValue(enumerator.Current);

                recipes.Add(entry.m_order.name);
            }

            return recipes;
        }

        public static string GetNewOrder(ClientKitchenFlowControllerBase flowController) {
            ArrayList orders = GetOrders(flowController);

            int index = 1;
            
            foreach (string order in orders) {
                Logger.Log($"Order #{index++}: {order}");
            }
            
            return (string) orders[0];
        }

        public static string PredictOrderByPlayer(PlayerControls playerControls) {
            throw new NotImplementedException();
        }

        public static string GetIngredientFromOrder(string order) {
            return ItemUtil.GetIngredientsForOrder(order)[0];
        }

        public static string GetRemainingIngredientFromOrder(string order, List<string> ingredients) {
            List<string> ingredientsList = new List<string>();
            ingredientsList.AddRange(ingredients);
            string[] orderIngredients = ItemUtil.GetIngredientsForOrder(order);

            foreach (string ingredient in orderIngredients) {
                if (!ingredientsList.Contains(ingredient)) {
                    return ingredient;
                }
                ingredientsList.Remove(ingredient);
            }

            return "";
        }

    }
}
