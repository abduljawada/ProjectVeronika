using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class NetworkButtons : MonoBehaviour
{
    public void StartClient()
    {
        Debug.Log("Starting Client");
        UnloadScene();
        NetworkManager.Singleton.StartClient();
        Destroy(Movement.Singleton.gameObject.GetComponentInChildren<Light2D>().gameObject);
    }

    public void StartHost()
    {
        Debug.Log("Starting Host");
        UnloadScene();
        NetworkManager.Singleton.StartHost();
    }

    void UnloadScene()
    {
        Debug.Log("Unloading Scene");
        SceneManager.UnloadSceneAsync(0);
    }
}