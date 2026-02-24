using UnityEngine;

public class DetectEnemyArrived : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

        GameManager.instance.EnemiesArrived();
    }
}
