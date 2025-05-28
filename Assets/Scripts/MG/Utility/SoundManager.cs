using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum SoundType
{
    Buy,
    Sell,
    Bargain,
    Reject,
    UI_Button,
    HorseUIChange,
}

public enum BGMtype
{
    Main,
    Meat,
    Mine,
    Wheat,
    Harbor,
    Cloth,
    MeatRoad,
    MineRoad,
    WheatRoad,
    HarborRoad,
    ClothRoad
}

public class SoundManager : Singleton<SoundManager>
{
    [Header("BGM")]
    public AudioClip[] bgmClips;
    AudioSource bgmPlayer;

    [Header("SFX")]
    public AudioClip[] sfxClips;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }

    private void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        BGMplay(true, BGMtype.Main);

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for(int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
        }
    }

    public void SetBgmVolumeSlider(Slider slider)
    {
        slider.minValue = 0f;
        slider.maxValue = 1f;

        slider.value = bgmPlayer.volume;

        slider.onValueChanged.AddListener(delegate { OnbgmVolumeSliderChanged(slider); });
    }

    public void SetSfxVolumeSlider(Slider slider)
    {
        slider.minValue = 0f;
        slider.maxValue = 1f;

        slider.value = sfxPlayers.Length > 0 ? sfxPlayers[0].volume : 1f;

        slider.onValueChanged.AddListener(delegate { OnsfxVolumeSliderChanged(slider); });
    }

    public void SFXplay(SoundType sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
            {
                continue;
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    public void BGMplay(bool isPlay, BGMtype bgm)
    {
        bgmPlayer.clip = bgmClips[(int)bgm];

        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    private void OnbgmVolumeSliderChanged(Slider slider)
    {
        bgmPlayer.volume = slider.value;
    }

    private void OnsfxVolumeSliderChanged(Slider slider)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index].volume = slider.value;
        }
    }
}
