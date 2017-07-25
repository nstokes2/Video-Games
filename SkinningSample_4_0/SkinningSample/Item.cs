using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SmellOfRevenge2011
{
    public class Item
    {
        public BoundingSphere BS;
        public int Type;
        public int Count;
        public bool pickedUp = false;
        
        public void Update(GameTime gameTime)
        {
            if (Vector3.Distance(BS.Center, ScreenManager.Theseus.Position) < 15)
            {
                if (Type == 0)
                    ScreenManager.Theseus.antennas += Count;
                if (Type == 1)
                    ScreenManager.Theseus.cpus += Count;
                if (Type == 2)
                    ScreenManager.Theseus.powerCharges += Count;
                if (Type == 3)
                    ScreenManager.Theseus.spareParts += Count;
                pickedUp = true;
            }


        }
        public Item(Vector3 position, int type, int count)
        {
            BS = new BoundingSphere(position, 10.0f);
            Type = type;
            Count = count;
        }
    }
}
