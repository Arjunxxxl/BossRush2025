using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class WeaponData
{
    public WeaponType weaponType;

    [Header("Prefab")]
    public string bulletPrefabTag;

    [Header("Shooting Data")]
    public float bulletPerSec;

    [Header("Ammo Data")]
    public int clipSize;
    
    [Header("Reload Data")]
    public float reloadDuration;
}
