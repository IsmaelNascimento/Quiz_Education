using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;

    [SerializeField] private ButtonAnswer btnPrefabAnswer;
    [SerializeField] private Transform contentAnswer;
    [SerializeField] private Text txtQuestionCurrent;
    [SerializeField] private Text txtDownQuestionsCurrent;
    [SerializeField] private Text txtCountQuestions;
    [SerializeField] private Text txtTimerCurrent;
    [SerializeField] private Text txtCountCorrect;
    [SerializeField] private Text txtCountQuestionsFinish;
    [SerializeField] private Text txtTimeFinish;
    [SerializeField] private List<Question> questions = new List<Question>();
    [SerializeField] private GameObject panelDetailsFinish;
    [SerializeField] private AudioSource audioCorrect;
    [SerializeField] private AudioSource audioWrong;
    [SerializeField] private GameObject screenStart;


    private int indexQuestionCurrent = 0;
    private List<Button> lastAnswerCreated = new List<Button>();

    private int countCorrect = 0;
    private int countQuestions;
    private int timerCurrent = 0;
    private int countDownQuestions = 0;




    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        panelDetailsFinish.SetActive(false);
        ButtonAnswer.OnClickedAnswer += OnButtonAnswerClicked;
        countQuestions = questions.Count;
        countDownQuestions = countQuestions;
        LoadQuestion();
        
        txtDownQuestionsCurrent.text = countQuestions.ToString();
        txtTimerCurrent.text = 0.ToString();

    }

    public void LoadQuestion()
    {
        if(indexQuestionCurrent == questions.Count)
        {
            LoadResults();
            return;
        }

        foreach (var answer in lastAnswerCreated)
            Destroy(answer.gameObject);

        lastAnswerCreated.Clear();

        txtQuestionCurrent.text = questions[indexQuestionCurrent].question;

        foreach (var answer in questions[indexQuestionCurrent].answers)
        {
            var btnAnswer = Instantiate(btnPrefabAnswer, contentAnswer);

            btnAnswer.ChangeTextAnswer(answer.answer);
            btnAnswer.ChangeCorret(answer.correct);
            btnAnswer.ChangeColorButton(answer.colorBackground);

            lastAnswerCreated.Add(btnAnswer.GetComponent<Button>());
        }
    }

    private void OnButtonAnswerClicked(bool isAnswerCorrect)
    {
        countDownQuestions--;
        txtDownQuestionsCurrent.text = countDownQuestions.ToString();

        if (isAnswerCorrect)
            countCorrect++;

        indexQuestionCurrent++;
        StartCoroutine(LoadQuestion_Coroutine(isAnswerCorrect));
    }

    private void Timer()
    {
        timerCurrent++;
        txtTimerCurrent.text = timerCurrent.ToString();
    }

    private IEnumerator LoadQuestion_Coroutine(bool lastAnswerCorrect)
    {
        foreach (var button in lastAnswerCreated)
            button.enabled = false;

        if (lastAnswerCorrect)
        {
            audioCorrect.Play();
            yield return new WaitForSeconds(audioCorrect.clip.length);
        }
        else
        {
            audioWrong.Play();
            yield return new WaitForSeconds(audioWrong.clip.length);
        }

        LoadQuestion();        
    }

    private void LoadResults()
    {
        CancelInvoke("Timer");
        txtCountCorrect.text = "Acertou:\n" + countCorrect.ToString();
        txtCountQuestionsFinish.text = "Perguntas:\n" + countQuestions.ToString();
        txtTimeFinish.text = "Tempo:\n" + timerCurrent.ToString();
        panelDetailsFinish.SetActive(true);
    }

    public void LoadScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    public void OnButtonStartClicked()
    {
        screenStart.SetActive(false);
        InvokeRepeating("Timer", 1f, 1f);
    }
}