using UnityEngine;
using UnityEngine.Serialization;

public class EnemyEye : MonoBehaviour
{
    [SerializeField] float angleRange = 45f;
    [SerializeField] private float raycastOffset = 0.5f;
    public float sightDistance = 7f;
    public LayerMask raycastLayer;
    EnemyMind Mind => GetComponent<EnemyMind>();
    public Vector2 LastSeenPos { get; private set; }
    
    // Update is called once per frame
    void Update()
    {
        if (Mind.MyState != States.Attack)
        {
            if (IsPlayerInSight())
            {
                LastSeenPos = PlayerController.Singleton.transform.position;
                Mind.FoundPlayer();
            }
        }
        else
        {
            if (!IsPlayerInSight())
            {
                Mind.LostPlayer();
            }
            else
            {
                LastSeenPos = PlayerController.Singleton.transform.position;
                Mind.LookAtPlayer();
            }
        }

    }

    bool IsPlayerInSight()
    {
        if (!PlayerController.Singleton) return false;
        
        //Debug.Log("Player In Distance");
        Vector2 playerPos = PlayerController.Singleton.transform.position;
        Vector2 myPos = transform.position;
        float distX = playerPos.x - myPos.x;
        float distY = playerPos.y - myPos.y;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(distX, distY);
        angle += transform.eulerAngles.z;
        if (angle >= 360)
        {
            angle -= 360;
        }
        else if (angle < 0)
        {
            angle += 360;
        }
        //Debug.Log("angle after subtraction: " + angle);
        if (angle >= 360 - angleRange || angle <= angleRange)
        {
            //Debug.Log("Player In Angle Range");
            Vector3 pos = transform.position;
            
            RaycastHit2D raycastHit = Physics2D.Raycast(pos, (PlayerController.Singleton.transform.position - pos).normalized, sightDistance, raycastLayer);

            if (!raycastHit.collider) return false;
                
            if (raycastHit.transform.gameObject == PlayerController.Singleton.gameObject)
            {
                //Debug.Log("Player In Sight");
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        var position = transform.position;
        Gizmos.DrawWireSphere(position, sightDistance);
        Gizmos.color = Color.red;
        var eulerAngles = transform.eulerAngles;
        Gizmos.DrawLine(position, new Vector2(position.x + sightDistance * Mathf.Sin((angleRange - eulerAngles.z) *Mathf.Deg2Rad), position.y + sightDistance * Mathf.Cos((angleRange - eulerAngles.z) * Mathf.Deg2Rad)));
        Gizmos.DrawLine(position, new Vector2(position.x + sightDistance * Mathf.Sin(-(angleRange + eulerAngles.z) * Mathf.Deg2Rad), position.y + sightDistance * Mathf.Cos(-(angleRange + eulerAngles.z) * Mathf.Deg2Rad)));
    }
}