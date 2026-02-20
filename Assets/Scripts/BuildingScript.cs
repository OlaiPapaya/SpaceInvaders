using UnityEngine;

public class BuildingScript : MonoBehaviour, Entity
{
    [SerializeField] private int _startingLife;
    [SerializeField] GameObject _deathParticles;
    private int _life;
    private float _startingXsize;

    private void Awake()
    {
        // Initializing the life and saving the size in the beginning:
        _life = _startingLife;
        _startingXsize = transform.localScale.x;
    }

    void Entity.Damage(bool addPoints)
    {
        // When shot at, the building loses a life, and if it's the last one, we call the destroy function:
        _life--;
        if (_life <= 0)
        {
            BuildingDestroyed();
            return;
        }
        // If the building hasn't died, it shrinks in an amount exact so that when it dies, the size would be zero:
        transform.localScale = new Vector3(_startingXsize * _life / _startingLife, transform.localScale.y, transform.localScale.z);
    }

    void BuildingDestroyed()
    {
        // When destroyed, we just add some particles and erase the building:
        Instantiate(_deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
