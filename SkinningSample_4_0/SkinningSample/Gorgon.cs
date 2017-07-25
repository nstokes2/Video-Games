using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;


namespace SmellOfRevenge2011
{
    public class Gorgon 
    {
        public TimeSpan currentAnimationTime = TimeSpan.Zero;
        public float rotAmt = 0.0f;
        public float thrustAmount = 0.0f;
        public Matrix[] standing, rdy, atk1a, atk1b, previousAnimation;
        List<Move> availableMoves;
        Move playerMove;
        TimeSpan playerMoveTime;


        public bool isAtk1, isStanding;

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
        public Gorgon(Vector3 pos, Vector3 dir)
           
        {
            position = pos;
            direction = dir;
            Up = Vector3.Up;
            right = Vector3.Right;

            
            inputManager = new InputManager((PlayerIndex)0, ScreenManager.moveList.LongestMoveLength);

            isAtk1 = false;
            isStanding = false;



        }

        public void setAnimationPlayers()
        {

            masterPlayer = new AnimationPlayer(skinningData);
            masterClip = skinningData.AnimationClips["Take 001"];
            masterPlayer.StartClip(masterClip);

            rdy = new Matrix[skinningData.BindPose.Count];
            atk1a = new Matrix[skinningData.BindPose.Count];
            atk1b = new Matrix[skinningData.BindPose.Count];
            standing = new Matrix[skinningData.BindPose.Count];
            previousAnimation = new Matrix[skinningData.BindPose.Count];
            worldTrans = new Matrix[skinningData.BindPose.Count];
            skinTrans = new Matrix[skinningData.BindPose.Count];
            justBones = new Matrix[skinningData.BindPose.Count];
            legBones = new Matrix[skinningData.BindPose.Count];
            upperBones = new Matrix[skinningData.BindPose.Count];

            masterPlayer.Update(TimeSpan.FromMilliseconds(500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(standing, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(1000), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(rdy, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(1500), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(atk1a, 0);
            masterPlayer.Update(TimeSpan.FromMilliseconds(1999), false, Matrix.Identity);
            masterPlayer.GetBoneTransforms().CopyTo(atk1b, 0);




        }
        public void UpdateCrete(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            oldGamePadState = currentGamePadState;
            oldkeyBoardState = currentKeyBoardState;
            World.Decompose(out scale, out rota, out trans);
            oldPosition = Position;
            oldDirection = Direction;
            currentGamePadState = GamePad.GetState(PlayerIndex.One);



        }
        public void UpdateParisTS(GameTime gameTime)
        {





            rotAmt = TurnToFace(position, ScreenManager.TheseusTS.Position, Vector3.Zero);
            Direction = new Vector3((float)Math.Sin(rotAmt), 0.0f, (float)Math.Cos(rotAmt));

            Up.Normalize();
            Direction.Normalize();


            Right = Vector3.Cross(Direction, Up);

          


            standing.CopyTo(justBones, 0);

            world.Forward = new Vector3(-Direction.X, Direction.Y, -Direction.Z);
            world.Up = Vector3.Up;
            world.Right = Vector3.Cross(world.Forward, world.Up);
            world.Translation = Position;
            
          

            // world.Translation = new Vector3(0.0f, 0.0f, 0.0f);
            UpdateWorldTransforms(Matrix.Identity);
            UpdateSkinTransforms();





        }
        public void Update(GameTime gameTime)
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


            rotAmt = TurnToFace(ScreenManager.medusa.Position, ScreenManager.michael.Position, Vector3.Zero);
            Direction = new Vector3((float)Math.Sin(rotAmt), 0.0f, (float)Math.Cos(rotAmt));
            

            //if (gameTime.TotalGameTime - playerMoveTime > MoveTimeOut)
            //{
            //    playerMove = null;
            //}
            //inputManager.Update(gameTime);
            //Move newMove = ScreenManager.moveList.DetectMove(inputManager);

            //if (newMove != null)
            //{
            //    playerMove = newMove;

            //    playerMoveTime = gameTime.TotalGameTime;

            //    if (playerMove.Name == "A")
            //    {
            //        if (!isAtk1)
            //        {
            //            isAtk1 = true;
            //            currentAnimationTime = TimeSpan.Zero;
            //            justBones.CopyTo(previousAnimation, 0);
            //        }

            //    }
                
            //}
            if (Math.Abs(rotationAmount.X) > .3f || Math.Abs(rotationAmount.Y) > .3f)
            {
              //  Direction = new Vector3(rotationAmount.X, 0.0f, rotationAmount.Y);
                thrustAmount = 1.0f;
                //  if (Math.Abs(rotationAmount.X) > Math.Abs(rotationAmount.Y))
                //    thrustAmount = rotationAmount.X;
                //  else
                //  thrustAmount = rotationAmount.Y;
            }
            else
                thrustAmount = 0.0f;

            Up.Normalize();
            Direction.Normalize();


            Right = Vector3.Cross(Direction, Up);

            currentAnimationTime += gameTime.ElapsedGameTime;
            if (isAtk1)
            {
                if (currentAnimationTime.TotalSeconds > 3.0f)
                {

                    if (currentAnimationTime.TotalSeconds < 1.0f)
                    {

                        for (int i = 0; i < rdy.Length; i++)
                        {
                            previousAnimation[i].Decompose(out scale, out rota, out trans);
                            rdy[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds) / 1.0f));

                        }

                    }
                    else if (currentAnimationTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            rdy[i].Decompose(out scale, out rota, out trans);
                            atk1a[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (currentAnimationTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            atk1a[i].Decompose(out scale, out rota, out trans);
                            atk1b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }




                }
                else
                {

                    isAtk1 = false;
                    currentAnimationTime = TimeSpan.Zero;



                }

            }


            standing.CopyTo(justBones, 0);
            if (ScreenManager.michael.Position.X < position.X)
                world.Forward = Vector3.Right;
            else
                world.Forward = Vector3.Left;

            //world.Forward = new Vector3(-Direction.X, Direction.Y, -Direction.Z);
            world.Up = Vector3.Up;
            world.Right = Vector3.Cross(world.Forward, world.Up);
            world.Translation = new Vector3(100.0f, 0.0f, 0.0f);
            position = new Vector3(100.0f, 0.0f, 0.0f);

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

    }
}
