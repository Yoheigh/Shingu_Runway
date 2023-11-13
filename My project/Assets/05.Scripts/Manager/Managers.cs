using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }

    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);  //Scene 이 종료되도 파괴 되지 않게 
            s_instance = go.GetComponent<Managers>();
        }
    }

    private void Awake()
    {
        GameObject go = GameObject.Find("@Data");
        _data = go.GetComponent<DataManager>();

        go = GameObject.Find("@Material");
        _material = go.GetComponent<MaterialManager>();   
        
        go = GameObject.Find("@AlembicPlayer");
        _player = go.GetComponent<AlembicPlayController>();
    }

    DataManager _data;
    PoolManager _pool = new PoolManager();
    ObjectManager _object = new ObjectManager();
    ResourceManager _resource = new ResourceManager();
    MaterialManager _material;
    AlembicPlayController _player;
    UIManager _ui = new UIManager();

    public static DataManager Data { get { return Instance?._data; } }
    public static PoolManager Pool { get { return Instance?._pool; } }
    public static ObjectManager Object { get { return Instance?._object; } }
    public static ResourceManager Resource { get { return Instance?._resource; } }
    public static MaterialManager Mat { get { return Instance?._material; } }
    public static AlembicPlayController Player { get { return Instance?._player; } }
    public static UIManager UI { get { return Instance?._ui; } }
}