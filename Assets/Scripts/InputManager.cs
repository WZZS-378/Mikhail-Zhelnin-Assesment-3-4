using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
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
        walkAudio = player.GetComponent<AudioSource>();
        playerAnimator = player.GetComponent<Animator>();

     
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
        if (i == 1) { playerAnimator.SetTrigger("TrRight"); }
        else if (i == 2) { playerAnimator.SetTrigger("TrDown"); }
        else if (i == 3) { playerAnimator.SetTrigger("TrLeft"); }
        else if (i > 3) { playerAnimator.SetTrigger("TrUp"); i = 0; }

        Vector2 nextPos = lst[i];
        tweener.AddTween(player.transform, player.transform.position, nextPos, 1.2f);

        StartCoroutine(PlayFootsteps(2.3f));

        i++;
    }

    private IEnumerator PlayFootsteps(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            walkAudio.Play();
            yield return new WaitForSeconds(0.47f);
            elapsed += 0.47f;
        }
    }
}
