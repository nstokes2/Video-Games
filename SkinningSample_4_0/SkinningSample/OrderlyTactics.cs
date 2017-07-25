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

    public class OrderlyTactics : GameScreen
    {
        /// <summary>
        /// N is north W is west S is south E is east
        /// </summary>
        public char side;
        /// <summary>
        /// 
        /// </summary>
        public bool firstRun = true;
        public int formInt = 0;
        public static List<bool> openStrikes;
        public static List<List<boundingSphere>> strikeSpheres;
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
   

        public OrderlyTactics()
        {
           
            openStrikes = new List<bool>();
            strikeSpheres = new List<List<boundingSphere>>();

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

        void CreateShadowMap()
        {

            ScreenManager.GraphicsDevice.SetRenderTarget(ScreenManager.shadowRenderTarget);
            ScreenManager.GraphicsDevice.Clear(Color.White);
            ScreenManager.lightViewProjection = ScreenManager.CreateLightViewProjectionMatrix();
            //Matrix[] transforms = new Matrix[ScreenManager.spearMan.Bones.Count];
            //ScreenManager.spearMan.CopyAbsoluteBoneTransformsTo(transforms);
            //foreach (ModelMesh mesh in ScreenManager.spearMan.Meshes)
            //{
            //    foreach (Effect effect in mesh.Effects)
            //    {
            //        effect.CurrentTechnique = effect.Techniques["CreateShadowMap"];
            //        effect.Parameters["Bones"].SetValue(ScreenManager.ps1.SkinTrans);
            //        effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
            //        effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);


            //    }
            //    //if (draw)
            //    mesh.Draw();
            //}
            DrawSpear(ScreenManager.ps1, "CreateShadowMap");
            DrawSpearAi(ScreenManager.es1, "CreateShadowMap");
            DrawSword(ScreenManager.ess1, "CreateShadowMap");

            DrawArcher(ScreenManager.a1, "CreateShadowMap");

           // DrawThor(ScreenManager.thor1, "CreateShadowMap");

            // Set render target back to the back buffer
            ScreenManager.GraphicsDevice.SetRenderTarget(null);
     
        }



        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            //ScreenManager.es1.

            if (!firstRun)
            {
                checkBodyCollisions();
                checkCollisions();
                checkCollisionsSwords();
                checkCollisionsArcher();
                checkCollisionProjectile(ScreenManager.Theseus, ScreenManager.jSp1);
            }
            firstRun = false;
            ScreenManager.open[1][0] = false;
            ScreenManager.open[2][0] = false;
            ScreenManager.open[1][1] = false;

            //ScreenManager.Alecto.UpdateEngineer(gameTime);
            ScreenManager.Theseus.UpdateTheseus(gameTime);
            ScreenManager.Cupid.UpdateCupid(gameTime);
            //ScreenManager.ps1.UpdatePlayer(gameTime);
            //ScreenManager.eng1.UpdatePlayer(gameTime);
            //ScreenManager.es1.UpdateLeader(gameTime);
            ScreenManager.jSp1.UpdateJSpearL(gameTime);
            ScreenManager.a1.UpdateLeader(gameTime);

            ScreenManager.runner1.PathingUpdate1(gameTime);

    
            foreach (JuneXnaModel juney in ScreenManager.enemyRunners)
            {


                juney.UpdateEnemyRunner(gameTime);
               
            }

            for (int i = 0; i < ScreenManager.enemyRunners.Count; i++)
            {
                if (ScreenManager.enemyRunners[i].successRun)
                    ScreenManager.enemyRunners.RemoveAt(i);

            }


            
           

            foreach (JuneXnaModel juney in ScreenManager.playerTowers)
                juney.UpdateSword(gameTime);



            //ScreenManager.thor1.UpdateLeader(gameTime);
          //  if (ScreenManager.ess1.state == 0)

                ScreenManager.ess1.UpdateLeader(gameTime);
           // else if (ScreenManager.ess1.state == 1)
             //   ScreenManager.ess1.UpdateFollower(gameTime);

            //foreach (SpearMan spear in ScreenManager.enemySpears)
            //{
            //    if(spear.state == 1)
            //        spear.UpdateLeader(gameTime);

            //    //if (spear.state == 1)
            //    //   spear.UpdateFollower(gameTime);

            //}
            ScreenManager.es2.UpdateLeader(gameTime);
            ScreenManager.es3.UpdateLeader(gameTime);


            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
        void DrawBoxes()
        {


            ScreenManager.primitiveBatch.Begin(PrimitiveType.TriangleList);
            foreach (BoundingBox[] theBoxes in ScreenManager.openBoxes)
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

        public void checkBodyCollisions()
        {

            if(ScreenManager.ess1.collisionS[0].BS.Intersects(ScreenManager.es1.collisionS[0].BS))
            {
                //Console.WriteLine("needtobounce");

                //ScreenManager.ess1.formVec = ScreenManager.es1.CFormation[ScreenManager.ess1.charNum];
               // ScreenManager.ess1.state = 1;

                //ScreenManager.ess1.state = 1;
                //ScreenManager.es1.state = 1; 

            }
            if (ScreenManager.ess1.collisionS[0].BS.Intersects(ScreenManager.a1.collisionS[0].BS))
            {

                //ScreenManager.ess1.state = 1;
                //ScreenManager.a1.state = 1;

            }
            if (ScreenManager.es1.collisionS[0].BS.Intersects(ScreenManager.a1.collisionS[0].BS))
            {

                //ScreenManager.es1.state = 1;
                //ScreenManager.a1.state = 1;

            }


            
            //foreach (SpearMan ses in ScreenManager.enemySpears)
            //{
            //    if (ses.collisionS[0].BS.Intersects(ScreenManager.es1.collisionS[0].BS))
            //    {
            //        ses.state = 1;

            //        ses.formVec = ScreenManager.es1.CFormation[ScreenManager.es1.charNum];

            //    }
            //    if (ses.collisionS[0].BS.Intersects(ScreenManager.ess1.collisionS[0].BS))
            //    {
            //        ses.state = 1;

            //        ses.formVec = ScreenManager.es1.CFormation[ScreenManager.es1.charNum];

            //    }

            //}


            formInt = 0; 

        }
        public void checkCollisionsArcher()
        {
             if (ScreenManager.a1.openStrike)
                foreach (boundingSphere bs in ScreenManager.a1.rBow)
                {
                    if (!ScreenManager.ps1.shieldEngaged)
                    {
                        foreach (boundingSphere pbs in ScreenManager.ps1.spheres)
                            if (bs.BS.Intersects(pbs.BS))
                            {
                                ScreenManager.ps1.struck = true;
                                ScreenManager.a1.openStrike = false;
                            }
                    }


                }
             if (ScreenManager.ps1.openStrike)
             {
                 foreach (boundingSphere bs in ScreenManager.ps1.spheres)
                 {
                     if (!ScreenManager.a1.shieldEngaged)
                     {
                         foreach (boundingSphere ebs in ScreenManager.a1.spheres)
                             if (bs.BS.Intersects(ebs.BS))
                             {
                                 ScreenManager.a1.isKnocked = true;

                                 ScreenManager.a1.hearts -= 1;
                                 ScreenManager.ps1.openStrike = false;
                             }
                     }



                 }
             }





        }
        public void checkCollisionsSwords()
        {


            if (ScreenManager.ess1.openStrike)
                foreach (boundingSphere bs in ScreenManager.ess1.rSword)
                {
                    if (!ScreenManager.ps1.shieldEngaged)
                    {
                        foreach (boundingSphere pbs in ScreenManager.ps1.spheres)
                            if (bs.BS.Intersects(pbs.BS))
                            {
                                ScreenManager.ps1.struck = true;
                                ScreenManager.es1.openStrike = false;
                                Console.WriteLine("sword hurts player");
                            }
                    }


                }
            if (ScreenManager.ps1.openStrike)
            {
                foreach (boundingSphere bs in ScreenManager.ps1.rSpear)
                {
                    if (!ScreenManager.ess1.shieldEngaged)
                    {
                        foreach (boundingSphere ebs in ScreenManager.ess1.spheres)
                            if (bs.BS.Intersects(ebs.BS))
                            {
                                ScreenManager.ess1.isKnocked = true;

                                ScreenManager.ess1.hearts -= 1;
                                ScreenManager.ps1.openStrike = false;
                            }
                    }



                }
            }

        }
        public void checkCollisionProjectile(JuneXnaModel actor, JuneXnaModel target)
        {

            foreach(Projectile2 proj in actor.projectiles)
            {
                foreach(boundingSphere bs in target.spheres)
                    if (proj.BS.Contains(bs.BS) != ContainmentType.Disjoint)
                    {
                        Console.WriteLine("hit");
                        proj.alive = false;
                        target.health -= 1;



                    }


            }




        }
        public void checkCollisions()
        {
            if(ScreenManager.jSp1.isAtk1)
                foreach (boundingSphere bs in ScreenManager.jSp1.rSpear)
                {
                   
                    {
                        foreach (boundingSphere pbs in ScreenManager.Theseus.spheres)
                            if (bs.BS.Contains(pbs.BS) != ContainmentType.Disjoint)
                            {
                               // ScreenManager.Theseus.isHurt = true;
                               // ScreenManager.Theseus.currentAnimationTime = TimeSpan.Zero;
                                ScreenManager.es1.openStrike = false;
                            }
                    }


                }
            if (ScreenManager.Theseus.isShieldBash)
            {
                foreach (boundingSphere bs in ScreenManager.Theseus.roundShield)
                {
                    if (!ScreenManager.jSp1.isShield)
                    {
                        foreach (boundingSphere ebs in ScreenManager.jSp1.spheres)
                            if (bs.BS.Intersects(ebs.BS))
                            {
                                //ScreenManager.jSp1.isHurt = true;
                                ScreenManager.jSp1.currentAnimationTime = TimeSpan.Zero;
                                //ScreenManager.es1.hearts-=1;
                                //ScreenManager.ps1.openStrike = false;
                            }
                    }
                }
            }





        }
        
        public override void Draw(GameTime gameTime)
        {

            ScreenManager.lightViewProjection = ScreenManager.CreateLightViewProjectionMatrix();
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
            ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;

            float time = gameTime.TotalGameTime.Seconds * .3f;
            CreateShadowMap();

            ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            DrawJSpear(ScreenManager.jSp1, "SkinnedEffect");
            //DrawEngineer(ScreenManager.Alecto, "SkinnedEffect");
            DrawTheseus(ScreenManager.Theseus);
            DrawTheseus(ScreenManager.Cupid);
            //DrawSpear(ScreenManager.ps1, "SkinnedEffect");
           //DrawSpearAi(ScreenManager.es1, "SkinnedEffect");
           // DrawSword(ScreenManager.ess1, "SkinnedEffect");
           // DrawArcher(ScreenManager.a1, "SkinnedEffect");

           // DrawJSpear(ScreenManager.runner1, "SkinnedEffect");

            //foreach (JuneXnaModel juney in ScreenManager.playerTowers)
             //   DrawEngineer(juney, "SkinnedEffect");

            foreach(JuneXnaModel juney in ScreenManager.enemyRunners)
            DrawJSpear(juney, "SkinnedEffect");



            //DrawMichael(ScreenManager.Michael, "SkinnedEffect");
            //DrawEngineer(ScreenManager.eng1, "SkinnedEffect");
            //DrawBoard(gameTime);
            //DrawTdMap(gameTime);
            Matrix[] transforms;

            transforms = new Matrix[ScreenManager.build.Bones.Count];
            ScreenManager.build.CopyAbsoluteBoneTransformsTo(transforms);
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 6; j++)
                {
                    foreach (ModelMesh mesh in ScreenManager.build.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(i * 100, 0.0f, j * 100);
                            effect.View = ScreenManager.camera.View;
                            effect.Projection = ScreenManager.camera.Projection;
                            if(ScreenManager.pathBoard[i][j] == true)
                                effect.DiffuseColor = Vector3.One;
                            //if ( ScreenManager. board[i][j] == true)
                            // effect.DiffuseColor = Vector3.One;
                             else
                            effect.DiffuseColor = Vector3.Zero;

                            if (ScreenManager.runner1.uiPath.start.X == i && ScreenManager.runner1.uiPath.start.Y== j)
                                effect.DiffuseColor = new Vector3(0, 1, 0);
                            if (ScreenManager.runner1.uiPath.end.X == i && ScreenManager.runner1.uiPath.end.Y == j)
                                effect.DiffuseColor = new Vector3(0, 1, 0);

                        }
                        if(mesh.Name == "Floor")
                        mesh.Draw();
                    }
                }

            foreach (SearchNode node in ScreenManager.runner1.uiPath.openList)
            {
                foreach (ModelMesh mesh in ScreenManager.square.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(node.Position.X * 100, 0.0f, node.Position.Y * 100);
                        effect.View = ScreenManager.camera.View;
                        effect.Projection = ScreenManager.camera.Projection;
                        effect.DiffuseColor = new Vector3(0.0f, 0.0f, 1.0f);
                    }
                    mesh.Draw();
                }

            }
            foreach (SearchNode node in ScreenManager.runner1.uiPath.closedList)
            {
                foreach (ModelMesh mesh in ScreenManager.square.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(node.Position.X * 100, 0.0f, node.Position.Y * 100);
                        effect.View = ScreenManager.camera.View;
                        effect.Projection = ScreenManager.camera.Projection;
                        effect.DiffuseColor = new Vector3(1.0f, 0.0f, 0.0f);
                    }
                    mesh.Draw();
                }
            }

            //transforms = new Matrix[ScreenManager.grassSquare.Bones.Count];
            //ScreenManager.grassSquare.CopyAbsoluteBoneTransformsTo(transforms);
            //foreach (ModelMesh mesh in ScreenManager.grassSquare.Meshes)
            //{
            //    foreach (AlphaTestEffect effect in mesh.Effects)
            //    {
            //        effect.World = transforms[mesh.ParentBone.Index];
            //        effect.View = ScreenManager.camera.View;
            //        effect.Projection = ScreenManager.camera.Projection;
            //        effect.Texture = ScreenManager.grass;




            //    }
            //    mesh.Draw();


            //}
            transforms = new Matrix[ScreenManager.newBoard.Bones.Count];
            ScreenManager.newBoard.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.newBoard.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = transforms[mesh.ParentBone.Index];
                    effect.View = ScreenManager.camera.View;
                    effect.Projection = ScreenManager.camera.Projection;
                    effect.DiffuseColor = Color.Red.ToVector3();
                    effect.EnableDefaultLighting();
                }

              //  mesh.Draw();
            }

            transforms  = new Matrix[ScreenManager.finalBuild.Bones.Count];
            ScreenManager.finalBuild.CopyAbsoluteBoneTransformsTo(transforms);

            ModelMesh pylon = ScreenManager.finalBuild.Meshes["Pylon"];
            ModelMesh minotaur = ScreenManager.finalBuild.Meshes["minotaur"];
            ModelMesh rubble = ScreenManager.finalBuild.Meshes["Rubble"];
            for (int i = 0; i < 6; i++)
            {
                
                if (ScreenManager.lvl1[0][i] == "P")
                {
             
                foreach (BasicEffect effect in pylon.Effects)
                {
                    effect.World = transforms[pylon.ParentBone.Index] * Matrix.CreateTranslation(new Vector3( i * 100.0f, 0.0f, 0.0f));
                    effect.View = ScreenManager.camera.View;
                    effect.Projection = ScreenManager.camera.Projection;
                    effect.DiffuseColor = Color.Red.ToVector3();
                    effect.EnableDefaultLighting();
                }

                pylon.Draw();
            }
          }
            for (int i = 0; i < 6; i++)
            {

                if (ScreenManager.lvl1[0][i] == "M")
                {

                    foreach (BasicEffect effect in minotaur.Effects)
                    {
                        effect.World = transforms[minotaur.ParentBone.Index] * Matrix.CreateTranslation(new Vector3(i * 100.0f, 0.0f, 0.0f));
                        effect.View = ScreenManager.camera.View;
                        effect.Projection = ScreenManager.camera.Projection;
                        effect.DiffuseColor = Color.Red.ToVector3();
                        effect.EnableDefaultLighting();
                    }

                    minotaur.Draw();
                }
            }
            for (int i = 0; i < 6; i++)
            {

                if (ScreenManager.lvl1[0][i] == "R")
                {

                    foreach (BasicEffect effect in rubble.Effects)
                    {
                        effect.World = transforms[rubble.ParentBone.Index] * Matrix.CreateTranslation(new Vector3(i * 100.0f, 0.0f, 0.0f));
                        effect.View = ScreenManager.camera.View;
                        effect.Projection = ScreenManager.camera.Projection;
                        effect.DiffuseColor = Color.Red.ToVector3();
                        effect.EnableDefaultLighting();
                    }

                    rubble.Draw();
                }
            }
            //ScreenManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            DrawHud();




            ScreenManager.enemyVecs.Clear();

           // DrawBoxes();
            base.Draw(gameTime);

        }
        public void DrawTheseus(JuneXnaModel player)
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
            Matrix[] tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);

            player.formation.Decompose(out scale, out rota, out trans);
            tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            foreach(Projectile2 proj in player.projectiles)

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
          

                    if(mesh.Name == "BS")
                    {

                        targetMat = proj.world;
                        targetMat.Decompose(out scale, out rota, out trans);
                        proj.BS = new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X);


                    }
                    BoundingSphereRenderer.Render(proj.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);




                }
            }


  
        }

        public void DrawThor(Thor actor, string nameEffect)
        {

            Matrix[] transforms;
            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" || mesh.Name == "TorsoPlate" || mesh.Name == "lHairF" || mesh.Name == "ponytail" || mesh.Name == "shin"
                    || mesh.Name == "Mjolnir" || mesh.Name == "GreekPants" || mesh.Name == "Scalp" || mesh.Name == "eyeball" || mesh.Name == "EyeBrow"
                    && mesh.Name != "aosterrain")
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.CurrentTechnique = effect.Techniques[nameEffect];


                        effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                        effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);

                        effect.Parameters["DiffuseColor"].SetValue(new Vector4(2.0f, 2.0f, 2.0f, 1.0f));
                        effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));


                        effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
                        effect.Parameters["DirLight0Direction"].SetValue(l0Dir);
                        effect.Parameters["DirLight0DiffuseColor"].SetValue(l0Dif);
                        effect.Parameters["DirLight0SpecularColor"].SetValue(l0Spec);
                        effect.Parameters["DirLight1Direction"].SetValue(l1Dir);
                        effect.Parameters["DirLight1DiffuseColor"].SetValue(l1Dif);
                        effect.Parameters["DirLight1SpecularColor"].SetValue(l1Spec);
                        effect.Parameters["DirLight2Direction"].SetValue(l2Dir);
                        effect.Parameters["DirLight2DiffuseColor"].SetValue(l2Dif);
                        effect.Parameters["DirLight2SpecularColor"].SetValue(l2Spec);
                        effect.Parameters["World"].SetValue(Matrix.Identity);
                        effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera.View).Translation);
                        if (mesh.Name == "Alecto")
                            effect.Parameters["Texture"].SetValue(ScreenManager.frostTex);
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.gold1);
                        effect.Parameters["Bones"].SetValue(actor.SkinTrans);
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

            Vector3 scale, trans;
            Quaternion rota;
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
                    if (mesh.Name == "rSword1" || mesh.Name == "rSword2" || mesh.Name == "rSword3")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.rSword[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.collisionS[0] = new boundingSphere("collisionS", new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X * 2.0f));


                    }
                    if (mesh.Name == "KnockBackCheck")
                    {


                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.knockBackSphere[0] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }

                    if (mesh.Name == "forwardSpell")
                    {
                        actor.World.Decompose(out scale, out rota, out trans);

                        actor.forwardSpell = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);

                    }
                    if (mesh.Name == "headS1")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.head];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));


                    }

                    if (mesh.Name == "hipS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.hips];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.lULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.lLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    //if (mesh.Name == "lFootS")
                    //{

                    //    targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rFoot];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    actor.physicalSphere[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    //}




                }

            }

           // BoundingSphereRenderer.Render(actor.collisionS[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
        }

        public void DrawArcher(Archer actor,string nameEffect)
        {

            Matrix[] transforms;
            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" || mesh.Name == "TorsoPlate" || mesh.Name == "lHairF" || mesh.Name == "ponytail" || mesh.Name == "shin"
                    || mesh.Name == "Arrow" || mesh.Name == "Bow" || mesh.Name == "GreekPants" || mesh.Name == "Scalp" || mesh.Name == "eyeball" || mesh.Name == "EyeBrow")
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.CurrentTechnique = effect.Techniques[nameEffect];

                        effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                        effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);

                        effect.Parameters["DiffuseColor"].SetValue(new Vector4(2.0f, 2.0f, 2.0f, 1.0f));
                        effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));


                        effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
                        effect.Parameters["DirLight0Direction"].SetValue(l0Dir);
                        effect.Parameters["DirLight0DiffuseColor"].SetValue(l0Dif);
                        effect.Parameters["DirLight0SpecularColor"].SetValue(l0Spec);
                        effect.Parameters["DirLight1Direction"].SetValue(l1Dir);
                        effect.Parameters["DirLight1DiffuseColor"].SetValue(l1Dif);
                        effect.Parameters["DirLight1SpecularColor"].SetValue(l1Spec);
                        effect.Parameters["DirLight2Direction"].SetValue(l2Dir);
                        effect.Parameters["DirLight2DiffuseColor"].SetValue(l2Dif);
                        effect.Parameters["DirLight2SpecularColor"].SetValue(l2Spec);
                        effect.Parameters["World"].SetValue(Matrix.Identity);
                        effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera.View).Translation);
                        if (mesh.Name == "Alecto")
                            effect.Parameters["Texture"].SetValue(ScreenManager.frostTex);
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.gold1);
                        effect.Parameters["Bones"].SetValue(actor.SkinTrans);
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
                    //if (mesh.Name == "shieldS1" || mesh.Name == "shieldS2" || mesh.Name == "shieldS3" || mesh.Name == "shieldS4" || mesh.Name == "shieldS5")
                    //{
                    //    targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.lHand];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    actor.shieldS[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    //}
                    
                    if (mesh.Name == "bowS1" || mesh.Name == "bowS2" || mesh.Name == "bowS3"
                        || mesh.Name == "bowS4" || mesh.Name == "bowS5" || mesh.Name == "bowS6")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.rBow[k++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "KnockBackCheck")
                    {


                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.knockBackSphere[0] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }

                    if (mesh.Name == "forwardSpell")
                    {
                        actor.World.Decompose(out scale, out rota, out trans);

                        actor.forwardSpell = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);




                    }
                    if (mesh.Name == "headS1")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.head];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));


                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "hipS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.hips];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.lULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.lLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }

                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.collisionS[0] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X * 2.0f));
                    }
                    //if (mesh.Name == "lFootS")
                    //{

                    //    targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rFoot];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    actor.physicalSphere[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    //}




                }

            }

           // BoundingSphereRenderer.Render(actor.collisionS[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
            //foreach (boundingSphere bs in actor.rSpear)
            //{
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);

            //}
            //foreach (boundingSphere bs in actor.knockBackSphere)
            //{
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);

            //}
            //foreach (boundingSphere bs in actor.spheres)
            //{

            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);
            //}

            transforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if (mesh.Name == "Arrow2")
                {
                    ScreenManager.a1.World.Decompose(out scale, out rota, out trans);

                    //ScreenManager.ThorTVH.arrowWorld =Matrix.CreateScale(scale) *  Matrix.CreateTranslation(transforms[mesh.ParentBone.Index].Translation) * ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];// *ScreenManager.ThorTVH.World;
                    // ScreenManager.ThorTVH.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.ThorTVH.World.Translation), rota);
                    ScreenManager.a1.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);// *ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];
                    //ScreenManager.ParisTS.arrowWorld = transforms[mesh.ParentBone.Index] * ScreenManager.ParisTS.SkinTrans[ScreenManager.rHand];

                }

            }


            if (nameEffect != "CreateShadowMap")
            {
                foreach (Projectile2 proj in ScreenManager.a1.projectiles)
                {
                    foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
                    {
                        if (mesh.Name == "Arrow2")
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
        public void DrawMichael(JuneXnaModel player, string nameEffect )
        {


            Matrix[] transforms;

            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" || mesh.Name == "TorsoPlate" || mesh.Name == "Helmet" || mesh.Name == "ponytail" || mesh.Name == "shin"
                    || mesh.Name == "RSword" || mesh.Name == "RoundShield" || mesh.Name == "Cylinder01" || mesh.Name == "Scalp" || mesh.Name == "eyeball" || mesh.Name == "EyeBrow"
                    || mesh.Name == "wings")
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.CurrentTechnique = effect.Techniques[nameEffect];

                        effect.Parameters["DiffuseColor"].SetValue(new Vector4(2.0f, 2.0f, 2.0f, 1.0f));
                        effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));


                        effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
                        effect.Parameters["DirLight0Direction"].SetValue(l0Dir);
                        effect.Parameters["DirLight0DiffuseColor"].SetValue(l0Dif);
                        effect.Parameters["DirLight0SpecularColor"].SetValue(l0Spec);
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
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.gold1);
                        effect.Parameters["Bones"].SetValue(player.SkinTrans);
                        effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);

                        if (mesh.Name == "sHair")
                            effect.Parameters["Texture"].SetValue(ScreenManager.white);
                        if (mesh.Name == "longHairF")
                            effect.Parameters["Texture"].SetValue(ScreenManager.white);
                        if (mesh.Name == "longHairB")
                            effect.Parameters["Texture"].SetValue(ScreenManager.white);

                    }

                    mesh.Draw();
                }
            }

            Matrix world = new Matrix();
            Vector3 scale, trans;
            Quaternion rota;

            player.formation.Decompose(out scale, out rota, out trans);
            Matrix[] tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if ( mesh.Name == "LeaderStar")
                {

                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);
                        effect.View = ScreenManager.camera.View;
                        effect.Projection = ScreenManager.camera.Projection;
                        effect.DiffuseColor = Color.Black.ToVector3();
                        effect.TextureEnabled = false;
                    }
                    mesh.Draw();
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
                    if (mesh.Name == "KnockBackCheck")
                    {


                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.knockBackSphere[0] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rPunchS")// || mesh.Name == "rSpearS2" || mesh.Name == "rSpearS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.physicalSphere[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "lPunchS")// || mesh.Name == "rSpearS2" || mesh.Name == "rSpearS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.physicalSphere[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

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
                    if (mesh.Name == "lFootS")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rFoot];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.physicalSphere[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }




                }

            }
            foreach (boundingSphere bs in player.rSword)
                BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);
            foreach (boundingSphere bs in player.knockBackSphere)
            {
                BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Violet);

            }
            foreach (boundingSphere bs in player.spheres)
            {
                BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Violet);

            }

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

                        effect.DiffuseColor = Color.Azure.ToVector3();


                    if (mesh.Name == "N0,0" || mesh.Name == "N0,1" || mesh.Name == "N0,2"
                    || mesh.Name == "N1,0" || mesh.Name == "N1,1" || mesh.Name == "N1,2"
                        || mesh.Name == "N2,0" || mesh.Name == "N2,1" || mesh.Name == "N2,2")
                        effect.DiffuseColor = Color.Azure.ToVector3();


                    if (mesh.Name == "E0,0" || mesh.Name == "E0,1" || mesh.Name == "E0,2"
 || mesh.Name == "E1,0" || mesh.Name == "E1,1" || mesh.Name == "E1,2"
 || mesh.Name == "E2,0" || mesh.Name == "E2,1" || mesh.Name == "E2,2")
                        effect.DiffuseColor = Color.Azure.ToVector3();

                    if (mesh.Name == "W0,0" || mesh.Name == "W0,1" || mesh.Name == "W0,2"
 || mesh.Name == "W1,0" || mesh.Name == "W1,1" || mesh.Name == "W1,2"
 || mesh.Name == "W2,0" || mesh.Name == "W2,1" || mesh.Name == "W2,2")
                        effect.DiffuseColor = Color.Azure.ToVector3();

                }

                mesh.Draw();
            }

        }
        public void DrawSword(SwordAndShield actor, string nameEffect)
        {

            Matrix[] transforms;
            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" || mesh.Name == "TorsoPlate" || mesh.Name == "lHairF" || mesh.Name == "ponytail" || mesh.Name == "shin"
                    || mesh.Name == "RoundShield" || mesh.Name == "RSword" || mesh.Name == "GreekPants" || mesh.Name == "Scalp" || mesh.Name == "eyeball" || mesh.Name == "EyeBrow")
                {
                    foreach (Effect effect in mesh.Effects)
                    {

                        effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                        effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);

                        effect.CurrentTechnique = effect.Techniques[nameEffect];

                        effect.Parameters["DiffuseColor"].SetValue(new Vector4(2.0f, 2.0f, 2.0f, 1.0f));
                        effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));


                        effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
                        effect.Parameters["DirLight0Direction"].SetValue(l0Dir);
                        effect.Parameters["DirLight0DiffuseColor"].SetValue(l0Dif);
                        effect.Parameters["DirLight0SpecularColor"].SetValue(l0Spec);
                        effect.Parameters["DirLight1Direction"].SetValue(l1Dir);
                        effect.Parameters["DirLight1DiffuseColor"].SetValue(l1Dif);
                        effect.Parameters["DirLight1SpecularColor"].SetValue(l1Spec);
                        effect.Parameters["DirLight2Direction"].SetValue(l2Dir);
                        effect.Parameters["DirLight2DiffuseColor"].SetValue(l2Dif);
                        effect.Parameters["DirLight2SpecularColor"].SetValue(l2Spec);
                        effect.Parameters["World"].SetValue(Matrix.Identity);
                        effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera.View).Translation);
                        if (mesh.Name == "Alecto")
                            effect.Parameters["Texture"].SetValue(ScreenManager.frostTex);
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.gold1);
                        effect.Parameters["Bones"].SetValue(actor.SkinTrans);
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
                    if (mesh.Name == "shieldS1" || mesh.Name == "shieldS2" || mesh.Name == "shieldS3" || mesh.Name == "shieldS4" || mesh.Name == "shieldS5")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.lHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.shieldS[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rSwordS1" || mesh.Name == "rSwordS2" || mesh.Name == "rSwordS3")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.rSword[k++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "KnockBackCheck")
                    {


                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.knockBackSphere[0] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }

                    if (mesh.Name == "forwardSpell")
                    {
                        actor.World.Decompose(out scale, out rota, out trans);

                        actor.forwardSpell = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);




                    }
                    if (mesh.Name == "headS1")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.head];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));


                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "hipS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.hips];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.lULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.lLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }

                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.collisionS[0] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X * 2.0f));
                    }
                    //if (mesh.Name == "lFootS")
                    //{

                    //    targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rFoot];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    actor.physicalSphere[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    //}




                }

            }

            //BoundingSphereRenderer.Render(actor.collisionS[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
            //foreach (boundingSphere bs in actor.rSpear)
            //{
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);

            //}
            //foreach (boundingSphere bs in actor.knockBackSphere)
            //{
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);

            //}
            //foreach (boundingSphere bs in actor.spheres)
            //{

            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);
            //}


        }
        public void DrawJSpear(JuneXnaModel actor, string nameEffect)
        {
           // ScreenManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            Matrix[] transforms;
            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" || mesh.Name == "TorsoPlate" || mesh.Name == "lHairF" || mesh.Name == "ponytail" || mesh.Name == "shin"
                    || mesh.Name == "LSpear" || mesh.Name == "RSpear" || mesh.Name == "GreekPants" || mesh.Name == "Scalp" || mesh.Name == "eyeball" || mesh.Name == "EyeBrow"
                    && mesh.Name != "aosterrain")
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.CurrentTechnique = effect.Techniques[nameEffect];

                        effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                        effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);

                        effect.Parameters["DiffuseColor"].SetValue(new Vector4(2.0f, 2.0f, 2.0f, 1.0f));
                        effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));


                        effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
                        effect.Parameters["DirLight0Direction"].SetValue(l0Dir);
                        effect.Parameters["DirLight0DiffuseColor"].SetValue(l0Dif);
                        effect.Parameters["DirLight0SpecularColor"].SetValue(l0Spec);
                        effect.Parameters["DirLight1Direction"].SetValue(l1Dir);
                        effect.Parameters["DirLight1DiffuseColor"].SetValue(l1Dif);
                        effect.Parameters["DirLight1SpecularColor"].SetValue(l1Spec);
                        effect.Parameters["DirLight2Direction"].SetValue(l2Dir);
                        effect.Parameters["DirLight2DiffuseColor"].SetValue(l2Dif);
                        effect.Parameters["DirLight2SpecularColor"].SetValue(l2Spec);
                        effect.Parameters["World"].SetValue(Matrix.Identity);
                        effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera.View).Translation);
                        if (mesh.Name == "Alecto")
                            effect.Parameters["Texture"].SetValue(ScreenManager.frostTex);
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.chain);
                        effect.Parameters["Bones"].SetValue(actor.SkinTrans);
                        effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);

                        if (mesh.Name == "sHair")
                            effect.Parameters["Texture"].SetValue(ScreenManager.black);
                        if (mesh.Name == "longHairF")
                            effect.Parameters["Texture"].SetValue(ScreenManager.black);
                        if (mesh.Name == "longHairB")
                            effect.Parameters["Texture"].SetValue(ScreenManager.black);

                    }

                    mesh.Draw();
                }
            }

            Vector3 scale, trans;
            Quaternion rota;
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
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.rSpear[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                        if (mesh.Name == "rSpearS1")
                            actor.transRayStart = trans;
                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                        actor.transRayEnd = trans;
                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.collisionS[0] = new boundingSphere("collisionS", new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X * 2.0f));


                    }
                    if (mesh.Name == "KnockBackCheck")
                    {


                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.knockBackSphere[0] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }

                    if (mesh.Name == "forwardSpell")
                    {
                        actor.World.Decompose(out scale, out rota, out trans);

                        actor.forwardSpell = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);

                    }
                    if (mesh.Name == "headS1")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.head];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));


                    }

                    if (mesh.Name == "hipS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.hips];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.lULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.lLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
  
                    }
                    if (mesh.Name == "rULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    //if (mesh.Name == "lFootS")
                    //{

                    //    targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rFoot];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    actor.physicalSphere[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    //}




                }

            }
            BoundingSphereRenderer.Render(actor.knockBackSphere[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
             BoundingSphereRenderer.Render(actor.collisionS[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
            foreach(boundingSphere bs in actor.rSpear)
                 BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
             RayRenderer.Render(actor.ray,200, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Black);

             ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);
             foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
             {
                 if (mesh.Name == "LSpear2")
                 {
                     actor.World.Decompose(out scale, out rota, out trans);

                     //ScreenManager.ThorTVH.arrowWorld =Matrix.CreateScale(scale) *  Matrix.CreateTranslation(transforms[mesh.ParentBone.Index].Translation) * ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];// *ScreenManager.ThorTVH.World;
                     // ScreenManager.ThorTVH.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.ThorTVH.World.Translation), rota);
                     actor.spearWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);// *ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];
                 }
   


             }

             foreach (Projectile2 proj in actor.projectiles)
             {
                 foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
                 {
                     if ((mesh.Name == "LSpear2" && proj.Name == "Spear"))
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
                         proj.BS = new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X);


                     }
                     BoundingSphereRenderer.Render(proj.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);




                 }
             }
           //  ScreenManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        }
        public void DrawSpearAi(SpearMan actor, string nameEffect)
        {

            Matrix[] transforms;
            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" || mesh.Name == "TorsoPlate" || mesh.Name == "lHairF" || mesh.Name == "ponytail" || mesh.Name == "shin"
                    || mesh.Name == "LSpear" || mesh.Name == "RSpear" || mesh.Name == "GreekPants" || mesh.Name == "Scalp" || mesh.Name == "eyeball" || mesh.Name == "EyeBrow"
                    && mesh.Name != "aosterrain")
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.CurrentTechnique = effect.Techniques[nameEffect];

                        effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                        effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);

                        effect.Parameters["DiffuseColor"].SetValue(new Vector4(2.0f, 2.0f, 2.0f, 1.0f));
                        effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));


                        effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
                        effect.Parameters["DirLight0Direction"].SetValue(l0Dir);
                        effect.Parameters["DirLight0DiffuseColor"].SetValue(l0Dif);
                        effect.Parameters["DirLight0SpecularColor"].SetValue(l0Spec);
                        effect.Parameters["DirLight1Direction"].SetValue(l1Dir);
                        effect.Parameters["DirLight1DiffuseColor"].SetValue(l1Dif);
                        effect.Parameters["DirLight1SpecularColor"].SetValue(l1Spec);
                        effect.Parameters["DirLight2Direction"].SetValue(l2Dir);
                        effect.Parameters["DirLight2DiffuseColor"].SetValue(l2Dif);
                        effect.Parameters["DirLight2SpecularColor"].SetValue(l2Spec);
                        effect.Parameters["World"].SetValue(Matrix.Identity);
                        effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera.View).Translation);
                        if (mesh.Name == "Alecto")
                            effect.Parameters["Texture"].SetValue(ScreenManager.frostTex);
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.gold1);
                        effect.Parameters["Bones"].SetValue(actor.SkinTrans);
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

            Vector3 scale, trans;
            Quaternion rota;
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
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.rSpear[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.collisionS[0] = new boundingSphere("collisionS", new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X * 2.0f));


                    }
                    if (mesh.Name == "KnockBackCheck")
                    {


                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.knockBackSphere[0] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }

                    if (mesh.Name == "forwardSpell")
                    {
                        actor.World.Decompose(out scale, out rota, out trans);

                        actor.forwardSpell = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);

                    }
                    if (mesh.Name == "headS1")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.head];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));


                    }

                    if (mesh.Name == "hipS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.hips];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.lULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.lLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    //if (mesh.Name == "lFootS")
                    //{

                    //    targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rFoot];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    actor.physicalSphere[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    //}




                }

            }

           // BoundingSphereRenderer.Render(actor.collisionS[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
        }
        public void DrawSpearShadow(SpearMan actor)
        {

            Matrix[] transforms;
            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" || mesh.Name == "TorsoPlate" || mesh.Name == "lHairF" || mesh.Name == "ponytail" || mesh.Name == "shin"
                    || mesh.Name == "LSpear" || mesh.Name == "RSpear" || mesh.Name == "GreekPants" ||
                    mesh.Name == "Scalp" || mesh.Name == "eyeball" || mesh.Name == "EyeBrow"
                    )
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.CurrentTechnique = effect.Techniques["CreateShadowMap"];

                        effect.Parameters["DiffuseColor"].SetValue(new Vector4(2.0f, 2.0f, 2.0f, 1.0f));
                        effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));


                        effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
                        effect.Parameters["DirLight0Direction"].SetValue(l0Dir);
                        effect.Parameters["DirLight0DiffuseColor"].SetValue(l0Dif);
                        effect.Parameters["DirLight0SpecularColor"].SetValue(l0Spec);
                        effect.Parameters["DirLight1Direction"].SetValue(l1Dir);
                        effect.Parameters["DirLight1DiffuseColor"].SetValue(l1Dif);
                        effect.Parameters["DirLight1SpecularColor"].SetValue(l1Spec);
                        effect.Parameters["DirLight2Direction"].SetValue(l2Dir);
                        effect.Parameters["DirLight2DiffuseColor"].SetValue(l2Dif);
                        effect.Parameters["DirLight2SpecularColor"].SetValue(l2Spec);
                        effect.Parameters["World"].SetValue(Matrix.Identity);
                        effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera.View).Translation);
                        if (mesh.Name == "Alecto")
                            effect.Parameters["Texture"].SetValue(ScreenManager.frostTex);
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.gold1);
                        effect.Parameters["Bones"].SetValue(actor.SkinTrans);
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

            Vector3 scale, trans;
            Quaternion rota;
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
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.rSpear[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.collisionS[0] = new boundingSphere("collisionS", new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X * 2.0f));


                    }
                    if (mesh.Name == "KnockBackCheck")
                    {


                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.knockBackSphere[0] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }

                    if (mesh.Name == "forwardSpell")
                    {
                        actor.World.Decompose(out scale, out rota, out trans);

                        actor.forwardSpell = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);

                    }
                    if (mesh.Name == "headS1")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.head];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));


                    }

                    if (mesh.Name == "hipS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.hips];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.lULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.lLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    //if (mesh.Name == "lFootS")
                    //{

                    //    targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rFoot];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    actor.physicalSphere[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    //}




                }

            }

            //  BoundingSphereRenderer.Render(actor.collisionS[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
            //foreach (boundingSphere bs in actor.rSpear)
            //{
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);

            //}
            //foreach (boundingSphere bs in actor.knockBackSphere)
            //{
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);

            //}
            //foreach (boundingSphere bs in actor.spheres)
            //{

            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);
            //}

            Matrix world = new Matrix();
            //Vector3 scale, trans;
            //Quaternion rota;

            actor.formation.Decompose(out scale, out rota, out trans);

            transforms = new Matrix[ScreenManager.humanFormation.Bones.Count];
            ScreenManager.humanFormation.CopyAbsoluteBoneTransformsTo(transforms);


            foreach (ModelMesh mesh in screenManager.humanFormation.Meshes)
            {
                world = transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(trans);

                if (mesh.Name == "0,0")
                {
                    actor.CFormation[0] = world.Translation;
                    ScreenManager.es1.formVec = world.Translation;
                }
                if (mesh.Name == "0,1")
                {
                    actor.CFormation[3] = world.Translation;
                    ScreenManager.es2.formVec = world.Translation;
                }
                if (mesh.Name == "0,2")
                {
                    actor.CFormation[6] = world.Translation;
                    ScreenManager.es3.formVec = world.Translation;
                }
                if (mesh.Name == "1,0")
                {
                    actor.CFormation[1] = world.Translation;
                    ScreenManager.ess1.formVec = world.Translation;
                }
                if (mesh.Name == "1,1")
                    actor.CFormation[4] = world.Translation;
                if (mesh.Name == "1,2")
                {
                    actor.CFormation[7] = world.Translation;

                }
                if (mesh.Name == "2,0")
                {
                    //ScreenManager.a1.formVec = world.Translation;
                    actor.CFormation[2] = world.Translation;
                }
                if (mesh.Name == "2,1")
                {
                    actor.CFormation[5] = world.Translation;
                }
                if (mesh.Name == "2,2")
                    actor.CFormation[8] = world.Translation;
                if (mesh.Name == "E0,0")
                    actor.EFormation[0] = world.Translation;
                if (mesh.Name == "E0,1")
                    actor.EFormation[3] = world.Translation;
                if (mesh.Name == "E0,2")
                    actor.EFormation[6] = world.Translation;
                if (mesh.Name == "E1,0")
                    actor.EFormation[1] = world.Translation;
                if (mesh.Name == "E1,1")
                    actor.EFormation[4] = world.Translation;
                if (mesh.Name == "E1,2")
                    actor.EFormation[7] = world.Translation;
                if (mesh.Name == "E2,0")
                    actor.EFormation[2] = world.Translation;
                if (mesh.Name == "E2,1")
                    actor.EFormation[5] = world.Translation;
                if (mesh.Name == "E2,2")
                    actor.EFormation[8] = world.Translation;
                if (mesh.Name == "W0,0")
                    actor.WFormation[0] = world.Translation;
                if (mesh.Name == "W0,1")
                    actor.WFormation[3] = world.Translation;
                if (mesh.Name == "W0,2")
                    actor.WFormation[6] = world.Translation;
                if (mesh.Name == "W1,0")
                    actor.WFormation[1] = world.Translation;
                if (mesh.Name == "W1,1")
                    actor.WFormation[4] = world.Translation;
                if (mesh.Name == "W1,2")
                    actor.WFormation[7] = world.Translation;
                if (mesh.Name == "W2,0")
                    actor.WFormation[2] = world.Translation;
                if (mesh.Name == "W2,1")
                    actor.WFormation[5] = world.Translation;
                if (mesh.Name == "W2,2")
                    actor.WFormation[8] = world.Translation;
                if (mesh.Name == "N0,0")
                    actor.NFormation[0] = world.Translation;
                if (mesh.Name == "N0,1")
                    actor.NFormation[3] = world.Translation;
                if (mesh.Name == "N0,2")
                    actor.NFormation[6] = world.Translation;
                if (mesh.Name == "N1,0")
                    actor.NFormation[1] = world.Translation;
                if (mesh.Name == "N1,1")
                    actor.NFormation[4] = world.Translation;
                if (mesh.Name == "N1,2")
                    actor.NFormation[7] = world.Translation;
                if (mesh.Name == "N2,0")
                    actor.NFormation[2] = world.Translation;
                if (mesh.Name == "N2,1")
                {
                    actor.NFormation[5] = world.Translation;

                    ScreenManager.a1.formVec = world.Translation;
                }
                if (mesh.Name == "N2,2")
                    actor.NFormation[8] = world.Translation;

                if (mesh.Name == "S0,0")
                    actor.SFormation[0] = world.Translation;
                if (mesh.Name == "S0,1")
                    actor.SFormation[3] = world.Translation;
                if (mesh.Name == "S0,2")
                    actor.SFormation[6] = world.Translation;
                if (mesh.Name == "S1,0")
                    actor.SFormation[1] = world.Translation;
                if (mesh.Name == "S1,1")
                    actor.SFormation[4] = world.Translation;
                if (mesh.Name == "S1,2")
                    actor.SFormation[7] = world.Translation;
                if (mesh.Name == "S2,0")
                    actor.SFormation[2] = world.Translation;
                if (mesh.Name == "S2,1")
                    actor.SFormation[5] = world.Translation;
                if (mesh.Name == "S2,2")
                    actor.SFormation[8] = world.Translation;

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


        }
        public void DrawSpear(SpearMan actor, string nameEffect)
        {

            Matrix[] transforms;
            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" || mesh.Name == "TorsoPlate" || mesh.Name == "lHairF" || mesh.Name == "ponytail" || mesh.Name == "shin"
                    || mesh.Name == "LSpear" || mesh.Name == "RSpear" || mesh.Name == "GreekPants" ||
                    mesh.Name == "Scalp" || mesh.Name == "eyeball" || mesh.Name == "EyeBrow"
                    )
                {
                    foreach (Effect effect in mesh.Effects)
                    {

                        effect.CurrentTechnique = effect.Techniques[nameEffect];

                               effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                              effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);

                        effect.Parameters["DiffuseColor"].SetValue(new Vector4(2.0f, 2.0f, 2.0f, 1.0f));
                        effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));


                        effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
                        effect.Parameters["DirLight0Direction"].SetValue(l0Dir);
                        effect.Parameters["DirLight0DiffuseColor"].SetValue(l0Dif);
                        effect.Parameters["DirLight0SpecularColor"].SetValue(l0Spec);
                        effect.Parameters["DirLight1Direction"].SetValue(l1Dir);
                        effect.Parameters["DirLight1DiffuseColor"].SetValue(l1Dif);
                        effect.Parameters["DirLight1SpecularColor"].SetValue(l1Spec);
                        effect.Parameters["DirLight2Direction"].SetValue(l2Dir);
                        effect.Parameters["DirLight2DiffuseColor"].SetValue(l2Dif);
                        effect.Parameters["DirLight2SpecularColor"].SetValue(l2Spec);
                        effect.Parameters["World"].SetValue(Matrix.Identity);
                        effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera.View).Translation);
                        if (mesh.Name == "Alecto")
                            effect.Parameters["Texture"].SetValue(ScreenManager.frostTex);
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.gold1);
                        effect.Parameters["Bones"].SetValue(actor.SkinTrans);
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

            Vector3 scale, trans;
            Quaternion rota;
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
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.rSpear[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "chestS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.collisionS[0] = new boundingSphere("collisionS", new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X *2.0f));


                    }
                    if (mesh.Name == "KnockBackCheck")
                    {


                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.knockBackSphere[0] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }

                    if (mesh.Name == "forwardSpell")
                    {
                        actor.World.Decompose(out scale, out rota, out trans);

                        actor.forwardSpell = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);

                    }
                    if (mesh.Name == "headS1")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.head];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));


                    }

                    if (mesh.Name == "hipS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.hips];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.lULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "lLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.lLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    if (mesh.Name == "rLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    }
                    //if (mesh.Name == "lFootS")
                    //{

                    //    targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rFoot];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    actor.physicalSphere[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    //}




                }

            }

          //  BoundingSphereRenderer.Render(actor.collisionS[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
            //foreach (boundingSphere bs in actor.rSpear)
            //{
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);

            //}
            //foreach (boundingSphere bs in actor.knockBackSphere)
            //{
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);

            //}
            //foreach (boundingSphere bs in actor.spheres)
            //{

            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);
            //}

            Matrix world = new Matrix();
            //Vector3 scale, trans;
            //Quaternion rota;

            actor.formation.Decompose(out scale, out rota, out trans);

            transforms = new Matrix[ScreenManager.humanFormation.Bones.Count];
            ScreenManager.humanFormation.CopyAbsoluteBoneTransformsTo(transforms);


            foreach (ModelMesh mesh in screenManager.humanFormation.Meshes)
            {
                world = transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(trans);

                if (mesh.Name == "0,0")
                {
                    actor.CFormation[0] = world.Translation;
                    ScreenManager.es1.formVec = world.Translation;
                }
                if (mesh.Name == "0,1")
                {
                    actor.CFormation[3] = world.Translation;
                    ScreenManager.es2.formVec = world.Translation;
                }
                if (mesh.Name == "0,2")
                {
                    actor.CFormation[6] = world.Translation;
                    ScreenManager.es3.formVec = world.Translation;
                }
                if (mesh.Name == "1,0")
                {
                    actor.CFormation[1] = world.Translation;
                    ScreenManager.ess1.formVec = world.Translation;
                }
                if (mesh.Name == "1,1")
                    actor.CFormation[4] = world.Translation;
                if (mesh.Name == "1,2")
                {
                    actor.CFormation[7] = world.Translation;

                }
                if (mesh.Name == "2,0")
                {
                    //ScreenManager.a1.formVec = world.Translation;
                    actor.CFormation[2] = world.Translation;
                }
                if (mesh.Name == "2,1")
                {
                    actor.CFormation[5] = world.Translation;
                }
                if (mesh.Name == "2,2")
                    actor.CFormation[8] = world.Translation;
                if (mesh.Name == "E0,0")
                    actor.EFormation[0] = world.Translation;
                if (mesh.Name == "E0,1")
                    actor.EFormation[3] = world.Translation;
                if (mesh.Name == "E0,2")
                    actor.EFormation[6] = world.Translation;
                if (mesh.Name == "E1,0")
                    actor.EFormation[1] = world.Translation;
                if (mesh.Name == "E1,1")
                    actor.EFormation[4] = world.Translation;
                if (mesh.Name == "E1,2")
                    actor.EFormation[7] = world.Translation;
                if (mesh.Name == "E2,0")
                    actor.EFormation[2] = world.Translation;
                if (mesh.Name == "E2,1")
                    actor.EFormation[5] = world.Translation;
                if (mesh.Name == "E2,2")
                    actor.EFormation[8] = world.Translation;
                if (mesh.Name == "W0,0")
                    actor.WFormation[0] = world.Translation;
                if (mesh.Name == "W0,1")
                    actor.WFormation[3] = world.Translation;
                if (mesh.Name == "W0,2")
                    actor.WFormation[6] = world.Translation;
                if (mesh.Name == "W1,0")
                    actor.WFormation[1] = world.Translation;
                if (mesh.Name == "W1,1")
                    actor.WFormation[4] = world.Translation;
                if (mesh.Name == "W1,2")
                    actor.WFormation[7] = world.Translation;
                if (mesh.Name == "W2,0")
                    actor.WFormation[2] = world.Translation;
                if (mesh.Name == "W2,1")
                    actor.WFormation[5] = world.Translation;
                if (mesh.Name == "W2,2")
                    actor.WFormation[8] = world.Translation;
                if (mesh.Name == "N0,0")
                    actor.NFormation[0] = world.Translation;
                if (mesh.Name == "N0,1")
                    actor.NFormation[3] = world.Translation;
                if (mesh.Name == "N0,2")
                    actor.NFormation[6] = world.Translation;
                if (mesh.Name == "N1,0")
                    actor.NFormation[1] = world.Translation;
                if (mesh.Name == "N1,1")
                    actor.NFormation[4] = world.Translation;
                if (mesh.Name == "N1,2")
                    actor.NFormation[7] = world.Translation;
                if (mesh.Name == "N2,0")
                    actor.NFormation[2] = world.Translation;
                if (mesh.Name == "N2,1")
                {
                    actor.NFormation[5] = world.Translation;

                    ScreenManager.a1.formVec = world.Translation;
                }
                if (mesh.Name == "N2,2")
                    actor.NFormation[8] = world.Translation;

                if (mesh.Name == "S0,0")
                    actor.SFormation[0] = world.Translation;
                if (mesh.Name == "S0,1")
                    actor.SFormation[3] = world.Translation;
                if (mesh.Name == "S0,2")
                    actor.SFormation[6] = world.Translation;
                if (mesh.Name == "S1,0")
                    actor.SFormation[1] = world.Translation;
                if (mesh.Name == "S1,1")
                    actor.SFormation[4] = world.Translation;
                if (mesh.Name == "S1,2")
                    actor.SFormation[7] = world.Translation;
                if (mesh.Name == "S2,0")
                    actor.SFormation[2] = world.Translation;
                if (mesh.Name == "S2,1")
                    actor.SFormation[5] = world.Translation;
                if (mesh.Name == "S2,2")
                    actor.SFormation[8] = world.Translation;

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
                        proj.BS = new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X);


                    }
                    BoundingSphereRenderer.Render(proj.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);




                }
            }

            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if (mesh.Name == "LSpear2")
                {
                    actor.World.Decompose(out scale, out rota, out trans);

                    //ScreenManager.ThorTVH.arrowWorld =Matrix.CreateScale(scale) *  Matrix.CreateTranslation(transforms[mesh.ParentBone.Index].Translation) * ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];// *ScreenManager.ThorTVH.World;
                    // ScreenManager.ThorTVH.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.ThorTVH.World.Translation), rota);
                    actor.spearWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);// *ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];
                }



            }



        }

        public void DrawEngineer(JuneXnaModel actor, string nameEffect)
        {

            Matrix[] transforms;
            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" || mesh.Name == "TorsoPlate" || mesh.Name == "lHairF" || mesh.Name == "ponytail" || mesh.Name == "shin"
                    || mesh.Name == "Mjolnir"  || mesh.Name == "GreekPants" ||
                    mesh.Name == "Scalp" || mesh.Name == "eyeball" || mesh.Name == "EyeBrow"
                    )
                {
                    foreach (Effect effect in mesh.Effects)
                    {

                        effect.CurrentTechnique = effect.Techniques[nameEffect];

                        effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                        effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);

                        effect.Parameters["DiffuseColor"].SetValue(new Vector4(2.0f, 2.0f, 2.0f, 1.0f));
                        effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));


                        effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
                        effect.Parameters["DirLight0Direction"].SetValue(l0Dir);
                        effect.Parameters["DirLight0DiffuseColor"].SetValue(l0Dif);
                        effect.Parameters["DirLight0SpecularColor"].SetValue(l0Spec);
                        effect.Parameters["DirLight1Direction"].SetValue(l1Dir);
                        effect.Parameters["DirLight1DiffuseColor"].SetValue(l1Dif);
                        effect.Parameters["DirLight1SpecularColor"].SetValue(l1Spec);
                        effect.Parameters["DirLight2Direction"].SetValue(l2Dir);
                        effect.Parameters["DirLight2DiffuseColor"].SetValue(l2Dif);
                        effect.Parameters["DirLight2SpecularColor"].SetValue(l2Spec);
                        effect.Parameters["World"].SetValue(Matrix.Identity);
                        effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera.View).Translation);
                        if (mesh.Name == "Alecto")
                            effect.Parameters["Texture"].SetValue(ScreenManager.frostTex);
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.gold1);
                        effect.Parameters["Bones"].SetValue(actor.SkinTrans);
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

            Vector3 scale, trans;
            Quaternion rota;
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
                    //if (mesh.Name == "rSpearS1" || mesh.Name == "rSpearS2" || mesh.Name == "rSpearS3")
                    //{
                    //    targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rHand];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    actor.rSpear[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    //}
                    //if (mesh.Name == "chestS")
                    //{
                    //    targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    //}
                    //if (mesh.Name == "chestS")
                    //{
                    //    targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    actor.collisionS[0] = new boundingSphere("collisionS", new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X * 2.0f));


                    //}
                    //if (mesh.Name == "KnockBackCheck")
                    //{


                    //    targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.spine1];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    actor.knockBackSphere[0] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    //}

                    //if (mesh.Name == "forwardSpell")
                    //{
                    //    actor.World.Decompose(out scale, out rota, out trans);

                    //    actor.forwardSpell = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);

                    //}
                    //if (mesh.Name == "headS1")
                    //{
                    //    targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.head];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));


                    //}

                    //if (mesh.Name == "hipS")
                    //{
                    //    targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.hips];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    //}
                    //if (mesh.Name == "lULegS")
                    //{
                    //    targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.lULeg];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    //}
                    //if (mesh.Name == "lLLegS")
                    //{
                    //    targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.lLLeg];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    //}
                    //if (mesh.Name == "rULegS")
                    //{
                    //    targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rULeg];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    //}
                    //if (mesh.Name == "rLLegS")
                    //{
                    //    targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rLLeg];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    actor.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
                    //}
                    //if (mesh.Name == "lFootS")
                    //{

                    //    targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rFoot];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    actor.physicalSphere[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    //}




                }

            }

            //  BoundingSphereRenderer.Render(actor.collisionS[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
            //foreach (boundingSphere bs in actor.rSpear)
            //{
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);

            //}
            //foreach (boundingSphere bs in actor.knockBackSphere)
            //{
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);

            //}
            //foreach (boundingSphere bs in actor.spheres)
            //{

            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);
            //}

            Matrix world = new Matrix();
            //Vector3 scale, trans;
            //Quaternion rota;

            actor.formation.Decompose(out scale, out rota, out trans);

            transforms = new Matrix[ScreenManager.humanFormation.Bones.Count];
            ScreenManager.humanFormation.CopyAbsoluteBoneTransformsTo(transforms);


            foreach (ModelMesh mesh in screenManager.humanFormation.Meshes)
            {
                world = transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(trans);

                if (mesh.Name == "0,0")
                {
                    actor.CFormation[0] = world.Translation;
                    ScreenManager.es1.formVec = world.Translation;
                }
                if (mesh.Name == "0,1")
                {
                    actor.CFormation[3] = world.Translation;
                    ScreenManager.es2.formVec = world.Translation;
                }
                if (mesh.Name == "0,2")
                {
                    actor.CFormation[6] = world.Translation;
                    ScreenManager.es3.formVec = world.Translation;
                }
                if (mesh.Name == "1,0")
                {
                    actor.CFormation[1] = world.Translation;
                    ScreenManager.ess1.formVec = world.Translation;
                }
                if (mesh.Name == "1,1")
                    actor.CFormation[4] = world.Translation;
                if (mesh.Name == "1,2")
                {
                    actor.CFormation[7] = world.Translation;

                }
                if (mesh.Name == "2,0")
                {
                    //ScreenManager.a1.formVec = world.Translation;
                    actor.CFormation[2] = world.Translation;
                }
                if (mesh.Name == "2,1")
                {
                    actor.CFormation[5] = world.Translation;
                }
                if (mesh.Name == "2,2")
                    actor.CFormation[8] = world.Translation;
                if (mesh.Name == "E0,0")
                    actor.EFormation[0] = world.Translation;
                if (mesh.Name == "E0,1")
                    actor.EFormation[3] = world.Translation;
                if (mesh.Name == "E0,2")
                    actor.EFormation[6] = world.Translation;
                if (mesh.Name == "E1,0")
                    actor.EFormation[1] = world.Translation;
                if (mesh.Name == "E1,1")
                    actor.EFormation[4] = world.Translation;
                if (mesh.Name == "E1,2")
                    actor.EFormation[7] = world.Translation;
                if (mesh.Name == "E2,0")
                    actor.EFormation[2] = world.Translation;
                if (mesh.Name == "E2,1")
                    actor.EFormation[5] = world.Translation;
                if (mesh.Name == "E2,2")
                    actor.EFormation[8] = world.Translation;
                if (mesh.Name == "W0,0")
                    actor.WFormation[0] = world.Translation;
                if (mesh.Name == "W0,1")
                    actor.WFormation[3] = world.Translation;
                if (mesh.Name == "W0,2")
                    actor.WFormation[6] = world.Translation;
                if (mesh.Name == "W1,0")
                    actor.WFormation[1] = world.Translation;
                if (mesh.Name == "W1,1")
                    actor.WFormation[4] = world.Translation;
                if (mesh.Name == "W1,2")
                    actor.WFormation[7] = world.Translation;
                if (mesh.Name == "W2,0")
                    actor.WFormation[2] = world.Translation;
                if (mesh.Name == "W2,1")
                    actor.WFormation[5] = world.Translation;
                if (mesh.Name == "W2,2")
                    actor.WFormation[8] = world.Translation;
                if (mesh.Name == "N0,0")
                    actor.NFormation[0] = world.Translation;
                if (mesh.Name == "N0,1")
                    actor.NFormation[3] = world.Translation;
                if (mesh.Name == "N0,2")
                    actor.NFormation[6] = world.Translation;
                if (mesh.Name == "N1,0")
                    actor.NFormation[1] = world.Translation;
                if (mesh.Name == "N1,1")
                    actor.NFormation[4] = world.Translation;
                if (mesh.Name == "N1,2")
                    actor.NFormation[7] = world.Translation;
                if (mesh.Name == "N2,0")
                    actor.NFormation[2] = world.Translation;
                if (mesh.Name == "N2,1")
                {
                    actor.NFormation[5] = world.Translation;

                    ScreenManager.a1.formVec = world.Translation;
                }
                if (mesh.Name == "N2,2")
                    actor.NFormation[8] = world.Translation;

                if (mesh.Name == "S0,0")
                    actor.SFormation[0] = world.Translation;
                if (mesh.Name == "S0,1")
                    actor.SFormation[3] = world.Translation;
                if (mesh.Name == "S0,2")
                    actor.SFormation[6] = world.Translation;
                if (mesh.Name == "S1,0")
                    actor.SFormation[1] = world.Translation;
                if (mesh.Name == "S1,1")
                    actor.SFormation[4] = world.Translation;
                if (mesh.Name == "S1,2")
                    actor.SFormation[7] = world.Translation;
                if (mesh.Name == "S2,0")
                    actor.SFormation[2] = world.Translation;
                if (mesh.Name == "S2,1")
                    actor.SFormation[5] = world.Translation;
                if (mesh.Name == "S2,2")
                    actor.SFormation[8] = world.Translation;

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


        }
        public void DrawHud()
        {
            int width = ScreenManager.GraphicsDevice.Viewport.Width - 20;
            int height = ScreenManager.GraphicsDevice.Viewport.Height - 20;

            Vector3 projectedVec = ScreenManager.GraphicsDevice.Viewport.Project(ScreenManager.jSp1.World.Translation, ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(ScreenManager.lilMan, new Vector2(20.0f, 30.0f), Color.White);
            ScreenManager.SpriteBatch.Draw(ScreenManager.white, new Rectangle((int)projectedVec.X,(int) projectedVec.Y - 80, (int)ScreenManager.jSp1.health * 10, 10), Color.Red);
            ScreenManager.SpriteBatch.End();
            //Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            //Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);

            //Vector3 projectedVec = ScreenManager.GraphicsDevice.Viewport.Project(ScreenManager.ps1.World.Translation, ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);
            //ScreenManager.SpriteBatch.Begin();
            //ScreenManager.SpriteBatch.Draw(ScreenManager.gradient, new Rectangle(3, 3, 100, 100), Color.White);

          //  foreach(Vector3 vec in ScreenManager.enemyVecs)
             //   ScreenManager.SpriteBatch.Draw(ScreenManager.white, new Rectangle(3, 3, 100, 100), Color.White);



            //draw current bubble
           // ScreenManager.SpriteBatch.End();
            //Vector2 textSize = ScreenManager.Font.MeasureString(ScreenManager.scriptMessage[0]);
            //Vector2 textPosition = (viewportSize - textSize) / 2;

            //Vector2[] tSizes = new Vector2[ScreenManager.scriptMessage.Count];
            //Vector2[] tPositions = new Vector2[ScreenManager.scriptMessage.Count];

            //float widest = textSize.X;
            //int widestIndex = 0;
            //for (int i = 0; i < ScreenManager.scriptMessage.Count; i++)
            //{
            //    float newX = ScreenManager.Font.MeasureString(ScreenManager.scriptMessage[i]).X;


            //    if (newX > widest)
            //    {
            //        widest = newX;
            //        widestIndex = i;
            //    }
            //    tPositions[i] = (viewportSize - textSize) / 2;


            //}



            //// The background includes a border somewhat larger than the text itself.
            //const int hPad = 32;
            //const int vPad = 16;

            //Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
            //                                              (int)textPosition.Y - vPad,
            //                                              (int)textSize.X + hPad * 2,
            //                                              (int)textSize.Y + vPad * 2);

            //backgroundRectangle = new Rectangle((int)tPositions[widestIndex].X - hPad,
            //    (int)tPositions[0].Y - vPad, (int)widest + hPad * 2, (int)textSize.Y * tPositions.Length + vPad * 2);
            //Vector3 projectedVec = Vector3.Zero;
            //ScreenManager.SpriteBatch.Begin();
            //// Draw the background rectangle.
            //ScreenManager.SpriteBatch.Draw(ScreenManager.gradient, backgroundRectangle, Color.White);

            //// Draw the message box text.
            //for (int i = 0; i < tPositions.Length; i++)
            //{
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, ScreenManager.scriptMessage[i], new Vector2(tPositions[i].X, tPositions[i].Y + textSize.Y * i + 2), Color.Blue);
            //}


        }
        public void DrawTdMap(GameTime gameTime)
        {


            Vector3 lightDirection = Vector3.Normalize(new Vector3(3, -1, 1));
            Vector3 lightColor = new Vector3(0.3f, 0.4f, 0.2f);

            // Time is scaled down to make things wave in the wind more slowly.
            float time = (float)gameTime.TotalGameTime.TotalSeconds * 0.333f;

            bool draw = false;


            Matrix[] transforms = new Matrix[ScreenManager.tdMap.Bones.Count];
            ScreenManager.tdMap.CopyAbsoluteBoneTransformsTo(transforms);
            ScreenManager.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
            foreach (ModelMesh mesh in ScreenManager.tdMap.Meshes)
            {
           
                    foreach (Effect effect in mesh.Effects)
                    {

                        //if (mesh.Name == "openGate")
                        //    effect.Parameters["Texture"].SetValue(ScreenManager.slashTga);
                        //else if (mesh.Name == "closedGate")
                        //    effect.Parameters["Texture"].SetValue(ScreenManager.fire);
                        //else
                 
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


                    
                    // if ((mesh.Name == "openGate" && ScreenManager.gateOpen) || (mesh.Name == "closedGate" & !ScreenManager.gateOpen) || (mesh.Name != "openGate" && mesh.Name != "closedGate"))

                    mesh.Draw();
                }
            }

            //foreach (boundingSphere bs in ScreenManager.creteSpheres)
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Green);



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
                    // if ((mesh.Name == "openGate" && ScreenManager.gateOpen) || (mesh.Name == "closedGate" & !ScreenManager.gateOpen) || (mesh.Name != "openGate" && mesh.Name != "closedGate"))

                    mesh.Draw();
                }
            }

            //foreach (boundingSphere bs in ScreenManager.creteSpheres)
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Green);



        }
     

    }
}
