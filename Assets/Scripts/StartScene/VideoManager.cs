using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public bool isDelayCheck;

    public static VideoManager instance;

    public VideoPlayer videoPlayer;
    public GameObject videoCanvas;
    public Text filckText;

    public float touchDelay;

    bool isPlaying;
    public bool IsPlaying
    {
        get { return isPlaying; }
        set 
        {
            isPlaying = value;

            if (value) 
            { 

            }
        }
    }
    private void Awake() 
    {
        if (instance == null) instance = this;


        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        isPlaying = videoCanvas.activeSelf;
    }

    public void VideoPlay(VideoClip clip, float timeScale = 1)
    {
        if (timeScale.Equals(0)) Time.timeScale = 0;

        videoCanvas.SetActive(true);
        videoPlayer.clip = clip;
        videoPlayer.Play();

        StartCoroutine(TouchDelay());
    }

    IEnumerator TouchDelay()
    {
        yield return new WaitForSecondsRealtime(touchDelay);
        isDelayCheck = true;
    }

    public void VideoStop()
    {
        if(isDelayCheck)
        {
            Time.timeScale = 1;

            GameManager.instance.ClickVib();
            SoundManager.instance.PlaySfx("Pop");

            videoCanvas.SetActive(false);

            videoPlayer.Stop();
            videoPlayer.clip = null;

            isDelayCheck = false;
        }
    }

}
