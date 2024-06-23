using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageUI : MonoBehaviour
{
    public static DamageUI Instance { get; private set; }

    private class ActiveText
    {
        public TextMeshProUGUI UIText;
        public float maxTime;
        public float Timer;
        public Vector3 UnitPosition;

        public void MoveText(Camera camera)
        {
            float delta = 1.0f - (Timer / maxTime);
            Vector3 pos = UnitPosition + new Vector3(delta, delta, 0.0f);
            pos = camera.WorldToScreenPoint(pos);
            pos.z = 0.0f;

            UIText.transform.position = pos;
        }
    }

    public TextMeshProUGUI m_TextPrefab;

    const int POOL_SIZE = 64;

    Queue<TextMeshProUGUI> m_TextPool = new Queue<TextMeshProUGUI>();
    List<ActiveText> m_ActiveText = new List<ActiveText>();

    private Camera m_Camera;
    private Transform m_Transform;

    private void Awake()
    {
        Instance = this;
        m_Camera = Camera.main;
        m_Transform = transform;

        for (int i = 0; i < POOL_SIZE; i++)
        {
            TextMeshProUGUI temp = Instantiate(m_TextPrefab, m_Transform);
            temp.gameObject.SetActive(false);
            m_TextPool.Enqueue(temp);
        }
    }

    private void Update()
    {
        for (int i = 0; i < m_ActiveText.Count; i++)
        {
            ActiveText at = m_ActiveText[i];
            at.Timer -= Time.deltaTime;

            if (at.Timer <= 0.0f)
            {
                at.UIText.gameObject.SetActive(false);
                m_TextPool.Enqueue(at.UIText);
                m_ActiveText.RemoveAt(i);
                --i;
            }
            else
            {
                var color = at.UIText.color;
                color.a = at.Timer / at.maxTime;
                at.UIText.color = color;

                at.MoveText(m_Camera);
            }
        }
    }

    public void AddText(int amount, Vector3 UnitPos)
    {
        if (m_TextPool.Count == 0) return;

        var t = m_TextPool.Dequeue();
        t.text = amount.ToString();
        t.gameObject.SetActive(true);
        t.transform.position = m_Camera.WorldToScreenPoint(UnitPos);

        ActiveText at = new ActiveText() { UIText = t, maxTime = 1.0f, Timer = 1.0f, UnitPosition = UnitPos };
        m_ActiveText.Add(at);
    }
}
