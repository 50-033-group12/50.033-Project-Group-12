using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPainter : MonoBehaviour
{
    private Dictionary<Material, Material> _materialMap;
    private List<MeshRenderer> _meshRenderers;

    public void CloneMaterials()
    {
        _materialMap = new Dictionary<Material, Material>();
        _meshRenderers = new List<MeshRenderer>();
        foreach (var o in transform.FindObjectsWithTag("ChangeColour"))
        {
            var meshRenderer = o.GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                Debug.LogWarning("GameObject has changeColor tag but no MeshRenderer: " + o.name);
                continue;
            }

            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                var m = meshRenderer.materials[i];
                if (!_materialMap.ContainsKey(m))
                {
                    Material materialClone = new Material(m);
                    _materialMap[m] = materialClone;
                }

                meshRenderer.materials[i] = _materialMap[m];
            }
                _meshRenderers.Add(meshRenderer);
        }
    }

    public void PaintTeamColors(Color color)
    {
        foreach (var o in transform.FindObjectsWithTag("ChangeColour"))
        {
            var meshRenderer = o.GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                Debug.LogWarning("GameObject has changeColor tag but no MeshRenderer: " + o.name);
                continue;
            }

            foreach (Material m in meshRenderer.materials)
            {
                m.color = color;
            }
            meshRenderer.material.color = color;
            meshRenderer.UpdateGIMaterials();
        }
        
        foreach (Material m in _materialMap.Values)
        {
            m.color = color;
        }

        foreach (MeshRenderer meshRenderer in _meshRenderers)
        {
            
            meshRenderer.UpdateGIMaterials();
        }
    }
}