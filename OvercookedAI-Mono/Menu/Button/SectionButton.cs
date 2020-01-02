using System.Collections;
using UnityEngine;

namespace AI {
    
    internal class SectionButton : Button {

        private bool hasParent;
        private SectionButton parent;
        private readonly ArrayList children;

        public SectionButton(string display, Button[] children) : base(display) {
            this.children = new ArrayList();
            this.children.AddRange(children);
            hasParent = false;

            foreach (Button button in children) {
                if (button is SectionButton) {
                    ((SectionButton) button).SetParent(this);
                }
            }
        }
        
        public override void Draw(ref int drawIndex, bool selected) {
            Rect rectangle = new Rect(BOX_X_OFFSET,  BOX_Y_OFFSET + (BOX_HEIGHT + BOX_MARGIN) * drawIndex, BOX_WIDTH, BOX_HEIGHT);
            GUI.color = new Color(0f, 0f, 0f, 0.5f);
            GUI.DrawTexture(rectangle, Texture2D.whiteTexture);
            GUI.color = selected ? Color.white : Color.grey;
            GUI.Label(rectangle, (selected ? "> " : string.Empty) + display);
        }

        public override void Execute() {
        }

        public bool HasParent() {
            return hasParent;
        }

        public SectionButton GetParent() {
            return parent;
        }

        public void SetParent(SectionButton parent) {
            this.parent = parent;
            hasParent = true;
        }

        public ArrayList GetChildren() {
            return children;
        }

        public string GetHeaderText() {
            return display;
        }
    }
}