using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [System.Serializable]
    private class MovementData
    {
        public float moveSpeed;
        public float moveSpeedLerpFac;
        public float moveSpeedTarget;
        public float moveSpeedWalk;
        public float moveSpeedRun;
        public float moveSpeedDash;
        public Vector3 moveDir;
        public float ySpeed;
    }
    
    [System.Serializable]
    private class JumpData
    {
        public bool jumpTriggered;
        public bool jump;
        public bool jumpActive;
        public bool isJumping;
        public int jumpCt;
        
        [Space]
        
        public bool isForwardJump;
        public bool isBackwardJump;
        
        [Space]
        
        public float jumpBufferTimeElapsed;
        public float jumpActiveTimeElapsed;
        public float minJumpActiveDuration;
        
        [Space]
        
        public float jumpForceApplied;
        public float jumpForce;
    }
    
    [System.Serializable]
    private class GravityData
    {
        public float gravity;
        public float maxGravity;
        public float minGravity;
    }
    
    [System.Serializable]
    private class RotationData
    {
        public float rotationSpeed;
        public float rotationXSensitivity;
        public Vector3 finalRotationAngle;
    }

    [Header("Input")]
    [SerializeField] private Vector2 inputDir;
    [SerializeField] private bool isWalking;
    [SerializeField] private float mouseMoveX;

    [Header("Movement Data")]
    [SerializeField] private MovementData movementData;
    private bool isPlayerDashing;
    
    [Header("Jump Data")]
    [SerializeField] private JumpData jumpData;

    [Header("Gravity Data")]
    [SerializeField] private GravityData gravityData;

    [Header("Rotation Data")]
    [SerializeField] private RotationData rotationData;

    private CharacterController characterController;
    private Player player;

    private void Update()
    {
        //Input
        GetInput();

        //Jump
        CheckForJumpTrigger();
        KeepJumpInCache();
        SetJumpActive();
        UpdateJumpActiveTime();
        Jump();
        
        //Rotation
        Rotate();
    }

    private void FixedUpdate()
    {
        //Gravity
        ApplyGravity();
        
        //Movement
        Move();
    }

    #region SetUp

    internal void SetUp(Player player)
    {
        this.player = player;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        characterController = GetComponent<CharacterController>();

        jumpData.jumpCt = 0;

        SetUpGravity(Constants.Player.MaxGravity, Constants.Player.GroundedGravity);
    }

    #endregion
    
    #region Input

    private void GetInput()
    {
        inputDir.x = Input.GetAxisRaw("Horizontal");
        inputDir.y = Input.GetAxisRaw("Vertical");
        inputDir.Normalize();

        isWalking = Input.GetKey(KeyCode.LeftShift);
        jumpData.jumpTriggered = Input.GetKeyDown(KeyCode.Space);
        
        mouseMoveX = Input.GetAxis("Mouse X");
        mouseMoveX *= rotationData.rotationXSensitivity;
    }

    #endregion

    #region Movement

    private void Move()
    {
        movementData.moveDir = inputDir.x * transform.right +
                               inputDir.y * transform.forward;

        if (isPlayerDashing)
        {
            movementData.moveSpeedTarget = movementData.moveSpeedDash;
            movementData.moveSpeed = movementData.moveSpeedTarget;
        }
        else
        {
            movementData.moveSpeedTarget = isWalking ? movementData.moveSpeedWalk : movementData.moveSpeedRun;
        }
        
        movementData.moveSpeed = Mathf.Lerp(movementData.moveSpeed, movementData.moveSpeedTarget,
                                            Time.fixedUnscaledDeltaTime * movementData.moveSpeedLerpFac);

        movementData.moveDir *= movementData.moveSpeed;
        movementData.moveDir.y = movementData.ySpeed;
        
        characterController.Move(movementData.moveDir * Time.fixedUnscaledDeltaTime);
    }

    #endregion

    #region Dash
    
    internal void StartDash(float dashMoveSpeed)
    {
        isPlayerDashing = true;
        
        player.playerEfxManager.PlayDashEfx();
            
        PostProcessingManager.Instance.SetPostProcessingDashVisuals();
        CameraMovement.OnPlayerDash?.Invoke(true);
    }

    internal void StopDash()
    {
        isPlayerDashing = false;
        
        player.playerEfxManager.StopDashEfx();
            
        PostProcessingManager.Instance.SetPostProcessingIdleVisuals();
        CameraMovement.OnPlayerDash?.Invoke(false);
    }
    
    #endregion
    
    #region Jump
    
    private void CheckForJumpTrigger()
    {
        if (jumpData.jumpTriggered)
        {
            jumpData.jump = true;
            jumpData.jumpBufferTimeElapsed = Constants.Player.PlayerJumpBufferTime;
        }
    }
    
    private void KeepJumpInCache()
    {
        if (jumpData.jump)
        {
            jumpData.jumpBufferTimeElapsed -= Time.unscaledDeltaTime;

            if (jumpData.jumpBufferTimeElapsed <= 0)
            {
                jumpData.jump = false;
            }
        }
    }
    
    private void SetJumpActive()
    {
        if (jumpData.jump && jumpData.jumpCt < Constants.Player.PlayerMaxJumpCt)
        {
            jumpData.jumpActiveTimeElapsed = 0;

            if(Vector3.Dot(movementData.moveDir, transform.forward) > 0)
            {
                jumpData.isForwardJump = true;
                jumpData.isBackwardJump = false;
            }
            else if (Vector3.Dot(movementData.moveDir, transform.forward) < 0)
            {
                jumpData.isForwardJump = false;
                jumpData.isBackwardJump = true;
            }
            else
            {
                jumpData.isForwardJump = false;
                jumpData.isBackwardJump = false;
            }

            jumpData.jumpCt++;
            jumpData.jump = false;
            jumpData.jumpActive = true;
            jumpData.isJumping = true;

            if (!player.playerCollisionDetection.IsGrounded)
            {
                jumpData.jumpCt = Constants.Player.PlayerMaxJumpCt;
            }
        }
    }

    private void UpdateJumpActiveTime()
    {
        if(jumpData.jumpActive)
        {
            jumpData.jumpActiveTimeElapsed += Time.unscaledDeltaTime;

            if(jumpData.jumpActiveTimeElapsed > jumpData.minJumpActiveDuration && !jumpData.jump)
            {
                jumpData.jumpActiveTimeElapsed = 0;
                jumpData.jumpActive = false;
            }
        }
    }

    private void Jump()
    {
        if(jumpData.jumpActive)
        {
            jumpData.jumpForceApplied = jumpData.jumpForce;
            movementData.ySpeed = jumpData.jumpForceApplied;
        }
    }
    
    #endregion
    
    #region Gravity

    private void SetUpGravity(float maxGravity, float minGravity)
    {
        gravityData.maxGravity = maxGravity;
        gravityData.minGravity = minGravity;
    }

    internal void UpdateMaxGravity(float gravityVal)
    {
        gravityData.maxGravity = gravityVal;
    }
    
    private void ApplyGravity()
    {
        if (!player.playerCollisionDetection.IsGrounded)
        {
            gravityData.gravity = gravityData.maxGravity;
            movementData.ySpeed += gravityData.gravity * Time.fixedUnscaledDeltaTime;
        }
        else
        {
            if (!jumpData.jumpActive)
            {
                gravityData.gravity = gravityData.minGravity;
                movementData.ySpeed = 0;
                
                jumpData.jumpCt = 0;
                jumpData.isJumping = false;
            }
        }
    }
    
    #endregion
    
    #region Rotation
    
    private void Rotate()
    {
        rotationData.finalRotationAngle = transform.rotation.eulerAngles;
        rotationData.finalRotationAngle.y += mouseMoveX;

        rotationData.finalRotationAngle.y = Mathf.Repeat(rotationData.finalRotationAngle.y, 360);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotationData.finalRotationAngle), 
                                             Time.unscaledDeltaTime * rotationData.rotationSpeed);
    }
    
    #endregion
}
