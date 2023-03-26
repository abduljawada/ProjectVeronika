using UnityEngine;
using Pathfinding;

public class AIDestinationManager : MonoBehaviour
{
    AIDestinationSetter DestinationSetter => GetComponent<AIDestinationSetter>();

    public void AssignPathPoint(Vector2 pos)
    {
        if (DestinationSetter.target != null)
        {
            if (DestinationSetter.target.CompareTag("PathPoint"))
            {
                Destroy(DestinationSetter.target.gameObject);
            }
        }

        DestinationSetter.target = new GameObject().transform;
        DestinationSetter.target.position = pos;
        DestinationSetter.target.tag = "PathPoint";
    }
}
