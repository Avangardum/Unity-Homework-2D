using UnityEngine;

public class FinishController : MonoBehaviour
{
    [SerializeField] private string _nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            SceneLoader.Instance.LoadScene(_nextScene);
    }
}
