using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
[ExecuteInEditMode]
public class DirectionalIntensity : MonoBehaviour
{
    public Light directionalLight;
    public AnimationCurve brightnessCurve = new AnimationCurve(new Keyframe(0,0.01f),new Keyframe(0.45f,0.01f),new Keyframe(1,1));

    private void Awake() 
    {
        if(directionalLight.type != LightType.Directional)
        {
            Debug.Log("Light is not of type Diretional, setting it to directional.. ");
            directionalLight.type = LightType.Directional;
        }
    }

    void Update()
    {
        float angle = Vector3.Dot(Vector3.down,directionalLight.transform.forward.normalized); // range -1 <> 1
        angle = angle * .5f + .5f; // remap range to 0 <> 1
        directionalLight.intensity = brightnessCurve.Evaluate(angle);
    }
}
