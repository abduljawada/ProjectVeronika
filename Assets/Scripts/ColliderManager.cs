using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    private Vector2 _defaultSize;
    private CapsuleCollider2D CapsuleCollider2D => GetComponent<CapsuleCollider2D>();
    
    // Start is called before the first frame update
    void Start()
    {
        _defaultSize = CapsuleCollider2D.size;
    }
    
    public void ResizeX(float newSize)
    {
        CapsuleCollider2D.size = new Vector2(newSize, CapsuleCollider2D.size.y);
    }
    
    public void ResizeY(float newSize)
    {
        CapsuleCollider2D.size = new Vector2(CapsuleCollider2D.size.x, newSize);
    }

    public void Reset()
    {
        CapsuleCollider2D.size = _defaultSize;
    }
}
