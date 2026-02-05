using UnityEngine;

public class EnemyGroupController : MonoBehaviour
{
    [SerializeField] float
        _speed,
        _leapDown,
        _cooldownTime;

    float _lastTimeDirChanged;

    bool _goingRight = true;

    void Update()
    {
        transform.position += (_goingRight ? Vector3.right : Vector3.left) * _speed * Time.deltaTime;
    }

    public void DirChange()
    {
        // If the cooldown hasn't finished, don't change:
        if (_lastTimeDirChanged + _cooldownTime > Time.time) return;
        _goingRight = !_goingRight; // Change direction
        transform.position += Vector3.down * _leapDown; // Move enemies down
        _lastTimeDirChanged = Time.time; // Restart cooldown
    }
}
