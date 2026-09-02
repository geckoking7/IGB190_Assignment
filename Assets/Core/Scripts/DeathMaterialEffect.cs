
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DeathMaterialEffect : MonoBehaviour
{
    private float loadTime;
    Material[] newMats;

    List<Material> allMaterials = new List<Material>();

    private Material materialToUse;

    public void SetMaterial (Material material)
    {
        materialToUse = material;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadTime = Time.time;
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (var rend in renderers)
        {
            if (rend == null) continue;

            Material[] originalMats = rend.sharedMaterials;
            newMats = new Material[originalMats.Length];

            for (int i = 0; i < originalMats.Length; i++)
            {
                var src = originalMats[i];
                if (src == null) continue;

                // Clone your reference dissolve material
                Material dst = null;
                if (materialToUse != null)
                {
                    dst = new Material(materialToUse);
                    
                }
                else
                {
                    dst = new Material(GameManager.assets.dissolveMaterial);
                } 

                // Copy main Standard Shader properties for visual consistency
                if (src.HasProperty("_MainTex"))
                    dst.SetTexture("_MainTex", src.GetTexture("_MainTex"));
                if (src.HasProperty("_Color"))
                    dst.SetColor("_Color", src.GetColor("_Color"));
                if (src.HasProperty("_Glossiness"))
                    dst.SetFloat("_Glossiness", src.GetFloat("_Glossiness"));
                if (src.HasProperty("_Metallic"))
                    dst.SetFloat("_Metallic", src.GetFloat("_Metallic"));
                if (src.HasProperty("_BumpMap"))
                    dst.SetTexture("_BumpMap", src.GetTexture("_BumpMap"));
                if (src.HasProperty("_EmissionColor"))
                    dst.SetColor("_EmissionColor", src.GetColor("_EmissionColor"));
                if (src.HasProperty("_EmissionMap"))
                    dst.SetTexture("_EmissionMap", src.GetTexture("_EmissionMap"));

                // Always start undissolved
                dst.SetFloat("_DissolveAmount", 0f);

                newMats[i] = dst;
                allMaterials.Add(dst);
            }

            rend.sharedMaterials = newMats;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Material m in allMaterials)
        {
            m.SetFloat("_DissolveAmount", Mathf.Clamp01((Time.time - loadTime) * 0.5f - 1.0f));
        }
    }
}
