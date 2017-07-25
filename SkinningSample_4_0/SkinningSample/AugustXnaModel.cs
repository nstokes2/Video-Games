using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;


namespace SkinningSample
{
    public class AugustXnaModel
    {

        TimeSpan currentAnimationTime;
        TimeSpan runTime;
        //Matrix[] upperBones;
        //Matrix[] previousAnimation;
        #region formationSpheres
        public Vector3[] CFormation = new Vector3[9];
        public Vector3[] NFormation = new Vector3[9];
        public Vector3[] SFormation = new Vector3[9];
        public Vector3[] EFormation = new Vector3[9];
        public Vector3[] WFormation = new Vector3[9];
        #endregion
        #region animationArrays
        public Matrix[] standing, brace, atk1a, atk1b, atk1c, atk1d, atk2a, atk2b, atk3a, atk3b, atk3c, lRun1, lRun2, lRun3, lRun4, rRun1, rRun2, rRun3, rRun4, previousAnimation;
        public Matrix[] shield1, shield2, shieldBash1, shieldBash2, shieldUpper1, shieldUpper2, shieldUpper3, shieldUpper4, shieldToss1, shieldToss2,
            shieldToss3, shieldSpin1, shieldSpin2, shieldSpin3, shieldSpin4, spear1, spear2, spear3, spearSpin1, spearSpin2, spearSpin3, spearSpin4, spearSpin5, spearSpin6,
            flySpear1, flySpear2, flySpear3, spearPin1, spearPin2, spearPin3, spearThrow1, spearThrow2, spearThrow3, spearThrow4, bow1, aim, release, releaseFollowThru, kb1, kb2, kb3, knockDown,
            hammerStrike1, hammerStrike2, hammerStrike3, hammerFlight1, hammerFlight2, hammerToss1, hammerToss2, hammerToss3, hammerToss4,
            rockStomp1, rockStomp2, punch1, punch2, punch3, punchUpper1, punchUpper2, punchUpper3, punchGround1, punchGround2,
            highKick1, highKick2, AchiSpin1, AchiSpin2, AchiSpin3, AchiSpin4, AchiSpin5, AchiSpinSlash1, AchiSpinSlash2,
            AchiSpinSlash3, AchiSpinSlash4, AchiSpinSlash5, AchiSpinSlash6, AchiSpinSlash7, AchiSwordJump1, AchiSwordJump2,
            AchiSwordJump3, AchiSwordRise1, AchiSwordRise2, AchiSwordRise3, AchiSwordPound1, AchiSwordPound2, AchiSwordPound3,
            wing1, wing2, angelAtka1, angelAtka2, angelAtka3, angelAtkb1, angelAtkb2, angelAtkb3, angelSwoosh1,
            angelSwoosh2, angelSwoosh3, angelSwoosh4, angelSwoosh5;


        public bool isAtk1, isStanding, isAtk2, isAtk3;
        public bool isRapidStrikes, isDoubleStrike;
        public bool isRun;

        public bool isGetUp; //goes to brace
        #endregion
    }
}
