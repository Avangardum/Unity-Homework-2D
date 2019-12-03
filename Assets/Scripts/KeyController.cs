using UnityEngine;

public class KeyController : MonoBehaviour
{
    [SerializeField] private GameObject _сonnectedDoor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _сonnectedDoor.SetActive(false);
            GameObject.Destroy(gameObject);
        }
    }
}
