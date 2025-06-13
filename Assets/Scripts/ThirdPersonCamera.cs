using UnityEngine;

public class ThirdPersonCamera : Singleton<ThirdPersonCamera>
{
    new Camera camera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform aimTarget;
    void Start()
    {
        camera = GetComponent<Camera>();
    }
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        mousePos = camera.ScreenToWorldPoint(mousePos);
        aimTarget.position = mousePos;
        Vector3 direc = mousePos - transform.position;
        Physics.Raycast(transform.position, direc, Mathf.Infinity, layerMask);
        Debug.DrawRay(transform.position, direc, Color.red);
        // print(mousePos.x + " " + mousePos.y + " " + mousePos.z);
    }
}
