using System.Collections;

namespace AI {
    class ItemUtil {

        public static string[] GetIngredientsForOrder(string orderName) {
            if (orderName.Equals("Sushi_PlainFish")) {
                return new [] { "SushiFish" };
            }
            
            if (orderName.Equals("Sushi_PlainPrawn")) {
                return new [] { "SushiPrawn" };
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

    }
}
