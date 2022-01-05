using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class RingManager : MonoBehaviour
{
    public AutoRotate ringRot;
    Item currItem;
    [Space(5)]
    public ParticleSystem effect;
    [Space(5)]
    public GameObject counterObj;
    public GameObject counterAnim;
    public uNumber countTxt;

    public SpriteRenderer circleSprite;

    public float minRotSpeed;
    public float maxRotSpeed;

    private void OnEnable()
    {
        UIBtnHandler.inst.BtnClicked += BtnClicked;
        Time.timeScale = 0;
        StartCoroutine(StartGame());
    }

    private void Start()
    {
        minRotSpeed = 20f;
        maxRotSpeed = 22f;
    }

    private IEnumerator StartGame()
    {
        short i = 3;
        countTxt.Value = i;
        counterObj.SetActive(true);
        while (i > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            iTween.PunchScale(counterAnim, iTween.Hash("amount", new Vector3(.2f, .2f, 0f), "time", 0.5f, "ignoretimescale", true));
            countTxt.Value = i;
            i--;
        }
        counterObj.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnDisable()
    {
        UIBtnHandler.inst.BtnClicked -= BtnClicked;
    }

    private void BtnClicked(ItemType type, bool isDark)
    {
        //print(string.Format("Btn Pressed \n Type : {0} , isDark : {1}", type, isDark));
        if (currItem != null && currItem.isActive)
        {
            if ((isDark && type == currItem.type) || (!isDark && type != currItem.type)) return;
            currItem.HideSprite();
            SetRandomRingRotation();
            ShowEffect();
            StartCoroutine(ColorFade());
            SetRandomDarkMode();
            AudioPlayer.PlaySFX("Blast");
            UIManager.inst.UpdateScore();
            new Delayed.Action(currItem.ActivateItem, 1.4f);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        //print("Triggerd " + col.name);
        if (col.CompareTag("Item"))
        {
            Item item = col.GetComponent<Item>();
            if (item != null)
            {
                //print(string.Format("Item \n Name : {0} , Type : {1} , Color : {2}", item.name, item.type, item.color));
                currItem = item;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        //print("Triggerd exit " + col.name);
        if (col.CompareTag("Item"))
        {
            Item item = col.GetComponent<Item>();
            if (item.isActive)
            {
                print("gameOver");
                currItem = null;
                AudioPlayer.PlaySFX("GameOver");
                UIManager.inst.OnGameOver();
            }
        }
    }

    public void SetRandomRingRotation()
    {
        maxRotSpeed += 2;
        minRotSpeed += 2;
        ringRot.speed = (UnityEngine.Random.value > .5f) ? new Vector3(0f, 0f, -Rand.GetFloat(minRotSpeed, maxRotSpeed)) : new Vector3(0f, 0f, Rand.GetFloat(minRotSpeed, maxRotSpeed));
    }

    public void SetRandomDarkMode()
    {
        UIBtnHandler.inst.isDarkMode.Value = (UnityEngine.Random.value > .5f);
    }

    public void ShowEffect()
    {
        ParticleSystem.MainModule setting = effect.main;
        setting.startColor = GetCurrItemColor();
        effect.Play();
        new Delayed.Action(effect.Stop, 0.5f);

    }

    public IEnumerator ColorFade()
    {
        Color startColor = GetCurrItemColor();
        startColor.a = .8f;
        circleSprite.color = startColor;
        circleSprite.gameObject.SetActive(true);
        iTween.ValueTo(gameObject, iTween.Hash("from", .8f, "to", 0f, "time", 0.3f, "onupdate", "SetColor", "onupdatetarget", gameObject));
        yield return new WaitForSecondsRealtime(.5f);
        circleSprite.gameObject.SetActive(false);
    }

    void SetColor(float val)
    {
        Color startColor = circleSprite.color;
        startColor.a = val;
        circleSprite.color = startColor;
    }


    public Color GetCurrItemColor()
    {
        if (currItem == null) return Color.white;
        switch (currItem.color)
        {
            case ItemColor.Red:
                return Color.red;
            case ItemColor.Blue:
                return Color.blue;
            case ItemColor.Yellow:
                return Color.yellow;
            default:
                return Color.white;
        }
    }
}
