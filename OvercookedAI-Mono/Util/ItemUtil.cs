using System;
using System.Collections;

namespace AI {
    internal static class ItemUtil {

        public static string[] GetIngredientsForOrder(string orderName) {
            if (orderName.Equals("Sushi_PlainFish")) {
                return new[] { "SushiFish" };
            }
            
            if (orderName.Equals("Sushi_PlainPrawn")) {
                return new[] { "SushiPrawn" };
            }

            if (orderName.Equals("Sushi_Fish")) {
                return new[] { "SushiRice", "Seaweed", "SushiFish" };
            }

            return null;
        }

        public static int GetIngredientsInNode(AssembledDefinitionNode node, ArrayList contents) {
            if (node is IngredientAssembledNode ingredientNode) {
                contents.Add(ingredientNode.m_ingriedientOrderNode.name);

                return 1;
            }
            
            if (node is CompositeAssembledNode compositeNode) {
                int number = 0;

                foreach (AssembledDefinitionNode innerNode in compositeNode.m_composition) {
                    number += GetIngredientsInNode(innerNode, contents);
                }

                return number;
            }
            
            return 0;
        }

        public static bool IsProcessedIngredient(string ingredient, string currentItem) {
            return ingredient.Equals("SushiFish") && currentItem.Equals("ChoppedSushiFish") ||
                   ingredient.Equals("SushiPrawn") && currentItem.Equals("ChoppedSushiPrawn");
        }

    }
}
