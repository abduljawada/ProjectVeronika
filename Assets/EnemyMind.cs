using Pathfinding;
using UnityEngine;

public class EnemyMind : MonoBehaviour
{
    public States MyState { get; private set; } = States.Idle;
    AIDestinationSetter DestinationSetter => GetComponent<AIDestinationSetter>();
    AIPatrol Patrol => GetComponent<AIPatrol>();
    AIPath Path => GetComponent<AIPath>();
    AIDestinationManager DestinationManager => GetComponent<AIDestinationManager>();
    EnemyEye Eye => GetComponent<EnemyEye>();

    // Start is called before the first frame update
    void Start()
    {
        Vector2 nextPoint = Patrol.GetNextPoint();
        if (nextPoint != null)
        {
            DestinationManager.AssignPathPoint(nextPoint);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MyState == States.Idle)
        {
            if (DestinationSetter.target != null)
            {
                if (transform.position == DestinationSetter.target.position)
                {
                    if (DestinationSetter.target.CompareTag("PathPoint"))
                    {
                        Destroy(DestinationSetter.target.gameObject);
                    }

                    DestinationManager.AssignPathPoint(Patrol.GetNextPoint());
                }
            }
        }
    }

    public void FoundPlayer()
    {
        MyState = States.Attack;
        DestinationSetter.target = Movement.Singleton.transform;
        Path.whenCloseToDestination = CloseToDestinationMode.Stop;
        
    }

    public void LostPlayer()
    {
        MyState = States.Searching;
        Path.whenCloseToDestination = CloseToDestinationMode.ContinueToExactDestination;
        DestinationManager.AssignPathPoint(Eye.LastSeenPos);
    }

    //void LookAtPlayer()
    //{
    //    float distX = gameManager.player.transform.position.x - transform.position.x;
    //    float distY = gameManager.player.transform.position.y - transform.position.y;
    //    transform.eulerAngles = new Vector3(0, 0, (Mathf.Rad2Deg * Mathf.Atan2(distY, distX)) - 90f);
    //}
}

public enum States
{
    Idle,
    Searching,
    Attack
}
