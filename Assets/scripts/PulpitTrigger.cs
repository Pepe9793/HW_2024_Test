using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PulpitTrigger : MonoBehaviour
{
    private bool walkedOn = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !walkedOn)
        {
            walkedOn = true;
            FindObjectOfType<ScoreManager>().IncrementScore();
        }
    }
}