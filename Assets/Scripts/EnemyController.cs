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

    private void Update()
    {
        // If the enemies arrive to the end and the game isn't over yet, the player loses:
        if (transform.position.y <= GameManager.instance._enemyWinYposition &&
            !GameManager.instance.GameOver) GameManager.instance.EnemiesArrived();
    }

    void Entity.Damage()
    {
        _enemyGroupScript.EnemyDied(); // Increase the speed of the group of enemies
        Destroy(gameObject);
    }
}
