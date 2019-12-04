using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZoneController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damagable = collision.gameObject.GetComponent<Damagable>();
        if (damagable != null)
        {
            damagable.Die();
        }
    }
}
