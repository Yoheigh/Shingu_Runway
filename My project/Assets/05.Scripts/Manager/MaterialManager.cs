using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public List<Material> materials;

    private Dictionary<int, Material> savedMaterial = new Dictionary<int, Material>();

    [SerializeField]
    private Material _default;

    public Material GetMaterial(int _id)
    {
        // �̹� ������ �ִ� material�� ���
        if(savedMaterial.ContainsKey(_id))
        {
            return savedMaterial[_id];
        }

        Debug.Log("���ο� ���׸����� �߰��մϴ�.");

        Material material;
        TextureData textureData = Managers.Data.GetTextureData(_id);

        if(textureData == null)
        {
            Debug.Log($"�ش��ϴ� material�� �����ϴ� : {_id}");
            return savedMaterial[0];
        }

        // �̸� �����Ϳ� ����� ��Ͻ��� ���� material�� ���� ���
        if(textureData.material != null)
        {
            // �ش� material�� ���̽��� ���� ����
            material = new Material(textureData.material);
            savedMaterial.Add(_id, material);

            return material;
        }

        material = new Material(_default);

        // shaderKeywords�� ���� �߰������ �ٸ� map�� ������ �� ����
        material.shaderKeywords = material.GetTexturePropertyNames();

        SetTextureCheck(material, "_BaseMap", textureData.main);     // Base Map

        // ���ػ� �ؽ�ó Ÿ�ϸ� ����
        if(textureData.main != null)
        {
            material.mainTextureScale = new Vector2(5, 5);
        }
        SetTextureCheck(material, "_BumpMap", textureData.normal);   // Normal Map
        SetTextureCheck(material, "_HeightMap", textureData.height);   // Height Map
        SetTextureCheck(material, "_OcclusionMap", textureData.occlusion);   // Occlusion Map

        material.SetFloat("_Smoothness", textureData.smoothnessValue);
        material.name = textureData.name;

        savedMaterial.Add(_id, material);

        return material;
    }

    public void SetTextureCheck(Material material, string _keyword, Texture value)
    {
        if (value != null)
        {
            material.SetTexture(_keyword, value);
        }
    }
}
