using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;


namespace SmellOfRevenge2011
{
    public class EnemyBattleGroup
    {
        
        /// <summary>
        /// 0 is two swords, 1 is sword and spear
        /// </summary>
        /// 


        public Vector3 oldPosition;

   
   

        public Matrix formation = new Matrix();

        public Vector3[] CFormation = new Vector3[9];
        public Vector3[] NFormation = new Vector3[9];
        public Vector3[] SFormation = new Vector3[9];
        public Vector3[] EFormation = new Vector3[9];
        public Vector3[] WFormation = new Vector3[9];
        private Matrix world;
        public Matrix World
        {
            get
            {
                return world;
            }
            set
            {
                world = value;
            }
        }
        private Vector3 up;
        public Vector3 Up
        {
            get
            {
                return up;
            }
            set
            {
                up = value;
            }
        }
        private Vector3 right;
        public Vector3 Right
        {
            get
            {
                return right;
            }
            set
            {
                right = value;
            }
        }
        private Vector3 position;
        public Vector3 Position
        {

            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        private Vector3 direction;
        public Vector3 Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
            }
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


        public int type;
        List<JuneXnaModel> models;
         JuneXnaModel spearer;
         JuneXnaModel sword;

        public EnemyBattleGroup(int type, Vector3 translation, Vector3 direction)
        {
            Position = translation;
            Direction = direction; 
            if (type == 0)
            {
                models = new List<JuneXnaModel>();

                //spearer = new JuneXnaModel(new Vector3(200.0f, 0.0f, 200.0f), Vector3.Backward);
                //sword = new JuneXnaModel(new Vector3(260.0f, 0.0f, 200.0f), Vector3.Backward);

                //models.Add(spearer);
                //models.Add(sword);

            }
        }

        public void updateSpearAndSword()
        {

            oldPosition = Position;
            float dToT = Vector3.Distance(position, ScreenManager.Michael.World.Translation);
            float rotationAmount = 0.0f;
            rotationAmount = TurnToFace(position, ScreenManager.Michael.World.Translation, new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));



            Position += new Vector3(Direction.X, Direction.Y, Direction.Z) *  5.0f;

            world.Forward = new Vector3(-Direction.X, Direction.Y, -Direction.Z);

            // world.Forward = new Vector3(Direction.X, Direction.Y, Direction.Z);
            world.Up = Vector3.Up;
            world.Right = Vector3.Cross(world.Forward, world.Up);
            world.Translation = Position;


        }




    }
}
