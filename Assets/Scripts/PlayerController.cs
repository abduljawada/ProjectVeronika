using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : NetworkBehaviour
{
    public static PlayerController Singleton { get; private set; }

    [SerializeField]
    private bool isCrouching;

    [SerializeField]
    private float lineDisplayTime = 0.5f;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float acceleration = 0.1f;

    [SerializeField]
    private float crouchingSpeed = 1.5f;

    [SerializeField]
    private float shootingDistance = 7f;

    [SerializeField]
    private float fireRate = 0.5f;

    [SerializeField]
    private LayerMask raycastLayer;

    private float _nextFireTime;

    [SerializeField]
    private UnityEvent onCrouch;

    [SerializeField]
    private UnityEvent onStopCrouch;

    private Rigidbody2D Rb => GetComponent<Rigidbody2D>();

    private LineRenderer LineRenderer => GetComponent<LineRenderer>();
    
    private Vector2 _velocity;
    
    private Camera _camera;

    private void Awake()
    {
        if (!Singleton)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (!IsOwner) return;
        
        if (!_camera) _camera = Camera.main;
        
        Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputVector.Normalize();

        /*if (Input.GetKeyDown(KeyCode.X))
        {
            isCrouching = !isCrouching;
            
            if (isCrouching)
            {
                onCrouch?.Invoke();
            }
            else
            {
                onStopCrouch?.Invoke();
            }
        }*/

        if (isCrouching)
        {
            _velocity = inputVector * crouchingSpeed;
        }
        else
        {
            _velocity = inputVector * speed;
        }

        if (Input.GetMouseButton(0) && Time.time > _nextFireTime)
        {
            Debug.Log("Shooting!");

            _nextFireTime = Time.time + fireRate;

            if (_camera)
            {
                Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

                Vector2 pos = transform.position;
                Vector2 direction = mousePosition - pos;

                RaycastHit2D hit = Physics2D.Raycast(pos, direction, shootingDistance, raycastLayer);
                
                AnimateShootingLineClientRPC(direction);
                
                Debug.Log("Hitting " + hit.collider.name);
                
                if (hit.collider.tag.Equals("Enemy")) hit.collider.GetComponent<Health>().LoseHealth();
            }
        }
    }

    private void FixedUpdate()
    {
        Rb.velocity = isCrouching ? _velocity : Vector2.Lerp(Rb.velocity, _velocity, acceleration);
    }

    [ClientRpc]
    private void AnimateShootingLineClientRPC(Vector2 dir)
    {
        StartCoroutine(AnimateShootingLine(dir));
    }
    
    private IEnumerator AnimateShootingLine(Vector2 dir)
    {
        var position = transform.position;
        LineRenderer.SetPositions(new []{position, position + (Vector3) dir.normalized * shootingDistance});
        LineRenderer.enabled = true;
        yield return new WaitForSeconds(lineDisplayTime);
        LineRenderer.enabled = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = transform.position;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(pos, mousePos);
    }
}