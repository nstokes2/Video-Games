using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SmellOfRevenge2011
{
    public class JuneAIModel
    {


        public Vector3 knockBackVec;
        public bool isKnockedBack = false;

        //0 is Theseus, 1 is Perseus, 2 is Paris
        public int testingIndex = 0;
        public int health = 10;
        public Matrix forwardSpell;
        public List<Projectile2> projectiles;


        public List<boundingSphere> spheres;
        public List<boundingSphere> spearSpheres;



        public List<boundingSphere> spellSpheres;
        public List<boundingSphere> rSpear;
        public List<boundingSphere> lSpear;
        public List<boundingSphere> rSword;
        public List<boundingSphere> lSword;
        public List<boundingSphere> axe;
        public List<boundingSphere> lTAxe;
        public List<boundingSphere> rTAxe;
        public List<boundingSphere> arrow;
        public List<boundingSphere> roundShield;
        public List<boundingSphere> towerShield;
        public List<boundingSphere> bow;




        public Vector3[] spots = new Vector3[10];
        public Matrix formation = new Matrix();



        public Vector3[] CFormation = new Vector3[9];
        public Vector3[] NFormation = new Vector3[9];
        public Vector3[] SFormation = new Vector3[9];
        public Vector3[] EFormation = new Vector3[9];
        public Vector3[] WFormation = new Vector3[9];


        public bool resetFormations = true;
        public TimeSpan currentAnimationTime = TimeSpan.Zero;
        TimeSpan runTime = TimeSpan.Zero;
        public float rotAmt = 0.0f;
        public float thrustAmount = 0.0f;

        //        shield1 10 shield2 10.5 shieldbash1 11 shieldbash2 11.5 shieldUpper 12 sU2 12.5 sU3 13 sU4 13.5
        // shield toss 14  shield toss2 14.5 sT3 15 Shield Spin 15.15 SS2 16 SS3 16.5 SS4 17 Spear1 17.5 S2 18 SpearSpin 18.5 - 21 FlyingSpear 21.5 22.5
        //Spear Pinion 23 -24 Spear throw 1 24.5 St2 25 ST followthrough 25.5 SpearOFfhandPinion 26 26.5 bow1 27 aim 27.5 release 28 release followthrough on special arrow
        //KB1 28.5 KB1.5 29 KB2 29.5 KnockDown = 30

        public Matrix[] standing, brace, atk1a, atk1b, atk1c, atk1d, atk2a, atk2b, atk3a, atk3b, atk3c, lRun1, lRun2, lRun3, lRun4, rRun1, rRun2, rRun3, rRun4, previousAnimation;
        public Matrix[] shield1, shield2, shieldBash1, shieldBash2, shieldUpper1, shieldUpper2, shieldUpper3, shieldUpper4, shieldToss1, shieldToss2,
            shieldToss3, shieldSpin1, shieldSpin2, shieldSpin3, shieldSpin4, spear1, spear2, spearSpin1, spearSpin2, spearSpin3, spearSpin4, spearSpin5, spearSpin6,
            flySpear1, flySpear2, flySpear3, spearPin1, spearPin2, spearPin3, spearThrow1, spearThrow2, spearThrowFinish, spearOffhandPin1, spearOffHandPin2,
            bow1, aim, release, releaseFollowThru, kb1, kb2, kb3, knockDown;

        Move playerMove;
        TimeSpan playerMoveTime;

        public bool isShield, isShieldBash, isShieldUpper, isShieldToss, isShieldSpin, isSpear, isSpearSpin, isFlyingSpear, isSpearPin, isSpearThrow, isSpearOffPin, isBow, isBowSpecial,
            isKnockBack, isKnockDown, isTripleBow;

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



        public void UpdateTheseus(GameTime gameTime)
        {


            oldPosition = Position;
            float dToT = Vector3.Distance(Position, ScreenManager.p1June.World.Translation);
            float rotationAmount = 0.0f;
            rotationAmount = TurnToFace(position, ScreenManager.p1June.World.Translation, new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));


            //dToT = Vector3.Distance(Position, formVec);
            //rotationAmount = TurnToFace(position, formVec, new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));


            float x = (float)Math.Sin(rotationAmount);
            float y = (float)Math.Cos(rotationAmount);
            Direction = new Vector3(x, 0.0f, y);

            if(!isKnockBack)
            brace.CopyTo(previousAnimation, 0);

            Up = Vector3.Up;
            Right = Vector3.Cross(Direction, Up);
            if (!isKnockBack)
                isShieldSpin = true;
            else
                isShieldSpin = false;


            if (isKnockedBack)
            {

                if (!isKnockBack)
                {
                    isKnockBack = true;

                    currentAnimationTime = TimeSpan.Zero;

                    justBones.CopyTo(previousAnimation, 0);
                    isShieldBash = false;
                    isShieldUpper = false;
                    isShieldToss = false;
                }

            }
            currentAnimationTime += gameTime.ElapsedGameTime;
            if (isShieldSpin)
            {
                if (currentAnimationTime.TotalSeconds < 4.0f)
                {

                    if (currentAnimationTime.TotalSeconds < 1.0f)
                    {

                        for (int i = 0; i < brace.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            shieldSpin1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds) / 1.0f));

                        }

                    }
                    else if (currentAnimationTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            shieldSpin1[i].Decompose(out scale, out rota, out trans);
                            shieldSpin2[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            shieldSpin2[i].Decompose(out scale, out rota, out trans);
                            shieldSpin3[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            shieldSpin3[i].Decompose(out scale, out rota, out trans);
                            shieldSpin4[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }




                }
                else
                {
                    currentAnimationTime = TimeSpan.Zero;
                    isShieldSpin = false;
                }

            }
            currentAnimationTime += new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long)8);
            if (isKnockBack)
            {

                if (currentAnimationTime.TotalSeconds < 4.0f)
                {

                    if (currentAnimationTime.TotalSeconds < 1.0f)
                    {

                        for (int i = 0; i < brace.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            kb1[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds) / 1.0f));

                        }

                    }
                    else if (currentAnimationTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            kb1[i].Decompose(out scale, out rota, out trans);
                            kb2[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            kb2[i].Decompose(out scale, out rota, out trans);
                            kb3[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < brace.Length; i++)
                        {
                            kb3[i].Decompose(out scale, out rota, out trans);
                            knockDown[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }

                }
                else
                {
                    currentAnimationTime = TimeSpan.Zero;
                    isKnockBack = false;
                    isKnockedBack = false;
                }












            }







            if (isShieldSpin)
                upperBones.CopyTo(justBones, 0);


            
            if (isKnockBack)
                Position = oldPosition + knockBackVec * 1.2f;

            world.Forward = new Vector3(Direction.X, Direction.Y, Direction.Z);

            // world.Forward = new Vector3(Direction.X, Direction.Y, Direction.Z);
            world.Up = Vector3.Up;
            world.Right = Vector3.Cross(world.Forward, world.Up);
            world.Translation = Position;
            // world.Translation = new Vector3(0.0f, 0.0f, 0.0f);
            UpdateWorldTransforms(Matrix.Identity);
            UpdateSkinTransforms();


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

                public JuneAIModel(Vector3 pos, Vector3 dir)
        {
            forwardSpell = new Matrix();
            projectiles = new List<Projectile2>();
            spheres = new List<boundingSphere>();
            spearSpheres = new List<boundingSphere>();



            spellSpheres = new List<boundingSphere>();
            rSpear = new List<boundingSphere>();
            lSpear = new List<boundingSphere>();
            rSword = new List<boundingSphere>();
            lSword = new List<boundingSphere>();
            axe = new List<boundingSphere>();
            lTAxe = new List<boundingSphere>();
            rTAxe = new List<boundingSphere>();
            arrow = new List<boundingSphere>();
            roundShield = new List<boundingSphere>();
            towerShield = new List<boundingSphere>();
            bow = new List<boundingSphere>();





            position = pos;
            direction = dir;
            Up = Vector3.Up;
            right = Vector3.Right;


            inputManager = new InputManager((PlayerIndex)0, ScreenManager.moveList.LongestMoveLength);

            isAtk1 = false;
            isAtk2 = false;
            isAtk3 = false;
            isRapidStrikes = false;
            isDoubleStrike = false;
            isStanding = false;

            isRun = false;


            isShield= false; 
            isShieldBash= false; 
            isShieldUpper= false;
            isShieldToss= false; 
            isShieldSpin= false; 
            isSpear= false;
            isSpearSpin= false;
            isFlyingSpear= false;
            isSpearPin= false;
            isSpearThrow= false; 
            isSpearOffPin= false; 
            isBow= false;
            isBowSpecial= false;
            isKnockBack = false; 
            isKnockDown = false;
            isTripleBow = false;

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


            shield1= new Matrix[skinningData.BindPose.Count];
            shield2= new Matrix[skinningData.BindPose.Count]; 
            shieldBash1= new Matrix[skinningData.BindPose.Count]; 
            shieldBash2= new Matrix[skinningData.BindPose.Count];  
            shieldUpper1= new Matrix[skinningData.BindPose.Count];  
            shieldUpper2= new Matrix[skinningData.BindPose.Count]; 
            shieldUpper3= new Matrix[skinningData.BindPose.Count]; 
            shieldUpper4= new Matrix[skinningData.BindPose.Count]; 
            shieldToss1= new Matrix[skinningData.BindPose.Count]; 
            shieldToss2= new Matrix[skinningData.BindPose.Count]; 
            shieldToss3= new Matrix[skinningData.BindPose.Count]; 
            shieldSpin1= new Matrix[skinningData.BindPose.Count];  
            shieldSpin2= new Matrix[skinningData.BindPose.Count]; 
            shieldSpin3= new Matrix[skinningData.BindPose.Count];  
            shieldSpin4= new Matrix[skinningData.BindPose.Count]; 
            spear1= new Matrix[skinningData.BindPose.Count]; 
            spear2= new Matrix[skinningData.BindPose.Count];  
            spearSpin1= new Matrix[skinningData.BindPose.Count]; 
            spearSpin2= new Matrix[skinningData.BindPose.Count]; 
            spearSpin3= new Matrix[skinningData.BindPose.Count];  
            spearSpin4= new Matrix[skinningData.BindPose.Count]; 
            spearSpin5= new Matrix[skinningData.BindPose.Count]; 
            spearSpin6= new Matrix[skinningData.BindPose.Count]; 
            flySpear1= new Matrix[skinningData.BindPose.Count];  
            flySpear2= new Matrix[skinningData.BindPose.Count]; 
            flySpear3= new Matrix[skinningData.BindPose.Count];  
            spearPin1= new Matrix[skinningData.BindPose.Count];  
            spearPin2= new Matrix[skinningData.BindPose.Count]; 
            spearPin3= new Matrix[skinningData.BindPose.Count];  
            spearThrow1= new Matrix[skinningData.BindPose.Count]; 
            spearThrow2= new Matrix[skinningData.BindPose.Count]; 
            spearThrowFinish= new Matrix[skinningData.BindPose.Count]; 
            spearOffhandPin1= new Matrix[skinningData.BindPose.Count];  
            spearOffHandPin2= new Matrix[skinningData.BindPose.Count]; 
            bow1= new Matrix[skinningData.BindPose.Count];  
            aim= new Matrix[skinningData.BindPose.Count]; 
            release= new Matrix[skinningData.BindPose.Count]; 
            releaseFollowThru= new Matrix[skinningData.BindPose.Count]; 
            kb1= new Matrix[skinningData.BindPose.Count]; 
            kb2= new Matrix[skinningData.BindPose.Count]; 
            kb3= new Matrix[skinningData.BindPose.Count];
            knockDown = new Matrix[skinningData.BindPose.Count];


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
            masterPlayer.Update(TimeSpan.FromMilliseconds(9500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(rRun4, 0);




            masterPlayer.Update(TimeSpan.FromMilliseconds(10000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(shield1, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(10500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(shield2, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(11000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(shieldBash1, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(11500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(shieldBash2, 0);

            masterPlayer.Update(TimeSpan.FromMilliseconds(12000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(shieldUpper1, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(12500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(shieldUpper2, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(13000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(shieldUpper3, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(13500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(shieldUpper4, 0);



            masterPlayer.Update(TimeSpan.FromMilliseconds(14000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(shieldToss1, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(14500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(shieldToss2, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(15000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(shieldToss3, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(15500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(shieldSpin1, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(16000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(shieldSpin2, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(16500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(shieldSpin3, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(17000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(shieldSpin4, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(17500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(spear1, 0);

            //shield1, shield2, shieldBash1, shieldBash2, shieldUpper1, shieldUpper2, shieldUpper3, shieldUpper4, shieldToss1, shieldToss2,
            //shieldToss3, shieldSpin1, shieldSpin2, shieldSpin3, shieldSpin4, spear1, spear2, spearSpin1, spearSpin2, spearSpin3, spearSpin4, spearSpin5, spearSpin6,
            //flySpear1, flySpear2, flySpear3, spearPin1, spearPin2, spearPin3, spearThrow1, spearThrow2, spearThrowFinish, spearOffhandPin1, spearOffHandPin2,
            //bow1, aim, release, releaseFollowThru, kb1, kb2, kb3, knockDown;

            masterPlayer.Update(TimeSpan.FromMilliseconds(18000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(spear2, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(18500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(spearSpin1, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(19000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(spearSpin2, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(19500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(spearSpin3, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(20000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(spearSpin4, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(20500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(spearSpin5, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(21000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(spearSpin6, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(21500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(flySpear1, 0);


            //shield1, shield2, shieldBash1, shieldBash2, shieldUpper1, shieldUpper2, shieldUpper3, shieldUpper4, shieldToss1, shieldToss2,
            //shieldToss3, shieldSpin1, shieldSpin2, shieldSpin3, shieldSpin4, spear1, spear2, spearSpin1, spearSpin2, spearSpin3, spearSpin4, spearSpin5, spearSpin6,
            //flySpear1, flySpear2, flySpear3, spearPin1, spearPin2, spearPin3, spearThrow1, spearThrow2, spearThrowFinish, spearOffhandPin1, spearOffHandPin2,
            //bow1, aim, release, releaseFollowThru, kb1, kb2, kb3, knockDown;

            masterPlayer.Update(TimeSpan.FromMilliseconds(22000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(flySpear2, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(22500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(flySpear3, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(23000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(spearPin1, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(23500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(spearPin2, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(24000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(spearPin3, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(24500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(spearThrow1, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(25000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(spearThrow2, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(25500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(spearThrowFinish, 0);


            //shield1, shield2, shieldBash1, shieldBash2, shieldUpper1, shieldUpper2, shieldUpper3, shieldUpper4, shieldToss1, shieldToss2,
            //shieldToss3, shieldSpin1, shieldSpin2, shieldSpin3, shieldSpin4, spear1, spear2, spearSpin1, spearSpin2, spearSpin3, spearSpin4, spearSpin5, spearSpin6,
            //flySpear1, flySpear2, flySpear3, spearPin1, spearPin2, spearPin3, spearThrow1, spearThrow2, spearThrowFinish, spearOffhandPin1, spearOffHandPin2,
            //bow1, aim, release, releaseFollowThru, kb1, kb2, kb3, knockDown;

            masterPlayer.Update(TimeSpan.FromMilliseconds(26000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(spearOffhandPin1, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(26500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(bow1, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(27000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(aim, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(27500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(release, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(28000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(releaseFollowThru, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(28500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(kb1, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(29000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(kb2, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(29500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(kb3, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(29999), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(knockDown, 0);






        }
    }
}
