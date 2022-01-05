using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBtnHandler : MonoBehaviour
{
    public static UIBtnHandler inst;
    public ButtonClass hexaBtn;
    public ButtonClass pentaBtn;
    public ItemColorCollection colorCollection;

    public uBool isDarkMode;

    public event Action<ItemType, bool> BtnClicked;

    private void Awake()
    {
        inst = this;
    }
    private void OnEnable()
    {
        hexaBtn.btn.Clicked += HexaBtn_Clicked;
        pentaBtn.btn.Clicked += PentaBtn_Clicked;
        isDarkMode.ValueChanged += OnDarkModeChange;
    }

    private void OnDarkModeChange()
    {
        AudioPlayer.PlaySFX("Change");
    }

    private void OnDisable()
    {
        hexaBtn.btn.Clicked -= HexaBtn_Clicked;
        pentaBtn.btn.Clicked -= PentaBtn_Clicked;
        isDarkMode.ValueChanged -= OnDarkModeChange;
    }

    private void HexaBtn_Clicked()
    {
        BtnClicked?.Invoke(ItemType.Hexa, isDarkMode.Value);
    }

    private void PentaBtn_Clicked()
    {
        BtnClicked?.Invoke(ItemType.Penta, isDarkMode.Value);
    }

    private void LateUpdate()
    {
        hexaBtn.img.color = colorCollection.GetBtnColor(isDarkMode.Value);
        hexaBtn.shadow.effectColor = colorCollection.GetShadowColor(isDarkMode.Value);
        pentaBtn.img.color = colorCollection.GetBtnColor(isDarkMode.Value);
        pentaBtn.shadow.effectColor = colorCollection.GetShadowColor(isDarkMode.Value);
    }
}

[Serializable]
public class ButtonClass
{
    public UIButton btn;
    public Image img;
    public Shadow shadow;
}