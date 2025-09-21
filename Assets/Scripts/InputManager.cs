using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject item;
    [SerializeField] private AudioSource walkAudio;
    private Animator playerAnimator;

    private Tweener tweener;
    private List<Vector2> lst = new List<Vector2>{
        new Vector2(-12.5f, 12.5f),
        new Vector2(-7.5f, 12.5f),
        new Vector2(-7.5f, 8.5f),
        new Vector2(-12.5f, 8.5f)
        };

    private int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
        walkAudio = GetComponent<AudioSource>();
        playerAnimator = item.GetComponent<Animator>();

        StartNextTween();
    }

    // Update is called once per frame
    void Update()
    {
        if (!tweener.isTweening())
        {
            StartNextTween();
        }

    }

    void StartNextTween()
    {
        Debug.Log(i);
        if (i == 1) { playerAnimator.SetTrigger("TrRight"); }
        else if (i == 2) { playerAnimator.SetTrigger("TrDown"); }
        else if (i == 3) { playerAnimator.SetTrigger("TrLeft"); }
        else if (i > 3) { playerAnimator.SetTrigger("TrUp"); i = 0; }

        Vector2 nextPos = lst[i];
        tweener.AddTween(item.transform, item.transform.position, nextPos, 1.2f);
        i++;
    }
}
