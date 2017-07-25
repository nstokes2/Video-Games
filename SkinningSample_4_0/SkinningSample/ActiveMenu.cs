using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmellOfRevenge2011
{
    class ActiveMenu : MenuScreen
    {

        public ActiveMenu()
            : base("Active Menu")
        {
            MenuEntry changeFormation = new MenuEntry("Change Formation");
            MenuEntry reinforce = new MenuEntry("ReinForce");
            MenuEntry assignTarget = new MenuEntry("Target");
            MenuEntry subordinate = new MenuEntry("Set AIs");


            assignTarget.Selected += TargetSelected;
            MenuEntries.Add(changeFormation);
            MenuEntries.Add(reinforce);
            MenuEntries.Add(assignTarget);
            MenuEntries.Add(subordinate);
        }

        void ChangeFormationSelected(object sender, PlayerIndexEventArgs e)
        {
            //Not implemented

        }
        void ReinforceSelected(object sender, PlayerIndexEventArgs e)
        {


        }
        void TargetSelected(object sender, PlayerIndexEventArgs e)
        {


        }
        void SubordinateSelected(object sender, PlayerIndexEventArgs e)
        {


        }









    }
}
