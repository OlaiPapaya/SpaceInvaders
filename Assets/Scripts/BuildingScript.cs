using UnityEngine;

public class BuildingScript : MonoBehaviour, Entity
{
    [SerializeField] private int _startingLife;
    private int _life;
    private float _startingXsize;

    private void Awake()
    {
        _life = _startingLife;
        _startingXsize = transform.localScale.x;
    }

    void Entity.Damage()
    {
        _life--;
        transform.localScale = new Vector3(_startingXsize * _life / _startingLife, transform.localScale.y, transform.localScale.z);
        if (_life <= 0) BuildingDestroyed();
    }

    void BuildingDestroyed()
    {
        Destroy(gameObject);
    }
}
