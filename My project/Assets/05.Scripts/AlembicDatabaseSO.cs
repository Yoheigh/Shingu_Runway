using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AlembicDatabaseSO : ScriptableObject
{
    public AlembicData[] alembics;
    public TextureData[] textures;

#if UNITY_EDITOR
    //private void OnValidate()
    //{

    //}
#endif
}