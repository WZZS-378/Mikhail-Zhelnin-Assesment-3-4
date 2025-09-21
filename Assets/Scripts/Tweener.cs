using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    private Tween activeTween;

    public void AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        if(activeTween == null)
        {
            activeTween = new Tween(targetObject, startPos, endPos, Time.time, duration);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activeTween == null) return;

        activeTween.Elapsed += Time.deltaTime;
        float t = activeTween.Elapsed / activeTween.Duration;

        if (t < 1f)
        {
            activeTween.Target.position = Vector3.Lerp(activeTween.StartPos, activeTween.EndPos, t);
        }
        else
        {
            activeTween.Target.position = activeTween.EndPos;
            activeTween = null;
        }
    }

    public bool isTweening()
    {
        return activeTween != null;
    }
}
