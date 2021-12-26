using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour
{
    [SerializeField] OrbitController m_orbitController;
    [SerializeField] Whitney m_whitney;
	[SerializeField] float m_timeBetweenRandomizations = 60.0f;

	protected float m_timeSinceLastRandomization = 0f;

    void Update()
    {
 		if(Input.GetMouseButtonDown(1))
		{
			m_timeSinceLastRandomization = 0f;
			m_whitney.SetWithRandomSettings();
			m_orbitController.ResetToInitialPosition();
		}
		else
		{
			if(m_timeSinceLastRandomization > m_timeBetweenRandomizations)
			{
				m_timeSinceLastRandomization = 0f;
				m_whitney.SetWithRandomSettings();
				m_orbitController.ResetToInitialPosition();
			}
			else
			{
				m_timeSinceLastRandomization += Time.deltaTime;
			}
		}
    }
}
