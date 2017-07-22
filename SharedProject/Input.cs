using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject
{
    public abstract class Input
    {
        public Vector2 vec;

        public bool isTouched;

        public virtual void Init() { }

        public virtual void Update() { }

    }
}
