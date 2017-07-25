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
    public class ReturnToCrete : GameScreen
    {




        Vector3 scale, trans, trans2;
        Quaternion rota;
        int player = 0;
        public ReturnToCrete(ScreenManager screenManager)
        {
            ScreenManager = screenManager;

            // minotaurs = new List<Minotaur>();
            // gorgons = new List<Gorgon>();


            scale = Vector3.Zero;
            trans = Vector3.Zero;
            rota = Quaternion.Identity;

            trans2 = Vector3.Zero;

        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            screenManager.fireParticles.AddParticle(ScreenManager.p1Mino.World.Translation, Vector3.Up);
            //ScreenManager.medusa.Update(gameTime);
            //ScreenManager.asterion.UpdateCrete(gameTime);

            if (ScreenManager.player == 0)
                ScreenManager.p1Angel.UpdateCrete(gameTime);

            if (ScreenManager.player == 1)
                ScreenManager.p1Gorgon.UpdateCrete(gameTime);

            if (ScreenManager.player == 2)
                ScreenManager.p1Mino.UpdateCrete(gameTime);

            if (ScreenManager.player == 3)
                ScreenManager.p1Spear.UpdateCrete(gameTime);

            ScreenManager.perseusAI.UpdateCrete(gameTime);

            foreach (Tower tower in ScreenManager.Towers)
                tower.update(gameTime);

            foreach (MinotaurAI minoAi in ScreenManager.minotaurs)
                minoAi.UpdateCrete(gameTime);
            foreach (GorgonAI actorAi in ScreenManager.gorgons)
                actorAi.UpdateCrete(gameTime);

        }

        public static void RemapModel(Model model, Effect effect)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                }
            }

        }

        void CreateShadowMap()
        {
            // Set our render target to our floating point render target
            ScreenManager.GraphicsDevice.SetRenderTarget(ScreenManager.shadowRenderTarget);

            // Clear the render target to white or all 1's
            // We set the clear to white since that represents the 
            // furthest the object could be away
            ScreenManager.GraphicsDevice.Clear(Color.White);

            Matrix[] transforms;
            if (ScreenManager.player == 0)
            {
                transforms = new Matrix[ScreenManager.angel.Bones.Count];
                ScreenManager.angel.CopyAbsoluteBoneTransformsTo(transforms);
                foreach (ModelMesh mesh in ScreenManager.angel.Meshes)
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.CurrentTechnique = effect.Techniques["CreateShadowMap"];
                        effect.Parameters["Bones"].SetValue(ScreenManager.p1Angel.SkinTrans);
                        effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                        effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);


                    }
                    //if (draw)
                    mesh.Draw();
                }
            }
            if (ScreenManager.player == 1)
            {
                transforms = new Matrix[ScreenManager.gorgon.Bones.Count];
                ScreenManager.gorgon.CopyAbsoluteBoneTransformsTo(transforms);
                foreach (ModelMesh mesh in ScreenManager.gorgon.Meshes)
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.CurrentTechnique = effect.Techniques["CreateShadowMap"];
                        effect.Parameters["Bones"].SetValue(ScreenManager.p1Gorgon.SkinTrans);
                        effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                        effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);

                       
                    }
                    //if (draw)
                    mesh.Draw();
                }
            }
            else if (ScreenManager.player == 2)
            {
                transforms = new Matrix[ScreenManager.minotaur.Bones.Count];
                ScreenManager.minotaur.CopyAbsoluteBoneTransformsTo(transforms);
                foreach (ModelMesh mesh in ScreenManager.minotaur.Meshes)
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.CurrentTechnique = effect.Techniques["CreateShadowMap"];
                        //effect.Parameters["View"].SetValue(ScreenManager.camera.View);
                        effect.Parameters["Bones"].SetValue(ScreenManager.p1Mino.SkinTrans);
                        //effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
                        effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                        effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);

                        //  effect.Parameters["ShadowMap"].SetValue(ScreenManager.shadowRenderTarget);

                        //textureParam = Parameters["Texture"];
                        //diffuseColorParam = Parameters["DiffuseColor"];
                        //emissiveColorParam = Parameters["EmissiveColor"];
                        //specularColorParam = Parameters["SpecularColor"];
                        //specularPowerParam = Parameters["SpecularPower"];
                        //eyePositionParam = Parameters["EyePosition"];
                        //fogColorParam = Parameters["FogColor"];
                        //fogVectorParam = Parameters["FogVector"];
                        //worldParam = Parameters["World"];
                        //worldInverseTransposeParam = Parameters["WorldInverseTranspose"];
                        //worldViewProjParam = Parameters["WorldViewProj"];
                        //bonesParam = Parameters["Bones"];
                        //shaderIndexParam = Parameters["ShaderIndex"];

                        //effect.SetBoneTransforms(ScreenManager.asterion.SkinTrans);
                        //effect.View = ScreenManager.camera.View;
                        //effect.Projection = ScreenManager.camera.Projection;
                        // effect.DirectionalLight0 = new DirectionalLight(new EffectParameter(Vector3.Down), Color.Yellow, Color.White, null);
                        ////effect.EnableDefaultLighting();
                        ////effect.Parameters["DirLight0DiffuseColor"].SetValue(Color.Tomato.ToVector3());
                        ////effect.Parameters["DirLight0Direction"].SetValue(new Vector3(-.333f, .2f, .46f));
                        //effect.Parameters["DiffuseColor"].SetValue(Vector4.One);
                        //effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));

                        //effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
                        //effect.Parameters["DirLight0Direction"].SetValue(new Vector3(-0.5265408f, -0.5735765f, -0.6275069f));
                        //effect.Parameters["DirLight0DiffuseColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
                        //effect.Parameters["DirLight0SpecularColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
                        //effect.Parameters["World"].SetValue(Matrix.Identity);
                        //effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera.View).Translation);
                        //effect.Parameters["Texture"].SetValue(ScreenManager.white);
                        //effect.Parameters["Bones"].SetValue(ScreenManager.asterion.SkinTrans);
                        //effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);

                        //effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
                        //effect.Parameters["LightDirection"].SetValue(new Vector3(.03f, .05f, -1.0f));
                        //  effect.Texture = ScreenManager.white;

                        //effect.AmbientLightColor = Color.White.ToVector3();
                        //effect.DiffuseColor = Color.White.ToVector3();
                        ////effect.EmissiveColor = Color.Brown.ToVector3();

                        //effect.DirectionalLight0.Enabled = true;
                        //effect.DirectionalLight0.DiffuseColor = Color.Blue.ToVector3();
                        //effect.DirectionalLight0.Direction = new Vector3(0, -.7f, .5f);
                        //effect.DirectionalLight0.SpecularColor = Color.Yellow.ToVector3();


                        //effect.DirectionalLight1.Enabled = true;
                        //effect.DirectionalLight1.DiffuseColor = Color.White.ToVector3();
                        //effect.DirectionalLight1.Direction = new Vector3(-.8f, 0, .6f);
                        //effect.DirectionalLight1.SpecularColor = Color.White.ToVector3();

                        //effect.DirectionalLight2.Enabled = true;
                        //effect.DirectionalLight2.DiffuseColor = Color.Blue.ToVector3();
                        //effect.DirectionalLight2.Direction = Vector3.Right;
                        //effect.DirectionalLight2.SpecularColor = Color.Yellow.ToVector3();



                    }
                    //if (draw)
                    mesh.Draw();
                }
            }
            if (ScreenManager.player == 3)
            {
                transforms = new Matrix[ScreenManager.spearMan.Bones.Count];
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
            }

            // Draw any occluders in our case that is just the dude model

            // Set the models world matrix so it will rotate
            //world = Matrix.CreateRotationY(MathHelper.ToRadians(rotateDude));
            // Draw the dude model
            // DrawModel(dudeModel, true);

            // Set render target back to the back buffer
            ScreenManager.GraphicsDevice.SetRenderTarget(null);
        }
        void DrawBoxes()
        {


            ScreenManager.primitiveBatch.Begin(PrimitiveType.LineList);
            foreach(BoundingBox[] theBoxes in ScreenManager.openBoxes)
            foreach (BoundingBox box in theBoxes)
            {
                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Max.Z), Color.Red);
                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Max.Z), Color.Red);

                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Max.Z), Color.Red);
                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Max.Z), Color.Red);

                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Max.Z), Color.Red);
                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Max.Z), Color.Red);

                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Max.Z), Color.Red);
                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Max.Z), Color.Red);


                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), Color.Red);
                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Min.Z), Color.Red);

                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Min.Z), Color.Red);
                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Min.Z), Color.Red);

                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Min.Z), Color.Red);
                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Min.Z), Color.Red);

                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Min.Z), Color.Red);
                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), Color.Red);



                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), Color.Red);
                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Max.Z), Color.Red);

                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Min.Z), Color.Red);
                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Max.Z), Color.Red);

                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Min.Z), Color.Red);
                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Max.Z), Color.Red);

                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Min.Z), Color.Red);
                ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Max.Z), Color.Red);

            }


            ScreenManager.primitiveBatch.End();



        }
        void DrawModel(Model model, bool createShadowMap)
        {
            string techniqueName = createShadowMap ? "CreateShadowMap" : "DrawWithShadowMap";

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            // Loop over meshs in the model
            foreach (ModelMesh mesh in model.Meshes)
            {
                // Loop over effects in the mesh
                foreach (Effect effect in mesh.Effects)
                {
                    // Set the currest values for the effect
                    effect.CurrentTechnique = effect.Techniques[techniqueName];
                    effect.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index]);
                    effect.Parameters["View"].SetValue(ScreenManager.camera.View);
                    effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
                    effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                    effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);

                    effect.Parameters["ShadowMap"].SetValue(ScreenManager.shadowRenderTarget);
                }
                // Draw the mesh
                mesh.Draw();
            }
        }
        public override void Draw(GameTime gameTime)
        {

            ScreenManager.fireParticles.SetCamera(ScreenManager.camera.View, ScreenManager.camera.Projection);

            ScreenManager.slashParticles.SetCamera(ScreenManager.camera.View, ScreenManager.camera.Projection);
            ScreenManager.lightViewProjection = ScreenManager.CreateLightViewProjectionMatrix();

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                   Color.CornflowerBlue, 0, 0);
            //ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
             ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;

            //without this createshadowmap throws an error
            ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;


          



             CreateShadowMap();


            //ScreenManager.GraphicsDevice.
            bool draw = false;

            ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            Matrix[] transforms;

            //if (ScreenManager.player == 2)
            //{
            //    transforms = new Matrix[ScreenManager.minotaur.Bones.Count];
            //    ScreenManager.minotaur.CopyAbsoluteBoneTransformsTo(transforms);
            //    foreach (ModelMesh mesh in ScreenManager.minotaur.Meshes)
            //    {
            //        foreach (Effect effect in mesh.Effects)
            //        {
            //            effect.CurrentTechnique = effect.Techniques["SkinnedEffect"];

            //            effect.Parameters["DiffuseColor"].SetValue(Vector4.One);
            //            effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));

            //            effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
            //            effect.Parameters["DirLight0Direction"].SetValue(new Vector3(-0.5265408f, -0.5735765f, -0.6275069f));
            //            effect.Parameters["DirLight0DiffuseColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
            //            effect.Parameters["DirLight0SpecularColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
            //            effect.Parameters["World"].SetValue(Matrix.Identity);
            //            effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera.View).Translation);
            //            effect.Parameters["Texture"].SetValue(ScreenManager.white);
            //            effect.Parameters["Bones"].SetValue(ScreenManager.asterion.SkinTrans);
            //            effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);




            //        }
            //        //if (draw)
            //        mesh.Draw();
            //    }
            //}
            #region oldstuff

            // foreach (GorgonAI gorgAI in ScreenManager.gorgons)
            // {

            //     transforms = new Matrix[ScreenManager.gorgon.Bones.Count];
            //     ScreenManager.gorgon.CopyAbsoluteBoneTransformsTo(transforms);
            //     //foreach (ModelMesh mesh in ScreenManager.gorgon.Meshes)
            //     //{
            //     //    foreach (SkinnedEffect effect in mesh.Effects)
            //     //    {
            //     //        effect.SetBoneTransforms(gorgAI.SkinTrans);
            //     //        effect.View = ScreenManager.camera.View;
            //     //        effect.Projection = ScreenManager.camera.Projection;
            //     //        // effect.DirectionalLight0 = new DirectionalLight(new EffectParameter(Vector3.Down), Color.Yellow, Color.White, null);
            //     //        effect.EnableDefaultLighting();
            //     //    }
            //     //    mesh.Draw();
            //     //}


            // }
            // foreach (MinotaurAI minoAI in ScreenManager.minotaurs)
            // {



            //      transforms = new Matrix[ScreenManager.minotaur.Bones.Count];
            //     ScreenManager.minotaur.CopyAbsoluteBoneTransformsTo(transforms);
            //     foreach (ModelMesh mesh in ScreenManager.minotaur.Meshes)
            //     {
            //         //foreach (SkinnedEffect effect in mesh.Effects)
            //         //{
            //         //    effect.SetBoneTransforms(minoAI.SkinTrans);
            //         //    effect.View = ScreenManager.camera.View;
            //         //    effect.Projection = ScreenManager.camera.Projection;
            //         //    // effect.DirectionalLight0 = new DirectionalLight(new EffectParameter(Vector3.Down), Color.Yellow, Color.White, null);
            //         //    effect.EnableDefaultLighting();

            //         //    effect.Texture = ScreenManager.white;
            //         //    //  effect.Texture = ScreenManager.white;

            //         //    //effect.AmbientLightColor = Color.White.ToVector3();
            //         //    //effect.DiffuseColor = Color.White.ToVector3();
            //         //    ////effect.EmissiveColor = Color.Brown.ToVector3();

            //         //    //effect.DirectionalLight0.Enabled = true;
            //         //    //effect.DirectionalLight0.DiffuseColor = Color.Blue.ToVector3();
            //         //    //effect.DirectionalLight0.Direction = new Vector3(0, -.7f, .5f);
            //         //    //effect.DirectionalLight0.SpecularColor = Color.Yellow.ToVector3();


            //         //    //effect.DirectionalLight1.Enabled = true;
            //         //    //effect.DirectionalLight1.DiffuseColor = Color.White.ToVector3();
            //         //    //effect.DirectionalLight1.Direction = new Vector3(-.8f, 0, .6f);
            //         //    //effect.DirectionalLight1.SpecularColor = Color.White.ToVector3();

            //         //    //effect.DirectionalLight2.Enabled = true;
            //         //    //effect.DirectionalLight2.DiffuseColor = Color.Blue.ToVector3();
            //         //    //effect.DirectionalLight2.Direction = Vector3.Right;
            //         //    //effect.DirectionalLight2.SpecularColor = Color.Yellow.ToVector3();



            //         //}
            //         ////if (draw)
            //         //mesh.Draw();
            //     }
            // }

            // transforms = new Matrix[screenManager.tower.Bones.Count];
            // ScreenManager.tower.CopyAbsoluteBoneTransformsTo(transforms);


            // foreach (Tower tower in ScreenManager.Towers)
            // {
            //     foreach (ModelMesh mesh in ScreenManager.tower.Meshes)
            //     {
            //         foreach (BasicEffect effect in mesh.Effects)
            //         {

            //             transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);
            //             effect.View = ScreenManager.camera.View;
            //             effect.Projection = ScreenManager.camera.Projection;
            //             effect.World = Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(rota) * Matrix.CreateTranslation(tower.translation + trans);
            //             effect.EnableDefaultLighting();


            //         }

            //         if(mesh.Name == "tower")
            //         mesh.Draw();
            //     }
            //     foreach (Projectile towerProj in tower.projectiles)
            //     {
            //         foreach (ModelMesh mesh in ScreenManager.tower.Meshes)
            //         {
            //             foreach (BasicEffect effect in mesh.Effects)
            //             {
            //                 transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans2);
            //                 effect.View = ScreenManager.camera.View;
            //                 effect.Projection = ScreenManager.camera.Projection;
            //                 effect.World = Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(rota) * Matrix.CreateTranslation(towerProj.Translation);
            //                 effect.EnableDefaultLighting();


            //             }
            //             if (mesh.Name == "Arrow")
            //                 mesh.Draw();


            //         }
            //     }



            // }

            // transforms = new Matrix[screenManager.flag.Bones.Count];
            // ScreenManager.flag.CopyAbsoluteBoneTransformsTo(transforms);

            // foreach (ModelMesh mesh in ScreenManager.flag.Meshes)
            //     foreach (BasicEffect effect in mesh.Effects)
            //         transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);

            //// scale *= new Vector3(0.0f, 10.0f, 0.0f);
            // foreach (Vector3 vec in ScreenManager.flags)
            // {
            //     foreach (ModelMesh mesh in ScreenManager.flag.Meshes)
            //     {
            //         foreach (BasicEffect effect in mesh.Effects)
            //         {
            //             effect.View = ScreenManager.camera.View;
            //             effect.Projection = ScreenManager.camera.Projection;
            //             effect.World =  Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(rota)  * Matrix.CreateTranslation(vec + trans);

            //             effect.EnableDefaultLighting();

            //         }

            //         mesh.Draw();
            //     }

            // }
            #endregion
            //ScreenManager.GraphicsDevice.
            Vector3 lightDirection = Vector3.Normalize(new Vector3(3, -1, 1));
            Vector3 lightColor = new Vector3(0.3f, 0.4f, 0.2f);

            // Time is scaled down to make things wave in the wind more slowly.
            float time = (float)gameTime.TotalGameTime.TotalSeconds * 0.333f;

            draw = false;
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
                    else if (mesh.Name == "boat")
                    {
                        draw = true;
                    }
                    else if (mesh.Name == "boatPole")
                    {
                        draw = true;
                    }
                    else if (mesh.Name == "boatSails")
                    {
                        draw = true;
                    }
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
                if(draw)
                mesh.Draw();
            }

            foreach (boundingSphere bs in ScreenManager.creteSpheres)
                BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Green);


            //ScreenManager.GraphicsDevice.BlendState = BlendState.Additive;

            if (ScreenManager.player == 0)
            {
                transforms = new Matrix[ScreenManager.angel.Bones.Count];
                ScreenManager.angel.CopyAbsoluteBoneTransformsTo(transforms);
                foreach (ModelMesh mesh in ScreenManager.angel.Meshes)
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
                        effect.Parameters["Bones"].SetValue(ScreenManager.p1Angel.SkinTrans);
                        effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);




                    }
                    if (mesh.Name != "wings")
                        mesh.Draw();
                }
            }
            if (ScreenManager.player == 1)
            {
                transforms = new Matrix[ScreenManager.gorgon.Bones.Count];
                ScreenManager.gorgon.CopyAbsoluteBoneTransformsTo(transforms);
                foreach (ModelMesh mesh in ScreenManager.gorgon.Meshes)
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
                        effect.Parameters["Bones"].SetValue(ScreenManager.p1Gorgon.SkinTrans);
                        effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);






                    }
                    
                    mesh.Draw();
                }
            }
            if (ScreenManager.player == 2)
            {
                transforms = new Matrix[ScreenManager.minotaur.Bones.Count];
                ScreenManager.minotaur.CopyAbsoluteBoneTransformsTo(transforms);
                foreach (ModelMesh mesh in ScreenManager.minotaur.Meshes)
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.CurrentTechnique = effect.Techniques["SkinnedEffect"];
                        //textureParam = Parameters["Texture"];
                        //diffuseColorParam = Parameters["DiffuseColor"];
                        //emissiveColorParam = Parameters["EmissiveColor"];
                        //specularColorParam = Parameters["SpecularColor"];
                        //specularPowerParam = Parameters["SpecularPower"];
                        //eyePositionParam = Parameters["EyePosition"];
                        //fogColorParam = Parameters["FogColor"];
                        //fogVectorParam = Parameters["FogVector"];
                        //worldParam = Parameters["World"];
                        //worldInverseTransposeParam = Parameters["WorldInverseTranspose"];
                        //worldViewProjParam = Parameters["WorldViewProj"];
                        //bonesParam = Parameters["Bones"];
                        //shaderIndexParam = Parameters["ShaderIndex"];

                        //effect.SetBoneTransforms(ScreenManager.asterion.SkinTrans);
                        //effect.View = ScreenManager.camera.View;
                        //effect.Projection = ScreenManager.camera.Projection;
                        // effect.DirectionalLight0 = new DirectionalLight(new EffectParameter(Vector3.Down), Color.Yellow, Color.White, null);
                        ////effect.EnableDefaultLighting();
                        //effect.Parameters["DirLight0DiffuseColor"].SetValue(Color.Tomato.ToVector3());
                        //effect.Parameters["DirLight0Direction"].SetValue(new Vector3(-.333f, .2f, .46f));
                        effect.Parameters["DiffuseColor"].SetValue(Vector4.One);
                        effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));

                        effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
                        effect.Parameters["DirLight0Direction"].SetValue(new Vector3(-0.5265408f, -0.5735765f, -0.6275069f));
                        effect.Parameters["DirLight0DiffuseColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
                        effect.Parameters["DirLight0SpecularColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
                        effect.Parameters["World"].SetValue(Matrix.Identity);
                        effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera.View).Translation);
                        effect.Parameters["Texture"].SetValue(ScreenManager.white);
                        effect.Parameters["Bones"].SetValue(ScreenManager.p1Mino.SkinTrans);
                        effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);



                        //effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
                        //effect.Parameters["LightDirection"].SetValue(new Vector3(.03f, .05f, -1.0f));
                        //  effect.Texture = ScreenManager.white;

                        //effect.AmbientLightColor = Color.White.ToVector3();
                        //effect.DiffuseColor = Color.White.ToVector3();
                        ////effect.EmissiveColor = Color.Brown.ToVector3();

                        //effect.DirectionalLight0.Enabled = true;
                        //effect.DirectionalLight0.DiffuseColor = Color.Blue.ToVector3();
                        //effect.DirectionalLight0.Direction = new Vector3(0, -.7f, .5f);
                        //effect.DirectionalLight0.SpecularColor = Color.Yellow.ToVector3();


                        //effect.DirectionalLight1.Enabled = true;
                        //effect.DirectionalLight1.DiffuseColor = Color.White.ToVector3();
                        //effect.DirectionalLight1.Direction = new Vector3(-.8f, 0, .6f);
                        //effect.DirectionalLight1.SpecularColor = Color.White.ToVector3();

                        //effect.DirectionalLight2.Enabled = true;
                        //effect.DirectionalLight2.DiffuseColor = Color.Blue.ToVector3();
                        //effect.DirectionalLight2.Direction = Vector3.Right;
                        //effect.DirectionalLight2.SpecularColor = Color.Yellow.ToVector3();

           
                    }
                    if (mesh.Name == "minotaur")
                    mesh.Draw();
                }
                

                ScreenManager.minoSphere.CopyAbsoluteBoneTransformsTo(transforms);
                Matrix targetMat =new Matrix();
                foreach (ModelMesh mesh in ScreenManager.minoSphere.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        if (mesh.Name == "rClaw")
                            targetMat = transforms[mesh.ParentBone.Index] * ScreenManager.p1Mino.SkinTrans[ScreenManager.mrHand];
                        //if(mesh.Name == "lClaw")
                        //    targetMat = transforms[mesh.ParentBone.Index] * ScreenManager.p1Mino.SkinTrans[ScreenManager.lHand];


                        effect.World = targetMat;
                        effect.View = ScreenManager.camera.View;
                        effect.Projection = ScreenManager.camera.Projection;
                    }

                   // if (mesh.Name == "rClaw" || mesh.Name == "lClaw")
                       // mesh.Draw();

                }
              //  if (ScreenManager.p1Mino.isAtk1)
                   // ScreenManager.slashParticles.AddParticle(targetMat.Translation, Vector3.Zero);

            }
            if (ScreenManager.player == 3)
            {
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
                        effect.Parameters["Texture"].SetValue(ScreenManager.white);
                        effect.Parameters["Bones"].SetValue(ScreenManager.p1Spear.SkinTrans);
                        effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);






                    }

                    mesh.Draw();
                }
            }

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
                        effect.Parameters["Texture"].SetValue(ScreenManager.white);
                        effect.Parameters["Bones"].SetValue(ScreenManager.perseusAI.SkinTrans);
                        effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);






                    }

                    mesh.Draw();
                }


                DrawBoxes();
           // ScreenManager.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
           // transforms = new Matrix[ScreenManager.board.Bones.Count];
           //ScreenManager.board.CopyAbsoluteBoneTransformsTo(transforms);
           //foreach (ModelMesh mesh in ScreenManager.board.Meshes)
           //{
           //    // Loop over effects in the mesh
           //    foreach (Effect effect in mesh.Effects)
           //    {
           //        // Set the currest values for the effect
           //        effect.CurrentTechnique = effect.Techniques["DrawWithShadowMap"];
           //        effect.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index]);
           //        effect.Parameters["View"].SetValue(ScreenManager.camera.View);
           //        effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
           //        effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
           //        effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);

           //        // if (!createShadowMap)
           //        effect.Parameters["ShadowMap"].SetValue(ScreenManager.shadowRenderTarget);
           //    }
           //    // Draw the mesh
           //    mesh.Draw();
           //}



            // First we draw the ground geometry using BasicEffect.
            //foreach (ModelMesh mesh in ScreenManager.board.Meshes)
            //{
            //    if (mesh.Name != "Billboards")
            //    {
            //        foreach (Effect effect in mesh.Effects)
            //        {
            //            effect.CurrentTechnique = effect.Techniques["DrawWithShadowMap"];

            //            effect.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index]);
            //            effect.Parameters["View"].SetValue(ScreenManager.camera.View);
            //            effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
            //            effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
            //            effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);
            //            //effect.View = ScreenManager.camera.View;
            //            //effect.Projection = ScreenManager.camera.Projection;
            //            //effect.World = transforms[mesh.ParentBone.Index];


            //            //effect.LightingEnabled = true;

            //            //effect.DirectionalLight0.Enabled = true;
            //            //effect.DirectionalLight0.Direction = lightDirection;
            //            //effect.DirectionalLight0.DiffuseColor = lightColor;

            //            //effect.AmbientLightColor = new Vector3(0.1f, 0.2f, 0.1f);
            //        }

            //        ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
            //        ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            //        ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            //        mesh.Draw();
            //    }
            //}

            //// Then we use a two-pass technique to render alpha blended billboards with
            //// almost-correct depth sorting. The only way to make blending truly proper for
            //// alpha objects is to draw everything in sorted order, but manually sorting all
            //// our billboards would be very expensive. Instead, we draw in two passes.
            ////
            //// The first pass has alpha blending turned off, alpha testing set to only accept
            //// ~95% or more opaque pixels, and the depth buffer turned on. Because this is only
            //// rendering the solid parts of each billboard, the depth buffer works as
            //// normal to give correct sorting, but obviously only part of each billboard will
            //// be rendered.
            ////
            //// Then in the second pass we enable alpha blending, set alpha test to only accept
            //// pixels with fractional alpha values, and set the depth buffer to test against
            //// the existing data but not to write new depth values. This means the translucent
            //// areas of each billboard will be sorted correctly against the depth buffer
            //// information that was previously written while drawing the opaque parts, although
            //// there can still be sorting errors between the translucent areas of different
            //// billboards.
            ////
            //// In practice, sorting errors between translucent pixels tend not to be too
            //// noticable as long as the opaque pixels are sorted correctly, so this technique
            //// often looks ok, and is much faster than trying to sort everything 100%
            //// correctly. It is particularly effective for organic textures like grass and
            //// trees.

            //foreach (ModelMesh mesh in ScreenManager.board.Meshes)
            //{
            //    if (mesh.Name == "Billboards")
            //    {
            //        // First pass renders opaque pixels.
            //        foreach (Effect effect in mesh.Effects)
            //        {
            //            effect.CurrentTechnique = effect.Techniques["Billboards"];
            //            //effect.Parameters["World"].SetValue(transforms[0]);
            //            //effect.Parameters["World"].SetValue(Matrix.CreateRotationY(MathHelper.PiOver2));
            //            effect.Parameters["View"].SetValue(ScreenManager.camera.View);
            //            effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
            //            effect.Parameters["LightDirection"].SetValue(lightDirection);
            //            effect.Parameters["WindTime"].SetValue(time);
            //            effect.Parameters["AlphaTestDirection"].SetValue(1f);
            //        }

            //        ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
            //        ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            //        ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            //        ScreenManager.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

            //        mesh.Draw();

            //        // Second pass renders the alpha blended fringe pixels.
            //        foreach (Effect effect in mesh.Effects)
            //        {
            //            effect.Parameters["AlphaTestDirection"].SetValue(-1f);
            //        }

            //        ScreenManager.GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            //        ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            //        mesh.Draw();
            //    }
            //}


            //ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            //ScreenManager.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;

            // transforms = new Matrix[ScreenManager.board.Bones.Count];
            //ScreenManager.board.CopyAbsoluteBoneTransformsTo(transforms);

            //foreach (ModelMesh mesh in ScreenManager.board.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.World = transforms[mesh.ParentBone.Index];
            //        effect.View = ScreenManager.camera.View;
            //        effect.Projection = ScreenManager.camera.Projection;



            //    }
            //    mesh.Draw();
            //}
            // Loop over meshs in the model
            //foreach (ModelMesh mesh in ScreenManager.board.Meshes)
            //{
            //    // Loop over effects in the mesh
            //    foreach (Effect effect in mesh.Effects)
            //    {
            //        // Set the currest values for the effect
            //        effect.CurrentTechnique = effect.Techniques["DrawWithShadowMap"];
            //        effect.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index]);
            //        effect.Parameters["View"].SetValue(ScreenManager.camera.View);
            //        effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
            //        effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
            //        effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);

            //        effect.Parameters["ShadowMap"].SetValue(ScreenManager.shadowRenderTarget);
            //    }
            //    // Draw the mesh
            //    mesh.Draw();
            //}
           //  DrawShadowMapToScreen();



        }

        void DrawShadowMapToScreen()
        {
            ScreenManager.SpriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
            ScreenManager.SpriteBatch.Draw(ScreenManager.shadowRenderTarget, new Rectangle(0, 0, 128, 128), Color.White);
            ScreenManager.SpriteBatch.End();

            ScreenManager.GraphicsDevice.Textures[0] = null;
            ScreenManager.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }







    }
}
