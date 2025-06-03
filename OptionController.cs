using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class OptionsMenu : MonoBehaviour
{
    // Dropdown UI reference for resolution options
    public Dropdown resolutionDropdown;
    // Toggle UI reference for fullscreen option
    public Toggle fullscreenToggle;
    // Slider UI reference for volume control
    public Slider volumeSlider;

    private Resolution[] resolutions;

    void Start()
    {
        // Get all the available screen resolutions
        resolutions = Screen.resolutions;
        
        // Check if resolutions array is empty
        if (resolutions.Length == 0)
        {
            Debug.LogWarning("No resolutions found");
            return;
        }

        // Clear existing options
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        // Populate dropdown with resolution options
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Add resolution options to dropdown and set currently selected resolution
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Set fullscreen toggle to current fullscreen state
        fullscreenToggle.isOn = Screen.fullScreen;

        // Set volume slider to current volume
        volumeSlider.value = AudioListener.volume;
    }

    // Called when resolution dropdown value changes
       public void SetResolution(int resolutionIndex)
   {
       if (resolutionIndex < 0 || resolutionIndex >= Screen.resolutions.Length)
           return;

       Resolution resolution = Screen.resolutions[resolutionIndex];
       Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
       Debug.Log("Resolution set to: " + resolution.width + "x" + resolution.height);
   }
   

    // Called when fullscreen toggle value changes
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Debug.Log("Fullscreen set to: " + isFullscreen);
    }

    // Called when volume slider value changes
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        Debug.Log("Volume set to: " + volume);
    }
}
