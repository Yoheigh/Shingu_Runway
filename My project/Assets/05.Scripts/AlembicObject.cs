using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;
using UnityEngine.Rendering;
using static Define;

public class AlembicObject : MonoBehaviour
{
    public string key;

    public AlembicData data;
    public AlembicStreamPlayer player;
    public Renderer renderer;

    private bool _init = false;

    void Start()
    {
        Init();
        ChangeMaterial((int)PatternType.None);
        // mat.SetColor("_Color", Color.blue);
    }

    public void Init(Action callback = null)
    {
        if(_init == false)
        { 
            player = GetComponent<AlembicStreamPlayer>();
            renderer = GetComponentInChildren<Renderer>();
        }
        gameObject.SetActive(true);
        callback?.Invoke();
    }

    public void Clear()
    {
        gameObject.SetActive(false);
        ChangeMaterial((int)PatternType.None);
        player.CurrentTime = 0f;

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

    public void ChangeMaterial(PatternType type)
    {
        renderer.material = Managers.Mat.GetMaterial((int)type);
    }
}
