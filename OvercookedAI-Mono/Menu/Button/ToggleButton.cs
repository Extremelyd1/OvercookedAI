using UnityEngine;

namespace AI {
    
    internal class ToggleButton : Button {
        
        private readonly ToggleAction toggleAction;
        
        private bool toggled;
        
        public ToggleButton(string display, ToggleAction toggleAction) : base(display) {
            this.toggleAction = toggleAction;
        }

        public override void Draw(ref int drawIndex, bool selected) {
            Rect rectangle = new Rect(BOX_X_OFFSET,  BOX_Y_OFFSET + (BOX_HEIGHT + BOX_MARGIN) * drawIndex, BOX_WIDTH, BOX_HEIGHT);
            GUI.color = new Color(0f, 0f, 0f, 0.5f);
            GUI.DrawTexture(rectangle, Texture2D.whiteTexture);
            GUI.color = selected ? Color.white : Color.grey;
            GUI.Label(rectangle, (selected ? "> " : string.Empty) + (toggled ? "Disable " : "Enable ") + display);
        }

        public override void Execute() {
            toggled = !toggled;

            toggleAction(toggled);
        }
        
    }
}