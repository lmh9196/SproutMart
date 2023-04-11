using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] AudioMixer masterMixer;
    [SerializeField] Slider audioSlider;
    [SerializeField] Slider BGMAudioSlider;


    [SerializeField] Transform audioSFXParent;
    [SerializeField] Transform audioBGMParent;
    AudioSource[] SFXAudioSorceArray;
    [HideInInspector] public AudioSource[] BGMAudioSorceArray;
    private void Awake()
    {
        if (null == instance) instance = this;

        SFXAudioSorceArray = audioSFXParent.GetComponentsInChildren<AudioSource>();
        BGMAudioSorceArray = audioBGMParent.GetComponentsInChildren<AudioSource>();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        AudioControl();
    }

    private void Update()
    {
        if (GameManager.instance.CheckTutorial() && !BGMAudioSorceArray[MenuManager.instance.setting.BGMSelectNum].isPlaying) 
        { PlayBGM(MenuManager.instance.setting.BGMSelectNum); }
    }

    public void AudioControl()
    {
        float SFXsound = audioSlider.value;
        float BGMsound = BGMAudioSlider.value + 3f;

        if (SFXsound == -40f) masterMixer.SetFloat("SFX", -80);
        else masterMixer.SetFloat("SFX", SFXsound);

        if (BGMsound == -37f) masterMixer.SetFloat("BGM", -80);
        else masterMixer.SetFloat("BGM", BGMsound);
    }

    public void ToggleAudioVolume()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }

    public void PlaySfx(string soundObjName)
    {
        //SoundManager.instance.PlaySfx("");

        for (int i = 0; i < SFXAudioSorceArray.Length; i++)
        {
            if (soundObjName.Equals(SFXAudioSorceArray[i].gameObject.name)) { SFXAudioSorceArray[i].Play(); return; }

            if(i == SFXAudioSorceArray.Length -1)
            { Debug.Log("Error : "+ soundObjName +" is Null"); }
        }
    }
    public void PlayBGM(int bgmNum)
    {
        for (int i = 0; i < BGMAudioSorceArray.Length; i++)
        {
            if (i.Equals(bgmNum)) { BGMAudioSorceArray[bgmNum].Play(); }
            else { BGMAudioSorceArray[i].Stop(); }
        }
    }
}
