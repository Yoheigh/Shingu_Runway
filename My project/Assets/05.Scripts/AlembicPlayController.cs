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
    private bool isPlaying = false;

    public Transform RootTransform;

    [SerializeField]
    private AlembicStreamPlayer topStreamPlayer;
    private AlembicObject topCurrentAlembic;

    [SerializeField]
    private AlembicStreamPlayer bottomStreamPlayer;
    private AlembicObject bottomCurrentAlembic;

    public Material sampleMat;

    // private variables
    private bool _init = false;

    void Start()
    {
        Init();

        ChangeAlembicObject(115101);
        ChangeAlembicObject(125102);
    }

    private void Init()
    {
        if (_init == false)
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
        }
    }

    void Update()
    {
        UpdateAlembicAnimation();

        if (Input.GetKeyDown(KeyCode.Space)) { ChangePlayStateToggle(); }
        if (Input.GetKeyDown(KeyCode.Alpha0)) { ChangePlayProgress(ClothesType.Both, 0f); }
        if (Input.GetKeyDown(KeyCode.U)) { ChangeMaterial(ClothesType.Top, sampleMat); }
        if (Input.GetKeyDown(KeyCode.P)) { ChangeAlembicObject(115103); }
        if (Input.GetKeyDown(KeyCode.O)) { ChangeAlembicObject(115101); }


    }

    private void UpdateAlembicAnimation()
    {
        if (isPlaying)
        {
            if (topStreamPlayer != null)
                topStreamPlayer.CurrentTime += Time.deltaTime;

            if (bottomStreamPlayer != null)
                bottomStreamPlayer.CurrentTime += Time.deltaTime;
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
    }

    public void ChangeAlembicObject(int key)
    {
        if (alembics.TryGetValue(key, out AlembicObject alembic) == false)
            alembic = InstantiateAlembic(key);

        ChangePlayProgress(ClothesType.Both, 0f);

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
                break;
        }
    }

    public void ChangePlayProgress(ClothesType type, float time)
    {
        switch (type)
        {
            case ClothesType.Top:
                if (topStreamPlayer != null)
                    topStreamPlayer.CurrentTime = time;
                break;
            case ClothesType.Bottom:
                if (bottomStreamPlayer != null)
                    bottomStreamPlayer.CurrentTime = time;
                break;
            case ClothesType.Both:
                if (topStreamPlayer != null && bottomStreamPlayer != null)
                {
                    topStreamPlayer.CurrentTime = time;
                    bottomStreamPlayer.CurrentTime = time;
                }
                break;
        }
    }

    public void ChangeMaterial(ClothesType type, Material material)
    {
        switch (type)
        {
            case ClothesType.Top:
                topCurrentAlembic.ChangeMaterial(material);
                break;
            case ClothesType.Bottom:
                bottomCurrentAlembic.ChangeMaterial(material);
                break;
            case ClothesType.Both:
                topCurrentAlembic.ChangeMaterial(material);
                bottomCurrentAlembic.ChangeMaterial(material);
                break;
        }
    }

    public float StreamProgressParameter(ClothesType type = ClothesType.Both)
    {
        switch (type)
        {
            case ClothesType.Top:
                float topReturn = topStreamPlayer.CurrentTime / topStreamPlayer.EndTime;
                return topReturn;
            case ClothesType.Bottom:
                float botReturn = bottomStreamPlayer.CurrentTime / bottomStreamPlayer.EndTime;
                return botReturn;
            default:
                // 상의 기준
                float bothReturn = topStreamPlayer.CurrentTime / topStreamPlayer.EndTime;
                return bothReturn;
        }
    }

    public AlembicObject InstantiateAlembic(int index)
    {
        if (Managers.Data.GetAlembicData(index) == null)
        {
            Debug.Log($"{index}에 해당하는 Data가 없습니다.");
            return null;
        }

        ChangePlayState(false);

        GameObject prefab = Instantiate(Managers.Data.GetAlembicData(index).prefab, RootTransform);
        prefab.transform.SetParent(RootTransform, false);
        AlembicObject alembic = prefab.AddComponent<AlembicObject>();

        alembic.data = Managers.Data.GetAlembicData(index);
        alembics.Add(alembic.data.id, alembic);
        Debug.Log($"{alembic.data.id},{alembic.data.prefab} 성공적으로 등록 완료");

        return alembic;
    }
}

public class Define
{
    public enum PlayStateEnum
    {
        Play,
        Stop
    }

    public enum ClothesType
    {
        Top,
        Bottom,
        Both
    }
}