using UnityEngine;
using System.Collections;

public class TorchWall : MonoBehaviour {

    public Light TorchLeft;
    public Light TorchRight;

    public float MaxIntesity = 0.8f;
    public float MinIntesity = 0.5f;

    private float LeftTorchTargetIntesity;
    private float RightTorchTargetIntesity;
    private float light;

	// Use this for initialization
	void Start () {
        LeftTorchTargetIntesity = Random.Range(MinIntesity, MaxIntesity);
        RightTorchTargetIntesity = Random.Range(MinIntesity, MaxIntesity);
	}
	
	// Update is called once per frame
	void Update () {
        if (TorchLeft.intensity != LeftTorchTargetIntesity)
        {
            TorchLeft.intensity = Mathf.Lerp(TorchLeft.intensity, LeftTorchTargetIntesity, Mathf.Abs(Mathf.Sin(Time.time)));
            TorchRight.intensity = Mathf.Lerp(TorchRight.intensity, RightTorchTargetIntesity, Mathf.Abs(Mathf.Sin(Time.time)));
            
        } 
        else
        {
            LeftTorchTargetIntesity = Random.Range(MinIntesity, MaxIntesity);
            RightTorchTargetIntesity = Random.Range(MinIntesity, MaxIntesity);
        }        
	}
}
