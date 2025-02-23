using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement playerMovement { get; private set; }
    public PlayerCollisionDetection playerCollisionDetection { get; private set; }
    public PlayerEfxManager playerEfxManager { get; private set; }
    public WeaponManager weaponManager { get; private set; }

    private void OnEnable()
    {
        AbilityManager.ApplyAbilityEffect += ApplyAbilityEffect;
        AbilityManager.RemoveAbilityEffect += RemoveAbilityEffect;
    }

    private void OnDisable()
    {
        AbilityManager.ApplyAbilityEffect -= ApplyAbilityEffect;
        AbilityManager.RemoveAbilityEffect -= RemoveAbilityEffect;
    }

    private void Start()
    {
        SetUp();
    }

    #region SetUp

    internal void SetUp()
    {
        CacheReferences();
        SetUpPlayScripts();
    }

    private void CacheReferences()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCollisionDetection = GetComponent<PlayerCollisionDetection>();
        playerEfxManager = GetComponent<PlayerEfxManager>();
        weaponManager = GetComponent<WeaponManager>();
    }
    
    private void SetUpPlayScripts()
    {
        playerMovement.SetUp(this);
        playerCollisionDetection.SetUp(this);
        playerEfxManager.SetUp(this);
        weaponManager.SetUp(this);
    }
    
    #endregion

    #region Ability

    private void ApplyAbilityEffect(AbilityData.Data abilityData)
    {
        if (abilityData.abilityType == AbilityType.Dash)
        {
            playerMovement.StartDash(abilityData.abilityEffectiveness);
        }
    }
    
    private void RemoveAbilityEffect(AbilityData.Data abilityData)
    {
        if (abilityData.abilityType == AbilityType.Dash)
        {
            playerMovement.StopDash();
        }
    }

    #endregion
}
