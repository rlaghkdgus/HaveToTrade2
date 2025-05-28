using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEditor.UI;

public class Resolution : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;

    public Toggle fullScreenToggle;

    UnityEngine.Resolution[] AllResolutions;
    private bool isFullScreen;
    int SelectedResolution;

    List<UnityEngine.Resolution> SelectedResolutionList = new List<UnityEngine.Resolution>();

    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Awake()
    {
        isFullScreen = true;
        AllResolutions = Screen.resolutions;

        List<string> resolutionStringList = new List<string>();
        string newRes;
        foreach(UnityEngine.Resolution res in AllResolutions)
        {
            float aspectRatio = (float)res.width / res.height;
            if(Mathf.Abs(aspectRatio - 16f / 9f) < 0.01f)
            {
                newRes = res.width.ToString() + " x " + res.height.ToString();
                if (!resolutionStringList.Contains(newRes))
                {
                    resolutionStringList.Add(newRes);
                    SelectedResolutionList.Add(res);
                }
            }
        }

        resolutionDropdown.AddOptions(resolutionStringList);

        UnityEngine.Resolution currentResolution = Screen.currentResolution;

        Screen.SetResolution(currentResolution.width, currentResolution.height, true);

        for(int i = 0; i < resolutionDropdown.options.Count; ++i)
        {
            if (resolutionDropdown.options[i].text == currentResolution.width.ToString() + " x " + currentResolution.height.ToString())
            {
                resolutionDropdown.value = i;
                resolutionDropdown.RefreshShownValue();
                return;
            }
        }
    }

    private void OnEnable()
    {
        SoundManager.Instance.SetBgmVolumeSlider(bgmSlider);
        SoundManager.Instance.SetSfxVolumeSlider(sfxSlider);
    }

    public void ChangeResolution()
    {
        SelectedResolution = resolutionDropdown.value;
        Screen.SetResolution(SelectedResolutionList[SelectedResolution].width, SelectedResolutionList[SelectedResolution].height, isFullScreen);
    }

    public void ChangeFullScreen()
    {
        isFullScreen = fullScreenToggle.isOn;
        Screen.SetResolution(SelectedResolutionList[SelectedResolution].width, SelectedResolutionList[SelectedResolution].height, isFullScreen);
    }
}
