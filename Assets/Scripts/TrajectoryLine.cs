using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryLine : MonoBehaviour
{
    [SerializeField] private InputScreen _input;
    [SerializeField] private Transform _grenade;
    [SerializeField] private Transform _pointView;
    [SerializeField] private float _maxZposition;
    [SerializeField] private float _minDistance;


    private LineRenderer _lineRenderer;
    private uint _maxPoints = 25;
    private bool _isCollision;
    private float zPosition;
    private Vector3 targetPosition;

    private const float TimeStep = 0.1f;
    private const float Half = 2f;

    public event UnityAction<Vector3> ChangedPush;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        _input.ChangedTouch += OnChangedTouch;
        _input.ChangedPosition += OnChangedPosition;
    }

    private void OnDisable()
    {
        _input.ChangedTouch -= OnChangedTouch;
        _input.ChangedPosition -= OnChangedPosition;
    }

    private void OnChangedTouch(bool active)
    {
        if (active == false)
        {
            zPosition = 0f;
            _lineRenderer.positionCount = 0;

            if (Vector3.Distance(_grenade.position,_pointView.position) > _minDistance)
                ChangedPush?.Invoke(targetPosition);
            
            targetPosition = Vector3.zero;
        }
        ChangeTrajectoryVisibility(active);
    }

    private void ChangeTrajectoryVisibility(bool isVisible)
    {
        _lineRenderer.enabled = isVisible;
        _pointView.gameObject.SetActive(isVisible);
    }

    private void OnChangedPosition(float x, float y)
    {
        _lineRenderer.enabled = true;
        zPosition += y;
        zPosition = Mathf.Clamp(zPosition, 0, _maxZposition);
        targetPosition = new Vector3(targetPosition.x + x, targetPosition.y + y, zPosition);

        ShowLine(_grenade.position, targetPosition);
        _isCollision = false;
    }

    private void ShowLine(Vector3 startPosition, Vector3 direction)
    {
        Vector3[] points = new Vector3[_maxPoints];
        Vector3 collisionPoint;
        float time;

        for (int i = 0; i < _maxPoints; i++)
        {
            time = i * TimeStep;
            points[i] = startPosition + direction * time + Physics.gravity * time * time / Half;


            _lineRenderer.positionCount = i;

            if (_isCollision)
                break;

            if (i > 0)
            {
                if (TryGetCollision(points[i - 1], points[i], out collisionPoint))
                {
                    _isCollision = true;
                    _pointView.position = collisionPoint;
                    
                    if (Vector3.Distance(_grenade.position, collisionPoint) > 5f)
                        _pointView.position -= new Vector3(0, 0, 0.1f);
                }
            }
        }
        _lineRenderer.SetPositions(points);
    }



    private bool TryGetCollision(Vector3 start, Vector3 end, out Vector3 collisionPosition)
    {
        float size = _lineRenderer.startWidth / Half;
        collisionPosition = Vector3.zero;
        float maxDistance = Vector3.Distance(start, end);
        Vector3 direction = (end - start).normalized;
        RaycastHit hit;

        if (Physics.SphereCast(start, size, direction, out hit, maxDistance))
        {
            collisionPosition = hit.point;
            return true;
        }

        return false;
    }
}
