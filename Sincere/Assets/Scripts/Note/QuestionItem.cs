using UnityEngine;
using UnityEngine.UI;

public class QuestionItem : MonoBehaviour
{
    [Header("UI ÂüÁ¶")]
    public Text questionName;
    public Text questionDescription;
    public Text questionMap;
    public Text questionClue;

    public void Setup(QuestionData question)
    {
        if (questionName != null)
        {
            questionName.text = question.questionName;
        }

        if (questionDescription != null)
        {
            questionDescription.text = question.questionDescription;
        }

        if (questionMap != null)
        {
            questionMap.text = question.questionMap;
        }

        if ( questionClue != null)
        {
            questionClue.text = question.questionClue;
        }
    }
}