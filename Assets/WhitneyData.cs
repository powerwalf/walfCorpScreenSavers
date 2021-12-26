using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhitneyData : ScriptableObject
{
	[Header("Prefab")]
	public GameObject m_objectPrefab;

	[Header("Whitney")]
	[Range(1, 100)] public int m_numberOfObjects = 50; // magic number alert! make sure Range(max) matches k_maxNumberOfObjects
	[Range(0.5f, 2.0f)] public float m_circleSize = 1.0f;
	[Range(0.001f, 0.1f)] public float m_speedScaler = 0.01f;

	[Header("Scale")]
	[Range(0.001f, 1f)] public float m_globalScale = 0.25f;
	[Range(0.1f, 1f)] public float m_baseScaleX = 1.0f;
	[Range(0.1f, 1f)] public float m_baseScaleY = 1.0f;
	[Range(0.1f, 1f)] public float m_baseScaleZ = 1.0f;

	[Header("Color")]
	[Range(0.001f, 1f)] public float m_colorHueSpeed = 1f;
	[Range(0f, 1f)] public float m_colorSaturation = 1f;
	[Range(0f, 1f)] public float m_colorBrightness = 1f;
	[Range(0f, 1f)] public float m_colorAlpha = 0.75f;
	//[SerializeField] [Range(0f, 1f)] protected float m_metallic = 0.7f;
	//[SerializeField] [Range(0f, 1f)] protected float m_smoothness = 0.7f;

	[Header("Rotation")]
	public bool m_rotateX = false;
	public bool m_rotateY = false;
	public bool m_rotateZ = false;
	[Range(0f, 360f)] public float m_rotationX = 0.0f;
	[Range(0f, 360f)] public float m_rotationY = 0.0f;
	[Range(0f, 360f)] public float m_rotationZ = 0.0f;

	[Header("3D Mode")]
	public bool m_3dMode = false;
	[Range(0.01f, 1f)] public float m_tubeSpacing = 1.0f;
}
