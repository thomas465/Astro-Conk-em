using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LocalHighScoreManager : MonoBehaviour
{
    public static LocalHighScoreManager g_instance;
    public struct Pair
    {
       public Pair(float _score, string _name) { score = _score; name = _name; }

       public float score;
       public string name;
    }
    private Pair[] m_leaderBoard;
    private int m_maxScores = 5;
    private int m_numScores = 0;
    private bool m_savedDataUpToDate = false;

    //private because only needs to be called once, when the game starts
    private void loadScores()
    {
        m_savedDataUpToDate = true;
        int index = 0;
        StreamReader fsreader = new StreamReader("scores.txt");
        try
        {
            while (fsreader.Peek() != -1) 
            {
                float f;
                float.TryParse(fsreader.ReadLine(), out f);
                m_leaderBoard[index].score = f;
                m_leaderBoard[index].name = fsreader.ReadLine();
                Debug.Log(m_leaderBoard[index].name);
                Debug.Log(m_leaderBoard[index].score);

                ++index;         //index update for next loop    
            }
        }
        catch
        {}
        finally
        {
            fsreader.Close();
        }
        m_numScores = index;
    }

    //at the moment it'll just save after a player has been added but
    //if you want it to save at the end of the level instead 
    //then ask Orlando ;)
    private void saveScores()
    {
        //no need to save if the cache is up to date
        if (m_savedDataUpToDate == false)
        {
            StreamWriter fswriter = new StreamWriter("scores.txt");
            for (int i=0; i < m_numScores; ++i)
            {
                fswriter.WriteLine(m_leaderBoard[i].score);
                fswriter.WriteLine(m_leaderBoard[i].name);
            }
     
            fswriter.Close();
            m_savedDataUpToDate = true;
        }
    }

    //if you want to check the player score against the highest score.
    //usefull if you want an in-game display when they are now the champion (during gameplay)
    public bool scoreIsHighestScore(float _score)
    {
        //return true if the score is better than the worst OR there is a free slot!
        return m_numScores == 0 || _score >= m_leaderBoard[0].score;
    }

    //This is checked against when adding a player to the leaderboard
    //but you can use it if you want to let the player know they'll be on the leaderboard
    public bool scoreIsHighScore(float _score)
    {
        //return true if the score is better than the worst OR there is a free slot!
        return m_numScores < m_maxScores || _score >= m_leaderBoard[m_numScores-1].score;
    }

	//returns true if the player placed on the scoreboard
    public bool addPlayerScore(string _name, float _score)
    {
		return addPlayerScore(new Pair(_score, _name));
	}
	
    //returns true if the player placed on the scoreboard
    public bool addPlayerScore(Pair _player)
    {
        //early out if they didn't place
        if (scoreIsHighScore(_player.score) == false) return false;

        bool placed = false;
        for (int i  = 0; i <  m_numScores; ++i)
        {
            if (_player.score >= m_leaderBoard[i].score)
            {
                //add player to list here, reshuffle rest of list
                addAndShuffle(_player, i);
                if (m_numScores < m_maxScores) m_numScores++;
                placed = true;
                break;
            }
        }

        //if there is space and they didn't get on the list then add them
        if (!placed  && m_numScores < m_maxScores)
        {
            //add player to list at m_numScores +1
            m_leaderBoard[m_numScores++] = _player;
            placed = true;
            Debug.Log("appending score");
        }

        //if we have changed the board then it's
        m_savedDataUpToDate = m_savedDataUpToDate && !placed;

        if (placed) saveScores();
        return placed;
    }

    private void addAndShuffle(Pair _p, int _index)
    {
        Pair temp;
        Pair prev = _p;
        for (int i = _index; i < m_maxScores; ++i)
        {
            temp =  m_leaderBoard[i];
            m_leaderBoard[i] = prev;
            prev = temp;
        }
    }

	// Use this for initialization
	void Start ()
    {
        g_instance = this;
        m_leaderBoard = new Pair[m_maxScores];
        loadScores();
    }

    public int maxScores()
    {
        return m_maxScores;
    }
    public int numberOfScores()
    {
        return m_numScores;
    }

    public Pair getScore(int index)
    {
        return m_leaderBoard[index];
    }

    public Pair[] copyLeaderBoard()
    {
        return m_leaderBoard;
    }
	// Update is called once per frame
	void Update ()
    {
	
	}
}
