using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image ImageFill = null;
    [SerializeField] public Text FireflyAmount = null;
    [SerializeField] public Text notEnough = null;

    private PlayerController controller = null;

    public void setController(PlayerController _controller)
    {
        controller = _controller;
    }

    private void Update()
    {
        SetWingFill(controller.GetWingPowerAmount());
    }
    void SetWingFill (float _amount)
    {
        ImageFill.fillAmount = _amount;
    }

    public void setFireFly (int _amount)
    {
        FireflyAmount.text = _amount.ToString();
    }
    public void TellNoEnd(int _amount)
    {
        StartCoroutine(ActTellNoEnd(_amount));
    }

    public void TellNotEnough(int _amount)
    {
        StartCoroutine(ActTellNotEnough(_amount));
    }

    IEnumerator ActTellNoEnd (int _amount)
    {
        notEnough.text = "You need to hit " + (10 - _amount).ToString() + " more Fireflies to end this level";
        StartCoroutine(FadeTextToFullAlpha(1f, notEnough));
        yield return new WaitForSeconds(5f);
        StartCoroutine(FadeTextToZeroAlpha(1f, notEnough));
        yield return null;
    }
    IEnumerator ActTellNotEnough (int _amount)
    {
        notEnough.text = "You need to hit " + (10 - _amount).ToString() + " more Fireflies to open this secret portal";
        StartCoroutine(FadeTextToFullAlpha(1f, notEnough));
        yield return new WaitForSeconds(5f);
        StartCoroutine(FadeTextToZeroAlpha(1f, notEnough));
        yield return null;
    }
    IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
