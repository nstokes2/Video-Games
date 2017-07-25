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
    class MinoSneakAttack : GameScreen
    {
        public MinoSneakAttack()
        {

            ScreenManager.script = true;
            ScreenManager.globalInput.pause = true;
            ScreenManager.scriptMessage = new List<string>();

            ScreenManager.scriptMessage.Add("Armed with the claw of Mannalos, a right handed slash will");
            ScreenManager.scriptMessage.Add("add a debilitating poison to the bloodstream that will silence");
            ScreenManager.scriptMessage.Add("your enemies.  Try not to alert the full guard until you can");
            ScreenManager.scriptMessage.Add("get through the gates and allow the invasion to start");

        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
               bool coveredByOtherScreen)
        {


            //ScreenManager.p1June.UpdateParis(gameTime);
            //ScreenManager.eJune.UpdateTheseus(gameTime);

            if (ScreenManager.gateOpen)
            {
                ScreenManager.betweenLevels += gameTime.ElapsedGameTime;
                if(ScreenManager.storyIndex == 0)
                ScreenManager.storyIndex++;
                if (ScreenManager.betweenLevels > TimeSpan.FromSeconds(3))
                {
                   
                    ScreenManager.AddScreen(new TheseusStand(), null);
                    ExitScreen();

                }
            }

            if(!ScreenManager.globalInput.pause)
            ScreenManager.p1Mino.UpdateCrete(gameTime);



            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
    
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

            if(ScreenManager.script)
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
                 
                    if(mesh.Name == "openGate")
                        effect.Parameters["Texture"].SetValue(ScreenManager.slashTga);
                   else if(mesh.Name == "closedGate")
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
                if((mesh.Name == "openGate" && ScreenManager.gateOpen) || (mesh.Name == "closedGate" &! ScreenManager.gateOpen) || (mesh.Name != "openGate" && mesh.Name != "closedGate"))
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

        public void DrawMinoSpheres(Minotaur mino)
        {
            Matrix[] transforms = new Matrix[ScreenManager.minoSphere.Bones.Count];
            ScreenManager.minoSphere.CopyAbsoluteBoneTransformsTo(transforms);
            Matrix targetMat = new Matrix();
            foreach (ModelMesh mesh in ScreenManager.minoSphere.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (mesh.Name == "rClaw")
                        targetMat = transforms[mesh.ParentBone.Index] * ScreenManager.p1Mino.SkinTrans[ScreenManager.mrHand];
                    effect.World = targetMat;
                    effect.View = ScreenManager.camera.View;
                    effect.Projection = ScreenManager.camera.Projection;
                }

                // if (mesh.Name == "rClaw" || mesh.Name == "lClaw")
                // mesh.Draw();

            }




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
