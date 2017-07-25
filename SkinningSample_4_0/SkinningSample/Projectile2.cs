
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;


namespace SmellOfRevenge2011
{
    public class Projectile2
    {
        
        public float Scale = 0.0f;
        public Vector3 Direction = Vector3.Zero;
        public Vector3 Translation = Vector3.Zero;

        public int Rise1 = 0;
        public int Rise2 = 0;
        public int Rise3 = 0;
        public int Rise4 = 0; 
        public Vector3 Dir2 = Vector3.Zero;
        public Vector3 Dir3 = Vector3.Zero;
        public Vector3 Dir4 = Vector3.Zero;
        public Vector3 Trans2 = Vector3.Zero;
        public Vector3 Trans3 = Vector3.Zero;
        public Vector3 Trans4 = Vector3.Zero;
        public Matrix world2 = Matrix.Identity;
        public Matrix world3 = Matrix.Identity;
        public Matrix world4 = Matrix.Identity;
        public string Name = "";
        public TimeSpan currentTime;
        public bool alive = true;
        public Vector3 TravelDirection = Vector3.Zero;

        public Matrix world = Matrix.Identity;

        public BoundingSphere BS = new BoundingSphere();
        public BoundingSphere BS2 = new BoundingSphere();
        public BoundingSphere BS3 = new BoundingSphere();
        public BoundingSphere BS4 = new BoundingSphere();
        public Projectile2(string name, Matrix w1, Vector3 dir1, Vector3 dir2, Vector3 dir3, Vector3 dir4,
                           int ris1, int ris2, int ris3, int ris4)
        {
            Rise1 = ris1;
            Rise2 = ris2;
            Rise3 = ris3;
            Rise4 = ris4;
            world = w1;
            Name = name;
            Direction = world.Forward;
            Translation = world.Translation;
            Trans2 = Translation;
            Trans3 = Translation;
            Trans4 = Translation;
            TravelDirection = dir1;
            Dir2 = dir2;
            Dir3 = dir3;
            Dir4 = dir4;
            
        }
        public Projectile2(string name, Matrix World, Vector3 direction, int fortower)
        {
            world = World;
            Name = name;
            
            Translation = world.Translation;
            world.Forward = direction;
            world.Up = Vector3.Up;
            world.Right = Vector3.Cross(direction, Vector3.Up);
            Direction = direction;
            TravelDirection = direction;

        }
        public Projectile2(string name, Matrix World, Vector3 direction)
        {

          
            world = World;
            Name = name;
            Direction = world.Forward;
            Translation = world.Translation;

            TravelDirection = direction;

        }
        public Projectile2(float scale, Vector3 direction, Vector3 translation, string name)
        {
            Scale = scale;
            Direction = direction;
            Translation = translation;
            Name = name;
            currentTime = TimeSpan.Zero;
            world = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(direction.X, direction.Y, direction.Z) * Matrix.CreateTranslation(translation);
        }
        public void updateEruption(GameTime gameTime)
        {
            gameTime = new GameTime(gameTime.TotalGameTime, new TimeSpan(gameTime.ElapsedGameTime.Ticks/ (long)2.0));
            currentTime += gameTime.ElapsedGameTime;

            Translation = Vector3.Add(Translation, new Vector3(-TravelDirection.X, 0, -TravelDirection.Z) * (float)gameTime.ElapsedGameTime.TotalSeconds*400.9f);
            Trans2 = Vector3.Add(Trans2, new Vector3(-Dir2.X, 0, -Dir2.Z) * (float)gameTime.ElapsedGameTime.TotalSeconds*300.0f);
            Trans3 = Vector3.Add(Trans3, new Vector3(-Dir3.X, 0, -Dir3.Z) * (float)gameTime.ElapsedGameTime.TotalSeconds *700.0f);
            Trans4 = Vector3.Add(Trans4, new Vector3(-Dir4.X, 0, -Dir4.Z) * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (Rise1 > 0)
            {
                Translation.Y += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                Rise1 -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else
                Translation.Y -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Rise2 > 0)
            {
                Trans2.Y += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                Rise2 -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else
                Trans2.Y -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Rise3 > 0)
            {
                Trans3.Y += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                Rise3 -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else
                Trans3.Y -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Rise4 > 0)
            {
                Trans4.Y += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                Rise4 -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else
                Trans4.Y -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Translation.Y<0 && Trans2.Y <0&& Trans3.Y<0 && Trans4.Y < 0)
                alive = false;

            BS = new BoundingSphere(Translation, 10.0f);
            BS2 = new BoundingSphere(Trans2, 10.0f);
            BS3 = new BoundingSphere(Trans3, 10.0f);
            BS4 = new BoundingSphere(Trans4, 10.0f);

            world.Translation = Translation;


        }
        public void updateRockUp(float interp)
        {


            Translation = Vector3.Lerp(new Vector3(Translation.X, 0.0f, Translation.Z), Translation, interp);
            world.Translation = Translation;
            if (interp > 1.0)
                alive = false;
         


        }
        public void update2T(GameTime gameTime)
        {
            currentTime += gameTime.ElapsedGameTime;

            Translation = Vector3.Add(Translation, new Vector3(Direction.X, Direction.Y, Direction.Z) * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 5);

            BS.Center = Translation;
            BS.Radius = 20.0f;
            if (currentTime.TotalSeconds > 10.0f)
                alive = false;

            //Direction.Normalize();

            // world = Matrix.Identity;
             world.Forward = new Vector3(-Direction.X, Direction.Y, -Direction.Z);
            world.Translation = Translation;
            
           // world.Forward = TravelDirection;
            // world.Up = Vector3.Up;
            //// Vector3 Right = Vector3.Cross(Direction, Vector3.Up);

            // world.Right = Vector3.Cross(Direction, Vector3.Up);


        }
        public void update2E(GameTime gameTime)
        {
            currentTime += gameTime.ElapsedGameTime;

            Translation = Vector3.Add(Translation, new Vector3(TravelDirection.X, TravelDirection.Y, TravelDirection.Z) * (float)gameTime.ElapsedGameTime.TotalMilliseconds);


                if (currentTime.TotalSeconds > 10.0f)
                alive = false;

            //Direction.Normalize();
            
           // world = Matrix.Identity;
           // world.Forward = new Vector3(Direction.X, Direction.Y, Direction.Z);
            world.Translation = Translation;
           // world.Up = Vector3.Up;
           //// Vector3 Right = Vector3.Cross(Direction, Vector3.Up);
            
           // world.Right = Vector3.Cross(Direction, Vector3.Up);


        }
        public void update2ENonMoving(GameTime gameTime)
        {


            world.Translation = Translation;




        }
        public void update2RTS(GameTime gameTime)
        {
            currentTime += gameTime.ElapsedGameTime;

            Translation = Vector3.Add(Translation, new Vector3(TravelDirection.X, TravelDirection.Y, TravelDirection.Z) * (float)gameTime.ElapsedGameTime.TotalMilliseconds);


            if (currentTime.TotalSeconds > 10.0f)
                alive = false;

            world.Translation = Translation;

        }
        public void update2(GameTime gameTime)
        {
            currentTime += gameTime.ElapsedGameTime;

            Translation = Vector3.Add(Translation, new Vector3(-TravelDirection.X, TravelDirection.Y, -TravelDirection.Z) * (float)gameTime.ElapsedGameTime.TotalMilliseconds );


            if (currentTime.TotalSeconds > 10.0f)
                alive = false;

            world.Translation = Translation;

        }
        public void update(GameTime gameTime)
        {

            currentTime += gameTime.ElapsedGameTime;

            Translation = Vector3.Add(Translation, Direction * (float)currentTime.TotalSeconds * 2.0f);


            if (currentTime.TotalSeconds > 3.0f)
                alive = false;

            world = Matrix.CreateScale(Scale);
            world.Forward = Direction;
            world.Translation = Translation;
            world.Up = Vector3.Up;
            world.Right = Vector3.Cross(Direction, Vector3.Up);

            




        }


    }
}
