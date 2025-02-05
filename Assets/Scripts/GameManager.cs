using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour
{
    GameObject countdownTextObject;
    GameObject startScreen;
    GameObject gameScreen;
    GameObject gameOverScreen;
    TMP_Text gameOverText;
    TMP_Text statusText;
    UnityEngine.UI.Image statusImage;
    GameObject timer;
    Button endGameButton; 

    void Awake()
    {
        countdownTextObject = transform.Find("CountdownText").gameObject;
        startScreen = transform.Find("StartScreen").gameObject;
        gameScreen = transform.Find("GameScreen").gameObject;
        gameOverScreen = transform.Find("GameOverScreen").gameObject;
        gameOverText = gameOverScreen.transform.Find("GameOverText").GetComponent<TMP_Text>();
        timer = gameScreen.transform.Find("Timer").gameObject;
        statusImage = gameScreen.transform.Find("StatusContainer").gameObject.transform.Find("Image").gameObject.GetComponent<UnityEngine.UI.Image>();
        statusText = gameScreen.transform.Find("StatusContainer").gameObject.transform.Find("Status").GetComponent<TMP_Text>();

        
        endGameButton = gameOverScreen.transform.Find("EndGameButton").GetComponent<Button>();
        endGameButton.onClick.AddListener(EndGame);
        endGameButton.gameObject.SetActive(false); 
    }

    void OnEnable()
    {
        EventManager.Instance.Subscribe("GameStart", StartCountdown);
        EventManager.Instance.Subscribe("Timer", UpdateTimer);
        EventManager.Instance.Subscribe("TimeIsUp", TimeIsUp);
        EventManager.Instance.Subscribe("GameWon", GameWon);
        EventManager.Instance.Subscribe("GameLost", GameLost);
        EventManager.Instance.Subscribe("PlayerSpeed", DisplayPlayerSpeed);
        EventManager.Instance.Subscribe("PlayerStatus", DisplayPlayerStatus);
    }

    void StartCountdown(object message)
    {
        countdownTextObject.SetActive(true);
        startScreen.SetActive(false);
        StartCoroutine(CountdownRoutine());
    }

    void DisplayPlayerSpeed(object message) => statusImage.transform.localScale = new Vector3((float)message, 1, 1);

    void DisplayPlayerStatus(object message)
    {
        statusImage.color = (string)message switch
        {
            "Running" => Color.red,
            "Stopping" => Color.yellow,
            _ => Color.green,
        };
        statusText.text = (string)message switch
        {
            "Stopping" => "<color=#FFF700><b>STOP</b></color>",
            "Running" => "<color=#FF0000><b>Biegniesz</b></color>",
            _ => "<color=#00FF2B><b>Zatrzymany</b></color>",
        };
    }

    void UpdateTimer(object message)
    {
        if (timer.activeSelf == false)
        {
            timer.SetActive(true);
        }
        if (message is string m)
        {
            TMP_Text timerText = timer.GetComponent<TMP_Text>();
            timerText.text = "Pozostały czas: " + "<color=#FF0000>" + m + "</color>";
        }
    }

    void TimeIsUp(object message)
    {
        TMP_Text timerText = timer.GetComponent<TMP_Text>();
        timerText.text = "<color=#FF0000>Czas minął!</color>";
    }

    void GameWon(object message)
    {
        gameOverScreen.SetActive(true);
        gameOverText.text = "Wygrałeś!";
        endGameButton.gameObject.SetActive(true); 
    }

    void GameLost(object message)
    {
        gameOverScreen.SetActive(true);
        gameOverText.text = "Przegrałeś!";
    }

    IEnumerator CountdownRoutine()
    {
        TMP_Text countdownText = countdownTextObject.GetComponent<TMP_Text>();
        
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString(); 
            yield return new WaitForSeconds(1); 
        }

        countdownText.text = "idz"; 
        EventManager.Instance.TriggerEvent("CountdownFinished", true);
        gameScreen.SetActive(true);
        yield return new WaitForSeconds(1); 
        countdownTextObject.SetActive(false); 
    }

    public void StartGame()
    {
        EventManager.Instance.TriggerEvent("GameStart", null);
    }

    public void RestartScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    
    public void EndGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}