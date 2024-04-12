using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    //Save Keys
    private string Key_MouseSensitivity = "data_MouseSensitivity";
    private string Key_GameSoundVolume = "data_GameSoundVolume";
    private string Key_MusicSoundVolume = "data_MusicSoundVolume";

    public void SaveMouseSensitivity(float value) 
    {
        PlayerPrefs.SetFloat(Key_MouseSensitivity, value);
    }
    public void SaveGameSoundVolume(float value)
    {
        PlayerPrefs.SetFloat(Key_GameSoundVolume, value);
    }
    public void SaveMusicSoundVolume(float value)
    {
        PlayerPrefs.SetFloat(Key_MusicSoundVolume, value);
    }
    public float LoadMouseSensitivity() 
    {
        float value = 0;
        try
        {
            value = PlayerPrefs.GetFloat(Key_MouseSensitivity);
            if (value == 0)
            {
                value = 2;
            }
        }
        catch (System.Exception)
        {

            value = 2f;
        }
        return value;
    }
    public float LoadGameSoundVolume()
    {
        float value = 0;
        try
        {
            value = PlayerPrefs.GetFloat(Key_GameSoundVolume);
            if (value == 0)
            {
                value = 1;
            }
        }
        catch 
        {
            value = 1;
        }
        return value;
    }
    public float LoadMusicSoundVolume()
    {
        float value = 0;
        try
        {
            value = PlayerPrefs.GetFloat(Key_MusicSoundVolume);
            if (value == 0)
            {
                value = 1;
            }
        }
        catch 
        {
            value = 1;
        }

        return value;
    }
}
