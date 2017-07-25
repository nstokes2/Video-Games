using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SmellOfRevenge2011
{
    public class horizontalCircle
    {
        public float height = 0.0f;
        double currentDouble = 0.0;
        public Matrix World;
        public Vector3 Position;
        public Vector3 NextPos1;
        public Vector3 NextPos2;
        public Vector3 Direction;
        public BoundingSphere BS;
        /// <summary>
        /// 0 is left 1 is right
        /// </summary>
        /// 
        Vector3 midPoint;
            //List<List<BoundingSphere>> spheres = new List<List<BoundingSphere>>();
            //for (int i = 0; i < 5; i++)
            //{
            //    spheres.Add(new List<BoundingSphere>());
            //    for (int j = 0; j < i * 2 + 1; j++)
            //    {
            //        spheres[i].Add(new BoundingSphere(new Vector3(midpoint.X + (float)Math.Cos(Math.PI * 2)  * 20, 0.0f, midpoint.Z + (float)Math.Sin(Math.PI* 2 )  * 20), 10.0f));
            //        BoundingSphereRenderer.Render(spheres[i][j], ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Azure);
            //    }
            //}
            //for(double i = 0.0; i< 1.0; i+=.2)
            //{
            //               const float radius = 30;
            //const float height = 40;
                
            //double angle = i * Math.PI * 2;

            //float x = (float)Math.Cos(angle);
            //float y = (float)Math.Sin(angle);

            //BoundingSphereRenderer.Render(new BoundingSphere(new Vector3(midPoint.X + x * radius, 10, midPoint.Z + y * radius), 10), ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);


            //}
        public int State;

        public horizontalCircle(Matrix world, int state, double cdoub)
    {
        currentDouble = cdoub;
                    World = world;
            Position = world.Translation;
            
            State = state;
            Direction = world.Forward;

    }


        public horizontalCircle(Matrix world, int state, double cdoub, float Ht)
        {
            height = Ht;
            currentDouble = cdoub;
            World = world;
            Position = world.Translation;

            State = state;
            Direction = world.Forward;

        }
        public horizontalCircle(Matrix world, int state)
        {
            World = world;
            Position = world.Translation;
            
            State = state;
            Direction = world.Forward;
            if (state == 0)
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
        public void updateROF(GameTime gameTime, Vector3 Position)
        {



            currentDouble += gameTime.ElapsedGameTime.TotalSeconds;
            if (currentDouble > 2.0)
                currentDouble -= 2.0;

            double angle = currentDouble * Math.PI * 2.0;
            const float radius = 30;
            //const float height = 40;
            float x = (float)Math.Cos(angle);
            float y = (float)Math.Sin(angle);

            BS = new BoundingSphere(new Vector3(Position.X + x * radius, height, Position.Z + y * radius), 10);

           // Position += Direction * 5.0f;


        }
        public void update(GameTime gameTime)
        {

          
            currentDouble += gameTime.ElapsedGameTime.TotalSeconds;
            if (currentDouble > 2.0)
                currentDouble -= 2.0;

            double angle = currentDouble * Math.PI *2.0 ;
            const float radius = 30;
            const float height = 40;
            float x = (float)Math.Cos(angle);
            float y = (float)Math.Sin(angle);

            BS = new BoundingSphere(new Vector3(Position.X + x * radius, 10, Position.Z + y * radius), 10);

            Position += Direction * 5.0f;

        }




    }
}
