using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Quiz/Question")]
public class Question : ScriptableObject
{
    [TextArea(5, 10)]
    public string question;
    public Answer[] answers;
}