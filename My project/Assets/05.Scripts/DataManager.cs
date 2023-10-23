using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    private AlembicDatabaseSO dataSO;
    private Dictionary<int, AlembicData> data = new Dictionary<int, AlembicData>();

    private void Awake()
    {
        LoadAllAlembicData();
    }

    public AlembicData GetAlembicData(int index)
    {
        data.TryGetValue(index, out AlembicData value);
        return value;
    }

    public void LoadAllAlembicData()
    {
        for(int i = 0; i < dataSO.alembics.Length; i++)
        {
            data.Add(dataSO.alembics[i].id, dataSO.alembics[i]);
            Debug.Log($"{dataSO.alembics[i].id} Ãß°¡");
        }
    }
}
