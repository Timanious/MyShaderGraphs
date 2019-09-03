using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumetricCloudsGenerator : MonoBehaviour
{
    public int      stackSize   = 20;
    public float    cloudHeight = 1f;
    public Mesh     quadMesh;
    public Material cloudMaterial;

    [GradientUsage(true)]
    public Gradient gradient;

    public int      layer;
    public Camera   camera;
    public bool     castShadows      = false;
    public bool     receiveShadows   = true;
    public bool     useGpuInstancing = false;

    private Matrix4x4   matrix;
    private Matrix4x4[] matrices;
    private float       offset;
    private Vector3     startPosition;
    
    void Update()
    {
        cloudMaterial.SetFloat("_cloudHeight",  cloudHeight);
        cloudMaterial.SetFloat("_topYValue",    transform.position.y + (cloudHeight/2f));
        cloudMaterial.SetFloat("_middleYValue", transform.position.y);
        cloudMaterial.SetFloat("_bottomYValue", transform.position.y - (cloudHeight/2f));

        startPosition = transform.position + (Vector3.up * cloudHeight/2f);

        // offset = cloudHeight / ((float)stackSize);

        if(stackSize == 1)
        {
            offset        = 0;
            startPosition = transform.position;
        }
        else if(stackSize == 2)
        {
            offset = cloudHeight;
        }
        else
        {
            offset = cloudHeight / ((float)stackSize-1);
        }
    

        if (useGpuInstancing) // initialize matrix array
            matrices = new Matrix4x4[stackSize];

        for (int i = 0; i < stackSize; i++)
        {
            matrix = Matrix4x4.TRS(startPosition + (Vector3.down * offset * (float)i), transform.rotation, transform.localScale);

            if (useGpuInstancing)
            {
                matrices[i] = matrix; // build the matrices array if using GPU instancing
                // matrices[horizontalStackSize-1-i] = matrix; // build the matrices array if using GPU instancing in reverse
            }
            else
            {
                MaterialPropertyBlock properties = new MaterialPropertyBlock();
                Graphics.DrawMesh(quadMesh, matrix, cloudMaterial, layer, camera, 0, null, castShadows, receiveShadows, false); // otherwise just draw it now
            }
        }

        //// "Use this function in situations where you want to draw the same mesh for a particular amount of times using an instanced shader. 
        ////  Meshes are not further culled by the view frustum or baked occluders, nor sorted for transparency or z efficiency." -Unity Documentation
        //// This can cause problemd with the draw order so the clouds looks inverted when viewed from the top down, or when the matrices array is filled in reverse, when viewed from below.
        if (useGpuInstancing) // draw the built matrix array 
        {
            UnityEngine.Rendering.ShadowCastingMode shadowCasting = UnityEngine.Rendering.ShadowCastingMode.Off;
            // UnityEngine.Rendering.ShadowCastingMode s = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
            if (castShadows)
                shadowCasting = UnityEngine.Rendering.ShadowCastingMode.On;

            Graphics.DrawMeshInstanced(quadMesh, 0, cloudMaterial, matrices, stackSize, null, shadowCasting, receiveShadows, layer, camera);
        }
    }
}

