using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [System.Serializable]
    private class MovementData
    {
        public float moveSpeed;
        public float moveSpeedLerpFac;
        public float moveSpeedWalk;
        public float moveSpeedRun;
        public Vector3 moveDir;
        public float ySpeed;
    }
    
    [System.Serializable]
    private class JumpData
    {
        public bool jump;
        public bool jumpActive;
        public bool isJumping;
        
        [Space]
        
        public bool isForwardJump;
        public bool isBackwardJump;
        
        [Space]
        
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

    [Header("Input")]
    [SerializeField] private Vector2 inputDir;
    [FormerlySerializedAs("isRunning")]
    [SerializeField] private bool isWalking;

    [Header("Movement Data")]
    [SerializeField] private MovementData movementData;

    [Header("Jump Data")]
    [SerializeField] private JumpData jumpData;

    [Header("Gravity Data")]
    [SerializeField] private GravityData gravityData;

    private CharacterController characterController;
    private PlayerCollisionDetection playerCollisionDetection;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCollisionDetection = GetComponent<PlayerCollisionDetection>();
    }

    private void Update()
    {
        GetInput();

        SetJumpActive();
        UpdateJumpActiveTime();
        Jump();
        
        ApplyGravity();
    }

    private void FixedUpdate()
    {
        Move();
    }

    #region Input

    private void GetInput()
    {
        inputDir.x = Input.GetAxisRaw("Horizontal");
        inputDir.y = Input.GetAxisRaw("Vertical");
        inputDir.Normalize();

        isWalking = Input.GetKey(KeyCode.LeftShift);
        jumpData.jump = Input.GetKeyDown(KeyCode.Space);
    }

    #endregion

    #region Movement

    private void Move()
    {
        movementData.moveDir = inputDir.x * transform.right +
                               inputDir.y * transform.forward;

        movementData.moveSpeed = Mathf.Lerp(movementData.moveSpeed,
                                            isWalking ? movementData.moveSpeedWalk : movementData.moveSpeedRun,
                                            Time.deltaTime * movementData.moveSpeedLerpFac);

        movementData.moveDir *= movementData.moveSpeed;
        movementData.moveDir.y = movementData.ySpeed;
        
        characterController.Move(movementData.moveDir * Time.deltaTime);
    }

    #endregion
    
    #region Jump
    
    private void SetJumpActive()
    {
        if (jumpData.jump && !jumpData.isJumping)
        {
            if (!jumpData.jumpActive)
            {
                jumpData.jumpActiveTimeElapsed = 0;
            }

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

            jumpData.jumpActive = true;
            jumpData.isJumping = true;
        }
    }

    private void UpdateJumpActiveTime()
    {
        if(jumpData.jumpActive)
        {
            jumpData.jumpActiveTimeElapsed += Time.deltaTime;

            if(jumpData.jumpActiveTimeElapsed > jumpData.minJumpActiveDuration && !jumpData.jump)
            {
                jumpData.jumpActiveTimeElapsed = 0;
                jumpData.jumpActive = false;
            }
        }
    }

    private void Jump()
    {
        if(jumpData.jumpActive && playerCollisionDetection.IsGrounded)
        {
            
            jumpData.jumpForceApplied = jumpData.jumpForce;
            movementData.ySpeed = jumpData.jumpForceApplied;
        }
    }
    
    #endregion
    
    #region Gravity
    
    private void ApplyGravity()
    {
        if (!playerCollisionDetection.IsGrounded)
        {
            gravityData.gravity = gravityData.maxGravity;
            movementData.ySpeed += gravityData.gravity * Time.deltaTime;
        }
        else
        {
            if (!jumpData.jumpActive)
            {
                gravityData.gravity = gravityData.minGravity;
                movementData.ySpeed = 0;
                
                jumpData.isJumping = false;
            }
        }
    }
    
    #endregion
}