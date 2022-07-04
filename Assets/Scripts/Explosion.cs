using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Explosion : MonoBehaviour
{
    [SerializeField] private TrajectoryLine _trajectoryLine;
    [SerializeField] private ParticleSystem _vfxBoom;
    [SerializeField] private float _delayDetanate;
    [SerializeField] private float _radius;

    private WaitForSeconds _waitDetanation;
    private WaitForSeconds _waitReturn;
    private Renderer[] _meshes;

    private const float DelayResetPosition = 2f;

    public event UnityAction Exploded;

    private void Start()
    {
        _waitReturn = new WaitForSeconds(DelayResetPosition);
        _meshes = GetComponentsInChildren<Renderer>();
    }

    private void OnEnable()
    {
        _trajectoryLine.ChangedPush += OnChangedPush;
    }

    private void OnDisable()
    {
        _trajectoryLine.ChangedPush -= OnChangedPush;
    }

    private void OnChangedPush(Vector3 direction)
    {
        _waitDetanation = new WaitForSeconds(_delayDetanate);
        StartCoroutine(BombActivate());
    }

    private IEnumerator BombActivate()
    {
        yield return _waitDetanation;
        GetTargets(_radius);
        StartVFX(transform.position);
        HideMesh(true);
        yield return _waitReturn;
        Exploded?.Invoke();
        HideMesh(false);
    }

    private void StartVFX(Vector3 position)
    {
        ParticleSystem vfx = Instantiate(_vfxBoom, position, Quaternion.identity);
    }

    private void HideMesh(bool visible)
    {
        for (int i = 0; i < _meshes.Length; i++)
        {
            _meshes[i].enabled = !visible;
        }
    }

    private void GetTargets(float radius)
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, radius);

        if (targets.Length > 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].TryGetComponent(out Destroy destroyObject))
                {
                    destroyObject.TakeDamage();
                }
            }
        }
    }
}
