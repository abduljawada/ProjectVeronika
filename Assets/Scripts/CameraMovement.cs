using Unity.Netcode;
using UnityEngine;

public class CameraMovement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    // Update is called once per frame
    void Update()
    {
        if (!IsHost)
        {
            Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
            transform.position += moveDir * (moveSpeed * Time.deltaTime);
        }
        else
        {
            if (!PlayerController.Singleton) return;
            transform.position = PlayerController.Singleton.transform.position + Vector3.forward * -10;
        }
        

    }
}