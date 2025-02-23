using System;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    [Header("Time Scale Data")]
    [SerializeField] private float timeScaleUpdateSpeed;
    [SerializeField] private AnimationCurve timeScaleUpdateCurve;
    private float startTimeScale;
    private float targetTimeScale;
    private float currentTimeScale;
    private float timeElapsed;

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

    private void Update()
    {
        UpdateCurrentTimeScale();
    }

    #region SetUp

    internal void SetUp()
    {
        timeElapsed = 0;
        startTimeScale = Constants.TimeScale.DefaultTimeScale;
        targetTimeScale = Constants.TimeScale.DefaultTimeScale;
        currentTimeScale = targetTimeScale;
        
        SetTimeScaleToCurrent();
    }

    #endregion

    #region Time Scale

    private void UpdateCurrentTimeScale()
    {
        timeElapsed += Time.unscaledDeltaTime * timeScaleUpdateSpeed;
        float delta = timeScaleUpdateCurve.Evaluate(timeElapsed);
        currentTimeScale = Mathf.Lerp(startTimeScale, targetTimeScale, delta);

        SetTimeScaleToCurrent();
    }

    private void SetTimeScaleToCurrent()
    {
        Time.timeScale = currentTimeScale;
        Time.fixedDeltaTime = Constants.TimeScale.DefaultFixedDeltaTime * currentTimeScale;
    }

    #endregion
    
    #region Ability

    private void ApplyAbilityEffect(AbilityData.Data abilityData)
    {
        if (abilityData.abilityType == AbilityType.TimeSlow)
        {
            timeElapsed = 0;
            startTimeScale = currentTimeScale;
            targetTimeScale = abilityData.abilityEffectiveness;
            
            PostProcessingManager.Instance.SetPostProcessingTimeSlowValues();
        }
    }
    
    private void RemoveAbilityEffect(AbilityData.Data abilityData)
    {
        if (abilityData.abilityType == AbilityType.TimeSlow)
        {
            timeElapsed = 0;
            startTimeScale = currentTimeScale;
            targetTimeScale = Constants.TimeScale.DefaultTimeScale;
            
            PostProcessingManager.Instance.RemovePostProcessingTimeSlowValues();
        }
    }

    #endregion
}
