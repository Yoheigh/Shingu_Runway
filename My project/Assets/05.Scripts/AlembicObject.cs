using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;
using static Define;

public class AlembicObject : MonoBehaviour
{
    public string key;

    public AlembicData data;
    public AlembicStreamPlayer player;
    public MeshRenderer renderer;

    private bool _init = false;

    void Start()
    {
        Init();
    }

    public void Init(Action callback = null)
    {
        if(_init == false)
        {
            player = GetComponentInChildren<AlembicStreamPlayer>();
            renderer = GetComponentInChildren<MeshRenderer>();
        }
        gameObject.SetActive(true);
        callback?.Invoke();
    }

    public void Clear()
    {
        gameObject.SetActive(false);
        // 풀에 집어넣거나 치우기
    }

    public AlembicObject(string key)
    {
        this.key = key;
    }

    public void ChangeMaterial(Material material)
    {
        renderer.material = material;
    }
}
