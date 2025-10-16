using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public bool isRotating = false;
    public bool rotateX = false;
    public bool rotateY = false;
    public bool rotateZ = false;
    public float rotationSpeedX = 90f; // Degrees per second
    public float rotationSpeedY = 90f; // Degrees per second
    public float rotationSpeedZ = 90f; // Degrees per second
    public bool isRotatingAround = false;
    public float rotationSpeedAround = 90f; // Degrees per second
    public Transform target;
    void Update()
    {
        if (isRotating)
        {
            Vector3 rotationVector = new Vector3(
                rotateX ? rotationSpeedX : 0,
                rotateY ? rotationSpeedY : 0,
                rotateZ ? rotationSpeedZ : 0
            );
            transform.Rotate(rotationVector * Time.deltaTime);
        }
        if (isRotatingAround)
        {
            transform.RotateAround(target.transform.position, Vector3.up, rotationSpeedAround);
        }
    }
}