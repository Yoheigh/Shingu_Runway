using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class AlembicPlayScene : MonoBehaviour
{
    // 이벤트
    public Action OnPlay;
    // public Action OnPause;
    public Action OnReplay;
    public Action OnChangeAlembic;

    public Action<float> OnMoveSlider;

    // 버튼
    public Button Btn_OnPlay;
    public Button Btn_OnPause;
    public Button Btn_OnReplay;
    public Button Btn_OnChangeAlembic;

    public Slider Slider_OnMoveSlider;

    AlembicPlayController controller;

    void Start()
    {
        Btn_OnPlay.onClick.AddListener(() => { OnPlay?.Invoke(); });
        // Btn_OnPause.onClick.AddListener(() => { OnPause?.Invoke(); });
        Btn_OnReplay.onClick.AddListener(() => { OnReplay?.Invoke(); });
        Btn_OnChangeAlembic.onClick.AddListener(() => { OnChangeAlembic?.Invoke(); });
        Slider_OnMoveSlider.onValueChanged.AddListener((obj) => {
            obj = Slider_OnMoveSlider.value;
            OnMoveSlider?.Invoke(obj);
            });
    }

    private void Init()
    {
        OnPlay -= OnPlayFunction;
        OnPlay += OnPlayFunction;
        OnReplay -= OnReplayFunction;
        OnReplay += OnReplayFunction;
    }

    private void Update()
    {
        // UI 설정에 따라서 변경
        // StreamProgressParameter()
    }

    public void OnPlayFunction()
    {
        controller.ChangePlayStateToggle();
    }

    public void OnReplayFunction()
    {
        controller.ChangePlayState(false);
        controller.ChangePlayerProgress(ClothesType.Both, 0f);
    }

    public void OnChangeAlembicFunction()
    {
        // 불러온 데이터의 키 값으로 검색해서 바꾸기
        // controller.ChangeAlembicObject(key);
    }

    public void OnMoveSliderFunction()
    {
        controller.ChangePlayState(false);
        controller.StreamProgressParameter(ClothesType.Both);
    }
}