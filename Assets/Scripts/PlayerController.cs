using System;
using UnityEngine;

[RequireComponent(typeof(Damagable))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : SingletonMonoBehavoiur<PlayerController>
{
    #region Variables
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _minePrefab;
    [SerializeField] private Transform _bulletSpawningPoint;
    [SerializeField] private Transform _mineSpawningPoint;
    [SerializeField] private float _speed;
    [SerializeField] private float _walkAcceleration;
    [SerializeField] private float _brakeAcceleration;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _groundedError;//Расстояние до земли, при котором игрок считается приземлённым
    [SerializeField] private float _shootingCooldown;
    [SerializeField] private float _mineCooldown;
    [SerializeField] private int _mineCount;
    [SerializeField] private LayerMask _groundedLayerMask;

    [Header("Sound Sources")]
    [SerializeField] AudioSource _shootingSoundSource;

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private Quaternion _right = Quaternion.identity;
    private Quaternion _left = Quaternion.Euler(0, 180, 0);
    private Damagable _damagable;
    private float _currentShootingCooldown = 0;
    private float _currentMineCooldown = 0;
    #endregion

    #region Properties
    public int MineCount
    {
        get => _mineCount;
        private set
        {
            _mineCount = value;
            MineCountChanged.Invoke();
        }
    }

    private bool isGrounded
    {
        get
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, _groundedLayerMask);
            return hit.distance < _groundedError;
        }
    }
    #endregion

    public event Action MineCountChanged;
    public event Action Death;

    protected override void Awake()
    {
        base.Awake();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _damagable = GetComponent<Damagable>();
        _damagable.Death += () => Death?.Invoke();
    }

    private void Start()
    {
        Death += SceneLoader.Instance.ReloadScene;
    }

    private void Update()
    {
        if (PauseController.Instance.IsPaused)
            return;
        DecreaseCooldowns();
        Moving();
        Jumping();
        Shooting();
        MinePlacement();
    }

    private void Moving()
    {
        bool goRight = Input.GetAxis("Horizontal") == 1;
        bool goLeft = Input.GetAxis("Horizontal") == -1;
        bool idle = Input.GetAxis("Horizontal") == 0;
        _animator.SetBool("IsMoving", !idle);
        float xVelocity = _rigidbody2D.velocity.x;
        if (goLeft)
        {
            transform.rotation = _left;
            xVelocity -= _walkAcceleration * Time.deltaTime;
        }
        if (goRight)
        {
            transform.rotation = _right;
            xVelocity += _walkAcceleration * Time.deltaTime;
        }
        if (idle)
        {
            if (xVelocity > 0)
            {
                xVelocity -= _brakeAcceleration * Time.deltaTime;
                if (xVelocity < 0)
                    xVelocity = 0;
            }
            else if (xVelocity < 0)
            {
                xVelocity += _brakeAcceleration * Time.deltaTime;
                if (xVelocity > 0)
                    xVelocity = 0;
            }
        }
        xVelocity = Mathf.Clamp(xVelocity, -_speed, _speed);
        _rigidbody2D.velocity = new Vector2(xVelocity, _rigidbody2D.velocity.y);
    }

    private void Jumping()
    {
        _animator.SetBool("IsGrounded", isGrounded);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Shooting()
    {
        if (Input.GetButton("Fire1") && _currentShootingCooldown == 0)
        {
            Instantiate(_bulletPrefab, _bulletSpawningPoint.position, _bulletSpawningPoint.rotation);
            _currentShootingCooldown = _shootingCooldown;
            _shootingSoundSource.Play();
        }
    }

    private void DecreaseCooldowns()
    {
        _currentShootingCooldown -= Time.deltaTime;
        if (_currentShootingCooldown < 0)
            _currentShootingCooldown = 0;
        _currentMineCooldown -= Time.deltaTime;
        if (_currentMineCooldown < 0)
            _currentMineCooldown = 0;
    }

    private void MinePlacement()
    {
        if (Input.GetButton("Fire2") && MineCount > 0 && _currentMineCooldown == 0)
        {
            MineCount--;
            Instantiate(_minePrefab, _mineSpawningPoint.position, Quaternion.identity);
            _currentMineCooldown = _mineCooldown;
        }
    }
}
