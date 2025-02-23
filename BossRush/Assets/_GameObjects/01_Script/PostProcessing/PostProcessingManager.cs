using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class PostProcessingManager : MonoBehaviour
{
    private Volume volume;

    [Header("Lens Distortion")]
    [SerializeField] private PostProcessingStates lensDistortionAnimStates = PostProcessingStates.Unknown;
    private LensDistortion lensDistortion;
    [SerializeField] private float maxDistortion;
    [SerializeField] private float idleDistortion;
    private float lensCurDistortion;
    [SerializeField] private float lensDistortionChangeSpeed;

    [Header("Chromatic Abb")]
    [SerializeField] private PostProcessingStates chromaticAbbAnimStates = PostProcessingStates.Unknown;
    private ChromaticAberration chromaticAberration;
    [SerializeField] private float maxChromaticAbb;
    [SerializeField] private float idleChromaticAbb;
    private float curChromaticAbb;
    [SerializeField] private float chromaticAbbChangeSpeed;

    [Header("Vignette")]
    [SerializeField] private PostProcessingStates vignetteAnimStates = PostProcessingStates.Unknown;
    private Vignette vignette;
    [SerializeField] private float maxVignette;
    [SerializeField] private float idleVignette;
    private float curVignette;
    [SerializeField] private float vignetteChangeSpeed;

    #region SingleTon

    public static PostProcessingManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    #endregion
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLensDistortion();
        UpdateChromaticAberration();
        UpdateVignette();
    }

    #region SetUp

    private void SetUp()
    {
        volume = GetComponent<Volume>();

        SetUpLensDistortion();
        SetUpChromaticAberration();
        SetUpVignette();
    }

    #endregion

    #region Dash

    internal void SetPostProcessingDashVisuals()
    {
        SetLensDistortionToMaxDistortion();
        SetChromaticAberrationToMaxDistortion();
    }
    
    internal void SetPostProcessingIdleVisuals()
    {
        SetLensDistortionToIdleDistortion();
        SetChromaticAberrationToIdleDistortion();
    }

    #endregion

    #region Ability

    internal void SetPostProcessingTimeSlowValues()
    {
        SetVignetteToMaxDistortion();
    }

    internal void RemovePostProcessingTimeSlowValues()
    {
        SetVignetteToIdleDistortion();
    }

    #endregion
    
    #region Lens Distortion

    private void SetUpLensDistortion()
    {
        volume.profile.TryGet<LensDistortion>(out lensDistortion);
        lensCurDistortion = idleDistortion;

        lensDistortion.intensity.value = lensCurDistortion;

        lensDistortionAnimStates = PostProcessingStates.Idle;
    }

    private void SetLensDistortionToMaxDistortion()
    {
        lensDistortionAnimStates = PostProcessingStates.Max;
    }

    private void SetLensDistortionToIdleDistortion()
    {
        lensDistortionAnimStates = PostProcessingStates.Idle;
    }

    private void UpdateLensDistortion()
    {
        if (lensDistortionAnimStates == PostProcessingStates.Max)
        {
            lensCurDistortion = Mathf.Lerp(lensCurDistortion, maxDistortion,
                                           1 - Mathf.Pow(0.5f, Time.unscaledDeltaTime * lensDistortionChangeSpeed));
            
            if (Mathf.Abs(lensCurDistortion - maxDistortion) <= 0.01f)
            {
                lensCurDistortion = maxDistortion;
            }
            
            lensDistortion.intensity.value = lensCurDistortion;
        }
        else if (lensDistortionAnimStates == PostProcessingStates.Idle)
        {
            lensCurDistortion = Mathf.Lerp(lensCurDistortion, idleDistortion,
                                           1 - Mathf.Pow(0.5f, Time.unscaledDeltaTime * lensDistortionChangeSpeed));
            
            if (Mathf.Abs(lensCurDistortion - idleDistortion) <= 0.01f)
            {
                lensCurDistortion = idleDistortion;
            }
            
            lensDistortion.intensity.value = lensCurDistortion;
        }
    }

    #endregion
    
    #region Chromatic Aberration

    private void SetUpChromaticAberration()
    {
        volume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
        curChromaticAbb = idleChromaticAbb;

        chromaticAberration.intensity.value = curChromaticAbb;

        chromaticAbbAnimStates = PostProcessingStates.Idle;
    }

    private void SetChromaticAberrationToMaxDistortion()
    {
        chromaticAbbAnimStates = PostProcessingStates.Max;
    }

    private void SetChromaticAberrationToIdleDistortion()
    {
        chromaticAbbAnimStates = PostProcessingStates.Idle;
    }

    private void UpdateChromaticAberration()
    {
        if (chromaticAbbAnimStates == PostProcessingStates.Max)
        {
            curChromaticAbb = Mathf.Lerp(curChromaticAbb, maxChromaticAbb,
                                         1 - Mathf.Pow(0.5f, Time.unscaledDeltaTime * chromaticAbbChangeSpeed));
            
            if (Mathf.Abs(curChromaticAbb - maxChromaticAbb) <= 0.01f)
            {
                curChromaticAbb = maxChromaticAbb;
            }
            
            chromaticAberration.intensity.value = curChromaticAbb;
        }
        else if (chromaticAbbAnimStates == PostProcessingStates.Idle)
        {
            curChromaticAbb = Mathf.Lerp(curChromaticAbb, idleChromaticAbb,
                                         1 - Mathf.Pow(0.5f, Time.unscaledDeltaTime * chromaticAbbChangeSpeed));
            
            if (Mathf.Abs(curChromaticAbb - idleChromaticAbb) <= 0.01f)
            {
                curChromaticAbb = idleChromaticAbb;
            }
            
            chromaticAberration.intensity.value = curChromaticAbb;
        }
    }

    #endregion
    
    #region Vignette

    private void SetUpVignette()
    {
        volume.profile.TryGet<Vignette>(out vignette);
        curVignette = idleVignette;

        vignette.intensity.value = curVignette;

        vignetteAnimStates = PostProcessingStates.Idle;
    }

    private void SetVignetteToMaxDistortion()
    {
        vignetteAnimStates = PostProcessingStates.Max;
    }

    private void SetVignetteToIdleDistortion()
    {
        vignetteAnimStates = PostProcessingStates.Idle;
    }

    private void UpdateVignette()
    {
        if (vignetteAnimStates == PostProcessingStates.Max)
        {
            curVignette = Mathf.Lerp(curVignette, maxVignette,
                                     1 - Mathf.Pow(0.5f, Time.unscaledDeltaTime * vignetteChangeSpeed));
            
            if (Mathf.Abs(curVignette - maxVignette) <= 0.01f)
            {
                curVignette = maxVignette;
            }
            
            vignette.intensity.value = curVignette;
        }
        else if (vignetteAnimStates == PostProcessingStates.Idle)
        {
            curVignette = Mathf.Lerp(curVignette, idleVignette,
                                     1 - Mathf.Pow(0.5f, Time.unscaledDeltaTime * vignetteChangeSpeed));
            
            if (Mathf.Abs(curVignette - idleVignette) <= 0.01f)
            {
                curVignette = idleVignette;
            }
            
            vignette.intensity.value = curVignette;
        }
    }

    #endregion
}
