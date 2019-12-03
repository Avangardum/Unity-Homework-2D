using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerController : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawningPoint;
    [SerializeField] private Transform _eyesPosition;
    [SerializeField] private float _speed;
    [SerializeField] private float _shootingCooldown;
    [SerializeField] private float _patrolZoneSize;
    
    
    private Rigidbody2D _rigidbody2D;
    private readonly Quaternion _rightRotation = Quaternion.identity;
    private readonly Quaternion _leftRotation = Quaternion.Euler(0, 180, 0);
    private float _currentShootingCooldown;
    private float _patrolZoneLeftBorder;
    private float _patrolZoneRightBorder;
    private LayerMask _layerMask;
    private Direction _direction;

    private bool _playerInVision
    {
        get
        {
            RaycastHit2D hit = Physics2D.Raycast(_eyesPosition.position, transform.right, Mathf.Infinity, _layerMask);
            if (hit.collider == null)
                return false;
            return hit.collider.gameObject.CompareTag("Player");
        }
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _patrolZoneLeftBorder = transform.position.x - _patrolZoneSize / 2;
        _patrolZoneRightBorder = transform.position.x + _patrolZoneSize / 2;
        if (transform.eulerAngles.y == 0)
            _direction = Direction.Right;
        else
            _direction = Direction.Left;
        _layerMask = LayerMask.GetMask("Player", "Platform");
    }

    private void FixedUpdate()
    {
        DecreaseCooldowns();
        if (_playerInVision)
        {
            Shooting();
        }
        else
        {
            Moving();
        }
    }

    private void DecreaseCooldowns()
    {
        _currentShootingCooldown -= Time.fixedDeltaTime;
        if (_currentShootingCooldown < 0)
            _currentShootingCooldown = 0;
    }

    private void Shooting()
    {
        if (_currentShootingCooldown == 0)
        {
            GameObject.Instantiate(_bulletPrefab, _bulletSpawningPoint.position, _bulletSpawningPoint.rotation);
            _currentShootingCooldown = _shootingCooldown;
        }
    }

    private void Moving()
    {
        if (transform.position.x < _patrolZoneLeftBorder)
            TurnRight();
        if (transform.position.x > _patrolZoneRightBorder)
            TurnLeft();

        Vector2 movement = Vector2.zero;
        movement.x = _speed;
        movement *= Time.fixedDeltaTime;
        transform.Translate(movement, Space.Self);

        ////Тут я пытался сделать перемещение через изменение скорости, но несмотря на точ, что print показывал, что скорость меняется, по факту враг не двигался(хотя с игроком то же самое работало), я погуглил, сказали, что надо использовать AddForce в режиме ForceMode.VelocityChange, но в 2D нельзя выбирать режимы для AddForce, так что я забил, и сделал перемещение через Transform.Translate, но это костыль, так как тогда его не отбрасывают мины.
        //float xVelocity = 0;
        //if (_direction == Direction.Right)
        //    xVelocity = _speed;
        //if (_direction == Direction.Left)
        //    xVelocity = -_speed;
        //_rigidbody2D.velocity = new Vector2(xVelocity, _rigidbody2D.velocity.y);
        //print(_rigidbody2D.velocity);
    }

    private void TurnLeft()
    {
        transform.rotation = _leftRotation;
        _direction = Direction.Left;
    }

    private void TurnRight()
    {
        transform.rotation = _rightRotation;
        _direction = Direction.Right;
    }

    protected void LateUpdate()//Тут я вручную ограничиваю вращение по X, так как несмотря на включённый freeze rotation, при попадании на мину персонаж падает на бок
    {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
