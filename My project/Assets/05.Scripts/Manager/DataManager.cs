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
                Debug.Log("�� �ε� �Ϥ���!");
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
        // ���� �ֱ⸸ �ϸ� SO�� �˾Ƽ� ���� ó��
        if (GetDataFromAlembicScene)
        {
            Debug.Log("AlembicScene�� ��ġ�� ������Ʈ�� ã�Ƽ� �����մϴ�.");

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
                    Debug.Log($"�ش� index�� �̹� �����մϴ� : {id}");

                    // ������Ʈ Ǯ�� ����� �ƿ� ���ϴ� ������ �ʿ�
                    Destroy(alembicObjects[i]);
                    continue;
                }

                AlembicData data = new AlembicData(id,
                                                   names[1],
                                                   (ClothesType)Int32.Parse($"{names[0][1]}"),
                                                   (SequenceType)Int32.Parse($"{names[0][2]}{names[0][3]}"),
                                                   null,
                                                   null);

                // ������ ����
                dataSO.alembics[i] = data;

                // ���� ��ġ�� ������ ���
                data.prefab = alembicObjects[i];
                bool isAssigned = data.prefab == null ? false : true;

                alembicDic.Add(data.id, data);
                Debug.Log($"{data.id} �߰�, ������ ���� {isAssigned}");
            }
        }
        else
        {
            for (int i = 0; i < dataSO.alembics.Length; i++)
            {
                // �̰� �̻��ϰ� �ڲ� �����. �񵿱� ���Ե� for�� ���� ���� i�� ĳ������� ��
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
                    Debug.LogError($"{data.id}_{data.name} �̶�� �̸��� Alembic�� ���� �����ϴ�.");
                    continue;
                }

                data.prefab = go;
                bool isAssigned = data.prefab == null ? false : true;

                alembicDic.Add(data.id, data);
                Debug.Log($"{data.id} �߰�, ������ ���� {isAssigned}");
            }
        }


        for (int i = 0; i < dataSO.textures.Length; i++)
        {
            textureDic.Add(dataSO.textures[i].id, dataSO.textures[i]);
            Debug.Log($"{dataSO.textures[i].id}. {dataSO.textures[i].name} �߰�");
        }
    }
}
