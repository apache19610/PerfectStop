using UnityEngine;

public class ChooseCarMainScene : MonoBehaviour {

    public GameObject[] cars;
    public AnimationClip mainCar;

    private void Start() {
        if (!PlayerPrefs.HasKey("NowCar")) {
            CreateCar(cars[0]);
            return;
        }

        foreach (GameObject car in cars) {
            if (car.name == PlayerPrefs.GetString("NowCar")) {
                CreateCar(car);
                break;
            }
        }
    }

    private void CreateCar(GameObject car) {
        GameObject newCar = Instantiate(car, new Vector3(-3.36f, -0.18f, -14.01f), Quaternion.Euler(0, -90, 0));
        Animation animCar = newCar.AddComponent<Animation>();
        animCar.AddClip(mainCar, "MainCar");
        animCar.clip = mainCar;
        animCar.playAutomatically = true;
        animCar.Play();
    }
}
