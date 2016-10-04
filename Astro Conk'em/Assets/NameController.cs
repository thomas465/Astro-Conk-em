using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class NameController : MonoBehaviour
{
    private char m_inputMin = 'A';
    private char m_inputMax = 'Z';
    private char m_currentSelection = 'A';
    public Text m_prevChar;
    public Text m_curChar;
    public Text m_nextChar;
    public Text m_nameField;

    public float m_moveTimer=0.0f;
    public float m_timeBetweenMove;

    public bool m_canAddChar = true;
    // Use this for initialization
    void Start ()
    {
        m_timeBetweenMove = 0.15f;
        m_curChar.text = m_inputMin.ToString();
        m_prevChar.text = m_inputMax.ToString();
        m_nextChar.text = ((char)(m_inputMin+1)).ToString();
        m_nameField.text = "";
    }

    //public void GetInput(string input)
    //{
    //    Debug.Log("Name: " + input);
    //    LocalHighScoreManager.g_instance.addPlayerScore(input, ScoreManager.score);
    //    LeaderBoardVisualsScript.singleton.Activate();
    //}
	
	// Update is called once per frame
	void Update ()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(horizontalInput) >= 0.4f)
        {
            if (m_moveTimer >= m_timeBetweenMove)
            {
                m_currentSelection += (char)Mathf.Ceil(horizontalInput);
                m_currentSelection = loopChar(m_currentSelection);
                m_curChar.text = m_currentSelection.ToString();

                //using increment/decrement all over the place b/c turns out c# is meany about chars
                m_prevChar.text = loopChar(--m_currentSelection).ToString();
                ++m_currentSelection;
                m_nextChar.text = loopChar(++m_currentSelection).ToString();
               -- m_currentSelection;

                m_moveTimer = 0.0f;
            }

            m_moveTimer += Time.deltaTime;
        }
        else
        {
            const float minDetect = 0.8f;
            //up to add letter
            if (verticalInput <= -minDetect && m_canAddChar)
            {
                m_canAddChar = false;
                m_nameField.text += m_currentSelection.ToString();
            }
            //down to finish
            else if (verticalInput >= minDetect)
            {
                LocalHighScoreManager.g_instance.addPlayerScore(m_nameField.text, ScoreManager.score);
                LeaderBoardVisualsScript.singleton.Activate();
                this.enabled = false;
            }
        }

        if (Mathf.Abs(verticalInput) <= 0.1f) m_canAddChar = true;
    }
    public char loopChar(char _ch)
    {
       return _ch > m_inputMax ? m_inputMin : _ch < m_inputMin ? m_inputMax : _ch;
    }
}
