using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AbilityManager : MonoBehaviour
{
    [Header("Ability Pool")]
    [SerializeField] private List<AbilityType> abilityPool;
    
    [Header("Primary Ability")]
    [SerializeField] private AbilityType primaryAbility;
    private AbilityData.Data primaryAbilityData;
    private bool isPrimaryAbilityActivated;
    private bool isPrimaryAbilityOnCoolDown;
    private float primaryAbilityTimeElapsed;
    
    [Header("Secondary Ability")]
    [SerializeField] private AbilityType secondaryAbilityType;
    private AbilityData.Data secondaryAbilityData;
    private bool isSecondaryAbilityActivated;
    private bool isSecondaryAbilityOnCoolDown;
    private float secondaryAbilityTimeElapsed;

    [Header("Data")]
    [SerializeField] private AbilityData abilityData;
    
    // Actions And Events
    public static Action<AbilityData.Data> ApplyAbilityEffect;
    public static Action<AbilityData.Data> RemoveAbilityEffect;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetUp();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivatePrimaryAbility();
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            ActivateSecondaryAbility();
        }

        TickPrimaryAbilityTimeElapsed();
        UpdatePrimaryAbilityCooldown();
        
        TickSecondaryAbilityTimeElapsed();
        UpdateSecondaryAbilityCooldown();
    }

    #region SetUp

    internal void SetUp()
    {
        CalcPrimaryAbilityData();
        CalcSecondaryAbility();
    }

    #endregion

    #region Primary Ability

    private void CalcPrimaryAbilityData()
    {
        primaryAbilityData = abilityData.CalcAbilityData(primaryAbility);
    }

    private void ActivatePrimaryAbility()
    {
        if (isPrimaryAbilityActivated || isPrimaryAbilityOnCoolDown)
        {
            return;
        }

        isPrimaryAbilityActivated = true;
        primaryAbilityTimeElapsed = 0f;
        
        ApplyAbilityEffect?.Invoke(primaryAbilityData);
    }

    private void RemovePrimaryAbilityEffect()
    {
        RemoveAbilityEffect?.Invoke(primaryAbilityData);
    }

    private void TickPrimaryAbilityTimeElapsed()
    {
        if (!isPrimaryAbilityActivated || isPrimaryAbilityOnCoolDown)
        {
            return;
        }

        primaryAbilityTimeElapsed += Time.unscaledDeltaTime;

        if (primaryAbilityTimeElapsed >= primaryAbilityData.abilityActiveDuration)
        {
            primaryAbilityTimeElapsed = 0f;
            isPrimaryAbilityActivated = false;

            RemovePrimaryAbilityEffect();
            SetPrimaryAbilityOnCoolDown();
        }
    }

    private void SetPrimaryAbilityOnCoolDown()
    {
        if (isPrimaryAbilityOnCoolDown)
        {
            return;
        }

        isPrimaryAbilityOnCoolDown = true;
        primaryAbilityTimeElapsed = 0f;
    }

    private void UpdatePrimaryAbilityCooldown()
    {
        if (!isPrimaryAbilityOnCoolDown)
        {
            return;
        }

        primaryAbilityTimeElapsed += Time.unscaledDeltaTime;

        if (primaryAbilityTimeElapsed >= primaryAbilityData.abilityCooldownDuration)
        {
            primaryAbilityTimeElapsed = 0f;
            isPrimaryAbilityOnCoolDown = false;
        }
    }

    #endregion
    
    #region Secondary Ability

    private void CalcSecondaryAbility()
    {
        int randAbilityIdx = Random.Range(0, abilityPool.Count);
        secondaryAbilityType = abilityPool[randAbilityIdx];
        secondaryAbilityData = abilityData.CalcAbilityData(secondaryAbilityType);
    }
    
    private void ActivateSecondaryAbility()
    {
        if (isSecondaryAbilityActivated || isSecondaryAbilityOnCoolDown)
        {
            return;
        }

        isSecondaryAbilityActivated = true;
        secondaryAbilityTimeElapsed = 0f;
        
        ApplyAbilityEffect?.Invoke(secondaryAbilityData);
    }
    
    private void RemoveSecondaryAbilityEffect()
    {
        RemoveAbilityEffect?.Invoke(secondaryAbilityData);
    }
    
    private void TickSecondaryAbilityTimeElapsed()
    {
        if (!isSecondaryAbilityActivated || isSecondaryAbilityOnCoolDown)
        {
            return;
        }

        secondaryAbilityTimeElapsed += Time.unscaledDeltaTime;

        if (secondaryAbilityTimeElapsed >= secondaryAbilityData.abilityActiveDuration)
        {
            secondaryAbilityTimeElapsed = 0f;
            isSecondaryAbilityActivated = false;

            RemoveSecondaryAbilityEffect();
            SetSecondaryAbilityOnCoolDown();
        }
    }
    
    private void SetSecondaryAbilityOnCoolDown()
    {
        if (isSecondaryAbilityOnCoolDown)
        {
            return;
        }

        isSecondaryAbilityOnCoolDown = true;
        secondaryAbilityTimeElapsed = 0f;
    }
    
    private void UpdateSecondaryAbilityCooldown()
    {
        if (!isSecondaryAbilityOnCoolDown)
        {
            return;
        }

        secondaryAbilityTimeElapsed += Time.unscaledDeltaTime;

        if (secondaryAbilityTimeElapsed >= secondaryAbilityData.abilityCooldownDuration)
        {
            secondaryAbilityTimeElapsed = 0f;
            isSecondaryAbilityOnCoolDown = false;
        }
    }

    #endregion
}
