using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SmellOfRevenge2011
{
    public class DRay
    {
        public Ray ray;
        public float maxDistance;
        public float distance;
        public List<HSphere> HSes;
        public int index = 0; 
        
        public DRay(Vector3 position, Vector3 Direction, float dis)
        {
            ray = new Ray(position, Direction);
            maxDistance = dis;
            distance = maxDistance;
            HSes = new List<HSphere>();
        }
        public void update(GameTime gameTime)
        {
            if (HSes.Count > 0)
                HSes[index].health -= (float) gameTime.ElapsedGameTime.TotalSeconds * 40;
            



        }
        public void findDistance()
        {
            List<float> distances;
           // int index = 0;
            float lowest = 0;
            int i = 0;
            float current = 0;
            distances = new List<float>();

            foreach (HSphere hs in HSes)
            {
            //    distances.Add(Vector3.Distance(ray.Position, hs.BS.Center));
                if (i == 0)
                    lowest = Vector3.Distance(ray.Position, hs.BS.Center);
                else
                {
                    current = Vector3.Distance(ray.Position, hs.BS.Center);
                    if(current < lowest)
                        index = i;
                }
                i++;
            }

            distance = lowest;
            if (HSes.Count == 0)
                distance = maxDistance;


        }
    }
}
