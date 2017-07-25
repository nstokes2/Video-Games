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
    public class FormationPractice : GameScreen
    {

        
        public FormationPractice(ScreenManager screenManager)
        {
            ScreenManager = screenManager;

        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                               bool coveredByOtherScreen)
        {

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
          //  ScreenManager.open[(int)ScreenManager.p1Spear.World.Translation.X / 100][(int)ScreenManager.p1Spear.World.Translation.Z / 100] = true;

            ScreenManager.open[1][0] = false;
            ScreenManager.open[2][0] = false;
            ScreenManager.open[1][1] = false;

            ScreenManager.p1Spear.UpdateCrete(gameTime);
           // ScreenManager.spears[0].UpdatePathing(gameTime);  //normally updateformation


            ScreenManager.eSpearLeader.UpdateLeader(gameTime);
            foreach (SpearManAI spearer in ScreenManager.eSpears)
                spearer.UpdateFormation(gameTime);
            ScreenManager.eSpears[0].UpdateLeader(gameTime);
            ScreenManager.eSpears[1].UpdateFormation(gameTime);

            foreach (SpearManAI spearer in ScreenManager.spears)
            {
                spearer.UpdateFormation(gameTime);


            }

           // ScreenManager.open[(int)ScreenManager.p1Spear.World.Translation.X / 100][(int)ScreenManager.p1Spear.World.Translation.Z / 100] = false;

        }


        void CreateShadowMap()
        {
            // Set our render target to our floating point render target
            ScreenManager.GraphicsDevice.SetRenderTarget(ScreenManager.shadowRenderTarget);

            // Clear the render target to white or all 1's
            // We set the clear to white since that represents the 
            // furthest the object could be away
            ScreenManager.GraphicsDevice.Clear(Color.White);
               Matrix[]  transforms = new Matrix[ScreenManager.spearMan.Bones.Count];
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


        public override void Draw(GameTime gameTime)
        {
            Vector3 scale, trans;
            Quaternion rota;

            ScreenManager.lightViewProjection = ScreenManager.CreateLightViewProjectionMatrix();

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
            //ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;

            ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;

            CreateShadowMap();

            bool draw = false;

            ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            //ScreenManager.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;

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
                    effect.Parameters["Texture"].SetValue(ScreenManager.gold1);
                    effect.Parameters["Bones"].SetValue(ScreenManager.p1Spear.SkinTrans);
                    effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);


                    



                }

                //if(mesh.Name == "Alecto")
                mesh.Draw();
            }

            foreach (SpearManAI spearer in ScreenManager.spears)
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
                        effect.Parameters["Texture"].SetValue(ScreenManager.satin);
                        effect.Parameters["Bones"].SetValue(spearer.SkinTrans);
                        effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);






                    }

                    mesh.Draw();
                }
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
                    effect.Parameters["Texture"].SetValue(ScreenManager.white);
                    effect.Parameters["Bones"].SetValue(ScreenManager.eSpearLeader.SkinTrans);
                    effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);






                }

                mesh.Draw();
            }

            foreach (SpearManAI spearer in ScreenManager.eSpears)
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
                        effect.Parameters["Texture"].SetValue(ScreenManager.white);
                        effect.Parameters["Bones"].SetValue(spearer.SkinTrans);
                        effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);






                    }

                    mesh.Draw();
                }



            //foreach(SpearManAI spearer in ScreenManager.eSpears)
            //foreach (ModelMesh mesh in ScreenManager.spearMan.Meshes)
            //{
            //    foreach (Effect effect in mesh.Effects)
            //    {
            //        effect.CurrentTechnique = effect.Techniques["SkinnedEffect"];

            //        effect.Parameters["DiffuseColor"].SetValue(Vector4.One);
            //        effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));

            //        effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
            //        effect.Parameters["DirLight0Direction"].SetValue(new Vector3(-0.5265408f, -0.5735765f, -0.6275069f));
            //        effect.Parameters["DirLight0DiffuseColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
            //        effect.Parameters["DirLight0SpecularColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
            //        effect.Parameters["World"].SetValue(Matrix.Identity);
            //        effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera.View).Translation);
            //        effect.Parameters["Texture"].SetValue(ScreenManager.white);
            //        effect.Parameters["Bones"].SetValue(spearer.SkinTrans);
            //        effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);






            //    }

            //    mesh.Draw();
            //}

            Matrix targetMat= new Matrix();
            transforms =new Matrix[ScreenManager.spearSphere.Bones.Count];
            ScreenManager.spearSphere.CopyAbsoluteBoneTransformsTo(transforms);
            int i = 0; 
            foreach (ModelMesh mesh in ScreenManager.spearSphere.Meshes)
            {
                draw = false;
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (mesh.Name == "forwardSpell")
                    {
                        ScreenManager.p1Spear.World.Decompose(out scale, out rota, out trans);

                        ScreenManager.p1Spear.forwardSpell  = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);




                    }
                    if (mesh.Name == "headS1")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * ScreenManager.p1Spear.SkinTrans[ScreenManager.head];
                        targetMat.Decompose(out scale, out rota, out trans);
                        ScreenManager.p1Spear.spheres[i++]= new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                        
                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * ScreenManager.p1Spear.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        ScreenManager.p1Spear.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "hipS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * ScreenManager.p1Spear.SkinTrans[ScreenManager.hips];
                        targetMat.Decompose(out scale, out rota, out trans);
                        ScreenManager.p1Spear.spheres[i++]= new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * ScreenManager.p1Spear.SkinTrans[ScreenManager.lULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        ScreenManager.p1Spear.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * ScreenManager.p1Spear.SkinTrans[ScreenManager.lLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        ScreenManager.p1Spear.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * ScreenManager.p1Spear.SkinTrans[ScreenManager.rULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        ScreenManager.p1Spear.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * ScreenManager.p1Spear.SkinTrans[ScreenManager.rLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        ScreenManager.p1Spear.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    




                }

            }
            for (i = 0; i < 7; i++)
                BoundingSphereRenderer.Render(ScreenManager.p1Spear.spheres[i].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);


            ScreenManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            foreach (Projectile2 pro in ScreenManager.p1Spear.projectiles)
            {
                foreach (ModelMesh mesh in ScreenManager.spearSphere.Meshes)
                {
                    if (mesh.Name == "forwardSpell")
                    {

                        foreach (BasicEffect effect in mesh.Effects)
                        {

                            effect.TextureEnabled = true;
                            effect.Texture = ScreenManager.fire;
                            effect.World = pro.world;
                            effect.View = ScreenManager.camera.View;
                            effect.Projection = ScreenManager.camera.Projection;
                            //effect.EnableDefaultLighting();
                            //effect.DiffuseColor = Vector3.One;
                            //effect.AmbientLightColor = Vector3.One;
                            //effect.EmissiveColor = Vector3.One;


                        }

                        ScreenManager.fireParticles.AddParticle(pro.world.Translation, Vector3.Up);
                        mesh.Draw();




                    }





                }


            }

            //ScreenManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;


            //ScreenManager.eSpears[0].pathFinder.Draw();

            //Enemy Spears

            //foreach(SpearManAI spearAI in ScreenManager.eSpears)
            //foreach (ModelMesh mesh in ScreenManager.spearMan.Meshes)
            //{
            //    foreach (Effect effect in mesh.Effects)
            //    {
            //        effect.CurrentTechnique = effect.Techniques["SkinnedEffect"];

            //        effect.Parameters["DiffuseColor"].SetValue(Vector4.One);
            //        effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));

            //        effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
            //        effect.Parameters["DirLight0Direction"].SetValue(new Vector3(-0.5265408f, -0.5735765f, -0.6275069f));
            //        effect.Parameters["DirLight0DiffuseColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
            //        effect.Parameters["DirLight0SpecularColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
            //        effect.Parameters["World"].SetValue(Matrix.Identity);
            //        effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera.View).Translation);
            //        effect.Parameters["Texture"].SetValue(ScreenManager.white);
            //        effect.Parameters["Bones"].SetValue(spearAI.SkinTrans);
            //        effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);






            //    }

            //    mesh.Draw();
            //}

            ScreenManager.p1Spear.formation.Decompose(out scale, out rota, out trans);
            transforms = new Matrix[ScreenManager.humanFormation.Bones.Count];
            ScreenManager.humanFormation.CopyAbsoluteBoneTransformsTo(transforms);
            Matrix world = new Matrix();
            int x = ScreenManager.x;
            int y = ScreenManager.y;
            Console.WriteLine(x + " " + y);
            foreach(ModelMesh mesh in ScreenManager.humanFormation.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    effect.View = ScreenManager.camera.View;
                    effect.Projection = ScreenManager.camera.Projection;
                    world = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans) ;
                    effect.World = world;

                    if ((mesh.Name == "0,0" && x % 3 == 1 && y % 4 == 1) ||
                        (mesh.Name == "0,1" && x % 3 == 1 && y % 4 == 2) ||
                        (mesh.Name == "0,2" && x % 3 == 1 && y % 4 == 3) ||
                        (mesh.Name == "1,0" && x % 3 == 2 && y % 4 == 1) ||
                        (mesh.Name == "1,1" && x % 3 == 2 && y % 4 == 2) ||
                        (mesh.Name == "1,2" && x % 3 == 2 && y % 4 == 3) ||
                        (mesh.Name == "2,0" && x % 3 == 0 && y % 4 == 1) ||
                        (mesh.Name == "2,1" && x % 3 == 0 && y % 4 == 2) ||
                        (mesh.Name == "2,2" && x % 3 == 0 && y % 4 == 3) ||
                        (mesh.Name == "1,3" && x % 3 == 2 && y % 4 == 0))
                    {

                        effect.DiffuseColor = Color.Black.ToVector3();

                        ScreenManager.spears[0].formVec = world.Translation;
                    }
                    else
                        effect.DiffuseColor = Color.Red.ToVector3();

                    if (mesh.Name == "0,0")
                    {

                        ScreenManager.p1Spear.spots[0] = world.Translation;
                        
                    }
                    if (mesh.Name == "0,1")
                    {
                        ScreenManager.p1Spear.spots[0] = world.Translation;
                        ScreenManager.spears[0].formVec = world.Translation;
                    }
                    if (mesh.Name == "0,2")
                    {
                        ScreenManager.p1Spear.spots[2] = world.Translation;

                    }
                    if (mesh.Name == "1,0")
                    {

                        ScreenManager.p1Spear.spots[3] = world.Translation;
                       
                    }
                    if (mesh.Name == "1,2")
                    {
                        ScreenManager.p1Spear.spots[4] = world.Translation;

                        ScreenManager.spears[2].formVec = world.Translation;
                    }
                    if (mesh.Name == "1,2")
                    {
                        ScreenManager.p1Spear.spots[5] = world.Translation;
                    }
                    if (mesh.Name == "2,0")
                    {
                        ScreenManager.p1Spear.spots[6] = world.Translation;
                    }
                    if (mesh.Name == "2,1")
                    {
                        ScreenManager.spears[1].formVec = world.Translation;
                        ScreenManager.p1Spear.spots[7] = world.Translation;
                    }
                    if (mesh.Name == "2,2")
                    {
                        ScreenManager.p1Spear.spots[8] = world.Translation;
                    }
                    if (mesh.Name == "1,3")
                    {
                        ScreenManager.p1Spear.spots[9] = world.Translation;

                        ScreenManager.spears[3].formVec = world.Translation;
                    }






                }
                mesh.Draw();
        }

            ScreenManager.eSpearLeader.formation.Decompose(out scale, out rota, out trans);
            transforms = new Matrix[ScreenManager.humanFormation.Bones.Count];
            ScreenManager.humanFormation.CopyAbsoluteBoneTransformsTo(transforms);
             world = new Matrix();
             x = ScreenManager.x;
             y = ScreenManager.y;
            Console.WriteLine(x + " " + y);
            foreach (ModelMesh mesh in ScreenManager.humanFormation.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    effect.View = ScreenManager.camera.View;
                    effect.Projection = ScreenManager.camera.Projection;
                    world = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);
                    effect.World = world;

                    if ((mesh.Name == "0,0" && x % 3 == 1 && y % 4 == 1) ||
                        (mesh.Name == "0,1" && x % 3 == 1 && y % 4 == 2) ||
                        (mesh.Name == "0,2" && x % 3 == 1 && y % 4 == 3) ||
                        (mesh.Name == "1,0" && x % 3 == 2 && y % 4 == 1) ||
                        (mesh.Name == "1,1" && x % 3 == 2 && y % 4 == 2) ||
                        (mesh.Name == "1,2" && x % 3 == 2 && y % 4 == 3) ||
                        (mesh.Name == "2,0" && x % 3 == 0 && y % 4 == 1) ||
                        (mesh.Name == "2,1" && x % 3 == 0 && y % 4 == 2) ||
                        (mesh.Name == "2,2" && x % 3 == 0 && y % 4 == 3) ||
                        (mesh.Name == "1,3" && x % 3 == 2 && y % 4 == 0))
                    {

                        effect.DiffuseColor = Color.Black.ToVector3();

                       // ScreenManager.eSpears[0].formVec = world.Translation;
                    }
                    else
                        effect.DiffuseColor = Color.Red.ToVector3();

                    if (mesh.Name == "0,0")
                    {

                        ScreenManager.eSpearLeader.spots[0] = world.Translation;

                    }
                    if (mesh.Name == "0,1")
                    {
                        ScreenManager.eSpearLeader.spots[1] = world.Translation;
                        ScreenManager.eSpears[0].formVec = world.Translation;
                    }
                    if (mesh.Name == "0,2")
                    {
                        ScreenManager.eSpearLeader.spots[2] = world.Translation;

                    }
                    if (mesh.Name == "1,0")
                    {

                        ScreenManager.eSpearLeader.spots[3] = world.Translation;

                    }
                    if (mesh.Name == "1,2")
                    {
                        ScreenManager.eSpearLeader.spots[4] = world.Translation;

                        ScreenManager.eSpears[2].formVec = world.Translation;
                    }
                    if (mesh.Name == "1,2")
                    {
                        ScreenManager.eSpearLeader.spots[5] = world.Translation;
                    }
                    if (mesh.Name == "2,0")
                    {
                        ScreenManager.eSpearLeader.spots[6] = world.Translation;
                    }
                    if (mesh.Name == "2,1")
                    {
                        ScreenManager.eSpears[1].formVec = world.Translation;
                        ScreenManager.eSpearLeader.spots[7] = world.Translation;
                    }
                    if (mesh.Name == "2,2")
                    {
                        ScreenManager.eSpearLeader.spots[8] = world.Translation;
                    }
                    if (mesh.Name == "1,3")
                    {
                        ScreenManager.eSpearLeader.spots[9] = world.Translation;

                        ScreenManager.eSpears[3].formVec = world.Translation;
                    }






                }
                mesh.Draw();
            }


            ScreenManager.eSpears[0].formation.Decompose(out scale, out rota, out trans);
            foreach (ModelMesh mesh in ScreenManager.humanFormation.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    effect.View = ScreenManager.camera.View;
                    effect.Projection = ScreenManager.camera.Projection;
                    world = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);
                    effect.World = world;

                    if (mesh.Name == "0,0")
                    {
                        ScreenManager.p1Spear.spots[0] = world.Translation;
                        ScreenManager.eSpears[1].formVec = world.Translation;
                    }
                }

            }


            transforms = new Matrix[ScreenManager.aosBoard.Bones.Count];
            ScreenManager.aosBoard.CopyAbsoluteBoneTransformsTo(transforms);
            ScreenManager.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
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








            DrawBoxes();

            ScreenManager.fireParticles.SetCamera(ScreenManager.camera.View, ScreenManager.camera.Projection);

            base.Draw(gameTime);
        }
        void DrawBoxes()
        {

            Color boxColor = Color.DarkGreen;
            ScreenManager.primitiveBatch.Begin(PrimitiveType.LineList);

            for(int i = 0; i<ScreenManager.openBoxes.Length; i++)
                for (int j = 0; j < ScreenManager.openBoxes.Length; j++)
                {
                    if (ScreenManager.open[i][j] == false)
                    {
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);


                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);



                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                    }


                }
            //foreach (BoundingBox[] theBoxes in ScreenManager.openBoxes)
            //    foreach (BoundingBox box in theBoxes)
            //    {
                    
            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Max.Z), Color.Red);
            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Max.Z), Color.Red);

            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Max.Z), Color.Red);
            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Max.Z), Color.Red);

            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Max.Z), Color.Red);
            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Max.Z), Color.Red);

            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Max.Z), Color.Red);
            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Max.Z), Color.Red);


            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), Color.Red);
            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Min.Z), Color.Red);

            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Min.Z), Color.Red);
            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Min.Z), Color.Red);

            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Min.Z), Color.Red);
            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Min.Z), Color.Red);

            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Min.Z), Color.Red);
            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), Color.Red);



            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), Color.Red);
            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Max.Z), Color.Red);

            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Min.Z), Color.Red);
            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Max.Z), Color.Red);

            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Min.Z), Color.Red);
            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Max.Z), Color.Red);

            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Min.Z), Color.Red);
            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Max.Z), Color.Red);

            //    }


            ScreenManager.primitiveBatch.End();



        }

        

        }



    }

