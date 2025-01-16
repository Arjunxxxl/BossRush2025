using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RodManager : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject elementPrefab; // The prefab to spawn
    private GameObject container;
    
    [Header("Spawn Data")]
    public int elementCount = 25; // Number of elements to spawn
    public float circleRadius = 5f; // Radius of the circle
    public float spawnRadiusMin = 6f; // Minimum spawn distance from the center (must be greater than circleRadius)
    public float spawnRadiusMax = 10f; // Maximum spawn distance from the center
    public Vector3 spawnCenter = Vector3.zero; // Center of the circle

    void Start()
    {
        CleanUp();
        SpawnRods();
    }

    #region CleanUp

    private void CleanUp()
    {
        if (container != null)
        {
            Destroy(container);
        }

        container = new GameObject();
        container.name = "Container";
        container.transform.SetParent(transform);
    }

    #endregion
    
    #region Spawning

    private void SpawnRods()
    {
        if (elementPrefab == null)
        {
            Debug.LogError("Element prefab is not assigned.");
            return;
        }

        for (int i = 0; i < elementCount; i++)
        {
            SpawnElementOutsideCircle();
        }
    }

    void SpawnElementOutsideCircle()
    {
        // Ensure the spawn radius is valid
        if (spawnRadiusMin <= circleRadius)
        {
            Debug.LogError("spawnRadiusMin must be greater than circleRadius.");
            return;
        }

        // Randomize an angle in radians
        float angle = Random.Range(0f, Mathf.PI * 2f);

        // Randomize a distance within the valid spawn range
        float distance = Random.Range(spawnRadiusMin, spawnRadiusMax);

        // Calculate the spawn position
        float x = spawnCenter.x + distance * Mathf.Cos(angle);
        float y = spawnCenter.y + Random.Range(-5f, 5f);
        float z = spawnCenter.z + distance * Mathf.Sin(angle);
        Vector3 spawnPosition = new Vector3(x, y, z);

        // Instantiate the element
        GameObject rodObj = Instantiate(elementPrefab, spawnPosition, Quaternion.identity, container.transform);
        Rod rod = rodObj.GetComponent<Rod>();
        rod.SetUpRod();
    }
    
    #endregion
    
    #region Gizmo

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadiusMin);
        Gizmos.DrawWireSphere(transform.position, spawnRadiusMax);
    }

    #endregion
}
