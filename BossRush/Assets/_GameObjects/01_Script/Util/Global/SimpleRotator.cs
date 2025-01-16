using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    [SerializeField] private bool autoInit;
    [SerializeField] private Vector3 axis;
    [SerializeField] private float speed;
    [SerializeField] private Space rotationSpace;
    private bool isRotating;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (autoInit)
        {
            isRotating = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating)
        {
            transform.Rotate(axis, speed * Time.deltaTime, rotationSpace);
        }
    }
}
