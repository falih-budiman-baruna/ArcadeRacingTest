using UnityEngine;
using TMPro;
using System.Collections;
using System.Threading;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI carPositionText;
    public CarController car;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI GameOverText;

    void Update()
    {
        carPositionText.text = car.racePosition.ToString() + " / " + GameManager.instance.cars.Count.ToString();
    }

    public void StartCountDownDisplay ()
    {
        StartCoroutine(Countdown());

        IEnumerator Countdown ()
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = "3";
            yield return new WaitForSeconds(1.0f);
            countdownText.text = "2";
            yield return new WaitForSeconds(1.0f);
            countdownText.text = "1";
            yield return new WaitForSeconds(1.0f);
            countdownText.text = "GO";
            yield return new WaitForSeconds(1.0f);
            countdownText.gameObject.SetActive(false);
        }
    }

    public void GameOver (bool winner)
    {
        GameOverText.gameObject.SetActive(true);
        GameOverText.color = winner == true ? Color.green : Color.red;
        GameOverText.text = winner == true ? "You Win!" : "You Lost ;c";
    }
}
