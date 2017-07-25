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

    public class Arena : GameScreen
    {

        public void checkCollisions()
        {

            if(ScreenManager.etLvl1Player.isAtk1 && !ScreenManager.etLvl1Player.hit.Contains(ScreenManager.etLvl1Sword.id))
                foreach(boundingSphere ps in ScreenManager.etLvl1Player.rSword)
                {
                    foreach (boundingSphere es in ScreenManager.etLvl1Sword.spheres)
                    {
                        if (ps.BS.Contains(es.BS) != ContainmentType.Disjoint)
                        {
                            ScreenManager.etLvl1Sword.isHurt = true;
                            ScreenManager.etLvl1Player.hit.Add(ScreenManager.etLvl1Sword.id);
                            break;
                        }
                        
                    }



                }


        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

            ScreenManager.etLvl1Sword.EUpdate(gameTime);

            ScreenManager.etLvl1Player.UpdatePlayerLevel1(gameTime);

            checkCollisions();
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {

            ScreenManager.lightViewProjection = ScreenManager.CreateLightViewProjectionMatrix();
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
            ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;




            ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            DrawTheseus(ScreenManager.etLvl1Player);
            DrawTheseus(ScreenManager.etLvl1Sword);

            base.Draw(gameTime);
        }


        public void DrawTheseus(JuneXnaModel player)
        {
            Matrix[] transforms;

            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" ||
                    //mesh.Name == "TorsoPlate" || 
                    mesh.Name == "lHairF" || mesh.Name == "lHairB" || mesh.Name == "shin"
                    || mesh.Name == "RoundShield" || mesh.Name == "RSword"
                    //|| mesh.Name == "GreekPants"
                    || mesh.Name == "Scalp"
                    || mesh.Name == "eyeball" || mesh.Name == "EyeBrow")
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
                        player.rSword[k++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "shieldS1" || mesh.Name == "shieldS2" || mesh.Name == "shieldS3" || mesh.Name == "shieldS4" || mesh.Name == "shieldS5")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.roundShield[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

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
                    //if (mesh.Name == "lFootS")
                    //{

                    //    targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rFoot];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    player.physicalSphere[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    //}
                }

            }
            foreach (boundingSphere bs in player.spheres)
                BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);
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
            #endregion


            player.formation.Decompose(out scale, out rota, out trans);
            Matrix[] tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);
 

            player.formation.Decompose(out scale, out rota, out trans);
            tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];

            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if (mesh.Name == "RoundShield2")
                {
                    ScreenManager.TheseusTS.World.Decompose(out scale, out rota, out trans);

                    //ScreenManager.ThorTVH.arrowWorld =Matrix.CreateScale(scale) *  Matrix.CreateTranslation(transforms[mesh.ParentBone.Index].Translation) * ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];// *ScreenManager.ThorTVH.World;
                    // ScreenManager.ThorTVH.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.ThorTVH.World.Translation), rota);
                    ScreenManager.TheseusTS.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);// *ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];
                }

            }

            foreach (Projectile2 proj in ScreenManager.TheseusTS.projectiles)
            {
                foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
                {
                    if (mesh.Name == "RoundShield2")
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



                }
            }


           
        }
    }
}
