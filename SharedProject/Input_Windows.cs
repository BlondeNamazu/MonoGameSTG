using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject
{
    class Input_Windows : Input
    {
        private KeyboardState keyboardState;
        private float coefficient;

        public override void Init()
        {
            base.Init();
            vec = Vector2.Zero;
            coefficient = Game1.ScreenSize.X / 144; 
        }

        public override void Update()
        {
            base.Update();
            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Right)) vec.X += 1.0f*coefficient;
            if (keyboardState.IsKeyDown(Keys.Left)) vec.X -= 1.0f*coefficient;
            if (keyboardState.IsKeyDown(Keys.Up)) vec.Y -= 1.0f*coefficient;
            if (keyboardState.IsKeyDown(Keys.Down)) vec.Y += 1.0f*coefficient;

            isTouched = keyboardState.IsKeyDown(Keys.Z);
        }
    }
}
