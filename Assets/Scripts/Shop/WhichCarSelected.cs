using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class WhichCarSelected : MonoBehaviour {

    [DllImport("__Internal")]
    private static extern void ShowAdv();

    public Image[] checkButtons;
    public Sprite closeImage, checkImage;
    
    private void Start() {
        CheckButtons();
        ShowAdv();
    }

    public void CheckButtons() {
        foreach (Image img in checkButtons) {
            img.sprite = closeImage;
        }

        if (!PlayerPrefs.HasKey("NowCar") || PlayerPrefs.GetString("NowCar") == "PizzaCar") {
            checkButtons[0].sprite = checkImage;
        } else if (PlayerPrefs.GetString("NowCar") == "HotDog") {
            checkButtons[1].sprite = checkImage;
        }
    }
    
}
