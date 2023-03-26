using Unity.Netcode;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    public static Movement Singleton { get; private set; }
    [SerializeField] bool isCrouching;
    [SerializeField] float speed = 5f;
    [SerializeField] float acceleration = 0.1f;
    [SerializeField] float crouchingSpeed = 1.5f;
    [SerializeField] Color crouchColor = new(0, 0, 0);
    Rigidbody2D Rb => GetComponent<Rigidbody2D>();
    SpriteManager SpriteManager => GetComponent<SpriteManager>();
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

    // Update is called once per frame
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
                SpriteManager.UpdateColor(crouchColor);
            }
            else
            {
                SpriteManager.ResetColor();
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