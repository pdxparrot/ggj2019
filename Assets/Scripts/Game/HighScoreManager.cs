using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game
{
    public sealed class HighScoreManager : SingletonBehavior<HighScoreManager>
    {
        public struct HighScore : IComparable<HighScore>
        {
            public string playerName;

            public int score;

            public int CompareTo(HighScore other)
            {
                return score.CompareTo(other.score);
            }
        }

        private readonly SortedSet<HighScore> _highScores = new SortedSet<HighScore>();

        public IReadOnlyCollection<HighScore> HighScores => _highScores;

#region Unity Lifecycle
        private void Awake()
        {
            InitDebugMenu();
        }
#endregion

        public void AddHighScore(string playerName, int score)
        {
            _highScores.Add(new HighScore
            {
                playerName = playerName,
                score = score
            });
        }

        public string HighScoresText()
        {
            if(_highScores.Count < 1) {
                return "No High Scores!";
            }

            StringBuilder builder = new StringBuilder();
            int i=1;
            foreach(HighScore highScore in _highScores) {
                builder.AppendLine($"{i}. {highScore.score}");
                i++;
            }
            return builder.ToString();
        }

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Game.HighScoreManager");
            debugMenuNode.RenderContentsAction = () => {
                GUILayout.BeginVertical("High Scores", GUI.skin.box);
                    foreach(HighScore highScore in HighScores) {
                        GUILayout.Label($"{highScore.playerName}: {highScore.score}");
                    }
                GUILayout.EndVertical();
            };
        }
    }
}
