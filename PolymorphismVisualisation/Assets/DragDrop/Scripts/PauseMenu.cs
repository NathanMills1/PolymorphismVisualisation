using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle soundToggle;
    public TextMeshProUGUI completedValue;

    public void Update()
    {

        completedValue.text = GameManager.getSectionProgress();
        volumeSlider.value = GameManager.volumeLevel;
        soundToggle.isOn = !GameManager.muted;
    }

    public void goToMenu()
    {
        ActivityManager.togglePause();
        ActivityManager.paused = false;
        SceneManager.LoadScene(0);
    }

    public void toggleMute()
    {
        GameManager.muted = !soundToggle.isOn;
    }

    public void adjustVolume()
    {
        GameManager.updateVolume(volumeSlider.value);
    }


}
