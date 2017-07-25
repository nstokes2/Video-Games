using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;


namespace SmellOfRevenge2011
{
    public class SpearManAI
    {
        /// <summary>
        /// 0 is leader 1 is follower
        /// </summary>
        public int type = 1;


        public bool bump = false;

        public int health = 10; 

        public Vector3 enemyVec = Vector3.Zero;
        public Vector3 guardVec = Vector3.Zero;

        public Vector3 formVec = Vector3.Zero;
        public Vector3 battleVec = Vector3.Zero;
        /// <summary>
        /// 0 in formation, 1 charge, 2 attack, 3 protect, 4 brawl, 5 intercept
        /// </summary>
        public int aiState; 
        bool needPath = false;
        bool waiting = false;

        public Matrix forwardSpell;
        public List<Projectile2> projectiles;
        public List<boundingSphere> spheres;
        public List<boundingSphere> spearSpheres;
        public Vector3[] spots = new Vector3[10];

        public Vector3[] CFormation = new Vector3[9];
        public Vector3[] NFormation = new Vector3[9];
        public Vector3[] SFormation = new Vector3[9];
        public Vector3[] EFormation = new Vector3[9];
        public Vector3[] WFormation = new Vector3[9];


        public Queue<Vector3> Waypoints;
        public AIPathFinder pathFinder;
        //need to make a determination for when enemey leader calls to formation
        public Matrix formation = new Matrix();
        public bool resetFormation = false;
        
        public bool inFormation = false;



        /// <summary>
        /// 0 tight formation 1 attack
        /// </summary>
        public int state = 0; 
        public Vector3 Target;
        protected bool moving;
        public bool Moving
        {
            get { return moving; }
            set { moving = value; }
        }  

        /// <summary>
        /// Linear distance to the Tanks' current destination
        /// </summary>
        public float DistanceToDestination
        {
            get { return Vector3.Distance(Position, destination); }
        }

        public Vector3 target;
        private Vector3 destination;
        public Vector3 Destination
        {
            get { return destination; }
        }


        /// True when the tank is "close enough" to it's destination
        /// </summary>
        public bool AtDestination
        {
            get { return DistanceToDestination < 10.0f; }
        }
        public Vector2 Position2
        {
            get
            {
                return position2;
            }
            set
            {
                position2 = value;
            }
        }
        protected Vector2 position2;

        float orientation;
        const float chaseDistance = 60.0f;
        const float caughtDistance = 50.0f;
        const float hysteresis = 15.0f;

        

        public TimeSpan currentAnimationTime = TimeSpan.Zero;
        TimeSpan runTime = TimeSpan.Zero;
        public float rotAmt = 0.0f;
        public float thrustAmount = 0.0f;
        public Matrix[] standing, brace, atk1a, atk1b, atk1c, atk1d, atk2a, atk2b, atk3a, atk3b, atk3c, lRun1, lRun2, lRun3, lRun4, rRun1, rRun2, rRun3, rRun4, previousAnimation;

        List<Move> availableMoves;
        Move playerMove;
        TimeSpan playerMoveTime;


        public bool isAtk1, isStanding, isAtk2, isAtk3;
        public bool isRapidStrikes, isDoubleStrike;
        public bool isRun;


        readonly TimeSpan MoveTimeOut = TimeSpan.FromSeconds(1.0);
        protected SkinningData skinningData;
        public SkinningData SkinningData
        {
            get
            {
                return this.skinningData;
            }
            set
            {
                this.skinningData = value;
            }


        }
        public InputManager inputManager;

        public AnimationPlayer masterPlayer;
        public AnimationClip masterClip;

        protected Matrix[] justBones;
        public Matrix[] JustBones
        {
            get
            {
                return justBones;
            }
            set
            {
                justBones = value;
            }
        }
        protected Matrix[] legBones;
        public Matrix[] LegBones;
        protected Matrix[] upperBones;
        public Matrix[] UpperBones;
        protected Matrix[] worldTrans;
        public Matrix[] WorldTrans
        {
            get
            {
                return worldTrans;
            }
            set
            {
                worldTrans = value;
            }
        }
        protected Matrix[] skinTrans;
        public Matrix[] SkinTrans
        {
            get
            {
                return skinTrans;
            }
            set
            {
                skinTrans = value;
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


        public Vector3 oldPosition;
        public Vector3 oldDirection;
        Vector3 scale, scale2, scale3, scale4;
        Quaternion rota, rota2, rota3, rota4;
        Vector3 trans, trans2, trans3, trans4;

        GamePadState currentGamePadState;
        GamePadState oldGamePadState;
        KeyboardState currentKeyBoardState, oldkeyBoardState;

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
        public SpearManAI(Vector3 pos, Vector3 dir)
        {
            forwardSpell = new Matrix();
            projectiles = new List<Projectile2>();
            spheres = new List<boundingSphere>();
            spearSpheres = new List<boundingSphere>();

            moving = false;
            Waypoints = new Queue<Vector3>();
            pathFinder = new AIPathFinder();
            pathFinder.Initialize();
            //pathFinder.Reset();
            position = pos;
            direction = dir;
            Up = Vector3.Up;
            right = Vector3.Right;


           // inputManager = new InputManager((PlayerIndex)0, ScreenManager.moveList.LongestMoveLength);

            isAtk1 = false;
            isAtk2 = false;
            isAtk3 = false;
            isRapidStrikes = false;
            isDoubleStrike = false;
            isStanding = false;

            isRun = false;


        }

        public void setAnimationPlayers()
        {

            masterPlayer = new AnimationPlayer(skinningData);
            masterClip = skinningData.AnimationClips["Take 001"];
            masterPlayer.StartClip(masterClip);

            brace = new Matrix[skinningData.BindPose.Count];
            atk1a = new Matrix[skinningData.BindPose.Count];
            atk1b = new Matrix[skinningData.BindPose.Count];
            standing = new Matrix[skinningData.BindPose.Count];

            atk1c = new Matrix[skinningData.BindPose.Count];
            atk1d = new Matrix[skinningData.BindPose.Count];
            atk2a = new Matrix[skinningData.BindPose.Count];
            atk2b = new Matrix[skinningData.BindPose.Count];

            atk3a = new Matrix[skinningData.BindPose.Count];
            atk3b = new Matrix[skinningData.BindPose.Count];
            atk3c = new Matrix[skinningData.BindPose.Count];

            lRun1 = new Matrix[skinningData.BindPose.Count];
            lRun2 = new Matrix[skinningData.BindPose.Count];
            lRun3 = new Matrix[skinningData.BindPose.Count];
            lRun4 = new Matrix[skinningData.BindPose.Count];
            rRun1 = new Matrix[skinningData.BindPose.Count];
            rRun2 = new Matrix[skinningData.BindPose.Count];
            rRun3 = new Matrix[skinningData.BindPose.Count];
            rRun4 = new Matrix[skinningData.BindPose.Count];


            previousAnimation = new Matrix[skinningData.BindPose.Count];
            worldTrans = new Matrix[skinningData.BindPose.Count];
            skinTrans = new Matrix[skinningData.BindPose.Count];
            justBones = new Matrix[skinningData.BindPose.Count];
            legBones = new Matrix[skinningData.BindPose.Count];
            upperBones = new Matrix[skinningData.BindPose.Count];

            masterPlayer.Update(TimeSpan.FromMilliseconds(500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(standing, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(1000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(brace, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(1500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(atk1a, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(2000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(atk1b, 0);

            masterPlayer.Update(TimeSpan.FromMilliseconds(2500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(atk1c, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(3000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(atk1d, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(3500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(atk2a, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(4000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(atk2b, 0);

            masterPlayer.Update(TimeSpan.FromMilliseconds(4500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(atk3a, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(5000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(atk3b, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(5500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(atk3c, 0);

            masterPlayer.Update(TimeSpan.FromMilliseconds(6000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(lRun1, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(6500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(lRun2, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(7000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(lRun3, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(7500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(lRun4, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(8000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(rRun1, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(8500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(rRun2, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(9000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(rRun3, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(9499), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(rRun4, 0);





        }

        public void UpdateLBattle(GameTime gameTime)
        {

            float dToT = Vector3.Distance(Position, ScreenManager.p1Spear.World.Translation);
            float rotationAmount = 0.0f;
            rotationAmount = TurnToFace(position, ScreenManager.p1Spear.World.Translation, new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));


             dToT = Vector3.Distance(Position, formVec);
             rotationAmount = TurnToFace(position, formVec, new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));

            if(dToT<300.0f)
                if (dToT > 200.0f)
                {
                    //  resetFormation = true;
                    thrustAmount = 1.0f;
                    if (!isRun)
                    {
                        isRun = true;
                        justBones.CopyTo(previousAnimation, 0);
                    }
                }
                else
                {
                    //   resetFormation = false;
                    thrustAmount = 0.0f;
                    runTime = TimeSpan.Zero;
                    isRun = false;

                    if (dToT > 80)
                    {
                        thrustAmount = 0.3f;

                    }
                    else
                    {
                        thrustAmount = 0.0f;
                        isAtk1 = true;
                    }

                }





            float x = (float)Math.Sin(rotationAmount);
            float y = (float)Math.Cos(rotationAmount);
            Direction = new Vector3(x, 0.0f, y);

            Up = Vector3.Up;
            Right = Vector3.Cross(Direction, Up);

            if (isRun)
            {
                runTime += new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long)3.0);
                if (runTime.TotalSeconds < 9.0f)
                {
                    if (runTime.TotalSeconds < 1.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            lRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun1[i].Decompose(out scale, out rota, out trans);
                            lRun2[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun2[i].Decompose(out scale, out rota, out trans);
                            lRun3[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun3[i].Decompose(out scale, out rota, out trans);
                            lRun4[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 5.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun4[i].Decompose(out scale, out rota, out trans);
                            rRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 4.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 4.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 6.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun1[i].Decompose(out scale, out rota, out trans);
                            rRun2[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 5.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 5.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 7.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun2[i].Decompose(out scale, out rota, out trans);
                            rRun3[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 6.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 6.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 8.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun3[i].Decompose(out scale, out rota, out trans);
                            rRun4[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 7.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 7.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 9.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun4[i].Decompose(out scale, out rota, out trans);
                            lRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 8.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 8.0) / 1.0f));

                        }


                    }

                }
                else
                {

                    runTime = TimeSpan.Zero;
                    lRun1.CopyTo(previousAnimation, 0);



                }

            }
            Position += new Vector3(Direction.X, Direction.Y, Direction.Z) * thrustAmount * 2.0f;
            if (isRun)
                upperBones.CopyTo(justBones, 0);
            else
                brace.CopyTo(justBones, 0);
            world = Matrix.Identity;
            world.Forward = new Vector3(-Direction.X, Direction.Y, -Direction.Z);
            world.Up = Vector3.Up;
            world.Right = Vector3.Cross(world.Forward, world.Up);
            //position.Y = ScreenManager.heightData[pX, pZ];
            world.Translation = Position;

            UpdateWorldTransforms(Matrix.Identity);
            UpdateSkinTransforms();

            //if(resetFormation)
            formation = world;

        }
        public void UpdateFBattle(GameTime gameTime)
        {
            oldPosition = Position;
            float dToT = 0.0f;
            float rotationAmount = 0.0f;
            if (aiState == 0)
            {
                dToT = Vector3.Distance(Position, formVec);
                rotationAmount = TurnToFace(position, formVec, new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));

            }
            if (aiState == 1)
            {
                dToT = Vector3.Distance(Position, battleVec);
                rotationAmount = TurnToFace(position, battleVec, new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));
            }

            if (dToT > 15.0f)
            {

                thrustAmount = 1.0f;
                if (!isRun)
                {
                    isRun = true;
                    justBones.CopyTo(previousAnimation, 0);
                }

            }
            else
            {
                //   resetFormation = false;
                thrustAmount = 0.0f;
                runTime = TimeSpan.Zero;
                isRun = false;
            }
            float x = (float)Math.Sin(rotationAmount);
            float y = (float)Math.Cos(rotationAmount);
            Direction = new Vector3(x, 0.0f, y);

            Up = Vector3.Up;
            Right = Vector3.Cross(Direction, Up);

            if (isRun)
            {
                runTime += new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long)3.0);
                if (runTime.TotalSeconds < 9.0f)
                {
                    if (runTime.TotalSeconds < 1.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            lRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun1[i].Decompose(out scale, out rota, out trans);
                            lRun2[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun2[i].Decompose(out scale, out rota, out trans);
                            lRun3[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun3[i].Decompose(out scale, out rota, out trans);
                            lRun4[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 5.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun4[i].Decompose(out scale, out rota, out trans);
                            rRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 4.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 4.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 6.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun1[i].Decompose(out scale, out rota, out trans);
                            rRun2[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 5.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 5.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 7.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun2[i].Decompose(out scale, out rota, out trans);
                            rRun3[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 6.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 6.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 8.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun3[i].Decompose(out scale, out rota, out trans);
                            rRun4[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 7.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 7.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 9.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun4[i].Decompose(out scale, out rota, out trans);
                            lRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 8.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 8.0) / 1.0f));

                        }


                    }

                }
                else
                {

                    runTime = TimeSpan.Zero;
                    lRun1.CopyTo(previousAnimation, 0);



                }

            }
            Position += new Vector3(Direction.X, Direction.Y, Direction.Z) * thrustAmount * 2.0f;

            int battleX = (int)Position.X / 70;
            int battleZ = (int)Position.Z / 70;

            if (battleX < 0)
                battleX = 0;
            if (battleZ < 0)
                battleZ = 0;
            if (ScreenManager.bigOpen[battleX][battleZ] == false)
            {
                Position = oldPosition;
                bump = true;
            }
            else
                ScreenManager.bigOpen[battleX][battleZ] = false;

            if (isRun)
                upperBones.CopyTo(justBones, 0);
            else
                brace.CopyTo(justBones, 0);
            world = Matrix.Identity;
            world.Forward = new Vector3(-Direction.X, Direction.Y, -Direction.Z);
            world.Up = Vector3.Up;
            world.Right = Vector3.Cross(world.Forward, world.Up);
            //position.Y = ScreenManager.heightData[pX, pZ];
            world.Translation = Position;

            UpdateWorldTransforms(Matrix.Identity);
            UpdateSkinTransforms();





        }
        public void UpdateLeader(GameTime gameTime)
        {

            float dToT = Vector3.Distance(position, ScreenManager.p1Spear.World.Translation);
            float rotationAmount = 0.0f;
            rotationAmount = TurnToFace(position, ScreenManager.p1Spear.World.Translation, new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));
            //Console.WriteLine(dToT);



            if (state == 1)
            {


                if (pathFinder.SearchStatus == SearchStatus.PathFound && !moving)
                {
                    foreach (Point point in pathFinder.FinalPath())
                    {
                        Waypoints.Enqueue(new Vector3(point.X * 60, 0.0f, point.Y * 60));
                    }
                    Moving = true;
                }
                pathFinder.Update(gameTime);
            }
            if (moving)
            {


                if (Waypoints.Count >= 1)
                {

                    destination = Waypoints.Peek();
                }
                if (AtDestination && Waypoints.Count >= 1)
                {
                    Waypoints.Dequeue();
                }
                if (AtDestination && Waypoints.Count == 0)
                {

                    isRun = false;
                    thrustAmount = 0.0f;
                    //moving = false;


                }

                if (!AtDestination)
                {
                    rotationAmount = TurnToFace(position, destination, new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));
                    thrustAmount = 1.0f;
                    if (!isRun)
                    {
                        isRun = true;
                        justBones.CopyTo(previousAnimation, 0);
                    }
                }









            }

            if(dToT < 300.0f)//if it can be seen
            if (dToT > 200.0f)
            {
                //  resetFormation = true;
                thrustAmount = 1.0f;
                if (!isRun)
                {
                    isRun = true;
                    justBones.CopyTo(previousAnimation, 0);
                }
            }
            else
            {
                //   resetFormation = false;
                thrustAmount = 0.0f;
                runTime = TimeSpan.Zero;
                isRun = false;

                if (dToT > 80)
                {
                    thrustAmount = 0.3f;

                }
                else
                {
                    thrustAmount = 0.0f;
                    isAtk1 = true;
                }

            }

          
            
           

            float x = (float)Math.Sin(rotationAmount);
            float y = (float)Math.Cos(rotationAmount);
            Direction = new Vector3(x, 0.0f, y);

            Up = Vector3.Up;
            Right = Vector3.Cross(Direction, Up);

            if (isRun)
            {
                runTime += new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long)3.0);
                if (runTime.TotalSeconds < 9.0f)
                {
                    if (runTime.TotalSeconds < 1.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            lRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun1[i].Decompose(out scale, out rota, out trans);
                            lRun2[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun2[i].Decompose(out scale, out rota, out trans);
                            lRun3[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun3[i].Decompose(out scale, out rota, out trans);
                            lRun4[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 5.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun4[i].Decompose(out scale, out rota, out trans);
                            rRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 4.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 4.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 6.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun1[i].Decompose(out scale, out rota, out trans);
                            rRun2[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 5.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 5.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 7.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun2[i].Decompose(out scale, out rota, out trans);
                            rRun3[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 6.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 6.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 8.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun3[i].Decompose(out scale, out rota, out trans);
                            rRun4[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 7.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 7.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 9.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun4[i].Decompose(out scale, out rota, out trans);
                            lRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 8.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 8.0) / 1.0f));

                        }


                    }

                }
                else
                {

                    runTime = TimeSpan.Zero;
                    lRun1.CopyTo(previousAnimation, 0);



                }

            }

            currentAnimationTime += new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long)8);

            if (isAtk3)
            {


                if (currentAnimationTime.TotalSeconds < 4.0f)
                {

                    if (currentAnimationTime.TotalSeconds < 1.0f)
                    {

                        for (int i = 0; i < brace.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            atk3a[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds) / 1.0f));

                        }

                    }
                    else if (currentAnimationTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk3a[i].Decompose(out scale, out rota, out trans);
                            atk3b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk3b[i].Decompose(out scale, out rota, out trans);
                            atk3c[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk3c[i].Decompose(out scale, out rota, out trans);
                            atk3b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }




                }
                else
                {
                    currentAnimationTime = TimeSpan.Zero;
                    isAtk3 = false;
                }

            }
            if (isAtk1)
            {
                if (currentAnimationTime.TotalSeconds < 4.0f)
                {

                    if (currentAnimationTime.TotalSeconds < 1.0f)
                    {

                        for (int i = 0; i < brace.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            atk1a[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds) / 1.0f));

                        }

                    }
                    else if (currentAnimationTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk1a[i].Decompose(out scale, out rota, out trans);
                            atk1b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk1b[i].Decompose(out scale, out rota, out trans);
                            atk1c[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk1c[i].Decompose(out scale, out rota, out trans);
                            atk1d[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }




                }
                else if (!isDoubleStrike)
                {

                    isAtk1 = false;
                    currentAnimationTime = TimeSpan.Zero;



                }

                else if (currentAnimationTime.TotalSeconds < 5.0f)
                {
                    for (int i = 0; i < brace.Length; i++)
                    {
                        atk1d[i].Decompose(out scale, out rota, out trans);
                        atk2a[i].Decompose(out scale2, out rota2, out trans2);
                        upperBones[i] = Matrix.CreateScale(scale2) *
        Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 4.0) / 1.0f)) *
        Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 4.0) / 1.0f));

                    }


                }
                else if (currentAnimationTime.TotalSeconds < 6.0f)
                {
                    for (int i = 0; i < brace.Length; i++)
                    {
                        atk2a[i].Decompose(out scale, out rota, out trans);
                        atk2b[i].Decompose(out scale2, out rota2, out trans2);
                        upperBones[i] = Matrix.CreateScale(scale2) *
        Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 5.0) / 1.0f)) *
        Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 5.0) / 1.0f));

                    }


                }
                else
                {
                    currentAnimationTime = TimeSpan.Zero;
                    isAtk1 = false;
                    isDoubleStrike = false;

                }


            }

            Position += new Vector3(Direction.X, Direction.Y, Direction.Z) * thrustAmount * 2.0f;
            if (isRun)
                upperBones.CopyTo(justBones, 0);
            else if (isAtk3)
                upperBones.CopyTo(justBones, 0);
            else if (isAtk1)
                upperBones.CopyTo(justBones, 0);
            else
                brace.CopyTo(justBones, 0);



            int pX = (int)position.X / 100;
            int pZ = (int)position.Z / 100;

            if (Position.X < 0)
                pX = 0;
            if (Position.Z < 0)
                pZ = 0;
            if (position.X > 12799)
                pX = 127;
            if (position.Z > 12799)
                pZ = 127;
            //Console.WriteLine(
            // world.Forward = new Vector3(Direction.X, Direction.Y, Direction.Z);
            world = Matrix.Identity;
            world.Forward = new Vector3(-Direction.X, Direction.Y, -Direction.Z);
            world.Up = Vector3.Up;
            world.Right = Vector3.Cross(world.Forward, world.Up);
            //position.Y = ScreenManager.heightData[pX, pZ];
            world.Translation = Position;




            // world.Translation = new Vector3(0.0f, 0.0f, 0.0f);
            UpdateWorldTransforms(Matrix.Identity);
            UpdateSkinTransforms();

            //if(resetFormation)
            formation = world;





        }

        public void UpdatePathing(GameTime gameTime)
        {


            Console.WriteLine(pathFinder.SearchStatus);


            float dToT = Vector3.Distance(position, formVec);
            float rotationAmount = 0.0f;


            rotationAmount = TurnToFace(position, ScreenManager.p1Spear.World.Translation, new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));

            if (state == 1)
            {
                rotationAmount = TurnToFace(position, ScreenManager.eSpears[0].World.Translation, new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));

                dToT = Vector3.Distance(position, ScreenManager.eSpears[0].World.Translation);


                if (pathFinder.SearchStatus == SearchStatus.PathFound && !moving)
                {
                    foreach (Point point in pathFinder.FinalPath())
                    {
                        Waypoints.Enqueue(new Vector3(point.X * 100, 0.0f, point.Y * 100));
                    }
                    Moving = true;
                }
                pathFinder.Update(gameTime);

            }

            if (moving)
            {


                if (Waypoints.Count >= 1)
                {

                    destination = Waypoints.Peek();
                }
                if (AtDestination && Waypoints.Count >= 1)
                {
                    Waypoints.Dequeue();
                }

                if (!AtDestination)
                {
                    rotationAmount = TurnToFace(position, destination, new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));
                    thrustAmount = 1.0f;
                    if (!isRun)
                    {
                        isRun = true;
                        justBones.CopyTo(previousAnimation, 0);
                    }
                }









            }

            //if (dToT > 80.0f)
            //{
            //    thrustAmount = 1.0f;
            //    if (!isRun)
            //    {
            //        isRun = true;
            //        justBones.CopyTo(previousAnimation, 0);
            //    }
            //}
            //else
            //{
            //    thrustAmount = 0.0f;
            //    runTime = TimeSpan.Zero;
            //    isRun = false;
            //    if (state == 1)
            //    {
            //        if (!isAtk1)
            //        {
            //            isAtk1 = true;
            //            currentAnimationTime = TimeSpan.Zero;
            //        }
            //        if (dToT > 40.0f && isAtk1)
            //            thrustAmount = 1.0f;
            //        else
            //            thrustAmount = 0.0f;
            //    }

            //}

            //waiting
            if (state == 0)
            {

                isRun = false;
                thrustAmount = 0.0f;


            }




            float x = (float)Math.Sin(rotationAmount);
            float y = (float)Math.Cos(rotationAmount);
            Direction = new Vector3(x, 0.0f, y);

            Up = Vector3.Up;
            Right = Vector3.Cross(Direction, Up);

            if (isRun)
            {
                runTime += new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long)3.0);
                if (runTime.TotalSeconds < 9.0f)
                {
                    if (runTime.TotalSeconds < 1.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            lRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun1[i].Decompose(out scale, out rota, out trans);
                            lRun2[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun2[i].Decompose(out scale, out rota, out trans);
                            lRun3[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun3[i].Decompose(out scale, out rota, out trans);
                            lRun4[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 5.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun4[i].Decompose(out scale, out rota, out trans);
                            rRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 4.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 4.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 6.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun1[i].Decompose(out scale, out rota, out trans);
                            rRun2[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 5.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 5.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 7.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun2[i].Decompose(out scale, out rota, out trans);
                            rRun3[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 6.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 6.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 8.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun3[i].Decompose(out scale, out rota, out trans);
                            rRun4[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 7.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 7.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 9.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun4[i].Decompose(out scale, out rota, out trans);
                            lRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 8.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 8.0) / 1.0f));

                        }


                    }

                }
                else
                {

                    runTime = TimeSpan.Zero;
                    lRun1.CopyTo(previousAnimation, 0);



                }

            }
            currentAnimationTime += new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long)8);

            if (isAtk3)
            {


                if (currentAnimationTime.TotalSeconds < 4.0f)
                {

                    if (currentAnimationTime.TotalSeconds < 1.0f)
                    {

                        for (int i = 0; i < brace.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            atk3a[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds) / 1.0f));

                        }

                    }
                    else if (currentAnimationTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk3a[i].Decompose(out scale, out rota, out trans);
                            atk3b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk3b[i].Decompose(out scale, out rota, out trans);
                            atk3c[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk3c[i].Decompose(out scale, out rota, out trans);
                            atk3b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }




                }
                else
                {
                    currentAnimationTime = TimeSpan.Zero;
                    isAtk3 = false;
                }

            }
            if (isAtk1)
            {
                if (currentAnimationTime.TotalSeconds < 4.0f)
                {

                    if (currentAnimationTime.TotalSeconds < 1.0f)
                    {

                        for (int i = 0; i < brace.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            atk1a[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds) / 1.0f));

                        }

                    }
                    else if (currentAnimationTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk1a[i].Decompose(out scale, out rota, out trans);
                            atk1b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk1b[i].Decompose(out scale, out rota, out trans);
                            atk1c[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk1c[i].Decompose(out scale, out rota, out trans);
                            atk1d[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }




                }
                else if (!isDoubleStrike)
                {

                    isAtk1 = false;
                    currentAnimationTime = TimeSpan.Zero;



                }

                else if (currentAnimationTime.TotalSeconds < 5.0f)
                {
                    for (int i = 0; i < brace.Length; i++)
                    {
                        atk1d[i].Decompose(out scale, out rota, out trans);
                        atk2a[i].Decompose(out scale2, out rota2, out trans2);
                        upperBones[i] = Matrix.CreateScale(scale2) *
        Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 4.0) / 1.0f)) *
        Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 4.0) / 1.0f));

                    }


                }
                else if (currentAnimationTime.TotalSeconds < 6.0f)
                {
                    for (int i = 0; i < brace.Length; i++)
                    {
                        atk2a[i].Decompose(out scale, out rota, out trans);
                        atk2b[i].Decompose(out scale2, out rota2, out trans2);
                        upperBones[i] = Matrix.CreateScale(scale2) *
        Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 5.0) / 1.0f)) *
        Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 5.0) / 1.0f));

                    }


                }
                else
                {
                    currentAnimationTime = TimeSpan.Zero;
                    isAtk1 = false;
                    isDoubleStrike = false;

                }


            }



            Position += new Vector3(Direction.X, Direction.Y, Direction.Z) * thrustAmount * 2.0f;
            if (isRun)
                upperBones.CopyTo(justBones, 0);
            else if (isAtk3)
                upperBones.CopyTo(justBones, 0);
            else if (isAtk1)
                upperBones.CopyTo(justBones, 0);
            else
                brace.CopyTo(justBones, 0);



            int pX = (int)position.X / 100;
            int pZ = (int)position.Z / 100;

            if (Position.X < 0)
                pX = 0;
            if (Position.Z < 0)
                pZ = 0;
            if (position.X > 12799)
                pX = 127;
            if (position.Z > 12799)
                pZ = 127;
            //Console.WriteLine(
            // world.Forward = new Vector3(Direction.X, Direction.Y, Direction.Z);
            world = Matrix.Identity;
            world.Forward = new Vector3(-Direction.X, Direction.Y, -Direction.Z);
            world.Up = Vector3.Up;
            world.Right = Vector3.Cross(world.Forward, world.Up);
            //position.Y = ScreenManager.heightData[pX, pZ];
            world.Translation = Position;




            // world.Translation = new Vector3(0.0f, 0.0f, 0.0f);
            UpdateWorldTransforms(Matrix.Identity);
            UpdateSkinTransforms();




        }

        public void UpdateFormation(GameTime gameTime)
        {
            
            oldPosition = Position;
            float dToT = Vector3.Distance(position, formVec);
            float rotationAmount = 0.0f;


            rotationAmount = TurnToFace(position, formVec, new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));


            if (needPath)
            {
                if (pathFinder.SearchStatus == SearchStatus.PathFound && !moving)
                {
                    foreach (Point point in pathFinder.FinalPath())
                    {
                        Waypoints.Enqueue(new Vector3(point.X * 60, 0.0f, point.Y * 60));
                    }
                    Moving = true;
                    
                }
                pathFinder.Update(gameTime);




            }

            if (moving)
            {

                if (Waypoints.Count >= 1)
                {

                    destination = Waypoints.Peek();
                }
                if (AtDestination && Waypoints.Count >= 1)
                {
                    Waypoints.Dequeue();
                }
                if (AtDestination && Waypoints.Count == 0)
                {
                    needPath = false;
                    moving = false;

                    isRun = false;
                    thrustAmount = 0.0f;
                    //moving = false;


                }

                if (!AtDestination)
                {
                    rotationAmount = TurnToFace(position, destination, new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));
                    thrustAmount = 1.0f;
                    if (!isRun)
                    {
                        isRun = true;
                        justBones.CopyTo(previousAnimation, 0);
                    }
                }



            }

            //if (state == 1)
            //{
            //    rotationAmount = TurnToFace(position, ScreenManager.eSpears[0].World.Translation, new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));

            //    dToT = Vector3.Distance(position, ScreenManager.eSpears[0].World.Translation);

            //}

            if (!needPath)
            {



                if (dToT > 80.0f)
                {
                    thrustAmount = 1.0f;
                    if (!isRun)
                    {
                        isRun = true;
                        justBones.CopyTo(previousAnimation, 0);
                    }
                }
                else
                {
                    thrustAmount = 0.0f;
                    runTime = TimeSpan.Zero;
                    isRun = false;
                    if (state == 1)
                    {
                        if (!isAtk1)
                        {
                            isAtk1 = true;
                            currentAnimationTime = TimeSpan.Zero;
                        }
                        if (dToT > 40.0f && isAtk1)
                            thrustAmount = 1.0f;
                        else
                            thrustAmount = 0.0f;
                    }

                }

            }



            
            float x = (float)Math.Sin(rotationAmount);
            float y = (float)Math.Cos(rotationAmount);
            Direction = new Vector3(x, 0.0f, y);

            Up = Vector3.Up;
            Right = Vector3.Cross(Direction, Up);

            if (isRun)
            {
                runTime += new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long)3.0);
                if (runTime.TotalSeconds < 9.0f)
                {
                    if (runTime.TotalSeconds < 1.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            lRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun1[i].Decompose(out scale, out rota, out trans);
                            lRun2[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun2[i].Decompose(out scale, out rota, out trans);
                            lRun3[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun3[i].Decompose(out scale, out rota, out trans);
                            lRun4[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 5.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun4[i].Decompose(out scale, out rota, out trans);
                            rRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 4.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 4.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 6.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun1[i].Decompose(out scale, out rota, out trans);
                            rRun2[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 5.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 5.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 7.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun2[i].Decompose(out scale, out rota, out trans);
                            rRun3[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 6.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 6.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 8.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun3[i].Decompose(out scale, out rota, out trans);
                            rRun4[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 7.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 7.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 9.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun4[i].Decompose(out scale, out rota, out trans);
                            lRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 8.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 8.0) / 1.0f));

                        }


                    }

                }
                else
                {

                    runTime = TimeSpan.Zero;
                    lRun1.CopyTo(previousAnimation, 0);



                }

            }
            currentAnimationTime += new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long)8);

            if (isAtk3)
            {


                if (currentAnimationTime.TotalSeconds < 4.0f)
                {

                    if (currentAnimationTime.TotalSeconds < 1.0f)
                    {

                        for (int i = 0; i < brace.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            atk3a[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds) / 1.0f));

                        }

                    }
                    else if (currentAnimationTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk3a[i].Decompose(out scale, out rota, out trans);
                            atk3b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk3b[i].Decompose(out scale, out rota, out trans);
                            atk3c[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk3c[i].Decompose(out scale, out rota, out trans);
                            atk3b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }




                }
                else
                {
                    currentAnimationTime = TimeSpan.Zero;
                    isAtk3 = false;
                }

            }
            if (isAtk1)
            {
                if (currentAnimationTime.TotalSeconds < 4.0f)
                {

                    if (currentAnimationTime.TotalSeconds < 1.0f)
                    {

                        for (int i = 0; i < brace.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            atk1a[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds) / 1.0f));

                        }

                    }
                    else if (currentAnimationTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk1a[i].Decompose(out scale, out rota, out trans);
                            atk1b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk1b[i].Decompose(out scale, out rota, out trans);
                            atk1c[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk1c[i].Decompose(out scale, out rota, out trans);
                            atk1d[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }




                }
                else if (!isDoubleStrike)
                {

                    isAtk1 = false;
                    currentAnimationTime = TimeSpan.Zero;



                }

                else if (currentAnimationTime.TotalSeconds < 5.0f)
                {
                    for (int i = 0; i < brace.Length; i++)
                    {
                        atk1d[i].Decompose(out scale, out rota, out trans);
                        atk2a[i].Decompose(out scale2, out rota2, out trans2);
                        upperBones[i] = Matrix.CreateScale(scale2) *
        Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 4.0) / 1.0f)) *
        Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 4.0) / 1.0f));

                    }


                }
                else if (currentAnimationTime.TotalSeconds < 6.0f)
                {
                    for (int i = 0; i < brace.Length; i++)
                    {
                        atk2a[i].Decompose(out scale, out rota, out trans);
                        atk2b[i].Decompose(out scale2, out rota2, out trans2);
                        upperBones[i] = Matrix.CreateScale(scale2) *
        Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 5.0) / 1.0f)) *
        Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 5.0) / 1.0f));

                    }


                }
                else
                {
                    currentAnimationTime = TimeSpan.Zero;
                    isAtk1 = false;
                    isDoubleStrike = false;

                }


            }


            if(!needPath || moving)
            Position += new Vector3(Direction.X, Direction.Y, Direction.Z) * thrustAmount * 2.0f;
             int x2 = (int)Position.X / 60;
             int y2 = (int)Position.Z / 60;

             if (x2 < 0)
                 x2 = 0;
             if (x2 > 9)
                 x2 = 9;
             if (y2 < 0)
                 y2 = 0;
             if (y2 > 9)
                 y2 = 9;

            //if (!ScreenManager.open[x2][y2] &!needPath)
            //{
            //    Position = oldPosition;
                
            //    needPath = true;
            //    pathFinder.Reset2(formVec, oldPosition);
            //}
            if (isRun)
                upperBones.CopyTo(justBones, 0);
            else if (isAtk3)
                upperBones.CopyTo(justBones, 0);
            else if (isAtk1)
                upperBones.CopyTo(justBones, 0);
            else
                brace.CopyTo(justBones, 0);



            int pX = (int)position.X / 100;
            int pZ = (int)position.Z / 100;

            if (Position.X < 0)
                pX = 0;
            if (Position.Z < 0)
                pZ = 0;
            if (position.X > 12799)
                pX = 127;
            if (position.Z > 12799)
                pZ = 127;
            //Console.WriteLine(
            // world.Forward = new Vector3(Direction.X, Direction.Y, Direction.Z);
            world = Matrix.Identity;
            world.Forward = new Vector3(-Direction.X, Direction.Y, -Direction.Z);
            world.Up = Vector3.Up;
            world.Right = Vector3.Cross(world.Forward, world.Up);
            //position.Y = ScreenManager.heightData[pX, pZ];
            world.Translation = Position;




            // world.Translation = new Vector3(0.0f, 0.0f, 0.0f);
            UpdateWorldTransforms(Matrix.Identity);
            UpdateSkinTransforms();




        }

        public void UpdateCrete(GameTime gameTime)
        {
            ///distance to target
            float dToT = Vector3.Distance(position, ScreenManager.p1Mino.World.Translation);
            float rotationAmount = 0.0f;
        
                rotationAmount = TurnToFace(position, ScreenManager.p1Mino.World.Translation, new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));

                if (dToT > 80.0f)
                {
                    thrustAmount = 1.0f;
                    isRun = true;
                }
                else
                {
                    thrustAmount = 0.0f;
                    runTime = TimeSpan.Zero;
                    isRun = false;
                }

                if (dToT < 80.0f)
                {
                    if (!isAtk1)
                    {
                        isAtk1 = true;
                        currentAnimationTime = TimeSpan.Zero;
                    }
                    if (dToT > 40.0f && isAtk1)
                        thrustAmount = 1.0f;
                    else
                        thrustAmount = 0.0f;
                    
                }
            



            float x = (float)Math.Sin(rotationAmount);
            float y = (float)Math.Cos(rotationAmount);
            Direction = new Vector3(x, 0.0f, y);

            Up = Vector3.Up;
            Right = Vector3.Cross(Direction, Up);

            if (isRun)
            {
                runTime += new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long)3.0);
                if (runTime.TotalSeconds < 9.0f)
                {
                    if (runTime.TotalSeconds < 1.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            lRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun1[i].Decompose(out scale, out rota, out trans);
                            lRun2[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun2[i].Decompose(out scale, out rota, out trans);
                            lRun3[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun3[i].Decompose(out scale, out rota, out trans);
                            lRun4[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 5.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun4[i].Decompose(out scale, out rota, out trans);
                            rRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 4.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 4.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 6.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun1[i].Decompose(out scale, out rota, out trans);
                            rRun2[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 5.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 5.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 7.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun2[i].Decompose(out scale, out rota, out trans);
                            rRun3[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 6.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 6.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 8.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun3[i].Decompose(out scale, out rota, out trans);
                            rRun4[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 7.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 7.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 9.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun4[i].Decompose(out scale, out rota, out trans);
                            lRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 8.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 8.0) / 1.0f));

                        }


                    }

                }
                else
                {

                    runTime = TimeSpan.Zero;
                    lRun1.CopyTo(previousAnimation, 0);



                }

            }

            currentAnimationTime += new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long)8);

            if (isAtk3)
            {


                if (currentAnimationTime.TotalSeconds < 4.0f)
                {

                    if (currentAnimationTime.TotalSeconds < 1.0f)
                    {

                        for (int i = 0; i < brace.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            atk3a[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds) / 1.0f));

                        }

                    }
                    else if (currentAnimationTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk3a[i].Decompose(out scale, out rota, out trans);
                            atk3b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk3b[i].Decompose(out scale, out rota, out trans);
                            atk3c[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk3c[i].Decompose(out scale, out rota, out trans);
                            atk3b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }




                }
                else
                {
                    currentAnimationTime = TimeSpan.Zero;
                    isAtk3 = false;
                }

            }
            if (isAtk1)
            {
                if (currentAnimationTime.TotalSeconds < 4.0f)
                {

                    if (currentAnimationTime.TotalSeconds < 1.0f)
                    {

                        for (int i = 0; i < brace.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            atk1a[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds) / 1.0f));

                        }

                    }
                    else if (currentAnimationTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk1a[i].Decompose(out scale, out rota, out trans);
                            atk1b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk1b[i].Decompose(out scale, out rota, out trans);
                            atk1c[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk1c[i].Decompose(out scale, out rota, out trans);
                            atk1d[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }




                }
                else if (!isDoubleStrike)
                {

                    isAtk1 = false;
                    currentAnimationTime = TimeSpan.Zero;



                }

                else if (currentAnimationTime.TotalSeconds < 5.0f)
                {
                    for (int i = 0; i < brace.Length; i++)
                    {
                        atk1d[i].Decompose(out scale, out rota, out trans);
                        atk2a[i].Decompose(out scale2, out rota2, out trans2);
                        upperBones[i] = Matrix.CreateScale(scale2) *
        Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 4.0) / 1.0f)) *
        Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 4.0) / 1.0f));

                    }


                }
                else if (currentAnimationTime.TotalSeconds < 6.0f)
                {
                    for (int i = 0; i < brace.Length; i++)
                    {
                        atk2a[i].Decompose(out scale, out rota, out trans);
                        atk2b[i].Decompose(out scale2, out rota2, out trans2);
                        upperBones[i] = Matrix.CreateScale(scale2) *
        Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 5.0) / 1.0f)) *
        Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 5.0) / 1.0f));

                    }


                }
                else
                {
                    currentAnimationTime = TimeSpan.Zero;
                    isAtk1 = false;
                    isDoubleStrike = false;

                }


            }




            //world = Matrix.Identity;
            //world.Forward = new Vector3(Direction.X, Direction.Y, Direction.Z);
            //world.Up = Up;
            //world.Right = Right;
            //world.Translation = Position + new Vector3(0.0f, 0.0f, 0.0f);


            //if (isAtk1)
            // upperBones.CopyTo(justBones, 0);
            //else

            // world = Matrix.Identity;

            Position += new Vector3(Direction.X, Direction.Y, Direction.Z) * thrustAmount * 2.0f;
            if (isRun)
                upperBones.CopyTo(justBones, 0);
            else if (isAtk3)
                upperBones.CopyTo(justBones, 0);
            else if (isAtk1)
                upperBones.CopyTo(justBones, 0);
            else
            brace.CopyTo(justBones, 0);



            int pX = (int)position.X / 100;
            int pZ = (int)position.Z / 100;

            if (Position.X < 0)
                pX = 0;
            if (Position.Z < 0)
                pZ = 0;
            if (position.X > 12799)
                pX = 127;
            if (position.Z > 12799)
                pZ = 127;
            //Console.WriteLine(
            // world.Forward = new Vector3(Direction.X, Direction.Y, Direction.Z);
            world = Matrix.Identity;
            world.Forward = new Vector3(-Direction.X, Direction.Y, -Direction.Z);
            world.Up = Vector3.Up;
            world.Right = Vector3.Cross(world.Forward, world.Up);
            //position.Y = ScreenManager.heightData[pX, pZ];
            world.Translation = Position;




            // world.Translation = new Vector3(0.0f, 0.0f, 0.0f);
            UpdateWorldTransforms(Matrix.Identity);
            UpdateSkinTransforms();

            // position = new Vector3(-100.0f, 0.0f, 0.0f);



        }

        public void UpdateCrucible(GameTime gameTime)
        {


            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            oldGamePadState = currentGamePadState;
            oldkeyBoardState = currentKeyBoardState;
            World.Decompose(out scale, out rota, out trans);
            oldPosition = Position;
            oldDirection = Direction;
            currentGamePadState = GamePad.GetState(PlayerIndex.One);


            Vector2 rotationAmount = currentGamePadState.ThumbSticks.Left;

            Vector2 inputAmount = currentGamePadState.ThumbSticks.Right;

            //rotAmt = TurnToFace(ScreenManager.asterion.Position, ScreenManager.medusa.Position, Vector3.Zero);
            // Direction = new Vector3((float)Math.Sin(rotAmt), 0.0f, (float)Math.Cos(rotAmt));

            if (gameTime.TotalGameTime - playerMoveTime > MoveTimeOut)
            {
                playerMove = null;
            }
            //inputManager.Update(gameTime);
            Move newMove = ScreenManager.moveList.DetectMove(ScreenManager.inputManager);

            if (newMove != null)
            {
                playerMove = newMove;

                playerMoveTime = gameTime.TotalGameTime;

                if (playerMove.Name == "A")
                {
                    if (!isAtk1)
                    {
                        isAtk1 = true;
                        currentAnimationTime = TimeSpan.Zero;
                        justBones.CopyTo(previousAnimation, 0);
                    }

                }
                if (playerMove.Name == "AA")
                {
                    //if (!isAtk1 && !isAtk2)

                    if (!isDoubleStrike)
                    {

                        isDoubleStrike = true;
                        //currentAnimationTime = TimeSpan.Zero;
                        //justBones.CopyTo(previousAnimation, 0);

                        if (!isAtk1)
                            currentAnimationTime = TimeSpan.Zero;


                    }


                }
                if (playerMove.Name == "AAA")
                {

                    if (!isRapidStrikes)
                    {
                        isRapidStrikes = true;

                        if (!isAtk1)
                            currentAnimationTime = TimeSpan.Zero;


                        //  justBones.CopyTo(previousAnimation, 0);

                    }




                }
                if (playerMove.Name == "X")
                {
                    if (!isAtk3)
                    {
                        isAtk3 = true;
                        currentAnimationTime = TimeSpan.Zero;
                    }
                    justBones.CopyTo(previousAnimation, 0);




                }
                Console.WriteLine(playerMove.Name);

            }
            //if (Math.Abs(rotationAmount.X) > .3f || Math.Abs(rotationAmount.Y) > .3f)
            {
                //Direction = new Vector3(rotationAmount.X, 0.0f, rotationAmount.Y);


                if (ScreenManager.medusa.Position.X < position.X)
                    Direction = Vector3.Right;
                else
                    Direction = Vector3.Left;


                thrustAmount = rotationAmount.X;
                if (Math.Abs(thrustAmount) > 0.0f)
                {
                    isRun = true;
                    justBones.CopyTo(previousAnimation, 0);
                }
                else
                {
                    isRun = false;
                    runTime = TimeSpan.Zero;
                }
                //  else
                //  thrustAmount = rotationAmount.Y;
            }
            //else
            //    thrustAmount = 0.0f;

            Up.Normalize();
            Direction.Normalize();


            Right = Vector3.Cross(Direction, Up);
            if (isRun)
            {
                runTime += new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long)3.0);
                if (runTime.TotalSeconds < 9.0f)
                {

                    //        if (runTime.TotalSeconds < 1.0f)
                    //        {

                    //            for (int i = 0; i < brace.Length; i++)
                    //            {
                    //                previousAnimation[i].Decompose(out scale, out rota, out trans);
                    //                run1a[i].Decompose(out scale2, out rota2, out trans2);
                    //                upperBones[i] = Matrix.CreateScale(scale2) *
                    //Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds) / 1.0f)) *
                    //Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds) / 1.0f));

                    //            }

                    //        }
                    //        else 
                    if (runTime.TotalSeconds < 1.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            lRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun1[i].Decompose(out scale, out rota, out trans);
                            lRun2[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun2[i].Decompose(out scale, out rota, out trans);
                            lRun3[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun3[i].Decompose(out scale, out rota, out trans);
                            lRun4[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 5.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            lRun4[i].Decompose(out scale, out rota, out trans);
                            rRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 4.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 4.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 6.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun1[i].Decompose(out scale, out rota, out trans);
                            rRun2[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 5.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 5.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 7.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun2[i].Decompose(out scale, out rota, out trans);
                            rRun3[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 6.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 6.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 8.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun3[i].Decompose(out scale, out rota, out trans);
                            rRun4[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 7.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 7.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 9.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            rRun4[i].Decompose(out scale, out rota, out trans);
                            lRun1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 8.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 8.0) / 1.0f));

                        }


                    }

                }
                else
                {

                    runTime = TimeSpan.Zero;
                    lRun1.CopyTo(previousAnimation, 0);



                }

            }
            currentAnimationTime += new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long)8);

            if (isAtk3)
            {


                if (currentAnimationTime.TotalSeconds < 4.0f)
                {

                    if (currentAnimationTime.TotalSeconds < 1.0f)
                    {

                        for (int i = 0; i < brace.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            atk3a[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds) / 1.0f));

                        }

                    }
                    else if (currentAnimationTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk3a[i].Decompose(out scale, out rota, out trans);
                            atk3b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk3b[i].Decompose(out scale, out rota, out trans);
                            atk3c[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk3c[i].Decompose(out scale, out rota, out trans);
                            atk3b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }




                }
                else
                {
                    currentAnimationTime = TimeSpan.Zero;
                    isAtk3 = false;
                }

            }
            if (isAtk1)
            {
                if (currentAnimationTime.TotalSeconds < 4.0f)
                {

                    if (currentAnimationTime.TotalSeconds < 1.0f)
                    {

                        for (int i = 0; i < brace.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            atk1a[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds) / 1.0f));

                        }

                    }
                    else if (currentAnimationTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk1a[i].Decompose(out scale, out rota, out trans);
                            atk1b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk1b[i].Decompose(out scale, out rota, out trans);
                            atk1c[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            atk1c[i].Decompose(out scale, out rota, out trans);
                            atk1d[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }




                }
                else if (!isDoubleStrike)
                {

                    isAtk1 = false;
                    currentAnimationTime = TimeSpan.Zero;



                }

                else if (currentAnimationTime.TotalSeconds < 5.0f)
                {
                    for (int i = 0; i < brace.Length; i++)
                    {
                        atk1d[i].Decompose(out scale, out rota, out trans);
                        atk2a[i].Decompose(out scale2, out rota2, out trans2);
                        upperBones[i] = Matrix.CreateScale(scale2) *
        Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 4.0) / 1.0f)) *
        Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 4.0) / 1.0f));

                    }


                }
                else if (currentAnimationTime.TotalSeconds < 6.0f)
                {
                    for (int i = 0; i < brace.Length; i++)
                    {
                        atk2a[i].Decompose(out scale, out rota, out trans);
                        atk2b[i].Decompose(out scale2, out rota2, out trans2);
                        upperBones[i] = Matrix.CreateScale(scale2) *
        Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 5.0) / 1.0f)) *
        Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 5.0) / 1.0f));

                    }


                }
                else
                {
                    currentAnimationTime = TimeSpan.Zero;
                    isAtk1 = false;
                    isDoubleStrike = false;

                }


            }




            //world = Matrix.Identity;
            //world.Forward = new Vector3(Direction.X, Direction.Y, Direction.Z);
            //world.Up = Up;
            //world.Right = Right;
            //world.Translation = Position + new Vector3(0.0f, 0.0f, 0.0f);


            //if (isAtk1)
            // upperBones.CopyTo(justBones, 0);
            //else

            // world = Matrix.Identity;

            Position += new Vector3(thrustAmount, 0.0f, 0.0f);
            if (isRun)
                upperBones.CopyTo(justBones, 0);
            else if (isAtk3)
                upperBones.CopyTo(justBones, 0);
            else if (isAtk1)
                upperBones.CopyTo(justBones, 0);
            else
                brace.CopyTo(justBones, 0);
            if (ScreenManager.medusa.Position.X < position.X)
                world.Forward = Vector3.Right;
            else
                world.Forward = Vector3.Left;

            // world.Forward = new Vector3(Direction.X, Direction.Y, Direction.Z);
            world.Up = Vector3.Up;
            world.Right = Vector3.Cross(world.Forward, world.Up);
            world.Translation = Position;

            // world.Translation = new Vector3(0.0f, 0.0f, 0.0f);
            UpdateWorldTransforms(Matrix.Identity);
            UpdateSkinTransforms();

            // position = new Vector3(-100.0f, 0.0f, 0.0f);



        }
        public void UpdateWorldTransforms(Matrix rootTransform)
        {
            // Root bone.
            worldTrans[0] = justBones[0] * world;

            // Child bones.
            for (int bone = 1; bone < worldTrans.Length; bone++)
            {
                int parentBone = skinningData.SkeletonHierarchy[bone];

                worldTrans[bone] = justBones[bone] *
                                             worldTrans[parentBone];
            }
        }
        /// <summary>
        /// Helper used by the Update method to refresh the SkinTransforms data.
        /// </summary>
        public void UpdateSkinTransforms()
        {
            for (int bone = 0; bone < skinTrans.Length; bone++)
            {
                skinTrans[bone] = skinningData.InverseBindPose[bone] *
                                            worldTrans[bone];
            }
        }



    }
}
