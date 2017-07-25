using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SmellOfRevenge2011
{
    public class horizontalForward
    {
        public Matrix World;
        public Vector3 Position;
        public Vector3 NextPos1;
        public Vector3 NextPos2;
        public Vector3 Direction;
        public BoundingSphere BS;
        /// <summary>
        /// 0 is left 1 is right
        /// </summary>
        public int State;
        public horizontalForward(Matrix world, int state)
        {
            World = world;
            Position = world.Translation;
            State = state;
            Direction = world.Forward;
            if(state == 0)
            {
                NextPos1 = Position + world.Left * 100.0f;
                NextPos1 = NextPos1 + world.Forward * 30.0f;

                NextPos2 = Position + world.Right * 100.0f;
                NextPos2 = NextPos2 + world.Forward * 60.0f;
            }

            BS = new BoundingSphere(Position, 30.0f);


        }
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
        public void update(GameTime gameTime)
        {

            float rot = 0.0f;
                
            if(State == 0)
                rot = TurnToFace(Position, NextPos1, new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));
            if(State == 1)
                rot = TurnToFace(Position, NextPos2, new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));


            float x = (float)Math.Sin(rot);
            float y = (float)Math.Cos(rot);
            Direction = new Vector3(x, 0.0f, y);

            float dTo1 = Vector3.Distance(Position, NextPos1);
            float dTo2 = Vector3.Distance(Position, NextPos2);

            if (State == 0)
            {
                if (dTo1 < 10)
                {
                    NextPos1 = NextPos1 + World.Forward * 60;
                    State = 1;
                }
                else
                    Position += Direction * 5.0f;
            }
            if (State == 1)
            {
                if (dTo2 < 10)
                {
                    NextPos2 = NextPos2 + World.Forward * 60;
                    State = 0;
                }
                else
                    Position += Direction * 5.0f ;
            }

            Vector3 Up = Vector3.Up;
            Vector3 Right = Vector3.Cross(Direction, Up);
            BS.Center = Position;
        }




    }
}
