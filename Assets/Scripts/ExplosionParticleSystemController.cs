using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticleSystemController : MonoBehaviour
{
    [SerializeField] private float _workTime;

    private ParticleSystem _particleSystem;
    private float _particleLifetime;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particleLifetime = _particleSystem.main.startLifetime.constant;
        Invoke(nameof(Stop), _workTime);
    }

    private void Stop()
    {
        _particleSystem.Stop();
        GameObject.Destroy(gameObject, _particleLifetime);
    }
}
