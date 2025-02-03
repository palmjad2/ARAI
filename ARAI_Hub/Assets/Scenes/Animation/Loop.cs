using UnityEngine;
using System.Collections;

public class LegacyAnimationLooper : MonoBehaviour
{
    public Animation anim;
    public string clipName = "MyLegacyClip";
    public float holdTime = 2f;

    void Start()
    {
        StartCoroutine(PlayAndPause());
    }

    IEnumerator PlayAndPause()
    {
        while (true)
        {
            // Play the clip
            anim.Play(clipName);

            // Wait for the clip to finish
            yield return new WaitForSeconds(anim[clipName].length);

            // Stay on the final pose for 2 seconds
            yield return new WaitForSeconds(holdTime);

            // Reset the clip to frame 0
            anim[clipName].time = 0f;
            anim.Sample();

            // Then loop
        }
    }
}
