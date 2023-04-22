using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class TriggerForNetworkPlayer : NetworkBehaviour
{
    [FormerlySerializedAs("TriggerForHost")]
    [FormerlySerializedAs("disableForHost")]
    [FormerlySerializedAs("destroyForHost")]
    [SerializeField]
    private bool triggerForHost;

    [FormerlySerializedAs("TriggerForServer")]
    [FormerlySerializedAs("disableForServer")]
    [FormerlySerializedAs("destroyForServer")]
    [SerializeField]
    private bool triggerForServer;

    [FormerlySerializedAs("TriggerForClient")]
    [FormerlySerializedAs("disableForClient")]
    [FormerlySerializedAs("destroyForClient")]
    [SerializeField]
    private bool triggerForClient;

    [FormerlySerializedAs("TriggerEvent")] [SerializeField]
    private UnityEvent triggerEvent;

    // Start is called before the first frame update
    void Start()
    {
        if (IsHost && triggerForHost || IsServer && triggerForServer || IsClient && !IsHost && triggerForClient)
            triggerEvent?.Invoke();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Start();
    }
}