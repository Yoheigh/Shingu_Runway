using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;
using static Define;

public enum PatternType
{
    None,
    Stripe,
}

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

        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        //mat.SetColor("_Color", Color.blue);
        renderer.material = mat;

        int[] iDs = renderer.material.GetTexturePropertyNameIDs();
        string[] names = renderer.material.GetTexturePropertyNames();

        Debug.Log($"새로 불러온 의상 : {data.id}");

        for (int i = 0, imax = iDs.Length - 1; i < imax; i++)
        {
            renderer.material.EnableKeyword(names[i]);
            Debug.Log($"{iDs[i]}, {renderer.material.IsKeywordEnabled(names[i])} {names[i]}");
        }

        renderer.material.EnableKeyword("_Color");
        // renderer.material.SetColor("_Color", Color.blue);

        //renderer.material.IsKeywordEnabled("_NORMALMAP");
        //renderer.material.IsKeywordEnabled("_METALLICGLOSSMAP");
        //renderer.material.EnableKeyword("_NORMALMAP");
        //renderer.material.EnableKeyword("_METALLICGLOSSMAP");



        ////Set the Texture you assign in the Inspector as the main texture (Or Albedo)
        //renderer.material.SetTexture("_MainTex", m_MainTexture);
        ////Set the Normal map using the Texture you assign in the Inspector
        //renderer.material.SetTexture("_BumpMap", m_Normal);
        ////Set the Metallic Texture as a Texture you assign in the Inspector
        //renderer.material.SetTexture("_MetallicGlossMap", m_Metal);
    }

    public void Init(Action callback = null)
    {
        if(_init == false)
        {
            player = GetComponentInChildren<AlembicStreamPlayer>();
            renderer = GetComponentInChildren<Renderer>();
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

    public void ChangeMaterial(PatternType type)
    {
        Material mat;
    }
}
