using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;


namespace SmellOfRevenge2011
{
    public class HSphere
    {
        public float health = 100;
        public BoundingSphere BS;
        public bool alive = true;
   
        public HSphere(float hea, Vector3 trans, float radi)
        {
            health = hea;
            BS = new BoundingSphere(trans, radi);

        }

        public void update()
        {
            if (health < 0)
                alive = false;
           // Console.WriteLine(health);

        }
    }
}
