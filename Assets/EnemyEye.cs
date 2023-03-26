using UnityEngine;

public class EnemyEye : MonoBehaviour
{
    [SerializeField] float angleRange = 45f;
    [SerializeField] float sightDistance = 7f;
    EnemyMind Mind => GetComponent<EnemyMind>();
    public Vector2 LastSeenPos { get; private set; }
    
    // Update is called once per frame
    void Update()
    {
        if (Mind.MyState != States.Attack)
        {
            if (IsPlayerInSight())
            {
                LastSeenPos = Movement.Singleton.transform.position;
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
                LastSeenPos = Movement.Singleton.transform.position;
            }
        }

    }

    bool IsPlayerInSight()
    {
        if (!Movement.Singleton) return false;
        
        if (Vector2.Distance(transform.position, Movement.Singleton.transform.position) < sightDistance)
        {
            Debug.Log("Player In Distance");
            float distX = Movement.Singleton.transform.position.x - transform.position.x;
            float distY = Movement.Singleton.transform.position.y - transform.position.y;
            float angle = Mathf.Rad2Deg * Mathf.Atan2(distX, distY);
            angle += transform.eulerAngles.z;
            if (angle >= 360)
            {
                angle -= 360;
            }
            else if (angle <= -360)
            {
                angle += 360;
            }
            //Debug.Log("angle after subtraction: " + angle);
            if (angle >= -angleRange && angle <= angleRange)
            {
                Debug.Log("Player In Angle Range");
                RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, (Movement.Singleton.transform.position - transform.position).normalized);
                if (raycastHit.transform.gameObject == Movement.Singleton.gameObject)
                {
                    Debug.Log("Player In Sight");
                    return true;
                }
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + sightDistance * Mathf.Sin((angleRange - transform.eulerAngles.z) *Mathf.Deg2Rad), transform.position.y + sightDistance * Mathf.Cos((angleRange - transform.eulerAngles.z) * Mathf.Deg2Rad)));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + sightDistance * Mathf.Sin(-(angleRange + transform.eulerAngles.z) * Mathf.Deg2Rad), transform.position.y + sightDistance * Mathf.Cos(-(angleRange + transform.eulerAngles.z) * Mathf.Deg2Rad)));
    }
}
