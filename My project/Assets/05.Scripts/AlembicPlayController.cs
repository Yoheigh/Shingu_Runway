using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;
using static Define;

public class AlembicPlayController : MonoBehaviour
{
    public Dictionary<string, AlembicObject> alembics;
    private bool isPlaying = false;

    public Transform topStartPos;
    public Transform bottomStartPos;

    [SerializeField]
    private AlembicStreamPlayer topStreamPlayer;
    private MeshRenderer topMeshRenderer;

    [SerializeField]
    private AlembicStreamPlayer bottomStreamPlayer;
    private MeshRenderer bottomMeshRenderer;

    // private variables
    private bool _init = false;

    void Start()
    {
        Init();
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
    }

    private void UpdateAlembicAnimation()
    {
        if (isPlaying)
        {
            topStreamPlayer.CurrentTime += Time.deltaTime;
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

    public void ChangeAlembicObject(string key)
    {
        if (alembics.TryGetValue(key, out AlembicObject alembic))
        {
            ChangePlayerProgress(ClothesType.Both, 0f);

            switch (alembic.data.type)
            {
                case ClothesType.Top:
                    topStreamPlayer = alembic.player;
                    topMeshRenderer = alembic.renderer;
                    break;
                case ClothesType.Bottom:
                    bottomStreamPlayer = alembic.player;
                    bottomMeshRenderer = alembic.renderer;
                    break;
                case ClothesType.Both:
                    break;
            }
        }
    }

    public void ChangePlayerProgress(ClothesType type, float time)
    {
        switch (type)
        {
            case ClothesType.Top:
                topStreamPlayer.CurrentTime = time;
                break;
            case ClothesType.Bottom:
                bottomStreamPlayer.CurrentTime = time;
                break;
            case ClothesType.Both:
                topStreamPlayer.CurrentTime = time;
                bottomStreamPlayer.CurrentTime = time;
                break;
        }
    }

    public void ChangeMaterial(ClothesType type, Material material)
    {
        switch (type)
        {
            case ClothesType.Top:
                topMeshRenderer.material = material;
                break;
            case ClothesType.Bottom:
                bottomMeshRenderer.material = material;
                break;
            case ClothesType.Both:
                topMeshRenderer.material = material;
                bottomMeshRenderer.material = material;
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