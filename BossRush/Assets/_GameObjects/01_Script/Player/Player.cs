using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement playerMovement { get; private set; }
    public PlayerCollisionDetection playerCollisionDetection { get; private set; }
    public PlayerEfxManager playerEfxManager { get; private set; }
    public PlayerHp playerHp { get; private set; }
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
        playerHp = GetComponent<PlayerHp>();
        weaponManager = GetComponent<WeaponManager>();
    }
    
    private void SetUpPlayScripts()
    {
        playerMovement.SetUp(this);
        playerCollisionDetection.SetUp(this);
        playerEfxManager.SetUp(this);
        playerHp.SetUp(this);
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
        else if (abilityData.abilityType == AbilityType.ReducedGravity)
        {
            playerMovement.UpdateMaxGravity(abilityData.abilityEffectiveness);
        }
        else if (abilityData.abilityType == AbilityType.InvincibilityShield)
        {
            playerHp.SetShieldActivated(true);
        }
    }
    
    private void RemoveAbilityEffect(AbilityData.Data abilityData)
    {
        if (abilityData.abilityType == AbilityType.Dash)
        {
            playerMovement.StopDash();
        }
        else if (abilityData.abilityType == AbilityType.ReducedGravity)
        {
            playerMovement.UpdateMaxGravity(Constants.Player.MaxGravity);
        }
        else if (abilityData.abilityType == AbilityType.InvincibilityShield)
        {
            playerHp.SetShieldActivated(false);
        }
    }

    #endregion
}
