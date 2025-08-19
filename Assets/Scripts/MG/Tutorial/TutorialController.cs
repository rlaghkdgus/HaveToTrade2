using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private List<TutorialBase> tutorials;

    public TutorialHighlight highlighter;
    [SerializeField] private TutorialBase currentTutorial = null;
    private int currentIndex = -1;

    public UnityEvent onTutorialFinish;
    public FadeLoad loadManager;

    private void Start()
    {
        SetNextTutorial();
    }

    private void Update()
    {
        if(currentTutorial != null)
        {
            currentTutorial.Execute(this);
        }
    }

    public void SetNextTutorial()
    {
        if(currentTutorial != null)
        {
            currentTutorial.Exit(this);
        }

        if(currentIndex >= tutorials.Count - 1)
        {
            CompletedAllTutorials();
            return;
        }

        currentIndex++;
        currentTutorial = tutorials[currentIndex];

        currentTutorial.Enter(this);
    }

    public void CompletedAllTutorials()
    {
        Debug.Log("튜토리얼 종료");
        highlighter?.Hide();
        currentTutorial = null;
        loadManager.StartFadeLoad();
    }
}
