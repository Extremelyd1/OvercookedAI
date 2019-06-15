using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AI {
    class OrderUtil {

        //private static OrderUtil INSTANCE;

        //private OrderUtil() {
        //}

        //public static OrderUtil Get() {
        //    if (INSTANCE == null) {
        //        INSTANCE = new OrderUtil();
        //    }
        //
        //    return INSTANCE;
        //}

        public static ArrayList GetOrders(ClientKitchenFlowControllerBase flowController) {
            ArrayList recipes = new ArrayList();

            ClientOrderControllerBase orderController = flowController.GetMonitorForTeam(TeamID.One).OrdersController;

            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
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

        public static String GetNewOrder(ClientKitchenFlowControllerBase flowController) {
            return (String) GetOrders(flowController)[0];
        }

        public static String PredictOrderByPlayer(PlayerControls playerControls) {
            throw new NotImplementedException();
        }

        public static String GetIngredientFromOrder(String order) {
            return ItemUtil.GetIngredientsForOrder(order)[0];
        }

        public static String GetRemainingIngredientFromOrder(String order, params String[] ingredients) {
            List<String> ingredientsList = ingredients.OfType<String>().ToList();
            String[] orderIngredients = ItemUtil.GetIngredientsForOrder(order);

            foreach (String ingredient in orderIngredients) {
                if (!ingredientsList.Contains(ingredient)) {
                    return ingredient;
                }
                ingredientsList.Remove(ingredient);
            }

            return "";
        }

    }
}
