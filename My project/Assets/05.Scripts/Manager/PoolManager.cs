using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;                         // 오브젝트 풀 네임스페이스

class Pool
{
    GameObject _prefab;                         // 오브젝트 풀에 넣을 오브젝트
    IObjectPool<GameObject> _pool;              // 유니티에서 지원하는 IObjectPool 인터페이스

    Transform _root;                            // 오브젝트 풀 부모 오브젝트

    Transform Root                              // 오브젝트 풀 부모 오브젝트 ( 프로퍼티 )
    {
        get
        {
            if (_root == null)
            {
                GameObject temp = new GameObject() { name = $"@{_prefab.name}Pool"};
                _root = temp.transform;
            }

            return _root;
        }
    }

    public Pool(GameObject prefab)
    {
        _prefab = prefab;
        _pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy);      // 풀에서 사용할 Create, Get, Release, Destroy 함수 추가
    }
    
    // ObjectPool<T> 생성자를 뜯어보면...

    // Func<GameObject>     createFunc      : OnCreate
    // Action<GameObject>   actionOnGet     : OnGet
    // Action<GameObject>   actionOnRelease : OnRelease
    // Action<GameObject>   actionOnDestroy : OnDestroy
    
    // 이렇게 집어넣은 함수는 _pool.Get / _pool.Release 함수를 호출할 때마다 함께 실행된다.
    // 오브젝트 풀의 최대 개수보다 많은 경우 _pool.Release 함수에서 OnDestroy를 호출해준다.

    // 생성자 함수 부분 셔터 닫아놓고 뜯는 바람에 좀 헤맸다
    // HashSetPool, GenericPool, CollectionPool 등 종류가 많으니 필요한 경우 더 공부해서 사용하자

    public void Push(GameObject go)
    {
        if (go.activeSelf)
            _pool.Release(go);
    }

    public GameObject Pop()
    {
        return _pool.Get();
    }

    GameObject OnCreate()       
    {
        GameObject go = GameObject.Instantiate(_prefab);
        go.transform.SetParent(Root);
        go.name = _prefab.name;
        return go;
    }

    void OnGet(GameObject go)
    {
        go.SetActive(true);
    }

    void OnRelease(GameObject go)
    {
        go.SetActive(false);
    }

    void OnDestroy(GameObject go)
    {
        GameObject.Destroy(go);
    }
}

public class PoolManager
{
    private Dictionary<string, Pool> _pools { get; } = new Dictionary<string, Pool>();

    public GameObject Pop(GameObject prefab)
    {
        if (_pools.ContainsKey(prefab.name) == false)
            CreatePool(prefab);

        return _pools[prefab.name].Pop();
    }

    public bool Push(GameObject go)
    {
        if (_pools.ContainsKey(go.name) == false)
            return false;

        _pools[go.name].Push(go);
        return true;
    }

    public void Clear()
    {
        _pools.Clear();
    }

    void CreatePool(GameObject original)
    {
        Pool pool = new Pool(original);
        _pools.Add(original.name, pool);
    }
}
