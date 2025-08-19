using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneAnimation : MonoBehaviour
{
    private Image target;
    public Sprite[] sprites;
    [SerializeField] private float frameDuration = 0.2f;

    private int currentIndex = 0;

    private void Awake()
    {
        target = GetComponent<Image>();
    }

    private void Start()
    {
        if(sprites.Length > 0)
        {
            StartCoroutine(PlayAnimation());
        }
    }

    IEnumerator PlayAnimation()
    {
        while (true)
        {
            target.sprite = sprites[currentIndex];
            currentIndex = (currentIndex + 1) % sprites.Length;
            yield return new WaitForSeconds(frameDuration);
        }
    }

    private void OnDestroy()
    {
        StopCoroutine(PlayAnimation());
    }
}
