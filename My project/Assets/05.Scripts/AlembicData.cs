using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;
using static Define;

[System.Serializable]
public class AlembicData
{
    public int id;
    public string name;
    public ClothesType type;
    public string description;
    public Sprite image;
    public GameObject prefab;
}
