using UnityEngine;

// if you see a magic number 100, its the max number of objects
public class Whitney : MonoBehaviour
{
	protected const int k_maxNumberOfObjects = 500;  // magic number alert! make sure to update m_numberOfObjects Range(max)


	[Header("Prefab")]
	[SerializeField] protected GameObject m_objectPrefab;

	[Header("Whitney")]
	[SerializeField] [Range(1, 500)] protected int m_numberOfObjects = 50; // magic number alert! make sure Range(max) matches k_maxNumberOfObjects
	[SerializeField] [Range(0.5f, 5.0f)] protected float m_circleSize = 1.0f;
	[SerializeField] [Range(0.001f, 0.01f)] protected float m_speedScaler = 0.01f;
	[SerializeField] protected bool m_disableHarmonicScaling = false;

	[Header("3D Tube Mode")]
	[SerializeField] protected bool m_3dMode = false;
	[SerializeField] [Range(0.001f, 1f)] protected float m_tubeSpacing = 1.0f;
	[SerializeField] protected bool m_reverseZOrderIn3dMode = true;

	[Header("Scale")]
	[SerializeField] [Range(0.001f, 1f)] protected float m_globalScale = 0.25f;
	[SerializeField] [Range(0.01f, 1f)] protected float m_baseScaleX = 1.0f;
	[SerializeField] [Range(0.01f, 1f)] protected float m_baseScaleY = 1.0f;
	[SerializeField] [Range(0.01f, 1f)] protected float m_baseScaleZ = 1.0f;

	[Header("Color")]
	[SerializeField] [Range(0.001f, 1f)] protected float m_colorHueSpeed = 1f;
	[SerializeField] [Range(0f, 1f)] protected float m_colorSaturation = 1f;  // boring
	[SerializeField] [Range(0f, 1f)] protected float m_colorBrightness = 1f;  // boring

	[SerializeField] [Range(0f, 1f)] protected float m_colorAlpha = 0.75f;
	//[SerializeField] [Range(0f, 1f)] protected float m_metallic = 0.7f;
	//[SerializeField] [Range(0f, 1f)] protected float m_smoothness = 0.7f;

	[Header("Rotation")]
	[SerializeField] protected bool m_rotateX = false;
	[SerializeField] protected bool m_rotateY = false;
	[SerializeField] protected bool m_rotateZ = false;
	[SerializeField] [Range(0f, 360f)] protected float m_rotationX = 0.0f;
	[SerializeField] [Range(0f, 360f)] protected float m_rotationY = 0.0f;
	[SerializeField] [Range(0f, 360f)] protected float m_rotationZ = 0.0f;

	protected GameObject[] m_objects;
	protected Renderer[] m_objectRenderers;
	//protected Light[] m_lights;
	protected float m_phase = 0.0f;
	protected bool m_was3dModeOnLastFrame;


	void Start()
	{
		m_was3dModeOnLastFrame = m_3dMode;

		m_objects = new GameObject[k_maxNumberOfObjects];
		m_objectRenderers = new Renderer[k_maxNumberOfObjects];
		//m_lights = new Light[k_maxNumberOfObjects];
		for(int i = 0; i < m_objects.Length; i++)
		{
			m_objects[i] = GameObject.Instantiate(m_objectPrefab, this.transform);
			m_objects[i].SetActive(false);
			m_objectRenderers[i] = m_objects[i].GetComponent<Renderer>();
			m_objectRenderers[i].sortingOrder = i;
		//	m_lights[i] = m_objects[i].GetComponent<Light>();
		}
	}

	protected void On3dModeChanged()
	{
		for(int i = 0; i < m_objectRenderers.Length; i++)
		{
			m_objectRenderers[i].sortingOrder = m_3dMode ? 0 : i;
		}
	}

	private void Update()
	{
#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.S))
		{
			SaveCurrentWhitneyDataToFile();
		}
#endif

		Vector3 baseScale = new Vector3(m_baseScaleX, m_baseScaleY, m_baseScaleZ) * m_globalScale;

		for(int i = 0; i < m_objects.Length; i++)
		{
			if(i > m_numberOfObjects - 1)
			{
				m_objects[i].SetActive(false);
				continue;
			}

			m_objects[i].SetActive(true);

			float scaledPhase = m_phase * (i + 1);  // add 1 so there arent any objects that arent moving

			// position
			Vector3 position = new Vector3(Mathf.Cos(scaledPhase) * m_circleSize, Mathf.Sin(scaledPhase) * m_circleSize, 0.0f) + this.transform.position;
			if(m_3dMode)
			{
				if(m_reverseZOrderIn3dMode)
				{
					position.z += m_tubeSpacing * (m_numberOfObjects - i);
				}
				else
				{
					position.z += m_tubeSpacing * i;
				}
			}
			m_objects[i].transform.position = position;

			// color
			Color color = Color.HSVToRGB(scaledPhase * m_colorHueSpeed % 1.0f, m_colorSaturation, m_colorBrightness);
			color.a = m_colorAlpha;
			m_objectRenderers[i].material.color = color;
			//m_lights[i].color = color;
			//m_objectRenderers[i].material.SetFloat("_Metallic", m_metallic);
			//m_objectRenderers[i].material.SetFloat("_Smoothness", m_smoothness);
			//m_objects[i].GetComponent<Renderer>().material.SetColor("_EmissionColor",color);

			// scale
			if(m_3dMode)
			{
				m_objects[i].transform.localScale = baseScale;
			}
			else
			{
				const float harmonicScalingOffset = 0.02f;
				m_objects[i].transform.localScale = baseScale * i * harmonicScalingOffset;
			}

			// rotation (probably dont need to modulo)
			Quaternion rotation = Quaternion.Euler(m_rotateX ? (scaledPhase * 360f + m_rotationX) % 360f : m_rotationX,
				m_rotateY ? (scaledPhase * 360f + m_rotationY) % 360f : m_rotationY,
				m_rotateZ ? (scaledPhase * 360f + m_rotationZ) % 360f : m_rotationZ);
			m_objects[i].transform.rotation = rotation;
		}

		if(m_3dMode != m_was3dModeOnLastFrame)
		{
			On3dModeChanged();
			m_was3dModeOnLastFrame = m_3dMode;
		}

		m_phase += Time.deltaTime * m_speedScaler * (k_maxNumberOfObjects / m_numberOfObjects);
	}

	public void SetFromWhitneyData(WhitneyData _data)
	{
		//m_objectPrefab;

		m_numberOfObjects = _data.m_numberOfObjects;
		m_circleSize = _data.m_circleSize;
		m_speedScaler = _data.m_speedScaler;

		m_globalScale = _data.m_globalScale;
		m_baseScaleX = _data.m_baseScaleX;
		m_baseScaleY = _data.m_baseScaleY;
		m_baseScaleZ = _data.m_baseScaleZ;

		m_colorHueSpeed = _data.m_colorHueSpeed;
		m_colorSaturation = _data.m_colorSaturation;
		m_colorBrightness = _data.m_colorBrightness;
		m_colorAlpha = _data.m_colorAlpha;

		m_rotateX = _data.m_rotateX;
		m_rotateY = _data.m_rotateY;
		m_rotateZ = _data.m_rotateZ;
		m_rotationX = _data.m_rotationX;
		m_rotationY = _data.m_rotationY;
		m_rotationZ = _data.m_rotationZ;

		m_3dMode = _data.m_3dMode;
		m_tubeSpacing = _data.m_tubeSpacing;
	}

	public WhitneyData GetRandomWhitneyData()
	{
		WhitneyData randomData = ScriptableObject.CreateInstance<WhitneyData>();

		randomData.m_3dMode = (Random.Range(0, 2) == 0);
		randomData.m_tubeSpacing = Random.Range(0.01f, 0.5f);

		if(randomData.m_3dMode)
		{
			randomData.m_numberOfObjects = Random.Range(75, k_maxNumberOfObjects);
			randomData.m_circleSize = Random.Range(2f, 3f);
		}
		else
		{
			randomData.m_numberOfObjects = Random.Range(25, k_maxNumberOfObjects);
			randomData.m_circleSize = Random.Range(1f, 2f);
		}

		randomData.m_speedScaler = Random.Range(0.003f, 0.008f);

		randomData.m_globalScale = Random.Range(0.5f, 1f);

		if(Random.Range(0, 20) == 0)  // every once in a while, make a perfect orb
		{
			float randomScale = Random.Range(1f, 2f);
			randomData.m_baseScaleX = randomData.m_baseScaleY = randomData.m_baseScaleZ = randomScale;
		}
		else
		{
			randomData.m_baseScaleX = Random.Range(0.1f, 1f);
			randomData.m_baseScaleY = Random.Range(0.1f, 1f);
			randomData.m_baseScaleZ = Random.Range(0.1f, 1f);
		}

		randomData.m_colorHueSpeed = Random.Range(0.01f, 0.75f);
		randomData.m_colorSaturation = 1f;
		randomData.m_colorBrightness = 1f;

		randomData.m_colorAlpha = Random.Range(0.25f, 1f);

		randomData.m_rotateX = Random.Range(0, 3) == 0;
		randomData.m_rotateY = Random.Range(0, 3) == 0;
		randomData.m_rotateZ = Random.Range(0, 3) == 0;
		randomData.m_rotationX = Random.Range(0f, 360f);
		randomData.m_rotationY = Random.Range(0f, 360f);
		randomData.m_rotationZ = Random.Range(0f, 360f);

		return randomData;
	}

	public void SetWithRandomSettings()
	{
		SetFromWhitneyData(GetRandomWhitneyData());
	}

	public void SaveCurrentWhitneyDataToFile()
	{
		WhitneyData _data = ScriptableObject.CreateInstance<WhitneyData>();

		_data.m_numberOfObjects = m_numberOfObjects;
		_data.m_circleSize = m_circleSize;
		_data.m_speedScaler = m_speedScaler;

		_data.m_globalScale = m_globalScale;
		_data.m_baseScaleX = m_baseScaleX;
		_data.m_baseScaleY = m_baseScaleY;
		_data.m_baseScaleZ = m_baseScaleZ;

		_data.m_colorHueSpeed = m_colorHueSpeed;
		_data.m_colorSaturation = m_colorSaturation;
		_data.m_colorBrightness = m_colorBrightness;
		_data.m_colorAlpha = m_colorAlpha;

		_data.m_rotateX = m_rotateX;
		_data.m_rotateY = m_rotateY;
		_data.m_rotateZ = m_rotateZ;
		_data.m_rotationX = m_rotationX;
		_data.m_rotationY = m_rotationY;
		_data.m_rotationZ = m_rotationZ;

		_data.m_3dMode = m_3dMode;
		_data.m_tubeSpacing = m_tubeSpacing;

		string prefix = "Assets/WhitneyDatas/randomWhitney_";
		int fileNumber = 0;
		string extension = ".asset";
		while(true)
		{
			fileNumber++;
			string filePath = prefix + fileNumber + extension;
			if(!UnityEngine.Windows.File.Exists(prefix + fileNumber.ToString() + extension))
			{
				UnityEditor.AssetDatabase.CreateAsset(_data, filePath);
				return;
			}
		}
	}
}

