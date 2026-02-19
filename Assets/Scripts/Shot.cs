using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] Vector3 movementDir;

    [SerializeField] float
        speed,
        disappearingTime;

    [SerializeField] bool
        damagePlayer,
        damageEnemies,
        damageBuildings;

    private void Start()
    {
        // Destroying the shot after a cooldown:
        Destroy(gameObject, disappearingTime);
    }

    void Update()
    {
        // Moving in the shoot's direction
        transform.position += movementDir.normalized * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the found object is not one we want to damage, we ignore it:
        if (other.CompareTag("Player") && !damagePlayer ||
            other.CompareTag("Enemy") && !damageEnemies ||
            other.CompareTag("Building") && !damageBuildings) return;
        Destroy(gameObject);

        // If an object can be damaged, it will have the interface 'Entity':
        Entity targetEntity = other.GetComponent<Entity>();
        // If it doesn't have it, it means it can't be damaged:
        if (targetEntity == null) return;
        // So the only thing we need to do is call the Damage() function from the interface:
        targetEntity.Damage();
    }
}
