using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;
using static Define;

public class AlembicObject : MonoBehaviour
{
    public string key;

    public AlembicData data;
    public AlembicStreamPlayer player;
    public MeshRenderer renderer;

    void Start()
    {
        player = GetComponentInChildren<AlembicStreamPlayer>();
        renderer = GetComponentInChildren<MeshRenderer>();
    }

    public AlembicObject(string key)
    {
        this.key = key;
    }
}
