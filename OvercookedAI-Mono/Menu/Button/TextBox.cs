using UnityEngine;

namespace AI {
 
    internal class TextBox : Button {

        private readonly ParamFunctionAction functionAction;

        private string speechText;

        public TextBox(string placeholder, ParamFunctionAction functionAction) : base(placeholder) {
            this.functionAction = functionAction;
            speechText = placeholder;
        }

        public override void Draw(ref int drawIndex, bool selected) {
            Rect rectangle = new Rect(BOX_X_OFFSET,  BOX_Y_OFFSET + (BOX_HEIGHT + BOX_MARGIN) * drawIndex, BOX_WIDTH, BOX_HEIGHT);
            GUI.color = selected ? Color.white : Color.grey;
            speechText = GUI.TextField(rectangle, speechText);
            functionAction(speechText);
        }

        public override void Execute() {
        }

    }
}