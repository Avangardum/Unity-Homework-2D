using UnityEngine;
using UnityEngine.UI;

public class UIController : SingletonMonoBehavoiur<UIController>
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private Text _mineCount;

    private GameObject _player;
    private PlayerController _playerController;
    private Damagable _playerDamagable;

    private void UpdateHealth()
    {
        float healthPercentage = (float)_playerDamagable.Health / _playerDamagable.MaxHealth;
        _healthBar.fillAmount = healthPercentage;
    }

    private void UpdateMineCount()
    {
        _mineCount.text = $"<b>{_playerController.MineCount}</b>";
    }

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = PlayerController.Instance;
        _playerDamagable = _player.GetComponent<Damagable>();
        _playerDamagable.HealthChanged += UpdateHealth;
        _playerController.MineCountChanged += UpdateMineCount;
        UpdateMineCount();
        UpdateHealth();
    }
}
