using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _lifetime;
    [SerializeField] private int _damage;

    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _rigidbody2D.velocity = transform.right;
        Destroy(gameObject, _lifetime);
    }

    void Update()
    {
        _rigidbody2D.velocity = _rigidbody2D.velocity.normalized;
        _rigidbody2D.velocity *= _speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision?.gameObject?.GetComponent<Damagable>()?.Damage(_damage);
        GameObject.Destroy(gameObject);
    }
}
