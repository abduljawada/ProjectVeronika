using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();
    Color _orgColor;
    [SerializeField] private Color altColor;

    // Start is called before the first frame update
    void Start()
    {
        _orgColor = SpriteRenderer.color;
    }

    public void ChangeToAltColor()
    {
        SpriteRenderer.color = altColor;
    }

    public void Reset()
    {
        SpriteRenderer.color = _orgColor;
    }
}