using UnityEngine;
using UnityEngine.UIElements;

public class ThirdPersonCamera : Singleton<ThirdPersonCamera>
{
    new Camera camera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform aimTarget;
    private bool isSelecting = false;
    private bool isBuilding = false;
    private TowerHologram hologram;
    private BuildUI buildUI;
    void Start()
    {
        buildUI = BuildUI.Instance;
        buildUI.gameObject.SetActive(false);
        camera = GetComponent<Camera>();
    }
    void Update()
    {
        if (GameManager.Instance.IsPausing()) return;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        mousePos = camera.ScreenToWorldPoint(mousePos);
        aimTarget.position = mousePos;
        Vector3 direc = mousePos - transform.position;
        if (Input.GetKeyDown(KeyCode.P))
        {
            isSelecting = !isSelecting;
            if (isSelecting)
            {
                if (buildUI == null) buildUI = BuildUI.Instance;
                buildUI.gameObject.SetActive(true);
            }
            else
            {
                if (buildUI == null) buildUI = BuildUI.Instance;
                buildUI.gameObject.SetActive(false);
            }
        }
        if (!isBuilding && Input.GetKeyDown(KeyCode.B))
            {
                isBuilding = true;
                hologram = Instantiate(LevelManager.Instance.GetSelectedTower(), transform.position, Quaternion.identity);
                TowerHologram.onComplete += OnBuildComplete;
            }
        if (isBuilding && Input.GetKeyDown(KeyCode.Escape))
        {
            isBuilding = false;
            hologram.Cancel();
            hologram = null;
            TowerHologram.onComplete -= OnBuildComplete;
        }
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, direc, out raycastHit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, direc, Color.red);
            // print(mousePos.x + " " + mousePos.y + " " + mousePos.z);
            if (isBuilding)
            {
                hologram.transform.position = raycastHit.point;
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(hologram.Build(raycastHit.point));
                }
            }
        }
    }
    private void OnBuildComplete(bool valid)
    {
        if (valid)
        {
            isBuilding = false;
            hologram = null;
            TowerHologram.onComplete -= OnBuildComplete;
        }
    }
}
