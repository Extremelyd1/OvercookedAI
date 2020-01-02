using UnityEngine;

namespace AI {

    internal class Button {

        public static readonly float BOX_WIDTH = 300f;
        public static readonly float BOX_HEIGHT = 20f;

        public static readonly float BOX_X_OFFSET = 25f;
        public static readonly float BOX_Y_OFFSET = 47f;
        public static readonly float BOX_MARGIN = 2f;
        
        protected readonly string display;
        
        public Button(string display) {
            this.display = display;
        }

        public virtual void Draw(ref int drawIndex, bool selected) {
            Rect rectangle = new Rect(BOX_X_OFFSET,  BOX_Y_OFFSET + (BOX_HEIGHT + BOX_MARGIN) * drawIndex, BOX_WIDTH, BOX_HEIGHT);
            GUI.color = new Color(0f, 0f, 0f, 0.5f);
            GUI.DrawTexture(rectangle, Texture2D.whiteTexture);
            GUI.color = selected ? Color.white : Color.grey;
            GUI.Label(rectangle, (selected ? "> " : string.Empty) + display);
        }

        public virtual void Execute() {
        }
        
    }
}