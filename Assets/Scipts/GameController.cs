using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Element
{
    Rock = 1,
    Paper = 2,
    Scissors = 3,
    Lizard = 4,
    Spock = 5
}

public static class StringConstants
{
    public const string won = "You Won!";
    public const string loose = "You Lost!";
    public const string highscore = "Highscore : ";
    public const string score = "Score : ";
    public const string playerPrefHighscore = "Highscore";
}

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("MainMenu")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameplay;
    [SerializeField] private Button playButton;
    [SerializeField] private Text highscoreMenu;

    [Header("Gameplay")]
    [SerializeField] private Text botMoveText;
    [SerializeField] private Slider timeSlider;
    [SerializeField] private Text timeLeftText;
    [SerializeField] private PlayerButton[] buttons;

    [Header("Popup")]
    [SerializeField] private GameObject popup;
    [SerializeField] private Text title;
    [SerializeField] private Button button;
    [SerializeField] private Text buttonText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highscoreText;

    private int score;
    private int highScore = 0;

    private Element botElement;
    private float timeLeft = 2;

    private bool isGameComplete;

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(StringConstants.playerPrefHighscore))
            highScore = PlayerPrefs.GetInt(StringConstants.playerPrefHighscore);

        highscoreMenu.text = StringConstants.highscore + highScore.ToString();

        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(PlayGame);
    }

    private void Update()
    {
        if (timeLeft > 0 && !isGameComplete)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimer(timeLeft);
        }
        if(timeLeft<=0)
        {
            isGameComplete = true;
        }
    }

    private void StartGame()
    {
        StartCoroutine(StartGameCo());
    }

    IEnumerator StartGameCo()
    {
        timeLeft = 2;
        isGameComplete = true;

        var time = 3;
        SetButtonInteractablity(false);
        while(time>0)
        {
            botMoveText.text = time.ToString();
            yield return new WaitForSeconds(1);
            time--;
        }
        isGameComplete = false;
        PlayBotTurn();
        SetButtonInteractablity(true);
    }

    private void PlayBotTurn()
    {
        int random = Random.Range(1, 6);
        botElement = (Element)random;
        botMoveText.text = botElement.ToString();
    }

    public void PlayUserTurn(ElementsScriptable element)
    {
        isGameComplete = true;
        var won = element.CanWinFrom(botElement);
        ShowPopup(won);
    }

    private void SetButtonInteractablity(bool state)
    {
        foreach (var item in buttons)
        {
            item.SetButtonInteractablity(state);
        }
    }

    private void ShowPopup(bool won)
    {
        button.onClick.RemoveAllListeners();
        popup.SetActive(true);

        if(won)
        {
            title.text = StringConstants.won;
            buttonText.text = "Next";
            button.onClick.AddListener(PlayGame);
            score++;
        }
        else
        {
            title.text = StringConstants.loose;
            buttonText.text = "Back";
            button.onClick.AddListener(BackToMenu);
        }

        if(score>highScore)
        {
            PlayerPrefs.SetInt(StringConstants.playerPrefHighscore, score);
            highScore = score;
        }    

        scoreText.text = StringConstants.score + score.ToString();
        highscoreText.text = StringConstants.highscore + highScore.ToString();
    }

    private void PlayGame()
    {
        popup.SetActive(false);
        mainMenu.SetActive(false);
        gameplay.SetActive(true);
        UpdateTimer(2);
        StartGame();
    }

    private void BackToMenu()
    {
        popup.SetActive(false);
        mainMenu.SetActive(true);
        gameplay.SetActive(false);
        highscoreMenu.text = StringConstants.highscore + highScore.ToString();
    }

    private void UpdateTimer(float time)
    {
        timeSlider.value = time;
        timeLeftText.text = "Time left : " + time.ToString("0.0") + "s";
    }

}
