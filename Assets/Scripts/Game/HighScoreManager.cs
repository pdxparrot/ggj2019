using System;
using System.Collections.Generic;
using System.Text;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game
{
    public sealed class HighScoreManager : SingletonBehavior<HighScoreManager>
    {
        public enum HighScoreSortOrder
        {
            Ascending,
            Descending
        }

        public struct HighScore : IComparable<HighScore>
        {
            public string playerName;

            public int playerCount;

            public int score;

            public int CompareTo(HighScore other)
            {
                // sort descending by default
                return other.score.CompareTo(score);
            }
        }

        public HighScoreSortOrder SortOrder { get; set; } = HighScoreSortOrder.Descending;

        private readonly SortedSet<HighScore> _highScores = new SortedSet<HighScore>();

        public IEnumerable<HighScore> HighScores
        {
            get
            {
                switch(SortOrder)
                {
                case HighScoreSortOrder.Ascending:
                    return _highScores.Reverse();
                case HighScoreSortOrder.Descending:
                default:
                    return _highScores;
                }
            }
        }

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
                playerCount = 1,
                score = score
            });
        }

        public void AddHighScore(int playerCount, int score)
        {
            _highScores.Add(new HighScore
            {
                playerName = string.Empty,
                playerCount = playerCount,
                score = score,
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
                if(string.IsNullOrWhiteSpace(highScore.playerName)) {
                    builder.AppendLine($"{i}. {highScore.score} ({highScore.playerCount})");
                } else {
                    builder.AppendLine($"{i}. {highScore.score} {highScore.playerName}");
                }
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
                        GUILayout.Label($"{highScore.playerName} {highScore.playerCount} {highScore.score}");
                    }
                GUILayout.EndVertical();
            };
        }
    }
}
