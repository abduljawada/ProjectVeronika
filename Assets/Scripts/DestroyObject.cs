using Unity.Netcode;

public class DestroyObject : NetworkBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
    }

    [ClientRpc]
    public void DestroyClientRPC()
    {
        Destroy(gameObject);
    }
}
