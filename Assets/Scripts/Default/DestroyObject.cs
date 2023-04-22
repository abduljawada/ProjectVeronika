using Unity.Netcode;

public class DestroyObject : NetworkBehaviour
{
    [ClientRpc]
    public void DestroyClientRPC()
    {
        Destroy(gameObject);
    }
}