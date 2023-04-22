using UnityEngine;

public class TransformManager : MonoBehaviour
{
    private Vector2 _defaultPos;
    private Vector3 _defaultScale;

    private void Start()
    {
        _defaultPos = transform.position;
        _defaultScale = transform.localScale;
    }

    public void ScaleX(float newScale)
    {
        Transform myTransform = transform;
        if (myTransform != null)
            myTransform.localScale = new Vector3(newScale, myTransform.localScale.y, myTransform.localScale.z);
    }

    public void ScaleY(float newScale)
    {
        Transform myTransform = transform;
        myTransform.localScale = new Vector3(myTransform.localScale.x, newScale, myTransform.localScale.z);
    }

    public void ScaleZ(float newScale)
    {
        Transform myTransform = transform;
        myTransform.localScale = new Vector3(myTransform.localScale.x, myTransform.localScale.y, newScale);
    }

    public void Reset()
    {
        Transform myTransform = transform;
        myTransform.position = _defaultPos;
        myTransform.localScale = _defaultScale;
    }

    public void ResetScale()
    {
        transform.localScale = _defaultScale;
    }
}