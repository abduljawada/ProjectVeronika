using Unity.Netcode;
using UnityEngine;

public class DestroyForNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private bool destroyForHost;
    [SerializeField] private bool destroyForServer;
    [SerializeField] private bool destroyForClient;

    // Start is called before the first frame update
    void Start()
    {
        if (IsHost && destroyForHost || IsServer && destroyForServer || IsClient && !IsHost && destroyForClient) 
            Destroy(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Start();
    }
}