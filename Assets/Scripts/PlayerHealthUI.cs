using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    public static PlayerHealthUI Singleton { get; private set; }

    private void Awake()
    {
        if (Singleton)
        {
            Destroy(gameObject);
        }
        else
        {
            Singleton = this;
        }
    }
}
