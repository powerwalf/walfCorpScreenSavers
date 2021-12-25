using UnityEngine;

// if you see a magic number 100, its the max number of objects
public class Whitney : MonoBehaviour
{
	protected const int k_maxNumberOfObjects = 100;  // magic number alert! make sure to update m_numberOfObjects Range(max)

	[Header("Prefab")]
	[SerializeField] protected GameObject m_objectPrefab;

	[Header("Whitney")]
	[SerializeField] [Range(1, 100)] protected int m_numberOfObjects = 50; // magic number alert! make sure Range(max) matches k_maxNumberOfObjects
	[SerializeField] [Range(0.5f, 2.0f)] protected float m_circleSize = 1.0f;
	[SerializeField] [Range(0.001f, 0.1f)] protected float m_speedScaler = 0.01f;
	[SerializeField] protected bool m_3dMode = false;
	[SerializeField] [Range(0.01f, 1f)] protected float m_tubeSpacing = 1.0f;

	[Header("Scale")]
	[SerializeField] [Range(0.001f, 1f)] protected float m_globalScale = 0.25f;
	[SerializeField] [Range(0.1f, 1f)] protected float m_baseScaleX = 1.0f;
	[SerializeField] [Range(0.1f, 1f)] protected float m_baseScaleY = 1.0f;
	[SerializeField] [Range(0.1f, 1f)] protected float m_baseScaleZ = 1.0f;

	[Header("Color")]
	[SerializeField] [Range(0.001f, 1f)] protected float m_colorHueSpeed = 1f;
	[SerializeField] [Range(0f, 1f)] protected float m_colorSaturation = 1f;
	[SerializeField] [Range(0f, 1f)] protected float m_colorBrightness = 1f;

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
	protected float m_timeSinceLastPhaseSync = 0f;
	protected const float k_timeBetweenPhaseSyncs = 10.0f;


	void Start()
	{
		m_objects = new GameObject[k_maxNumberOfObjects];
		m_objectRenderers = new Renderer[k_maxNumberOfObjects];
		//m_lights = new Light[k_maxNumberOfObjects];
		for(int i = 0; i < m_objects.Length; i++)
		{
			m_objects[i] = GameObject.Instantiate(m_objectPrefab, this.transform);
			m_objects[i].SetActive(false);
			m_objectRenderers[i] = m_objects[i].GetComponent<Renderer>();
		//	m_lights[i] = m_objects[i].GetComponent<Light>();
		}
	}

	private void Update()
	{
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
				position.z += m_tubeSpacing * i;
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

		m_phase += Time.deltaTime * m_speedScaler * (k_maxNumberOfObjects / m_numberOfObjects);

		m_timeSinceLastPhaseSync += Time.deltaTime;
	}
}

