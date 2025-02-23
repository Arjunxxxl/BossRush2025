using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movement Data")]
    [SerializeField] private float moveSpeed;
    private Vector3 moveDir;
    private bool isActive;
    private bool isTargetedBullet;
    private Vector3 targetPt;

    [Header("Time To Live Data")]
    [SerializeField] private float timeToLive;
    private float timeActive;

    [Header("Collision Data")]
    [SerializeField] private Vector3 boxCastSize;

    [Header("Impact Data")]
    [SerializeField] private string bulletImpactEfxTag;

    private void Update()
    {
        MoveBullet();
        UpdateTimeToLive();
        CheckForCollision();
    }

    #region Set Up
 
    internal void SetUp(Vector3 moveDir, bool isTargetedBullet, Vector3 targetPt)
    {
        this.moveDir = moveDir;
        this.isTargetedBullet = isTargetedBullet;
        this.targetPt = targetPt;
        
        timeActive = 0;
        
        isActive = true;
    }

    #endregion

    #region Movement

    private void MoveBullet()
    {
        if (isActive)
        {
            if (isTargetedBullet)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPt,
                                                         (float)(1.0f - 
                                                                 Math.Pow(0.5f, Time.deltaTime * moveSpeed)));

                if (Vector3.Distance(transform.position, targetPt) < 0.05f)
                {
                    isActive = false;
                    gameObject.SetActive(false);
                }
            }
            else
            {
                transform.Translate(moveDir * moveSpeed * Time.deltaTime);
            }
        }
    }

    #endregion

    #region Time to live

    private void UpdateTimeToLive()
    {
        if (isActive)
        {
            timeActive += Time.deltaTime;

            if (timeActive >= timeToLive)
            {
                gameObject.SetActive(false);
            }
        }
    }

    #endregion

    #region Collision

    private void CheckForCollision()
    {
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, 
                                               boxCastSize * 0.5f, 
                                               transform.forward, 
                                               Quaternion.identity, 0);

        if (hits.Length > 0)
        {
            SpawnBulletImpactEfx(transform.position);
            gameObject.SetActive(false);
        }
    }

    #endregion

    #region Impact

    private void SpawnBulletImpactEfx(Vector3 spawnPos)
    {
        ObjectPooler.Instance.SpawnFormPool(bulletImpactEfxTag, spawnPos, Quaternion.identity);
    }

    #endregion
    
    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxCastSize);
    }

    #endregion
}
