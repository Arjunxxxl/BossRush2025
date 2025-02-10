using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [System.Serializable]
    private class AmmoData
    {
        public WeaponType weaponType;
        public int carryingAmmo;
    }
    
    [Header("Carrying Weapon")]
    [SerializeField] private List<Weapon> carryingWeapons;
    [SerializeField] private Weapon equippedWeapon;

    [Header("Ammo Data")]
    [SerializeField] private List<AmmoData> allWeaponAmmoData;

    private void Start()
    {
        SetUpEquippedWeapon();
    }

    #region Eqipped Weapon

    private void SetUpEquippedWeapon()
    {
        equippedWeapon.Init(this);
        UpdateCarryingAmmoUi();
    }

    #endregion

    #region Ammo

    internal void UseAmmo(WeaponType weaponType, int amtUsed)
    {
        for (int i = 0; i < allWeaponAmmoData.Count; i++)
        {
            AmmoData ammoData = allWeaponAmmoData[i];
            if (ammoData.weaponType == weaponType)
            {
                ammoData.carryingAmmo -= amtUsed;
            }
        }

        UpdateCarryingAmmoUi();
    }
    
    internal void AddAmmo(WeaponType weaponType, int amtUsed)
    {
        for (int i = 0; i < allWeaponAmmoData.Count; i++)
        {
            AmmoData ammoData = allWeaponAmmoData[i];
            if (ammoData.weaponType == weaponType)
            {
                ammoData.carryingAmmo += amtUsed;
            }
        }

        UpdateCarryingAmmoUi();
    }

    internal int CalcCarryingAmmo(WeaponType weaponType)
    {
        int carryingAmmo = 0;
        
        for (int i = 0; i < allWeaponAmmoData.Count; i++)
        {
            AmmoData ammoData = allWeaponAmmoData[i];
            if (ammoData.weaponType == weaponType)
            {
                carryingAmmo = ammoData.carryingAmmo;
            }
        }

        return carryingAmmo;
    }

    #endregion

    #region Ui

    #region Ammo

    private void UpdateCarryingAmmoUi()
    {
        WeaponType equippedWeaponType = equippedWeapon.GetWeaponData().weaponType;
        int carryingAmmo = CalcCarryingAmmo(equippedWeaponType);

        GameplayMenu.Instance.WeaponUi.UpdateCarryingAmmoTxt(carryingAmmo);
    }

    #endregion

    #endregion
}
