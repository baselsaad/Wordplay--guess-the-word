using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialsManager : MonoBehaviour
{
    [SerializeField] private Square m_wrongLetter, m_rightLetter, m_wrongPosition;

    void Start()
    {
        m_rightLetter.SetColor(GameManager.s_instance.GetRightColor());
        m_wrongPosition.SetColor(GameManager.s_instance.GetWrongPositionColor());
        m_wrongLetter.SetColor(GameManager.s_instance.GetWrongColor());
    }
}
