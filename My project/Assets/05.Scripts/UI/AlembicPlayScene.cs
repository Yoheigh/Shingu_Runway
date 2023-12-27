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

    public Image Image_Play;
    public Image Image_Pause;

    public Button Btn_OnPause;
    public Button Btn_OnReplay;
    public Button Btn_OnChangeAlembic;

    public Toggle TopToggle;
    public Toggle BotToggle;
    public Toggle AccToggle;

    public Slider Slider_OnMoveSlider;

    AlembicPlayController Player => Managers.Player;

    void Start()
    {
        Init();

        Btn_OnPlay.onClick.AddListener(() => { OnPlay?.Invoke(); });
        // Btn_OnPause.onClick.AddListener(() => { OnPause?.Invoke(); });
        Btn_OnReplay.onClick.AddListener(() => { OnReplay?.Invoke(); });
        // Btn_OnChangeAlembic.onClick.AddListener(() => { OnChangeAlembic?.Invoke(); });
        //Slider_OnMoveSlider.onValueChanged.AddListener((obj) =>
        //{
        //    obj = Slider_OnMoveSlider.value;
        //    OnMoveSlider?.Invoke(obj);
        //});

        Image_Pause.gameObject.SetActive(false);

        if (Player.IsPlaying)
        {
            Image_Pause.gameObject.SetActive(true);
            Image_Play.gameObject.SetActive(false);
            Btn_OnPlay.targetGraphic = Image_Play;
        }
        else
        {
            Image_Pause.gameObject.SetActive(false);
            Image_Play.gameObject.SetActive(true);
            Btn_OnPlay.targetGraphic = Image_Pause;
        }
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
        StreamProgressParameter();

        if (Input.GetKeyDown(KeyCode.B)) OnPlay?.Invoke();
    }

    public void OnPlayFunction()
    {
        Player.ChangePlayStateToggle();

        if (Player.IsPlaying)
        {
            Image_Pause.gameObject.SetActive(true);
            Image_Play.gameObject.SetActive(false);
            Btn_OnPlay.targetGraphic = Image_Play;
        }
        else
        {
            Image_Pause.gameObject.SetActive(false);
            Image_Play.gameObject.SetActive(true);
            Btn_OnPlay.targetGraphic = Image_Pause;
        }
    }

    public void OnReplayFunction()
    {
        Player.ChangePlayState(false);
        Player.ChangePlayProgress(ClothesType.Both, 0f);
        Player.ChangePlayProgress(ClothesType.Accessory, 0f);

        if (Player.IsPlaying)
        {
            Image_Pause.gameObject.SetActive(true);
            Image_Play.gameObject.SetActive(false);
            Btn_OnPlay.targetGraphic = Image_Play;
        }
        else
        {
            Image_Pause.gameObject.SetActive(false);
            Image_Play.gameObject.SetActive(true);
            Btn_OnPlay.targetGraphic = Image_Pause;
        }
    }

    public void OnChangeAlembicFunction()
    {
        // 불러온 데이터의 키 값으로 검색해서 바꾸기
        // controller.ChangeAlembicObject(key);
    }

    public void OnMoveSliderFunction()
    {
        Player.ChangePlayState(false);
        Player.ChangePlayProgress(ClothesType.Both, Slider_OnMoveSlider.value);

        // 재생 시간 나타내기 위해 취소
        // Player.StreamProgressParameter(ClothesType.Both);
    }

    public void StreamProgressParameter()
    {
        if (Player.CurrentStream != null)
        {
            Slider_OnMoveSlider.minValue = Player.CurrentStream.StartTime;
            Slider_OnMoveSlider.maxValue = Player.CurrentStream.EndTime;
            Slider_OnMoveSlider.value = Player.CurrentStream.CurrentTime;
        }
        //else if (Player.CurrentStream.CurrentTime == Player.CurrentStream.EndTime)
        //{
        //    Slider_OnMoveSlider.minValue = 0f;
        //    Slider_OnMoveSlider.maxValue = 0f;
        //    Slider_OnMoveSlider.value = 0f;
        //    OnPlayFunction();
        //}
        else
        {
            Player.ChangePlayProgress(ClothesType.Both, 0f);
            Player.ChangePlayProgress(ClothesType.Accessory, 0f);
            OnPlayFunction();
        }
    }
}