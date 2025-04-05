using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectButton : MonoBehaviour
{
    public structureType s_Type;

    private SpriteRenderer sr;
    private Material origin_M;
    public Material outline_M;

    [SerializeField] private Customer customer;
    [SerializeField] private QuestSystem quest;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        origin_M = sr.material;
        outline_M.mainTexture = GetComponent<SpriteRenderer>().sprite.texture;
    }

    private void OnMouseEnter()
    {
        sr.material = outline_M;
    }

    private void OnMouseDown()
    {
        
    }

    private void OnMouseExit()
    {
        sr.material = origin_M;
    }
}

public enum structureType
{
    Trade,
    Upgrade,
    Bar,
    Guild
}
