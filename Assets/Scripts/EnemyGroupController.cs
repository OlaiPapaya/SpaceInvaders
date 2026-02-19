using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyGroupController : MonoBehaviour
{
    float _speed;
    public float Speed { set { _speed = value; } }

    bool _goingRight = true;
    public bool GoingRight { set { _goingRight = value; } }

    void Update()
    {
        // Moving the group of enemies in the current horizontal direction:
        transform.position += (_goingRight ? Vector3.right : Vector3.left) * _speed * Time.deltaTime;
    }
}
