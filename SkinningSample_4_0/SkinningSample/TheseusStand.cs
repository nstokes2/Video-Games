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
    public class TheseusStand : GameScreen
    {

     public  TheseusStand()
        {
            //ScreenManager.storyIndex++;

            //ScreenManager.script = true;
            //ScreenManager.globalInput.pause = true;
            //ScreenManager.scriptMessage = new List<string>();

            //ScreenManager.scriptMessage.Add("While celebrating past victories with your allies");
            //ScreenManager.scriptMessage.Add("you feel a hint of battle in the air and make your");
            //ScreenManager.scriptMessage.Add("way to the city to investigate.  You make eye contact ");
            //ScreenManager.scriptMessage.Add("with two other warriors, right as a horn sounsd");
        }

       public override void Update(GameTime gameTime, bool otherScreenHasFocus,
      bool coveredByOtherScreen)
       {


           //ScreenManager.p1June.UpdateParis(gameTime);
           //ScreenManager.eJune.UpdateTheseus(gameTime);


           if (!ScreenManager.globalInput.pause)
               ScreenManager.p1June.UpdateTheseus(gameTime);



           base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

       }
       public void DrawTheseus(JuneXnaModel player)
       {
           Matrix[] transforms;

           transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
           ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);
           foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
           {
               if (mesh.Name == "Alecto" || mesh.Name == "TorsoPlate" || mesh.Name == "lHairF" || mesh.Name == "lHairB" || mesh.Name == "shin"
                   || mesh.Name == "RoundShield" || mesh.Name == "RSword" || mesh.Name == "GreekPants" ||  mesh.Name == "Scalp")
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

                       if (mesh.Name == "sHair")
                           effect.Parameters["Texture"].SetValue(ScreenManager.white);
                       if (mesh.Name == "longHairF")
                           effect.Parameters["Texture"].SetValue(ScreenManager.white);
                       if (mesh.Name == "longHairB")
                           effect.Parameters["Texture"].SetValue(ScreenManager.humanTex);

                   }

                   mesh.Draw();
               }
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
                   if (mesh.Name == "RThrowingAxe")
                       player.rTAxeWorld = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                   if (mesh.Name == "LSpear")
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
       public override void Draw(GameTime gameTime)
       {

           ScreenManager.lightViewProjection = ScreenManager.CreateLightViewProjectionMatrix();
           ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
           ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
           ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;

           //without this createshadowmap throws an error
           ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
           CreateShadowMap();
           ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
           DrawMino(ScreenManager.p1Mino);
           DrawBoard(gameTime);


           DrawTheseus(ScreenManager.p1June);

           if (ScreenManager.script)
               DrawHud();



           base.Draw(gameTime);
       }
       public void DrawBoard(GameTime gameTime)
       {


           Vector3 lightDirection = Vector3.Normalize(new Vector3(3, -1, 1));
           Vector3 lightColor = new Vector3(0.3f, 0.4f, 0.2f);

           // Time is scaled down to make things wave in the wind more slowly.
           float time = (float)gameTime.TotalGameTime.TotalSeconds * 0.333f;

           bool draw = false;


           Matrix[] transforms = new Matrix[ScreenManager.minoBoard1.Bones.Count];
           ScreenManager.minoBoard1.CopyAbsoluteBoneTransformsTo(transforms);
           ScreenManager.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
           foreach (ModelMesh mesh in ScreenManager.minoBoard1.Meshes)
           {
               foreach (Effect effect in mesh.Effects)
               {

                   if (mesh.Name == "openGate")
                       effect.Parameters["Texture"].SetValue(ScreenManager.slashTga);
                   else if (mesh.Name == "closedGate")
                       effect.Parameters["Texture"].SetValue(ScreenManager.fire);
                   else
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
               if ((mesh.Name == "openGate" && ScreenManager.gateOpen) || (mesh.Name == "closedGate" & !ScreenManager.gateOpen) || (mesh.Name != "openGate" && mesh.Name != "closedGate"))
                   mesh.Draw();
           }

           foreach (boundingSphere bs in ScreenManager.creteSpheres)
               BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Green);



       }

       public void DrawHud()
       {
           int width = ScreenManager.GraphicsDevice.Viewport.Width - 20;
           int height = ScreenManager.GraphicsDevice.Viewport.Height - 20;

           Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
           Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
           Vector2 textSize = ScreenManager.Font.MeasureString(ScreenManager.scriptMessage[0]);
           Vector2 textPosition = (viewportSize - textSize) / 2;

           Vector2[] tSizes = new Vector2[ScreenManager.scriptMessage.Count];
           Vector2[] tPositions = new Vector2[ScreenManager.scriptMessage.Count];

           float widest = textSize.X;
           int widestIndex = 0;
           for (int i = 0; i < ScreenManager.scriptMessage.Count; i++)
           {
               float newX = ScreenManager.Font.MeasureString(ScreenManager.scriptMessage[i]).X;


               if (newX > widest)
               {
                   widest = newX;
                   widestIndex = i;
               }
               tPositions[i] = (viewportSize - textSize) / 2;


           }



           // The background includes a border somewhat larger than the text itself.
           const int hPad = 32;
           const int vPad = 16;

           Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                         (int)textPosition.Y - vPad,
                                                         (int)textSize.X + hPad * 2,
                                                         (int)textSize.Y + vPad * 2);

           backgroundRectangle = new Rectangle((int)tPositions[widestIndex].X - hPad,
               (int)tPositions[0].Y - vPad, (int)widest + hPad * 2, (int)textSize.Y * tPositions.Length + vPad * 2);
           Vector3 projectedVec = Vector3.Zero;
           ScreenManager.SpriteBatch.Begin();
           // Draw the background rectangle.
           ScreenManager.SpriteBatch.Draw(ScreenManager.gradient, backgroundRectangle, Color.White);

           // Draw the message box text.
           for (int i = 0; i < tPositions.Length; i++)
           {
               ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, ScreenManager.scriptMessage[i], new Vector2(tPositions[i].X, tPositions[i].Y + textSize.Y * i + 2), Color.Blue);
           }

           ScreenManager.SpriteBatch.End();
       }

       public void DrawMino(Minotaur mino)
       {

           Matrix[] transforms = new Matrix[ScreenManager.minotaur.Bones.Count];
           ScreenManager.minotaur.CopyAbsoluteBoneTransformsTo(transforms);
           foreach (ModelMesh mesh in ScreenManager.minotaur.Meshes)
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
                   effect.Parameters["Texture"].SetValue(ScreenManager.minoTex);
                   effect.Parameters["Bones"].SetValue(mino.SkinTrans);
                   effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);

               }
               if (mesh.Name == "minotaur")
                   mesh.Draw();
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

           DrawMino(ScreenManager.p1Mino);

           // Set render target back to the back buffer
           ScreenManager.GraphicsDevice.SetRenderTarget(null);
       }

    }
}
