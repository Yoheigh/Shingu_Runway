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
        // 이미 가지고 있는 material일 경우
        if(savedMaterial.ContainsKey(_id))
        {
            return savedMaterial[_id];
        }

        Debug.Log("새로운 마테리얼을 추가합니다.");

        Material material;
        TextureData textureData = Managers.Data.GetTextureData(_id);

        if(textureData == null)
        {
            Debug.Log($"해당하는 material이 없습니다 : {_id}");
            return savedMaterial[0];
        }

        // 미리 데이터에 만들어 등록시켜 놓은 material이 있을 경우
        if(textureData.material != null)
        {
            // 해당 material을 베이스로 새로 생성
            material = new Material(textureData.material);
            savedMaterial.Add(_id, material);

            return material;
        }

        material = new Material(_default);

        // shaderKeywords를 직접 추가해줘야 다른 map에 접근할 수 있음
        material.shaderKeywords = material.GetTexturePropertyNames();

        SetTextureCheck(material, "_BaseMap", textureData.main);     // Base Map

        // 저해상도 텍스처 타일링 조절
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
