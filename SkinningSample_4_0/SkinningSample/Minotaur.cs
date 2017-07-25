using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;


namespace SmellOfRevenge2011
{
    public class Minotaur
    {
        public bool isKnockBack = false;
        public List<boundingSphere> bSpheres; 
        public Vector3 slashTrans = Vector3.Zero;
        public TimeSpan currentAnimationTime = TimeSpan.Zero;
        public TimeSpan runTime = TimeSpan.Zero;
        public float rotAmt = 0.0f;
        public float thrustAmount = 0.0f;
        public Matrix[] previousAnimation;
        public Matrix[] standing, rdy, atk1a, atk1b, atk2a, atk2b, atk3a, atk3b, atk3c, atk3d, rdy2, runA, run1a, run1b, run2a, run2b;
        List<Move> availableMoves;
        Move playerMove;
        TimeSpan playerMoveTime;

        public Vector3 rWep1 = Vector3.Zero;

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
        public Minotaur(Vector3 pos, Vector3 dir)
        {
            Matrix[] transforms = new Matrix[ScreenManager.minoSphere.Bones.Count];
            ScreenManager.minoSphere.CopyAbsoluteBoneTransformsTo(transforms);

            bSpheres = new List<boundingSphere>();
            foreach (ModelMesh mesh in ScreenManager.minoSphere.Meshes)
            {
                transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);
                bSpheres.Add(new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X))); 
                
            }
            position = pos;
            direction = dir;
            Up = Vector3.Up;
            right = Vector3.Right;

           // dir = Vector3.Backward;
            
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
        public void UpdateCharacterSelect(GameTime gameTime)
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
                        isAtk3 = true;

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
                //  if (Math.Abs(rotationAmount.X) > Math.Abs(rotationAmount.Y))
                //    thrustAmount = rotationAmount.X;
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
                if (runTime.TotalSeconds < 2.0f)
                {

                    //        if (runTime.TotalSeconds < 1.0f)
                    //        {

                    //            for (int i = 0; i < rdy.Length; i++)
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
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            run1a[i].Decompose(out scale, out rota, out trans);
                            run1b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            run2a[i].Decompose(out scale, out rota, out trans);
                            run2b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            run2a[i].Decompose(out scale, out rota, out trans);
                            run2b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            run2b[i].Decompose(out scale, out rota, out trans);
                            run1a[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }
                    else
                    {
                        runTime = TimeSpan.Zero;
                        run2b.CopyTo(previousAnimation, 0);



                    }




                }
                else
                {

                    isRun = false;
                    runTime = TimeSpan.Zero;



                }

            }
            currentAnimationTime += new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long)3);

            if (isAtk3)
            {


                if (currentAnimationTime.TotalSeconds < 4.0f)
                {

                    if (currentAnimationTime.TotalSeconds < 1.0f)
                    {

                        for (int i = 0; i < rdy.Length; i++)
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
                        for (int i = 0; i < rdy.Length; i++)
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
                        for (int i = 0; i < rdy.Length; i++)
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
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            atk3c[i].Decompose(out scale, out rota, out trans);
                            atk3d[i].Decompose(out scale2, out rota2, out trans2);
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
                if (currentAnimationTime.TotalSeconds < 3.0f)
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
                else if (!isDoubleStrike)
                {

                    isAtk1 = false;
                    currentAnimationTime = TimeSpan.Zero;



                }

                else if (currentAnimationTime.TotalSeconds < 4.0f)
                {
                    for (int i = 0; i < rdy.Length; i++)
                    {
                        atk1b[i].Decompose(out scale, out rota, out trans);
                        atk2a[i].Decompose(out scale2, out rota2, out trans2);
                        upperBones[i] = Matrix.CreateScale(scale2) *
        Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f)) *
        Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f));

                    }


                }
                else if (currentAnimationTime.TotalSeconds < 5.0f)
                {
                    for (int i = 0; i < rdy.Length; i++)
                    {
                        atk2a[i].Decompose(out scale, out rota, out trans);
                        atk2b[i].Decompose(out scale2, out rota2, out trans2);
                        upperBones[i] = Matrix.CreateScale(scale2) *
        Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 4.0) / 1.0f)) *
        Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 4.0) / 1.0f));

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
                rdy.CopyTo(justBones, 0);
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

                    if(!isDoubleStrike)
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

                        if(!isAtk1)
                        currentAnimationTime = TimeSpan.Zero;


                      //  justBones.CopyTo(previousAnimation, 0);

                    }




                }
                if (playerMove.Name == "X")
                {
                    if(!isAtk3)
                    isAtk3 = true;

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
                //  if (Math.Abs(rotationAmount.X) > Math.Abs(rotationAmount.Y))
                //    thrustAmount = rotationAmount.X;
                //  else
                //  thrustAmount = rotationAmount.Y;
            }
            //else
            //    thrustAmount = 0.0f;

            Up.Normalize();
            Direction.Normalize();


            Right = Vector3.Cross(Direction, Up);
            if(isRun)
            {
                runTime += new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long)3.0);
                if (runTime.TotalSeconds < 2.0f)
                {

            //        if (runTime.TotalSeconds < 1.0f)
            //        {

            //            for (int i = 0; i < rdy.Length; i++)
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
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            run1a[i].Decompose(out scale, out rota, out trans);
                            run1b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            run2a[i].Decompose(out scale, out rota, out trans);
                            run2b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            run2a[i].Decompose(out scale, out rota, out trans);
                            run2b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            run2b[i].Decompose(out scale, out rota, out trans);
                            run1a[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }
                    else
                    {
                        runTime = TimeSpan.Zero;
                        run2b.CopyTo(previousAnimation, 0);



                    }




                }
                else
                {

                    isRun = false;
                    runTime = TimeSpan.Zero;



                }

            }
            currentAnimationTime += new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long)3);

            if (isAtk3)
            {


                if (currentAnimationTime.TotalSeconds < 4.0f)
                {

                    if (currentAnimationTime.TotalSeconds < 1.0f)
                    {

                        for (int i = 0; i < rdy.Length; i++)
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
                        for (int i = 0; i < rdy.Length; i++)
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
                        for (int i = 0; i < rdy.Length; i++)
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
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            atk3c[i].Decompose(out scale, out rota, out trans);
                            atk3d[i].Decompose(out scale2, out rota2, out trans2);
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
                if (currentAnimationTime.TotalSeconds < 3.0f)
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
                else if(!isDoubleStrike)
                {

                    isAtk1 = false;
                    currentAnimationTime = TimeSpan.Zero;



                }
            
                else if (currentAnimationTime.TotalSeconds < 4.0f)
                {
                    for (int i = 0; i < rdy.Length; i++)
                    {
                        atk1b[i].Decompose(out scale, out rota, out trans);
                        atk2a[i].Decompose(out scale2, out rota2, out trans2);
                        upperBones[i] = Matrix.CreateScale(scale2) *
        Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f)) *
        Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f));

                    }


                }
                else if (currentAnimationTime.TotalSeconds < 5.0f)
                {
                    for (int i = 0; i < rdy.Length; i++)
                    {
                        atk2a[i].Decompose(out scale, out rota, out trans);
                        atk2b[i].Decompose(out scale2, out rota2, out trans2);
                        upperBones[i] = Matrix.CreateScale(scale2) *
        Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 4.0) / 1.0f)) *
        Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 4.0) / 1.0f));

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
                rdy.CopyTo(justBones, 0);
            if (ScreenManager.medusa.Position.X < position.X)
                world.Forward = Vector3.Right;
            else
                world.Forward = Vector3.Left;

           // world.Forward = new Vector3(Direction.X, Direction.Y, Direction.Z);
            world.Up =Vector3.Up;
            world.Right = Vector3.Cross(world.Forward, world.Up);
            world.Translation = Position;

            // world.Translation = new Vector3(0.0f, 0.0f, 0.0f);
            UpdateWorldTransforms(Matrix.Identity);
            UpdateSkinTransforms();

           // position = new Vector3(-100.0f, 0.0f, 0.0f);



        }

        public void UpdateBrace(GameTime gameTime)
    {

        rdy.CopyTo(justBones, 0);
        //rdy.CopyTo(previousAnimation, 0);
        world.Up = Vector3.Up;
        world.Forward = Direction;
        world.Right = Vector3.Cross(world.Forward, world.Up);
        world.Translation = Position;


        UpdateWorldTransforms(Matrix.Identity);
        UpdateSkinTransforms();

       // formation = world;

    }

        public void UpdateTheseusStandAsterion(GameTime gameTime)
        {


            float dToT = Vector3.Distance(position, ScreenManager.TheseusTS.World.Translation);
            float rotationAmount = 0.0f;
            rotationAmount = TurnToFace(position, ScreenManager.TheseusTS.World.Translation, new Vector3(0.0f, (float)Math.Atan((double)(Direction.Z / Direction.X)), 0.0f));

            if (dToT < 300.0f)//if it can be seen
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
                        if (!isAtk1)
                        {
                            isAtk1 = true;
                            justBones.CopyTo(previousAnimation, 0);
                            currentAnimationTime = TimeSpan.Zero;

                            EternalStruggle.LSs.Add(new EternalStruggle.RageHit(TimeSpan.FromSeconds(3.0)));
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
                if (runTime.TotalSeconds < 2.0f)
                {

                    //        if (runTime.TotalSeconds < 1.0f)
                    //        {

                    //            for (int i = 0; i < rdy.Length; i++)
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
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            run1a[i].Decompose(out scale, out rota, out trans);
                            run1b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            run2a[i].Decompose(out scale, out rota, out trans);
                            run2b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            run2a[i].Decompose(out scale, out rota, out trans);
                            run2b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            run2b[i].Decompose(out scale, out rota, out trans);
                            run1a[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }
                    else
                    {
                        runTime = TimeSpan.Zero;
                        run2b.CopyTo(previousAnimation, 0);



                    }




                }
                else
                {

                    isRun = false;
                    runTime = TimeSpan.Zero;



                }

            }

            currentAnimationTime += new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long)3);
            foreach(EternalStruggle.RageHit rageHit in EternalStruggle.LSs)
            {
                rageHit.update(gameTime);
            }
            if (isAtk3)
            {


                if (currentAnimationTime.TotalSeconds < 4.0f)
                {

                    if (currentAnimationTime.TotalSeconds < 1.0f)
                    {

                        for (int i = 0; i < rdy.Length; i++)
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
                        for (int i = 0; i < rdy.Length; i++)
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
                        for (int i = 0; i < rdy.Length; i++)
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
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            atk3c[i].Decompose(out scale, out rota, out trans);
                            atk3d[i].Decompose(out scale2, out rota2, out trans2);
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
                if (currentAnimationTime.TotalSeconds < 3.0f)
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
                else if (!isDoubleStrike)
                {

                    isAtk1 = false;
                    currentAnimationTime = TimeSpan.Zero;



                }

                else if (currentAnimationTime.TotalSeconds < 4.0f)
                {
                    for (int i = 0; i < rdy.Length; i++)
                    {
                        atk1b[i].Decompose(out scale, out rota, out trans);
                        atk2a[i].Decompose(out scale2, out rota2, out trans2);
                        upperBones[i] = Matrix.CreateScale(scale2) *
        Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f)) *
        Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f));

                    }


                }
                else if (currentAnimationTime.TotalSeconds < 5.0f)
                {
                    for (int i = 0; i < rdy.Length; i++)
                    {
                        atk2a[i].Decompose(out scale, out rota, out trans);
                        atk2b[i].Decompose(out scale2, out rota2, out trans2);
                        upperBones[i] = Matrix.CreateScale(scale2) *
        Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 4.0) / 1.0f)) *
        Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 4.0) / 1.0f));

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

            
            Position += new Vector3(Direction.X, Direction.Y, Direction.Z) * thrustAmount * 20.0f;

            if (isKnockBack)
            {
                Position += new Vector3(-Direction.X, Direction.Y, -Direction.Z) * 10.0f;
                Position -= new Vector3(Direction.X, Direction.Y, Direction.Z) * thrustAmount * 20.0f;
            }

            if (!EternalStruggle.TheseusStandRun2)
            {
                if (isRun)
                    upperBones.CopyTo(justBones, 0);
                else if (isAtk3)
                    upperBones.CopyTo(justBones, 0);
                else if (isAtk1)
                    upperBones.CopyTo(justBones, 0);
                else
                    rdy.CopyTo(justBones, 0);
            }


            // world.Forward = new Vector3(Direction.X, Direction.Y, Direction.Z);
            world = Matrix.Identity;
            world.Forward = new Vector3(-Direction.X, Direction.Y, -Direction.Z);
            world.Up = up;
            world.Right = right;
            //position.Y = ScreenManager.heightData[pX, pZ];
            world.Translation = Position;



            // world.Translation = new Vector3(0.0f, 0.0f, 0.0f);
            UpdateWorldTransforms(Matrix.Identity);
            UpdateSkinTransforms();

            // position = new Vector3(-100.0f, 0.0f, 0.0f);



        }
        public void UpdateCrete(GameTime gameTime)
        {
            bool addFlag = false;
            bool addTower = false;
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

                if (playerMove.Name == "Start")
                {
                    ScreenManager.addDialogue = true;

                }
                    
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
                        isAtk3 = true;

                    justBones.CopyTo(previousAnimation, 0);


                    ScreenManager.gateOpen = true;



                }
                if (playerMove.Name == "LT")
                {

                  

                }
                if (playerMove.Name == "RT")
                {

                    addTower = true;
                }

            }

            if ((rotationAmount.X) > .3f)
            {

                Direction  = Vector3.Transform(Direction, Matrix.CreateFromAxisAngle(Vector3.Up, -.03f));
            }
            if ((rotationAmount.X) < -.3f)
            {
                Direction = Vector3.Transform(Direction, Matrix.CreateFromAxisAngle(Vector3.Up, .03f));

            }
            thrustAmount = -currentGamePadState.ThumbSticks.Left.Y;

            //if (Math.Abs(rotationAmount.X) > .3f || Math.Abs(rotationAmount.Y) > .3f)
            //{
            //    Direction = new Vector3(rotationAmount.X, 0.0f, rotationAmount.Y);
            //    if (rotationAmount.X > rotationAmount.Y)
            //        thrustAmount = rotationAmount.X;
            //    else if (rotationAmount.Y > rotationAmount.X)
            //        thrustAmount = rotationAmount.Y;

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
                //  if (Math.Abs(rotationAmount.X) > Math.Abs(rotationAmount.Y))
                //    thrustAmount = rotationAmount.X;
                //  else
                //  thrustAmount = rotationAmount.Y;

            //else
            //    thrustAmount = 0.0f;

            Up.Normalize();
            Direction.Normalize();


            right = Vector3.Cross(Direction, Up);
            if (isRun)
            {
                runTime += new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long)3.0);
                if (runTime.TotalSeconds < 2.0f)
                {

                    //        if (runTime.TotalSeconds < 1.0f)
                    //        {

                    //            for (int i = 0; i < rdy.Length; i++)
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
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            run1a[i].Decompose(out scale, out rota, out trans);
                            run1b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 2.0f)
                    {
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            run2a[i].Decompose(out scale, out rota, out trans);
                            run2b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 1.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 1.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 3.0f)
                    {
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            run2a[i].Decompose(out scale, out rota, out trans);
                            run2b[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 2.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 2.0) / 1.0f));

                        }


                    }
                    else if (runTime.TotalSeconds < 4.0f)
                    {
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            run2b[i].Decompose(out scale, out rota, out trans);
                            run1a[i].Decompose(out scale2, out rota2, out trans2);
                            upperBones[i] = Matrix.CreateScale(scale2) *
            Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(runTime.TotalSeconds - 3.0) / 1.0f)) *
            Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(runTime.TotalSeconds - 3.0) / 1.0f));

                        }


                    }
                    else
                    {
                        runTime = TimeSpan.Zero;
                        run2b.CopyTo(previousAnimation, 0);



                    }




                }
                else
                {

                    isRun = false;
                    runTime = TimeSpan.Zero;



                }

            }
            currentAnimationTime += new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long)3);

            if (isAtk3)
            {


                if (currentAnimationTime.TotalSeconds < 4.0f)
                {

                    if (currentAnimationTime.TotalSeconds < 1.0f)
                    {

                        for (int i = 0; i < rdy.Length; i++)
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
                        for (int i = 0; i < rdy.Length; i++)
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
                        for (int i = 0; i < rdy.Length; i++)
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
                        for (int i = 0; i < rdy.Length; i++)
                        {
                            atk3c[i].Decompose(out scale, out rota, out trans);
                            atk3d[i].Decompose(out scale2, out rota2, out trans2);
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
                if (currentAnimationTime.TotalSeconds < 3.0f)
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
                else if (!isDoubleStrike)
                {

                    isAtk1 = false;
                    currentAnimationTime = TimeSpan.Zero;



                }

                else if (currentAnimationTime.TotalSeconds < 4.0f)
                {
                    for (int i = 0; i < rdy.Length; i++)
                    {
                        atk1b[i].Decompose(out scale, out rota, out trans);
                        atk2a[i].Decompose(out scale2, out rota2, out trans2);
                        upperBones[i] = Matrix.CreateScale(scale2) *
        Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f)) *
        Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 3.0) / 1.0f));

                    }


                }
                else if (currentAnimationTime.TotalSeconds < 5.0f)
                {
                    for (int i = 0; i < rdy.Length; i++)
                    {
                        atk2a[i].Decompose(out scale, out rota, out trans);
                        atk2b[i].Decompose(out scale2, out rota2, out trans2);
                        upperBones[i] = Matrix.CreateScale(scale2) *
        Matrix.CreateFromQuaternion(Quaternion.Slerp(rota, rota2, (float)(currentAnimationTime.TotalSeconds - 4.0) / 1.0f)) *
        Matrix.CreateTranslation(Vector3.Lerp(trans, trans2, (float)(currentAnimationTime.TotalSeconds - 4.0) / 1.0f));

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

            
            Position += new Vector3(Direction.X, Direction.Y, Direction.Z) * thrustAmount * 20.0f;

            if (isRun)
                upperBones.CopyTo(justBones, 0);
            else if (isAtk3)
                upperBones.CopyTo(justBones, 0);
            else if (isAtk1)
                upperBones.CopyTo(justBones, 0);
            else
                rdy.CopyTo(justBones, 0);

           

            // world.Forward = new Vector3(Direction.X, Direction.Y, Direction.Z);
            world = Matrix.Identity; 
            world.Forward = new Vector3(Direction.X, Direction.Y, Direction.Z);
            world.Up = up;
            world.Right = right;
            //position.Y = ScreenManager.heightData[pX, pZ];
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
