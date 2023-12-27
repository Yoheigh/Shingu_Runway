using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;
using UnityEngine.SceneManagement;
using static Define;

public class AlembicPlayController : MonoBehaviour
{
    // 실제 생성된 오브젝트들
    public Dictionary<int, AlembicObject> alembics = new Dictionary<int, AlembicObject>();

    public bool IsPlaying { get { return isPlaying; } private set { } }
    private bool isPlaying = false;

    public Transform RootTransform;

    public AlembicStreamPlayer CurrentStream
    {
        get
        {
            if (topStreamPlayer != null && topStreamPlayer.CurrentTime != topStreamPlayer.EndTime)
                return topStreamPlayer;
            else if (bottomStreamPlayer != null && bottomStreamPlayer.CurrentTime != bottomStreamPlayer.EndTime)
                return bottomStreamPlayer;
            else if (AccessoryStreamPlayer != null && AccessoryStreamPlayer.CurrentTime != AccessoryStreamPlayer.EndTime)
                return AccessoryStreamPlayer;
            else
                return null;
        }
        private set { }
    }

    [SerializeField]
    private AlembicStreamPlayer topStreamPlayer;
    private AlembicObject topCurrentAlembic;

    [SerializeField]
    public AlembicStreamPlayer bottomStreamPlayer;
    private AlembicObject bottomCurrentAlembic;

    [SerializeField]
    public AlembicStreamPlayer AccessoryStreamPlayer;
    private AlembicObject AccessoryCurrentAlembic;

    public AudioSource audioSource;
    public AudioClip[] clips;

    // private variables
    private bool _init = false;

    private void Start()
    {
        Init();
        PlayRandomMusic();
    }

    private void Init()
    {
        if (_init == false)
        {
            _init = false;
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            isPlaying = false;
        }
    }

    void Update()
    {
        UpdateAlembicAnimation();

        if (Input.GetKeyDown(KeyCode.Space)) { ChangePlayStateToggle(); }
        if (Input.GetKeyDown(KeyCode.Minus)) { ChangePlayProgress(ClothesType.Both, 0f); ChangePlayProgress(ClothesType.Accessory, 0f); }

        int temp = UnityEngine.Random.Range(0, 3);
        int gacha = UnityEngine.Random.Range(0, 10);

        if (Input.GetKeyDown(KeyCode.Alpha1)) Set01();
        if (Input.GetKeyDown(KeyCode.Alpha2)) Set02();
        if (Input.GetKeyDown(KeyCode.Alpha3)) Set03();
        if (Input.GetKeyDown(KeyCode.Alpha4)) Set04();
        if (Input.GetKeyDown(KeyCode.Alpha5)) Set05();
        if (Input.GetKeyDown(KeyCode.Alpha6)) Set06();
        if (Input.GetKeyDown(KeyCode.Alpha7)) Set07();
        if (Input.GetKeyDown(KeyCode.Alpha8)) Set08();
        if (Input.GetKeyDown(KeyCode.Alpha9)) Set09();
        if (Input.GetKeyDown(KeyCode.Alpha0)) Set09();

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            ChangeMaterial((ClothesType)Enum.GetValues(typeof(ClothesType)).GetValue(temp),
                           (PatternType)Enum.GetValues(typeof(PatternType)).GetValue(gacha));
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            ChangeMaterial(ClothesType.Top,
                           (PatternType)Enum.GetValues(typeof(PatternType)).GetValue(gacha));
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            ChangeMaterial(ClothesType.Bottom,
                           (PatternType)Enum.GetValues(typeof(PatternType)).GetValue(gacha));
        }


        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            ChangeMaterial(ClothesType.Both, (PatternType)Enum.GetValues(typeof(PatternType)).GetValue(gacha));
            ChangeMaterial(ClothesType.Accessory, (PatternType)Enum.GetValues(typeof(PatternType)).GetValue(gacha));
        }

        if (Input.GetKeyDown(KeyCode.Plus)) { ChangePlayProgress(ClothesType.Both, 0.5f); ChangePlayProgress(ClothesType.Accessory, 0.5f); }

    }

    private void UpdateAlembicAnimation()
    {
        if (isPlaying)
        {
            if (topStreamPlayer != null)
                topStreamPlayer.CurrentTime += Time.deltaTime;

            if (bottomStreamPlayer != null)
                bottomStreamPlayer.CurrentTime += Time.deltaTime;

            if (AccessoryStreamPlayer != null)
                AccessoryStreamPlayer.CurrentTime += Time.deltaTime;
        }
    }

    public void ChangePlayState(bool flag)
    {
        if (flag)
            isPlaying = true;
        else
            isPlaying = false;
    }

    public void ChangePlayStateToggle()
    {
        isPlaying = !isPlaying;

        if (topStreamPlayer != null)
        {
            if (topStreamPlayer.StartTime == topStreamPlayer.EndTime)
            {
                ChangePlayProgress(ClothesType.Both, 0);
                ChangePlayState(true);
            }
        }
    }

    public void ChangeAlembicObject(int key)
    {
        if (alembics.TryGetValue(key, out AlembicObject alembic) == false)
            alembic = InstantiateAlembic(key);

        if (alembic == null)
            return;

        // 시퀀스가 다르면 기존의 다른 의상 Clear
        CheckSequenceDifferent(alembic.data);

        switch (alembic.data.type)
        {
            case ClothesType.Top:
                topCurrentAlembic?.Clear();
                alembic.Init(() =>
                {
                    topStreamPlayer = alembic.player;
                    topCurrentAlembic = alembic;
                });
                break;

            case ClothesType.Bottom:
                bottomCurrentAlembic?.Clear();
                alembic.Init(() =>
                {
                    bottomStreamPlayer = alembic.player;
                    bottomCurrentAlembic = alembic;
                });
                break;

            case ClothesType.Both:
                topCurrentAlembic?.Clear();
                bottomCurrentAlembic?.Clear();

                alembic.Init(() =>
                {
                    topStreamPlayer = alembic.player;
                    topCurrentAlembic = alembic;
                });
                break;

            case ClothesType.Accessory:
                AccessoryCurrentAlembic?.Clear();
                alembic.Init(() =>
                {
                    AccessoryStreamPlayer = alembic.player;
                    AccessoryCurrentAlembic = alembic;
                });
                break;
        }

        ChangePlayProgress(ClothesType.Both, 0f);
    }

    public void CheckSequenceDifferent(AlembicData data)
    {
        if (topCurrentAlembic != null)
        {
            if (topCurrentAlembic.data.sequence != data.sequence)
            {
                topCurrentAlembic.Clear();
                topCurrentAlembic = null;
                topStreamPlayer = null;
            }
        }

        if (bottomCurrentAlembic != null)
        {
            if (bottomCurrentAlembic?.data.sequence != data.sequence)
            {
                bottomCurrentAlembic.Clear();
                bottomCurrentAlembic = null;
                bottomStreamPlayer = null;
            }
        }

        if (AccessoryCurrentAlembic != null)
        {
            //if (AccessoryCurrentAlembic?.data.sequence != data.sequence)
            //{
                AccessoryCurrentAlembic.Clear();
                AccessoryCurrentAlembic = null;
                AccessoryStreamPlayer = null;
            //}
        }
    }

    public void ChangePlayProgress(ClothesType type, float time)
    {
        switch (type)
        {
            case ClothesType.Top:
                if (topCurrentAlembic != null)
                    topStreamPlayer.CurrentTime = time;
                break;

            case ClothesType.Bottom:
                if (bottomCurrentAlembic != null)
                    bottomStreamPlayer.CurrentTime = time;
                break;

            case ClothesType.Both:
                if (topCurrentAlembic != null)
                    topStreamPlayer.CurrentTime = time;

                if (bottomCurrentAlembic != null)
                    bottomStreamPlayer.CurrentTime = time;
                break;

            case ClothesType.Accessory:
                if (AccessoryCurrentAlembic != null)
                    AccessoryStreamPlayer.CurrentTime = time;
                break;
        }
    }

    public void ChangeMaterial(ClothesType clothes, PatternType pattern)
    {
        switch (clothes)
        {
            case ClothesType.Top:
                topCurrentAlembic.ChangeMaterial(pattern);
                break;
            case ClothesType.Bottom:
                bottomCurrentAlembic.ChangeMaterial(pattern);
                break;
            case ClothesType.Both:
                topCurrentAlembic.ChangeMaterial(pattern);
                // bottomCurrentAlembic.ChangeMaterial(pattern);
                break;
            case ClothesType.Accessory:
                AccessoryCurrentAlembic.ChangeMaterial(pattern);
                break;
        }
    }

    public AlembicObject InstantiateAlembic(int index)
    {
        AlembicData data = Managers.Data.GetAlembicData(index);

        if (data == null)
        {
            Debug.Log($"{index}에 해당하는 Data가 없습니다.");
            return null;
        }

        // 새로운 Alembic 생성
        GameObject prefab;

        if (data.prefab == null)
        {
            prefab = Instantiate(Managers.Data.GetAlembicData(index).prefab, RootTransform);
        }
        else
        {
            prefab = Managers.Data.GetAlembicData(index).prefab;
        }

        prefab.transform.position = Vector3.zero;
        prefab.transform.SetParent(RootTransform, false);
        prefab.SetActive(true);
        AlembicObject alembic = prefab.AddComponent<AlembicObject>();

        alembic.data = data;
        alembics.Add(alembic.data.id, alembic);
        Debug.Log($"{alembic.data.id},{alembic.data.prefab} 성공적으로 등록 완료");

        return alembic;
    }

    public void PlayRandomMusic()
    {
        audioSource.clip = clips[UnityEngine.Random.Range(0, clips.Length)];
        audioSource.Play();

        Invoke("PlayRandomMusic", audioSource.clip.length);
    }

    public void Set01()
    {
        RootTransform.position = new Vector3(-2.0f, RootTransform.position.y, RootTransform.position.z);
        ChangeAlembicObject(210108);
        ChangeAlembicObject(120105);
    }

    public void Set02()
    {

        RootTransform.position = new Vector3(-2.0f, RootTransform.position.y, RootTransform.position.z);
        ChangeAlembicObject(210107);
        ChangeAlembicObject(420101);
    }

    public void Set03()
    {
        RootTransform.position = new Vector3(-1.7f, RootTransform.position.y, RootTransform.position.z);
        ChangeAlembicObject(415103);
        ChangeAlembicObject(225114);
        // ChangeAlembicObject(415105);    // 파일 인덱스는 425105로 되어있어서 다시 임포트해야함
    }

    public void Set04()
    {

        RootTransform.position = new Vector3(-1.7f, RootTransform.position.y, RootTransform.position.z);
        ChangeAlembicObject(400306);
    }

    public void Set05()
    {
        RootTransform.position = new Vector3(-1.7f, RootTransform.position.y, RootTransform.position.z);
        ChangeAlembicObject(125104);
        ChangeAlembicObject(115103);
    }

    public void Set06()
    {
        RootTransform.position = new Vector3(-2.0f, RootTransform.position.y, RootTransform.position.z);
        ChangeAlembicObject(210107);
        ChangeAlembicObject(220101);
    }

    public void Set07()
    {
        RootTransform.position = new Vector3(-1.7f, RootTransform.position.y, RootTransform.position.z);

        ChangeAlembicObject(415105);
        ChangeAlembicObject(425104);

        // 액세서리는 맨 마지막에 덮어씌우는 식으로만 작동함
        // 옷을 바꾸면 액세서리 바로 사라짐. 나중에 바꾸면 바꿔야 함
        ChangeAlembicObject(235118);
    }

    public void Set08()
    {
        RootTransform.position = new Vector3(-2.0f, RootTransform.position.y, RootTransform.position.z);

        ChangeAlembicObject(200104);
    }

    public void Set09()
    {
        RootTransform.position = new Vector3(-2.0f, RootTransform.position.y, RootTransform.position.z);

        ChangeAlembicObject(210112);
        ChangeAlembicObject(220101);
    }

    public void Set10()
    {
        RootTransform.position = new Vector3(-1.7f, RootTransform.position.y, RootTransform.position.z);

        ChangeAlembicObject(225114);
        ChangeAlembicObject(215115);

        
    }
}

public class Define
{
    public enum PlayStateEnum
    {
        Play,
        Stop
    }

    // 노션 메인 로비에 VR용 EventHandler 강좌 찾아놨음
    // 근데 볼 일은 없을 듯 괜히 너무 힘주고 했나봄
    public enum UIEvent
    {
        Click,
        Pressed,
        PointerEnter,
        PointerExit,
        PointerDown,
        PointerUp,
        BeginDrag,
        Drag,
        EndDrag,
    }

    public enum ClothesType
    {
        Both = 0,
        Top = 1,
        Bottom = 2,
        Accessory = 3,
    }

    public enum SequenceType
    {
        Women_Default = 0,
        Women_CatWalk01 = 1,
        Women_CatWalk02 = 2,
        Women_CatWalk03 = 3,
        Women_CatWalk04 = 4,
        Women_CatWalk05 = 5,

        Man_Default = 50,
        Man_CatWalk01 = 51,
    }

    public enum PatternType
    {
        None = 0, // Cotton_mat(CottonWhite)
        CottonBlack = 1,
        Fabric034 = 2,
        Fabric035 = 3,
        // FabricCamo003 = 4, // 현역병 의상 뭐냐 이거
        FabricNonwoven001 = 5,
        FabricPolyester002 = 6,
        FabricSilk001 = 7,
        FabricTarp002 = 8,
        Knitted006 = 9,
        Denim001 = 10,
        Denim002 = 11,
        Denim003 = 12,
        Denim004 = 13,
        Felt001 = 14,
        Felt002 = 15,
        Leather001 = 16,
        Leather002 = 17,
        Leather003 = 18,
        Leather004 = 19,
        Masdras001 = 20,
        MIcrofiber001 = 21,
        Moleskin001 = 22,
        Oxford001 = 23,
        Oxford002 = 24,
        Oxford003 = 25,
        Oxford004 = 26,
    }
}