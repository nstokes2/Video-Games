using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
namespace SmellOfRevenge2011
{
    public class Construct
    {
        public int eMana;
        public int fMana;
        public int wiMana;
        public int waMana;
        public double currentE = 0.0;
        public double currentF = 0.0;
        public double currentWi = 0.0;
        public double currentWa = 0.0;

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
        public void feed(GameTime gameTime, bool earth, bool fire, bool wind, bool water)
        {
            int addition = 0;
            currentDouble += gameTime.ElapsedGameTime.TotalMilliseconds / 30.0f;
            if (earth)
            {
                currentE += currentDouble;
                addition = (int)currentE;
                currentDouble -= addition;
                eMana += addition;
            }
            if (fire)
            {
                currentF += currentDouble;
                addition = (int)currentF;
                currentDouble -= addition;
                fMana += addition;
            }
            if (wind)
            {
                currentWi += currentDouble;
                addition = (int)currentWi;
                currentDouble -= addition;
                wiMana += addition;
            }
            if (water)
            {
                currentWa += currentDouble;
                addition = (int)currentWa;
                currentDouble -= addition;
                waMana += addition;
            }

        }
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
        public Construct(int FMana, int EMana, int WiMana, int WaMana, Vector3 trans)
        {
            fMana = FMana;
            eMana = EMana;
            wiMana = WiMana;
            waMana = WaMana;
            

            unproject = Vector3.Zero;
            BS = new BoundingSphere(trans, 50);
            Translation = trans;

        }

    }
}
