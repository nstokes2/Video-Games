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
    public class LastSonOfOlympus :GameScreen
    {
        TimeSpan time;
        Vector3 l0Dir;
        Vector3 l0Dif;
        Vector3 l0Spec;
        Vector3 l1Dir;
        Vector3 l1Dif;
        Vector3 l1Spec;
        Vector3 l2Dir;
        Vector3 l2Dif;
        Vector3 l2Spec;
        Vector3 ambientLightColor;
        Viewport vp1;
        Viewport vp2;
        public LastSonOfOlympus()
        {
            time = new TimeSpan();
            // Key light.
            l0Dir = new Vector3(-0.5265408f, -0.5735765f, -0.6275069f);
            l0Dif = new Vector3(1, 0.9607844f, 0.8078432f);
            l0Spec = new Vector3(1, 0.9607844f, 0.8078432f);


            // Fill light.
            l1Dir = new Vector3(0.7198464f, 0.3420201f, 0.6040227f);
            l1Dif = new Vector3(0.9647059f, 0.7607844f, 0.4078432f);
            l1Spec = Vector3.Zero;


            // Back light.
            l2Dir = new Vector3(0.4545195f, -0.7660444f, 0.4545195f);
            l2Dif = new Vector3(0.3231373f, 0.3607844f, 0.3937255f);
            l2Spec = new Vector3(0.3231373f, 0.3607844f, 0.3937255f);


            ambientLightColor = new Vector3(0.05333332f, 0.09882354f, 0.1819608f);



        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

            //if(ScreenManager.enemyRunners.Count < 1)
            //for (int i = 0; i < 1; i++)
            //{
            //    JuneXnaModel juney = new JuneXnaModel(Vector3.Zero, Vector3.Forward);
            //    juney.SkinningData = ScreenManager.juneModel.Tag as SkinningData;
            //    juney.setAnimationPlayers();
            //    ScreenManager.loadSpheresJuneModel(juney);
            //    ScreenManager.enemyRunners.Add(juney);
                

            //}

            ScreenManager.fakeStatue.UpdateBrace(gameTime);

 
            foreach (JuneXnaModel juney in ScreenManager.playerTowers)
                juney.UpdateBrace(gameTime);

            ScreenManager.LSMino.UpdateBrace(gameTime);

            if (ScreenManager.buildStage)
                ScreenManager.Cupid.UpdateEngineer(gameTime);
            else
            {
                ScreenManager.Cupid.UpdateEngineer(gameTime);
                ScreenManager.LSRunner.LSPathing(gameTime);
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        }
        public void DrawArena(GameTime gameTime)
        {
            Matrix[] transforms = new Matrix[ScreenManager.arena.Bones.Count];
            ScreenManager.arena.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ScreenManager.arena.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    //effect.World = transforms[mesh.ParentBone.Index];
                    //effect.View = ScreenManager.camera.View;
                    //effect.Projection = ScreenManager.camera.Projection;
                    //effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }












        }
        public void DrawBoard(GameTime gameTime)
        {


            Vector3 lightDirection = Vector3.Normalize(new Vector3(3, -1, 1));
            Vector3 lightColor = new Vector3(0.3f, 0.4f, 0.2f);

            // Time is scaled down to make things wave in the wind more slowly.
            float time = (float)gameTime.TotalGameTime.TotalSeconds * 0.333f;

            bool draw = false;


            Matrix[] transforms = new Matrix[ScreenManager.aosBoard.Bones.Count];
            ScreenManager.aosBoard.CopyAbsoluteBoneTransformsTo(transforms);

            ScreenManager.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
            foreach (ModelMesh mesh in ScreenManager.aosBoard.Meshes)
            {
                if (mesh.Name == "Zeus" || mesh.Name == "Pluto" || mesh.Name == "Ares" || mesh.Name == "Plane001" || mesh.Name == "closedGate")
                {
                    foreach (Effect effect in mesh.Effects)
                    {

                        //if (mesh.Name == "openGate")
                        //    effect.Parameters["Texture"].SetValue(ScreenManager.slashTga);
                        //else if (mesh.Name == "closedGate")
                        //    effect.Parameters["Texture"].SetValue(ScreenManager.fire);
                        //else
                        if (mesh.Name == "Plane001")
                            effect.Parameters["Texture"].SetValue(ScreenManager.road);
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.whiteG);
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
                    }
                    // if ((mesh.Name == "openGate" && ScreenManager.gateOpen) || (mesh.Name == "closedGate" & !ScreenManager.gateOpen) || (mesh.Name != "openGate" && mesh.Name != "closedGate"))

                    mesh.Draw();
                }
            }
            //transforms = new Matrix[ScreenManager.grassModel.Bones.Count];
            //ScreenManager.grassModel.CopyAbsoluteBoneTransformsTo(transforms);
            //foreach (ModelMesh mesh in ScreenManager.grassModel.Meshes)
            //{
            //    foreach (AlphaTestEffect effect in mesh.Effects)
            //    {

            //        effect.World = transforms[mesh.ParentBone.Index];
            //        effect.View = ScreenManager.camera.View;
            //        effect.Projection = ScreenManager.camera.Projection;
            //        effect.Texture = ScreenManager.grass;

            //        //effect.CurrentTechnique.Passes[0].Apply();
            //    }

            //    mesh.Draw();
            //}





            //foreach (boundingSphere bs in ScreenManager.creteSpheres)
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Green);



        }
        public void DrawBoardVP2(GameTime gameTime)
        {


            Vector3 lightDirection = Vector3.Normalize(new Vector3(3, -1, 1));
            Vector3 lightColor = new Vector3(0.3f, 0.4f, 0.2f);

            // Time is scaled down to make things wave in the wind more slowly.
            float time = (float)gameTime.TotalGameTime.TotalSeconds * 0.333f;

            bool draw = false;


            Matrix[] transforms = new Matrix[ScreenManager.aosBoard.Bones.Count];
            ScreenManager.aosBoard.CopyAbsoluteBoneTransformsTo(transforms);

            ScreenManager.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
            foreach (ModelMesh mesh in ScreenManager.aosBoard.Meshes)
            {
                if (mesh.Name == "Zeus" || mesh.Name == "Pluto" || mesh.Name == "Ares" || mesh.Name == "Plane001" || mesh.Name == "closedGate")
                {
                    foreach (Effect effect in mesh.Effects)
                    {

                        //if (mesh.Name == "openGate")
                        //    effect.Parameters["Texture"].SetValue(ScreenManager.slashTga);
                        //else if (mesh.Name == "closedGate")
                        //    effect.Parameters["Texture"].SetValue(ScreenManager.fire);
                        //else
                        if (mesh.Name == "Plane001")
                            effect.Parameters["Texture"].SetValue(ScreenManager.road);
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.whiteG);
                        effect.CurrentTechnique = effect.Techniques["DrawWithShadowMap"];
                        effect.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index]);
                        effect.Parameters["View"].SetValue(ScreenManager.camera2.View);
                        effect.Parameters["Projection"].SetValue(ScreenManager.camera2.Projection);
                        effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                        effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);
                        //tree treeLeaves parthenon boat boatPole boatSails arena pylons Plane002 GeoSphere001 Box001 Box002 
                        //tombSphere parthenonSphere tombSphere caveSphere crucibleSphere theseusSphere
                        // if (!createShadowMap)
                        effect.Parameters["ShadowMap"].SetValue(ScreenManager.shadowRenderTarget);
                    }
                    // if ((mesh.Name == "openGate" && ScreenManager.gateOpen) || (mesh.Name == "closedGate" & !ScreenManager.gateOpen) || (mesh.Name != "openGate" && mesh.Name != "closedGate"))

                    mesh.Draw();
                }
            }

            transforms = new Matrix[ScreenManager.finalBuild.Bones.Count];
            ScreenManager.finalBuild.CopyAbsoluteBoneTransformsTo(transforms);
             ModelMesh templeMesh = ScreenManager.finalBuild.Meshes["Temple"];

            foreach (BasicEffect effect in templeMesh.Effects)
            {
                effect.World = transforms[templeMesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.cupidTemple.position) * Matrix.CreateTranslation(-new Vector3(5.0f, 0.0f, 0.0f));
                effect.View = ScreenManager.camera2.View;
                effect.Projection = ScreenManager.camera2.Projection;
                effect.EnableDefaultLighting();
            }
            templeMesh.Draw();
            //transforms = new Matrix[ScreenManager.grassModel.Bones.Count];
            //ScreenManager.grassModel.CopyAbsoluteBoneTransformsTo(transforms);
            //foreach (ModelMesh mesh in ScreenManager.grassModel.Meshes)
            //{
            //    foreach (AlphaTestEffect effect in mesh.Effects)
            //    {

            //        effect.World = transforms[mesh.ParentBone.Index];
            //        effect.View = ScreenManager.camera.View;
            //        effect.Projection = ScreenManager.camera.Projection;
            //        effect.Texture = ScreenManager.grass;

            //        //effect.CurrentTechnique.Passes[0].Apply();
            //    }

            //    mesh.Draw();
            //}





            //foreach (boundingSphere bs in ScreenManager.creteSpheres)
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Green);



        }
        public void DrawTheseusVP2(JuneXnaModel player)
        {

            //RayRenderer.Render(player.ray, 200.0f, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);
            Matrix[] transforms;

            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" ||
                    mesh.Name == "TorsoPlate" ||
                    mesh.Name == "lHairF" || mesh.Name == "lHairB" || mesh.Name == "shin"

                    || mesh.Name == "GreekPants"
                    || mesh.Name == "Scalp"
                    || mesh.Name == "eyeball" || mesh.Name == "EyeBrow"

                    || ((player.fightClass == 0 && (mesh.Name == "RoundShield" || mesh.Name == "RSword"))
                    || (player.fightClass == 1 && (mesh.Name == "Bow" || mesh.Name == "Arrow" || mesh.Name == "Quiver"))))
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
                        effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera2.View).Translation);
                        if (mesh.Name == "Alecto")
                            effect.Parameters["Texture"].SetValue(ScreenManager.humanTex);
                        else if (mesh.Name == "sHair" || mesh.Name == "lHairF" || mesh.Name == "lHairB" || mesh.Name == "Scalp")
                            effect.Parameters["Texture"].SetValue(ScreenManager.black);
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.gold1);
                        effect.Parameters["Bones"].SetValue(player.SkinTrans);
                        effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera2.View * ScreenManager.camera2.Projection);



                    }

                    mesh.Draw();
                }


            }
            #region formation

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

                //mesh.Draw();
            }
            #endregion



            Matrix targetMat = new Matrix();
            transforms = new Matrix[ScreenManager.spearSphere.Bones.Count];
            ScreenManager.spearSphere.CopyAbsoluteBoneTransformsTo(transforms);
            int i = 0;
            int j = 0;
            int k = 0;
            foreach (ModelMesh mesh in ScreenManager.spearSphere.Meshes)
            {
                // draw = false;
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (mesh.Name == "rSwordS1" || mesh.Name == "rSwordS2" || mesh.Name == "rSwordS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.rSword[k++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));

                    }
                    if (mesh.Name == "shieldS1" || mesh.Name == "shieldS2" || mesh.Name == "shieldS3" || mesh.Name == "shieldS4" || mesh.Name == "shieldS5")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.roundShield[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));

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
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));


                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "hipS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.hips];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "lULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "lLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "rULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "rLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    //if (mesh.Name == "lFootS")
                    //{

                    //    targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rFoot];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    player.physicalSphere[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));

                    //}
                }

            }
            foreach (boundingSphere bs in player.spheres)
                BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);

            player.formation.Decompose(out scale, out rota, out trans);
            Matrix[] tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);

            player.formation.Decompose(out scale, out rota, out trans);
            tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            foreach (Projectile2 proj in player.projectiles)

                ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if (mesh.Name == "RoundShield2")
                {
                    ScreenManager.Theseus.World.Decompose(out scale, out rota, out trans);

                    //ScreenManager.ThorTVH.arrowWorld =Matrix.CreateScale(scale) *  Matrix.CreateTranslation(transforms[mesh.ParentBone.Index].Translation) * ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];// *ScreenManager.ThorTVH.World;
                    // ScreenManager.ThorTVH.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.ThorTVH.World.Translation), rota);
                    ScreenManager.Theseus.shieldWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);// *ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];
                }
                if (mesh.Name == "Arrow2")
                {
                    ScreenManager.Theseus.World.Decompose(out scale, out rota, out trans);
                    ScreenManager.Theseus.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);
                }


            }

            foreach (Projectile2 proj in ScreenManager.Theseus.projectiles)
            {
                foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
                {
                    if ((mesh.Name == "RoundShield2" && proj.Name == "Shield") ||
                        (mesh.Name == "Arrow2" && proj.Name == "Arrow"))
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            //transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);
                            effect.World = proj.world;
                            effect.EnableDefaultLighting();
                            effect.View = ScreenManager.camera.View;
                            effect.Projection = ScreenManager.camera.Projection;

                        }
                        mesh.Draw();
                    }


                    if (mesh.Name == "BS")
                    {

                        targetMat = proj.world;
                        targetMat.Decompose(out scale, out rota, out trans);
                        proj.BS = new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y));


                    }
                    //  BoundingSphereRenderer.Render(proj.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);




                }
            }



        }
        public void DrawTheseusFake(JuneXnaModel player)
        {

            //RayRenderer.Render(player.ray, 200.0f, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);
            Matrix[] transforms;

            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" ||
                    mesh.Name == "TorsoPlate" 
                    || mesh.Name == "GreekPants"
                    || mesh.Name == "Scalp"
                    || mesh.Name == "eyeball" || mesh.Name == "EyeBrow"
                   )
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
                        else if (mesh.Name == "sHair" || mesh.Name == "lHairF" || mesh.Name == "lHairB" || mesh.Name == "Scalp")
                            effect.Parameters["Texture"].SetValue(ScreenManager.black);
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.gold1);
                        effect.Parameters["Bones"].SetValue(player.SkinTrans);
                        effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);



                    }

                    mesh.Draw();
                }


            }
            #region formation

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

                //mesh.Draw();
            }
            #endregion



            Matrix targetMat = new Matrix();
            transforms = new Matrix[ScreenManager.spearSphere.Bones.Count];
            ScreenManager.spearSphere.CopyAbsoluteBoneTransformsTo(transforms);
            int i = 0;
            int j = 0;
            int k = 0;
            foreach (ModelMesh mesh in ScreenManager.spearSphere.Meshes)
            {
                // draw = false;
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (mesh.Name == "rSwordS1" || mesh.Name == "rSwordS2" || mesh.Name == "rSwordS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.rSword[k++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));

                    }
                    if (mesh.Name == "shieldS1" || mesh.Name == "shieldS2" || mesh.Name == "shieldS3" || mesh.Name == "shieldS4" || mesh.Name == "shieldS5")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.roundShield[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));

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
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));


                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "hipS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.hips];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "lULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "lLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "rULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "rLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    //if (mesh.Name == "lFootS")
                    //{

                    //    targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rFoot];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    player.physicalSphere[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));

                    //}
                }

            }
            foreach (boundingSphere bs in player.spheres)
                BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);

            player.formation.Decompose(out scale, out rota, out trans);
            Matrix[] tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);

            player.formation.Decompose(out scale, out rota, out trans);
            tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            foreach (Projectile2 proj in player.projectiles)

                ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if (mesh.Name == "RoundShield2")
                {
                    ScreenManager.Theseus.World.Decompose(out scale, out rota, out trans);

                    //ScreenManager.ThorTVH.arrowWorld =Matrix.CreateScale(scale) *  Matrix.CreateTranslation(transforms[mesh.ParentBone.Index].Translation) * ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];// *ScreenManager.ThorTVH.World;
                    // ScreenManager.ThorTVH.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.ThorTVH.World.Translation), rota);
                    ScreenManager.Theseus.shieldWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);// *ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];
                }
                if (mesh.Name == "Arrow2")
                {
                    ScreenManager.Theseus.World.Decompose(out scale, out rota, out trans);
                    ScreenManager.Theseus.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);
                }


            }

            foreach (Projectile2 proj in ScreenManager.Theseus.projectiles)
            {
                foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
                {
                    if ((mesh.Name == "RoundShield2" && proj.Name == "Shield") ||
                        (mesh.Name == "Arrow2" && proj.Name == "Arrow"))
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            //transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);
                            effect.World = proj.world;
                            effect.EnableDefaultLighting();
                            effect.View = ScreenManager.camera.View;
                            effect.Projection = ScreenManager.camera.Projection;

                        }
                        mesh.Draw();
                    }


                    if (mesh.Name == "BS")
                    {

                        targetMat = proj.world;
                        targetMat.Decompose(out scale, out rota, out trans);
                        proj.BS = new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y));


                    }
                    //  BoundingSphereRenderer.Render(proj.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);




                }
            }



        }
        public void DrawTheseus(JuneXnaModel player)
        {

            //RayRenderer.Render(player.ray, 200.0f, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);
            Matrix[] transforms;

            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" ||
                    mesh.Name == "TorsoPlate" ||
                    mesh.Name == "lHairF" || mesh.Name == "lHairB" || mesh.Name == "shin"

                    || mesh.Name == "GreekPants"
                    || mesh.Name == "Scalp"
                    || mesh.Name == "eyeball" || mesh.Name == "EyeBrow"

                    || ((player.fightClass == 0 && (mesh.Name == "RoundShield" || mesh.Name == "RSword"))
                    || (player.fightClass == 1 && (mesh.Name == "Bow" || mesh.Name == "Arrow" || mesh.Name == "Quiver"))))
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
                        else if (mesh.Name == "sHair" || mesh.Name == "lHairF" || mesh.Name == "lHairB" || mesh.Name == "Scalp")
                            effect.Parameters["Texture"].SetValue(ScreenManager.black);
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.gold1);
                        effect.Parameters["Bones"].SetValue(player.SkinTrans);
                        effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);



                    }

                    mesh.Draw();
                }


            }
            #region formation

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

                //mesh.Draw();
            }
            #endregion



            Matrix targetMat = new Matrix();
            transforms = new Matrix[ScreenManager.spearSphere.Bones.Count];
            ScreenManager.spearSphere.CopyAbsoluteBoneTransformsTo(transforms);
            int i = 0;
            int j = 0;
            int k = 0;
            foreach (ModelMesh mesh in ScreenManager.spearSphere.Meshes)
            {
                // draw = false;
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (mesh.Name == "rSwordS1" || mesh.Name == "rSwordS2" || mesh.Name == "rSwordS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.rSword[k++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));

                    }
                    if (mesh.Name == "shieldS1" || mesh.Name == "shieldS2" || mesh.Name == "shieldS3" || mesh.Name == "shieldS4" || mesh.Name == "shieldS5")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.roundShield[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));

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
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));


                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "hipS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.hips];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "lULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "lLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "rULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "rLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    //if (mesh.Name == "lFootS")
                    //{

                    //    targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rFoot];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    player.physicalSphere[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));

                    //}
                }

            }
            foreach (boundingSphere bs in player.spheres)
                BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);

            player.formation.Decompose(out scale, out rota, out trans);
            Matrix[] tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);

            player.formation.Decompose(out scale, out rota, out trans);
            tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            foreach (Projectile2 proj in player.projectiles)

                ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if (mesh.Name == "RoundShield2")
                {
                    ScreenManager.Theseus.World.Decompose(out scale, out rota, out trans);

                    //ScreenManager.ThorTVH.arrowWorld =Matrix.CreateScale(scale) *  Matrix.CreateTranslation(transforms[mesh.ParentBone.Index].Translation) * ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];// *ScreenManager.ThorTVH.World;
                    // ScreenManager.ThorTVH.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.ThorTVH.World.Translation), rota);
                    ScreenManager.Theseus.shieldWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);// *ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];
                }
                if (mesh.Name == "Arrow2")
                {
                    ScreenManager.Theseus.World.Decompose(out scale, out rota, out trans);
                    ScreenManager.Theseus.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);
                }


            }

            foreach (Projectile2 proj in ScreenManager.Theseus.projectiles)
            {
                foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
                {
                    if ((mesh.Name == "RoundShield2" && proj.Name == "Shield") ||
                        (mesh.Name == "Arrow2" && proj.Name == "Arrow"))
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            //transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);
                            effect.World = proj.world;
                            effect.EnableDefaultLighting();
                            effect.View = ScreenManager.camera.View;
                            effect.Projection = ScreenManager.camera.Projection;

                        }
                        mesh.Draw();
                    }


                    if (mesh.Name == "BS")
                    {

                        targetMat = proj.world;
                        targetMat.Decompose(out scale, out rota, out trans);
                        proj.BS = new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y));


                    }
                  //  BoundingSphereRenderer.Render(proj.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);




                }
            }



        }
        public void DrawCupid(JuneXnaModel player, string namedEffect)
        {

            RayRenderer.Render(player.ray, 200.0f, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);
            Matrix[] transforms;

            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" ||
                    mesh.Name == "TorsoPlate" ||
                    mesh.Name == "lHairF" || mesh.Name == "lHairB" || mesh.Name == "shin"

                    || mesh.Name == "GreekPants"
                    || mesh.Name == "Scalp"
                    || mesh.Name == "eyeball" || mesh.Name == "EyeBrow"

                    || ((player.fightClass == 0 && (mesh.Name == "RoundShield" || mesh.Name == "RSword"))
                    || (player.fightClass == 1 && (mesh.Name == "Bow" || mesh.Name == "Arrow" || mesh.Name == "Quiver"))))
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.CurrentTechnique = effect.Techniques[namedEffect];

                        effect.Parameters["DiffuseColor"].SetValue(new Vector4(2.0f, 2.0f, 2.0f, 1.0f));
                        effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));


                        effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
                        effect.Parameters["DirLight0Direction"].SetValue(new Vector3(-0.5265408f, -0.5735765f, -0.6275069f));
                        effect.Parameters["DirLight0DiffuseColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
                        effect.Parameters["DirLight0SpecularColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
                        effect.Parameters["DirLight1Direction"].SetValue(l1Dir);
                        effect.Parameters["DirLight1DiffuseColor"].SetValue(l1Dif);
                        effect.Parameters["DirLight1SpecularColor"].SetValue(l1Spec);
                        effect.Parameters["DirLight2Direction"].SetValue(l2Dir);
                        effect.Parameters["DirLight2DiffuseColor"].SetValue(l2Dif);
                        effect.Parameters["DirLight2SpecularColor"].SetValue(l2Spec);
                        effect.Parameters["World"].SetValue(Matrix.Identity);
                        effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera.View).Translation);
                        if (mesh.Name == "Alecto")
                            effect.Parameters["Texture"].SetValue(ScreenManager.humanTex);
                        else if (mesh.Name == "sHair" || mesh.Name == "lHairF" || mesh.Name == "lHairB" || mesh.Name == "Scalp")
                            effect.Parameters["Texture"].SetValue(ScreenManager.black);
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.gold1);
                        effect.Parameters["Bones"].SetValue(player.SkinTrans);
                        effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);



                    }

                    mesh.Draw();
                }


            }
            #region formation

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
            #endregion



            Matrix targetMat = new Matrix();
            transforms = new Matrix[ScreenManager.spearSphere.Bones.Count];
            ScreenManager.spearSphere.CopyAbsoluteBoneTransformsTo(transforms);
            int i = 0;
            int j = 0;
            int k = 0;
            foreach (ModelMesh mesh in ScreenManager.spearSphere.Meshes)
            {
                // draw = false;
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (mesh.Name == "rSwordS1" || mesh.Name == "rSwordS2" || mesh.Name == "rSwordS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.rSword[k++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));

                    }
                    if (mesh.Name == "shieldS1" || mesh.Name == "shieldS2" || mesh.Name == "shieldS3" || mesh.Name == "shieldS4" || mesh.Name == "shieldS5")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.roundShield[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));

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
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));


                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "hipS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.hips];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "lULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "lLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "rULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    if (mesh.Name == "rLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));
                    }
                    //if (mesh.Name == "lFootS")
                    //{

                    //    targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rFoot];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    player.physicalSphere[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y)));

                    //}
                }

            }
            foreach (boundingSphere bs in player.spheres)
                BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);

            player.formation.Decompose(out scale, out rota, out trans);
            Matrix[] tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);

            player.formation.Decompose(out scale, out rota, out trans);
            tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            foreach (Projectile2 proj in player.projectiles)

                ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if (mesh.Name == "RoundShield2")
                {
                    ScreenManager.Theseus.World.Decompose(out scale, out rota, out trans);

                    //ScreenManager.ThorTVH.arrowWorld =Matrix.CreateScale(scale) *  Matrix.CreateTranslation(transforms[mesh.ParentBone.Index].Translation) * ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];// *ScreenManager.ThorTVH.World;
                    // ScreenManager.ThorTVH.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.ThorTVH.World.Translation), rota);
                    ScreenManager.Theseus.shieldWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);// *ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];
                }
                if (mesh.Name == "Arrow2")
                {
                    ScreenManager.Theseus.World.Decompose(out scale, out rota, out trans);
                    ScreenManager.Theseus.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);
                }


            }

            foreach (Projectile2 proj in ScreenManager.Theseus.projectiles)
            {
                foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
                {
                    if ((mesh.Name == "RoundShield2" && proj.Name == "Shield") ||
                        (mesh.Name == "Arrow2" && proj.Name == "Arrow"))
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            //transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);
                            effect.World = proj.world;
                            effect.EnableDefaultLighting();
                            effect.View = ScreenManager.camera.View;
                            effect.Projection = ScreenManager.camera.Projection;

                        }
                        mesh.Draw();
                    }


                    if (mesh.Name == "BS")
                    {

                        targetMat = proj.world;
                        targetMat.Decompose(out scale, out rota, out trans);
                        proj.BS = new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Y));


                    }
                    BoundingSphereRenderer.Render(proj.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);




                }
            }



        }
        public void DrawMino(Minotaur mino)
        {

            Matrix[] transforms = new Matrix[ScreenManager.minotaur.Bones.Count];
            ScreenManager.minotaur.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.minotaur.Meshes)
            {
                if (mesh.Name == "minotaur" || mesh.Name == "minotaurEye")
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
                        effect.Parameters["DirLight1Direction"].SetValue(l1Dir);
                        effect.Parameters["DirLight1DiffuseColor"].SetValue(l1Dif);
                        effect.Parameters["DirLight1SpecularColor"].SetValue(l1Spec);
                        effect.Parameters["DirLight2Direction"].SetValue(l2Dir);
                        effect.Parameters["DirLight2DiffuseColor"].SetValue(l2Dif);
                        effect.Parameters["DirLight2SpecularColor"].SetValue(l2Spec);
                        effect.Parameters["World"].SetValue(Matrix.Identity);
                        effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera2.View).Translation);

                        effect.Parameters["Bones"].SetValue(mino.SkinTrans);
                        effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera2.View * ScreenManager.camera2.Projection);
                        if (mesh.Name == "minotaur")
                            effect.Parameters["Texture"].SetValue(ScreenManager.minoTex);
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.whiteG);





                    }

                    mesh.Draw();
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            if (!ScreenManager.buildStage)
            {
                Viewport oldviewport = ScreenManager.GraphicsDevice.Viewport;
                ScreenManager.GraphicsDevice.Viewport = ScreenManager.vp1;

                DrawCupid(ScreenManager.Cupid, "SkinnedEffect");
                DrawBoard(gameTime);


                ScreenManager.GraphicsDevice.Viewport = ScreenManager.vp2;

                DrawMino(ScreenManager.LSMino);

                foreach (JuneXnaModel juney in ScreenManager.playerTowers)
                    DrawTheseusVP2(juney);
                DrawTheseusVP2(ScreenManager.LSRunner);

               // DrawBoardVP2(gameTime);
                ModelMesh mesh = ScreenManager.finalBuild.Meshes["25Square"];
                Matrix[] transforms = new Matrix[ScreenManager.finalBuild.Bones.Count];
                ScreenManager.finalBuild.CopyAbsoluteBoneTransformsTo(transforms);
                for (int i = 0; i < 24; i++)
                    for (int j = 0; j < 24; j++)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(i * 30, 0.0f, j * 30);
                            effect.View = ScreenManager.camera2.View;
                            effect.Projection = ScreenManager.camera2.Projection;
                            if (ScreenManager.pathBoard[i][j] == true)
                                effect.DiffuseColor = Vector3.One;
                          
                            else
                                effect.DiffuseColor = Vector3.Zero;
                        }
                        mesh.Draw();


                    }
             



                if (ScreenManager.LSRunner.uiPath != null)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.LSRunner.uiPath.start.X * 30, 0.0f, ScreenManager.LSRunner.uiPath.start.Y * 30);
                        effect.View = ScreenManager.camera2.View;
                        effect.Projection = ScreenManager.camera2.Projection;
                        effect.DiffuseColor = Color.Red.ToVector3();
                    }
                    mesh.Draw();
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.LSRunner.uiPath.end.X * 30, 0.0f, ScreenManager.LSRunner.uiPath.end.Y * 30);
                        effect.View = ScreenManager.camera2.View;
                        effect.Projection = ScreenManager.camera2.Projection;
                        effect.DiffuseColor = Color.DarkGreen.ToVector3();
                    }
                    mesh.Draw();

                }




                 
                    


                ScreenManager.GraphicsDevice.Viewport = oldviewport;
            }
            else
            {
                //DrawArena(gameTime);
                DrawCupid(ScreenManager.Cupid, "SkinnedEffect");
                DrawTheseusFake(ScreenManager.fakeStatue);
                DrawBoard(gameTime);
                //25sq is actually 30
                ModelMesh mesh = ScreenManager.finalBuild.Meshes["25Square"];
                Matrix[] transforms = new Matrix[ScreenManager.finalBuild.Bones.Count];
                ScreenManager.finalBuild.CopyAbsoluteBoneTransformsTo(transforms);
                for(int i = 0; i< 24 ; i++)
                    for (int j = 0; j < 24; j++)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(i * 30, 0.0f, j * 30);
                            effect.View = ScreenManager.camera.View;
                            effect.Projection = ScreenManager.camera.Projection;
                            if (ScreenManager.pathBoard[i][j] == true)
                                effect.DiffuseColor = Vector3.One;
                            //if ( ScreenManager. board[i][j] == true)
                            // effect.DiffuseColor = Vector3.One;
                            else
                                effect.DiffuseColor = Vector3.Zero;



                        }
                        mesh.Draw();


                    }






                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.Cupid.Position.X / 30 , 0.0f, ScreenManager.Cupid.Position.Z / 30);
                    effect.View = ScreenManager.camera2.View;
                    effect.Projection = ScreenManager.camera2.Projection;
                    effect.DiffuseColor = Color.DarkOrange.ToVector3();
                }
                mesh.Draw();
                //foreach (BasicEffect effect in mesh.Effects)
                //{
                //    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.Cupid.Position.X / 30 + 1, 0.0f, ScreenManager.Cupid.Position.X / 30);
                //    effect.View = ScreenManager.camera2.View;
                //    effect.Projection = ScreenManager.camera2.Projection;
                //    effect.DiffuseColor = Color.DarkOrange.ToVector3();
                //}
                //mesh.Draw();
                //foreach (BasicEffect effect in mesh.Effects)
                //{
                //    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.Cupid.Position.X / 30 + 1, 0.0f, ScreenManager.Cupid.Position.X / 30 + 1);
                //    effect.View = ScreenManager.camera2.View;
                //    effect.Projection = ScreenManager.camera2.Projection;
                //    effect.DiffuseColor = Color.DarkOrange.ToVector3();
                //}
                //mesh.Draw();


                mesh = ScreenManager.finalBuild.Meshes["Temple"];

                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.cupidTemple.position) * Matrix.CreateTranslation(-new Vector3(5.0f, 0.0f,  0.0f));
                    effect.View = ScreenManager.camera.View;
                    effect.Projection = ScreenManager.camera.Projection;
                    effect.EnableDefaultLighting();
                }
                mesh.Draw();

                //mesh = ScreenManager.finalBuild.Meshes["Pylon"];
                //foreach (BasicEffect effect in mesh.Effects)
                //{
                //    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.Cupid.Position.X / 30 , 0.0f, ScreenManager.Cupid.Position.Z/30 );
                //    effect.View = ScreenManager.camera.View;
                //    effect.Projection = ScreenManager.camera.Projection;
                //    effect.EnableDefaultLighting();


                //}
                //mesh.Draw();

                







                foreach (JuneXnaModel juney in ScreenManager.playerTowers)

                    DrawTheseus(juney);



            }
            base.Draw(gameTime);

        }
    }
}
