using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour
{
    private Volume volume;

    [Header("Lens Distortion")]
    [SerializeField] private LensDistortionAnimStates lensDistortionAnimStates = LensDistortionAnimStates.Unknown;
    private LensDistortion lensDistortion;
    [SerializeField] private float dashDistortion;
    [SerializeField] private float dashIdleDistortion;
    private float lensCurDistortion;
    [SerializeField] private float lensDistortionChangeSpeed;

    [Header("Chromatic Abb")]
    [SerializeField] private ChromaticAbbAnimStates chromaticAbbAnimStates = ChromaticAbbAnimStates.Unknown;
    private ChromaticAberration chromaticAberration;
    [SerializeField] private float dashChromaticAbb;
    [SerializeField] private float dashIdleChromaticAbb;
    private float curChromaticAbb;
    [SerializeField] private float chromaticAbbChangeSpeed;

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
    }

    #region SetUp

    private void SetUp()
    {
        volume = GetComponent<Volume>();

        SetUpLensDistortion();
        SetUpChromaticAberration();
    }

    #endregion

    #region Dash

    internal void SetPostProcessingDashVisuals()
    {
        SetLensDistortionToDashDistortion();
        SetChromaticAberrationToDashDistortion();
    }
    
    internal void SetPostProcessingIdleVisuals()
    {
        SetLensDistortionToIdleDistortion();
        SetChromaticAberrationToIdleDistortion();
    }

    #endregion
    
    #region Lens Distortion

    private void SetUpLensDistortion()
    {
        volume.profile.TryGet<LensDistortion>(out lensDistortion);
        lensCurDistortion = dashIdleDistortion;

        lensDistortion.intensity.value = lensCurDistortion;

        lensDistortionAnimStates = LensDistortionAnimStates.Idle;
    }

    private void SetLensDistortionToDashDistortion()
    {
        lensDistortionAnimStates = LensDistortionAnimStates.Dash;
    }

    private void SetLensDistortionToIdleDistortion()
    {
        lensDistortionAnimStates = LensDistortionAnimStates.Idle;
    }

    private void UpdateLensDistortion()
    {
        if (lensDistortionAnimStates == LensDistortionAnimStates.Dash)
        {
            lensCurDistortion = Mathf.Lerp(lensCurDistortion, dashDistortion,
                                           1 - Mathf.Pow(0.5f, Time.deltaTime * lensDistortionChangeSpeed));
            
            if (Mathf.Abs(lensCurDistortion - dashDistortion) <= 0.01f)
            {
                lensCurDistortion = dashDistortion;
            }
            
            lensDistortion.intensity.value = lensCurDistortion;
        }
        else if (lensDistortionAnimStates == LensDistortionAnimStates.Idle)
        {
            lensCurDistortion = Mathf.Lerp(lensCurDistortion, dashIdleDistortion,
                                           1 - Mathf.Pow(0.5f, Time.deltaTime * lensDistortionChangeSpeed));
            
            if (Mathf.Abs(lensCurDistortion - dashIdleDistortion) <= 0.01f)
            {
                lensCurDistortion = dashIdleDistortion;
            }
            
            lensDistortion.intensity.value = lensCurDistortion;
        }
    }

    #endregion
    
    #region Chromatic Aberration

    private void SetUpChromaticAberration()
    {
        volume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
        curChromaticAbb = dashIdleChromaticAbb;

        chromaticAberration.intensity.value = curChromaticAbb;

        chromaticAbbAnimStates = ChromaticAbbAnimStates.Idle;
    }

    private void SetChromaticAberrationToDashDistortion()
    {
        chromaticAbbAnimStates = ChromaticAbbAnimStates.Dash;
    }

    private void SetChromaticAberrationToIdleDistortion()
    {
        chromaticAbbAnimStates = ChromaticAbbAnimStates.Idle;
    }

    private void UpdateChromaticAberration()
    {
        if (chromaticAbbAnimStates == ChromaticAbbAnimStates.Dash)
        {
            curChromaticAbb = Mathf.Lerp(curChromaticAbb, dashChromaticAbb,
                                         1 - Mathf.Pow(0.5f, Time.deltaTime * chromaticAbbChangeSpeed));
            
            if (Mathf.Abs(curChromaticAbb - dashChromaticAbb) <= 0.01f)
            {
                curChromaticAbb = dashChromaticAbb;
            }
            
            chromaticAberration.intensity.value = curChromaticAbb;
        }
        else if (chromaticAbbAnimStates == ChromaticAbbAnimStates.Idle)
        {
            curChromaticAbb = Mathf.Lerp(curChromaticAbb, dashIdleChromaticAbb,
                                         1 - Mathf.Pow(0.5f, Time.deltaTime * chromaticAbbChangeSpeed));
            
            if (Mathf.Abs(curChromaticAbb - dashIdleChromaticAbb) <= 0.01f)
            {
                curChromaticAbb = dashIdleChromaticAbb;
            }
            
            chromaticAberration.intensity.value = curChromaticAbb;
        }
    }

    #endregion
}
