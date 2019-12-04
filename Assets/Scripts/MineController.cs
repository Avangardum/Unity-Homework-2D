using UnityEngine;

public class MineController : MonoBehaviour
{
    [SerializeField] private GameObject _particleSystemPrefab;
    [SerializeField] private Transform _particleSystemSpawningPosition;
    [SerializeField] private LayerMask _explosionLayerMask;
    [SerializeField] private float _knockback;
    [SerializeField] private float _blastRadius;
    [SerializeField] private int _damage;

    private bool _wasExploded = false;
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_wasExploded)
            return;
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
            Explode();
    }

    private void Explode()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, _blastRadius, _explosionLayerMask);
        foreach(var collider in colliders)
        {
            Rigidbody2D anotherRigidbody2D = collider.GetComponent<Rigidbody2D>();
            if(anotherRigidbody2D != null)
            {
                Vector2 knockbackVector = collider.transform.position - this.transform.position;
                knockbackVector.Normalize();
                knockbackVector *= _knockback;
                anotherRigidbody2D.AddForce(knockbackVector);
            }
            Damagable AnotherDamagable = collider.GetComponent<Damagable>();
            if(AnotherDamagable != null)
            {
                AnotherDamagable.Damage(_damage);
            }
        }
        _wasExploded = true;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().Sleep();
        Instantiate(_particleSystemPrefab, _particleSystemSpawningPosition.position, Quaternion.identity);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        Destroy(gameObject, audioSource.clip.length);

    }    
}
