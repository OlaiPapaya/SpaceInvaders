using UnityEngine;

public class EnemyController : MonoBehaviour, Entity
{
    EnemyGroupController _enemyGroupScript;

    private void Start()
    {
        _enemyGroupScript = transform.parent.GetComponent<EnemyGroupController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If it's not a corner, we don't care:
        if (!other.CompareTag("LeftCorner") && !other.CompareTag("RightCorner")) return;

        // Tell the group of enemies to change the direction:
        _enemyGroupScript.DirChange();
    }

    void Entity.Damage()
    {
        
    }
}
