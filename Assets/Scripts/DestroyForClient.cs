using Unity.Netcode;

public class DestroyForClient : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (IsHost) return;
        
        Destroy(gameObject);
    }
}