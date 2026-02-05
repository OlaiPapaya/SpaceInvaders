using UnityEngine;

public class CornerPositioner : MonoBehaviour
{
    [SerializeField] bool isRight;

    void Start()
    {
        // This just puts the corner detector in the exact position of the edge of the camera.
        transform.position = new Vector3(
            Camera.main.ScreenToWorldPoint(Vector3.zero).x * (isRight ? -1 : 1),
            transform.position.y, transform.position.z
        );
    }
}
