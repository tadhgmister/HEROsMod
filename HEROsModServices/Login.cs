﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HEROsMod.UIKit;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
    class Login : HEROsModService
    {
        private static bool _loggedIn = false;
        public static bool LoggedIn
        {
            get { return _loggedIn; }
            set 
            { 
                _loggedIn = value;
				LoginStatusChanged?.Invoke(null, EventArgs.Empty);
			}
        }
        private static Texture2D _loginTexture;
        private static Texture2D _logoutTexture;
        static event EventHandler LoginStatusChanged;

        public Login()
        {
			MultiplayerOnly = true;
            if(_loginTexture == null)
            {
                _loginTexture = UIView.GetEmbeddedTexture("Images/login");
            }
            if(_logoutTexture == null)
            {
                _logoutTexture = UIView.GetEmbeddedTexture("Images/logout");
            }
            this._name = "Login";
            this._hotbarIcon = new UIImage(_loginTexture);
            this._hotbarIcon.onLeftClick += _hotbarIcon_onLeftClick;
            LoginStatusChanged += Login_LoginStatusChanged;
            this.HotbarIcon.Tooltip = "Login";
            this.HasPermissionToUse = true;
        }

        void Login_LoginStatusChanged(object sender, EventArgs e)
        {
			//ErrorLogger.Log("Login_LoginStatusChanged to "+ LoggedIn);
            if(LoggedIn)
            {
                this._hotbarIcon.Texture = _logoutTexture;
                this.HotbarIcon.Tooltip = "Logout";
            }
            else
            {
                this._hotbarIcon.Texture = _loginTexture;
                this.HotbarIcon.Tooltip = "Login";
            }
        }

        void _hotbarIcon_onLeftClick(object sender, EventArgs e)
        {
            if(LoggedIn)
            {
                HEROsModNetwork.LoginService.RequestLogout();
            }
            else
            {
                MasterView.gameScreen.AddChild(new LoginWindow());
            }
        }
        public override void Destroy()
        {
			//ErrorLogger.Log("Destroy");
            LoginStatusChanged -= Login_LoginStatusChanged;
            LoggedIn = false;
            base.Destroy();
        }
    }

    class LoginWindow : UIWindow
    {
        UILabel lPassword = null;
        UITextbox tbPassword = null;
        UITextbox tbUsername = null;
        UILabel lUsername = null;
        static float spacing = 16f;

        public LoginWindow()
        {
            UIView.exclusiveControl = this;

            Width = 600;
            this.Anchor = AnchorPosition.Center;

            lUsername = new UILabel("Username");
            tbUsername = new UITextbox();
            lPassword = new UILabel("Password");
            tbPassword = new UITextbox();
            tbPassword.PasswordBox = true;
            UIButton bLogin = new UIButton("Login");
            UIButton bCancel = new UIButton("Cancel");
            UIButton bRegister = new UIButton("Register");
            bRegister.AutoSize = false;
            bRegister.Width = 100;

            lUsername.Scale = .5f;
            lPassword.Scale = .5f;

            bLogin.Anchor = AnchorPosition.TopRight;
            bCancel.Anchor = AnchorPosition.TopRight;

            lUsername.X = spacing;
            lUsername.Y = spacing;
            tbUsername.X = lUsername.X + lUsername.Width + spacing;
            tbUsername.Y = lUsername.Y;
            lPassword.X = lUsername.X;
            lPassword.Y = lUsername.Y + lUsername.Height + spacing;
            tbPassword.X = tbUsername.X;
            tbPassword.Y = lPassword.Y;

            bCancel.Position = new Vector2(this.Width - spacing, tbPassword.Y + tbPassword.Height + spacing);
            bLogin.Position = new Vector2(bCancel.Position.X - bCancel.Width - spacing, bCancel.Position.Y);
            bRegister.X = spacing;
            bRegister.Y = bCancel.Y;
            this.Height = bCancel.Y + bCancel.Height + spacing;

            bCancel.onLeftClick += bCancel_onLeftClick;
            bLogin.onLeftClick += bLogin_onLeftClick;
            bRegister.onLeftClick += bRegister_onLeftClick;
            tbUsername.OnEnterPress += bLogin_onLeftClick;
            tbPassword.OnEnterPress += bLogin_onLeftClick;
            tbUsername.OnTabPress += tbUsername_OnTabPress;
            tbPassword.OnTabPress += tbPassword_OnTabPress;

            AddChild(lUsername);
            AddChild(tbUsername);
            AddChild(lPassword);
            AddChild(tbPassword);
            AddChild(bLogin);
            AddChild(bCancel);
            AddChild(bRegister);

            tbUsername.Focus();
        }

        void bRegister_onLeftClick(object sender, EventArgs e)
        {
            if (tbUsername.Text.Length > 0 && tbPassword.Text.Length > 0)
            {
                tbUsername.Unfocus();
                tbPassword.Unfocus();
                HEROsModNetwork.LoginService.RequestRegistration(tbUsername.Text, tbPassword.Text);
                Close();
            }
            else
            {
                Main.NewText("Please fill in the Username and Password fields.");
            }
        }

        void tbPassword_OnTabPress(object sender, EventArgs e)
        {
            tbPassword.Unfocus();
            tbUsername.Focus();
        }

        void tbUsername_OnTabPress(object sender, EventArgs e)
        {
            tbUsername.Unfocus();
            tbPassword.Focus();
        }

        void bLogin_onLeftClick(object sender, EventArgs e)
        {
            if (tbUsername.Text.Length > 0 && tbPassword.Text.Length > 0)
            {
                tbUsername.Unfocus();
                tbPassword.Unfocus();
                HEROsModNetwork.LoginService.RequestLogin(tbUsername.Text, tbPassword.Text);
                Close();
            }

            else
            {
                Main.NewText("Please fill in the Username and Password fields.");
            }
        }

        void bCancel_onLeftClick(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override float GetWidth()
        {
            return tbPassword.Width + lPassword.Width + spacing * 4;
        }

        private void Close()
        {
            UIView.exclusiveControl = null;
            this.Parent.RemoveChild(this);
        }

        public override void Update()
        {
            if (Main.gameMenu) this.Close();
            if (Parent != null)
                this.Position = new Vector2(Parent.Width / 2, Parent.Height / 2);
            base.Update();
        }
    }
}
