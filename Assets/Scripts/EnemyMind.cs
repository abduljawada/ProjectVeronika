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
    private float _angleSpun;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 nextPoint = Patrol.GetNextPoint();
        DestinationManager.AssignPathPoint(nextPoint);
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
        else if (MyState == States.Searching)
        {
            if (transform.position == DestinationSetter.target.position)
            {
                float amountToSpin = 360f * Time.deltaTime;
                _angleSpun += amountToSpin;
                transform.Rotate(new Vector3(0f, 0f, amountToSpin));

                if (_angleSpun >= 360f)
                {
                    _angleSpun = 0f;
                    Destroy(DestinationSetter.target.gameObject);
                    MyState = States.Idle;
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
        LookAtPlayer();
    }

    public void LostPlayer()
    {
        MyState = States.Searching;
        Path.whenCloseToDestination = CloseToDestinationMode.ContinueToExactDestination;
        DestinationManager.AssignPathPoint(Eye.LastSeenPos);
    }

    public void LookAtPlayer()
    {
        Vector2 direction = Eye.LastSeenPos - (Vector2) transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
}

public enum States
{
    Idle,
    Searching,
    Attack
}