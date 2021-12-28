using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour
{
    [SerializeField] OrbitController m_orbitController;
    [SerializeField] Whitney m_whitney;
	[SerializeField] float m_timeBetweenRandomizations = 60.0f;
	[SerializeField] bool m_useWhitneyData = true;
	[SerializeField] WhitneyData[] m_whitneyDatas;

	List<uint> m_notShownWhitneyDataIndices = new List<uint>();

	protected float m_timeSinceLastRandomization = 0f;

	private void Start()
	{
		if(m_whitneyDatas == null || m_whitneyDatas.Length < 1)
		{
			m_useWhitneyData = false;
		}
		ShowRandomWhitney();
	}

	void ShowRandomWhitney()
	{
		m_timeSinceLastRandomization = 0f;
		m_orbitController.ResetToInitialPosition();

		if(m_useWhitneyData)
		{
			if(m_notShownWhitneyDataIndices.Count < 1)
			{
				for(uint i = 0; i < m_whitneyDatas.Length; i++)
				{
					m_notShownWhitneyDataIndices.Add(i);
				}
			}
			uint index = m_notShownWhitneyDataIndices[Random.Range(0, m_notShownWhitneyDataIndices.Count)];
			m_notShownWhitneyDataIndices.Remove(index);
			m_whitney.SetFromWhitneyData(m_whitneyDatas[index]);
		}
		else
		{
			m_whitney.SetWithRandomSettings();
		}
	}

	void Update()
    {
 		if(Input.GetMouseButtonDown(1))
		{
			ShowRandomWhitney();
		}
		else
		{
			if(m_timeSinceLastRandomization > m_timeBetweenRandomizations)
			{
				ShowRandomWhitney();
			}
			else
			{
				m_timeSinceLastRandomization += Time.deltaTime;
			}
		}
    }
}

