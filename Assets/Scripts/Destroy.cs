using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField] private ParticleSystem _vfxDestroy;
    [SerializeField] private RagDoll _killEnemy;

    public void TakeDamage()
    {
        if (_vfxDestroy == null)
        {
            if (_killEnemy != null)
            {
                _killEnemy.Activate();
            }
        }
        else
        { 
            Instantiate(_vfxDestroy, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
