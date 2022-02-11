using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    [SerializeField]
    private AudioSource clickTile;
    [SerializeField]
    private AudioSource clickButton;
    [SerializeField]
    private AudioSource backgroundAudio;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        backgroundAudio.loop = true;
        backgroundAudio.Play();
    }

    public void ClickTile()
    {
        clickTile.Play();
    }

    public void ClickButton()
    {
        clickButton.Play();
    }
}
