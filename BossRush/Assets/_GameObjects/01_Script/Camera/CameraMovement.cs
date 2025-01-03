using System;
using Unity.Mathematics;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [System.Serializable]
    private class RigData
    {
        public Transform cameraRigT;
        public Vector3 rigFollowOffset;
        public float rigFollowSpeed;
        public float rigRotationSpeed;
        [HideInInspector] public Vector3 refVel;
    }
    
    [System.Serializable]
    private class PivotData
    {
        public Transform cameraPivotT;
        public Vector3 pivotOffset;
        public Vector3 pivotStandingOffset;
        public Vector3 pivotCrouchingOffset;
        public float pivotOffsetChangeSpeed;
        public float pivotRotationSpeed;
        public float pivotRotationSensitivity;
        public Vector3 pivotRotation;
        public Vector2 pivotRotationRange;
    }

    [Header("Input")]
    private float mouseMoveY;
    
    [Header("Target")]
    [SerializeField] private Transform playerT;

    [Header("Rig Data")]
    [SerializeField] private RigData rigData;

    [Header("Pivot Data")]
    [SerializeField] private PivotData pivotData;

    private void Start()
    {
        SetUpRig();
        SetUpPivot();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        
        RigFollowPlayer();
        RotateRig();
        
        UpdatePivotPos();
        RotatePivot();
    }

    #region Input

    private void GetInput()
    {
        mouseMoveY = Input.GetAxis("Mouse Y") * pivotData.pivotRotationSensitivity;
    }

    #endregion

    #region Rig

    private void SetUpRig()
    {
        rigData.cameraRigT.position = playerT.position + 
                                      (transform.right * rigData.rigFollowOffset.x) +
                                      (transform.up * rigData.rigFollowOffset.y) + 
                                      (transform.forward * rigData.rigFollowOffset.z);
    }

    private void RigFollowPlayer()
    {
        Vector3 destination = playerT.position + 
                              (transform.right * rigData.rigFollowOffset.x) +
                              (transform.up * rigData.rigFollowOffset.y) + 
                              (transform.forward * rigData.rigFollowOffset.z);

        rigData.cameraRigT.position = Vector3.SmoothDamp(rigData.cameraRigT.position,
                                                                    destination, ref rigData.refVel,
                                                                    1 - Mathf.Pow(0.5f,
                                                                        Time.deltaTime * rigData.rigFollowSpeed));
    }

    private void RotateRig()
    {
        Vector3 targetRotation = playerT.rotation.eulerAngles;
        targetRotation.x = 0;
        targetRotation.z = 0;
        
        rigData.cameraRigT.rotation = Quaternion.Lerp(rigData.cameraRigT.rotation,
                                                                Quaternion.Euler(targetRotation), 
                                                                1 - Mathf.Pow(0.5f, 
                                                                              Time.deltaTime * rigData.rigRotationSpeed));
    }

    #endregion

    #region Pivot

    private void SetUpPivot()
    {
        SetPivotToStandingPos();
        
        pivotData.cameraPivotT.localPosition = pivotData.pivotOffset;

        pivotData.pivotRotation = pivotData.cameraPivotT.localRotation.eulerAngles;
    }

    private void SetPivotToStandingPos()
    {
        pivotData.pivotOffset = pivotData.pivotStandingOffset;
    }

    private void SetPivotToCrouchingPos()
    {
        pivotData.pivotOffset = pivotData.pivotCrouchingOffset;
    }

    private void UpdatePivotPos()
    {
        pivotData.cameraPivotT.localPosition = Vector3.Lerp(pivotData.cameraPivotT.localPosition,
                                                                      pivotData.pivotOffset,
                                                                      1 - Mathf.Pow(0.5f,
                                                                          Time.deltaTime *
                                                                          pivotData.pivotOffsetChangeSpeed));
    }

    private void RotatePivot()
    {
        pivotData.pivotRotation.x += mouseMoveY * -1;

        if (pivotData.pivotRotation.x < pivotData.pivotRotationRange.x)
        {
            pivotData.pivotRotation.x = pivotData.pivotRotationRange.x;
        }
 
        if (pivotData.pivotRotation.x > pivotData.pivotRotationRange.y)
        {
            pivotData.pivotRotation.x = pivotData.pivotRotationRange.y;
        }
        
        pivotData.cameraPivotT.localRotation = Quaternion.Lerp(pivotData.cameraPivotT.localRotation,
                                                                         Quaternion.Euler(pivotData.pivotRotation), 
                                                                         1 - Mathf.Pow(0.5f, Time.deltaTime * 
                                                                                  pivotData.pivotRotationSpeed));
    }
    
    #endregion
}
