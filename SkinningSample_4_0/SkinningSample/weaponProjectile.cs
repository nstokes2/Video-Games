
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;
namespace AgeWithoutTime
{
    public class weaponProjectile
    {
        public Matrix World;

        public Vector3 Direction;

        public Vector3 RotateAxeese;

        public Vector3 BaseRotate = Vector3.Zero;

        protected int timer;
        public int Timer{
            get{
                return  this.timer;
            }
            set{
                this.timer = value;
            }
        }
        protected BoundingSphere bs;
        public BoundingSphere Bs
        {
            get
            {
                return bs;
            }
            set
            {
                bs = value;
            }
        }

        public double axisAngle = 0.0;

        public void Update(GameTime gameTime, bool player)
        {
            Vector3 transform;
            // if(rotation >= 0)
            //   transform = new Vector3(0, 0, 1);
            // else
            // transform = new Vector3(0, 0, -1);
            // translation = Vector3.Add(translation, Vector3.Transform(new Vector3(0, 0, -1 * speed), Matrix.CreateFromAxisAngle(Vector3.Up, (float)rotation)));

            // Console.WriteLine(rotation);

            BaseRotate = Vector3.Add(BaseRotate, RotateAxeese * speed);

            float thisY = translation.Y;
            //OLDUPDATE
            /*
           if(name != "Meteor")
            translation = translation + Direction * -speed;
            timer += gameTime.ElapsedGameTime.Milliseconds;

            //translation = new Vector3(translation.X, thisY, translation.Z);
            */
            if (name == "Meteor")
            {
                translation = new Vector3(translation.X + Direction.X * -2.0f, translation.Y - 10.0f, translation.Z + Direction.Z * -2.0f);
                // if (translation.Y > GamePlayScreen.heightData[(int)-translation.X / 10, (int)-translation.Z / 10])
                //  translation.Y -= 10.0f;

                if (translation.Y < -153)
                    timer = 2001;

                // Direction = translation;
                //  if (translation.Y < GamePlayScreen.heightData[(int)-translation.X / 10, (int)-translation.Z / 10])
                //   translation.Y = GamePlayScreen.heightData[(int)-translation.X / 10, (int)-translation.Z / 10];
            }
            if (timer > 2000)
            {
                alive = false;
                timer = 0;
            }
            if (name == "Ice Bolt" || name == "Fire Ball 1")
            {

                if(player)
                translation = Vector3.Add(translation, Vector3.Transform(new Vector3(0, 0, -1 * speed), Matrix.CreateFromAxisAngle(Vector3.Up, (float)rotation)));
                else
                translation = Vector3.Add(translation, Vector3.Transform(new Vector3(0, 0, 1 * speed), Matrix.CreateFromAxisAngle(Vector3.Up, (float)rotation)));



            }
            if (name == "Arrow")
            {

                translation = Vector3.Add(translation, Vector3.Transform(new Vector3(0, 0, -1 * speed), Matrix.CreateFromAxisAngle(Vector3.Up, (float)rotation)));


                //translation.Y = 0.0f;


            }
         //   if (name != "Meteor")
          //      bs = new BoundingSphere(Vector3.Add(bs.Center, Direction * -speed), bs.Radius);
         //   if (name == "Meteor")
          //      bs = new BoundingSphere(Vector3.Add(bs.Center, translation), bs.Radius);



            //foreach (boundingSphere aBS in bs)
            //  {
            //    aBS.BS = new BoundingSphere(Vector3.Add(aBS.BS.Center, Direction * -speed), aBS.BS.Radius);




            //}

        }

        public void Update(GameTime gameTime)
        {
            Vector3 transform;
           // if(rotation >= 0)
             //   transform = new Vector3(0, 0, 1);
           // else
               // transform = new Vector3(0, 0, -1);
           // translation = Vector3.Add(translation, Vector3.Transform(new Vector3(0, 0, -1 * speed), Matrix.CreateFromAxisAngle(Vector3.Up, (float)rotation)));
            
           // Console.WriteLine(rotation);

            BaseRotate = Vector3.Add(BaseRotate, RotateAxeese * speed);
            
            float thisY = translation.Y;
            //OLDUPDATE
            /*
           if(name != "Meteor")
            translation = translation + Direction * -speed;
            timer += gameTime.ElapsedGameTime.Milliseconds;

            //translation = new Vector3(translation.X, thisY, translation.Z);
            */
            if (name == "Meteor")
            {
                translation = new Vector3(translation.X + Direction.X * -2.0f, translation.Y - 10.0f, translation.Z + Direction.Z * -2.0f);
               // if (translation.Y > GamePlayScreen.heightData[(int)-translation.X / 10, (int)-translation.Z / 10])
                  //  translation.Y -= 10.0f;

                    if (translation.Y < -153)
                         timer = 2001;

                   // Direction = translation;
              //  if (translation.Y < GamePlayScreen.heightData[(int)-translation.X / 10, (int)-translation.Z / 10])
                 //   translation.Y = GamePlayScreen.heightData[(int)-translation.X / 10, (int)-translation.Z / 10];
            }
            if (timer > 2000)
            {
                alive = false;
                timer = 0;
            }
            if (name == "Ice Bolt" || name == "Fire Ball 1")
            {


                translation = Vector3.Add(translation, Vector3.Transform(new Vector3(0, 0, 1 * speed), Matrix.CreateFromAxisAngle(Vector3.Up, (float)rotation)));


            }
            if (name == "Arrow")
            {

                translation = Vector3.Add(translation, Vector3.Transform(new Vector3(0, 0, -1 * speed), Matrix.CreateFromAxisAngle(Vector3.Up, (float)rotation)));


                //translation.Y = 0.0f;


            }
           // if(name != "Meteor")
          //      bs = new BoundingSphere(Vector3.Add(bs.Center, Direction * -speed), bs.Radius);
          //  if(name == "Meteor")
           //     bs = new BoundingSphere(Vector3.Add(bs.Center, translation), bs.Radius);



            //foreach (boundingSphere aBS in bs)
          //  {
            //    aBS.BS = new BoundingSphere(Vector3.Add(aBS.BS.Center, Direction * -speed), aBS.BS.Radius);




            //}

        }
 /*       public void UpdatePlayer(GameTime gameTime)
        {


            Vector3 transform;
            if (axisAngle < 0)
                transform = new Vector3(0, 0, 1);
            else
                transform = new Vector3(0, 0, -1);
           // translation = Vector3.Add(translation, Vector3.Transform(new Vector3(0, 0, 1 * speed), Matrix.CreateFromAxisAngle(Vector3.Up, (float)axisAngle)));


            translation = translation + Direction * speed;
            // Console.WriteLine(rotation);

            timer += gameTime.ElapsedGameTime.Milliseconds;

            if (timer > 2000)
            {
                alive = false;
                timer = 0;
            }


        }*/
        protected bool alive;
        public bool Alive
        {
            get
            {
                return alive;
            }
            set
            {
                alive = value;
            }
        }
        protected int speed;
        public int Speed
        {
            get
            {
                return this.speed;
            }
            set
            {
                this.speed = value;
            }
        }
        protected Vector3 target;
        public Vector3 Target
        {
            get
            {
                return this.target;
            }
            set
            {
                this.target = value;
            }
        }
        protected Vector3 translation;
        public Vector3 Translation
        {
            get
            {
                return this.translation;
            }
            set
            {
                this.translation = value;
            }
        }
        protected Quaternion rota;
        public Quaternion Rota{
            get{
                return rota;
            }
        
            set{
            rota = value;
        }
        }
            protected double rotation;
            public double Rotation{
                get{
                    return this.rotation;
                }
                set{
                    this.Rotation = value;
                }
            }
        
        /// <summary>
        /// Name of the projectile (arrow, spell, spear, thrown)
        /// </summary>
            protected string name; //Name of the projectile arrow, spell, spear, thrown
            public string Name
            {
                get { return this.name; }
                set
                {
                    this.name = value;
                }
            }

            public weaponProjectile(string name, Vector3 Direction, Matrix world)
            {
                //  if(name != "Arrow" && name!= "Spear" && name!= "ThrowingAxe")
                World = world;
               // world.Decompose(

                bs = new BoundingSphere();
                alive = true;
                this.name = name;
                this.Direction = Direction;

                rotation = Math.Atan2(Direction.X, Direction.Z);
                translation = world.Translation;
                //this.translation = trans;
                //this.target = targ;
                this.speed = 5;

                RotateAxeese = Vector3.Zero;

                if (name == "Meteor")
                    RotateAxeese = new Vector3(0.0f, 0.0f, 1.0f);
                if (name == "Fire Ball 1")
                    RotateAxeese = new Vector3(1.0f, 1.0f, 3.3f);
                if (name == "Lightning Bolt 1")
                    RotateAxeese = new Vector3(2.0f, 1.0f, 1.0f);
                if (name == "Tornado 1")
                    RotateAxeese = new Vector3(5.0f, 1.5f, 3.2f);
                timer = 0;



            }
        public weaponProjectile(string name, Vector3 Direction, Vector3 trans)
        {
          //  if(name != "Arrow" && name!= "Spear" && name!= "ThrowingAxe")
            //World = world;

            RotateAxeese = Vector3.Zero;

            if (name == "Meteor")
                RotateAxeese = new Vector3(0.0f, 0.0f, 1.0f);
            if (name == "Fire Ball 1")
                RotateAxeese = new Vector3(1.0f, 1.0f, 3.3f);
            if (name == "Lightning Bolt 1")
                RotateAxeese = new Vector3(2.0f, 1.0f, 1.0f);
            if (name == "Tornado 1")
                RotateAxeese = new Vector3(5.0f, 1.5f, 3.2f);
            bs = new BoundingSphere();
            alive = true;
            this.name = name;
            this.Direction = Direction;
            this.translation = trans;
            //this.target = targ;
            this.speed = 5;
            rotation = Math.Atan2(Direction.X, Direction.Z);
            timer = 0;



        }
        /*
            public weaponProjectile(string name, double axisAngle, Vector3 trans, Vector3 targ)
            {
                bs = new List<boundingSphere>();
                alive = true;
                this.type = name;
                this.rotation = axisAngle;
                this.translation = trans;
                this.target = targ;
                this.speed = 5;
                timer = 0;



            }*/
    }

}
