using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVVM
{
    public class PlayerModel
    {
        // ÇÃ·¹ÀÌ¾î ½ºÅÝ ÇÁ·ÎÆÛÆ¼
        // Èû, ¹ÎÃ¸, Áö´É, ¿î
        public int Strength
        {
            get;
            set;
        }

        //
        public int Dexterity
        {
            get;
            set;
        }

        public int Intelligence
        {
            get;
            set;
        }

        public int Luck
        {
            get;
            set;
        }
    }
}