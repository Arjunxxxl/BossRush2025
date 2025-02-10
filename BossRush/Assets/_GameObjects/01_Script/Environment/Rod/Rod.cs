using UnityEngine;

public class Rod : MonoBehaviour
{
    [Header("Scale Data")]
    [SerializeField] private Vector3 rodMidScaleMin;
    [SerializeField] private Vector3 rodMidScaleMax;

    [Header("Objs")]
    [SerializeField] private GameObject rodMidObj;
    [SerializeField] private GameObject rodTopObj;
    [SerializeField] private GameObject rodBotObj;

    [Header("Oscilation movement")]
    [SerializeField] private Vector3 minPos;
    [SerializeField] private Vector3 maxPos;
    [SerializeField] private Vector2 oscilationSpeedRange;
    private float oscilationSpeed;
    private float osiclationFac;

    [Header("Collider")]
    [SerializeField] private BoxCollider rodBoxCollider;
    
    // Update is called once per frame
    void Update()
    {
        Oscilate();
    }

    #region SetUp

    internal void SetUpRod()
    {
        SetRodObjsScale();
        SetRodObjsPos();
        SetUp();
    }
    
    private void SetRodObjsScale()
    {
        Vector3 roadMidScale = new Vector3(Random.Range(rodMidScaleMin.x, rodMidScaleMax.x),
                                           Random.Range(rodMidScaleMin.y, rodMidScaleMax.y),
                                           Random.Range(rodMidScaleMin.z, rodMidScaleMax.z));

        Vector3 rodTopScale = rodTopObj.transform.localScale;
        rodTopScale.x = roadMidScale.x;
        rodTopScale.z = roadMidScale.z;
        
        Vector3 rodBotScale = rodBotObj.transform.localScale;
        rodBotScale.x = roadMidScale.x;
        rodBotScale.z = roadMidScale.z;
        
        rodMidObj.transform.localScale = roadMidScale;
        rodTopObj.transform.localScale = rodTopScale;
        rodBotObj.transform.localScale = rodBotScale;

        rodBoxCollider.size = new Vector3(roadMidScale.x,
                                          roadMidScale.y + + rodTopScale.y + rodBotScale.y,
                                          roadMidScale.z);
    }

    private void SetRodObjsPos()
    {
        Vector3 tobObjPos = rodMidObj.transform.localPosition;
        tobObjPos.y += rodMidObj.transform.localScale.y * 0.5f;
        tobObjPos.y += rodTopObj.transform.localScale.y * 0.5f;

        rodTopObj.transform.localPosition = tobObjPos;
        
        
        Vector3 botObjPos = rodMidObj.transform.localPosition;
        botObjPos.y -= rodMidObj.transform.localScale.y * 0.5f;
        botObjPos.y -= rodBotObj.transform.localScale.y * 0.5f;

        rodBotObj.transform.localPosition = botObjPos;
    }

    #endregion

    #region Oscilation

    private void SetUp()
    {
        Vector3 currentPos = transform.localPosition;
        minPos = currentPos + minPos;
        maxPos = currentPos + maxPos;

        transform.localPosition = new Vector3(Random.Range(minPos.x, maxPos.x),
                                              Random.Range(minPos.y, maxPos.y),
                                              Random.Range(minPos.z, maxPos.z));

        osiclationFac = Random.Range(0f, 1f);
        oscilationSpeed = Random.Range(oscilationSpeedRange.x, oscilationSpeedRange.y);
    }

    private void Oscilate()
    {
        osiclationFac = Mathf.PingPong(Time.time * oscilationSpeed, 1.0f);

        transform.localPosition = Vector3.Lerp(minPos, maxPos, osiclationFac);
    }

    #endregion
}
