using UnityEngine;
using System.Collections;

public class TerrainCircleInput : MonoBehaviour 
{
    public Camera m_camera;
    public Material[] m_terrainCircleMaterials;

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit))
            {
                for(int i=0;i<m_terrainCircleMaterials.Length;i++)
                    m_terrainCircleMaterials[i].SetVector("_circleLocation",hit.point);
            }
        }
    }
}