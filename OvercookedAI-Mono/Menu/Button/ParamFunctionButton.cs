using UnityEngine;

namespace AI {
 
    internal class ParamFunctionButton : Button {

        private readonly ParamFunctionAction functionAction;
        private readonly object param;

        public ParamFunctionButton(string display, ParamFunctionAction functionAction, object param) : base(display) {
            this.functionAction = functionAction;
            this.param = param;
        }

        public override void Draw(ref int drawIndex, bool selected) {
            Rect rectangle = new Rect(BOX_X_OFFSET,  BOX_Y_OFFSET + (BOX_HEIGHT + BOX_MARGIN) * drawIndex, BOX_WIDTH, BOX_HEIGHT);
            GUI.color = new Color(0f, 0f, 0f, 0.5f);
            GUI.DrawTexture(rectangle, Texture2D.whiteTexture);
            GUI.color = selected ? Color.white : Color.grey;
            GUI.Label(rectangle, (selected ? "> " : string.Empty) + display);
        }

        public override void Execute() {
            functionAction(param);
        }

    }
}