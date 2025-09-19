using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject item;
    private Tweener tweener;

    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
    }

    // Update is called once per frame
    void Update()
    {
            tweener.AddTween(item.transform, item.transform.position, new Vector2(-7.5f, 12.5f), 1.5f);
            tweener.AddTween(item.transform, item.transform.position, new Vector2(-7.5f, 7.5f), 1.5f);
            tweener.AddTween(item.transform, item.transform.position, new Vector2(-12.5f, 7.5f), 1.5f);
            tweener.AddTween(item.transform, item.transform.position, new Vector2(-12.5f, 12.5f), 1.5f);
    }
}
