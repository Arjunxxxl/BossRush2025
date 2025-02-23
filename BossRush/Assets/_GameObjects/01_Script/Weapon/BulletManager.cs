using System;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    #region SingleTon

    public static BulletManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    internal void SpawnBullet(string bulletTag, Vector3 spawnPos, Quaternion spawnRot, 
                                Vector3 moveDir, bool isTargetedBullet, Vector3 targetPt)
    {
        GameObject bulletGo = ObjectPooler.Instance.SpawnFormPool(bulletTag, spawnPos, spawnRot);
        Bullet bullet = bulletGo.GetComponent<Bullet>();
        bullet.SetUp(moveDir, isTargetedBullet, targetPt);
    }
}
