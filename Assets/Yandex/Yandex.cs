using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Yandex : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowAdv();

    public AudioSource audioSource;
    public GameObject gameObj;
    

    void Start()
    {
        audioSource = gameObj.GetComponent<AudioSource>();
        ShowAdv();
    }

    private void Update()
    {
        
    }

    public void OpenAdv()
    {
        audioSource.GetComponent<AudioSource>().volume = 0;
        Time.timeScale = 0;
    }

    public void CloseAdv()
    {
        audioSource.GetComponent<AudioSource>().volume = 1;
        Time.timeScale = 1;
    }
}
