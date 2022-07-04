using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Explosion))]
public class GrenadeMover : MonoBehaviour
{
    [SerializeField] private TrajectoryLine _trajectoryLine;

    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Rigidbody _rigidbody;
    private Explosion _explosion;

    private void Awake()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        _rigidbody = GetComponent<Rigidbody>();
        _explosion = GetComponent<Explosion>();
    }

    private void OnEnable()
    {
        _trajectoryLine.ChangedPush += OnChangedPush;
        _explosion.Exploded += BombReset;
    }

    private void OnDisable()
    {
        _trajectoryLine.ChangedPush -= OnChangedPush;
        _explosion.Exploded -= BombReset;
    }

    private void OnChangedPush(Vector3 direction)
    {
        Active(false);
        _rigidbody.AddForce(direction, ForceMode.VelocityChange);
        _rigidbody.AddTorque(direction, ForceMode.Impulse);
    }

    private void BombReset()
    {
        _rigidbody.velocity = Vector3.zero;
        transform.position = _startPosition;
        transform.rotation = _startRotation;
        Active(true);
    }

    private void Active(bool active)
    {
        _rigidbody.isKinematic = active;
        _trajectoryLine.enabled = active;
    }
}
