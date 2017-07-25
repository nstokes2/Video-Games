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
    public class EternalStruggle : GameScreen
    {
    ///This is an active partial real time strategy game.  It is a chess match, but chess pieces have computer controlled AI that you can 
    ///control based upon the spheres of influence that a unit occupies
    ///

        
        BasicEffect effect;
        List<VertexPositionColor> vertices = new List<VertexPositionColor>();
        List<ushort> indices = new List<ushort>();

        List<VertexPositionTexture> vert2 = new List<VertexPositionTexture>();
        List<ushort> indices2 = new List<ushort>();

        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        VertexBuffer vertexBuffer2;
        IndexBuffer indexBuffer2;


        KeyboardState keyboardState;

        List<HSphere> hspheres = new List<HSphere>();
        HSphere hsphere1 = new HSphere(100, new Vector3( 100.0f, 70.0f, 100.0f), 30);
        HSphere hsphere2 = new HSphere(100, new Vector3(100.0f, 70.0f, 200.0f), 30);
        DRay dray1;
        DRay dray2;
        Vector2 rightThumb = Vector2.Zero;
        Lightning lightning = new Lightning(new Vector3(100.0f, 100.0f, 100.0f), new Vector3(50.0f, 100.0f, 100.0f));
        float mTimer = 0;
        List<Vector3> mCast = new List<Vector3>();
        List<Vector2> mCast2 = new List<Vector2>();
        Vector3 nearPoint = Vector3.Zero;
        int frustCount = 0; 
        Rectangle selectionBox;
        Vector2 endMPos;
        Vector2 mPosition;
        GamePadState oldGamePadState;
        GamePadState gamePadState;
        //13.49 -27.692
        Vector2 startRect;
        Vector2 endRect;
        Ray cursorRay;
        MouseState mouse;
        MouseState oldMouse;
        Vector3 fakeStatueXYZ;
        EnemyBattleGroup eg1 = new EnemyBattleGroup(0, new Vector3(200.0f, 0.0f, 300.0f), Vector3.Backward);
        int timer;
        public BoundingBox[][] aosBoxes = new BoundingBox[20][];
        public bool[][] aosBools = new bool[20][];
        public static int aosCurrentTeam = 0;
        public static int aosCurrentSelection = 0;

        public static int targetX = 0; 
        public static int targetY = 0; 
        public static int aosTarget = 0; 

        public static List<RageHit> As;
        public static List<RageHit> Bs;
        public static List<RageHit> Xs;
        public static List<RageHit> Ys;

        public static List<RageHit> LTs;
        public static List<RageHit> RTs;
        public static List<RageHit> LSs;
        public static List<RageHit> RSs;

        public class RageHit
        {
            public int distance;
            public TimeSpan time;
            public TimeSpan maxTime;
            public bool dispose = false;
            public RageHit(TimeSpan max)
            {
                maxTime = max;
                time = TimeSpan.Zero;
                distance = 0; 
                
            }
            public void update(GameTime gameTime)
            {

                time += new TimeSpan(gameTime.ElapsedGameTime.Ticks);


                if (time >= maxTime)
                    dispose = true;

            }


        }
        public static int rageBlocks = 0; 
        Camera cam1 = new Camera();

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
        
        public static int enemy = 0;
        public static int hero = 0; 
        public static bool aosMenu = false;
        public static bool HerculesWins = false;
        public static bool HercFinishThor = false;
        
        public static bool AosRun = false;
        public static bool PreAosDialog = false;
        public static bool RageOfTheMachine = false;
        public static bool RageAgainstTheMachine = false;
        public static bool AchillesAlive = false;
        public static bool PerseusTheTraitor = false;
        public static bool EternalStruggleRun = true;
        public static bool HerculesVsThorRun = false;
        public static bool TheseusStandRun = false;
        public static bool TheseusStandRun2 = false;
        public static bool TheseusDeclineOrCancelQuest = false;
        public static bool HercDeclineOrCancelQuest = false;
        public static bool HercAcceptQuest = false;
        public static bool TheseusAcceptQuest = false;

        public static bool PerseusBetrayal = false;
        public static bool PerseusResurrection = false;
        
        bool drawEternalStruggle = true;
        bool drawHercVsThor = false;
        bool drawTheseusStand = false;
        //11.013 -21.648 0.0
        int targetIndex = 0;
        public static int currentIndex  = 1;
        bool activeMenu = true;
        bool firstRun = true;
        bool activeMenuActivated = false;
        TimeSpan currentTimer = TimeSpan.Zero;

        onScreenMenu MichaelsTurn = new onScreenMenu("Michael's Turn");

        
        onScreenMenu heroActiveMenu = new onScreenMenu("Active Menu");
        onScreenMenu targetMenu = new onScreenMenu("Target Menu");
        onScreenMenu reinforceMenu = new onScreenMenu("Reinforce Menu");
        onScreenMenu aiMenu = new onScreenMenu("AI Menu");


        onScreenMenu questMenu = new onScreenMenu("You have been offered a Quest");


        onScreenMenu HercQuestOne = new onScreenMenu("At your father's temple, you see a cosmic entity snooping around.  Do you wish to battle him?");

        
        onScreenMenu TheseusQuestOne = new onScreenMenu("Two brave warriors join you as you rush to find out what the commotion at the gate is.  Would you like to approach the gate?");

        dialog PerseusBetrayal1 = new dialog();
        dialog PerseusBetrayal2 = new dialog();

        dialog ThorAndhercules = new dialog();


        dialog Pluto = new dialog();

        dialog AosIntro = new dialog();
        
        bool isTheseusOfferedQuest = false;

        bool isTheseusOnQuest = false;

        bool isHercOfferedQuest = false;
     
        bool isHercOnQuest = false;

        bool isTarget = false;

        class dialog
        {


            public List<MenuEntry> menuEntries = new List<MenuEntry>();
            public int selectedEntry = 0;
            public List<string> script = new List<string>();
            public int scriptIndex = 0; 

        }
        class onScreenMenu
        {
            List<MenuEntry> menuEntries = new List<MenuEntry>();
            int selectedEntry = 0;
            string menuTitle;

            bool menuActive = true;
            public IList<MenuEntry> MenuEntries
            {
                get { return menuEntries; }
            }
            public int SelectedEntry
            {
                get
                {
                    return selectedEntry;
                }
                set
                {
                    selectedEntry = value;
                }
            }
            public string MenuTitle
            {
                get
                {
                    return menuTitle;
                }
            }

      public onScreenMenu(string menuTitle)
        {
            this.menuTitle = menuTitle;
        }
                    protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
                    {
                        menuEntries[selectedEntry].OnSelectEntry(playerIndex);
                    }
                    /// <summary>
                    /// Handler for when the user has cancelled the menu.
                    /// </summary>
                    protected virtual void OnCancel(PlayerIndex playerIndex)
                    {
                        menuActive = false;
                    }
                    protected void OnCancel(object sender, PlayerIndexEventArgs e)
                    {
                        OnCancel(e.PlayerIndex);
                    }

        }
        public void updateHSes(GameTime gameTime)
        {
            for (int i = 0; i < hspheres.Count; i++)
            {
                hspheres[i].update();
                if (hspheres[i].alive == false)
                    hspheres.RemoveAt(i);
            }


        }
        public void updateDRay(DRay dray, GameTime gameTime)
        {
            float? inter;
            dray.HSes.Clear();
            foreach (HSphere hs in hspheres)
            {
                inter = dray.ray.Intersects(hs.BS);

                if (inter != null)
                {
                    if(inter < (float?)dray.maxDistance)
                        dray.HSes.Add(hs);

                }

            }

            dray.findDistance();
            dray.update(gameTime);
            


        }

        public EternalStruggle()
        {


            //vertexBuffer.SetData(vertices.ToArray());
            float drayangle = TurnToFace(new Vector3(100.0f, 70.0f, 0.0f), hsphere2.BS.Center, Vector3.Forward);
            Vector3 drayVec =  new Vector3((float)Math.Sin(drayangle), 0.0f, (float)Math.Cos(drayangle));
            dray1 = new DRay(new Vector3(100.0f, 70.0f, 0.0f), drayVec, 500.0f);
            dray1.HSes.Add(hsphere1);
            dray1.HSes.Add(hsphere2);
            dray1.findDistance();

            hspheres.Add(hsphere1);
            hspheres.Add(hsphere2);
            //dray1.distance = 500.0f;
            selectionBox = new Rectangle();
            mPosition = new Vector2(200.0f, 200.0f);
            endMPos = mPosition;
            oldGamePadState = new GamePadState();
            gamePadState = new GamePadState();
            mouse = new MouseState();
            oldMouse = new MouseState();
            
            timer = 1000; 
            MenuEntry Start = new MenuEntry("Start");




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

            for (int i = 0; i < 20; i++)
            {
                aosBoxes[i] = new BoundingBox[20];

                for (int j = 0; j < 20; j++)
                {
                    aosBoxes[i][j] = new BoundingBox(new Vector3(i * 60, 1.0f, j * 60), new Vector3((i + 1) * 60, 1.0f, (j + 1) * 60));
                   // aosBools[i][j] = false;
                }
            }
            As = new List<RageHit>();
            Bs = new List<RageHit>();
            Xs = new List<RageHit>();
            Ys = new List<RageHit>();

            LSs = new List<RageHit>();
            LTs = new List<RageHit>();
            RSs = new List<RageHit>();
            RTs = new List<RageHit>();

            ThorAndhercules.script = new List<string>();
            ThorAndhercules.script.Add("Hercules : \" Now Let's talk about what you were doing at my father's temple\"");

            AosIntro.script = new List<string>();
            AosIntro.script.Add("Eventually, a crazed Achilles would single-handedly quell");
            AosIntro.script.Add("the invasion of Crete.  In his craze, however, he met his");
            AosIntro.script.Add("almost brother in law, Paris, the last son of Troy.  Paris's");
            AosIntro.script.Add("enchanted arrows reunited Achilles with his true love long");
            AosIntro.script.Add("enough to break the curse he was under.  As he returned");
            AosIntro.script.Add("with the invaders of Crete seeking revenge, he ran across");
            AosIntro.script.Add("Hercules, Thor, and Galahad, a noble knight from the West");
            AosIntro.script.Add("As Galahad reached out to greet Achilles, Perseus appeared.");
            AosIntro.script.Add("Thor sprung into action, revealing that Perseus was really");
            AosIntro.script.Add("the evil brother he was searching for.  Suddenly, a great ");
            AosIntro.script.Add("cloud engulfed the warriors and a fierce figure stood before");
            AosIntro.script.Add("them.  Just as suddenly, a mighty archon appeared, rallying");
            AosIntro.script.Add("all beings of Order, good and evil alike, to battle the ");
            AosIntro.script.Add("the ensuing chaos.");

            PerseusBetrayal1.script = new List<string>();
            PerseusBetrayal1.script.Add("Perseus makes a deal with Ares to bring back the master of War");
            PerseusBetrayal1.script.Add("Ares gives Perseus an elixir of Nihilism");


            PerseusBetrayal2.script = new List<string>();
            PerseusBetrayal2.script.Add("Perseus makes a deal with Pluto to add Thousands");
            PerseusBetrayal2.script.Add("of souls to the Underworld");
            PerseusBetrayal2.script.Add("Pluto releases the soul of the demigod Achilles");
            PerseusBetrayal2.script.Add("The villanous Perseus uses a strange type of alchemy");
            PerseusBetrayal2.script.Add("to dilute the soul of Achilles with the elixir");

            Pluto.script.Add("Hades is enraged by Perseus's actions");
            Pluto.script.Add("\"What have you done Perseus?  You dare\"");
            Pluto.script.Add("\"Defy my will?  My minions will not let you escape\"");
            Pluto.script.Add("Pluto summons 4 undead warriors");

            MenuEntry forward = new MenuEntry("Forward");
            MenuEntry backward = new MenuEntry("Backward");
            MenuEntry end = new MenuEntry("End");

            forward.Selected += ForwardDialog;
            backward.Selected += BackwardDialog;
            end.Selected += SkipDialog;

            Pluto.menuEntries.Add(forward);
            Pluto.menuEntries.Add(backward);
            Pluto.menuEntries.Add(end);

            AosIntro.menuEntries.Add(forward);
            AosIntro.menuEntries.Add(backward);
            AosIntro.menuEntries.Add(end);

            PerseusBetrayal1.menuEntries.Add(forward);
            PerseusBetrayal1.menuEntries.Add(backward);
            PerseusBetrayal1.menuEntries.Add(end);

            PerseusBetrayal2.menuEntries.Add(forward);
            PerseusBetrayal2.menuEntries.Add(backward);
            PerseusBetrayal2.menuEntries.Add(end);


            
            MenuEntry questInfo = new MenuEntry("Quest Information");
            MenuEntry accept = new MenuEntry("Accept Quest");
            MenuEntry decline = new MenuEntry("Decline Quest this Round");


            questMenu.MenuEntries.Add(questInfo);
            questMenu.MenuEntries.Add(accept);
            questMenu.MenuEntries.Add(decline);

            

            MenuEntry hercVsThorAccept = new MenuEntry("Accept Quest: Hercules vs Thor");
            MenuEntry hercVsThorDecline = new MenuEntry("Decline Quest");

            MenuEntry theseusStandAccept = new MenuEntry("Accept Quest: Theseus's Stand");
            MenuEntry theseusStandDecline = new MenuEntry("Decline Quest");

            hercVsThorAccept.Selected += HQOneAcceptSelected;
            hercVsThorDecline.Selected += HQOneDeclineSelected;

            theseusStandAccept.Selected += TheseusStandAccepted;
            theseusStandDecline.Selected += TheseusStandDeclined;

            TheseusQuestOne.MenuEntries.Add(theseusStandAccept);
            TheseusQuestOne.MenuEntries.Add(theseusStandDecline);
            HercQuestOne.MenuEntries.Add(hercVsThorAccept);
            HercQuestOne.MenuEntries.Add(hercVsThorDecline);


            activeMenu = false;
            GameTime gameTime = new GameTime(new TimeSpan(3000), new TimeSpan(3000), false);
            firstRun = true;
            MenuEntry changeFormation = new MenuEntry("Change Formation");
            MenuEntry reinforce = new MenuEntry("ReinForce");
            MenuEntry assignTarget = new MenuEntry("Target");
            MenuEntry subordinate = new MenuEntry("Set AIs");
            MenuEntry begin = new MenuEntry("Begin");
            assignTarget.Selected += TargetSelected;
            begin.Selected += Begin;

            heroActiveMenu.MenuEntries.Add(changeFormation);
            heroActiveMenu.MenuEntries.Add(reinforce);
            heroActiveMenu.MenuEntries.Add(assignTarget);
            heroActiveMenu.MenuEntries.Add(subordinate);
            heroActiveMenu.MenuEntries.Add(begin);

            cam1 = new Camera();
            cam1.LookAtOffset = new Vector3(0.0f, 100.0f, 0.0f);
            cam1.DesiredPositionOffset = new Vector3(0.0f, 100.0f, -300.0f);
            cam1.NearPlaneDistance = 1.0f;
            cam1.FarPlaneDistance = 10000.0f;
            cam1.CameraFrustum = new BoundingFrustum(Matrix.Identity);
            cam1.FakeFarPlaneDistance = 1000.0f;
            cam1.FakeNearPlaneDistance = 1.0f;
            cam1.State = 1;

            cam1.Reset();
           // MenuEntry targetReinForce = new MenuEntry("Target Reinforce");
            

        }
        void ForwardDialog(object sender, PlayerIndexEventArgs e)
        {


            if (PreAosDialog)
            {
                AosIntro.scriptIndex++;
                if (AosIntro.scriptIndex == AosIntro.script.Count)
                {
                    PreAosDialog = false;
                    AosRun = true;




                }




            }
            if (PerseusBetrayal)
            {
                PerseusBetrayal1.scriptIndex++;
                if (PerseusBetrayal1.scriptIndex == PerseusBetrayal1.script.Count)
                {
                    PerseusBetrayal = false;

                    EternalStruggleRun = true;
                    PerseusTheTraitor = true;
                    
                   // PerseusBetrayal1.scriptIndex--;
                }
            }
            else if (PerseusResurrection)
            {
                PerseusBetrayal2.scriptIndex++;
                if (PerseusBetrayal2.scriptIndex == PerseusBetrayal2.script.Count)
                {
                    PerseusResurrection = false;
                    EternalStruggleRun = true;
                    AchillesAlive = true;
                   // PerseusBetrayal2.scriptIndex--;


                }
            }

            else if (AchillesAlive)
            {
                Pluto.scriptIndex++;
                if (Pluto.scriptIndex == Pluto.script.Count)
                {
                    AchillesAlive = false;
                    EternalStruggleRun = true;
                    RageOfTheMachine = true;


                }


            }
            



        }
        void BackwardDialog(object sender, PlayerIndexEventArgs e)
        {


        }
        void SkipDialog(object sender, PlayerIndexEventArgs e)
        {
           
           


        }
        void TheseusStandAccepted(object sender, PlayerIndexEventArgs e)
        {
            EternalStruggleRun = false;
            TheseusStandRun = true;
            isTheseusOfferedQuest = false;
            TheseusAcceptQuest = true;
            

        }
        void TheseusStandDeclined(object sender, PlayerIndexEventArgs e)
        {
            EternalStruggleRun = true;
            isTheseusOfferedQuest = true;
            TheseusDeclineOrCancelQuest = true;

        }
        void HQOneAcceptSelected(object sender, PlayerIndexEventArgs e)
        {
            EternalStruggleRun = false;
            HerculesVsThorRun = true;
            isHercOfferedQuest = false;
            HercAcceptQuest = true;
        }
        void HQOneDeclineSelected(object sender, PlayerIndexEventArgs e)
        {

            EternalStruggleRun = true;
            isHercOfferedQuest = false;
            HercDeclineOrCancelQuest = true;


        }
        void QuestInfoSelect(object sender, PlayerIndexEventArgs e)
        {


        }
        void Begin(object sender, PlayerIndexEventArgs e)
        {
            if (activeMenu)
            {
                EternalStruggleRun = true;
                activeMenu = false;
            }
            if (aosMenu)
            {

                aosMenu = false;
            }




        }
        void TargetSelected(object sender, PlayerIndexEventArgs e)
        {

            isTarget = true;

        }

        
        
        public void checkTowerHits(Tower tower)
        {

            foreach (Projectile2 pro in tower.projectiles)
            {
               foreach(JuneXnaModel june in ScreenManager.fighters)
                   
                   {
                   if(june.activated)
                       if (pro.BS.Contains(june.spheres[1].BS) != ContainmentType.Disjoint)
                       {
                           pro.alive = false;
                           june.health -= 10;

                       }
                   }
            }

        }
        public void checkCollisionProjectile(JuneXnaModel actor, JuneXnaModel target)
        {

            foreach (Projectile2 proj in actor.projectiles)
            {
                foreach (boundingSphere bs in target.spheres)
                    if (proj.BS.Contains(bs.BS) != ContainmentType.Disjoint)
                    {
                        proj.alive = false;
                        //target.isKnockedBack = true;
                        target.health -= 1;
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
        BoundingFrustum UnprojectRectangle(Rectangle source, Viewport viewport, Matrix projection, Matrix view)
        {

            Vector2 regionCenterScreen = new Vector2(source.Center.X, source.Center.Y);
            Matrix regionProjMatrix = projection;

            regionProjMatrix.M11 /= ((float)source.Width / (float)viewport.Width);
            regionProjMatrix.M22 /= ((float)source.Height / (float)viewport.Height);
            regionProjMatrix.M31 = (regionCenterScreen.X - (viewport.Width / 2f)) / ((float)source.Width / 2f);
            regionProjMatrix.M32 = -(regionCenterScreen.Y - (viewport.Height / 2f)) / ((float)source.Height / 2f);

            return new BoundingFrustum(view * regionProjMatrix);
        }

        public void updateFighters(GameTime gameTime)
        {
            foreach (JuneXnaModel june in ScreenManager.fighters)
            {
                if (june.activated)
                {
                    if (june.health <= 0)
                    {
                      if (!june.ghost)
                        {
                            june.ghost = true;
                            JuneXnaModel ghost = (new JuneXnaModel(june.Position, june.Direction));
                            ScreenManager.loadSpheresJuneModel(ghost);
                            ghost.SkinningData = ScreenManager.juneModel.Tag as SkinningData;
                            ghost.setAnimationPlayers();
                            ScreenManager.ghosts.Add(ghost);
                        }
                    }
                    june.UpdateFighter(gameTime);
                }
                else //if (ScreenManager.stage1 == true)
                {
                    june.creationTime -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (june.creationTime < 0)
                        june.activated = true;

                }

                //else if (gameTime.TotalGameTime.Seconds > june.creationTime)
                //{
                //    june.activated = true;
                //    june.UpdateFighter(gameTime);
                //}




            }





        }
        public void checkResources(GameTime gameTime)
        {
            int count = 0;
            int absorbed= 0;
            count = gameTime.ElapsedGameTime.Milliseconds / 10;
            
            if (ScreenManager.Theseus.absorbAffinity)
            {

                foreach (Resource res in ScreenManager.resources)
                {

                    if (ScreenManager.Theseus.areaSphere.BS.Contains(res.BS) != ContainmentType.Disjoint)
                    {
                        if (res.Type == 0)
                        {
                            if (count > res.Current)
                            {
                                absorbed = res.Current;
                                res.Current = 0;
                                ScreenManager.Theseus.earthA += absorbed;
                            }
                            else
                            {
                                res.Current -= count;
                                ScreenManager.Theseus.earthA += count;

                            }


                        }
                        if (res.Type == 1)
                        {
                            if (count > res.Current)
                            {
                                absorbed = res.Current;
                                res.Current = 0;
                                ScreenManager.Theseus.fireA += absorbed;
                            }
                            else
                            {
                                res.Current -= count;
                                ScreenManager.Theseus.fireA += count;

                            }


                        }
                        if (res.Type == 2)
                        {
                            if (count > res.Current)
                            {
                                absorbed = res.Current;
                                res.Current = 0;
                                ScreenManager.Theseus.windA += absorbed;
                            }
                            else
                            {
                                res.Current -= count;
                                ScreenManager.Theseus.windA += count;

                            }


                        }
                        if (res.Type == 3)
                        {
                            if (count > res.Current)
                            {
                                absorbed = res.Current;
                                res.Current = 0;
                                ScreenManager.Theseus.waterA += absorbed;
                            }
                            else
                            {
                                res.Current -= count;
                                ScreenManager.Theseus.waterA += count;

                            }


                        }



                    }


                }





            }




        }
        public static bool isPickedUp(Item item)
        {
            return item.pickedUp;
        }
        public static bool isDead(JuneXnaModel june)
        {

            return june.health <= 0;
        }
        public void updateCursor()
        {
            ScreenManager.Theseus.hasAllyTargetCursor = false;
            ScreenManager.Theseus.allyTargetIndex = -1;
            Vector3 nearSource = new Vector3(ScreenManager.Theseus.TargetCursor, 0f);
            Vector3 farSource = new Vector3(ScreenManager.Theseus.TargetCursor, 1f);

            nearPoint = ScreenManager.GraphicsDevice.Viewport.Unproject(nearSource,
               ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);

            Vector3 farPoint = ScreenManager.GraphicsDevice.Viewport.Unproject(farSource,
                ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);

            // Console.WriteLine(nearPoint);
            // find the direction vector that goes from the nearPoint to the farPoint
            // and normalize it....
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            cursorRay = new Ray(nearPoint, direction);

            float[] distances = new float[ScreenManager.runners.Count];
            float distance = -1.0f;
            int index = 0;
            for (int i = 0; i<ScreenManager.deadRunners.Count; i++)
            {
                //distances[i] = -1;
                int lowestHitIndex = -1;
                float lowestHitDist = -1;
                float?[] sphereDistances = new float?[ScreenManager.deadRunners[i].spheres.Count];
                for (int j = 0; j < ScreenManager.deadRunners[i].spheres.Count; j++)
                {
                    sphereDistances[j] = cursorRay.Intersects(ScreenManager.deadRunners[i].spheres[j].BS);
                    if (sphereDistances[j] != null)
                    {
                        if (lowestHitDist == -1)
                        {
                            lowestHitDist = sphereDistances[j].Value;
                            lowestHitIndex = j;
                        }
                        else
                            if (sphereDistances[j].Value < lowestHitDist)
                            {
                                lowestHitDist = sphereDistances[j].Value;
                                lowestHitIndex = j;

                            }
                    }

                }
                if (lowestHitDist != -1)
                {
                    if (distance == -1)
                    {
                        distance = lowestHitDist;
                        index = i;

                        ScreenManager.Theseus.allyTargetIndex = index;

                    }
                    else if (lowestHitDist < distance)
                    {
                        distance = lowestHitDist;
                        index = i;
                        ScreenManager.Theseus.allyTargetIndex = index;

                    }

                }


            }

        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            keyboardState = Keyboard.GetState();
            oldGamePadState = gamePadState;
            gamePadState = GamePad.GetState(PlayerIndex.One);

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                ScreenManager.Game.Exit();

            }
            if (gamePadState.IsButtonDown(Buttons.Start) && oldGamePadState.IsButtonUp(Buttons.Start))
            {

                if (ScreenManager.paused)
                    ScreenManager.paused = false;
                else
                {
                    ScreenManager.paused = true;
                    ScreenManager.AddScreen(new PauseMenuScreen(), PlayerIndex.One);
                }
            }

            if (firstRun)
            {
                // Create a vertex buffer, and copy our vertex data into it.
                vertexBuffer = new VertexBuffer(screenManager.GraphicsDevice,
                                                typeof(VertexPositionColor),
                                                20, BufferUsage.None);
                // Create an index buffer, and copy our index data into it.
                indexBuffer = new IndexBuffer(ScreenManager.GraphicsDevice, typeof(ushort),
                                             60, BufferUsage.None);
                makeEffect();
                ScreenManager.Theseus.UpdateNinjaneer(gameTime);
                for (int i = 0; i < ScreenManager.dummies.Count; i++)
                    ScreenManager.dummies[i].UpdateBrace(gameTime);
                for (int i = 0; i < ScreenManager.runners.Count; i++)
                    ScreenManager.runners[i].UpdateBrace(gameTime);
                firstRun = false;
            }
            else if(!ScreenManager.paused)
            {
               // ScreenManager.factory1.Update(gameTime);
                ScreenManager.collisionGroups.Clear();
                //It was JuneXnaModel.UpdatePlatform
                ScreenManager.Theseus.UpdatePlatform(gameTime);

                checkMovePlayerVs(ScreenManager.dummies);
                for (int i = 0; i < ScreenManager.dummies.Count; i++)
                {
                    // Console.WriteLine(i);
                   // ScreenManager.dummies[i].setSpheres.Clear();
                    //if(ScreenManager.dummies[i].isFighter)
                    ScreenManager.dummies[i].UpdateAIMonsterB(gameTime);

                    //if (ScreenManager.dummies[i].isRunner)
                    //    ScreenManager.dummies[i].UpdateAIRunner(gameTime);
                   
                    // Console.WriteLine("check the collision");
                   // checkMoveEnemySingle(ScreenManager.dummies[i], gameTime);
                  //  ScreenManager.dummies[i].checkMoveEnemySingle(gameTime);
                }


                for (int i = 0; i < ScreenManager.archers.Count; i++)
                {
                    ScreenManager.archers[i].UpdateAIRange(gameTime);
                }

                for (int i = 0; i < ScreenManager.runners.Count; i++)
                {
                    ScreenManager.runners[i].UpdateAIRunner(gameTime);
                    

                }
     

                for (int i = 0; i < ScreenManager.dummies.Count; i++)
                {
                    //if (ScreenManager.dummies[i].isFighter)
                    checkTheseusAtk(ScreenManager.dummies[i]);
                    checkEnemyAttack(ScreenManager.dummies[i]);
                    if(ScreenManager.dummies[i].health <= 0)
                    ScreenManager.deadMonsters.Add(ScreenManager.dummies[i]);
                }

                for (int i = 0; i < ScreenManager.dummies.Count; i++)// (JuneXnaModel june in ScreenManager.runners)
                {
                    if (ScreenManager.dummies[i].health <= 0)
                    {
                        ScreenManager.deadMonsters.Add(ScreenManager.dummies[i]);


                    }

                }

                for (int i = 0; i < ScreenManager.runners.Count; i++)// (JuneXnaModel june in ScreenManager.runners)
                {
                    if (ScreenManager.runners[i].health <= 0)
                    {
                        ScreenManager.deadRunners.Add(ScreenManager.runners[i]);


                    }

                }
                foreach (Item item in ScreenManager.items)
                    item.Update(gameTime);

                //foreach (Item item in ScreenManager.items)
                    ScreenManager.items.RemoveAll(isPickedUp);
                foreach(JuneXnaModel runner in ScreenManager.deadRunners)
                runner.UpdateDeadJune(gameTime);
                foreach(JuneXnaModel monster in ScreenManager.deadMonsters)
                monster.UpdateDeadMonster(gameTime);
               ScreenManager.dummies.RemoveAll(isDead);
               ScreenManager.runners.RemoveAll(isDead);

               for (int i = 0; i < ScreenManager.dummies.Count; i++)
               {
                   ScreenManager.dummies[i].fighterIndex = i;

               }
               for (int i = 0; i < ScreenManager.runners.Count; i++)
                   ScreenManager.runners[i].fighterIndex = i + 100;
            }
            updateCursor();
            //checkMoves2();
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
        public void Update2(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

            
            oldGamePadState = gamePadState;
            gamePadState = GamePad.GetState(PlayerIndex.One);

            keyboardState = Keyboard.GetState();

            if(keyboardState.IsKeyDown(Keys.Escape))
            {
                ScreenManager.Game.Exit();

            }

            if (gamePadState.IsButtonDown(Buttons.Start) && oldGamePadState.IsButtonUp(Buttons.Start))
            {

                if (ScreenManager.paused)
                    ScreenManager.paused = false;
                else
                {
                    ScreenManager.paused = true;
                    ScreenManager.AddScreen(new PauseMenuScreen(), PlayerIndex.One);
                }

                


            }

            if (!ScreenManager.paused)
            {
                checkTarget();
               // updateFighters(gameTime);
                //foreach (JuneXnaModel june in ScreenManager.ghosts)
               //     june.UpdateGhost(gameTime);
                checkResources(gameTime);
                foreach (Resource res in ScreenManager.resources)
                {
                    
                    res.update(gameTime);
                    res.unproject = ScreenManager.GraphicsDevice.Viewport.Project(res.Translation, ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);

                }
              //  ScreenManager.rogue.UpdateRogue(gameTime);

                // ScreenManager.fighter.UpdateFighter(gameTime);
               // ScreenManager.RinnaAl.update(gameTime);

                foreach (Tower tower in ScreenManager.Towers)
                {
                    tower.update(gameTime);
                    tower.unproject = ScreenManager.GraphicsDevice.Viewport.Project(tower.BS.Center, ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);
                    checkTowerHits(tower);
                }
                

                updateDRay(dray1, gameTime);
                updateHSes(gameTime);


                //if (gamePadState.ThumbSticks.Right.X > .03f)
                //    mPosition.X += 3f;

                //if (gamePadState.ThumbSticks.Right.X < -.03f)
                //     -= 3;
                oldMouse = mouse;
                mouse = Mouse.GetState();

                //if(ScreenManager.Theseus.castState == 1)


                if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
                {
                    mPosition = new Vector2(mouse.X, mouse.Y);


                }
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    endMPos = new Vector2(mouse.X, mouse.Y);

                }
                if (mouse.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed)
                {

                    IntersectSelectionBox(UnprojectRectangle(selectionBox, ScreenManager.GraphicsDevice.Viewport, ScreenManager.camera.Projection, ScreenManager.camera.View));

                    mPosition = new Vector2(mouse.X, mouse.Y);
                    endMPos = new Vector2(mouse.X, mouse.Y);
                }

                // Vector3 spot = ScreenManager.GraphicsDevice.Viewport.Unproject(new Vector3(mPosition.X, mPosition.Y, 1), ScreenManager.camera.Projection, ScreenManager.camera.View, ScreenManager.camera.transform);



                //float a = cursorRay.Intersects(new Plane(1.0f, 1.0f, 1.0f, 1.0f));
                // Console.WriteLine(spot);
                Point start, end;
                if (mPosition.X < endMPos.X)
                {
                    start.X = (int)mPosition.X;
                    end.X = (int)endMPos.X;
                }
                else
                {
                    start.X = (int)endMPos.X;
                    end.X = (int)mPosition.X;
                }
                if (mPosition.Y < endMPos.Y)
                {
                    start.Y = (int)mPosition.Y;
                    end.Y = (int)endMPos.Y;
                }
                else
                {
                    start.Y = (int)endMPos.Y;
                    end.Y = (int)mPosition.Y;
                }
                selectionBox = new Rectangle(start.X, start.Y, end.X - start.X, end.Y - start.Y);

                IntersectBox();
                IntersectTheseus();
                checkCollisionProjectile(ScreenManager.Theseus, ScreenManager.Jailer);
                #region MouseRay
                /*
            Vector2 mPos = new Vector2(mouse.X, mouse.Y);
            Vector3 nearSource = new Vector3(mPosition, 0f);
            Vector3 farSource = new Vector3(mPosition, 1f);
            ScreenManager.fakeStatue.UpdateBrace(gameTime);
            // use Viewport.Unproject to tell what those two screen space positions
            // would be in world space. we'll need the projection matrix and view
            // matrix, which we have saved as member variables. We also need a world
            // matrix, which can just be identity.
             nearPoint = ScreenManager.GraphicsDevice.Viewport.Unproject(nearSource,
                ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);

            Vector3 farPoint = ScreenManager.GraphicsDevice.Viewport.Unproject(farSource,
                ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);

           // Console.WriteLine(nearPoint);
            // find the direction vector that goes from the nearPoint to the farPoint
            // and normalize it....
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            cursorRay = new Ray(nearPoint, direction);
            //Ray myray = new Ray();
            Plane plane = new Plane(new Vector3(1.0f, 0.0f, 0.0f), new Vector3(5.0f, 0.0f, 3.0f), new Vector3(40.0f, 0.0f, 5.0f));
            float? dist = cursorRay.Intersects(plane);
            //if (dist != null)
            //    Console.WriteLine(nearPoint + direction * dist);

            if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Pressed)
            {
                mTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (mTimer > 20)
                {
                    if (mCast2.Count > 0)
                    {
                        if (Vector2.Distance(mCast2[mCast2.Count - 1], mPos) > 5)
                            mCast2.Add(mPos);
                    }
                        else
                            mCast2.Add(mPos);
                    
                    mTimer = 0; 
                }

                
            }
            if (mouse.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed)
            {

                bool l1 = false;
                bool l2 = false;

                bool r1 = false;
                bool r2 = false;

                bool u1 = false;
                bool u2 = false;

                bool d1 = false;
                bool d2 = false;

if(mCast2.Count > 1)
{
    if (mCast2[0].X > mCast2[1].X)
        l1 = true;
    if (mCast2[0].X < mCast2[1].X)
        r1 = true;
    if (mCast2[0].Y > mCast2[1].Y)
        d1 = true;
    if (mCast2[0].Y < mCast2[1].Y)
        u1 = true;

    if (mCast2[mCast2.Count - 2].X > mCast2[mCast2.Count - 1].X)
        l2 = true;
    if (mCast2[mCast2.Count - 2].X < mCast2[mCast2.Count - 1].X)
        r2 = true;
    if (mCast2[mCast2.Count - 2].Y > mCast2[mCast2.Count - 1].Y)
        d2 = true;
    if (mCast2[mCast2.Count - 2].Y < mCast2[mCast2.Count - 1].Y)
        u2 = true;



    // what if 

    for (int i = 0; i < mCast2.Count - 1; i++) //cant look at the last one in the count
    {


    }




    }



                mCast2.Clear();
                mTimer = 0; 
            }
            if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                mPos = new Vector2(mouse.X, mouse.Y);
                Vector3 nearSource2 = new Vector3(mPos, 0f);
                Vector3 farSource2 = new Vector3(mPos, 1f);
                Vector3 nearPoint2 = ScreenManager.GraphicsDevice.Viewport.Unproject(nearSource2, ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);
                Vector3 farPoint2 = ScreenManager.GraphicsDevice.Viewport.Unproject(farSource2,
                ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);
                Vector3 direction2 = farPoint2 - nearPoint2;
                direction2.Normalize();
                Ray aray = new Ray(nearPoint2, direction2);
                Plane plane2 = new Plane(new Vector3(1.0f, 0.0f, 0.0f), new Vector3(5.0f, 0.0f, 3.0f), new Vector3(40.0f, 0.0f, 5.0f));
                float? dist2 = aray.Intersects(plane2);

                //ScreenManager.SmartMovement1.moveVec = (nearPoint2 + direction2 * (float)dist2);
                //ScreenManager.SmartMovement1.state = 1;
                if (dist != null)
                    ScreenManager.Theseus.atkVec = (nearPoint2 + direction2 * (float)dist2);
                if (ScreenManager.Theseus.castId != 0)
                {
                    ScreenManager.Theseus.castState = 1;

                   // ScreenManager.Theseus.projectiles.Add(new Projectile2("Fire", ScreenManager.Theseus.forwardSpell, ScreenManager.Theseus.Direction));
                }
                if (ScreenManager.Theseus.fightClass == 0)
                    if (!ScreenManager.Theseus.isAtk1)
                    {
                        ScreenManager.Theseus.aiState = 1;

                        ScreenManager.Theseus.isAtk1 = true;

                    }
                if (ScreenManager.Theseus.fightClass == 1)
                    if (!ScreenManager.Theseus.isBow)
                    {
                        ScreenManager.Theseus.aiState = 1;


                        ScreenManager.Theseus.isBow = true;
                    }

                checkRightClick(aray);






            }
            if (mouse.RightButton == ButtonState.Pressed && oldMouse.RightButton == ButtonState.Released)
            {
                mPos = new Vector2(mouse.X, mouse.Y);
                Vector3 nearSource2 = new Vector3(mPos, 0f);
                Vector3 farSource2 = new Vector3(mPos, 1f);
               Vector3 nearPoint2 = ScreenManager.GraphicsDevice.Viewport.Unproject(nearSource2, ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);
                Vector3 farPoint2 = ScreenManager.GraphicsDevice.Viewport.Unproject(farSource2,
                ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);
                Vector3 direction2 = farPoint2 - nearPoint2;
                direction2.Normalize();
                Ray aray = new Ray(nearPoint2, direction2);
                Plane plane2 = new Plane(new Vector3(1.0f, 0.0f, 0.0f), new Vector3(5.0f, 0.0f, 3.0f), new Vector3(40.0f, 0.0f, 5.0f));
                float? dist2 = aray.Intersects(plane2);
                if (dist != null)
                    ScreenManager.Theseus.moveVec = (nearPoint2 + direction2 *(float)dist2);
                ScreenManager.Theseus.aiState = 0;
                checkRightClick(aray);

            }
            */
                #endregion
                // fakeStatueXYZ = ScreenManager.GraphicsDevice.Viewport.Unproject(new Vector3(mouse.X, 0.0f, mouse.Y), ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);

                ScreenManager.fakeStatue.Position = fakeStatueXYZ;
                // ScreenManager.globalInput.Update();
                //LSs.RemoveAll(disposeButtons);

                // checkCollisions();


                //timer -= gameTime.ElapsedGameTime.Milliseconds;
                //if (timer <= 0)
                //{
                //    aosCurrentSelection++;
                //    if (aosCurrentSelection >= 3)
                //    {
                //        if (aosCurrentTeam == 1)
                //            aosCurrentTeam = 0;
                //        else if (aosCurrentTeam == 0)
                //            aosCurrentTeam = 1;
                //        aosCurrentSelection = 0; 

                //    }
                //    timer = 1000; 

                //}
                if (firstRun)
                {
                    // Create a vertex buffer, and copy our vertex data into it.
                    vertexBuffer = new VertexBuffer(screenManager.GraphicsDevice,
                                                    typeof(VertexPositionColor),
                                                    20, BufferUsage.None);

                    //vertexBuffer.SetData(vertices.ToArray());

                    // Create an index buffer, and copy our index data into it.
                    indexBuffer = new IndexBuffer(ScreenManager.GraphicsDevice, typeof(ushort),
                                                 60, BufferUsage.None);
                    makeEffect();
                    // makeCylinder(Vector3.Forward);
                    //ScreenManager.Hercules.UpdateHercules(gameTime);



                    //ScreenManager.Perseus.UpdatePerseus(gameTime);


                    ScreenManager.Theseus.UpdateTheseus(gameTime);
                    //ScreenManager.Theseus.UpdateNihon(gameTime);


                    //ScreenManager.Loki.updateSpearAi(gameTime);

                    //ScreenManager.Jailer.UpdateJSpearL(gameTime);
                    //ScreenManager.fighter.UpdateBrace(gameTime);
                    //ScreenManager.god.UpdateBrace(gameTime);
                    //ScreenManager.rogue.UpdateBrace(gameTime);

                    //ScreenManager.SmartMovement1.UpdateSmartMove(gameTime);
                    //ScreenManager.SmartMovement2.UpdateSmartMove(gameTime);
                    //ScreenManager.SmartMovement3.UpdateSmartMove(gameTime);
                    //ScreenManager.Wizard.UpdateFWiz(gameTime);

                    //ScreenManager.MinoTS.UpdateBrace(gameTime);

                    //for (int i = 0; i < ScreenManager.fighters.Count; i++)
                    //{
                    //    ScreenManager.fighters[i].UpdateBrace(gameTime);


                    //}
                    for (int i = 0; i < ScreenManager.dummies.Count; i++)
                        ScreenManager.dummies[i].UpdateBrace(gameTime);
                    firstRun = false;
                }
                else
                    checkTowerHits(ScreenManager.RinnaAl);

               // ScreenManager.Theseus.UpdateNihon(gameTime);
                ScreenManager.Theseus.UpdateTheseus(gameTime);

                for (int i = 0; i < ScreenManager.dummies.Count; i++)
                    ScreenManager.dummies[i].UpdateBrace(gameTime);
               //

                Vector3 nearSource = new Vector3(ScreenManager.Theseus.TargetCursor, 0f);
                Vector3 farSource = new Vector3(ScreenManager.Theseus.TargetCursor, 1f);
                nearPoint = ScreenManager.GraphicsDevice.Viewport.Unproject(nearSource,
   ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);

                Vector3 farPoint = ScreenManager.GraphicsDevice.Viewport.Unproject(farSource,
                    ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);
                Vector3 direction = farPoint - nearPoint;
                direction.Normalize();
                cursorRay = new Ray(nearPoint, direction);
                Plane plane = new Plane(new Vector3(1.0f, 0.0f, 0.0f), new Vector3(5.0f, 0.0f, 3.0f), new Vector3(40.0f, 0.0f, 5.0f));
                float? dist = cursorRay.Intersects(plane);
                if(dist!= null)
                ScreenManager.Theseus.TargetCursor3 = nearPoint + direction * dist;

                ScreenManager.Theseus.Projected = ScreenManager.GraphicsDevice.Viewport.Project(ScreenManager.Theseus.Position + ScreenManager.Theseus.Direction * 20.0f, ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);
                //ScreenManager.Theseus.TargetCursor3 = ScreenManager.GraphicsDevice.Viewport.Unproject(new Vector3(ScreenManager.Theseus.TargetCursor, 0.0f), ScreenManager.camera.Projection, ScreenManager.camera.View, ScreenManager.camera.transform);
                //ScreenManager.Theseus.Projected = ScreenManager.GraphicsDevice.Viewport.Project(ScreenManager.Theseus.Position, ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);
                // ScreenManager.SmartMovement1.UpdateSmartMove(gameTime);
                // ScreenManager.SmartMovement2.UpdateSmartMove(gameTime);
                //  ScreenManager.SmartMovement3.UpdateSmartMove(gameTime);
                //  ScreenManager.SmartMovement1.UpdateSmartMove(gameTime);
                //ScreenManager.Wizard.UpdateFWiz(gameTime);
                //ScreenManager.Jailer.UpdateJSpearL(gameTime);
                //ScreenManager.MinoTS.UpdateBrace(gameTime);
                
                //EnemyAtk(ScreenManager.rogue);
                //checkEAtkNihon(ScreenManager.rogue);
                //foreach (JuneXnaModel june in ScreenManager.fighters)
                //    //EnemyAtk(june);
                //    checkEAtkNihon(june);
                //checkTheseusAtk(ScreenManager.rogue);
                //forceMoves(ScreenManager.rogue);


                checkMoves();
                if(ScreenManager.Theseus.healing)
                checkHealing(gameTime);
                #region oldRunUpdates
                //   if (AosRun)
                //   {

                //       if (!aosMenu)
                //       {
                //           if (aosCurrentTeam == 0 && aosCurrentSelection == 0)
                //           {

                //               ScreenManager.Loki.updateSpearAi(gameTime);
                //               ScreenManager.Wisp.formationVector = ScreenManager.Loki.CFormation[7];
                //               ScreenManager.Wisp.updateSwordFollower(gameTime);
                //               ScreenManager.Michael.UpdateMichael(gameTime);
                //               ScreenManager.Dargahe.UpdateBrace(gameTime);
                //               ScreenManager.CaptThor.UpdateBrace(gameTime);
                //               ScreenManager.CaptWisp.UpdateBrace(gameTime);
                //               ScreenManager.LtHercules.UpdateBrace(gameTime);
                //               ScreenManager.LtOgthul.UpdateBrace(gameTime);

                //           }
                //           if (aosCurrentTeam == 0 && aosCurrentSelection == 1)
                //           {
                //               ScreenManager.Wisp.UpdateBrace(gameTime);
                //               ScreenManager.Loki.UpdateBrace(gameTime);
                //               ScreenManager.Michael.UpdateBrace(gameTime);
                //               ScreenManager.Dargahe.UpdateBrace(gameTime);
                //               ScreenManager.CaptThor.UpdateThor(gameTime);
                //               ScreenManager.CaptWisp.UpdateBrace(gameTime);
                //               ScreenManager.LtHercules.UpdateBrace(gameTime);
                //               ScreenManager.LtOgthul.UpdateBrace(gameTime);

                //           }
                //           if (aosCurrentTeam == 0 && aosCurrentSelection == 2)
                //           {
                //               ScreenManager.Wisp.UpdateBrace(gameTime);
                //               ScreenManager.Loki.UpdateBrace(gameTime);
                //               ScreenManager.Michael.UpdateBrace(gameTime);
                //               ScreenManager.Dargahe.UpdateBrace(gameTime);
                //               ScreenManager.CaptThor.UpdateBrace(gameTime);
                //               ScreenManager.CaptWisp.UpdateBrace(gameTime);
                //               ScreenManager.LtHercules.UpdateHercules(gameTime);
                //               ScreenManager.LtOgthul.UpdateBrace(gameTime);

                //           }

                //       }
                //       else
                //           {
                //               ScreenManager.Wisp.UpdateBrace(gameTime);
                //               ScreenManager.Loki.UpdateBrace(gameTime);
                //               ScreenManager.Michael.UpdateBrace(gameTime);
                //               ScreenManager.Dargahe.UpdateBrace(gameTime);
                //               ScreenManager.CaptThor.UpdateBrace(gameTime);
                //               ScreenManager.CaptWisp.UpdateBrace(gameTime);
                //               ScreenManager.LtHercules.UpdateBrace(gameTime);
                //               ScreenManager.LtOgthul.UpdateBrace(gameTime);
                //           }
                //       //ScreenManager.Dargahe.UpdatePerseus(gameTime);





                //   }
                //   if (RageOfTheMachine)
                //   {

                //       ScreenManager.AchillesRage1.UpdateAchillesRage1(gameTime);
                //       ScreenManager.UndeadLeader.UpdateUndeadLeader(gameTime);


                //   }
                //   if (RageAgainstTheMachine)
                //   {
                //       ScreenManager.AchillesRage1.UpdateAchillesRage1(gameTime);
                //       ScreenManager.UndeadLeader.UpdateUndeadLeader2(gameTime);

                //   }
                //   if (HerculesVsThorRun)
                //   {

                //       checkCollisions();
                //       if (HercFinishThor)
                //       {
                //           ScreenManager.HercTVH.UpdateHercFinishThor(gameTime);
                //          ScreenManager.ThorTVH.UpdateThorAgainstHercules(gameTime);

                //       }
                //       else
                //       {
                //           ScreenManager.HercTVH.UpdateHercules(gameTime);
                //           ScreenManager.ThorTVH.UpdateThorAgainstHercules(gameTime);
                //       }







                //   }

                //   if (TheseusStandRun)
                //   {
                //       ScreenManager.TheseusTS.UpdateTheseusTS(gameTime);
                //       ScreenManager.MinoTS.UpdateTheseusStandAsterion(gameTime);
                //       ScreenManager.ParisTS.UpdateParisTS(gameTime);


                //       if (rageBlocks > 4)
                //           TheseusStandRun2 = true;
                //       if (TheseusStandRun2)
                //           ScreenManager.GorgonTS.UpdateParisTS(gameTime);

                //   } 


                //if (EternalStruggleRun )

                //       {
                //           if(currentIndex == 0)
                //           ScreenManager.Hercules.UpdateHercules(gameTime);
                //           if (currentIndex == 1)
                //               ScreenManager.Theseus.UpdateTheseus(gameTime);
                //           if (currentIndex == 2)
                //               ScreenManager.Perseus.UpdatePerseus(gameTime);


                //          // Console.WriteLine(ScreenManager.Hercules.World.Translation);


                //           //ScreenManager.Perseus.UpdatePerseus(gameTime);


                //           //ScreenManager.Theseus.UpdateTheseus(gameTime);


                //       }

                //if (aosMenu)
                //{
                //    PlayerIndex pindex;
                //    if (!isTarget)
                //    {
                //        if (ScreenManager.globalInput.IsMenuUp(null))
                //        {

                //            heroActiveMenu.SelectedEntry--;

                //            if (heroActiveMenu.SelectedEntry < 0)
                //                heroActiveMenu.SelectedEntry = heroActiveMenu.MenuEntries.Count - 1;

                //        }
                //        if (ScreenManager.globalInput.IsMenuDown(null))
                //        {
                //            heroActiveMenu.SelectedEntry++;

                //            if (heroActiveMenu.SelectedEntry >= heroActiveMenu.MenuEntries.Count)
                //                heroActiveMenu.SelectedEntry = 0;
                //        }

                //        if (ScreenManager.globalInput.IsMenuSelect(null, out pindex))
                //        {
                //            heroActiveMenu.MenuEntries[heroActiveMenu.SelectedEntry].OnSelectEntry(pindex);
                //        }

                //        for (int i = 0; i < heroActiveMenu.MenuEntries.Count; i++)
                //        {
                //            bool isSelected = IsActive && (i == heroActiveMenu.SelectedEntry);
                //            heroActiveMenu.MenuEntries[i].Update(this, isSelected, gameTime);

                //        }



                //    }
                //    if (isTarget)
                //    {

                //        if(ScreenManager.globalInput.IsMenuUp(null))
                //        {
                //            targetY++;

                //        }
                //        if (ScreenManager.globalInput.IsMenuDown(null))
                //        {
                //            targetY--;

                //        }
                //        if (ScreenManager.globalInput.IsMenuLeft(null))
                //        {
                //            targetX++;

                //        }
                //        if (ScreenManager.globalInput.IsMenuRight(null))
                //        {
                //            targetX--;

                //        }
                //        if (ScreenManager.globalInput.IsMenuSelect(null, out pindex))
                //        {
                //            //isTarget = false;
                //        }
                //    }
                //}

                //   if (activeMenu & !activeMenuActivated)
                //   {

                //      // ScreenManager.AddScreen(new ActiveMenu(), PlayerIndex.One);
                //       activeMenuActivated = true;

                //   }
                //   if (PreAosDialog)
                //   {


                //       if (ScreenManager.globalInput.IsMenuUp(null))
                //       {

                //           AosIntro.selectedEntry--;

                //           if (AosIntro.selectedEntry < 0)
                //               AosIntro.selectedEntry = AosIntro.menuEntries.Count - 1;

                //       }
                //       if (ScreenManager.globalInput.IsMenuDown(ControllingPlayer))
                //       {
                //           AosIntro.selectedEntry++;

                //           if (AosIntro.selectedEntry >= AosIntro.menuEntries.Count)
                //               AosIntro.selectedEntry = 0;
                //       }
                //       PlayerIndex pindex;
                //       if (ScreenManager.globalInput.IsMenuSelect(null, out pindex))
                //       {
                //           AosIntro.menuEntries[AosIntro.selectedEntry].OnSelectEntry(pindex);
                //       }

                //       for (int i = 0; i < AosIntro.menuEntries.Count; i++)
                //       {
                //           bool isSelected = IsActive && (i == AosIntro.selectedEntry);
                //           AosIntro.menuEntries[i].Update(this, isSelected, gameTime);

                //       }

                //   }
                //   if(AchillesAlive)
                //   {
                //       if (ScreenManager.globalInput.IsMenuUp(null))
                //       {

                //           Pluto.selectedEntry--;

                //           if (Pluto.selectedEntry < 0)
                //               Pluto.selectedEntry = Pluto.menuEntries.Count - 1;

                //       }
                //       if (ScreenManager.globalInput.IsMenuDown(ControllingPlayer))
                //       {
                //           Pluto.selectedEntry++;

                //           if (Pluto.selectedEntry >= Pluto.menuEntries.Count)
                //               Pluto.selectedEntry = 0;
                //       }
                //       PlayerIndex pindex;
                //       if (ScreenManager.globalInput.IsMenuSelect(null, out pindex))
                //       {
                //           Pluto.menuEntries[Pluto.selectedEntry].OnSelectEntry(pindex);
                //       }

                //       for (int i = 0; i < Pluto.menuEntries.Count; i++)
                //       {
                //           bool isSelected = IsActive && (i == Pluto.selectedEntry);
                //           Pluto.menuEntries[i].Update(this, isSelected, gameTime);

                //       }




                //   }
                //   if (PerseusResurrection)
                //   {


                //       if (ScreenManager.globalInput.IsMenuUp(null))
                //       {

                //           PerseusBetrayal2.selectedEntry--;

                //           if (PerseusBetrayal2.selectedEntry < 0)
                //               PerseusBetrayal2.selectedEntry = PerseusBetrayal2.menuEntries.Count - 1;

                //       }
                //       if (ScreenManager.globalInput.IsMenuDown(ControllingPlayer))
                //       {
                //           PerseusBetrayal2.selectedEntry++;

                //           if (PerseusBetrayal2.selectedEntry >= PerseusBetrayal1.menuEntries.Count)
                //               PerseusBetrayal2.selectedEntry = 0;
                //       }
                //       PlayerIndex pindex;
                //       if (ScreenManager.globalInput.IsMenuSelect(null, out pindex))
                //       {
                //           PerseusBetrayal2.menuEntries[PerseusBetrayal2.selectedEntry].OnSelectEntry(pindex);
                //       }

                //       for (int i = 0; i < PerseusBetrayal2.menuEntries.Count; i++)
                //       {
                //           bool isSelected = IsActive && (i == PerseusBetrayal2.selectedEntry);
                //           PerseusBetrayal2.menuEntries[i].Update(this, isSelected, gameTime);

                //       }

                //   }
                //   if (PerseusBetrayal)
                //   {

                //       if (ScreenManager.globalInput.IsMenuUp(null))
                //       {

                //           PerseusBetrayal1.selectedEntry--;

                //           if (PerseusBetrayal1.selectedEntry < 0)
                //               PerseusBetrayal1.selectedEntry = PerseusBetrayal1.menuEntries.Count - 1;

                //       }
                //       if (ScreenManager.globalInput.IsMenuDown(ControllingPlayer))
                //       {
                //           PerseusBetrayal1.selectedEntry++;

                //           if (PerseusBetrayal1.selectedEntry >= PerseusBetrayal1.menuEntries.Count)
                //               PerseusBetrayal1.selectedEntry = 0;
                //       }
                //       PlayerIndex pindex;
                //       if (ScreenManager.globalInput.IsMenuSelect(null, out pindex))
                //       {
                //           PerseusBetrayal1.menuEntries[PerseusBetrayal1.selectedEntry].OnSelectEntry(pindex);
                //       }

                //       for (int i = 0; i < PerseusBetrayal1.menuEntries.Count; i++)
                //       {
                //           bool isSelected = IsActive && (i == PerseusBetrayal1.selectedEntry);
                //           PerseusBetrayal1.menuEntries[i].Update(this, isSelected, gameTime);

                //       }







                //   }
                //   if (isTheseusOfferedQuest)
                //   {
                //       if (ScreenManager.globalInput.IsMenuUp(null))
                //       {

                //          TheseusQuestOne.SelectedEntry--;

                //          if (TheseusQuestOne.SelectedEntry < 0)
                //              TheseusQuestOne.SelectedEntry = TheseusQuestOne.MenuEntries.Count - 1;

                //       }
                //       if (ScreenManager.globalInput.IsMenuDown(ControllingPlayer))
                //       {
                //           TheseusQuestOne.SelectedEntry++;

                //           if (TheseusQuestOne.SelectedEntry >= TheseusQuestOne.MenuEntries.Count)
                //               TheseusQuestOne.SelectedEntry = 0;
                //       }
                //       PlayerIndex pindex;
                //       if (ScreenManager.globalInput.IsMenuSelect(null, out pindex))
                //       {
                //           TheseusQuestOne.MenuEntries[TheseusQuestOne.SelectedEntry].OnSelectEntry(pindex);
                //       }

                //       for (int i = 0; i < TheseusQuestOne.MenuEntries.Count; i++)
                //       {
                //           bool isSelected = IsActive && (i == TheseusQuestOne.SelectedEntry);
                //           TheseusQuestOne.MenuEntries[i].Update(this, isSelected, gameTime);

                //       }



                //   }
                //   if (isHercOfferedQuest)
                //   {



                //       if (ScreenManager.globalInput.IsMenuUp(null))
                //       {

                //           HercQuestOne.SelectedEntry--;

                //           if ( HercQuestOne.SelectedEntry < 0)
                //                HercQuestOne.SelectedEntry =  HercQuestOne.MenuEntries.Count - 1;

                //       }
                //       if (ScreenManager.globalInput.IsMenuDown(ControllingPlayer))
                //       {
                //            HercQuestOne.SelectedEntry++;

                //           if ( HercQuestOne.SelectedEntry >=  HercQuestOne.MenuEntries.Count)
                //                HercQuestOne.SelectedEntry = 0;
                //       }
                //       PlayerIndex pindex;
                //       if (ScreenManager.globalInput.IsMenuSelect(null, out pindex))
                //       {
                //            HercQuestOne.MenuEntries[HercQuestOne.SelectedEntry].OnSelectEntry(pindex);
                //       }

                //       for (int i = 0; i < HercQuestOne.MenuEntries.Count; i++)
                //       {
                //           bool isSelected = IsActive && (i == HercQuestOne.SelectedEntry);
                //           HercQuestOne.MenuEntries[i].Update(this, isSelected, gameTime);

                //       }

                //   }

                //   if (activeMenu)
                //   {
                //       if (!isTarget)
                //       {
                //           if (ScreenManager.globalInput.IsMenuUp(null))
                //           {

                //               heroActiveMenu.SelectedEntry--;

                //               if (heroActiveMenu.SelectedEntry < 0)
                //                   heroActiveMenu.SelectedEntry = heroActiveMenu.MenuEntries.Count - 1;

                //           }
                //           if (ScreenManager.globalInput.IsMenuDown(ControllingPlayer))
                //           {
                //               heroActiveMenu.SelectedEntry++;

                //               if (heroActiveMenu.SelectedEntry >= heroActiveMenu.MenuEntries.Count)
                //                   heroActiveMenu.SelectedEntry = 0;
                //           }
                //           PlayerIndex pindex;
                //           if (ScreenManager.globalInput.IsMenuSelect(null, out pindex))
                //           {
                //              heroActiveMenu.MenuEntries[heroActiveMenu.SelectedEntry].OnSelectEntry(pindex);
                //           }

                //           for (int i = 0; i < heroActiveMenu.MenuEntries.Count; i++)
                //           {
                //               bool isSelected = IsActive && (i == heroActiveMenu.SelectedEntry);
                //               heroActiveMenu.MenuEntries[i].Update(this, isSelected, gameTime);

                //           }
                //       }
                //       if (isTarget)
                //       {

                //           if (ScreenManager.globalInput.IsMenuUp(null))
                //           {
                //               targetIndex--;
                //           }
                //           if (ScreenManager.globalInput.IsMenuDown(null))
                //           {
                //               targetIndex++;
                //           }

                //           if (targetIndex < 0)
                //               targetIndex = 1;
                //           if (targetIndex > 1)
                //               targetIndex = 0;
                //           //
                //           Console.WriteLine(targetIndex);



                //       }




                //   }

                //   if (Vector3.Distance(ScreenManager.Hercules.World.Translation, new Vector3(1100.0f, 0.0f, 2164.8f)) < 200.0f &! HercDeclineOrCancelQuest &! HercAcceptQuest)
                //   {
                //       isHercOfferedQuest = true;
                //      EternalStruggleRun = false;
                //   }

                //   if (Vector3.Distance(ScreenManager.Theseus.World.Translation, new Vector3(1349.0f, 0.0f, 2770.0f)) < 200.0f & !TheseusDeclineOrCancelQuest &! TheseusAcceptQuest)
                //   {
                //       isTheseusOfferedQuest = true;
                //       EternalStruggleRun = false;
                //   }
                //   if (Vector3.Distance(ScreenManager.Perseus.World.Translation, new Vector3(2200.0f, 0.0f, 810)) < 200.0f &! PerseusTheTraitor)
                //   {
                //       PerseusBetrayal = true;
                //       EternalStruggleRun = false;

                //   }
                //   if (Vector3.Distance(ScreenManager.Perseus.World.Translation, new Vector3(1463.0f, 0.0f, 1306.0f)) < 200.0f & !AchillesAlive)
                //   {
                //       EternalStruggleRun = false;
                //       PerseusResurrection = true;
                //   }
                #endregion
            }
            
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        }
        public bool disposeButtons(RageHit ragehit)
        {

            return ragehit.dispose;

        }
        public void DrawArena(GameTime gameTime)
        {


            Vector3 lightDirection = Vector3.Normalize(new Vector3(3, -1, 1));
            Vector3 lightColor = new Vector3(0.3f, 0.4f, 0.2f);

            // Time is scaled down to make things wave in the wind more slowly.
            float time = (float)gameTime.TotalGameTime.TotalSeconds * 0.333f;

            bool draw = false;


            Matrix[] transforms = new Matrix[ScreenManager.arena.Bones.Count];
            ScreenManager.arena.CopyAbsoluteBoneTransformsTo(transforms);

            ScreenManager.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
            foreach (ModelMesh mesh in ScreenManager.arena.Meshes)
            {
                if (mesh.Name == "Zeus" || mesh.Name == "Pluto" || mesh.Name == "Ares" || mesh.Name == "Plane001")// || mesh.Name == "closedGate" || mesh.Name == "Box004")
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
                        else if (mesh.Name == "Box004")
                        {

                            effect.Parameters["Texture"].SetValue(ScreenManager.rock);
                        }
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.white);

                        effect.CurrentTechnique = effect.Techniques["DrawWithShadowMap"];
                        ///added by NSIII
                        //effect.Parameters["Alpha"].SetValue(1.0f);
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
            transforms = new Matrix[ScreenManager.grassModel.Bones.Count];
            ScreenManager.grassModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.grassModel.Meshes)
            {
                foreach (AlphaTestEffect effect in mesh.Effects)
                {

                    effect.World = transforms[mesh.ParentBone.Index];
                    effect.View = ScreenManager.camera.View;
                    effect.Projection = ScreenManager.camera.Projection;
                    effect.Texture = ScreenManager.grass;

                    //effect.CurrentTechnique.Passes[0].Apply();
                }

                mesh.Draw();
            }





            //foreach (boundingSphere bs in ScreenManager.creteSpheres)
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Green);



        }
        public void DrawAosBoard(GameTime gameTime)
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
                if (mesh.Name == "aosTerrain")
                {
                    foreach (Effect effect in mesh.Effects)
                    {

                        //if (mesh.Name == "openGate")
                        //    effect.Parameters["Texture"].SetValue(ScreenManager.slashTga);
                        //else if (mesh.Name == "closedGate")
                        //    effect.Parameters["Texture"].SetValue(ScreenManager.fire);
                        //else
                        //if (mesh.Name == "Plane001")
                         //   effect.Parameters["Texture"].SetValue(ScreenManager.road);
                       // else
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
        public void DrawGrid(GameTime gameTime)
        {

            Matrix[] transforms = new Matrix[ScreenManager.grid.Bones.Count];
            ScreenManager.grid.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.grid.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.TextureEnabled = true;
                    effect.Texture = ScreenManager.collision;
                    effect.World = transforms[mesh.ParentBone.Index];
                    effect.View = ScreenManager.camera.View;
                    effect.Projection = ScreenManager.camera.Projection;

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
                        if(mesh.Name == "Plane001")
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
                    }
                    // if ((mesh.Name == "openGate" && ScreenManager.gateOpen) || (mesh.Name == "closedGate" & !ScreenManager.gateOpen) || (mesh.Name != "openGate" && mesh.Name != "closedGate"))
                    
                        mesh.Draw();
                }
            }
            transforms = new Matrix[ScreenManager.grassModel.Bones.Count];
            ScreenManager.grassModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.grassModel.Meshes)
            {
                foreach(AlphaTestEffect effect in mesh.Effects)
                {

                    effect.World = transforms[mesh.ParentBone.Index];
                    effect.View = ScreenManager.camera.View;
                    effect.Projection = ScreenManager.camera.Projection;
                    effect.Texture = ScreenManager.grass;

                    //effect.CurrentTechnique.Passes[0].Apply();
               }

                mesh.Draw();
            }

            



            //foreach (boundingSphere bs in ScreenManager.creteSpheres)
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Green);



        }
        public void DrawHerculesProjectiles(JuneXnaModel player)
        {

            Matrix world = new Matrix();
            Vector3 scale, trans;
            Quaternion rota;
            player.formation.Decompose(out scale, out rota, out trans);
            Matrix[] transforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if (currentIndex == 0 && mesh.Name == "LeaderStar")
                {

                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.TextureEnabled = false;
                        effect.World = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);
                        effect.View = ScreenManager.camera.View;
                        effect.Projection = ScreenManager.camera.Projection;
                        effect.DiffuseColor = Color.Yellow.ToVector3();


                    }
                    mesh.Draw();
                }
                int mod = 0; 
                if (mesh.Name == "Plane001" && player.lightninged)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.TextureEnabled = true;
                        effect.World = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);
                        effect.View = ScreenManager.camera.View;
                        effect.Projection = ScreenManager.camera.Projection;
                        mod = (int)(player.lightningTimer.Ticks) % 3;
                        if(mod == 1)
                        effect.Texture = ScreenManager.lightning1;
                        if (mod == 2)
                            effect.Texture = ScreenManager.lightning2;
                        if (mod == 0)
                            effect.Texture = ScreenManager.lightning3;


                    }
                    mesh.Draw();



                }






            }




        }
        public void DrawHercules(JuneXnaModel player)
        {
            Matrix[] transforms;
            bool draw = false;

            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                draw = false;
                if (mesh.Name == "Alecto" || mesh.Name == "ponytail" || mesh.Name == "Scalp" || mesh.Name == "eyeball" || mesh.Name == "EyeBrow")
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.CurrentTechnique = effect.Techniques["SkinnedEffect"];

                        effect.Parameters["DiffuseColor"].SetValue(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
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
                        {
                            effect.Parameters["Texture"].SetValue(ScreenManager.humanTex);
                            draw = true;
                        }
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.black);

                        

                        effect.Parameters["Bones"].SetValue(player.SkinTrans);
                        effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);

                        //if (mesh.Name == "sHair")
                        //    effect.Parameters["Texture"].SetValue(ScreenManager.white);
                        //if (mesh.Name == "longHairF")
                        ////    effect.Parameters["Texture"].SetValue(ScreenManager.white);
                        //if (mesh.Name == "longHairB")

                        //    effect.Parameters["Texture"].SetValue(ScreenManager.humanTex);



                    }

                    mesh.Draw();
                }
            }
                       Matrix world = new Matrix();
           Vector3 scale, trans;
           Quaternion rota;
            player.formation.Decompose(out scale, out rota, out trans);
            Matrix []tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];

            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if (mesh.Name == "Sphere002")
                {
                    ScreenManager.HercTVH.World.Decompose(out scale, out rota, out trans);

                    //ScreenManager.ThorTVH.arrowWorld =Matrix.CreateScale(scale) *  Matrix.CreateTranslation(transforms[mesh.ParentBone.Index].Translation) * ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];// *ScreenManager.ThorTVH.World;
                    // ScreenManager.ThorTVH.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.ThorTVH.World.Translation), rota);
                    ScreenManager.HercTVH.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);// *ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];
                }

            }

            //foreach (Projectile2 proj in ScreenManager.HercTVH.projectiles)
            //{
            //    foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            //    {
            //        if (mesh.Name == "Sphere002")
            //        {
            //            foreach (BasicEffect effect in mesh.Effects)
            //            {
            //                //transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);
            //                effect.World = proj.world;
            //                effect.EnableDefaultLighting();
            //                effect.View = ScreenManager.camera.View;
            //                effect.Projection = ScreenManager.camera.Projection;

            //            }
            //            mesh.Draw();
            //        }



            //    }
            //}
        

            player.formation.Decompose(out scale, out rota, out trans);
          tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if (currentIndex == 0 && mesh.Name == "LeaderStar")
                {

                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);
                        effect.View = ScreenManager.camera.View;
                        effect.Projection = ScreenManager.camera.Projection;
                        effect.DiffuseColor = Color.Yellow.ToVector3();


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
                    if(mesh.Name == "KnockBackCheck")
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
            foreach (boundingSphere bs in player.knockBackSphere)
            {
                BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);

            }

            //foreach (boundingSphere bs in player.spheres)
            //{
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);

            //}
            //foreach (boundingSphere bs in player.physicalSphere)
            //{
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Black);
            //}


        }

        public void DrawMichael(JuneXnaModel player)
        {


            Matrix[] transforms;

            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" || mesh.Name == "TorsoPlate" || mesh.Name == "Helmet" || mesh.Name == "ponytail" || mesh.Name == "shin"
                    || mesh.Name == "RSword" || mesh.Name == "RoundShield" || mesh.Name == "Cylinder01" || mesh.Name == "Scalp" ||  mesh.Name == "eyeball" || mesh.Name == "EyeBrow"
                    || mesh.Name == "wings")
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.CurrentTechnique = effect.Techniques["SkinnedEffect"];

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
                if (currentIndex == 0 && targetIndex == 0 && mesh.Name == "LeaderStar")
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
            foreach(boundingSphere bs in player.rSword)
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
        public void DrawDargahe(JuneXnaModel player)
        {


            Matrix[] transforms;

            transforms = new Matrix[ScreenManager.dargahe.Bones.Count];
            ScreenManager.dargahe.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.dargahe.Meshes)
            {
                if (mesh.Name == "DarGahe" || mesh.Name == "Axe" || mesh.Name == "eyeball" || mesh.Name == "EyeBrow" || mesh.Name == "sHair"
                    || mesh.Name == "lHairF" || mesh.Name == "lHairB" || mesh.Name == "ponytail" || mesh.Name == "Scalp")
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.CurrentTechnique = effect.Techniques["SkinnedEffect"];

                        effect.Parameters["DiffuseColor"].SetValue(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
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
                            effect.Parameters["Texture"].SetValue(ScreenManager.humanTex);

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
                if (currentIndex == 0 && targetIndex == 0 && mesh.Name == "LeaderStar")
                {

                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);
                        effect.View = ScreenManager.camera.View;
                        effect.Projection = ScreenManager.camera.Projection;
                        effect.DiffuseColor = Color.Black.ToVector3();

                    }
                    mesh.Draw();
                }




            }


        }
        public void DrawPerseus(JuneXnaModel player)
        {


            Matrix[] transforms;

            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" || mesh.Name == "TorsoPlate" || mesh.Name == "lHairF" || mesh.Name == "ponytail" || mesh.Name == "shin"
                    || mesh.Name == "LSpear" || mesh.Name == "RSpear" || mesh.Name == "GreekPants" || mesh.Name == "Scalp"  || mesh.Name == "eyeball" || mesh.Name == "EyeBrow")
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
            player.formation.Decompose(out scale, out rota, out trans);
            Matrix []tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if (currentIndex == 0 && targetIndex == 0 && mesh.Name == "LeaderStar")
                {

                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);
                        effect.View = ScreenManager.camera.View;
                        effect.Projection = ScreenManager.camera.Projection;
                        effect.DiffuseColor = Color.Black.ToVector3();

                    }
                    mesh.Draw();
                }




                }


        }
        public void DrawLoki(JuneXnaModel player)
        {


            Matrix[] transforms;

            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" || mesh.Name == "TorsoPlate" || mesh.Name == "lHairF" || mesh.Name == "ponytail" || mesh.Name == "shin"
                    || mesh.Name == "LSpear" || mesh.Name == "RSpear" || mesh.Name == "GreekPants" || mesh.Name == "Scalp" || mesh.Name == "eyeball" || mesh.Name == "EyeBrow")
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
                            effect.Parameters["Texture"].SetValue(ScreenManager.frostTex);
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

 

            Vector3 scale, trans;
            Quaternion rota;

            transforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if (mesh.Name == "LSpear2")
                {
                    ScreenManager.Loki.World.Decompose(out scale, out rota, out trans);

                    //ScreenManager.ThorTVH.arrowWorld =Matrix.CreateScale(scale) *  Matrix.CreateTranslation(transforms[mesh.ParentBone.Index].Translation) * ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];// *ScreenManager.ThorTVH.World;
                    // ScreenManager.ThorTVH.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.ThorTVH.World.Translation), rota);
                    ScreenManager.Loki.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);// *ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];
                    
                }
                if (mesh.Name == "impaleRSpear")
                {

                    ScreenManager.Loki.World.Decompose(out scale, out rota, out trans);
                    ScreenManager.Loki.spearWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);// *ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];
                }

            }

            foreach (Projectile2 proj in ScreenManager.Loki.projectiles)
            {

                foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
                {  
                    if (mesh.Name == "impaleRSpear")
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
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.rSpear[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    if (mesh.Name == "KnockBackCheck")
                    {


                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.knockBackSphere[0] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));
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
            foreach (boundingSphere bs in player.rSpear)
            {
                BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);

            }
            foreach (boundingSphere bs in player.knockBackSphere)
            {
                BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);

            }
            foreach (boundingSphere bs in player.spheres)
            {

                BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);
            }


            Matrix world = new Matrix();
            //Vector3 scale, trans;
            //Quaternion rota;

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
                {
                    player.CFormation[7] = world.Translation;

                }
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


        }
        public void DrawUndead(JuneXnaModel player)
        {
            Matrix[] transforms;

            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" ||
                    mesh.Name == "RSword" || mesh.Name == "GreekPants")
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
                            effect.Parameters["Texture"].SetValue(ScreenManager.white);
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
            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if (mesh.Name == "LeaderStar")
                {

                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);
                        effect.View = ScreenManager.camera.View;
                        effect.Projection = ScreenManager.camera.Projection;
                        if (targetIndex == 0)
                            effect.DiffuseColor = Color.Black.ToVector3();
                        else
                            effect.DiffuseColor = Color.White.ToVector3();

                    }
                    mesh.Draw();


                }


            }
        }
        public void DrawAchilles(JuneXnaModel player)
        {
            Matrix[] transforms;

            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" || mesh.Name == "TorsoPlate" || mesh.Name == "lHairF" || mesh.Name == "lHairB" || mesh.Name == "shin"
                    || mesh.Name == "RoundShield" || mesh.Name == "RSword" || mesh.Name == "GreekPants" || mesh.Name == "Scalp" || mesh.Name == "eyeball" || mesh.Name == "EyeBrow")
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
            //foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            //{
            //    if (mesh.Name == "LeaderStar")
            //    {

            //        foreach (BasicEffect effect in mesh.Effects)
            //        {
            //            effect.World = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);
            //            effect.View = ScreenManager.camera.View;
            //            effect.Projection = ScreenManager.camera.Projection;
            //            if (targetIndex == 0)
            //                effect.DiffuseColor = Color.Black.ToVector3();
            //            else
            //                effect.DiffuseColor = Color.White.ToVector3();

            //        }
            //        mesh.Draw();


            //    }


            //}
        }

        public void DrawFWiz(JuneXnaModel actor, string nameEffect)
        {
            // ScreenManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            //RayRenderer.Render(actor.forwardRay, 200.0f, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);
           // foreach (Ray ray in actor.rays)
            //    RayRenderer.Render(ray, 200.0f, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);
            Matrix[] transforms;
            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" || mesh.Name == "TorsoPlate" || mesh.Name == "lHairF" || mesh.Name == "ponytail" || mesh.Name == "shin"
                    ||  mesh.Name == "Cylinder01" || mesh.Name == "Scalp" || mesh.Name == "eyeball" || mesh.Name == "EyeBrow"
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
            // BoundingSphereRenderer.Render(actor.knockBackSphere[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
            //BoundingSphereRenderer.Render(actor.collisionS[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
            // foreach (boundingSphere bs in actor.rSpear)
            //     BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
            //  RayRenderer.Render(actor.ray, 200, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Black);
            //foreach (boundingSphere bs in actor.spheres)
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);

            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if (mesh.Name == "forwardSpell")
                {
                    actor.World.Decompose(out scale, out rota, out trans);

                    //ScreenManager.ThorTVH.arrowWorld =Matrix.CreateScale(scale) *  Matrix.CreateTranslation(transforms[mesh.ParentBone.Index].Translation) * ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];// *ScreenManager.ThorTVH.World;
                    // ScreenManager.ThorTVH.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.ThorTVH.World.Translation), rota);
                    actor.forwardSpell = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);// *ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];
                }



            }
            //makeCylinder(actor.Direction, actor.forwardSpell.Translation);
            BoundingSphereRenderer.Render(actor.areaSphere.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Green);
       //     makeCylinder(actor.vertices, actor.indices);
        //    DrawCylinder();

            foreach(horizontalRing hr in actor.hRs)
                foreach(horizontalCircle hc in hr.circles)
                    BoundingSphereRenderer.Render(hc.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Green);
            foreach(horizontalForward hf in actor.hFs)
                BoundingSphereRenderer.Render(hf.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Green);

            foreach (horizontalCircle hc in actor.hCs)
                BoundingSphereRenderer.Render(hc.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Green);

               foreach (Projectile2 proj in actor.eruptions)
            {

                BoundingSphereRenderer.Render(proj.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);
                BoundingSphereRenderer.Render(proj.BS2, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);
                BoundingSphereRenderer.Render(proj.BS3, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);
                
                
                
                BoundingSphereRenderer.Render(proj.BS4, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);
                ScreenManager.fireParticles.AddParticle(proj.BS.Center, Vector3.Zero);
                ScreenManager.fireParticles.AddParticle(proj.BS2.Center, Vector3.Zero);
                ScreenManager.fireParticles.AddParticle(proj.BS3.Center, Vector3.Zero);
                ScreenManager.fireParticles.AddParticle(proj.BS4.Center, Vector3.Zero);

            }
         //   BoundingSphereRenderer.Render(player.areaSphere.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);

               foreach (Projectile2 proj in actor.projectiles)
               {
                   if (proj.Name == "FireBlast")
                       DrawFire(proj.Translation);


                   foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
                   {
                       if (mesh.Name == "forwardSpell" && proj.Name == "Fire")
                       {
                           foreach (Effect effect in mesh.Effects)
                           {
                               //transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);
                               // effect.Alpha = 0.3f;
                               effect.CurrentTechnique = effect.Techniques["SkinnedEffect"];
                               effect.Parameters["World"].SetValue(proj.world);
                               //effect.EnableDefaultLighting();
                               effect.Parameters["View"].SetValue(ScreenManager.camera.View);
                               effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
                               effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);
                               effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                               effect.Parameters["Texture"].SetValue(ScreenManager.fire);

                               //effect.TextureEnabled = true;
                               //effect.Texture = ScreenManager.fire;
                               ScreenManager.fireParticles.AddParticle(proj.world.Translation, Vector3.Up);


                           }
                           mesh.Draw();
                       }
                   }
               }
            //foreach (Projectile2 proj in actor.projectiles)
            //{
            //    foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            //    {
            //        if ((mesh.Name == "LSpear2" && proj.Name == "Spear"))
            //        {
            //            foreach (BasicEffect effect in mesh.Effects)
            //            {
            //                //transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);
            //                effect.World = proj.world;
            //                effect.EnableDefaultLighting();
            //                effect.View = ScreenManager.camera.View;
            //                effect.Projection = ScreenManager.camera.Projection;

            //            }
            //            if (nameEffect == "SkinnedEffect")
            //                mesh.Draw();
            //        }


            //        if (mesh.Name == "BS")
            //        {

            //            targetMat = proj.world;
            //            targetMat.Decompose(out scale, out rota, out trans);
            //            proj.BS = new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X);


            //        }
            //        BoundingSphereRenderer.Render(proj.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);




            //    }
            //}
            //  ScreenManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        }
        public void DrawTheseus(JuneXnaModel player, string namedEffect)
        {

        //    RayRenderer.Render(player.ray, 200.0f, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);
         //   RayRenderer.Render(player.rRay, 200.0f, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);
         //   RayRenderer.Render(player.lRay, 200.0f, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);
            Matrix[] transforms;

            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" ||
                    mesh.Name == "TorsoPlate" ||
                //    mesh.Name == "lHairF" || mesh.Name == "lHairB" ||
                    mesh.Name == "shin"

                    || mesh.Name == "GreekPants"
                  //  || mesh.Name == "Scalp"
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
                        else if (mesh.Name == "sHair" || mesh.Name == "lHairF" || mesh.Name == "lHairB" || mesh.Name == "Scalp")
                            effect.Parameters["Texture"].SetValue(ScreenManager.black);
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.white);
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




            if (ScreenManager.Theseus.physicalTower)
            {
                BoundingSphereRenderer.Render(ScreenManager.Theseus.tower.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Black);
            }
            if (ScreenManager.Theseus.buildWall)
            {
                DrawWallPlace();
                
            }


            Matrix targetMat = new Matrix();
             transforms = new Matrix[ScreenManager.spearSphere.Bones.Count];
            ScreenManager.spearSphere.CopyAbsoluteBoneTransformsTo(transforms);
            int i = 0;
            int j = 0;
            int k = 0;
            int arrow = 0;
            foreach (ModelMesh mesh in ScreenManager.spearSphere.Meshes)
            {
                // draw = false;
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (mesh.Name == "arrowS1" || mesh.Name == "arrowS2" || mesh.Name == "arrowS3")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.arrow[arrow++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Z) ));


                    }
                    if (mesh.Name == "rSwordS1" || mesh.Name == "rSwordS2" || mesh.Name == "rSwordS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.rSword[k++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Z)));
                        if (player.isAtk1 || player.isAtk2 || player.isAtk3)
                        {
                            if (mesh.Name == "rSwordS1" && player.active)
                                player.slashPoints.Add(trans);
                            if (mesh.Name == "rSwordS3" && player.active)
                                player.slashPoints.Add(trans);
                        }

                    }

                    if (mesh.Name == "shieldS1" || mesh.Name == "shieldS2" || mesh.Name == "shieldS3" || mesh.Name == "shieldS4" || mesh.Name == "shieldS5")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.roundShield[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Z)));

                    }
                    //if (mesh.Name == "forwardSpell")
                    //{
                    //    player.World.Decompose(out scale, out rota, out trans);

                    //    player.forwardSpell = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);




                    //}
                    if (mesh.Name == "headS1")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.head];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Z)));


                    }
                    if (mesh.Name == "chestS")
                    {
                        
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.spine1];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Z)));
                        player.collisionS[0] = new boundingSphere("collisionS", new BoundingSphere(player.Position + new Vector3(0.0f, 70.0f, 0.0f), mesh.BoundingSphere.Radius * Math.Abs(scale.X) * 2.0f));
                        player.collisionS[1] = new boundingSphere("collisionS", new BoundingSphere(player.Position + new Vector3(0.0f, 70.0f, 0.0f), mesh.BoundingSphere.Radius * Math.Abs(scale.X) ));

                    }
                    if (mesh.Name == "hipS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.hips];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Z)));
                    }
                    if (mesh.Name == "lULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Z)));
                    }
                    if (mesh.Name == "lLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Z)));
                    }
                    if (mesh.Name == "rULegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rULeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Z)));
                    }
                    if (mesh.Name == "rLLegS")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rLLeg];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.spheres[i++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Z)));
                    }
                    //if (mesh.Name == "lFootS")
                    //{

                    //    targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rFoot];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    player.physicalSphere[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.Y));

                    //}
                }

            }
            //foreach (boundingSphere bs in player.spheres)
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);

            for (int a = 0; a < player.rSword.Count; a++)
                player.atkSpheres.Add(player.rSword[a].BS);

          //  for(int a= 0; a<player.atkSpheres.Count; a++)
              //  BoundingSphereRenderer.Render(player.atkSpheres[a], ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);

            player.formation.Decompose(out scale, out rota, out trans);
            Matrix[] tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);

            player.formation.Decompose(out scale, out rota, out trans);
            tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            foreach (Projectile2 proj in player.projectiles)

                ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                ScreenManager.Theseus.World.Decompose(out scale, out rota, out trans);
                //ScreenManager.Theseus.projectileWorld.Decompose(out scale, out rota, out trans);
                if (mesh.Name == "forwardSpell")
                {

                    player.forwardSpell = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);


                }
                if (mesh.Name == "RoundShield2")
                {

                    //ScreenManager.ThorTVH.arrowWorld =Matrix.CreateScale(scale) *  Matrix.CreateTranslation(transforms[mesh.ParentBone.Index].Translation) * ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];// *ScreenManager.ThorTVH.World;
                    // ScreenManager.ThorTVH.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.ThorTVH.World.Translation), rota);
                    ScreenManager.Theseus.shieldWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);// *ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];
                }
                if (mesh.Name == "Arrow2")
                {
                   // ScreenManager.Theseus.World.Decompose(out scale, out rota, out trans);
                    ScreenManager.Theseus.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);
                }


            }
                   //     ScreenManager.GraphicsDevice.BlendState = BlendState.Additive;
            
           // ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            foreach (Projectile2 proj in ScreenManager.Theseus.eruptions)
            {

                BoundingSphereRenderer.Render(proj.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);
                BoundingSphereRenderer.Render(proj.BS2, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);
                BoundingSphereRenderer.Render(proj.BS3, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);
                
                
                
                BoundingSphereRenderer.Render(proj.BS4, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);
                ScreenManager.fireParticles.AddParticle(proj.BS.Center, Vector3.Zero);
                ScreenManager.fireParticles.AddParticle(proj.BS2.Center, Vector3.Zero);
                ScreenManager.fireParticles.AddParticle(proj.BS3.Center, Vector3.Zero);
                ScreenManager.fireParticles.AddParticle(proj.BS4.Center, Vector3.Zero);

            }
            if (player.RingOfFire)
            {
                foreach(horizontalCircle hC in player.fireRings.circles)
                    BoundingSphereRenderer.Render(hC.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Firebrick);
            }
            if(player.IcyField)
                BoundingSphereRenderer.Render(player.IFSphere, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Aqua);
            BoundingSphereRenderer.Render(player.areaSphere.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);
            BoundingSphereRenderer.Render(player.TargetSphere, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);

            foreach (Construct construct in ScreenManager.Theseus.constructs)
            {
                
                BoundingSphereRenderer.Render(construct.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);
                

            }
            ModelMesh arrowMesh = ScreenManager.FinalProjectiles.Meshes["Arrow2"];
            foreach (Arrow arr in player.arrows)
            {
                foreach (Effect effect in arrowMesh.Effects)
                {
                    //transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);
                    // effect.Alpha = 0.3f;
                    effect.CurrentTechnique = effect.Techniques[namedEffect];
                    effect.Parameters["World"].SetValue(arr.world);
                    //effect.EnableDefaultLighting();
                    effect.Parameters["View"].SetValue(ScreenManager.camera.View);
                    effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
                    effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);
                    effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                    effect.Parameters["Texture"].SetValue(ScreenManager.white);
                }
                arrowMesh.Draw();
                for (int c = 0; c < 3; c++)
                    BoundingSphereRenderer.Render(arr.BSes[c], ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);
            }

            foreach (Projectile2 proj in ScreenManager.Theseus.projectiles)
            {
                if (proj.Name == "FireBlast")
                    DrawFire(proj.Translation);

               
                foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
                {
                    if (mesh.Name == "forwardSpell" && proj.Name == "Fire")
                    {
                        foreach (Effect effect in mesh.Effects)
                        {
                            //transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);
                           // effect.Alpha = 0.3f;
                            effect.CurrentTechnique = effect.Techniques[namedEffect];
                            effect.Parameters["World"].SetValue(proj.world);
                            //effect.EnableDefaultLighting();
                            effect.Parameters["View"].SetValue(ScreenManager.camera.View);
                            effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
                            effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);
                            effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                            effect.Parameters["Texture"].SetValue(ScreenManager.fire);

                            //effect.TextureEnabled = true;
                            //effect.Texture = ScreenManager.fire;
                            ScreenManager.fireParticles.AddParticle(proj.world.Translation, Vector3.Up);
                            
                            
                        }
                        mesh.Draw();
                    }
                    if (mesh.Name == "forwardSpell" && proj.Name == "Earth")
                    {
                        foreach (Effect effect in mesh.Effects)
                        {
                            //transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);
                            // effect.Alpha = 0.3f;
                            effect.CurrentTechnique = effect.Techniques[namedEffect];
                            effect.Parameters["World"].SetValue(proj.world);
                            //effect.EnableDefaultLighting();
                            effect.Parameters["View"].SetValue(ScreenManager.camera.View);
                            effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
                            effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);
                            effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                            effect.Parameters["Texture"].SetValue(ScreenManager.earth);

                            //effect.TextureEnabled = true;
                            //effect.Texture = ScreenManager.fire;
                           // ScreenManager.fireParticles.AddParticle(proj.world.Translation, Vector3.Up);


                        }
                        mesh.Draw();
                    }
                    if (mesh.Name == "forwardSpell" && proj.Name == "Wind")
                    {
                        foreach (Effect effect in mesh.Effects)
                        {
                            //transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);
                            // effect.Alpha = 0.3f;
                            effect.CurrentTechnique = effect.Techniques[namedEffect];
                            effect.Parameters["World"].SetValue(proj.world);
                            //effect.EnableDefaultLighting();
                            effect.Parameters["View"].SetValue(ScreenManager.camera.View);
                            effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
                            effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);
                            effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                            effect.Parameters["Texture"].SetValue(ScreenManager.wind);

                            //effect.TextureEnabled = true;
                            //effect.Texture = ScreenManager.fire;
                           // ScreenManager.fireParticles.AddParticle(proj.world.Translation, Vector3.Up);


                        }
                        mesh.Draw();
                    }
                    if (mesh.Name == "forwardSpell" && proj.Name == "Water")
                    {
                        foreach (Effect effect in mesh.Effects)
                        {
                            //transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);
                            // effect.Alpha = 0.3f;
                            effect.CurrentTechnique = effect.Techniques[namedEffect];
                            effect.Parameters["World"].SetValue(proj.world);
                            //effect.EnableDefaultLighting();
                            effect.Parameters["View"].SetValue(ScreenManager.camera.View);
                            effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
                            effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);
                            effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                            effect.Parameters["Texture"].SetValue(ScreenManager.water);

                            //effect.TextureEnabled = true;
                            //effect.Texture = ScreenManager.fire;
                            ScreenManager.fireParticles.AddParticle(proj.world.Translation, Vector3.Up);


                        }
                        mesh.Draw();
                    }

                   // ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
                    //ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                    if ((mesh.Name == "RoundShield2" && proj.Name == "Shield") ||
                        (mesh.Name == "Arrow2" && proj.Name == "Arrow"))
                    {
                        foreach (Effect effect in mesh.Effects)
                        {
                            //transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);
                            // effect.Alpha = 0.3f;
                            effect.CurrentTechnique = effect.Techniques[namedEffect];
                            effect.Parameters["World"].SetValue(proj.world);
                            //effect.EnableDefaultLighting();
                            effect.Parameters["View"].SetValue(ScreenManager.camera.View);
                            effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
                            effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);
                            effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                            effect.Parameters["Texture"].SetValue(ScreenManager.white);
                        }
                       // if(namedEffect == "SkinnedEffect")
                        mesh.Draw();
                    }


                    if (mesh.Name == "BS")
                    {

                        targetMat = proj.world;
                        targetMat.Decompose(out scale, out rota, out trans);
                        proj.BS = new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Z));


                    }
                 //   BoundingSphereRenderer.Render(proj.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);
                    
                  



                }
            }

            if (player.slashPoints.Count >= 3)
            {

               ScreenManager.primitiveBatch.Begin(PrimitiveType.TriangleList);
                
                for (int p = 0; p < player.slashPoints.Count-4; p+=2)
                {
                    ScreenManager.primitiveBatch.AddVertex(player.slashPoints[p], new Color(1.0f, 1.0f, 1.0f, 0.3f));
                    //ScreenManager.primitiveBatch.AddVertex(new Vector3(player.slashPoints[p].X + 10, player.slashPoints[p].Y + 10,player.slashPoints[p].Z + 10), Color.Blue);
                    ScreenManager.primitiveBatch.AddVertex(player.slashPoints[p + 1], new Color(1.0f, 1.0f, 1.0f, 0.3f));
                    ScreenManager.primitiveBatch.AddVertex(player.slashPoints[p + 2], new Color(1.0f, 1.0f, 1.0f, 0.3f));
                    ScreenManager.primitiveBatch.AddVertex(player.slashPoints[p + 2], new Color(1.0f, 1.0f, 1.0f, 0.3f));
                    ScreenManager.primitiveBatch.AddVertex(player.slashPoints[p + 3], new Color(1.0f, 1.0f, 1.0f, 0.3f));
                    ScreenManager.primitiveBatch.AddVertex(player.slashPoints[p + 1], new Color(1.0f, 1.0f, 1.0f, 0.3f));

                    
                    //ScreenManager.primitiveBatch.AddVertex(new Vector3(517.0f, 100.0f, 670.0f), Color.White);
                    //ScreenManager.primitiveBatch.AddVertex(new Vector3(516.0f, 100.0f, 670.0f), Color.White);
                    //ScreenManager.primitiveBatch.AddVertex(new Vector3(400.0f, 200.0f, 570.0f), Color.White);
                }
                //ScreenManager.primitiveBatch.AddVertex(player.slashPoints[1], new Color(1.0f, 1.0f, 1.0f, 0.3f));
                //ScreenManager.primitiveBatch.AddVertex(player.slashPoints[2], new Color(1.0f, 1.0f, 1.0f, 0.3f));

                //for(int i = 2; i<player.slashPoints.Count; i+=3)
                //{
                //ScreenManager.primitiveBatch.Begin(PrimitiveType.TriangleList);
                //ScreenManager.primitiveBatch.AddVertex(player.slashPoints
                //}

                ScreenManager.primitiveBatch.End();


            }

           // ScreenManager.fireParticles.AddParticle(player.CFormation[4], Vector3.Zero);

        }
        public void DrawSMove(JuneXnaModel actor, string nameEffect)
        {
            // ScreenManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            RayRenderer.Render(actor.forwardRay, 200.0f, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);
            foreach(Ray ray in actor.rays)
                RayRenderer.Render(ray, 200.0f, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);
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
                        actor.collisionS[0] = new boundingSphere("collisionS", new BoundingSphere(actor.Position + new Vector3(0.0f, 70.0f, 0.0f), mesh.BoundingSphere.Radius * scale.X * 2.0f));
                        actor.collisionS[1] = new boundingSphere("collisionS", new BoundingSphere(actor.Position + new Vector3(0.0f, 70.0f, 0.0f), mesh.BoundingSphere.Radius * scale.X * 2.0f));

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
            // BoundingSphereRenderer.Render(actor.knockBackSphere[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
            //BoundingSphereRenderer.Render(actor.collisionS[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
            // foreach (boundingSphere bs in actor.rSpear)
            //     BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
            //  RayRenderer.Render(actor.ray, 200, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Black);
            //foreach (boundingSphere bs in actor.spheres)
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);

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
                        if (nameEffect == "SkinnedEffect")
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
        public void DrawGates()
        {


            foreach (BoundingSphere bs in ScreenManager.gates)
            {




                BoundingSphereRenderer.Render(bs, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.DarkTurquoise);
            }


        }
        public void DrawTargetedRunner(JuneXnaModel actor, string nameEffect)
        {
            // ScreenManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            Matrix[] transforms;
            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" || mesh.Name == "lHairF" || mesh.Name == "ponytail" ||
                    mesh.Name == "GreekPants" || mesh.Name == "Scalp" || mesh.Name == "eyeball" || mesh.Name == "EyeBrow"
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
                        actor.collisionS[0] = new boundingSphere("collisionS", new BoundingSphere(actor.Position + new Vector3(0.0f, 70.0f, 0.0f), mesh.BoundingSphere.Radius * scale.X * 2.0f));
                        actor.collisionS[1] = new boundingSphere("collisionS", new BoundingSphere(actor.Position + new Vector3(0.0f, 70.0f, 0.0f), mesh.BoundingSphere.Radius * scale.X * 1.0f));


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
                }

            }


            BoundingSphereRenderer.Render(actor.collisionS[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Azure);


            foreach (Vector3 vec in actor.mWayPoints)
            {

                BoundingSphereRenderer.Render(new BoundingSphere(vec, 60), ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);

            }
        }
        public void DrawRunner(JuneXnaModel actor, string nameEffect)
        {
            // ScreenManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            Matrix[] transforms;
            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" || mesh.Name == "lHairF" || mesh.Name == "ponytail" ||
                    mesh.Name == "GreekPants" || mesh.Name == "Scalp" || mesh.Name == "eyeball" || mesh.Name == "EyeBrow"
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
                        actor.collisionS[0] = new boundingSphere("collisionS", new BoundingSphere(actor.Position + new Vector3(0.0f, 70.0f, 0.0f), mesh.BoundingSphere.Radius * scale.X * 2.0f));
                        actor.collisionS[1] = new boundingSphere("collisionS", new BoundingSphere(actor.Position + new Vector3(0.0f, 70.0f, 0.0f), mesh.BoundingSphere.Radius * scale.X * 1.0f));


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
                }

            }


            BoundingSphereRenderer.Render(actor.collisionS[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Azure);

          
            foreach (Vector3 vec in actor.mWayPoints)
            {

                BoundingSphereRenderer.Render(new BoundingSphere(vec, 60), ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);

            }
        }
        public void DrawFighter(JuneXnaModel actor, string nameEffect)
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
            int arrow = 0;
            foreach (ModelMesh mesh in ScreenManager.spearSphere.Meshes)
            {
                // draw = false;
                foreach (BasicEffect effect in mesh.Effects)
                {
                     if (mesh.Name == "arrowS1" || mesh.Name == "arrowS2" || mesh.Name == "arrowS3")
                    {
                        targetMat = transforms[mesh.ParentBone.Index] * actor.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        actor.arrow[arrow++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * Math.Abs(scale.Z)));


                    }
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
                        actor.collisionS[0] = new boundingSphere("collisionS", new BoundingSphere(actor.Position + new Vector3(0.0f, 70.0f, 0.0f), mesh.BoundingSphere.Radius * scale.X * 2.0f));
                        actor.collisionS[1] = new boundingSphere("collisionS", new BoundingSphere(actor.Position + new Vector3(0.0f, 70.0f, 0.0f), mesh.BoundingSphere.Radius * scale.X * 1.0f));
                        

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
            // BoundingSphereRenderer.Render(actor.knockBackSphere[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
            //BoundingSphereRenderer.Render(actor.collisionS[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
            // foreach (boundingSphere bs in actor.rSpear)
            //     BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
            //  RayRenderer.Render(actor.ray, 200, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Black);
            //foreach (boundingSphere bs in actor.spheres)
              //  BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);

//            foreach (boundingSphere bs in actor.spheres)
  //              BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);

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
                if (mesh.Name == "Arrow2")
                {
                    actor.World.Decompose(out scale, out rota, out trans);

                    //ScreenManager.ThorTVH.arrowWorld =Matrix.CreateScale(scale) *  Matrix.CreateTranslation(transforms[mesh.ParentBone.Index].Translation) * ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];// *ScreenManager.ThorTVH.World;
                    // ScreenManager.ThorTVH.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.ThorTVH.World.Translation), rota);
                    actor.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);// *ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];

                }


            }

            ModelMesh arrowMesh = ScreenManager.FinalProjectiles.Meshes["Arrow2"];
            foreach (Arrow arr in actor.arrows)
            {
                foreach (Effect effect in arrowMesh.Effects)
                {
                    //transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);
                    // effect.Alpha = 0.3f;
                    effect.CurrentTechnique = effect.Techniques["SkinnedEffect"];
                    effect.Parameters["World"].SetValue(arr.world);
                    //effect.EnableDefaultLighting();
                    effect.Parameters["View"].SetValue(ScreenManager.camera.View);
                    effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
                    effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);
                    effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                    effect.Parameters["Texture"].SetValue(ScreenManager.white);
                }
                arrowMesh.Draw();

            }

            foreach (Projectile2 proj in actor.projectiles)
            {
                foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
                {
                    if ((mesh.Name == "Arrow2" && proj.Name == "Arrow"))
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            //transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);
                            effect.World = proj.world;
                            effect.EnableDefaultLighting();
                            effect.View = ScreenManager.camera.View;
                            effect.Projection = ScreenManager.camera.Projection;

                        }
                        if (nameEffect == "SkinnedEffect")
                            mesh.Draw();
                    }

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
                        if (nameEffect == "SkinnedEffect")
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

            BoundingBoxRenderer.RenderBox(actor.collisionBox, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);
            //if (actor.moveState == 1 &&  actor.fighterIndex == 1)
            {
                for (int a = 0; a < 5; a++)
                    for (int b = 0; b < 5; b++)
                        BoundingBoxRenderer.RenderBox(actor.moveBoxes[a][b], ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);
               // for (int a = 0; a < actor.setSpheres.Count ; a++)
                 //   BoundingSphereRenderer.Render(actor.setSpheres[a], ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Black);
                //for (int a = 0; a < 5; a++)
                //    for (int b = 0; b < 5; b++)
                //    {
                //        if (actor.open[a][b])
                //            BoundingSphereRenderer.Render(actor.moveSpheres[a][b], ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Orange);
                //        else
                //            BoundingSphereRenderer.Render(actor.moveSpheres[a][b], ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Red);

                //    }


            }
            //for(int a = 0; a<3; a++)
            //    for(int b = 0; b<3; b++)
            //        BoundingSphereRenderer.Render(actor.IncrementalSpheres[a][b], ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Black);
            //for (int a = 0; a < actor.futureMovement.Count; a++)
            //    BoundingSphereRenderer.Render(actor.futureMovement[a], ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Azure);

            //BoundingSphereRenderer.Render(actor.collisionS[1].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Azure);

            //if(actor.fighterIndex == 0)
            //for (int a = 0; a < 5; a++)
            //    for (int b = 0; b < 5; b++)
            //    {
            //        if (actor.open[a][b])
            //            BoundingSphereRenderer.Render(actor.moveSpheres[a][b], ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Orange);
            //        else
            //            if (!actor.open[a][b])
            //                BoundingSphereRenderer.Render(actor.moveSpheres[a][b], ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Purple);
            //    }
      
                //BoundingSphereRenderer.Render(actor.collisionS[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Black);
            //foreach (List<BoundingSphere> ls in actor.moveSpheres)
            //    foreach (BoundingSphere bs in ls)
            //foreach (BoundingSphere bs in actor.setSpheres)
            //    {  
            //        BoundingSphereRenderer.Render(bs, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Orange);

            //    }

            //  ScreenManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;

                foreach (Vector3 vec in actor.mWayPoints)
                {

                    BoundingSphereRenderer.Render(new BoundingSphere(vec, 60), ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);

                }
        }

        public void DrawGhost(JuneXnaModel actor, string nameEffect)
        {
            // ScreenManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            Matrix[] transforms;
            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                if (mesh.Name == "Alecto" || mesh.Name == "GreekPants" || mesh.Name == "eyeball" || mesh.Name == "EyeBrow"
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
            // BoundingSphereRenderer.Render(actor.knockBackSphere[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
            //BoundingSphereRenderer.Render(actor.collisionS[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
            // foreach (boundingSphere bs in actor.rSpear)
            //     BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
            //  RayRenderer.Render(actor.ray, 200, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Black);
            //foreach (boundingSphere bs in actor.spheres)
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);

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
                        if (nameEffect == "SkinnedEffect")
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

            for (int a = 0; a < 5; a++)
                for (int b = 0; b < 5; b++)
                {
                    //  if(actor.open[a][b])
                    //      BoundingSphereRenderer.Render(actor.moveSpheres[a][b], ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Orange);
                    // else
                    //    if(!actor.open[a][b])
                    //        BoundingSphereRenderer.Render(actor.moveSpheres[a][b], ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Purple);
                }

            BoundingSphereRenderer.Render(actor.collisionS[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Black);
            //foreach (List<BoundingSphere> ls in actor.moveSpheres)
            //    foreach (BoundingSphere bs in ls)
            //foreach (BoundingSphere bs in actor.setSpheres)
            //    {  
            //        BoundingSphereRenderer.Render(bs, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Orange);

            //    }

            //  ScreenManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            foreach (Vector3 vec in actor.mWayPoints)
            {

                BoundingSphereRenderer.Render(new BoundingSphere(vec, 60), ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);

            }
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
           // BoundingSphereRenderer.Render(actor.knockBackSphere[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
            //BoundingSphereRenderer.Render(actor.collisionS[0].BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
           // foreach (boundingSphere bs in actor.rSpear)
           //     BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.AliceBlue);
          //  RayRenderer.Render(actor.ray, 200, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Black);
            //foreach (boundingSphere bs in actor.spheres)
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);
            
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
                        if(nameEffect == "SkinnedEffect")
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

        public void DrawWisp(JuneXnaModel player)
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
                            effect.Parameters["Texture"].SetValue(ScreenManager.deadTex);
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


 //           foreach (ModelMesh mesh in screenManager.humanFormation.Meshes)
 //           {
 //               world = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);

 //               if (mesh.Name == "0,0")
 //                   player.CFormation[0] = world.Translation;
 //               if (mesh.Name == "0,1")
 //                   player.CFormation[3] = world.Translation;
 //               if (mesh.Name == "0,2")
 //                   player.CFormation[6] = world.Translation;
 //               if (mesh.Name == "1,0")
 //                   player.CFormation[1] = world.Translation;
 //               if (mesh.Name == "1,1")
 //                   player.CFormation[4] = world.Translation;
 //               if (mesh.Name == "1,2")
 //                   player.CFormation[7] = world.Translation;
 //               if (mesh.Name == "2,0")
 //                   player.CFormation[2] = world.Translation;
 //               if (mesh.Name == "2,1")
 //                   player.CFormation[5] = world.Translation;
 //               if (mesh.Name == "2,2")
 //                   player.CFormation[8] = world.Translation;
 //               if (mesh.Name == "E0,0")
 //                   player.EFormation[0] = world.Translation;
 //               if (mesh.Name == "E0,1")
 //                   player.EFormation[3] = world.Translation;
 //               if (mesh.Name == "E0,2")
 //                   player.EFormation[6] = world.Translation;
 //               if (mesh.Name == "E1,0")
 //                   player.EFormation[1] = world.Translation;
 //               if (mesh.Name == "E1,1")
 //                   player.EFormation[4] = world.Translation;
 //               if (mesh.Name == "E1,2")
 //                   player.EFormation[7] = world.Translation;
 //               if (mesh.Name == "E2,0")
 //                   player.EFormation[2] = world.Translation;
 //               if (mesh.Name == "E2,1")
 //                   player.EFormation[5] = world.Translation;
 //               if (mesh.Name == "E2,2")
 //                   player.EFormation[8] = world.Translation;
 //               if (mesh.Name == "W0,0")
 //                   player.WFormation[0] = world.Translation;
 //               if (mesh.Name == "W0,1")
 //                   player.WFormation[3] = world.Translation;
 //               if (mesh.Name == "W0,2")
 //                   player.WFormation[6] = world.Translation;
 //               if (mesh.Name == "W1,0")
 //                   player.WFormation[1] = world.Translation;
 //               if (mesh.Name == "W1,1")
 //                   player.WFormation[4] = world.Translation;
 //               if (mesh.Name == "W1,2")
 //                   player.WFormation[7] = world.Translation;
 //               if (mesh.Name == "W2,0")
 //                   player.WFormation[2] = world.Translation;
 //               if (mesh.Name == "W2,1")
 //                   player.WFormation[5] = world.Translation;
 //               if (mesh.Name == "W2,2")
 //                   player.WFormation[8] = world.Translation;
 //               if (mesh.Name == "N0,0")
 //                   player.NFormation[0] = world.Translation;
 //               if (mesh.Name == "N0,1")
 //                   player.NFormation[3] = world.Translation;
 //               if (mesh.Name == "N0,2")
 //                   player.NFormation[6] = world.Translation;
 //               if (mesh.Name == "N1,0")
 //                   player.NFormation[1] = world.Translation;
 //               if (mesh.Name == "N1,1")
 //                   player.NFormation[4] = world.Translation;
 //               if (mesh.Name == "N1,2")
 //                   player.NFormation[7] = world.Translation;
 //               if (mesh.Name == "N2,0")
 //                   player.NFormation[2] = world.Translation;
 //               if (mesh.Name == "N2,1")
 //                   player.NFormation[5] = world.Translation;
 //               if (mesh.Name == "N2,2")
 //                   player.NFormation[8] = world.Translation;

 //               if (mesh.Name == "S0,0")
 //                   player.SFormation[0] = world.Translation;
 //               if (mesh.Name == "S0,1")
 //                   player.SFormation[3] = world.Translation;
 //               if (mesh.Name == "S0,2")
 //                   player.SFormation[6] = world.Translation;
 //               if (mesh.Name == "S1,0")
 //                   player.SFormation[1] = world.Translation;
 //               if (mesh.Name == "S1,1")
 //                   player.SFormation[4] = world.Translation;
 //               if (mesh.Name == "S1,2")
 //                   player.SFormation[7] = world.Translation;
 //               if (mesh.Name == "S2,0")
 //                   player.SFormation[2] = world.Translation;
 //               if (mesh.Name == "S2,1")
 //                   player.SFormation[5] = world.Translation;
 //               if (mesh.Name == "S2,2")
 //                   player.SFormation[8] = world.Translation;

 //               foreach (BasicEffect effect in mesh.Effects)
 //               {
 //                   effect.World = world;
 //                   effect.View = ScreenManager.camera.View;
 //                   effect.Projection = ScreenManager.camera.Projection;

 //                   if (mesh.Name == "S0,0" || mesh.Name == "S0,1" || mesh.Name == "S0,2"
 //                   || mesh.Name == "S1,0" || mesh.Name == "S1,1" || mesh.Name == "S1,2"
 //                       || mesh.Name == "S2,0" || mesh.Name == "S2,1" || mesh.Name == "S2,2")

 //                       effect.DiffuseColor = Color.WhiteSmoke.ToVector3();


 //                   if (mesh.Name == "N0,0" || mesh.Name == "N0,1" || mesh.Name == "N0,2"
 //                   || mesh.Name == "N1,0" || mesh.Name == "N1,1" || mesh.Name == "N1,2"
 //                       || mesh.Name == "N2,0" || mesh.Name == "N2,1" || mesh.Name == "N2,2")
 //                       effect.DiffuseColor = Color.Yellow.ToVector3();


 //                   if (mesh.Name == "E0,0" || mesh.Name == "E0,1" || mesh.Name == "E0,2"
 //|| mesh.Name == "E1,0" || mesh.Name == "E1,1" || mesh.Name == "E1,2"
 //|| mesh.Name == "E2,0" || mesh.Name == "E2,1" || mesh.Name == "E2,2")
 //                       effect.DiffuseColor = Color.Fuchsia.ToVector3();

 //                   if (mesh.Name == "W0,0" || mesh.Name == "W0,1" || mesh.Name == "W0,2"
 //|| mesh.Name == "W1,0" || mesh.Name == "W1,1" || mesh.Name == "W1,2"
 //|| mesh.Name == "W2,0" || mesh.Name == "W2,1" || mesh.Name == "W2,2")
 //                       effect.DiffuseColor = Color.Firebrick.ToVector3();

 //               }

 //               mesh.Draw();
 //           }
            #endregion


            player.formation.Decompose(out scale, out rota, out trans);
            Matrix[] tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if (mesh.Name == "LeaderStar")
                {

                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);
                        effect.View = ScreenManager.camera.View;
                        effect.Projection = ScreenManager.camera.Projection;
                        if (targetIndex == 0)
                            effect.DiffuseColor = Color.Black.ToVector3();
                        else
                            effect.DiffuseColor = Color.White.ToVector3();

                    }
                    mesh.Draw();


                }


            }

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


            player.formation.Decompose(out scale, out rota, out trans);
            tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if (currentIndex == 0 && mesh.Name == "LeaderStar")
                {

                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);
                        effect.View = ScreenManager.camera.View;
                        effect.Projection = ScreenManager.camera.Projection;
                        effect.DiffuseColor = Color.Yellow.ToVector3();


                    }
                    mesh.Draw();





                }






            }
        }
        public void IntersectTheseus()
        {
            float? dist = 0.0f;
            foreach (boundingSphere bs in ScreenManager.Theseus.spheres)
            {
                dist = bs.BS.Intersects(cursorRay);
                
                if (dist != null)
                {
                    //break;
                }
            }


        }
        public void IntersectSelectionBox(BoundingFrustum frust)
        {
            foreach (boundingSphere bs in ScreenManager.Theseus.spheres)
            {
                if (frust.Contains(bs.BS) != ContainmentType.Disjoint)
                {
                    
                    frustCount++;
                    break;
                }



            }



        }
        public void IntersectBox()
        {
            foreach (BoundingBox[] bbarray in ScreenManager.bbs)
                foreach (BoundingBox bb in bbarray)
                    if (bb.Intersects(cursorRay) != null)
                    {
                        fakeStatueXYZ = bb.Min +new Vector3(50.0f, 0.0f, 50.0f);
                    }   





        }

        public void checkRightClick(Ray aray)
        {
            foreach (boundingSphere bs in ScreenManager.Jailer.spheres)
                if (aray.Intersects(bs.BS) != null)
                {
                    ScreenManager.Theseus.target = 0;
                    ScreenManager.Theseus.aiState = 1;
                }


        }
        public bool isAtk(JuneXnaModel june)
        {
            if (june.isAtk1 || june.isAtk2 || june.isAtk3 || june.isHammerFlight)
                return true;
            else
                return false;


        }
        public void checkEnemyAttack(JuneXnaModel june)
        {
            foreach (Arrow arrow in june.arrows)
            {
                if(arrow.alive)
                foreach(BoundingSphere bs in arrow.BSes)
                {
                    if(arrow.alive)
                    foreach(JuneXnaModel runner in ScreenManager.runners)
                    {
                        foreach(boundingSphere rs in runner.spheres)
                        {
                            if(bs.Contains(rs.BS) != ContainmentType.Disjoint)
                            {

                                runner.health -= 20;

                                arrow.alive = false;
                                break;

                            }


                        }
                        if (!arrow.alive)
                            break;

                    }
         
                    if (!arrow.alive)
                        break;

           if (arrow.alive)
                    {
                        foreach (boundingSphere ts in ScreenManager.Theseus.spheres)
                        {
                            if (bs.Contains(ts.BS) != ContainmentType.Disjoint)
                            {
                                ScreenManager.Theseus.health -= 20;
                                arrow.alive = false;
                                break;

                            }



                        }
                        if (!arrow.alive)
                            break;



                    }


                }


                



            }
            if (june.isAtk1 || june.isAtk2 || june.isAtk3)
            {
                foreach (JuneXnaModel runner in ScreenManager.runners)
                {

                    foreach (boundingSphere bs in runner.spheres)
                    {
                        foreach (boundingSphere js in june.rSpear)
                        {
                            if (bs.BS.Contains(js.BS) != ContainmentType.Disjoint & !june.hits.Contains(runner.fighterIndex))
                            {


                                june.hits.Add(runner.fighterIndex);
                                //ScreenManager.Jailer.isKnockedBack = true;
                                runner.health -= 10;
                                //ScreenManager.Theseus.atkSuccess = true;
                                float Distance = Vector3.Distance(june.rSpear[0].BS.Center, runner.collisionS[0].BS.Center) - 25.6f;
                                float rotationAmount = TurnToFace(june.collisionS[0].BS.Center, runner.collisionS[0].BS.Center, new Vector3(0.0f, (float)Math.Atan((double)(ScreenManager.Theseus.Direction.Z / ScreenManager.Theseus.Direction.X)), 0.0f));
                                Vector3 Direction = new Vector3((float)Math.Cos((double)rotationAmount), 0.0f, (float)Math.Sin((double)rotationAmount));

                                break;
                            }

                        }
                    }
                }
                    foreach (boundingSphere bs in ScreenManager.Theseus.spheres)
                    {
                        foreach (boundingSphere js in june.rSpear)
                        {
                            if (bs.BS.Contains(js.BS) != ContainmentType.Disjoint & !june.hits.Contains(ScreenManager.Theseus.fighterIndex))
                            {
                                june.hits.Add(ScreenManager.Theseus.fighterIndex);
                                //ScreenManager.Jailer.isKnockedBack = true;
                                ScreenManager.Theseus.health -= 10;
                                //ScreenManager.Theseus.atkSuccess = true;
                                float Distance = Vector3.Distance(june.rSpear[0].BS.Center, ScreenManager.Theseus.collisionS[0].BS.Center) - 25.6f;
                                float rotationAmount = TurnToFace(june.collisionS[0].BS.Center, ScreenManager.Theseus.collisionS[0].BS.Center, new Vector3(0.0f, (float)Math.Atan((double)(ScreenManager.Theseus.Direction.Z / ScreenManager.Theseus.Direction.X)), 0.0f));
                                Vector3 Direction = new Vector3((float)Math.Cos((double)rotationAmount), 0.0f, (float)Math.Sin((double)rotationAmount));

                                break;
                            }

                        }

                    }
                



            }






        }
        public void checkTheseusAtk(JuneXnaModel june)
        {


            //if ((ScreenManager.Theseus.isAtk1 && ScreenManager.Theseus.currentAtkTime.TotalMilliseconds < 500) || (!ScreenManager.Theseus.isAtk1 && ScreenManager.Theseus.isAtk2 && ScreenManager.Theseus.currentAtkTime.TotalMilliseconds < 500) || (!ScreenManager.Theseus.isAtk1 && !ScreenManager.Theseus.isAtk2 && ScreenManager.Theseus.isAtk3 && ScreenManager.Theseus.currentAtkTime.TotalMilliseconds < 500))
            if(ScreenManager.Theseus.isAtk1 || ScreenManager.Theseus.isAtk2 || ScreenManager.Theseus.isAtk3)
            foreach (boundingSphere bs in june.spheres)
            {
                foreach (boundingSphere js in ScreenManager.Theseus.rSword)
                {

                    if (js.BS.Contains(bs.BS) != ContainmentType.Disjoint && !ScreenManager.Theseus.hits.Contains(june.fighterIndex))
                    {
                        //if (!ScreenManager.Theseus.atkSuccess)
                      //  if(!ScreenManager.Theseus.hits.Contains(june.fighterIndex))
                        {
                            ScreenManager.Theseus.hits.Add(june.fighterIndex);
                            //ScreenManager.Jailer.isKnockedBack = true;
                            june.health -= 50;
                            //ScreenManager.Theseus.atkSuccess = true;
                            float Distance = Vector3.Distance(ScreenManager.Theseus.rSword[0].BS.Center, june.collisionS[0].BS.Center) - 25.6f;
                            float rotationAmount = TurnToFace(june.collisionS[0].BS.Center,ScreenManager.Theseus.collisionS[0].BS.Center, new Vector3(0.0f, (float)Math.Atan((double)(ScreenManager.Theseus.Direction.Z / ScreenManager.Theseus.Direction.X)), 0.0f));
                            Vector3 Direction = new Vector3((float)Math.Cos((double)rotationAmount), 0.0f, (float)Math.Sin((double)rotationAmount));
                           // june.Position += Direction * Distance;
                            //june.isHurt = true;

                            //june.kbVec = Direction;
                            //june.kbTimer = ScreenManager.Theseus.currentAtkTime;
                            //june.isKnockBack = true;

                            break;
                        }

                    }
                    //if (ScreenManager.Theseus.atkSuccess)
                     //   break;
                }

            }
            //float time1 = 0;
            //float time2 = 0;
            foreach (Arrow arrow in ScreenManager.Theseus.arrows)
            {
            //    for(int i = 0; i<arrow.BSes.Count; i++)
            //    {
            //        if (arrow.alive)
            //        {
            //            for (int j = 0; j < june.spheres.Count; j++)
            //                if (SphereSphereSweep(arrow.BSes[i].Radius,
            //                    arrow.oldBSes[i].Center, arrow.BSes[i].Center,
            //                    june.spheres[j].BS.Radius,
            //                    june.oldSpheres[j].Center,
            //                    june.spheres[j].BS.Center,
            //                    time1,
            //                    time2))


            //                    if (arrow.alive)
            //                    {
            //                        arrow.alive = false;
            //                        june.health -= 100;
            //                        break;
            //                    }
            //            if (!arrow.alive)
            //                break;
            //        }

            //        }
                foreach (BoundingSphere arrowBs in arrow.BSes)
                {

                    if (arrow.alive)
                        foreach (boundingSphere bs in june.spheres)
                            //if(SphereSphereSweep(arrowBs.Radius,


                            if (arrowBs.Contains(bs.BS) != ContainmentType.Disjoint)
                            {
                                if (arrow.alive)
                                {
                                    arrow.alive = false;
                                    june.health -= 100;
                                    break;
                                }


                            }
                    if (!arrow.alive)
                        break;

                }




            }



        }
        bool Quadratic(float a, float b, float c, ref float r1, ref float r2)
        {
            double q = b * b - 4 * a * c;

            if (q >= 0)
            {
                double sq = Math.Sqrt(q);
                double d = 1 / (2 * a);
                r1 = (float)((-b + sq) * d);
                r2 = (float)((-b - sq) * d);
                return true; //real roots

            }
            else
            {
                return false; //complex roots
            }
            
        }

        float dot(Vector3 a, Vector3 b)
        {
            float result = 0;

            return result = a.X * b.X + a.Y * b.Y + a.Z + b.Z;

        }
        bool SphereSphereSweep
        (float ra, Vector3 a1, Vector3 a2, float rb, Vector3 b1,
            Vector3 b2, float u0, float u1)
        {
            Vector3 va = a2 - a1;
            Vector3 vb = b2 - b1;
            Vector3 AB = b1 - a1;
            //relative velocity in normalized time
            Vector3 vab = vb - va;
         

            float rab = ra + rb;
            float a = dot(vab, vab);
            float b = 2 * dot(vab, AB);
            float c = dot(AB, AB) - rab * rab;

            if (dot(AB, AB) <= rab * rab)
            {
                u0 = 0;
                u1 = 0;
                return true;

            }

            if (Quadratic(a, b, c, ref u0, ref u1))
            {
                //if(u0>u1)
                return true;

            }
            return false;

        }

        
        public void checkEAtkTheseus(JuneXnaModel june)
        {

            foreach (boundingSphere bs in ScreenManager.Theseus.spheres)
            {
                foreach (boundingSphere js in june.rSpear)
                {

                    if (js.BS.Contains(bs.BS) != ContainmentType.Disjoint && !june.atkSuccess)
                    {
                        if (!june.atkSuccess)
                        {
                            //ScreenManager.Jailer.isKnockedBack = true;
                            ScreenManager.Theseus.health -= 10;
                            june.atkSuccess = true;
                            //float Distance = Vector3.Distance(june.rSpear[0].BS.Center, ScreenManager.Theseus.collisionS[0].BS.Center) - 25.6f;
                            float rotationAmount = TurnToFace(june.collisionS[0].BS.Center, ScreenManager.Theseus.collisionS[0].BS.Center, new Vector3(0.0f, (float)Math.Atan((double)(june.Direction.Z / june.Direction.X)), 0.0f));
                            Vector3 Direction = new Vector3((float)Math.Cos((double)rotationAmount), 0.0f, (float)Math.Sin((double)rotationAmount));
                            //ScreenManager.Theseus.Position += Direction * Distance;

                            ScreenManager.Theseus.kbVec = Direction;
                            ScreenManager.Theseus.kbTimer = june.currentAtkTime;
                            ScreenManager.Theseus.isKnockBack = true;
                            break;
                        }

                    }

                    if (june.atkSuccess)
                        break;
                }
            }


        }

        public void EnemyAtk(JuneXnaModel june)
        {

            foreach (Tower tower in ScreenManager.Towers)
            {

                foreach (boundingSphere js in june.rSpear)
                {

                    if (js.BS.Contains(tower.BS) != ContainmentType.Disjoint && !june.atkSuccess)
                    {
                        if (!june.atkSuccess)
                        {
                            //ScreenManager.Jailer.isKnockedBack = true;
                            tower.health -= 10;
                            june.atkSuccess = true;
                            break;
                        }

                    }
                    if (june.atkSuccess)
                        break;
                }


            }
            if ((june.isHammerFlight && june.currentAnimationTime.TotalMilliseconds > 1000))
            {
                
                foreach (boundingSphere bs in ScreenManager.Theseus.spheres)
                {
                    foreach (boundingSphere js in june.rSpear)
                    {

                        if (js.BS.Contains(bs.BS) != ContainmentType.Disjoint && !june.atkSuccess)
                        {
                            if (!june.atkSuccess)
                            {
                                //ScreenManager.Jailer.isKnockedBack = true;
                                ScreenManager.Theseus.health -= 10;
                                june.atkSuccess = true;
                                //float Distance = Vector3.Distance(june.rSpear[0].BS.Center, ScreenManager.Theseus.collisionS[0].BS.Center) - 25.6f;
                                float rotationAmount = TurnToFace(june.collisionS[0].BS.Center, ScreenManager.Theseus.collisionS[0].BS.Center, new Vector3(0.0f, (float)Math.Atan((double)(june.Direction.Z / june.Direction.X)), 0.0f));
                                Vector3 Direction = new Vector3((float)Math.Cos((double)rotationAmount), 0.0f, (float)Math.Sin((double)rotationAmount));
                                //ScreenManager.Theseus.Position += Direction * Distance;

                                ScreenManager.Theseus.kbVec = Direction;
                                ScreenManager.Theseus.kbTimer = june.currentAtkTime;
                                ScreenManager.Theseus.isKnockBack = true;
                                break;
                            }

                        }
             
                        if (june.atkSuccess)
                            break;
                    }




                }
                if(!june.atkSuccess)
                    if (june.collisionS[0].BS.Contains(ScreenManager.Theseus.collisionS[0].BS)!= ContainmentType.Disjoint)
                    {

                        float rotationAmount = TurnToFace(june.collisionS[0].BS.Center, ScreenManager.Theseus.collisionS[0].BS.Center, new Vector3(0.0f, (float)Math.Atan((double)(june.Direction.Z / june.Direction.X)), 0.0f));
                        Vector3 Direction = new Vector3((float)Math.Cos((double)rotationAmount), 0.0f, (float)Math.Sin((double)rotationAmount));
                        //ScreenManager.Theseus.Position += Direction * Distance;

                        ScreenManager.Theseus.kbVec = Direction;
                        ScreenManager.Theseus.kbTimer = june.currentAtkTime;
                        ScreenManager.Theseus.isKnockBack = true;

                        
                    }


            }
            if ((june.isAtk1 && june.currentAtkTime.TotalMilliseconds < 500) || (!june.isAtk1 && june.isAtk2 && june.currentAtkTime.TotalMilliseconds < 500) || (!june.isAtk1 && !june.isAtk2 && june.isAtk3 && june.currentAtkTime.TotalMilliseconds < 500))
            foreach (boundingSphere bs in ScreenManager.Theseus.spheres)
            {
                foreach (boundingSphere js in june.rSpear)
                {

                    if (js.BS.Contains(bs.BS) != ContainmentType.Disjoint && !june.atkSuccess)
                    {
                        if (!june.atkSuccess)
                        {
                            //ScreenManager.Jailer.isKnockedBack = true;
                            ScreenManager.Theseus.health -= 10;
                            june.atkSuccess = true;
                            //float Distance = Vector3.Distance(june.rSpear[0].BS.Center, ScreenManager.Theseus.collisionS[0].BS.Center) - 25.6f;
                            float rotationAmount = TurnToFace(june.collisionS[0].BS.Center, ScreenManager.Theseus.collisionS[0].BS.Center, new Vector3(0.0f, (float)Math.Atan((double)(june.Direction.Z / june.Direction.X)), 0.0f));
                            Vector3 Direction = new Vector3((float)Math.Cos((double)rotationAmount), 0.0f, (float)Math.Sin((double)rotationAmount));
                            //ScreenManager.Theseus.Position += Direction * Distance;

                            ScreenManager.Theseus.kbVec = Direction;
                            ScreenManager.Theseus.kbTimer = june.currentAtkTime;
                            ScreenManager.Theseus.isKnockBack = true;
                            break;
                        }

                    }
                    if (june.atkSuccess)
                        break;
                }

            }


        }

        
        public void forceMoves(JuneXnaModel june)
        {

            if (isAtk(june))
            {
                if (june.collisionS[0].BS.Contains(ScreenManager.Theseus.collisionS[0].BS)!= ContainmentType.Disjoint)
                {
                    float Distance = Vector3.Distance(june.collisionS[0].BS.Center, ScreenManager.Theseus.collisionS[0].BS.Center) - 25.6f;
                    float rotationAmount = TurnToFace(june.collisionS[0].BS.Center, ScreenManager.Theseus.collisionS[0].BS.Center, new Vector3(0.0f, (float)Math.Atan((double)(june.Direction.Z / june.Direction.X)), 0.0f));
                    Vector3 Direction = new Vector3((float)Math.Cos((double)rotationAmount), 0.0f, (float)Math.Sin((double)rotationAmount));
                    june.Position -= Direction * Distance;

                }



            }



        }

        
        public void checkMoveEnemySingle(JuneXnaModel june, GameTime gameTime)
        {
            
            float rotationAmount = 0.0f;
            Vector3 sepDirection = Vector3.Zero;
            Vector3 delta = Vector3.Zero;
            june.moveWait = false;
            if (june.moveState == 3 || june.moveState == 0 ||june.moveState == 1 || june.moveState == 2)
            {
                if (june.thrustAmount > 0.0f)
                {
                    for (int j = 0; j < ScreenManager.dummies.Count; j++)
                    {
                        if (june.fighterIndex != j)
                        {
                            if (june.collisionS[0].BS.Contains(ScreenManager.dummies[j].collisionS[0].BS) != ContainmentType.Disjoint)
                            {
                               // delta = ScreenManager.dummies[j].Position - june.Position;
                               // june.Position -= Vector3.Clamp(delta, new Vector3(-1, 0, -1), new Vector3(1, 0, 1)) * (float)gameTime.ElapsedGameTime.TotalSeconds * 100;// new Vector3(Math.Abs(june.Position.X), 0.0f, Math.Abs(june.Position.Z));

                                rotationAmount = TurnToFace(june.Position, ScreenManager.dummies[j].Position, new Vector3(0.0f, (float)Math.Atan((double)(ScreenManager.dummies[j].Direction.Z / ScreenManager.dummies[j].Direction.X)), 0.0f));
                                
                       float x = (float)Math.Sin(rotationAmount);
                       float y = (float)Math.Cos(rotationAmount);
                       sepDirection = new Vector3(x, 0.0f, y);
                                while (june.collisionS[0].BS.Contains(ScreenManager.dummies[j].collisionS[1].BS) != ContainmentType.Disjoint)
                                {
                                   // rotationAmount = TurnToFace(june.Position, ScreenManager.dummies[j].Position, new Vector3(0.0f, (float)Math.Atan((double)(ScreenManager.Theseus.Direction.Z / ScreenManager.Theseus.Direction.X)), 0.0f));
                                    //june.moveState = 1;
                                    june.Position -= sepDirection * 10;
                                    june.collisionS[0].BS = new BoundingSphere(june.Position + new Vector3(0.0f, 70.0f, 0.0f), june.collisionS[0].BS.Radius);
                             
                                    sepDirection = new Vector3(x, 0.0f, y);

                                    //june.Direction = june.oldDirection;
                                   // june.moveWait = true;
                                }
                                june.moveState = 1;
                                if (ScreenManager.dummies[j].thrustAmount > 0.0f)
                                    ScreenManager.dummies[j].moveWait = true;

                            }
                            if (ScreenManager.Theseus.collisionS[0].BS.Contains(june.collisionS[0].BS) != ContainmentType.Disjoint)
                            {
                               // delta = ScreenManager.Theseus.Position - june.Position;
                                //june.Position -= Vector3.Clamp(delta, new Vector3(-1, 0, -1), new Vector3(1, 0, 1)) * (float)gameTime.ElapsedGameTime.TotalSeconds * 100;// new Vector3(Math.Abs(june.Position.X), 0.0f, Math.Abs(june.Position.Z));
                                rotationAmount = TurnToFace(june.Position, ScreenManager.Theseus.Position, new Vector3(0.0f, (float)Math.Atan((double)(ScreenManager.Theseus.Direction.Z / ScreenManager.Theseus.Direction.X)), 0.0f));

                                float x = (float)Math.Sin(rotationAmount);
                                float y = (float)Math.Cos(rotationAmount);
                                sepDirection = new Vector3(x, 0.0f, y);
                                while (june.collisionS[0].BS.Contains(ScreenManager.Theseus.collisionS[0].BS) != ContainmentType.Disjoint)
                                {
                                    // rotationAmount = TurnToFace(june.Position, ScreenManager.dummies[j].Position, new Vector3(0.0f, (float)Math.Atan((double)(ScreenManager.Theseus.Direction.Z / ScreenManager.Theseus.Direction.X)), 0.0f));
                                    //june.moveState = 1;
                                    june.Position -= sepDirection * 10;
                                    june.collisionS[0].BS = new BoundingSphere(june.Position + new Vector3(0.0f, 70.0f, 0.0f), june.collisionS[0].BS.Radius);

                                    sepDirection = new Vector3(x, 0.0f, y);

                                    //june.Direction = june.oldDirection;
                                   // june.moveWait = true;
                                }
                                june.moveState = 1;

                            }
                        }


                    }
                }

            }
            else if (june.moveState == 2)
            {
                if (june.setSpheres.Count > 0)
                {
                    if (june.setSphereCount == 1)
                    {
                        if (june.setSpheres[0].Contains(ScreenManager.Theseus.collisionS[0].BS) != ContainmentType.Disjoint)
                        {
                            june.moveState = 0;
                            june.Position = june.oldPosition;
                            june.Direction = june.oldDirection;
                            june.setSpheres.Clear();
                            june.havePlannedPosition = false;
                            //if (june.setSpheres.Count == 1)
                            //    june.moveState = 0;
                            //else
                            //    june.moveState = 1;
                            //       june.moveState = 0;
                            //    june.setSpheres.Clear();
                            //      june.havePlannedPosition = false;
                            //  delta = ScreenManager.Theseus.Position - june.Position;
                            // june.Position -= Vector3.Clamp(delta, new Vector3(-1, 0, -1), new Vector3(1, 0, 1)) * (float)gameTime.ElapsedGameTime.TotalSeconds * 100;// new Vector3(Math.Abs(june.Position.X), 0.0f, Math.Abs(june.Position.Z));
                        }
                    }
                    else
                    {   if(june.setSpheres.Count > 0)

                        if (june.collisionS[0].BS.Contains(ScreenManager.Theseus.collisionS[0].BS) != ContainmentType.Disjoint)
                        {
                            june.Position = june.oldPosition;
                            june.Direction = june.oldDirection;
                            june.moveWait = true;
                            june.moveState = 1;
                            june.setSpheres.Clear();

                        }
                        //was just giong back to movestate 1 now gonna check smaller radius
                        //if (june.setSpheres[0].Contains(ScreenManager.Theseus.collisionS[0].BS) != ContainmentType.Disjoint)
                        //{
                        //    //go back and get a new path
                        //    june.moveState = 1;
                        //    june.setSpheres.Clear();

                        //}
                    }


                    
                   for (int j = 0; j < ScreenManager.dummies.Count; j++)
                    {
                        if (j != june.fighterIndex)
                        {
                            if (june.setSpheres.Count == 1)
                            {
                                if (june.setSpheres.Count > 0)
                                    if (june.setSpheres[0].Contains(ScreenManager.dummies[j].collisionS[0].BS) != ContainmentType.Disjoint)
                                    {

                                        //    if (list[i].moveState != 2)
                                        {

                                            //          delta = ScreenManager.dummies[j].Position - june.Position;
                                            //         june.Position -= Vector3.Clamp(delta, new Vector3(-1, 0, -1), new Vector3(1, 0, 1)) * (float)gameTime.ElapsedGameTime.TotalSeconds * 100;// new Vector3(Math.Abs(june.Position.X), 0.0f, Math.Abs(june.Position.Z));
                                           // june.Position = june.oldPosition;
                                          //  june.Direction = june.oldDirection;
                                          //  june.moveWait = true;
                                            june.moveState = 0;
                                            june.setSpheres.Clear();
                                            june.havePlannedPosition = false;

                                        }
                                    }
                            }
                            else 
                            {
                                if (june.setSpheres.Count > 0)
                                {
                                    if (june.setSpheres[0].Contains(ScreenManager.Theseus.collisionS[0].BS) != ContainmentType.Disjoint)
                                    {
                                        //go back and get a new path
                                      //  june.Position = june.oldPosition;
                                      //  june.Direction = june.oldDirection;
                                     //   june.moveWait = true;
                                      //  june.moveState = 1;
                                        june.setSpheres.Clear();

                                    }
                                }
                            }


                        }
                    }

                }


            }
        }
        //if two enemies collide then set their move state to 1 which is colliding
        //the last collider is free to pathfind, the collidee has to wait for them to finish
        //check to see if the waiter is moving, if it isnt moving dont have to move wait
        public void checkMoveEnemyList(List<JuneXnaModel> list)
        {

            for (int i = 0; i < list.Count; i++)
            {
              //  list[i].moveState = 0;
                list[i].moveWait = false;
                if (list[i].moveState != 2)
                {
                    if (list[i].thrustAmount > 0.0f)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            if (i != j)
                            {

                                if (list[i].collisionS[0].BS.Contains(list[j].collisionS[0].BS) != ContainmentType.Disjoint)
                                {

                                    //    if (list[i].moveState != 2)
                                    {
                                        list[i].Position = list[i].oldPosition;
                                        list[i].moveState = 1;
                                        if (list[j].thrustAmount > 0.0f)
                                            list[j].moveWait = true;
                                    }
                                }

                            }
                        }
                        if (ScreenManager.Theseus.collisionS[0].BS.Contains(list[i].collisionS[0].BS) != ContainmentType.Disjoint)
                        {
                            // if (list[i].moveState != 2)
                            {
                                list[i].Position = list[i].oldPosition;

                                list[i].moveState = 1;
                            }
                        }
                    }
                }
                else if(list[i].moveState == 2)
                {
                   
                    //if (list[i].thrustAmount > 0.0f)
                    {
                        if(list[i].setSpheres.Count>0)
                        if (list[i].setSpheres[0].Contains(ScreenManager.Theseus.collisionS[0].BS) != ContainmentType.Disjoint)
                        {
                            list[i].moveState = 0;
                            list[i].setSpheres.Clear();
                            list[i].havePlannedPosition = false;
                        }
                        
                        for (int j = 0; j < list.Count; j++)
                        {
                            if (i != j)
                            {
                                if (list[i].setSpheres.Count > 0)
                                if (list[i].setSpheres[0].Contains(list[j].collisionS[0].BS) != ContainmentType.Disjoint)
                                {

                                    //    if (list[i].moveState != 2)
                                    {
                                        list[i].moveState = 0;
                                        list[i].setSpheres.Clear();
                                        list[i].havePlannedPosition = false;
                                             
                                    }
                                }

                            }
                        }


                    }


                }

            }

        }

        public void checkMovePlayerVs(List<JuneXnaModel> list)
        {
            float rotationAmount = 0.0f;
            Vector3 sepDirection = Vector3.Zero;
            
            if(ScreenManager.Theseus.thrustAmount > 0.0f)
                foreach (JuneXnaModel june in list)
                {
                    if (ScreenManager.Theseus.collisionS[1].BS.Contains(june.collisionS[1].BS) != ContainmentType.Disjoint)
                    {
                        //ScreenManager.Theseus.Position = ScreenManager.Theseus.oldPosition;
                        ScreenManager.Theseus.Position = ScreenManager.Theseus.oldPosition;
                        break;
                       //rotationAmount = TurnToFace(june.Position, ScreenManager.Theseus.Position, new Vector3(0.0f, (float)Math.Atan((double)(ScreenManager.Theseus.Direction.Z / ScreenManager.Theseus.Direction.X)), 0.0f));
                       // //june.moveState = 1;

                       //float x = (float)Math.Sin(rotationAmount);
                       //float y = (float)Math.Cos(rotationAmount);
                       //sepDirection = new Vector3(x, 0.0f, y);
                       //while (ScreenManager.Theseus.collisionS[0].BS.Contains(june.collisionS[0].BS) != ContainmentType.Disjoint)
                       //{
                       //    //june.Position -= sepDirection * 10;
                       //    ScreenManager.Theseus.Position += sepDirection * 10;
                       //    ScreenManager.Theseus.collisionS[0].BS = new BoundingSphere(ScreenManager.Theseus.Position + new Vector3(0.0f, 70.0f, 0.0f), ScreenManager.Theseus.collisionS[0].BS.Radius);
                       //    //june.collisionS[0].BS = new BoundingSphere(june.Position + new Vector3(0.0f, 70.0f, 0.0f), june.collisionS[0].BS.Radius);
                       //}

                    }
                }
            //foreach(JuneXnaModel june in list)
            //    if (june.thrustAmount > 0.0f)
            //    {
            //        if (ScreenManager.Theseus.collisionS[0].BS.Contains(june.collisionS[0].BS) != ContainmentType.Disjoint)
            //        {
            //            june.Position = june.oldPosition;
            //        }

            //    }


        }
        public void checkMoves2()
        {

            checkMovePlayerVs(ScreenManager.dummies);
            checkMoveEnemyList(ScreenManager.dummies);
            
        }

        //ultimate function that checks moves for all 
        public void checkMoves()
        {




            if(ScreenManager.Theseus.thrustAmount > 0)
                if (ScreenManager.Theseus.collisionS[0].BS.Contains(ScreenManager.rogue.collisionS[0].BS) != ContainmentType.Disjoint)
                {
                    ScreenManager.Theseus.Position = ScreenManager.Theseus.oldPosition;
                    
                }
            
            if (ScreenManager.rogue.thrustAmount > 0)
            if (ScreenManager.rogue.collisionS[0].BS.Contains(ScreenManager.Theseus.collisionS[0].BS) != ContainmentType.Disjoint)
            {
                ScreenManager.rogue.Position = ScreenManager.rogue.oldPosition;
                
            }
            //checks all available moves to see if the move is valid and if it is decide who gets to move
            for (int i = 0; i < ScreenManager.fighters.Count; i++)
            {
                if (ScreenManager.fighters[i].thrustAmount > 0 && ScreenManager.fighters[i].activated)
                {
                    for (int j = 0; j < ScreenManager.fighters.Count; j++)
                    {
                        if(i != j && ScreenManager.fighters[j].activated)
                        if (ScreenManager.fighters[i].collisionS[0].BS.Contains(ScreenManager.fighters[j].collisionS[0].BS) != ContainmentType.Disjoint)
                        {

                            ScreenManager.fighters[i].Position = ScreenManager.fighters[i].oldPosition;
                            ScreenManager.fighters[j].Position = ScreenManager.fighters[j].oldPosition;

                            ScreenManager.fighters[i].setSpheres.Clear();
                            ScreenManager.fighters[j].setSpheres.Clear();
                        }
                        
                    }


                }


            }
            
        }
        public void checkTarget()
        {
            ScreenManager.targetedTowers.Clear();
            ScreenManager.targetedJune.Clear();
            float closest = float.PositiveInfinity;
            int index = 0; 
            float? current = 0; 
            int i =0;
            bool found = false;
            foreach (Tower tower in ScreenManager.Towers)
            {
                current = cursorRay.Intersects(tower.BS);
                if (current != null)
                    if (current < closest)
                    {
                        index = i;
                        found = true;
                        
                    }
                i++;
            }
            if (found)
            {
                ScreenManager.targetedTowers.Add(index);
                ScreenManager.Theseus.towerSelected = true;

            }
            else
                ScreenManager.Theseus.towerSelected = false;
                     


        }
        public void checkCollisions()
        {
            foreach (boundingSphere bs in ScreenManager.Theseus.rSword)
            {
                foreach (boundingSphere js in ScreenManager.Jailer.spheres)
                {
                    if (bs.BS.Contains(js.BS) != ContainmentType.Disjoint)
                    {
                        //ScreenManager.Jailer.isKnockedBack = true;
                        ScreenManager.Jailer.health -= 1;


                    }




                }



            }
            
            #region oldCollision
            //if (AosRun)
            //{
            //    foreach (boundingSphere bs in ScreenManager.Loki.rSpear)
            //    foreach(boundingSphere pbs in ScreenManager.Michael.spheres)
            //    {

            //        if (bs.BS.Intersects(pbs.BS))

            //    }
            //}

            //if (HerculesVsThorRun)
            //{

            //    foreach(boundingSphere bs in ScreenManager.ThorTVH.knockBackSphere)
            //        foreach(boundingSphere bbs in ScreenManager.HercTVH.spheres)
            //    {


            //        if(bs.BS.Contains(bbs.BS) != ContainmentType.Disjoint)
            //            {
            //                ScreenManager.ThorTVH.attackSuccess = true;
            //                ScreenManager.HercTVH.injuries.Add(10);

            //            }


            //    }
               
            //    foreach (boundingSphere bs in ScreenManager.HercTVH.knockBackSphere)
            //        foreach (boundingSphere bbs in ScreenManager.ThorTVH.spheres)
            //        {


            //            if (bs.BS.Contains(bbs.BS) != ContainmentType.Disjoint)
            //            {
            //                ScreenManager.ThorTVH.injuries.Add(10);
            //                ScreenManager.HercTVH.attackSuccess = true;
            //                if (ScreenManager.HercTVH.isHighKick)
            //                {
            //                    ScreenManager.ThorTVH.isKnockedBack = true;
            //                    ScreenManager.ThorTVH.injuries.Add(60);

            //                }

            //            }



            //        }

            //}
            #endregion










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

        public void DrawTowers()
        {
            int i = 0;
            foreach (Tower tower in ScreenManager.Towers)
            {

                if (tower.alive)
                {
                    if (ScreenManager.targetedTowers.Count > 0 && i == ScreenManager.targetedTowers[0])
                        BoundingSphereRenderer.Render(tower.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Purple);
                    else
                        BoundingSphereRenderer.Render(tower.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Silver);

                }
                else
                    BoundingSphereRenderer.Render(tower.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Azure);
                i++;
                ModelMesh arrow = ScreenManager.tower.Meshes["Arrow"];
                foreach (Projectile2 proj in tower.projectiles)
                {
                    foreach (Effect effect in arrow.Effects)
                    {

                        effect.CurrentTechnique = effect.Techniques["SkinnedEffect"];
                        effect.Parameters["World"].SetValue(proj.world);
                        //effect.EnableDefaultLighting();
                        effect.Parameters["View"].SetValue(ScreenManager.camera.View);
                        effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
                        effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);
                        effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                        effect.Parameters["Texture"].SetValue(ScreenManager.white);


                    }
                    arrow.Draw();
                    BoundingSphereRenderer.Render(proj.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.SlateBlue);
                }
            }



        }

        public void DrawWallPlace()
        {

            BoundingSphereRenderer.Render(ScreenManager.Theseus.buildSphere1, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Black);
            BoundingSphereRenderer.Render(ScreenManager.Theseus.buildSphere2, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);
            BoundingSphereRenderer.Render(ScreenManager.Theseus.wall.Bs, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Gray);
            
        }
        
        public void DrawTowerPlace(Tower tower)
        {
            Matrix world = new Matrix();
            Vector3 scale, trans;
            Quaternion rota;



            Matrix[] transforms = new Matrix[ScreenManager.tower.Bones.Count];
            ScreenManager.tower.CopyAbsoluteBoneTransformsTo(transforms);
            //if (mesh.Name == "forwardSpell" && proj.Name == "Fire")
            foreach (ModelMesh mesh in ScreenManager.tower.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {

                    if (mesh.Name == "Arrow")
                    {
                        //we only need the scale
                        tower.arrowWorld = transforms[mesh.ParentBone.Index];
                                                
                    }
                    

                }
            }
            BoundingSphereRenderer.Render(tower.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Silver);




        }
        public void DrawTower(Tower tower, string namedEffect)
        {
            
            Matrix world = new Matrix();
            Vector3 scale, trans;
            Quaternion rota;



            Matrix[] transforms = new Matrix[ScreenManager.tower.Bones.Count];
            ScreenManager.tower.CopyAbsoluteBoneTransformsTo(transforms);
            //if (mesh.Name == "forwardSpell" && proj.Name == "Fire")
           foreach(ModelMesh mesh in ScreenManager.tower.Meshes)
           {
                foreach (Effect effect in mesh.Effects)
                {
                    tower.World.Decompose(out scale, out rota, out trans);
                    //transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);
                    // effect.Alpha = 1.0f;
                    if (mesh.Name == "Arrow")
                    {
                        tower.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);
                        tower.arrowWorld.Translation = new Vector3(tower.arrowWorld.Translation.X, 70.0f, tower.arrowWorld.Translation.Z);
                        //tower.arrowWorld
                    }
                    effect.CurrentTechnique = effect.Techniques[namedEffect];
                    effect.Parameters["World"].SetValue(tower.World);
                    //effect.EnableDefaultLighting();
                    effect.Parameters["View"].SetValue(ScreenManager.camera.View);
                    effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
                    effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);
                    effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                    effect.Parameters["Texture"].SetValue(ScreenManager.gold1);

                    //effect.TextureEnabled = true;
                    //effect.Texture = ScreenManager.fire;
                    //ScreenManager.fireParticles.AddParticle(tower.world.Translation, Vector3.Up);


                }
               if(mesh.Name == "tower")
                mesh.Draw();
            }

           ModelMesh arrow = ScreenManager.tower.Meshes["Arrow"];
           foreach (Projectile2 proj in tower.projectiles)
           {
               foreach (Effect effect in arrow.Effects)
               {

                   effect.CurrentTechnique = effect.Techniques[namedEffect];
                   effect.Parameters["World"].SetValue(proj.world);
                   //effect.EnableDefaultLighting();
                   effect.Parameters["View"].SetValue(ScreenManager.camera.View);
                   effect.Parameters["Projection"].SetValue(ScreenManager.camera.Projection);
                   effect.Parameters["LightViewProj"].SetValue(ScreenManager.lightViewProjection);
                   effect.Parameters["LightDirection"].SetValue(ScreenManager.lightDir);
                   effect.Parameters["Texture"].SetValue(ScreenManager.white);


               }
               arrow.Draw();
               BoundingSphereRenderer.Render(proj.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.SlateBlue);
           }







        }
        void DrawBoxes()
        {
            int i = 0;
            int j = 0;
            Color color = Color.Black;
            ScreenManager.primitiveBatch.Begin(PrimitiveType.LineList);
            foreach (BoundingBox[] theBoxes in ScreenManager.mainSearch)
            {
                j = 0;
                foreach (BoundingBox box in theBoxes)
                {
                    if (ScreenManager.mainOpen[i][j] == true)
                    {
                        //    ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Max.Z), Color.Red);
                        //    ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Max.Z), Color.Red);

                        //    ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Max.Z), Color.Red);
                        //    ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Max.Z), Color.Red);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Max.Z), color);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Max.Z), color);

                        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Max.Z), Color.Red);
                        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Max.Z), Color.Red);


                        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), Color.Red);
                        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Min.Z), Color.Red);

                        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Min.Z), Color.Red);
                        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Min.Z), Color.Red);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Min.Z), color);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Min.Z), color);

                        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Min.Z), Color.Red);
                        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), Color.Red);



                        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), Color.Red);
                        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Max.Z), Color.Red);

                        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Min.Z), Color.Red);
                        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Max.Z), Color.Red);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Min.Z), color);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Max.Z), color);

                        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Min.Z), color);
                        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Max.Z), color);
                    }
                        j++;
                    
                }
                i++;
            }

            ScreenManager.primitiveBatch.End();

            //Matrix[] transforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            //ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);

            //ModelMesh grid = ScreenManager.FinalProjectiles.Meshes["Grid"];

            //foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            //{
            //    if (mesh.Name == "Grid")
            //        grid = mesh;



            //}
            /*THIS WAS UNCOMMENTED B4 U WORKED ON ETERNAL STRUGGLE FOR THE AOS RUN*/
            //int x = (int)ScreenManager.Michael.World.Translation.X / 60;
            //int y = (int)ScreenManager.Michael.World.Translation.Z / 60;

            //for(int i = 0; i<10;  i++)
            //    for (int j = 0; j < 10; j++)
            //    {
            //        foreach (BasicEffect effect in grid.Effects)
            //        {

            //            effect.World = transforms[grid.ParentBone.Index] * Matrix.CreateTranslation(new Vector3(30.0f, 5.0f, 30.0f) + new Vector3(i * 60.0f, 0.0f, j * 60.0f));
            //            effect.View = ScreenManager.camera.View;
            //            effect.Projection = ScreenManager.camera.Projection;
            //            effect.TextureEnabled = true;
            //            effect.Texture = ScreenManager.whiteG;
            //            effect.DiffuseColor = Color.White.ToVector3();

            //            if (x == i && y == j)
            //                effect.Texture = ScreenManager.yellowG;
            //            else if(((x-5) <= i) && (i <=(x+5)) && ((y-5) <= j) && (j <= (y+5)))
            //                effect.Texture = ScreenManager.redG;

            //            if (targetX == i && targetY == j)
            //                effect.Texture = ScreenManager.blackG;

            //          //  if(i ==  && j == (int)ScreenManager.Michael.World.Translation.Z/100)
            //               // effect.DiffuseColor = 


            //        }
            //        grid.Draw();
            //    }
            /*END AOS RUN THINGAMJIG*/
            ///Old AosBoxes May 26th
            ///
            //ScreenManager.primitiveBatch.Begin(PrimitiveType.LineList);
            //foreach (BoundingBox[] theBoxes in aosBoxes)
            //    foreach (BoundingBox box in theBoxes)
            //    {
            //        //    ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Max.Z), Color.Red);
            //        //    ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Max.Z), Color.Red);

            //        //    ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Max.Z), Color.Red);
            //        //    ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Max.Z), Color.Red);

            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Max.Z), Color.Black);
            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Max.Z), Color.Black);

            //        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Max.Z), Color.Red);
            //        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Max.Z), Color.Red);


            //        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), Color.Red);
            //        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Min.Z), Color.Red);

            //        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Min.Z), Color.Red);
            //        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Min.Z), Color.Red);

            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Min.Z), Color.Black);
            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Min.Z), Color.Black);

            //        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Min.Z), Color.Red);
            //        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), Color.Red);



            //        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), Color.Red);
            //        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Min.Y, box.Max.Z), Color.Red);

            //        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Min.Z), Color.Red);
            //        //ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Min.Y, box.Max.Z), Color.Red);

            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Min.Z), Color.Black);
            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Max.X, box.Max.Y, box.Max.Z), Color.Black);

            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Min.Z), Color.Black);
            //        ScreenManager.primitiveBatch.AddVertex(new Vector3(box.Min.X, box.Max.Y, box.Max.Z), Color.Black);

            //    }


            //ScreenManager.primitiveBatch.End();



        }

        public void debugDraw()
        {

            Vector3 screenVec = ScreenManager.GraphicsDevice.Viewport.Project(ScreenManager.Loki.World.Translation, ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);
            ScreenManager.SpriteBatch.Begin();
            if (ScreenManager.Loki.energy > 4)
                ScreenManager.SpriteBatch.Draw(ScreenManager.black, new Rectangle((int)screenVec.X, (int)screenVec.Y - 80, 10, 10), Color.Blue);
            if (ScreenManager.Loki.energy > 8)
                ScreenManager.SpriteBatch.Draw(ScreenManager.black, new Rectangle((int)screenVec.X + 15, (int)screenVec.Y - 80, 10, 10), Color.Blue);
            ScreenManager.SpriteBatch.End();


        }

        public void drawPylons()
        {

            ModelMesh pylon = ScreenManager.FinalBuild2.Meshes["Pylon"];
            Matrix[] transforms = new Matrix[ScreenManager.FinalBuild2.Bones.Count];
            ScreenManager.FinalBuild2.CopyAbsoluteBoneTransformsTo(transforms);
            foreach(Pylon py in ScreenManager.pylons)
            {
            foreach (BasicEffect effect in pylon.Effects)
            {
                effect.World = transforms[pylon.ParentBone.Index] * Matrix.CreateTranslation(py.Translation);
                effect.View = ScreenManager.camera.View;
                effect.Projection = ScreenManager.camera.Projection;
                effect.EnableDefaultLighting();
                effect.Alpha = .3f;

            }
            pylon.Draw();
            }

        }
        public void drawWalls()
        {

            ModelMesh wall = ScreenManager.FinalBuild2.Meshes["wall"];

            Matrix[] transforms = new Matrix[ScreenManager.FinalBuild2.Bones.Count];
            ScreenManager.FinalBuild2.CopyAbsoluteBoneTransformsTo(transforms);

                foreach (BasicEffect effect in wall.Effects)
                {
                    effect.World = transforms[wall.ParentBone.Index];
                    effect.View = ScreenManager.camera.View;
                    effect.Projection = ScreenManager.camera.Projection;
                    effect.EnableDefaultLighting();
                    effect.Alpha = .3f;

                }
                wall.Draw();
           
        }
        public void DrawTarget()
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(ScreenManager.cursor, ScreenManager.Theseus.TargetCursor, Color.Red);

           // foreach (Vector2 v in mCast2)
         //       ScreenManager.SpriteBatch.Draw(ScreenManager.cursor, v, Color.White);

            //ScreenManager.SpriteBatch.Draw(ScreenManager.white, selectionBox, new Color(1.0f, 1.0f, 1.0f, .3f));
            ScreenManager.SpriteBatch.End();

        }
        public void DrawMouse()
        {
            
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(ScreenManager.cursor, new Vector2(mouse.X, mouse.Y), Color.White);

            foreach(Vector2 v in mCast2)
                ScreenManager.SpriteBatch.Draw(ScreenManager.cursor, v, Color.White);

            //ScreenManager.SpriteBatch.Draw(ScreenManager.white, selectionBox, new Color(1.0f, 1.0f, 1.0f, .3f));
            ScreenManager.SpriteBatch.End();



        }
        public void DrawLightning()
        {
            ScreenManager.primitiveBatch.Begin(PrimitiveType.TriangleList);
            for(int i = 0; i<lightning.segmentList.Count; i++)
            {
                ScreenManager.primitiveBatch.AddVertex(lightning.segmentList[i].startPoint, Color.White);
                ScreenManager.primitiveBatch.AddVertex(lightning.segmentList[i].startPoint + new Vector3(0.0f, 10.0f, 0.0f), Color.White);
                ScreenManager.primitiveBatch.AddVertex(lightning.segmentList[i].endPoint, Color.White);
                ScreenManager.primitiveBatch.AddVertex(lightning.segmentList[i].startPoint + new Vector3(0.0f, 10.0f, 0.0f), Color.White);
                ScreenManager.primitiveBatch.AddVertex(lightning.segmentList[i].endPoint, Color.White);
                ScreenManager.primitiveBatch.AddVertex(lightning.segmentList[i].endPoint + new Vector3(0.0f, 10.0f, 0.0f), Color.White);


            }
                ScreenManager.primitiveBatch.End();
        }
        public void DrawFire(Vector3 midPoint)
        {
            // ModelMesh chest = ScreenManager.spearSphere.Meshes["chestS"];

            //Vector3 midPoint = new Vector3(20.0f, 10.0f, 20.0f);
            //List<List<BoundingSphere>> spheres = new List<List<BoundingSphere>>();
            //for (int i = 0; i < 5; i++)
            //{
            //    spheres.Add(new List<BoundingSphere>());
            //    for (int j = 0; j < i * 2 + 1; j++)
            //    {
            //        spheres[i].Add(new BoundingSphere(new Vector3(midpoint.X + (float)Math.Cos(Math.PI * 2)  * 20, 0.0f, midpoint.Z + (float)Math.Sin(Math.PI* 2 )  * 20), 10.0f));
            //        BoundingSphereRenderer.Render(spheres[i][j], ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Azure);
            //    }
            //}
            for (double i = 0.0; i < 1.0; i += .2)
            {
                const float radius = 30;
                const float height = 40;

                double angle = i * Math.PI * 2;

                float x = (float)Math.Cos(angle);
                float y = (float)Math.Sin(angle);

                BoundingSphereRenderer.Render(new BoundingSphere(new Vector3(midPoint.X + x * radius, 10, midPoint.Z + y * radius), 10), ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);
                ScreenManager.fireParticles.AddParticle(new Vector3(midPoint.X + x * radius, 10, midPoint.Z + y * radius), Vector3.Down);


            }
            for (double i = 0.0; i < 1.0; i += .1)
            {
                const float radius = 60;
                const float height = 40;

                double angle = i * Math.PI * 2;

                float x = (float)Math.Cos(angle);
                float y = (float)Math.Sin(angle);

                BoundingSphereRenderer.Render(new BoundingSphere(new Vector3(midPoint.X + x * radius, 10, midPoint.Z + y * radius), 10), ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);
                ScreenManager.fireParticles.AddParticle(new Vector3(midPoint.X + x * radius, 10, midPoint.Z + y * radius), Vector3.Down);

            }
            BoundingSphereRenderer.Render(new BoundingSphere(midPoint, 10), ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);
            ScreenManager.fireParticles.AddParticle(midPoint, Vector3.Down);


        }
        public void makeEffect()
        {
            effect = new BasicEffect(ScreenManager.GraphicsDevice);

        }
        public void makeCylinder(List<VertexPositionColor> vertices, List<ushort> indices)
        {

           

            vertexBuffer.SetData(vertices.ToArray());
            indexBuffer.SetData(indices.ToArray());


        }
        public void makeCylinder(Vector3 Direction, Vector3 midPoint)
        {
            //effect = new BasicEffect(ScreenManager.GraphicsDevice);
            vertices.Clear();
            indices.Clear();
            Vector3 tempPoint = midPoint;
            for (int i = 0; i < 10; i += 1)
            {
                tempPoint = midPoint;
                const float radius = 30;
                const float height = 40;

                double angle = i / 10.0 * Math.PI * 2;

                float x = (float)Math.Cos(angle);
                float y = (float)Math.Sin(angle);

                vertices.Add(new VertexPositionColor(new Vector3(midPoint.X + x * radius, midPoint.Y + y * radius, midPoint.Z), Color.Blue));
              //  BoundingSphereRenderer.Render(new BoundingSphere(new Vector3(midPoint.X + x * radius, midPoint.Y + y * radius, midPoint.Z), 10), ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);
                 tempPoint += Direction * 100.0f;
                 vertices.Add(new VertexPositionColor(new Vector3(tempPoint.X + x * radius, tempPoint.Y + y * radius, tempPoint.Z), Color.Blue));

                indices.Add((ushort)(i * 2));
                indices.Add((ushort)(i * 2 + 1));
                indices.Add((ushort)((i * 2 + 2) % (10 * 2)));

                indices.Add((ushort)(i * 2 + 1));
                indices.Add((ushort)((i * 2 + 3) % (10 * 2)));
                indices.Add((ushort)((i * 2 + 2) % (10 * 2)));


             //   BoundingSphereRenderer.Render(new BoundingSphere(new Vector3(midPoint.X + x * radius, midPoint.Y + y * radius, midPoint.Z), 10), ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);
            }

            vertexBuffer.SetData(vertices.ToArray());
            indexBuffer.SetData(indices.ToArray());


        }
        public void DrawCylinder()
        {
            effect.World = Matrix.Identity;
            effect.View = ScreenManager.camera.View;
            effect.Projection = ScreenManager.camera.Projection;
            effect.VertexColorEnabled = true;
            ScreenManager.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            ScreenManager.GraphicsDevice.Indices = indexBuffer;
            foreach (EffectPass effectPass in effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();

                int primitiveCount = 60 / 3;

                ScreenManager.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                                                     20, 0, primitiveCount);

            }

          }
        public void DrawFire()
        {
           // ModelMesh chest = ScreenManager.spearSphere.Meshes["chestS"];

            Vector3 midPoint= new Vector3( 20.0f, 10.0f, 20.0f);
            //List<List<BoundingSphere>> spheres = new List<List<BoundingSphere>>();
            //for (int i = 0; i < 5; i++)
            //{
            //    spheres.Add(new List<BoundingSphere>());
            //    for (int j = 0; j < i * 2 + 1; j++)
            //    {
            //        spheres[i].Add(new BoundingSphere(new Vector3(midpoint.X + (float)Math.Cos(Math.PI * 2)  * 20, 0.0f, midpoint.Z + (float)Math.Sin(Math.PI* 2 )  * 20), 10.0f));
            //        BoundingSphereRenderer.Render(spheres[i][j], ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Azure);
            //    }
            //}
            for(double i = 0.0; i< 1.0; i+=.2)
            {
                           const float radius = 30;
            const float height = 40;
                
            double angle = i * Math.PI * 2;

            float x = (float)Math.Cos(angle);
            float y = (float)Math.Sin(angle);

            BoundingSphereRenderer.Render(new BoundingSphere(new Vector3(midPoint.X + x * radius, 10, midPoint.Z + y * radius), 10), ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);


            }
            for (double i = 0.0; i < 1.0; i += .1)
            {
                const float radius = 60;
                const float height = 40;

                double angle = i * Math.PI * 2;

                float x = (float)Math.Cos(angle);
                float y = (float)Math.Sin(angle);

                BoundingSphereRenderer.Render(new BoundingSphere(new Vector3(midPoint.X + x * radius, 10, midPoint.Z + y* radius), 10), ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);


            }
            BoundingSphereRenderer.Render(new BoundingSphere(midPoint, 10), ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);



        }
        public void drawResources()
        {

            foreach (Resource res in ScreenManager.resources)
            {
                //earth fire wind watre
                //if(res.Type == 0) 
                //BoundingSphereRenderer.Render(res.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Brown);
                //if (res.Type == 1)
                //    BoundingSphereRenderer.Render(res.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Firebrick);
                //if (res.Type == 2)
                //    BoundingSphereRenderer.Render(res.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Beige);
                //if (res.Type == 3)
                //    BoundingSphereRenderer.Render(res.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Aqua);

            }



        }
        public void DrawItems()
        {
            foreach (Item item in ScreenManager.items)
                BoundingSphereRenderer.Render(item.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);

        }
        public override void Draw(GameTime gameTime)
        {

            ScreenManager.lightViewProjection = ScreenManager.CreateLightViewProjectionMatrix();
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
            ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
           // ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
          // ScreenManager.GraphicsDevice.Adapter.


            if (ScreenManager.stage == 1)
            {


                ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
                CreateShadowMap();
                ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                ScreenManager.GraphicsDevice.BlendState = BlendState.NonPremultiplied;
                //DrawArena(gameTime);
                
               // ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                //DrawTowers();
                //DrawGates();
                DrawTheseus(ScreenManager.Theseus, "SkinnedEffect");
                DrawGrid(gameTime);
                foreach (JuneXnaModel dummy in ScreenManager.dummies)
                {
                   // if(dummy.isFighter)
                    DrawFighter(dummy, "SkinnedEffect");

                }
                foreach (JuneXnaModel archer in ScreenManager.archers)
                {
                    DrawFighter(archer, "SkinnedEffect");
                }
                foreach (JuneXnaModel monster in ScreenManager.deadMonsters)
                    DrawFighter(monster, "SkinnedEffect");
                foreach (JuneXnaModel runner in ScreenManager.runners)
                    DrawRunner(runner, "SkinnedEffect");
                for(int i = 0; i<ScreenManager.deadRunners.Count; i++)
                {
                    if(i == ScreenManager.Theseus.allyTargetIndex)
                        DrawTargetedRunner(ScreenManager.deadRunners[i], "SkinnedEffect");
                    else
                       DrawRunner(ScreenManager.deadRunners[i], "SkinnedEffect");



                }
    
                //foreach (JuneXnaModel runner in ScreenManager.runners)
                //    DrawRunner(runner, "SkinnedEffect");
               // DrawFWiz(ScreenManager.Wizard, "SkinnedEffect");
                //DrawTower(ScreenManager.RinnaAl, "SkinnedEffect");
               // DrawJSpear(ScreenManager.fighter, "SkinnedEffect");
                //DrawJSpear(ScreenManager.rogue, "SkinnedEffect");
               // DrawHercules(ScreenManager.god);
                
                //foreach (JuneXnaModel june in ScreenManager.fighters)
                //    if (june.activated)
                //        DrawFighter(june, "SkinnedEffect");

                //foreach (JuneXnaModel june in ScreenManager.ghosts)
                    
                //        DrawGhost(june, "SkinnedEffect");
                
             //   DrawSMove(ScreenManager.SmartMovement1, "SkinnedEffect");
                foreach (HSphere hs in hspheres)
                    RenderBoundingSphere(hs.BS, Color.Brown);
                //RenderBoundingSphere(hsphere1.BS, Color.CadetBlue);
                //RenderBoundingSphere(hsphere2.BS, Color.ForestGreen);
                RenderDRay(dray1, Color.Blue);
                DrawFire();
                drawResources();
               // DrawCylinder(Vector3.Forward);
               // DrawLightning();
                //DrawJSpear(ScreenManager.Jailer, "SkinnedEffect");
                //DrawMino(ScreenManager.MinoTS);
          
               // DrawJSpear(ScreenManager.SmartMovement1, "SkinnedEffect");
              //  DrawJSpear(ScreenManager.SmartMovement2, "SkinnedEffect");
               // DrawJSpear(ScreenManager.SmartMovement3, "SkinnedEffect");

               // DrawTheseusFake(ScreenManager.fakeStatue);
               // drawWalls();
              //  drawPylons();
                DrawTarget();
                DrawBoxes();
                DrawMouse();
                DrawHud();
                DrawItems();

               // RayRenderer.Render(cursorRay, 1000.0f, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Blue);

            }


            #region oldruns
            /*
            if (AosRun)
            {
                ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
                CreateShadowMap();

                ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                DrawAosBoard(gameTime);
                ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                DrawBoxes();
                DrawMichael(ScreenManager.Michael);
                DrawDargahe(ScreenManager.Dargahe);
                DrawThor(ScreenManager.CaptThor);
                DrawHercules(ScreenManager.LtHercules);
                DrawLoki(ScreenManager.Loki);
                DrawWisp(ScreenManager.Wisp);
                debugDraw();
            }
            if (RageAgainstTheMachine)
            {
                ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
                CreateShadowMap();
                ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                DrawHercvsThorBoard(gameTime);
                ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                DrawAchilles(ScreenManager.AchillesRage1);
                DrawUndead(ScreenManager.UndeadLeader);
            }
            if (RageOfTheMachine)
            {
                ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
                CreateShadowMap();
                ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                DrawHercvsThorBoard(gameTime);
                ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                DrawAchilles(ScreenManager.AchillesRage1);
                DrawUndead(ScreenManager.UndeadLeader);
            }
            if (drawEternalStruggle)
            {
                ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
                CreateShadowMap();
                ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                DrawBoard(gameTime);
                ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                DrawHercules(ScreenManager.Hercules);
                DrawPerseus(ScreenManager.Perseus);
                DrawTheseus(ScreenManager.Theseus);
            }
            if (HerculesVsThorRun)
            {
                ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
                CreateShadowMap();
                ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                DrawHercvsThorBoard(gameTime);
                ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                DrawHercules(ScreenManager.HercTVH);
                DrawThor(ScreenManager.ThorTVH);
              //  ScreenManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
              //  DrawHerculesProjectiles(ScreenManager.HercTVH);
                DrawRage();
            }
            if (TheseusStandRun)
            {
                ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
                CreateShadowMap();
                ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                DrawTheseusBoard(gameTime);
                ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                DrawTheseus(ScreenManager.TheseusTS);
                DrawParis(ScreenManager.ParisTS);
                DrawMino(ScreenManager.MinoTS);
                if (TheseusStandRun2)
                    DrawGorgon(ScreenManager.GorgonTS);
                DrawRage();
            }
            DrawMenu(gameTime);
            
            if (RageOfTheMachine || RageAgainstTheMachine)
                DrawRage();
            */
            #endregion
            base.Draw(gameTime);
        }
   
        public void RenderDRay(DRay dray, Color col)
        {
            RayRenderer.Render(dray.ray, dray.distance, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, col);

        }
        public void RenderBoundingSphere(BoundingSphere bs, Color col)
        {
            BoundingSphereRenderer.Render(bs, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, col);


        }
        public void DrawGorgon(Gorgon gorg)
        {
            Matrix[] transforms = new Matrix[ScreenManager.gorgon.Bones.Count];
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
                    effect.Parameters["Bones"].SetValue(ScreenManager.GorgonTS.SkinTrans);
                    effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);






                }

                mesh.Draw();
            }






        }
        public void DrawMino(Minotaur mino)
        {

           Matrix[] transforms = new Matrix[ScreenManager.minotaur.Bones.Count];
            ScreenManager.minotaur.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.minotaur.Meshes)
            {
                if(mesh.Name == "minotaur" || mesh.Name == "minotaurEye")
                {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.CurrentTechnique = effect.Techniques["SkinnedEffect"];
                    
                    effect.Parameters["DiffuseColor"].SetValue(Vector4.One);
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
                    
                    effect.Parameters["Bones"].SetValue(ScreenManager.MinoTS.SkinTrans);
                    effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);
                    if (mesh.Name == "minotaur")
                        effect.Parameters["Texture"].SetValue(ScreenManager.minoTex);
                    else
                        effect.Parameters["Texture"].SetValue(ScreenManager.white);


                   


                }
                
                    mesh.Draw();
            }
            }














        }
        public void DrawParis(JuneXnaModel player)
        {
            Vector3 scale, trans;
            Quaternion rota;
            Matrix[] transforms;
            bool draw = false;

            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                draw = false;
                if (mesh.Name == "Alecto" || mesh.Name == "sHair" || mesh.Name == "TorsoPlate"
                || mesh.Name == "shin" || mesh.Name == "Bow"  || mesh.Name == "Scalp" ||mesh.Name == "Arrow")
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.CurrentTechnique = effect.Techniques["SkinnedEffect"];

                        effect.Parameters["DiffuseColor"].SetValue(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
                        effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));


                        effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
                        effect.Parameters["DirLight0Direction"].SetValue(new Vector3(-0.5265408f, -0.5735765f, -0.6275069f));
                        effect.Parameters["DirLight0DiffuseColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
                        effect.Parameters["DirLight0SpecularColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
                        effect.Parameters["World"].SetValue(Matrix.Identity);
                        effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera.View).Translation);
                        if (mesh.Name == "Alecto")
                        {
                            effect.Parameters["Texture"].SetValue(ScreenManager.elvenTex);
                            draw = true;
                        }
                        else if (mesh.Name == "Scalp" || mesh.Name == "sHair")
                            effect.Parameters["Texture"].SetValue(ScreenManager.black);
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.gold1);
                      

                        
                        effect.Parameters["Bones"].SetValue(player.SkinTrans);
                        effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);

                        //if (mesh.Name == "sHair")
                        //    effect.Parameters["Texture"].SetValue(ScreenManager.white);
                        //if (mesh.Name == "longHairF")
                        ////    effect.Parameters["Texture"].SetValue(ScreenManager.white);
                        //if (mesh.Name == "longHairB")

                        //    effect.Parameters["Texture"].SetValue(ScreenManager.humanTex);

                        

                    }

                    mesh.Draw();
                }
            }

           


            transforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);
            
            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if (mesh.Name == "Arrow2")
                {
                    ScreenManager.ParisTS.World.Decompose(out scale, out rota, out trans);

                    //ScreenManager.ThorTVH.arrowWorld =Matrix.CreateScale(scale) *  Matrix.CreateTranslation(transforms[mesh.ParentBone.Index].Translation) * ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];// *ScreenManager.ThorTVH.World;
                    // ScreenManager.ThorTVH.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.ThorTVH.World.Translation), rota);
                    ScreenManager.ParisTS.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);// *ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];
                    //ScreenManager.ParisTS.arrowWorld = transforms[mesh.ParentBone.Index] * ScreenManager.ParisTS.SkinTrans[ScreenManager.rHand];

                }

            }


            foreach (Projectile2 proj in ScreenManager.ParisTS.projectiles)
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
        public void DrawThor(JuneXnaModel player)
        {
            Matrix[] transforms;
            bool draw = false;

            transforms = new Matrix[ScreenManager.juneModel.Bones.Count];
            ScreenManager.juneModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.juneModel.Meshes)
            {
                draw = false;
                if (mesh.Name == "Alecto" || mesh.Name == "ThorsHelmet" || mesh.Name == "Helmet" || mesh.Name == "TorsoPlate"
                || mesh.Name == "shin" || mesh.Name == "Mjolnir" || mesh.Name == "ponytail" || mesh.Name == "Scalp")
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.CurrentTechnique = effect.Techniques["SkinnedEffect"];

                        effect.Parameters["DiffuseColor"].SetValue(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
                        effect.Parameters["EmissiveColor"].SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));


                        effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Matrix.Identity)));
                        effect.Parameters["DirLight0Direction"].SetValue(new Vector3(-0.5265408f, -0.5735765f, -0.6275069f));
                        effect.Parameters["DirLight0DiffuseColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
                        effect.Parameters["DirLight0SpecularColor"].SetValue(new Vector3(1, 0.9607844f, 0.8078432f));
                        effect.Parameters["World"].SetValue(Matrix.Identity);
                        effect.Parameters["EyePosition"].SetValue(Matrix.Invert(ScreenManager.camera.View).Translation);
                        if (mesh.Name == "Alecto")
                        {
                            effect.Parameters["Texture"].SetValue(ScreenManager.humanTex);
                            draw = true;
                        }
                        else
                            effect.Parameters["Texture"].SetValue(ScreenManager.gold1);
                        effect.Parameters["Bones"].SetValue(player.SkinTrans);
                        effect.Parameters["WorldViewProj"].SetValue(ScreenManager.camera.View * ScreenManager.camera.Projection);

                        //if (mesh.Name == "sHair")
                        //    effect.Parameters["Texture"].SetValue(ScreenManager.white);
                        //if (mesh.Name == "longHairF")
                        ////    effect.Parameters["Texture"].SetValue(ScreenManager.white);
                        //if (mesh.Name == "longHairB")

                        //    effect.Parameters["Texture"].SetValue(ScreenManager.humanTex);



                    }

                    mesh.Draw();
                }
            }

            Vector3 scale, trans;
            Quaternion rota;

                       transforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if (mesh.Name == "Mjolnir2")
                {
                    ScreenManager.ThorTVH.World.Decompose(out scale, out rota, out trans);

                    //ScreenManager.ThorTVH.arrowWorld =Matrix.CreateScale(scale) *  Matrix.CreateTranslation(transforms[mesh.ParentBone.Index].Translation) * ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];// *ScreenManager.ThorTVH.World;
                   // ScreenManager.ThorTVH.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(ScreenManager.ThorTVH.World.Translation), rota);
                    ScreenManager.ThorTVH.arrowWorld = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);// *ScreenManager.ThorTVH.SkinTrans[ScreenManager.rHand];
                }

            }

            foreach (Projectile2 proj in ScreenManager.ThorTVH.projectiles)
            {
                foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
                {
                    if (mesh.Name == "Mjolnir2")
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                         //transforms[mesh.ParentBone.Index].Decompose(out scale, out rota, out trans);
                         effect.World = proj.world ;
                            effect.EnableDefaultLighting();
                            effect.View = ScreenManager.camera.View;
                            effect.Projection = ScreenManager.camera.Projection;

                        }
                        mesh.Draw();
                    }



                }
            }
            Matrix world = new Matrix();

            player.formation.Decompose(out scale, out rota, out trans);
            Matrix[] tranforms = new Matrix[ScreenManager.FinalProjectiles.Bones.Count];
            ScreenManager.FinalProjectiles.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.FinalProjectiles.Meshes)
            {
                if (currentIndex == 0 && mesh.Name == "LeaderStar")
                {

                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = Matrix.Transform(transforms[mesh.ParentBone.Index], rota) * Matrix.CreateTranslation(trans);
                        effect.View = ScreenManager.camera.View;
                        effect.Projection = ScreenManager.camera.Projection;
                        effect.DiffuseColor = Color.Yellow.ToVector3();


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
                    if (mesh.Name == "rSwordS1" || mesh.Name == "rSwordS2" || mesh.Name == "rSwordS3")// || mesh.Name == "rSpearS2" || mesh.Name == "rSpearS3")
                    {

                        targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.rHand];
                        targetMat.Decompose(out scale, out rota, out trans);
                        player.rSword[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    }
                    //if (mesh.Name == "lPunchS")// || mesh.Name == "rSpearS2" || mesh.Name == "rSpearS3")
                    //{

                    //    targetMat = transforms[mesh.ParentBone.Index] * player.SkinTrans[ScreenManager.lHand];
                    //    targetMat.Decompose(out scale, out rota, out trans);
                    //    player.physicalSphere[j++] = new boundingSphere(mesh.Name, new BoundingSphere(trans, mesh.BoundingSphere.Radius * scale.X));

                    //}
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

            //foreach (boundingSphere bs in player.spheres)
            //{
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.White);

            //}
            //foreach (boundingSphere bs in player.rSword)
            //{
            //    BoundingSphereRenderer.Render(bs.BS, ScreenManager.GraphicsDevice, ScreenManager.camera.View, ScreenManager.camera.Projection, Color.Black);
            //}



        }
        public void DrawTheseusBoard(GameTime gameTime)
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
                if (mesh.Name == "openGate" )
                {
                    foreach (Effect effect in mesh.Effects)
                    {

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

                    mesh.Draw();
                }
            }



        }
        public void DrawHercvsThorBoard(GameTime gameTime)
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
                if (mesh.Name == "HercVsThorTerrain" || mesh.Name == "HercVsThorTemple")
                {
                    foreach (Effect effect in mesh.Effects)
                    {

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

                    mesh.Draw();
                }
            }



        }
        public void DrawMenu(GameTime gameTime)
        {

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;
            Vector2 position = new Vector2(100, 150);

            spriteBatch.Begin();

            // Draw each menu entry in turn.
            if(activeMenu)
            for (int i = 0; i < heroActiveMenu.MenuEntries.Count; i++)
            {
                MenuEntry menuEntry = heroActiveMenu.MenuEntries[i];

                bool isSelected = IsActive && (i == heroActiveMenu.SelectedEntry);

                menuEntry.Draw(this, position, isSelected, gameTime);

                position.Y += menuEntry.GetHeight(this);
            }

            if (aosMenu)
            {
                for (int i = 0; i < heroActiveMenu.MenuEntries.Count; i++)
                {
                    MenuEntry menuEntry = heroActiveMenu.MenuEntries[i];

                    bool isSelected = IsActive && (i == heroActiveMenu.SelectedEntry);

                    menuEntry.Draw(this, position, isSelected, gameTime);

                    position.Y += menuEntry.GetHeight(this);
                }

            }
            if (PreAosDialog)
            {


                for (int i = 0; i < AosIntro.menuEntries.Count; i++)
                {
                    MenuEntry menuEntry = AosIntro.menuEntries[i];

                    bool isSelected = IsActive && (i == AosIntro.selectedEntry);

                    menuEntry.Draw(this, position, isSelected, gameTime);

                    position.Y += menuEntry.GetHeight(this);
                }


                if (AosIntro.scriptIndex < AosIntro.script.Count)
                {
                    Vector2 titlePosition = new Vector2(426, 80);
                    Vector2 titleOrigin = font.MeasureString(AosIntro.script[AosIntro.scriptIndex]) / 2;
                    Color titleColor = new Color(192, 192, 192, TransitionAlpha);
                    float titleScale = 1.25f;



                    spriteBatch.DrawString(font, AosIntro.script[AosIntro.scriptIndex], titlePosition, titleColor, 0,
                                               titleOrigin, titleScale, SpriteEffects.None, 0);

                }







            }
            if (PerseusResurrection)
            {

                for (int i = 0; i < PerseusBetrayal2.menuEntries.Count; i++)
                {
                    MenuEntry menuEntry = PerseusBetrayal2.menuEntries[i];

                    bool isSelected = IsActive && (i == PerseusBetrayal2.selectedEntry);

                    menuEntry.Draw(this, position, isSelected, gameTime);

                    position.Y += menuEntry.GetHeight(this);
                }

                if (PerseusBetrayal2.scriptIndex < PerseusBetrayal2.script.Count)
                {
                    Vector2 titlePosition = new Vector2(426, 80);
                    Vector2 titleOrigin = font.MeasureString(PerseusBetrayal2.script[PerseusBetrayal2.scriptIndex]) / 2;
                    Color titleColor = new Color(192, 192, 192, TransitionAlpha);
                    float titleScale = 1.25f;



                    spriteBatch.DrawString(font, PerseusBetrayal2.script[PerseusBetrayal2.scriptIndex], titlePosition, titleColor, 0,
                                               titleOrigin, titleScale, SpriteEffects.None, 0);
                }




            }
            if (AchillesAlive)
            {




                for (int i = 0; i < Pluto.menuEntries.Count; i++)
                {
                    MenuEntry menuEntry = Pluto.menuEntries[i];

                    bool isSelected = IsActive && (i == Pluto.selectedEntry);

                    menuEntry.Draw(this, position, isSelected, gameTime);

                    position.Y += menuEntry.GetHeight(this);
                }


                if (Pluto.scriptIndex < Pluto.script.Count)
                {
                    Vector2 titlePosition = new Vector2(426, 80);
                    Vector2 titleOrigin = font.MeasureString(Pluto.script[Pluto.scriptIndex]) / 2;
                    Color titleColor = new Color(192, 192, 192, TransitionAlpha);
                    float titleScale = 1.25f;



                    spriteBatch.DrawString(font, Pluto.script[Pluto.scriptIndex], titlePosition, titleColor, 0,
                                               titleOrigin, titleScale, SpriteEffects.None, 0);

                }







            }
            if (PerseusBetrayal)
            {

                for (int i = 0; i < PerseusBetrayal1.menuEntries.Count; i++)
                {
                    MenuEntry menuEntry = PerseusBetrayal1.menuEntries[i];

                    bool isSelected = IsActive && (i == PerseusBetrayal1.selectedEntry);

                    menuEntry.Draw(this, position, isSelected, gameTime);

                    position.Y += menuEntry.GetHeight(this);
                }


                if (PerseusBetrayal1.scriptIndex < PerseusBetrayal1.script.Count)
                {
                    Vector2 titlePosition = new Vector2(426, 80);
                    Vector2 titleOrigin = font.MeasureString(PerseusBetrayal1.script[PerseusBetrayal1.scriptIndex]) / 2;
                    Color titleColor = new Color(192, 192, 192, TransitionAlpha);
                    float titleScale = 1.25f;



                    spriteBatch.DrawString(font, PerseusBetrayal1.script[PerseusBetrayal1.scriptIndex], titlePosition, titleColor, 0,
                                               titleOrigin, titleScale, SpriteEffects.None, 0);

                }




            }
            if (isHercOfferedQuest)
            {
                for (int i = 0; i < HercQuestOne.MenuEntries.Count; i++)
                {
                    MenuEntry menuEntry = HercQuestOne.MenuEntries[i];

                    bool isSelected = IsActive && (i == HercQuestOne.SelectedEntry);

                    menuEntry.Draw(this, position, isSelected, gameTime);

                    position.Y += menuEntry.GetHeight(this);
                }
                Vector2 titlePosition = new Vector2(426, 80);
                Vector2 titleOrigin = font.MeasureString(HercQuestOne.MenuTitle) / 2;
                Color titleColor = new Color(192, 192, 192, TransitionAlpha);
                float titleScale = 1.25f;



                spriteBatch.DrawString(font,HercQuestOne.MenuTitle, titlePosition, titleColor, 0,
                                           titleOrigin, titleScale, SpriteEffects.None, 0);
            }
            if (isTheseusOfferedQuest)
            {
                for (int i = 0; i < TheseusQuestOne.MenuEntries.Count; i++)
                {
                    MenuEntry menuEntry = TheseusQuestOne.MenuEntries[i];

                    bool isSelected = IsActive && (i == TheseusQuestOne.SelectedEntry);

                    menuEntry.Draw(this, position, isSelected, gameTime);

                    position.Y += menuEntry.GetHeight(this);
                }
                Vector2 titlePosition = new Vector2(426, 80);
                Vector2 titleOrigin = font.MeasureString(TheseusQuestOne.MenuTitle) / 2;
                Color titleColor = new Color(192, 192, 192, TransitionAlpha);
                float titleScale = 1.25f;



                spriteBatch.DrawString(font, TheseusQuestOne.MenuTitle, titlePosition, titleColor, 0,
                                           titleOrigin, titleScale, SpriteEffects.None, 0);
            }
            if (activeMenu)
            {
                Vector2 titlePosition = new Vector2(426, 80);
                Vector2 titleOrigin = font.MeasureString(heroActiveMenu.MenuTitle) / 2;
                Color titleColor = new Color(192, 192, 192, TransitionAlpha);
                float titleScale = 1.25f;

                spriteBatch.DrawString(font, heroActiveMenu.MenuTitle, titlePosition, titleColor, 0,
                                       titleOrigin, titleScale, SpriteEffects.None, 0);
            }
            spriteBatch.End();

        }
        public void DrawRage()
        {
            Viewport viewport= ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);


            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(ScreenManager.A, new Rectangle( 5, 5, 20, 20), Color.White);
            ScreenManager.SpriteBatch.Draw(ScreenManager.B, new Rectangle(5, 30, 20, 20), Color.White);
            ScreenManager.SpriteBatch.Draw(ScreenManager.X, new Rectangle(5, 55, 20, 20), Color.White);
            ScreenManager.SpriteBatch.Draw(ScreenManager.Y, new Rectangle(5, 80, 20, 20), Color.White);

            ScreenManager.SpriteBatch.Draw(ScreenManager.LS, new Rectangle(viewport.Width - 25, 5, 20, 20), Color.White);
            ScreenManager.SpriteBatch.Draw(ScreenManager.LT, new Rectangle(viewport.Width - 25, 30, 20, 20), Color.White);
            ScreenManager.SpriteBatch.Draw(ScreenManager.RS, new Rectangle(viewport.Width - 25, 55, 20, 20), Color.White);
            ScreenManager.SpriteBatch.Draw(ScreenManager.RT, new Rectangle(viewport.Width - 25, 80, 20, 20), Color.White);

            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "LS", new Vector2(viewport.Width - 25, 5), Color.RosyBrown);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "LT", new Vector2(viewport.Width - 25, 30), Color.RosyBrown);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "RS", new Vector2(viewport.Width - 25, 55), Color.RosyBrown);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "RT", new Vector2(viewport.Width - 25, 80), Color.RosyBrown);



            foreach (RageHit ragehit in As)
            {
                ScreenManager.SpriteBatch.Draw(ScreenManager.A, new Rectangle( (int)((viewport.Width - 30) * (ragehit.time.TotalMilliseconds/ragehit.maxTime.TotalMilliseconds) ), 5, 20, 20), Color.White);


            }
            foreach(RageHit ragehit in LSs)
                ScreenManager.SpriteBatch.Draw(ScreenManager.LS, new Rectangle((int)((viewport.Width - 30)  - (viewport.Width - 30)* (ragehit.time.TotalMilliseconds / ragehit.maxTime.TotalMilliseconds)), 5, 20, 20), Color.White);

            if (rageBlocks < 2)
                ScreenManager.SpriteBatch.DrawString(ScreenManager.GameFont, "Rage Level 1", new Vector2(viewport.Width / 2, viewport.Height - 50), Color.White);
            else if(rageBlocks < 4)
            {
                ScreenManager.SpriteBatch.DrawString(ScreenManager.GameFont, "Rage Level 2", new Vector2(viewport.Width / 2, viewport.Height - 50), Color.White);

            }
            else if (rageBlocks < 6)
            {
                ScreenManager.SpriteBatch.DrawString(ScreenManager.GameFont, "Rage Level 3", new Vector2(viewport.Width / 2, viewport.Height - 50), Color.White);

            }

            ScreenManager.SpriteBatch.DrawString(ScreenManager.GameFont, rageBlocks.ToString(), new Vector2(viewport.Width / 2, viewport.Height - 80), Color.White);

            ScreenManager.SpriteBatch.End();






        }
        public void checkHealing(GameTime gameTime)
        {

            foreach (Tower tower in ScreenManager.Towers)
            {
                if (ScreenManager.Theseus.TargetSphere.Contains(tower.BS) != ContainmentType.Disjoint)
                {
                    tower.currentDouble += gameTime.ElapsedGameTime.Milliseconds / 30.0;

                }



            }


        }
        public void DrawHud()
        {

            int width = ScreenManager.GraphicsDevice.Viewport.Width - 20;
            int height = ScreenManager.GraphicsDevice.Viewport.Height - 20;

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
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
            Vector3 projectedVec = Vector3.Zero;
            //ScreenManager.GraphicsDevice.Viewport.Project(ScreenManager.Michael.World.Translation, ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);
            for (int i = 0; i < ScreenManager.dummies.Count; i++)
            {
                projectedVec = ScreenManager.GraphicsDevice.Viewport.Project(ScreenManager.dummies[i].Position, ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);

            }

            string resources = ScreenManager.Theseus.antennas + " Antennas " +
                ScreenManager.Theseus.cpus + " cpus " +
                ScreenManager.Theseus.powerCharges + " power charges " +
                ScreenManager.Theseus.spareParts + " spare parts " + ScreenManager.Theseus.Position;
            
            
            

            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, resources, new Vector2(100.0f, 100.0f), Color.White);

            for (int i = 0; i < ScreenManager.dummies.Count; i++)
            {
                projectedVec = ScreenManager.GraphicsDevice.Viewport.Project(ScreenManager.dummies[i].Position, ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);
                ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, ScreenManager.dummies[i].type.ToString(), new Vector2(projectedVec.X, projectedVec.Y), Color.SandyBrown);
            }

            for (int i = 0; i < ScreenManager.runners.Count; i++)
            {
                projectedVec = ScreenManager.GraphicsDevice.Viewport.Project(ScreenManager.runners[i].Position, ScreenManager.camera.Projection, ScreenManager.camera.View, Matrix.Identity);
                ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, ScreenManager.runners[i].health.ToString(), new Vector2(projectedVec.X, projectedVec.Y), Color.SandyBrown);
            }
            ScreenManager.SpriteBatch.End(); 
            //ScreenManager.SpriteBatch.Draw(ScreenManager.orbGui, new Rectangle(20, 20, 20, 20), Color.Red);
            //ScreenManager.SpriteBatch.Draw(ScreenManager.white, new Rectangle(20, 45, (int)(ScreenManager.Theseus.mana1/ScreenManager.Theseus.mana1Max * 50), 10), Color.Red);
            ////ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "1", new Vector2(25, 20), Color.White);
            //ScreenManager.SpriteBatch.Draw(ScreenManager.orbGui, new Rectangle(20, 60, 20, 20), Color.White);
            //ScreenManager.SpriteBatch.Draw(ScreenManager.white, new Rectangle(20, 85, 50, 10), Color.White);
            //ScreenManager.SpriteBatch.Draw(ScreenManager.orbGui, new Rectangle(20, 100, 20, 20), Color.Blue);
            //ScreenManager.SpriteBatch.Draw(ScreenManager.white, new Rectangle(20, 125, 50, 10), Color.Blue);
            //ScreenManager.SpriteBatch.Draw(ScreenManager.orbGui, new Rectangle(20, 140, 20, 20), Color.Brown);
            //ScreenManager.SpriteBatch.Draw(ScreenManager.white, new Rectangle(20, 165, 50, 10), Color.Brown);

            //ScreenManager.SpriteBatch.Draw(ScreenManager.white, new Rectangle(20, 180, (int)(ScreenManager.Theseus.charge1 / 100 * 50), 10), Color.Red);

            //foreach (Resource res in ScreenManager.resources)
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.MenuFont, res.Current.ToString(), new Vector2(res.unproject.X, res.unproject.Y), Color.Yellow);

            //foreach (Tower tower in ScreenManager.Towers)
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.MenuFont, tower.health.ToString(), new Vector2(tower.unproject.X, tower.unproject.Y), Color.Yellow);

            //ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Earth " + ScreenManager.Theseus.earthA, new Vector2(100, 30), Color.Brown);
            //ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Fire " + ScreenManager.Theseus.fireA, new Vector2(180, 30), Color.Firebrick);
            //ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Wind " + ScreenManager.Theseus.windA, new Vector2(260, 30), Color.Beige);
            //ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Water " + ScreenManager.Theseus.waterA, new Vector2(320, 30), Color.Aqua);

            //Vector2 stringMeasure = ScreenManager.MenuFont.MeasureString(ScreenManager.rune1);
            //for (int i = 0; i < ScreenManager.runes.Count; i++)
            //{
            //    if(i != ScreenManager.Theseus.slot)
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.MenuFont, ScreenManager.runes[i], new Vector2(30.0f + 100 * i, height - stringMeasure.Y), Color.Blue);
            //    else
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.MenuFont, ScreenManager.runes[i], new Vector2(30.0f + 100 * i, height - stringMeasure.Y), Color.Yellow);
            //}
            //Vector2 mid = new Vector2(width / 2, height / 2);
            //if (ScreenManager.Theseus.modTower)
            //{
            //    int i = 0;
            //    if(ScreenManager.targetedTowers.Count >0)
            //    foreach (Rune rune in ScreenManager.Towers[ScreenManager.targetedTowers[0]].runes)
            //    {
            //        if(i == ScreenManager.Theseus.lSelection)
            //            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, rune.name + " " + rune.count, mid + new Vector2(0.0f, 50.0f + i * 30), Color.Green);
            //        else
            //            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, rune.name + " " + rune.count, mid + new Vector2(0.0f, 50.0f + i * 30), Color.Blue);
            //        i++;
            //    }
            //    i = 0;
            //    if(ScreenManager.Theseus.runes.Count>0)
            //        foreach (Rune rune in ScreenManager.Theseus.runes)
            //        {
            //            if(i == ScreenManager.Theseus.rSelection)
            //            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, rune.name + " " +rune.count, mid + new Vector2(200.0f, 50.0f + i * 30), Color.Green);
            //            else
            //                ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, rune.name + " " + rune.count, mid + new Vector2(200.0f, 50.0f + i * 30), Color.Blue);
            //            i++;
            //        }
            //}
            //if (ScreenManager.Theseus.spellList)
            //{
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Offense", mid + new Vector2(0.0f, 50.0f), Color.Green);
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Defense", mid + new Vector2(-50.0f, 0.0f), Color.Green);
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Support", mid + new Vector2(0.0f, -50.0f), Color.Green);
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Construct", mid + new Vector2(50.0f, 0.0f), Color.Green);
            //}

            //if (ScreenManager.Theseus.physicalTower)
            //{
                
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Place", mid + new Vector2(0.0f, 50.0f), Color.Green);
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Cancel", mid + new Vector2(0.0f, -50.0f), Color.Green);

            //}
            //else if (ScreenManager.Theseus.healCon)
            //{
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Heal", mid + new Vector2(0.0f, 50.0f), Color.Green);
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Cancel", mid + new Vector2(0.0f, -50.0f), Color.Green);



            //}
            //else if (ScreenManager.Theseus.buildTower)
            //{
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Physical", mid + new Vector2(0.0f, 50.0f), Color.Green);
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Elemental", mid + new Vector2(-50.0f, 0.0f), Color.Green);
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Wall", mid + new Vector2(0.0f, -50.0f), Color.Green);
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Upgrade", mid + new Vector2(50.0f, 0.0f), Color.Green);


            //}
            //else if (ScreenManager.Theseus.construction)
            //{
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Build Tower", mid + new Vector2(0.0f, 50.0f), Color.Green);
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Heal Tower", mid + new Vector2(-50.0f, 0.0f), Color.Green);
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Upgrade Tower", mid + new Vector2(0.0f, -50.0f), Color.Green);
            //    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Bank Tower", mid + new Vector2(50.0f, 0.0f), Color.Green);
            //}

            

            //ScreenManager.SpriteBatch.DrawString(ScreenManager.MenuFont, ScreenManager.rune1, new Vector2(30.0f, height - stringMeasure.Y), Color.Blue);
            //ScreenManager.SpriteBatch.DrawString(ScreenManager.MenuFont, ScreenManager.rune2, new Vector2(130.0f, height - stringMeasure.Y), Color.Blue);
            //ScreenManager.SpriteBatch.DrawString(ScreenManager.MenuFont, ScreenManager.rune3, new Vector2(230.0f, height - stringMeasure.Y), Color.Blue);
            //ScreenManager.SpriteBatch.DrawString(ScreenManager.MenuFont, ScreenManager.rune4, new Vector2(330.0f, height - stringMeasure.Y), Color.Blue);
            //ScreenManager.SpriteBatch.DrawString(ScreenManager.MenuFont, ScreenManager.rune5, new Vector2(430.0f, height - stringMeasure.Y), Color.Blue);
            //ScreenManager.SpriteBatch.DrawString(ScreenManager.MenuFont, ScreenManager.rune6, new Vector2(530.0f, height - stringMeasure.Y), Color.Blue);
            //ScreenManager.SpriteBatch.DrawString(ScreenManager.MenuFont, ScreenManager.rune7, new Vector2(630.0f, height - stringMeasure.Y), Color.Blue);
            //ScreenManager.SpriteBatch.DrawString(ScreenManager.MenuFont, ScreenManager.rune8, new Vector2(730.0f, height - stringMeasure.Y), Color.Blue);
            //ScreenManager.SpriteBatch.DrawString(ScreenManager.MenuFont, 
           // ScreenManager.SpriteBatch.Draw(ScreenManager.whiteSq,new Vector2(projectedVec.X, projectedVec.Y -120), Color.Blue);
          //  ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, timer.ToString(), new Vector2(width / 2.0f, height), Color.Black);
            
            //draw current bubble


            //ScreenManager.SpriteBatch.End();
        }
        void CreateShadowMap()
        {
            // Set our render target to our floating point render target
            ScreenManager.GraphicsDevice.SetRenderTarget(ScreenManager.shadowRenderTarget);

            // Clear the render target to white or all 1's
            // We set the clear to white since that represents the 
            // furthest the object could be away
            ScreenManager.GraphicsDevice.Clear(Color.White);

            DrawTheseus(ScreenManager.Theseus, "CreateShadowMap");
            //DrawJSpear(ScreenManager.Jailer, "CreateShadowMap");
           // DrawMino(ScreenManager.p1Mino);

            //DrawHercules(ScreenManager.Hercules);


            if (AosRun)
            {
                DrawMichael(ScreenManager.Michael);
                //DrawDargahe(ScreenManager.Dargahe);
                //DrawThor(ScreenManager.CaptThor);
                //DrawHercules(ScreenManager.LtHercules);
                //DrawLoki(ScreenManager.Loki);
                //DrawWisp(ScreenManager.Wisp);




            }
            // Set render target back to the back buffer
            ScreenManager.GraphicsDevice.SetRenderTarget(null);
        }


    }
}
