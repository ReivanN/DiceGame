using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject draggedObject;
    private Camera mainCamera;

    public GameObject prefabToSpawn;
    public Vector2 areaMin; // Минимальные координаты области
    public Vector2 areaMax; // Максимальные координаты области
    public GameObject areaVisualizerPrefab; // Префаб для визуализации области
    private GameObject areaVisualizer;

    private RoundManager.RoundPhase currentPhase;

    void OnEnable()
    {
        RoundManager.OnPhaseStart += UpdateState;
    }

    void OnDisable()
    {
        RoundManager.OnPhaseStart -= UpdateState;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void UpdateState(RoundManager.RoundPhase phase, int roundNumber)
    {
        currentPhase = phase;
    }

    private void VisualizeArea()
    {
        if (areaVisualizerPrefab != null)
        {
            areaVisualizer = Instantiate(areaVisualizerPrefab);
            areaVisualizer.transform.position = new Vector3((areaMin.x + areaMax.x) / 2, 0.1f, (areaMin.y + areaMax.y) / 2);
            areaVisualizer.transform.localScale = new Vector3(areaMax.x - areaMin.x, 1, areaMax.y - areaMin.y);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentPhase != RoundManager.RoundPhase.Preparation)
        {
            return;
        }

        draggedObject = Instantiate(prefabToSpawn);
        VisualizeArea();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentPhase != RoundManager.RoundPhase.Preparation || draggedObject == null)
        {
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 newPosition = hit.point;
            newPosition.y = draggedObject.transform.position.y;
            draggedObject.transform.position = newPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentPhase != RoundManager.RoundPhase.Preparation || draggedObject == null)
        {
            return;
        }

        Vector3 position = draggedObject.transform.position;
        if (position.x >= areaMin.x && position.x <= areaMax.x &&
            position.z >= areaMin.y && position.z <= areaMax.y)
        {
            draggedObject.transform.position = new Vector3(position.x, position.y, position.z);
        }
        else
        {
            Destroy(draggedObject); // Уничтожаем объект, если он за пределами разрешенной области
        }

        if (areaVisualizer != null)
        {
            Destroy(areaVisualizer); // Уничтожаем визуализатор области
        }
    }
}
