using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Data")]
    [SerializeField] private WeaponData weaponData;

    [Header("States")]
    private WeaponStates weaponState = WeaponStates.Unknown;
    private float stateTimeElapsed;
    private float stateDuration;

    [Header("Shooting Data")]
    private int ammoLeftInClip;
    private float gapBtwBullets;

    [Header("Shooting Ray Cast")]
    [SerializeField] private Transform rayStartT;
    [SerializeField] private Vector3 rayStartOffset;
    [SerializeField] private LayerMask rayInteractingLayer;
    private float rayDist = 1000f;

    [Header("Transforms")]
    [SerializeField] private Transform bulletSpawnT;

    [Header("Particle Efx")]
    [SerializeField] private ParticleSystem muzzleFlash;

    [Header("Weapon Manager")]
    private WeaponManager weaponManager;

    [Header("Animator")]
    private WeaponAnimator weaponAnimator;
    
    internal void Update()
    {
        GetInput();
        CheckForReload();
        
        UpdateStateTimer();
    }

    #region Innit

    internal void Init(WeaponManager weaponManager)
    {
        weaponAnimator = GetComponent<WeaponAnimator>();
        
        this.weaponManager = weaponManager;
        
        ammoLeftInClip = 0;
        gapBtwBullets = 1.0f / weaponData.bulletPerSec;
        
        SetWeaponState(WeaponStates.Idle);

        UpdateClipAmmoUi();
    }

    #endregion

    #region Input

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            if (CanShootWeapon())
            {
                SetWeaponState(WeaponStates.Shoot);
            }
        }
    }

    #endregion
    
    #region States

    internal WeaponStates GetWeaponState()
    {
        return weaponState;
    }
    
    private void SetWeaponState(WeaponStates weaponState)
    {
        if (this.weaponState == weaponState)
        {
            return;
        }
        
        this.weaponState = weaponState;

        SetStateData();
        stateTimeElapsed = 0.0f;

        weaponAnimator.PlayWeaponAnimation(this.weaponState);
    }

    private void SetStateData()
    {
        switch (weaponState)
        {
            case WeaponStates.Idle:
                break;
            
            case WeaponStates.Shoot:
                Shoot();
                stateDuration = gapBtwBullets;
                break;
            
            case WeaponStates.Reloading:
                stateDuration = weaponData.reloadDuration;
                break;
            
            case WeaponStates.Reloaded:
                ReloadWeapon();
                break;
        }
    }

    private void UpdateStateTimer()
    {
        if (weaponState == WeaponStates.Reloaded)
        {
            SetWeaponState(WeaponStates.Idle);
        }
        
        if (weaponState == WeaponStates.Shoot)
        {
            stateTimeElapsed += Time.unscaledDeltaTime;

            if (stateTimeElapsed >= stateDuration)
            {
                SetWeaponState(WeaponStates.Idle);
            }
        }
        else if (weaponState == WeaponStates.Reloading)
        {
            stateTimeElapsed += Time.unscaledDeltaTime;

            if (stateTimeElapsed >= stateDuration)
            {
                SetWeaponState(WeaponStates.Reloaded);
            }
        }
    }

    #endregion
    
    #region Shooting

    private bool CanShootWeapon()
    {
        bool isReloading = IsWeaponReloading();
        bool isShooting = weaponState == WeaponStates.Shoot;
        bool haveAmmo = ammoLeftInClip > 0;
        
        bool canShoot = !isReloading && !isShooting && haveAmmo;

        return canShoot;
    }
    
    private void Shoot()
    {
        ammoLeftInClip--;

        var shootingRayHit = GetShootingRayHit();
        bool isRayHitSomething = shootingRayHit.Item1 && Constants.Weapon.UseTargetedBullets;
        Vector3 rayHitPt = shootingRayHit.Item2;
        
        BulletManager.Instance.SpawnBullet(weaponData.bulletPrefabTag,
                                           bulletSpawnT.transform.position,
                                           Quaternion.identity,
                                           transform.forward,
                                           isRayHitSomething,
                                           rayHitPt);
        
        muzzleFlash.Play();

        UpdateClipAmmoUi();
    }

    private (bool, Vector3) GetShootingRayHit()
    {
        Physics.Raycast(rayStartT.position + rayStartOffset, rayStartT.forward, out RaycastHit hit, rayDist, rayInteractingLayer);

        if (hit.collider != null)
        {
            return (true, hit.point);
        }
        
        return (false, Vector3.zero);
    }
    
    #endregion

    #region Reload

    private void CheckForReload()
    {
        if (Constants.Weapon.CanAutoReload)
        {
            if (ammoLeftInClip <= 0)
            {
                TriggerReloadWeapon();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (ammoLeftInClip < weaponData.clipSize)
            {
                TriggerReloadWeapon();
            }
        }
    }

    private void TriggerReloadWeapon()
    {
        SetWeaponState(WeaponStates.Reloading);
    }

    private void ReloadWeapon()
    {
        int carryingAmmo = weaponManager.CalcCarryingAmmo(weaponData.weaponType);
        int ammoReq = weaponData.clipSize - ammoLeftInClip;
        int ammoTaken = ammoReq > carryingAmmo ? carryingAmmo : ammoReq;

        ammoLeftInClip += ammoTaken;

        weaponManager.UseAmmo(weaponData.weaponType, ammoTaken);

        UpdateClipAmmoUi();
    }

    private bool IsWeaponReloading()
    {
        bool isReloading = weaponState == WeaponStates.Reloading || weaponState == WeaponStates.Reloaded;
        return isReloading;
    }
    
    #endregion

    #region Ui

    #region Ammo

    private void UpdateClipAmmoUi()
    {
        GameplayMenu.Instance.WeaponUi.UpdateClipAmmoTxt(ammoLeftInClip);
    }

    #endregion

    #endregion
    
    #region Getters

    internal WeaponData GetWeaponData()
    {
        return weaponData;
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(rayStartT.position + rayStartOffset, rayStartT.forward * rayDist);
    }

    #endregion
}
