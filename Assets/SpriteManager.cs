using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();
    Color _orgColor;

    // Start is called before the first frame update
    void Start()
    {
        _orgColor = SpriteRenderer.color;
    }

    public void UpdateColor(Color color)
    {
        SpriteRenderer.color = color;
    }

    public void ResetColor()
    {
        SpriteRenderer.color = _orgColor;
    }
}
