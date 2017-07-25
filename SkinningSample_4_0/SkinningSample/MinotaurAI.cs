using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;


namespace SmellOfRevenge2011
{
    public class MinotaurAI
    {
        public TimeSpan currentAnimationTime = TimeSpan.Zero;
        public TimeSpan runTime = TimeSpan.Zero;
        public float rotAmt = 0.0f;
        public float thrustAmount = 0.0f;
        public Matrix[] previousAnimation;
        public Matrix[] standing, rdy, atk1a, atk1b, atk2a, atk2b, atk3a, atk3b, atk3c, atk3d, rdy2, runA, run1a, run1b, run2a, run2b;
        List<Move> availableMoves;
        Move playerMove;
        TimeSpan playerMoveTime;


        public bool isAtk1, isAtk2, isAtk3, isRun, isStanding, isRapidStrikes, isDoubleStrike;

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

        public Vector3 oldPosition;
        public Vector3 oldDirection;
        Vector3 scale, scale2, scale3, scale4;
        Quaternion rota, rota2, rota3, rota4;
        Vector3 trans, trans2, trans3, trans4;


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
        public MinotaurAI(Vector3 pos, Vector3 dir)
        {
            position = pos;
            direction = dir;
            Up = Vector3.Up;
            right = Vector3.Right;


            //inputManager = new InputManager((PlayerIndex)0, ScreenManager.moveList.LongestMoveLength);


            isAtk1 = false;
            isAtk2 = false;
            isAtk3 = false;
            isRun = false;
            isStanding = false;
            isRapidStrikes = false;
            isDoubleStrike = false;





        }

        public void setAnimationPlayers()
        {

            masterPlayer = new AnimationPlayer(skinningData);
            masterClip = skinningData.AnimationClips["Take 001"];
            masterPlayer.StartClip(masterClip);





            rdy = new Matrix[skinningData.BindPose.Count];
            atk1a = new Matrix[skinningData.BindPose.Count];
            atk1b = new Matrix[skinningData.BindPose.Count];
            atk2a = new Matrix[skinningData.BindPose.Count];
            atk2b = new Matrix[skinningData.BindPose.Count];
            atk3a = new Matrix[skinningData.BindPose.Count];
            atk3b = new Matrix[skinningData.BindPose.Count];
            atk3c = new Matrix[skinningData.BindPose.Count];
            atk3d = new Matrix[skinningData.BindPose.Count];
            runA = new Matrix[skinningData.BindPose.Count];
            run1a = new Matrix[skinningData.BindPose.Count];
            run1b = new Matrix[skinningData.BindPose.Count];
            run2a = new Matrix[skinningData.BindPose.Count];
            run2b = new Matrix[skinningData.BindPose.Count];
            standing = new Matrix[skinningData.BindPose.Count];
            previousAnimation = new Matrix[skinningData.BindPose.Count];
            worldTrans = new Matrix[skinningData.BindPose.Count];
            skinTrans = new Matrix[skinningData.BindPose.Count];
            justBones = new Matrix[skinningData.BindPose.Count];
            legBones = new Matrix[skinningData.BindPose.Count];
            upperBones = new Matrix[skinningData.BindPose.Count];

            masterPlayer.Update(TimeSpan.FromMilliseconds(500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(rdy, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(1000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(atk1a, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(1500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(atk1b, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(2000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(atk2a, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(2500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(atk2b, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(3000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(atk3a, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(3500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(atk3b, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(4000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(atk3c, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(4500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(atk3d, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(5000), false, Matrix.Identity);

            ///5500 is standing again

            //masterPlayer.GetBoneTransforms().CopyTo(run1a, 0);
            //masterPlayer.Update(TimeSpan.FromMilliseconds(6000), false, Matrix.Identity);
            //masterPlayer.GetBoneTransforms().CopyTo(run1b, 0);
            //masterPlayer.Update(TimeSpan.FromMilliseconds(6500), false, Matrix.Identity);
            //masterPlayer.GetBoneTransforms().CopyTo(run2a, 0);
            //masterPlayer.Update(TimeSpan.FromMilliseconds(6999), false, Matrix.Identity);
            //masterPlayer.GetBoneTransforms().CopyTo(run2b, 0);

            masterPlayer.GetBoneTransforms().CopyTo(run1a, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(5500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(run1b, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(6000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(run2a, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(6499), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(run2b, 0);




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

        public void UpdateCrete(GameTime gameTime)
        {


            float rotationAmount = 0.0f;
            if (ScreenManager.flags.Count > 0)
            {
                rotationAmount = TurnToFace(position, ScreenManager.flags[ScreenManager.flags.Count - 1], new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));
                if (Vector3.Distance(position, ScreenManager.flags[ScreenManager.flags.Count - 1]) > 50)
                    thrustAmount = 1.0f;
                else
                    thrustAmount = 0.0f;
            }



            float x = (float)Math.Sin(rotationAmount);
            float y = (float)Math.Cos(rotationAmount);
            Direction = new Vector3(x, 0.0f, y);

            Up = Vector3.Up;
            Right = Vector3.Cross(Direction, Up);


            



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
            //if (isRun)
            //    upperBones.CopyTo(justBones, 0);
            //else if (isAtk3)
            //    upperBones.CopyTo(justBones, 0);
            //else if (isAtk1)
            //    upperBones.CopyTo(justBones, 0);
            //else
                rdy.CopyTo(justBones, 0);



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
            position.Y = ScreenManager.heightData[pX, pZ];
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
