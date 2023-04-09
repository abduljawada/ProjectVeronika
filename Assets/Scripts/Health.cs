using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int _currentHealth;

    [SerializeField] private UnityEvent onDeathEvent;

    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    public void LoseHealth(int damage = 1)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            onDeathEvent?.Invoke();
            
            if (gameObject.tag.Equals("Player")) SceneLoader.Singleton.ReloadNetworkScene();
        }
    }

    public void Reset()
    {
        _currentHealth = maxHealth;
    }
}
