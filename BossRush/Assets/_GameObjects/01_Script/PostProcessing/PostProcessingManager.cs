using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

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
    }

    #region SetUp

    private void SetUp()
    {
        volume = GetComponent<Volume>();

        SetUpLensDistortion();
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

    internal void SetLensDistortionToDashDistortion()
    {
        lensDistortionAnimStates = LensDistortionAnimStates.Dash;
    }

    internal void SetLensDistortionToIdleDistortion()
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
}
