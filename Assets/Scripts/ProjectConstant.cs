


//-------------------------------------------
//  author: 
//  description:  
//-------------------------------------------

using DS.Role;

namespace DS
{
    public static class ProjectConstant
    {

        public class Tag
        {
            public static readonly string WEAPON = "Weapon";
        }

        public class Layer
        {
        
            public static readonly string GROUND = "Ground";
            public static readonly string ENEMY = "Enemy";
        }

        public class AnimatorParameter
        {
            public static readonly string FORWARD = "forward";
            public static readonly string JUMP = "jump";
            public static readonly string ON_GROUND = "onGround";
            public static readonly string AIR = "air";
            public static readonly string ROLL = "roll";
            public static readonly string ATTACK = "attack";
            public static readonly string DIE = "die";
            public static readonly string RIGHT = "right";
            public static readonly string HIT = "hit";
            public static readonly string DEFENSE = "defense";
            public static readonly string BLOACKED = "blocked";
        }

        public class PlayerState
        {
            public static readonly string DEFENSE = "DefenseState";
            public static readonly string DEATH = "DeathState";
        }

        public class AnimatorLayer
        {
            public static readonly string BASE = "Base";
           
        }



    }
}

