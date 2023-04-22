using UnityEngine;

public class AIPatrol : MonoBehaviour
{
    [SerializeField] Vector2[] patrolPoints;
    int _currentPointIndex = -1;

    public Vector2 GetNextPoint()
    {
        _currentPointIndex++;

        if (_currentPointIndex >= patrolPoints.Length)
        {
            _currentPointIndex = 0;
        }

        return patrolPoints[_currentPointIndex];
    }
}