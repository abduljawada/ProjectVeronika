using UnityEngine;
using UnityEngine.Events;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField] private UnityEvent onDamageEvent;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            onDamageEvent.Invoke();
        }
    }
}