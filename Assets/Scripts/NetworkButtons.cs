using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Netcode.Transports.UTP;

public class NetworkButtons : MonoBehaviour
{
    [SerializeField] private TMP_InputField ipInputField;
    private UnityTransport Transport => GetComponent<UnityTransport>();
    
    public void StartClient()
    {
        Debug.Log("Starting Client");
        Transport.ConnectionData.Address = ipInputField.text;
        UnloadScene();
        NetworkManager.Singleton.StartClient();
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