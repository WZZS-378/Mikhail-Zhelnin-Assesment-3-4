using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagement : MonoBehaviour
{
    public AudioSource startMusic;
    public AudioSource normalGhostMusic;

    // Start is called before the first frame update
    void Start()
    {
        startMusic.Play();
        StartCoroutine(PlayGhostMusicAfterDelay(3.0f));
    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator PlayGhostMusicAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        startMusic.Stop();
        normalGhostMusic.loop = true;
        normalGhostMusic.Play();
    }

}
