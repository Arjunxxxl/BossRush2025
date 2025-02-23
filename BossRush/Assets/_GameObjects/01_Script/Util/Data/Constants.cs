using UnityEngine;

public static class Constants
{
    public static class Player
    {
        // Jump
        public static readonly float PlayerJumpBufferTime = 0.15f;
        public static readonly int PlayerMaxJumpCt = 2;
        
        // Gravity
        public static readonly float MaxGravity = -25f;
        public static readonly float MinGravity = -5f;
        public static readonly float GroundedGravity = 0.0f;
        
        // Hp
        public static readonly int MaxHp = 100;
    }
    
    public static class Weapon
    {
        // Reload
        public static readonly bool CanAutoReload = true;
        
        // Bullet
        public static readonly bool UseTargetedBullets = false;
    }

    public static class TimeScale
    {
        public static readonly float DefaultTimeScale = 1.0f;
        public static readonly float DefaultFixedDeltaTime = 0.02f;
    }
}
