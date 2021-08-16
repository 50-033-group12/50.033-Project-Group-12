using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurtainController : MonoBehaviour
{
    private static CurtainController _instance;

    public static CurtainController GetInstance()
    {
        return _instance;
    }

    private GameObject _leftCurtain;
    private GameObject _rightCurtain;

    private GameObject _logo;

    [SerializeField] private bool startOpen;

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
        _leftCurtain = transform.Find("CurtainLeft").gameObject;
        _rightCurtain = transform.Find("CurtainRight").gameObject;
        _logo = transform.Find("Logo").gameObject;
        if (startOpen)
        {
            var leftMovement = _leftCurtain.GetComponent<RectTransform>().rect.width;
            var rightMovement = _rightCurtain.GetComponent<RectTransform>().rect.width;
            LeanTween.alphaCanvas(_logo.GetComponent<CanvasGroup>(), 0, 0f).setOnComplete(() =>
            {
                LeanTween.moveLocalX(_leftCurtain, -leftMovement * 2, 0f)
                    .setEaseInCubic();
                LeanTween.moveLocalX(_rightCurtain, rightMovement * 2, 0f)
                    .setEaseInCubic();
            });
        }
    }

    public void CloseCurtain()
    {
        CloseCurtain(() => { });
    }

    public void CloseCurtain(Action onClose)
    {
        var leftMovement = _leftCurtain.GetComponent<RectTransform>().rect.width;
        var rightMovement = _rightCurtain.GetComponent<RectTransform>().rect.width;
        LeanTween.moveLocalX(_leftCurtain, leftMovement / -2, 1.0f)
            .setEaseOutCubic();
        LeanTween.moveLocalX(_rightCurtain, rightMovement / 2, 1.0f)
            .setEaseOutCubic()
            .setOnComplete(
                () =>
                {
                    LeanTween.alphaCanvas(_logo.GetComponent<CanvasGroup>(), 1, 1.0f)
                        .setOnComplete(onClose);
                }
            );
    }

    public void OpenCurtain()
    {
        OpenCurtain(() => { });
    }

    public void OpenCurtain(Action onOpen)
    {
        var leftMovement = _leftCurtain.GetComponent<RectTransform>().rect.width;
        var rightMovement = _rightCurtain.GetComponent<RectTransform>().rect.width;
        LeanTween.alphaCanvas(_logo.GetComponent<CanvasGroup>(), 0, 1.0f).setOnComplete(() =>
        {
            LeanTween.moveLocalX(_leftCurtain, -leftMovement * 2, 1.0f)
                .setEaseInCubic();
            LeanTween.moveLocalX(_rightCurtain, +rightMovement * 2, 1.0f)
                .setEaseInCubic()
                .setOnComplete(() =>
                {
                    Debug.Log(_leftCurtain.transform.localPosition.x);
                    Debug.Log(_rightCurtain.transform.localPosition.x);
                    onOpen();
                });
        });
    }
}