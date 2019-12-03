using System;
using UnityEngine;

public class ChomperController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private float _hitCooldown;
    private float _currentHitCoolown = 0;
    private GameObject _player;
    private Quaternion _right = Quaternion.identity;
    private Quaternion _left = Quaternion.Euler(0, 180, 0);
    /// <summary>
    /// Максимальное расстояние от игрока по оси X, при котором моб не разворачивается в его сторону
    /// </summary>
    [SerializeField] private float _maxNotFollowDistance;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        Rotate();
        transform.Translate(Vector3.right * _speed * Time.deltaTime, Space.Self);
        DecreaseCooldowns();
    }

    private void Rotate()
    {
        if (_player == null)
            return;
        float _xDistance = Mathf.Abs(_player.transform.position.x - transform.position.x);
        if (_xDistance < _maxNotFollowDistance)
            return;
        if (_player.transform.position.x > transform.position.x)
            transform.rotation = _right;
        else
            transform.rotation = _left;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("Platform"))
        //    TurnAround();
        /*else */if (collision.gameObject.CompareTag("Player") && _currentHitCoolown == 0)
        {
            collision.gameObject.GetComponent<Damagable>().Damage(_damage);
            _currentHitCoolown = _hitCooldown;
        }
    }

    void TurnAround()
    {
        var rotation = transform.eulerAngles;
        if (rotation.y == 0)
            rotation.y = 180;
        else
            rotation.y = 0;
        transform.eulerAngles = rotation;
    }

    void DecreaseCooldowns()
    {
        _currentHitCoolown -= Time.deltaTime;
        if (_currentHitCoolown < 0)
            _currentHitCoolown = 0;
    }
}
