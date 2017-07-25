using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;
using Microsoft.Xna.Framework.Graphics;


namespace SmellOfRevenge2011
{
    public class Arrow
    {
        public Matrix world;
        public Vector3 Direction;
        public Vector3 Translation;
        public Vector3 TravelDirection;
        public List<BoundingSphere> BSes;
        public List<BoundingSphere> oldBSes;
        public TimeSpan currentTime;
        public bool alive = true;
        public void updateE(GameTime gameTime)
        {

            currentTime += gameTime.ElapsedGameTime;

            Translation = Vector3.Add(Translation, new Vector3(TravelDirection.X, TravelDirection.Y, TravelDirection.Z) * (float)gameTime.ElapsedGameTime.TotalMilliseconds);


            if (currentTime.TotalSeconds > 10.0f)
                alive = false;

            world.Translation = Translation;
        }
        public void update2(GameTime gameTime, Matrix rHand)
        {
            for (int i = 0; i < 3; i++)
            {
                oldBSes[i] = new BoundingSphere(BSes[i].Center, BSes[i].Radius);
             //   oldBSes[i].Radius = BSes[i].Radius;
            }
            currentTime += gameTime.ElapsedGameTime;

            Translation = Vector3.Add(Translation, new Vector3(-TravelDirection.X, TravelDirection.Y, -TravelDirection.Z) * (float)gameTime.ElapsedGameTime.TotalMilliseconds);


            if (currentTime.TotalSeconds > 10.0f)
                alive = false;

            world.Translation = Translation;

            Matrix targetMat;
            Matrix[] transforms = new Matrix[ScreenManager.spearSphere.Bones.Count];
            ScreenManager.spearSphere.CopyAbsoluteBoneTransformsTo(transforms);
            int arrow = 0;
            Vector3 trans;
            Vector3 scale;
            Quaternion rota;
            foreach (ModelMesh mesh in ScreenManager.spearSphere.Meshes)
            {
                // draw = false;
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (mesh.Name == "arrowS1" || mesh.Name == "arrowS2" || mesh.Name == "arrowS3")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * rHand;
                        targetMat.Decompose(out scale, out rota, out trans);
                       BSes[arrow++] =  new BoundingSphere(world.Translation, mesh.BoundingSphere.Radius * Math.Abs(scale.Z));


                    }
                }
            }
        }
        public void update(GameTime gameTime)
        {
            currentTime += gameTime.ElapsedGameTime;

            Translation = Vector3.Add(Translation, new Vector3(-TravelDirection.X, TravelDirection.Y, -TravelDirection.Z) * (float)gameTime.ElapsedGameTime.TotalMilliseconds);


            if (currentTime.TotalSeconds > 10.0f)
                alive = false;

            world.Translation = Translation;


        }
                public Arrow(Matrix World, Vector3 direction, List<boundingSphere> bses)
        {
            alive = true;
                    BSes = new List<BoundingSphere>();
                    oldBSes = new List<BoundingSphere>();
                    for (int i = 0; i < 3; i++)
                    {
                        BSes.Add(bses[i].BS);
                        oldBSes.Add(bses[i].BS);
                    }
            world = World;
            
            Direction = world.Forward;
            Translation = world.Translation;

            TravelDirection = direction;

        }

    }
}
