using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int _currentHealth;

    [SerializeField] private UnityEvent onDeathEvent;

    private TMP_Text _playerHealthText;
    
    private EnemyMind EnemyMind => GetComponent<EnemyMind>(); 

    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    private void Start()
    {
        if (PlayerHealthUI.Singleton)
        {
            _playerHealthText = PlayerHealthUI.Singleton.GetComponent<TMP_Text>();
            return;
        }
            Debug.LogWarning("No PlayerHealthUI Singleton");
    }

    public void LoseHealth(int damage = 1)
    {
        _currentHealth -= damage;
        
        if (gameObject.tag.Equals("Player"))
        {
            if (_playerHealthText)
            {
                _playerHealthText.text = _currentHealth.ToString();
            }
        }

        if (EnemyMind)
        {
            EnemyMind.FoundPlayer();
        }

        if (_currentHealth <= 0)
        {
            onDeathEvent?.Invoke();
            
            if (gameObject.tag.Equals("Player")) SceneLoader.Singleton.ReloadNetworkScene();
        }
    }

    public void Reset()
    {
        _currentHealth = maxHealth;
        
        if (gameObject.tag.Equals("Player"))
        {
            if (_playerHealthText)
            {
                _playerHealthText.text = _currentHealth.ToString();
            }
        }
    }
}
