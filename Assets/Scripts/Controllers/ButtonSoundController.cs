using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public AudioClip clip;
    public AudioSource source;
    public void Click()
    {
        source.PlayOneShot(clip);
    }
}
