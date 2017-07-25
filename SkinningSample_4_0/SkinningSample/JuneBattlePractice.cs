using System;
using System.Collections.Generic;
using System.Text;
using SkinnedModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SmellOfRevenge2011
{
    class JuneBattlePractice : GameScreen
    {
        int battleX = 0;
        int battleZ = 0;

        public JuneBattlePractice(ScreenManager screenManager)
        {
            ScreenManager = screenManager;

        }

        public void freeBoxes(Vector3 vec)
        {
            int battleX = (int)vec.X / 70;
            int battleZ = (int)vec.Z / 70;

            if (battleX < 0)
                battleX = 0;
            if (battleZ < 0)
                battleZ = 0;
            ScreenManager.bigOpen[battleX][battleZ] = true;



        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                       bool coveredByOtherScreen)
        {


            ScreenManager.p1June.UpdateParis(gameTime);
            ScreenManager.eJune.UpdateTheseus(gameTime);
            checkCollisions();
            //checkOtherCollisions();

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);



            //freeBoxes(ScreenManager.p1Spear.World.Translation);
            //ScreenManager.p1Spear.UpdateBattle(gameTime);

            battleX = (int)ScreenManager.p1Spear.World.Translation.X / 60;
            battleZ = (int)ScreenManager.p1Spear.World.Translation.Z / 60;




            if (battleX > 5)
                battleX -= 5;
            else
                battleX = 0;
            if (battleZ > 5)
                battleZ -= 5;
            else
                battleZ = 0;
            //if (battleX < 5)
            //    battleX = 5 - battleX;
            //if(battleZ < 5)
            //    battleZ = 5 - battleZ;


            for (int i = 0; i < 11; i++)
                for (int j = 0; j < 11; j++)
                {

                    ScreenManager.openBoxes[i][j] = new BoundingBox(new Vector3((i + battleX) * 60, 5.0f, (j + battleZ) * 60), new Vector3((i + 1 + battleX) * 60, 50.0f, (j + 1 + battleZ) * 60));


                }

            //ScreenManager.eSpearLeader.UpdateLeader(gameTime);


            //foreach (SpearManAI spearer in ScreenManager.eSpears)
            //{
            //    freeBoxes(spearer.World.Translation);
            //    if (spearer.type == 0)
            //        spearer.UpdateLBattle(gameTime);
            //    else
            //        spearer.UpdateFBattle(gameTime);

            //}

            int formIndex = 0;
            //float ndist = Vector3.Distance(ScreenManager.eSpears[0].CFormation[4], ScreenManager.p1Spear.NFormation[4]);
            //float sdist = Vector3.Distance(ScreenManager.eSpears[0].CFormation[4], ScreenManager.p1Spear.SFormation[4]);
            //float wdist = Vector3.Distance(ScreenManager.eSpears[0].CFormation[4], ScreenManager.p1Spear.WFormation[4]);
            //float edist = Vector3.Distance(ScreenManager.eSpears[0].CFormation[4], ScreenManager.p1Spear.EFormation[4]);


            //if (ndist < sdist && ndist < wdist && ndist < edist)
            //{
            //    ScreenManager.eSpears[0].formVec = ScreenManager.p1Spear.NFormation[4];
            //    Console.WriteLine("N");
            //}
            //if (sdist < ndist && sdist < wdist && sdist < edist)
            //{
            //    ScreenManager.eSpears[0].formVec = ScreenManager.p1Spear.SFormation[4];
            //    Console.WriteLine("S");
            //}
            //if (wdist < ndist && wdist < sdist && wdist < edist)
            //    ScreenManager.eSpears[0].formVec = ScreenManager.p1Spear.WFormation[4];
            //if (edist < ndist && edist < wdist && edist < sdist)
            //    ScreenManager.eSpears[0].formVec = ScreenManager.p1Spear.EFormation[4];

            //if(formIndex
            //screenManager.eSpears[0].formVec = 


            //for (int i = 1; i < ScreenManager.eSpears.Count; i++)
            //{

            //    ScreenManager.eSpears[i].formVec = ScreenManager.eSpears[0].CFormation[i - 1];
            //    ScreenManager.eSpears[i].battleVec = ScreenManager.NFormation[i - 1];



            //}

            //ScreenManager.eSpears[0].UpdateLeader(gameTime);
            //ScreenManager.eSpears[1].UpdateFormation(gameTime);



            //foreach (SpearManAI spearer in ScreenManager.spears)
            //{
            //    spearer.UpdateFormation(gameTime);


            //}

            // ScreenManager.open[(int)ScreenManager.p1Spear.World.Translation.X / 100][(int)ScreenManager.p1Spear.World.Translation.Z / 100] = false;

        }

        public void DrawHud()
        {
            Vector3 projectedVec = Vector3.Zero;
            ScreenManager.SpriteBatch.Begin();

            projectedVec = ScreenManager.GraphicsDevice.Viewport.Project(ScreenManager.p1Spear.World.Translation, ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);

            ScreenManager.SpriteBatch.DrawString(screenManager.Font, ScreenManager.p1Spear.health.ToString(), new Vector2(projectedVec.X, projectedVec.Y + 20), Color.Yellow);





            ScreenManager.SpriteBatch.End();





        }

        void CreateShadowMap()
        {
            // Set our render target to our floating point render target
            ScreenManager.GraphicsDevice.SetRenderTarget(ScreenManager.shadowRenderTarget);

            // Clear the render target to white or all 1's
            // We set the clear to white since that represents the 
            // furthest the object could be away
            ScreenManager.GraphicsDevice.Clear(Color.White);
            Matrix[] transforms = new Matrix[ScreenManager.spearMan.Bones.Count];
            ScreenManager.spearMan.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.spearMan.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.CurrentTechnique = effect.Techniques["CreateShadowMap"];
                    effect.Parameters["Bones"].SetValue(ScreenManager.p1Spear.SkinTrans);
                    effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                    effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);


                }
                //if (draw)
                mesh.Draw();
            }

            // Set render target back to the back buffer
            ScreenManager.GraphicsDevice.SetRenderTarget(null);
        }

        public void DrawEnemySpeare(SpearManAI spearer)
        {

            Matrix world = new Matrix();
            Vector3 scale, trans;
            Quaternion rota;
            Matrix[] transforms;
            transforms = new Matrix[ScreenManager.spearMan.Bones.Count];
            ScreenManager.spearMan.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.spearMan.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.CurrentTechnique = effect.Techniques["SkinnedEffect"];
                    effect.Parameters["DiffuseColor"].SetValue(Vector4.One);
                    effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));
                    effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
                    effect.Parameters["DirLight0Direction"].SetValue(new Vector3(-0.5265408f, -0.5735765f, -0.6275069f));
                    effect.Parameters["DirLight0DiffuseColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
                    effect.Parameters["DirLight0SpecularColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
                    effect.Parameters["World"].SetValue(Matrix.Identity);
                    effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera.View).Translation);
                    if (mesh.Name == "Alecto")
                        effect.Parameters["Texture"].SetValue(ScreenManager.humanTex);
                    else
                        effect.Parameters["Texture"].SetValue(ScreenManager.satin);
                    effect.Parameters["Bones"].SetValue(spearer.SkinTrans);
                    effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);

                }

                mesh.Draw();
            }

            if (spearer.type == 0)
            {

                spearer.formation.Decompose(out scale, out rota, out trans);

                transforms = new Matrix[ScreenManager.humanFormation.Bones.Count];
                ScreenManager.humanFormation.CopyAbsoluteBoneTransformsTo(transforms);


                foreach (ModelMesh mesh in screenManager.humanFormation.Meshes)
                {
                    world = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);

                    if (mesh.Name == "0,0")
                        spearer.CFormation[0] = world.Translation;
                    if (mesh.Name == "0,1")
                        spearer.CFormation[3] = world.Translation;
                    if (mesh.Name == "0,2")
                        spearer.CFormation[6] = world.Translation;
                    if (mesh.Name == "1,0")
                        spearer.CFormation[1] = world.Translation;
                    if (mesh.Name == "1,1")
                        spearer.CFormation[4] = world.Translation;
                    if (mesh.Name == "1,2")
                        spearer.CFormation[7] = world.Translation;
                    if (mesh.Name == "2,0")
                        spearer.CFormation[2] = world.Translation;
                    if (mesh.Name == "2,1")
                        spearer.CFormation[5] = world.Translation;
                    if (mesh.Name == "2,2")
                        spearer.CFormation[8] = world.Translation;

                }

            }


            Matrix targetMat = new Matrix();
            transforms = new Matrix[ScreenManager.spearSphere.Bones.Count];
            ScreenManager.spearSphere.CopyAbsoluteBoneTransformsTo(transforms);
            int i = 0;
            int j = 0;
            foreach (ModelMesh mesh in ScreenManager.spearSphere.Meshes)
            {
                // draw = false;
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (mesh.Name == "rSpearS1" || mesh.Name == "rSpearS2" || mesh.Name == "rSpearS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * ScreenManager.p1Spear.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        spearer.spearSpheres[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "forwardSpell")
                    {
                        spearer.World.Decompose(out scale, out rota, out trans);

                        spearer.forwardSpell = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);




                    }
                    if (mesh.Name == "headS1")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * ScreenManager.p1Spear.SkinTrans[ScreenManager.head];
                        targetMat.Decompose(out scale, out rota, out trans);
                        spearer.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));


                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * spearer.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        spearer.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "hipS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * spearer.SkinTrans[ScreenManager.hips];
                        targetMat.Decompose(out scale, out rota, out trans);
                        spearer.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * spearer.SkinTrans[ScreenManager.lULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        spearer.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * spearer.SkinTrans[ScreenManager.lLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        spearer.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * spearer.SkinTrans[ScreenManager.rULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        spearer.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * spearer.SkinTrans[ScreenManager.rLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        spearer.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }





                }

            }
            //for (i = 0; i < 7; i++)
            //    BoundingSphereRenderer.Render(spearer.spheres[i].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);
            //for(j  = 0; j<3; j++)
            //    BoundingSphereRenderer.Render(spearer.spearSpheres[j].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);

        }
        public void checkOtherCollisions()
        {
            bool contact = false;

            if (!contact)
                foreach (boundingSphere bs in ScreenManager.eJune.spheres)
                    if (bs.Name == "chestS" & !contact)
                        foreach (boundingSphere bs2 in ScreenManager.p1June.roundShield)
                        {
                            if (bs2.BS.Intersects(bs.BS))
                            {
                                contact = true;

                                float rotationAmount = 0.0f;
                                rotationAmount = TurnToFace(ScreenManager.eJune.World.Translation, bs2.BS.Center, new Vector3(0.0f, (float)Math.Atan((double)(ScreenManager.p1June.World.Forward.Z / ScreenManager.p1June.World.Forward.X)), 0.0f));
                                ScreenManager.eJune.knockBackVec = new Vector3((float)Math.Sin(rotationAmount), 0.0f, (float)Math.Cos(rotationAmount));
                                ScreenManager.eJune.isKnockedBack = true;
                            }

                        }



        }
        public void checkCollisions()
        {
            bool contact = false;

            if(!contact)
            foreach(boundingSphere bs in ScreenManager.p1June.spheres)
                if(bs.Name == "chestS" &!contact)
                foreach(boundingSphere bs2 in ScreenManager.eJune.roundShield)
            {
                if (bs2.BS.Intersects(bs.BS))
                {
                    contact = true;

                    float rotationAmount = 0.0f;
                    rotationAmount = TurnToFace(ScreenManager.p1June.World.Translation, ScreenManager.eJune.World.Translation, new Vector3(0.0f, (float)Math.Atan((double)(ScreenManager.eJune.World.Forward.Z / ScreenManager.eJune.World.Forward.X)), 0.0f));
                    ScreenManager.p1June.knockBackVec = new Vector3((float)Math.Sin(rotationAmount), 0.0f, -(float)Math.Cos(rotationAmount));
                    ScreenManager.p1June.isKnockedBack = true;
                }

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
        public void DrawEnemySpear(JuneAIModel player)
        {
            Matrix[] transforms;

            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.CurrentTechnique = effect.Techniques["SkinnedEffect"];

                    effect.Parameters["DiffuseColor"].SetValue(new Vector4(2.0f, 2.0f, 2.0f, 1.0f));
                    effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));


                    effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
                    effect.Parameters["DirLight0Direction"].SetValue(new Vector3(-0.5265408f, -0.5735765f, -0.6275069f));
                    effect.Parameters["DirLight0DiffuseColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
                    effect.Parameters["DirLight0SpecularColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
                    effect.Parameters["World"].SetValue(Matrix.Identity);
                    effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera.View).Translation);
                    if (mesh.Name == "Alecto")
                        effect.Parameters["Texture"].SetValue(ScreenManager.humanTex);
                    else
                        effect.Parameters["Texture"].SetValue(ScreenManager.gold1);
                    effect.Parameters["Bones"].SetValue(player.SkinTrans);
                    effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);

                }

                mesh.Draw();
            }


            Matrix world = new Matrix();
            Vector3 scale, trans;
            Quaternion rota;
            player.formation.Decompose(out scale, out rota, out trans);

            transforms = new Matrix[ScreenManager.humanFormation.Bones.Count];
            ScreenManager.humanFormation.CopyAbsoluteBoneTransformsTo(transforms);


            foreach (ModelMesh mesh in screenManager.humanFormation.Meshes)
            {
                world = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);

                if (mesh.Name == "0,0")
                    player.CFormation[0] = world.Translation;
                if (mesh.Name == "0,1")
                    player.CFormation[3] = world.Translation;
                if (mesh.Name == "0,2")
                    player.CFormation[6] = world.Translation;
                if (mesh.Name == "1,0")
                    player.CFormation[1] = world.Translation;
                if (mesh.Name == "1,1")
                    player.CFormation[4] = world.Translation;
                if (mesh.Name == "1,2")
                    player.CFormation[7] = world.Translation;
                if (mesh.Name == "2,0")
                    player.CFormation[2] = world.Translation;
                if (mesh.Name == "2,1")
                    player.CFormation[5] = world.Translation;
                if (mesh.Name == "2,2")
                    player.CFormation[8] = world.Translation;
                if (mesh.Name == "E0,0")
                    player.EFormation[0] = world.Translation;
                if (mesh.Name == "E0,1")
                    player.EFormation[3] = world.Translation;
                if (mesh.Name == "E0,2")
                    player.EFormation[6] = world.Translation;
                if (mesh.Name == "E1,0")
                    player.EFormation[1] = world.Translation;
                if (mesh.Name == "E1,1")
                    player.EFormation[4] = world.Translation;
                if (mesh.Name == "E1,2")
                    player.EFormation[7] = world.Translation;
                if (mesh.Name == "E2,0")
                    player.EFormation[2] = world.Translation;
                if (mesh.Name == "E2,1")
                    player.EFormation[5] = world.Translation;
                if (mesh.Name == "E2,2")
                    player.EFormation[8] = world.Translation;
                if (mesh.Name == "W0,0")
                    player.WFormation[0] = world.Translation;
                if (mesh.Name == "W0,1")
                    player.WFormation[3] = world.Translation;
                if (mesh.Name == "W0,2")
                    player.WFormation[6] = world.Translation;
                if (mesh.Name == "W1,0")
                    player.WFormation[1] = world.Translation;
                if (mesh.Name == "W1,1")
                    player.WFormation[4] = world.Translation;
                if (mesh.Name == "W1,2")
                    player.WFormation[7] = world.Translation;
                if (mesh.Name == "W2,0")
                    player.WFormation[2] = world.Translation;
                if (mesh.Name == "W2,1")
                    player.WFormation[5] = world.Translation;
                if (mesh.Name == "W2,2")
                    player.WFormation[8] = world.Translation;
                if (mesh.Name == "N0,0")
                    player.NFormation[0] = world.Translation;
                if (mesh.Name == "N0,1")
                    player.NFormation[3] = world.Translation;
                if (mesh.Name == "N0,2")
                    player.NFormation[6] = world.Translation;
                if (mesh.Name == "N1,0")
                    player.NFormation[1] = world.Translation;
                if (mesh.Name == "N1,1")
                    player.NFormation[4] = world.Translation;
                if (mesh.Name == "N1,2")
                    player.NFormation[7] = world.Translation;
                if (mesh.Name == "N2,0")
                    player.NFormation[2] = world.Translation;
                if (mesh.Name == "N2,1")
                    player.NFormation[5] = world.Translation;
                if (mesh.Name == "N2,2")
                    player.NFormation[8] = world.Translation;

                if (mesh.Name == "S0,0")
                    player.SFormation[0] = world.Translation;
                if (mesh.Name == "S0,1")
                    player.SFormation[3] = world.Translation;
                if (mesh.Name == "S0,2")
                    player.SFormation[6] = world.Translation;
                if (mesh.Name == "S1,0")
                    player.SFormation[1] = world.Translation;
                if (mesh.Name == "S1,1")
                    player.SFormation[4] = world.Translation;
                if (mesh.Name == "S1,2")
                    player.SFormation[7] = world.Translation;
                if (mesh.Name == "S2,0")
                    player.SFormation[2] = world.Translation;
                if (mesh.Name == "S2,1")
                    player.SFormation[5] = world.Translation;
                if (mesh.Name == "S2,2")
                    player.SFormation[8] = world.Translation;

                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = ScreenManager.camera.View;
                    effect.Projection = ScreenManager.camera.Projection;

                    if (mesh.Name == "S0,0" || mesh.Name == "S0,1" || mesh.Name == "S0,2"
                    || mesh.Name == "S1,0" || mesh.Name == "S1,1" || mesh.Name == "S1,2"
                        || mesh.Name == "S2,0" || mesh.Name == "S2,1" || mesh.Name == "S2,2")

                        effect.DiffuseColor = Color.WhiteSmoke.ToVector3();


                    if (mesh.Name == "N0,0" || mesh.Name == "N0,1" || mesh.Name == "N0,2"
                    || mesh.Name == "N1,0" || mesh.Name == "N1,1" || mesh.Name == "N1,2"
                        || mesh.Name == "N2,0" || mesh.Name == "N2,1" || mesh.Name == "N2,2")
                        effect.DiffuseColor = Color.Yellow.ToVector3();


                    if (mesh.Name == "E0,0" || mesh.Name == "E0,1" || mesh.Name == "E0,2"
|| mesh.Name == "E1,0" || mesh.Name == "E1,1" || mesh.Name == "E1,2"
|| mesh.Name == "E2,0" || mesh.Name == "E2,1" || mesh.Name == "E2,2")
                        effect.DiffuseColor = Color.Fuchsia.ToVector3();

                    if (mesh.Name == "W0,0" || mesh.Name == "W0,1" || mesh.Name == "W0,2"
|| mesh.Name == "W1,0" || mesh.Name == "W1,1" || mesh.Name == "W1,2"
|| mesh.Name == "W2,0" || mesh.Name == "W2,1" || mesh.Name == "W2,2")
                        effect.DiffuseColor = Color.Firebrick.ToVector3();

                }

                mesh.Draw();
            }

            if (ScreenManager.battle)
            {
                foreach (ModelMesh mesh in screenManager.humanFormation.Meshes)
                {
                    world = Matrix.Transform(transforms[mesh.ParentBone.Index], ScreenManager.battleRota) * Matrix.CreateTranslation(ScreenManager.battleTrans);



                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.DiffuseColor = Color.Black.ToVector3();
                        effect.World = world;
                        effect.View = ScreenManager.camera.View;
                        effect.Projection = ScreenManager.camera.Projection;


                    }

                    mesh.Draw();
                }




            }


            Matrix targetMat = new Matrix();
            transforms = new Matrix[ScreenManager.spearSphere.Bones.Count];
            ScreenManager.spearSphere.CopyAbsoluteBoneTransformsTo(transforms);
            int i = 0;
            int j = 0;
            int rtAxeIndex = 0;
            int ltAxeIndex = 0;
            int rSpearIndex = 0;
            int lSpearIndex = 0;
            int rSwordIndex = 0;
            int lSwordIndex = 0;
            int axeIndex = 0;
            int bowIndex = 0;
            int arrowIndex = 0;
            int roundShieldIndex = 0;
            int bodyIndex = 0;

            foreach (ModelMesh mesh in ScreenManager.spearSphere.Meshes)
            {
                // draw = false;
                foreach (BasicEffect effect in mesh.Effects)
                {

                    //if (mesh.Name == "forwardSpell")
                    //    p1June.spellSpheres.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    //if (mesh.Name == "rSpearS1" || mesh.Name == "rSpearS2" || mesh.Name == "rSpearS3")
                    //    p1June.rSpear.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    //if (mesh.Name == "lSpearS1" || mesh.Name == "lSpearS2" || mesh.Name == "lSpearS3")
                    //    p1June.lSpear.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    //if (mesh.Name == "shieldS1" || mesh.Name == "shieldS2" || mesh.Name == "shieldS3" || mesh.Name == "shieldS4" || mesh.Name == "shieldS5")
                    //    p1June.roundShield.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    //if (mesh.Name == "axeS1" || mesh.Name == "axeS2" || mesh.Name == "axeS3")
                    //    p1June.axe.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    //if (mesh.Name == "rSwordS1" || mesh.Name == "rSwordS2" || mesh.Name == "rSwordS3")
                    //    p1June.rSword.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));

                    //if (mesh.Name == "lSwordS1" || mesh.Name == "lSwordS2" || mesh.Name == "lSwordS3")
                    //    p1June.lSword.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    //if (mesh.Name == "lTAxeS1")
                    //    p1June.lTAxe.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));

                    //if (mesh.Name == "rTAxeS1")
                    //    p1June.rTAxe.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    //if (mesh.Name == "bowS1" || mesh.Name == "bowS2" || mesh.Name == "bowS3" || mesh.Name == "bowS4" || mesh.Name == "bowS5" || mesh.Name == "bowS6")
                    //    p1June.bow.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    //if (mesh.Name == "arrowS1" || mesh.Name == "arrowS2" || mesh.Name == "arrowS3")
                    //    p1June.arrow.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    //else
                    //    p1June.spheres.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    if (mesh.Name == "rTAxeS1")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.rTAxe[rtAxeIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "lTAxeS1")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.lTAxe[ltAxeIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "bowS1" || mesh.Name == "bowS2" || mesh.Name == "bowS3" || mesh.Name == "bowS4" || mesh.Name == "bowS5" || mesh.Name == "bowS6")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.bow[bowIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "shieldS1" || mesh.Name == "shieldS2" || mesh.Name == "shieldS3" || mesh.Name == "shieldS4" || mesh.Name == "shieldS5")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.roundShield[roundShieldIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "rSwordS1" || mesh.Name == "rSwordS2" || mesh.Name == "rSwordS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.rSword[rSwordIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "lSwordS1" || mesh.Name == "lSwordS2" || mesh.Name == "lSwordS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.lSword[lSwordIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "arrowS1" || mesh.Name == "arrowS2" || mesh.Name == "arrowS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.arrow[arrowIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "axeS1" || mesh.Name == "axeS2" || mesh.Name == "axeS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.axe[axeIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }




                    if (mesh.Name == "rSpearS1" || mesh.Name == "rSpearS2" || mesh.Name == "rSpearS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.rSpear[rSpearIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }

                    if (mesh.Name == "lSpearS1" || mesh.Name == "lSpearS2" || mesh.Name == "lSpearS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.lSpear[lSpearIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "forwardSpell")
                    {
                        player.World.Decompose(out scale, out rota, out trans);

                        player.forwardSpell = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);




                    }
                    if (mesh.Name == "headS1")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.head];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));


                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "hipS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.hips];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }





                }

            }
            for (int a = 0; a < i; a++)
                BoundingSphereRenderer.Render(player.spheres[a].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);
            for (j = 0; j < 3; j++)
                BoundingSphereRenderer.Render(player.lSpear[j].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);
            for(j = 0; j<player.roundShield.Count; j++)
                BoundingSphereRenderer.Render(player.roundShield[j].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);



        }

        public void DrawPlayerSpear(JuneXnaModel player)
        {
            Matrix[] transforms;

            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.CurrentTechnique = effect.Techniques["SkinnedEffect"];

                    effect.Parameters["DiffuseColor"].SetValue(new Vector4(2.0f, 2.0f, 2.0f, 1.0f));
                    effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));


                    effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
                    effect.Parameters["DirLight0Direction"].SetValue(new Vector3(-0.5265408f, -0.5735765f, -0.6275069f));
                    effect.Parameters["DirLight0DiffuseColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
                    effect.Parameters["DirLight0SpecularColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
                    effect.Parameters["World"].SetValue(Matrix.Identity);
                    effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera.View).Translation);
                    if (mesh.Name == "Alecto")
                        effect.Parameters["Texture"].SetValue(ScreenManager.humanTex);
                    else
                        effect.Parameters["Texture"].SetValue(ScreenManager.gold1);
                    effect.Parameters["Bones"].SetValue(player.SkinTrans);
                    effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);

                }

                mesh.Draw();
            }


            Matrix world = new Matrix();
            Vector3 scale, trans;
            Quaternion rota;

            #region formation
            player.formation.Decompose(out scale, out rota, out trans);

            transforms = new Matrix[ScreenManager.humanFormation.Bones.Count];
            ScreenManager.humanFormation.CopyAbsoluteBoneTransformsTo(transforms);


            foreach (ModelMesh mesh in screenManager.humanFormation.Meshes)
            {
                world = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);

                if (mesh.Name == "0,0")
                    player.CFormation[0] = world.Translation;
                if (mesh.Name == "0,1")
                    player.CFormation[3] = world.Translation;
                if (mesh.Name == "0,2")
                    player.CFormation[6] = world.Translation;
                if (mesh.Name == "1,0")
                    player.CFormation[1] = world.Translation;
                if (mesh.Name == "1,1")
                    player.CFormation[4] = world.Translation;
                if (mesh.Name == "1,2")
                    player.CFormation[7] = world.Translation;
                if (mesh.Name == "2,0")
                    player.CFormation[2] = world.Translation;
                if (mesh.Name == "2,1")
                    player.CFormation[5] = world.Translation;
                if (mesh.Name == "2,2")
                    player.CFormation[8] = world.Translation;
                if (mesh.Name == "E0,0")
                    player.EFormation[0] = world.Translation;
                if (mesh.Name == "E0,1")
                    player.EFormation[3] = world.Translation;
                if (mesh.Name == "E0,2")
                    player.EFormation[6] = world.Translation;
                if (mesh.Name == "E1,0")
                    player.EFormation[1] = world.Translation;
                if (mesh.Name == "E1,1")
                    player.EFormation[4] = world.Translation;
                if (mesh.Name == "E1,2")
                    player.EFormation[7] = world.Translation;
                if (mesh.Name == "E2,0")
                    player.EFormation[2] = world.Translation;
                if (mesh.Name == "E2,1")
                    player.EFormation[5] = world.Translation;
                if (mesh.Name == "E2,2")
                    player.EFormation[8] = world.Translation;
                if (mesh.Name == "W0,0")
                    player.WFormation[0] = world.Translation;
                if (mesh.Name == "W0,1")
                    player.WFormation[3] = world.Translation;
                if (mesh.Name == "W0,2")
                    player.WFormation[6] = world.Translation;
                if (mesh.Name == "W1,0")
                    player.WFormation[1] = world.Translation;
                if (mesh.Name == "W1,1")
                    player.WFormation[4] = world.Translation;
                if (mesh.Name == "W1,2")
                    player.WFormation[7] = world.Translation;
                if (mesh.Name == "W2,0")
                    player.WFormation[2] = world.Translation;
                if (mesh.Name == "W2,1")
                    player.WFormation[5] = world.Translation;
                if (mesh.Name == "W2,2")
                    player.WFormation[8] = world.Translation;
                if (mesh.Name == "N0,0")
                    player.NFormation[0] = world.Translation;
                if (mesh.Name == "N0,1")
                    player.NFormation[3] = world.Translation;
                if (mesh.Name == "N0,2")
                    player.NFormation[6] = world.Translation;
                if (mesh.Name == "N1,0")
                    player.NFormation[1] = world.Translation;
                if (mesh.Name == "N1,1")
                    player.NFormation[4] = world.Translation;
                if (mesh.Name == "N1,2")
                    player.NFormation[7] = world.Translation;
                if (mesh.Name == "N2,0")
                    player.NFormation[2] = world.Translation;
                if (mesh.Name == "N2,1")
                    player.NFormation[5] = world.Translation;
                if (mesh.Name == "N2,2")
                    player.NFormation[8] = world.Translation;

                if (mesh.Name == "S0,0")
                    player.SFormation[0] = world.Translation;
                if (mesh.Name == "S0,1")
                    player.SFormation[3] = world.Translation;
                if (mesh.Name == "S0,2")
                    player.SFormation[6] = world.Translation;
                if (mesh.Name == "S1,0")
                    player.SFormation[1] = world.Translation;
                if (mesh.Name == "S1,1")
                    player.SFormation[4] = world.Translation;
                if (mesh.Name == "S1,2")
                    player.SFormation[7] = world.Translation;
                if (mesh.Name == "S2,0")
                    player.SFormation[2] = world.Translation;
                if (mesh.Name == "S2,1")
                    player.SFormation[5] = world.Translation;
                if (mesh.Name == "S2,2")
                    player.SFormation[8] = world.Translation;

                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = ScreenManager.camera.View;
                    effect.Projection = ScreenManager.camera.Projection;

                    if (mesh.Name == "S0,0" || mesh.Name == "S0,1" || mesh.Name == "S0,2"
                    || mesh.Name == "S1,0" || mesh.Name == "S1,1" || mesh.Name == "S1,2"
                        || mesh.Name == "S2,0" || mesh.Name == "S2,1" || mesh.Name == "S2,2")

                        effect.DiffuseColor = Color.WhiteSmoke.ToVector3();


                    if (mesh.Name == "N0,0" || mesh.Name == "N0,1" || mesh.Name == "N0,2"
                    || mesh.Name == "N1,0" || mesh.Name == "N1,1" || mesh.Name == "N1,2"
                        || mesh.Name == "N2,0" || mesh.Name == "N2,1" || mesh.Name == "N2,2")
                        effect.DiffuseColor = Color.Yellow.ToVector3();


                    if (mesh.Name == "E0,0" || mesh.Name == "E0,1" || mesh.Name == "E0,2"
|| mesh.Name == "E1,0" || mesh.Name == "E1,1" || mesh.Name == "E1,2"
|| mesh.Name == "E2,0" || mesh.Name == "E2,1" || mesh.Name == "E2,2")
                        effect.DiffuseColor = Color.Fuchsia.ToVector3();

                    if (mesh.Name == "W0,0" || mesh.Name == "W0,1" || mesh.Name == "W0,2"
|| mesh.Name == "W1,0" || mesh.Name == "W1,1" || mesh.Name == "W1,2"
|| mesh.Name == "W2,0" || mesh.Name == "W2,1" || mesh.Name == "W2,2")
                        effect.DiffuseColor = Color.Firebrick.ToVector3();

                }

               // mesh.Draw();
            }
            #endregion
            //draws the battle formation
            #region battle formation
            if (ScreenManager.battle)
            {
                foreach (ModelMesh mesh in screenManager.humanFormation.Meshes)
                {
                    world = Matrix.Transform(transforms[mesh.ParentBone.Index], ScreenManager.battleRota) * Matrix.CreateTranslation(ScreenManager.battleTrans);



                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.DiffuseColor = Color.Black.ToVector3();
                        effect.World = world;
                        effect.View = ScreenManager.camera.View;
                        effect.Projection = ScreenManager.camera.Projection;


                    }

                    mesh.Draw();
                }




            }
            #endregion

            Matrix targetMat = new Matrix();
            transforms = new Matrix[ScreenManager.spearSphere.Bones.Count];
            ScreenManager.spearSphere.CopyAbsoluteBoneTransformsTo(transforms);
            int i = 0;
            int j = 0;
            int rtAxeIndex = 0;
            int ltAxeIndex = 0;
            int rSpearIndex = 0;
            int lSpearIndex = 0;
            int rSwordIndex = 0;
            int lSwordIndex = 0;
            int axeIndex = 0;
            int bowIndex = 0;
            int arrowIndex = 0;
            int roundShieldIndex = 0;
            int bodyIndex = 0;


            transforms = new Matrix[ScreenManager.humanProjectiles.Bones.Count];
            ScreenManager.humanProjectiles.CopyAbsoluteBoneTransformsTo(transforms);

            ModelMesh arrowMM = ScreenManager.humanProjectiles.Meshes["Arrow"];
            foreach (ModelMesh mesh in ScreenManager.humanProjectiles.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    //Arrow RThrowingAxe LSpear LThrowingAxe forwardSpell roundShield

                    //if(mesh.Name == "Arrow")
                    //    player.arrowWorld = 

                     
                    if (mesh.Name == "Arrow")
                    {
                        player.arrowWorld = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        arrowMM = mesh;
                    }
                    if(mesh.Name == "RThrowingAxe")
                        player.rTAxeWorld = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                    if(mesh.Name == "LSpear")
                        player.spearWorld = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];

                    if (mesh.Name == "LThrowingAxe")
                        player.lTAxeWorld = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];

                    if (mesh.Name == "roundShield")
                        player.shieldWorld = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];

                }
            }


            foreach (Projectile2 pro in player.projectiles)
            {
                foreach (BasicEffect effect in arrowMM.Effects)
                {
                    effect.World = pro.world;
                    effect.View = ScreenManager.camera.View;
                    effect.Projection = ScreenManager.camera.Projection;



                }
                arrowMM.Draw();
            }

            #region bSpheres
            transforms = new Matrix[ScreenManager.spearSphere.Bones.Count];
            ScreenManager.spearSphere.CopyAbsoluteBoneTransformsTo(transforms);
                
            foreach (ModelMesh mesh in ScreenManager.spearSphere.Meshes)
            {
                // draw = false;
                foreach (BasicEffect effect in mesh.Effects)
                {

                    //if (mesh.Name == "forwardSpell")
                    //    p1June.spellSpheres.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    //if (mesh.Name == "rSpearS1" || mesh.Name == "rSpearS2" || mesh.Name == "rSpearS3")
                    //    p1June.rSpear.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    //if (mesh.Name == "lSpearS1" || mesh.Name == "lSpearS2" || mesh.Name == "lSpearS3")
                    //    p1June.lSpear.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    //if (mesh.Name == "shieldS1" || mesh.Name == "shieldS2" || mesh.Name == "shieldS3" || mesh.Name == "shieldS4" || mesh.Name == "shieldS5")
                    //    p1June.roundShield.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    //if (mesh.Name == "axeS1" || mesh.Name == "axeS2" || mesh.Name == "axeS3")
                    //    p1June.axe.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    //if (mesh.Name == "rSwordS1" || mesh.Name == "rSwordS2" || mesh.Name == "rSwordS3")
                    //    p1June.rSword.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));

                    //if (mesh.Name == "lSwordS1" || mesh.Name == "lSwordS2" || mesh.Name == "lSwordS3")
                    //    p1June.lSword.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    //if (mesh.Name == "lTAxeS1")
                    //    p1June.lTAxe.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));

                    //if (mesh.Name == "rTAxeS1")
                    //    p1June.rTAxe.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    //if (mesh.Name == "bowS1" || mesh.Name == "bowS2" || mesh.Name == "bowS3" || mesh.Name == "bowS4" || mesh.Name == "bowS5" || mesh.Name == "bowS6")
                    //    p1June.bow.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    //if (mesh.Name == "arrowS1" || mesh.Name == "arrowS2" || mesh.Name == "arrowS3")
                    //    p1June.arrow.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    //else
                    //    p1June.spheres.Add(new boundingSphere(mesh.Name, new BoundingSphere(mesh.BoundingSphere.Center, mesh.BoundingSphere.Radius * scale.X)));
                    if (mesh.Name == "rTAxeS1")
                    {
                        
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.rTAxe[rtAxeIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "lTAxeS1")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.lTAxe[ltAxeIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "bowS1" || mesh.Name == "bowS2" || mesh.Name == "bowS3" || mesh.Name == "bowS4" || mesh.Name == "bowS5" || mesh.Name == "bowS6")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.bow[bowIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "shieldS1" || mesh.Name == "shieldS2" || mesh.Name == "shieldS3" || mesh.Name == "shieldS4" || mesh.Name == "shieldS5")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.roundShield[roundShieldIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "rSwordS1" || mesh.Name == "rSwordS2" || mesh.Name == "rSwordS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.rSword[rSwordIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "lSwordS1" || mesh.Name == "lSwordS2" || mesh.Name == "lSwordS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.lSword[lSwordIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "arrowS1" || mesh.Name == "arrowS2" || mesh.Name == "arrowS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.arrow[arrowIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "axeS1" || mesh.Name == "axeS2" || mesh.Name == "axeS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.axe[axeIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }




                    if (mesh.Name == "rSpearS1" || mesh.Name == "rSpearS2" || mesh.Name == "rSpearS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.rSpear[rSpearIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }

                    if (mesh.Name == "lSpearS1" || mesh.Name == "lSpearS2" || mesh.Name == "lSpearS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.lSpear[lSpearIndex++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "forwardSpell")
                    {
                        player.World.Decompose(out scale, out rota, out trans);

                        player.forwardSpell = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);




                    }
                    if (mesh.Name == "headS1")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.head];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));


                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "hipS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.hips];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }





                }

            }
            for (int a = 0; a < i; a++)
                BoundingSphereRenderer.Render(player.spheres[a].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);
            for (j = 0; j < 3; j++)
                BoundingSphereRenderer.Render(player.lSpear[j].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);

            #endregion



        }

        public void DrawBoard()
        {
            Matrix[] transforms = new Matrix[ScreenManager.aosBoard.Bones.Count];
            ScreenManager.aosBoard.CopyAbsoluteBoneTransformsTo(transforms);
            ScreenManager.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
            bool draw = false;
            foreach (ModelMesh mesh in ScreenManager.aosBoard.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    if (mesh.Name == "tree")
                    {
                        draw = true;
                    }
                    else if (mesh.Name == "treeLeaves")
                    {
                        draw = true;
                    }
                    else if (mesh.Name == "parthenon")
                    {
                        draw = true;
                    }
                    //else if (mesh.Name == "boat")
                    //{
                    //    draw = true;
                    //}
                    //else if (mesh.Name == "boatPole")
                    //{
                    //    draw = true;
                    //}
                    //else if (mesh.Name == "boatSails")
                    //{
                    //    draw = true;
                    //}
                    else if (mesh.Name == "arena")
                    {
                        draw = true;
                    }
                    else if (mesh.Name == "pylons")
                    {
                        draw = true;
                    }
                    else if (mesh.Name == "Plane002")
                    {
                        draw = true;
                    }
                    else if (mesh.Name == "GeoSphere001")
                    {
                        draw = true;
                    }
                    else if (mesh.Name == "Box001")
                    {
                        draw = true;
                    }
                    else if (mesh.Name == "Box002")
                    {
                        draw = true;
                    }
                    else
                        draw = false;

                    // effect.Parameters["AmbientColor"].SetValue(new Vector4(.5f, .5f, .5f, 0f));
                    effect.Parameters["Texture"].SetValue(ScreenManager.white);
                    effect.CurrentTechnique = effect.Techniques["DrawWithShadowMap"];
                    effect.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index]);
                    effect.Parameters["View"].SetValue(ScreenManager.camera.View);
                    effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
                    effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                    effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);
                    //tree treeLeaves parthenon boat boatPole boatSails arena pylons Plane002 GeoSphere001 Box001 Box002 
                    //tombSphere parthenonSphere tombSphere caveSphere crucibleSphere theseusSphere
                    // if (!createShadowMap)
                    effect.Parameters["ShadowMap"].SetValue(ScreenManager.shadowRenderTarget);

                    //if(mesh.Name == 

                }
                if (draw)
                    mesh.Draw();
            }
            foreach (boundingSphere bs in ScreenManager.creteSpheres)
                BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Green);

        }
        void DrawBoxes()
        {

            Color boxColor = Color.DarkGreen;
            Color closedBox = Color.Red;
            ScreenManager.primitiveBatch.Begin(PrimitiveType.LineList);

            //for (int i = 0; i < 11; i++)
            //    for (int j = 0; j < 11; j++)
            //    {
            //        //if (ScreenManager.open[i][j] == false)
            //        {  

            //            //Front face
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

            //            //Back face
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);


            //            //Bottom face
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

            //            //Top face
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

            //            //Right
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

            //            //Left
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
            //            ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);


            //        }


            //    }

            for (int i = 0; i < 30; i++)
                for (int j = 0; j < 30; j++)
                {
                    //if (ScreenManager.open[i][j] == false)
                    {
                        if (ScreenManager.bigOpen[i][j] == true)
                            boxColor = Color.DarkGreen;
                        else
                            boxColor = Color.Red;

                        //Front face
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);

                        //Back face
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);


                        //Bottom face
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);

                        //Top face
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);

                        //Right
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Min.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);

                        //Left
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Min.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Max.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.bigOpenBoxes[i][j].Max.X, ScreenManager.bigOpenBoxes[i][j].Max.Y, ScreenManager.bigOpenBoxes[i][j].Min.Z), boxColor);


                    }


                }


            ScreenManager.primitiveBatch.End();



        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.lightViewProjection = ScreenManager.CreateLightViewProjectionMatrix();
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
            ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
            CreateShadowMap();
            ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            DrawPlayerSpear(ScreenManager.p1June);
            DrawEnemySpear(ScreenManager.eJune);
            //foreach (SpearManAI spearer in ScreenManager.eSpears)
            //    DrawEnemySpearer(spearer);
            DrawBoard();
            DrawBoxes();
            DrawHud();



            ScreenManager.fireParticles.SetCamera(ScreenManager.camera.View, ScreenManager.camera.Projection);
            base.Draw(gameTime);
        }





    }
}
