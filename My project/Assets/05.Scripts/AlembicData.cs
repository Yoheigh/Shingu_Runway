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
    public SequenceType sequence;
    public string description;
    public Sprite image;

    // 스크립트에서 적용할 부분이라 SO에선 관리안함
    // abc 확장자 특징 때문에 프리팹으로 만들 수가 없음
    [HideInInspector]
    public GameObject prefab;

    public AlembicData(int _id, 
                       string _name, 
                       ClothesType _type,
                       SequenceType _sequence,
                       string _description, 
                       Sprite _image)
    {
        id = _id;
        name = _name;
        type = _type;
        sequence = _sequence;
        description = _description;
        image = _image;
        // prefab = _go;
    }
}

[System.Serializable]
public class TextureData
{
    public int id;
    public string name;
    public Material material;
    public Texture2D main;
    public Texture2D normal;
    public Texture2D roughness;
    public Texture2D height;
    public Texture2D opacity;
    public Texture2D occlusion;

    [Range(0f, 1f)]
    public float smoothnessValue = 0.1f;

    [Range(0f, 1f)]
    public float HeightValue = 0.2f;

}
