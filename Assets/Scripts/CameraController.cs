using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    private void LateUpdate()
    {
        if (_player == null)
            return;
        Vector2 newPosition = Vector2.Lerp(transform.position, _player.transform.position, Time.deltaTime);
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }
}
