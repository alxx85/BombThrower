using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Collider))]
public class RagDoll : MonoBehaviour
{
    [SerializeField] private bool _isActive;
    [SerializeField] private Collider[] _dollColliders;
    [SerializeField] private Rigidbody[] _dollRigidbodies;

    private Animator _animator;
    private Collider _characterCollider;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterCollider = GetComponent<Collider>();
    }

    public void Activate()
    {
        _animator.enabled = _isActive;
        _characterCollider.enabled = _isActive;
        ColliderActivator(_dollColliders, !_isActive);
        KinematicActivator(_dollRigidbodies, _isActive);
    }

    private void ColliderActivator(Collider[] colliders, bool active)
    {
        foreach (var collider in colliders)
        {
            collider.enabled = active;
        }
    }

    private void KinematicActivator(Rigidbody[] rigidbodies, bool active)
    {
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = active;
        }
    }
}
