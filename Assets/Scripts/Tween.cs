using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween
{
    public Transform Target;
    public Vector2 StartPos;
    public Vector2 EndPos;
    public float StartTime;
    public float Duration;
    public float Elapsed;

    public Tween(Transform target, Vector2 startPos, Vector2 endPos, float startTime, float duration)
    {
        Target = target;
        StartPos = startPos;
        EndPos = endPos;
        StartTime = startTime;
        Duration = duration;
        Elapsed = 0.0f;
    }

}
