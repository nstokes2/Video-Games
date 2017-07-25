
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;
using Microsoft.Xna.Framework.Graphics;


namespace SmellOfRevenge2011
{
    public class Tower
    {
        /// <summary>
        /// faces target 
        /// </summary>
        /// 
        public List<Rune> runes;

        public List<string> upgrades;
        public int power = 0;
        public int split = 0;
        public int level = 1;
        public double currentDouble = 0.0;
        public bool alive = true;
        public BoundingSphere BS;
        public Matrix fakeWorld; 
        public Vector3 fakeDir;
        public Matrix arrowWorld;
        public Matrix forwardSpell;
        public Vector3 unproject;
        
        public int maxTargets = 1; 
        public int health = 100; 
        public List<int> target;
        public List<Projectile2> projectiles;
        public TimeSpan shotCooldown;
        public Vector3 translation;
        public Matrix World;
        public Vector3 Position;
        public Tower(Matrix world)
        {
            unproject = Vector3.Zero;
            fakeWorld = new Matrix();
            fakeDir = new Vector3();
            arrowWorld = new Matrix();
            forwardSpell = new Matrix();
            target = new List<int>();
            projectiles = new List<Projectile2>();
            shotCooldown = TimeSpan.Zero;
            translation = world.Translation;
            World = world;
            Position = translation - new Vector3(0.0f, 100.0f, 0.0f);

        }

        public Tower(Vector3 Translation)
        {
            //runes = new List<string>();
            //runes.Add("Empty");
            //runes.Add("Empty");
            //runes.Add("Empty");
            runes = new List<Rune>();
            runes.Add(new Rune("Empty", 0));
            runes.Add(new Rune("Empty", 0));
            runes.Add(new Rune("Empty", 0));

            unproject = Vector3.Zero;
            arrowWorld = new Matrix();
            forwardSpell = new Matrix();
            target = new List<int>();
            projectiles = new List<Projectile2>();
            shotCooldown = TimeSpan.Zero;
            translation = Translation;
            Position = translation;
            BS = new BoundingSphere(translation + new Vector3(0.0f, 70.0f, 0.0f), 50.0f);
            //World = Matrix.Identity



        }
        public bool isProjDead(Projectile2 proj)
        {
            return !proj.alive;


        }
        public void findNextTarget()
        {
            bool found = false;
            float lowest = 99999;
            int index = 0; 
            //int j= 0;
            float current = 0;

            for(int i= 0; i< ScreenManager.fighters.Count; i++)
            {
                if (ScreenManager.fighters[i].activated && ScreenManager.fighters[i].health > 0)
                {
                    current = Vector3.Distance(Position, ScreenManager.fighters[i].Position);
                    found = true;
                    if (current < lowest)
                    {
                        index = i;
                        lowest = current;
                    }
                }
            }
            if (found)
            {
                if (target.Count > 0)
                    target[0] = index;
                else
                    target.Add(index);
            }



        }
        public void update(GameTime gameTime)
        {

            int addition = 0;
            addition = (int)currentDouble;
            currentDouble -= addition;

            health += addition;
            foreach (Projectile2 projectile in projectiles)
            {
                projectile.update2T(gameTime);

            }
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (!projectiles[i].alive)
                    projectiles.RemoveAt(i);

            }

            if(health > 0)
                alive = true;

            else
                alive = false;
            if (alive)
            {
                if (target.Count > 0)
                    if (ScreenManager.fighters[target[0]].health <= 0)
                    {
                        findNextTarget();
                        target.RemoveAt(0);
                    }
                if (target.Count == 0)
                    findNextTarget();
                shotCooldown += gameTime.ElapsedGameTime;



                // projectiles.RemoveAll(isProjDead);

                Vector3 scale;
                Quaternion rota;
                Vector3 trans;

                Matrix[] transforms = new Matrix[ScreenManager.tower.Bones.Count];
                ScreenManager.tower.CopyAbsoluteBoneTransformsTo(transforms);
                //if (mesh.Name == "forwardSpell" && proj.Name == "Fire")
                foreach (ModelMesh mesh in ScreenManager.tower.Meshes)
                {
                    // World.Decompose(out scale, out rota, out trans);

                    if (mesh.Name == "Arrow")
                    {
                        arrowWorld = transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(translation);
                        arrowWorld.Translation = new Vector3(arrowWorld.Translation.X, 70.0f, arrowWorld.Translation.Z);

                    }




                }



                float rotationAmount = 0.0f;
                float rot2 = 0.0f;
                Vector3 Direction = Vector3.Forward;
                if (shotCooldown.TotalSeconds > 2 && target.Count > 0)
                {
                    arrowWorld.Decompose(out scale, out rota, out trans);
                    rotationAmount = TurnToFace(arrowWorld.Translation, ScreenManager.fighters[target[0]].Position, Vector3.Forward);
                    Direction = new Vector3((float)Math.Sin(rotationAmount), 0.0f, (float)Math.Cos(rotationAmount));
                    ///rot2 = TurnToFaceY(translation, ScreenManager.fighters[target[0]].Position, Direction);

                    projectiles.Add(new Projectile2(scale.Y, Direction, trans, "Arrow"));
                    // projectiles.Add(new Projectile2("Arrow", arrowWorld, Direction, 1));


                    shotCooldown = TimeSpan.Zero;
                }


            }

        }
        private static float TurnToFaceY(Vector3 position, Vector3 target, Vector3 rotation)
        {

            // float x = (target.X - position.X);
            float y = (target.Y - position.Y);
            float z = (target.Z - position.Z);

            float desiredAngle = (float)Math.Atan2(y, z);

            Vector3 tempDir = rotation;
            float difference = WrapAngle(desiredAngle - tempDir.X);

            // difference = MathHelper.Clamp(difference, -5.0f, 5.0f);
            return WrapAngle(tempDir.X + difference);


        }
        public float TurnToFace(Vector3 position, Vector3 target, Vector3 rotation)
        {
            float x = (target.X - position.X);
            float z = (target.Z - position.Z);

            float desiredAngle = (float)Math.Atan2(x, z);

            Vector3 tempDir = rotation;
            float difference = WrapAngle(desiredAngle - tempDir.Y);

            //  difference = MathHelper.Clamp(difference, -5.0f, 5.0f);

            return WrapAngle(tempDir.Y + difference);
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
    }
}
