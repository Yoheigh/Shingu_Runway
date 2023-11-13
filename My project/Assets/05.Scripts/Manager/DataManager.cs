using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    private AlembicDatabaseSO dataSO;
    private Dictionary<int, AlembicData> alembicDic = new Dictionary<int, AlembicData>();
    private Dictionary<int, TextureData> textureDic = new Dictionary<int, TextureData>();

    public bool GetDataFromAlembicScene = true;

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "PlayScene")
        {
            var op = SceneManager.LoadSceneAsync("AlembicScene", LoadSceneMode.Additive);
            op.completed += (obj) =>
            {
                Debug.Log("씬 로드 완ㄹ료!");
                LoadAllAlembicData();
                Managers.Player.ChangeAlembicObject(115103);
            };
        }
    }

    public bool IsDataAvailable()
    {
        if (dataSO == null)
            return false;
        else
            return true;
    }

    public void FindAlembicAll()
    {
        for (int i = 0; i < alembicDic.Count; i++)
        {

        }
    }

    public AlembicData GetAlembicData(int index)
    {
        alembicDic.TryGetValue(index, out AlembicData value);
        return value;
    }

    public TextureData GetTextureData(int index)
    {
        textureDic.TryGetValue(index, out TextureData value);
        return value;
    }

    public void LoadAllAlembicData()
    {
        // 씬에 넣기만 하면 SO에 알아서 들어가게 처리
        if (GetDataFromAlembicScene)
        {
            Debug.Log("AlembicScene에 배치된 오브젝트를 찾아서 저장합니다.");

            GameObject[] alembicObjects = GameObject.FindGameObjectsWithTag("PreloadAlembic");
            string fullName = string.Empty;

            dataSO.alembics = new AlembicData[alembicObjects.Length];

            for (int i = 0; i < alembicObjects.Length; i++)
            {
                fullName = alembicObjects[i].name;

                string[] names = fullName.Split('_');
                int id = Int32.Parse(names[0]);

                if (alembicDic.ContainsKey(id))
                {
                    Debug.Log($"해당 index가 이미 존재합니다 : {id}");

                    // 오브젝트 풀링 등록을 아예 안하는 과정이 필요
                    Destroy(alembicObjects[i]);
                    continue;
                }

                AlembicData data = new AlembicData(id,
                                                   names[1],
                                                   (ClothesType)Int32.Parse($"{names[0][1]}"),
                                                   (SequenceType)Int32.Parse($"{names[0][2]}{names[0][3]}"),
                                                   null,
                                                   null);

                // 데이터 저장
                dataSO.alembics[i] = data;

                // 씬에 배치된 프리팹 등록
                data.prefab = alembicObjects[i];
                bool isAssigned = data.prefab == null ? false : true;

                alembicDic.Add(data.id, data);
                Debug.Log($"{data.id} 추가, 프리팹 연결 {isAssigned}");
            }
        }
        else
        {
            for (int i = 0; i < dataSO.alembics.Length; i++)
            {
                // 이거 이상하게 자꾸 까먹음. 비동기 포함된 for문 돌릴 때는 i값 캐싱해줘야 함
                int index = i;

                AlembicData data = new AlembicData(dataSO.alembics[index].id,
                                                   dataSO.alembics[index].name,
                                                   dataSO.alembics[index].type,
                                                   dataSO.alembics[index].sequence,
                                                   dataSO.alembics[index].description,
                                                   dataSO.alembics[index].image);

                GameObject go = GameObject.Find($"{data.id}_{data.name}");

                if (go == null)
                {
                    Debug.LogError($"{data.id}_{data.name} 이라는 이름의 Alembic이 씬에 없습니다.");
                    continue;
                }

                data.prefab = go;
                bool isAssigned = data.prefab == null ? false : true;

                alembicDic.Add(data.id, data);
                Debug.Log($"{data.id} 추가, 프리팹 연결 {isAssigned}");
            }
        }


        for (int i = 0; i < dataSO.textures.Length; i++)
        {
            textureDic.Add(dataSO.textures[i].id, dataSO.textures[i]);
            Debug.Log($"{dataSO.textures[i].id}. {dataSO.textures[i].name} 추가");
        }
    }
}
