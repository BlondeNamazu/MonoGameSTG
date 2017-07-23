using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject
{
    class Bullet
    {
        public Vector2 pos,speed;

        public Bullet(Vector2 pos)
        {
            this.pos = pos;
            speed = new Vector2(0, -8.0f);
        }

        public void Update()
        {
            pos += speed;
        }

        public bool hitEnemy(Vector2 enemyPos,float range)
        {
            float xd = enemyPos.X - pos.X;
            float yd = enemyPos.Y - pos.Y;
            if (xd * xd + yd * yd < range * range) return true;
            return false;
        }
    }
}
