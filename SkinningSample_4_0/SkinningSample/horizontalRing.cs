using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SmellOfRevenge2011
{
    public class horizontalRing
    {

        public List<horizontalCircle> circles;
        public int count;
        public Matrix world ;
        public float height;

        public horizontalRing(int Count, Matrix World, float ht )
        {
            height = ht;
            world = World;
            circles = new List<horizontalCircle>();
            count = Count;
            double delta = 1.0 / (double)count;
            for (int i = 0; i < Count; i++)
            {
                circles.Add(new horizontalCircle(world, 0, i * delta, height));
            }

        }
        public horizontalRing(int Count, Matrix World)
        {
            world = World;
            circles = new List<horizontalCircle>();
            count = Count;
            double delta = 1.0 /(double) count;
            for (int i = 0; i < Count; i++)
            {
                circles.Add(new horizontalCircle(world, 0, i * delta));
            }
            
        }
        public void updateROF(GameTime gameTime, Vector3 Position)
        {
            foreach (horizontalCircle circle in circles)
            {

                circle.updateROF(gameTime, Position);

            }

        }
        public void update(GameTime gameTime)
        {

            foreach (horizontalCircle circle in circles)
            {

                circle.update(gameTime);

            }


        }




    }
}
