using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;

namespace SmellOfRevenge2011
{
   public class boundingSphere
        {
       
            protected Vector3 center;
            public Vector3 Center
            {
                get
                {
                    return this.center;
                }
                set
                {
                    this.center = value;
                }
            }
            protected BoundingSphere bs;
            public BoundingSphere BS
            {
                get
                {
                    return this.bs;
                }
                set
                {
                    this.bs = value;
                }
            }
            protected string name;
            public string Name
            {
                get
                {
                    return this.name;
                }
                set
                {
                    this.name = value;
                }
            }
            public boundingSphere(string n, BoundingSphere b)
            {
                name = n;
                bs = b;
            }
            public boundingSphere(boundingSphere bSphere)
            {
                name = bSphere.Name;
                bs = bSphere.BS;

            }
            public void setCenter()
            {
                bs.Center = center;
            }
        }
}
