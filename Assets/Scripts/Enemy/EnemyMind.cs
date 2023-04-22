using System.Collections;
using Default;
using Pathfinding;
using UnityEngine;

namespace Enemy
{
    public class EnemyMind : MonoBehaviour
    {
        public States MyState { get; private set; } = States.Idle;

        [SerializeField] private float rotationSpeed = 0.5f;

        [SerializeField] private float fireRate = 0.5f;

        [SerializeField] private float lineDisplayTime = 0.5f;

        private float _nextFireTime;

        private AIDestinationSetter DestinationSetter => GetComponent<AIDestinationSetter>();

        private AIPatrol Patrol => GetComponent<AIPatrol>();

        private AIPath Path => GetComponent<AIPath>();

        private AIDestinationManager DestinationManager => GetComponent<AIDestinationManager>();

        private EnemyEye Eye => GetComponent<EnemyEye>();

        private LineRenderer LineRenderer => GetComponent<LineRenderer>();

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

            if (MyState == States.Attack && Time.time > _nextFireTime)
            {
                if (Eye.IsPlayerInSight())
                {
                    StartCoroutine(AnimateShootingLine());

                    _nextFireTime = Time.time + fireRate;

                    Transform myTransform = transform;

                    Vector2 pos = myTransform.position;

                    Vector2 dir = myTransform.up;

                    RaycastHit2D hit = Physics2D.Raycast(pos, dir, Eye.sightDistance, Eye.raycastLayer);

                    if (!hit.collider) return;

                    Debug.Log(name + " is Hitting " + hit.collider.name);

                    if (hit.collider.tag.Equals("Player")) hit.collider.GetComponent<Health>().LoseHealth();
                }
            }
        }

        public void FoundPlayer()
        {
            MyState = States.Attack;
            DestinationSetter.target = PlayerController.Singleton.transform;
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
            Vector2 direction = Eye.LastSeenPos - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion nextRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, nextRotation, rotationSpeed * Time.deltaTime);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(transform.position, transform.position + transform.up * Eye.sightDistance);
        }

        private IEnumerator AnimateShootingLine()
        {
            var myTransform = transform;
            var position = myTransform.position;
            var up = myTransform.up;
            LineRenderer.SetPositions(new[] { position + up * 0.5f, position + up * Eye.sightDistance });
            LineRenderer.enabled = true;
            yield return new WaitForSeconds(lineDisplayTime);
            LineRenderer.enabled = false;
        }
    }

    public enum States
    {
        Idle,
        Searching,
        Attack
    }
}