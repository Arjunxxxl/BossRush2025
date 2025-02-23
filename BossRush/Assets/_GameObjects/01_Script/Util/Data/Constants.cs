using UnityEngine;

public static class Constants
{
    public static class Player
    {
        //Jump
        public static readonly float PlayerJumpBufferTime = 0.15f;
        public static readonly int PlayerMaxJumpCt = 2;
    }
    
    public static class Weapon
    {
        // Reload
        public static readonly bool CanAutoReload = true;
        
        //Bullet
        public static readonly bool UseTargetedBullets = false;
    }

    public static class TimeScale
    {
        public static readonly float DefaultTimeScale = 1.0f;
        public static readonly float DefaultFixedDeltaTime = 0.02f;
    }
}
