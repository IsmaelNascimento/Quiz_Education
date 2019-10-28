using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnswer : MonoBehaviour
{
    [SerializeField] private Image imgButton;
    [SerializeField] private Text txtAnswers;
    private bool isCorrect = false;

    public static Action<bool> OnClickedAnswer;


    public void ChangeColorButton(Color color)
    {
        imgButton.color = color;
    }

    public void ChangeTextAnswer(string answer)
    {
        txtAnswers.text = answer;
    }

    public void ChangeCorret(bool correct)
    {
        isCorrect = correct;
    }

    public void OnButtonAnswerClicked()
    {
        if (OnClickedAnswer != null)
            OnClickedAnswer(isCorrect);
    }
}