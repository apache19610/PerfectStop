using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class AddStartAdvertisement : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowAdv();

    private void Start()
    {
        ShowAdv();
    }

    public void OpenAdv()
    {
        Time.timeScale = 0;
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in audios)
        {
            a.Pause();
        }
    }

    public void CloseAdv()
    {
        Time.timeScale = 1;
        if (PlayerPrefs.GetString("music") == "No")
        {
            AudioSource[] audios = FindObjectsOfType<AudioSource>();
            foreach (AudioSource a in audios)
            {
                a.Pause();
            }
        }
        else
        {
            AudioSource[] audios = FindObjectsOfType<AudioSource>();
            foreach (AudioSource a in audios)
            {
                a.Play();
            }
        }
    }
}
