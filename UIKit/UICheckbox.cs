﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;

namespace HEROsMod.UIKit
{
    class UICheckbox : UIView
    {
        static Texture2D checkboxTexture = UIView.GetEmbeddedTexture("Images/UIKit/checkBox");
        static Texture2D checkmarkTexture = UIView.GetEmbeddedTexture("Images/UIKit/checkMark");

        private bool selected = false;
        public bool Selected
        {
            get { return selected; }
            set 
            {
                if (value != selected)
                {
                    selected = value; 
                    if(SelectedChanged != null)
                        SelectedChanged(this, EventArgs.Empty);
                }
            }
        }
        public string Text
        {
            get { return label.Text; }
            set { label.Text = value; }
        }

        public event EventHandler SelectedChanged;
        const int spacing = 4;

        UILabel label;

        

        public UICheckbox(string text)
        {
            label = new UILabel(text);
            label.Scale = .5f;
            label.Position = new Vector2(checkboxTexture.Width + spacing, 0);
            this.AddChild(label);
            this.onLeftClick += new EventHandler(UICheckbox_onLeftClick);
        }

        void UICheckbox_onLeftClick(object sender, EventArgs e)
        {
            this.Selected = !Selected;
        }

        protected override float GetHeight()
        {
            return label.Height;
        }

        protected override float GetWidth()
        {
            return checkboxTexture.Width + spacing + label.Width;
        }

        

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 pos = DrawPosition + new Vector2(0, (float)label.Height / 2 - (float)checkboxTexture.Height/1.2f);
            spriteBatch.Draw(checkboxTexture, pos, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0f);
            if(Selected)
                spriteBatch.Draw(checkmarkTexture, pos, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0f);

            base.Draw(spriteBatch);
        }
    }
}
