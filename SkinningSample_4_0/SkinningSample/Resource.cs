using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
namespace SmellOfRevenge2011
{
    public class Resource
    {
        public double currentDouble = 0.0;
        public int Max;
        public int Regen;
        public BoundingSphere BS;
        public int Current;
        /// <summary>
        /// 0 earth 1 fire 2 wind 3 water
        /// </summary>
        public int Type;
        public Vector3 Translation;
        public Vector3 unproject;
        public void update(GameTime gameTime)
        {
            int addition = 0;
            currentDouble += gameTime.ElapsedGameTime.TotalMilliseconds / 30.0f;

            addition = (int)currentDouble;
            currentDouble -= addition;
            Current += addition;
            if (Current > Max)
                Current = Max;

        }
        public Resource(int max, int regen, int current, int type, Vector3 trans)
        {
          
            unproject = Vector3.Zero;
            Max = max;
            Regen = regen;
            Current = current;
            Type = type;
            BS = new BoundingSphere(trans, 50);
            Translation = trans;

        }

    }
}
