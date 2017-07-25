using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;


namespace SmellOfRevenge2011
{
    public class Lightning
    {
        public Vector3 cardinalDirection;
        public Random rand;
        public int offsetAmount;
        public int maximumOffset;
        public List<Segment> segmentList;
        public List<Segment> segmentList2;
        public Vector3 midPoint;
        public class Segment
        { 
            public Vector3 startPoint;
            public Vector3 endPoint;

            public Segment(Vector3 sp, Vector3 ep)
            {
                startPoint = sp;
                endPoint = ep;
            }
        }
        public Lightning()
        {


        }
        public Lightning(Vector3 startPoint, Vector3 endPoint)
        {
            segmentList = new List<Segment>();
            segmentList2 = new List<Segment>();
            segmentList.Add(new Segment(startPoint, endPoint));
            rand = new Random();
            maximumOffset = 200;
            offsetAmount = maximumOffset;
            Vector3 NormalizedVector = Vector3.Zero;
            for (int i = 0; i < 5; i++)
            {
                foreach (Segment seg in segmentList)
                {
                    midPoint = new Vector3((startPoint.X + endPoint.X) / 2.0f, (startPoint.Y + endPoint.Y) / 2.0f, (startPoint.Z + endPoint.Z) / 2.0f);
                    //NormalizedVector = Vector3.Dot(endPoint - startPoint);
                    NormalizedVector.Normalize();
                    midPoint += NormalizedVector * rand.Next(-offsetAmount, offsetAmount);

                
                    segmentList2.Add(new Segment(seg.startPoint, midPoint));
                    segmentList2.Add(new Segment(midPoint, seg.endPoint));
                    //segmentList.Remove(seg);
                    
                }
                segmentList.Clear();
                foreach(Segment seg in segmentList2)
                segmentList.Add(seg);
                segmentList2.Clear();
                offsetAmount /= 2;
                
            }

        }
        
        
    }
}
