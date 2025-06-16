using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialClick : TutorialBase
{
    //[SerializeField] private GameObject ClickTrigger;
    [SerializeField] private GameObject blockPanel;

    private Material hole;
    private RaycastBlock RayB;

    [SerializeField] private bool isClick = false;
    private bool isUIClick = false;
    [SerializeField] private bool isUI;
    [SerializeField] private bool isDelay;

    public float xPos;
    public float yPos;
    public float width;
    public float height;

    private void Awake()
    {
        if(blockPanel != null)
        {
            hole = blockPanel.transform.GetChild(0).GetComponent<RawImage>().material;
            RayB = blockPanel.transform.GetChild(1).GetComponent<RaycastBlock>();
        }
        else
        {
            Debug.LogError("blockPanel 없음");
        }
    }

    private void OnEnable()
    {
        EventManager.TownBuildingClick += OnClick;
        EventManager.TownBuildingUIOff += nextTu;
    }

    private void OnDisable()
    {
        EventManager.TownBuildingClick -= OnClick;
        EventManager.TownBuildingUIOff -= nextTu;
    }

    public void OnClick()
    {
        isClick = true;
    }

    public void nextTu()
    {
        isUIClick = true;
    }
    
    public IEnumerator DelaySetActive()
    {
        yield return new WaitForSeconds(1f);
        blockPanel.SetActive(true);
    }

    public override void Enter()
    {
        if (hole != null)
        {
            hole.SetVector("_HoleRect", new Vector4(xPos, yPos, width, height));
        }
        else
        {
            Debug.LogError("hole 없음");
        }

        if(RayB != null)
        {
            RayB.xPos = xPos;
            RayB.yPos = yPos;
            RayB.width = width;
            RayB.height = height;
        }
        else
        {
            Debug.LogError("RayB 없음");
        }

        //ClickTrigger.SetActive(true);
        if (!isDelay)
        {
            blockPanel.SetActive(true);
        }
        else
        {
            StartCoroutine(DelaySetActive());
        }

        isClick = false;
        isUIClick = false;
    }

    public override void Execute(TutorialController controller)
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if(hit.collider != null)
            {
                if(hit.collider.gameObject == ClickTrigger)
                {
                    controller.SetNextTutorial();
                }
            }
        }*/

        if (isClick && !isUI)
        {
            controller.SetNextTutorial();
            Debug.Log("클릭 튜토리얼 완료");
        }
        else if(isUIClick && isUI)
        {
            controller.SetNextTutorial();
            Debug.Log("UI 닫기 튜토리얼 완료");
        }
    }

    public override void Exit()
    {
        blockPanel.SetActive(false);
    }
}
