using System;
using System.Linq;
using UISystem;
using UnityEngine;
public class DragSystem : MonoBehaviour
{
    [SerializeField] private bool isDragging;
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private GameObject draggedGameObject;
    private IDraggable _draggable;
    private int _initialLayer;

    private Vector3 _worldPosition;
    private Plane _plane = new Plane(Vector3.up, 0);

    private UIHandler _uiHandler;
    public void Awake()
    {
        _uiHandler = GetComponent<UIHandler>();
    }

    public void Update()
    {
        if (!_uiHandler.CanDrag)
        {
            if (isDragging) StopCurrentDrag();
            return;
        }

        UpdatePointerPosition();
        if (Input.GetMouseButton(0)) HandleMouseHeld();
        else if (isDragging) StopCurrentDrag();
    }

    private void UpdatePointerPosition()
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_plane.Raycast(ray, out distance)) _worldPosition = ray.GetPoint(distance);
    }

    private void HandleMouseHeld()
    {
        if (!isDragging) AttemptToDragObject();
        else UpdateTargetPosition();
    }

    private void AttemptToDragObject()
    {
        var target = GetGameobjectUnderMouse();
        var draggable = target.GetComponents<MonoBehaviour>().OfType<IDraggable>().FirstOrDefault();

        if (draggable == null) return;

        isDragging = true;
        _draggable = draggable;
        draggedGameObject = target;
        initialPosition = target.transform.position;
        _initialLayer = target.layer;
        target.layer = 2; // Ignore layer. Add something to replace magic number?

        UpdateTargetPosition();
    }
    private static GameObject GetGameobjectUnderMouse()
    {
        var ray = GetMousePosition();
        var didHit = Physics.Raycast(ray, out var hit);
        if (!didHit) return null;

        return hit.transform.gameObject;
    }

    private static Ray GetMousePosition() => Camera.main.ScreenPointToRay(Input.mousePosition);

    private void UpdateTargetPosition() => draggedGameObject.transform.position = _worldPosition;

    private void StopCurrentDrag()
    {
        var target = GetGameobjectUnderMouse();
        isDragging = false;
        draggedGameObject.transform.position = initialPosition;
        draggedGameObject.layer = _initialLayer;
        draggedGameObject = null;

        _draggable.OnDragStop(target);
    }
}
