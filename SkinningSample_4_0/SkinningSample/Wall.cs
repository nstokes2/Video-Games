using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;
using Microsoft.Xna.Framework.Graphics;


namespace SmellOfRevenge2011
{
    public class Wall
    {
        public BoundingSphere Bs;
        public Vector3 Direction;
        public Vector3 Start;
        public Vector3 End;
        public int health;
        private static float WrapAngle(float radians)
        {
            while (radians < -MathHelper.Pi)
            {
                radians += MathHelper.TwoPi;
            }
            while (radians > MathHelper.Pi)
            {
                radians -= MathHelper.TwoPi;
            }
            return radians;
        }
        public float TurnToFace(Vector3 position, Vector3 target, Vector3 rotation)
        {
            float x = (target.X - position.X);
            float z = (target.Z - position.Z);

            float desiredAngle = (float)Math.Atan2(x, z);

            Vector3 tempDir = rotation;
            float difference = WrapAngle(desiredAngle - tempDir.Y);

            difference = MathHelper.Clamp(difference, -20, 20);

            return WrapAngle(tempDir.Y + difference);
        }
        public Wall(Vector3 start, Vector3 end)
    {

        Start = start;
        End = end;
        float rot = TurnToFace(start, end, Vector3.Forward);
            float x = (float)Math.Sin(rot);
            float y = (float)Math.Cos(rot);
            Direction = new Vector3(x, 0.0f, y);

            if(Vector3.Distance(start, end) > 40)
                end = start + Direction * 40;
            Bs = new BoundingSphere((start + end) / 2 + new Vector3(0.0f, 70.0f, 0.0f), 20.0f);
    }
        
    }
}
