using Unity.Netcode;
using UnityEngine.SceneManagement;

public class SceneLoader : NetworkBehaviour
{
    public static SceneLoader Singleton { get; private set; }

    private void Awake()
    {
        if (Singleton) Destroy(this);

        Singleton = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        //DontDestroyOnLoad(this);
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        Destroy(gameObject);
    }

    public void ReloadNetworkScene()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }
}