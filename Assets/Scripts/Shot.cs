using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] Vector3 _movementDir;

    [SerializeField] float
        _speed,
        _disappearingTime;

    [SerializeField] bool
        _damagePlayer,
        _damageEnemies,
        _damageBuildings,
        _damageShots,
        _killedEnemyPointsAffectAbility;

    private void Start()
    {
        // Destroying the shot after a cooldown:
        Destroy(gameObject, _disappearingTime);
    }

    void Update()
    {
        // Moving in the shoot's direction
        transform.position += _movementDir.normalized * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the found object is not one we want to damage, we ignore it:
        if (other.CompareTag("Player") && !_damagePlayer ||
            other.CompareTag("Enemy") && !_damageEnemies ||
            other.CompareTag("Building") && !_damageBuildings ||
            other.CompareTag("Shot") && !_damageShots) return;
        Destroy(gameObject);

        // If an object can be damaged, it will have the interface 'Entity':
        Entity targetEntity = other.GetComponent<Entity>();
        // If it doesn't have it, it means it can't be damaged:
        if (targetEntity == null) return;
        // So the only thing we need to do is call the Damage() function from the interface:
        targetEntity.Damage(_killedEnemyPointsAffectAbility);
    }
}
