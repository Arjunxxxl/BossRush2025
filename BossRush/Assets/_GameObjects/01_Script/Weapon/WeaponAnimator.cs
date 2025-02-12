using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator weaponAnimator;

    private static readonly int Shooting = Animator.StringToHash("Shooting");
    private static readonly int Reload = Animator.StringToHash("Reload");

    internal void PlayWeaponAnimation(WeaponStates weaponState)
    {
        bool triggerShoot = weaponState == WeaponStates.Shoot;
        bool triggerReload = weaponState == WeaponStates.Reloading;

        AnimShoot(triggerShoot);
        
        if (triggerReload)
        {
            AnimReload();
        }
    }
    
    private void AnimShoot(bool play)
    {
        weaponAnimator.SetBool(Shooting, play);
    }

    private void AnimReload()
    {
        weaponAnimator.SetTrigger(Reload);
    }
}
