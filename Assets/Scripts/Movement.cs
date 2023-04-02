using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Movement : NetworkBehaviour
{
    public static Movement Singleton { get; private set; }
    [SerializeField] bool isCrouching;
    [SerializeField] float speed = 5f;
    [SerializeField] float acceleration = 0.1f;
    [SerializeField] float crouchingSpeed = 1.5f;
    [SerializeField] private UnityEvent onCrouch;
    [SerializeField] private UnityEvent onStopCrouch;
    Rigidbody2D Rb => GetComponent<Rigidbody2D>();
    Vector2 _velocity;

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
    
    void Update()
    {
        if (!IsOwner) return;
        
        Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputVector.Normalize();

        if (Input.GetKeyDown(KeyCode.X))
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
        }

        if (isCrouching)
        {
            _velocity = inputVector * crouchingSpeed;
        }
        else
        {
            _velocity = inputVector * speed;
        }
    }

    private void FixedUpdate()
    {
        Rb.velocity = isCrouching ? _velocity : Vector2.Lerp(Rb.velocity, _velocity, acceleration);
    }
}