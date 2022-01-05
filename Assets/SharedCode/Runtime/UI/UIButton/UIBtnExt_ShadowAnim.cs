using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIButton))]
public class UIBtnExt_ShadowAnim : UIBtnExt
{
    public Vector2 normalShadow = new Vector2(1f, -1f);
    public Vector2 howeredShadow = new Vector2(1f, -1f);
    public Vector2 pressedShadow = new Vector2(1f, -1f);
    public Vector2 disabledShadow = new Vector2(0f, 0f);

    public float time = 1f;

    public Shadow target;

    public override void OnButtonUpdated()
    {
        if (targetBtn.interactable)
        {
            switch (targetBtn.state)
            {
                case UIButton.State.Normal:
                    target.effectDistance = normalShadow; 
                    break;
                case UIButton.State.Howered:
                    StopAllCoroutines();
                    if (gameObject.activeInHierarchy) StartCoroutine(Scale(howeredShadow));
                    break;
                case UIButton.State.Pressed:
                    StopAllCoroutines();
                    if(gameObject.activeInHierarchy) StartCoroutine(Scale(pressedShadow));
                    break;
                default:
                    break;
            }
        }
        else
        {
            target.effectDistance = disabledShadow;
        }
    }

    IEnumerator Scale(Vector2 v)
    {
        target.effectDistance = Vector2.Lerp(target.effectDistance, v, time);
        yield return new WaitForSeconds(time);
    }

    public void OnValidate()
    {
        if (target == null) target = GetComponent<Shadow>();
    }
}
