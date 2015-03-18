using UnityEngine;
using System.Collections;

public class ClothingPart : MonoBehaviour {

    public string name;
    public float CharacterHeightOffset = 0.0f;
    public string mirrorObjectName;
    public ClothingCategory ClothingType;
    public CharacterParts[] bodyOverlaps;
    public Transform[] LocalBones;
    public CharacterBones[] AttachmentBones;
    public Vector3[] BonePositionOffsets;
    public Vector3[] BoneRotationOffsets;
}