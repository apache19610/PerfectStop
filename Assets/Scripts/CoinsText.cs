﻿using UnityEngine;
using UnityEngine.UI;

public class CoinsText : MonoBehaviour {
    private void Start() {
        GetComponent<Text>().text = PlayerPrefs.GetInt("Coins").ToString();
    }
}
