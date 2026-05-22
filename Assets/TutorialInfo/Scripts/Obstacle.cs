using UnityEngine;

public enum ObstacleType
{
    Ground,
    Air
}

public class Obstacle : MonoBehaviour
{
    public ObstacleType obstacleType;
}