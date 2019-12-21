using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualTeam {

    List<PlayerClass> player;
    bool ally;
    int numberOfPlayers;

    int margin;
    int avatarSize;
    int barLength;
    int avatarPosition;
    int barPosition;

    GameObject [] HealthBar;
    GameObject ScoreText;
    GameObject UserNameText;
    GameObject Highlight;

    string simplifiedScoreString;
    string detailedScoreString;

    public VisualTeam () {

    }

    public void CreatePlayerUI (List <PlayerClass> player, bool numerate, bool ally, int numberOfTeams, int numberOfPlayers, int teamPosition, int globalPosition) {
        GameObject Clone;
        int teamSize = player.Count;
        HealthBar = new GameObject [teamSize];

        //Debug.Log (ally + " numberOfTeams " + numberOfTeams + " numberOfPlayers " + numberOfPlayers + " teamSize " + teamSize + " teamPosition " + teamPosition + " globalPosition " + globalPosition + "  positionInTeam" + positionInTeam + " playerTurn " + playerTurn);
        this.player = player;
        this.ally = ally;
        this.numberOfPlayers = numberOfPlayers;

        margin = 150;
        avatarSize = 70;
        barLength = (1440 - margin * 2 - (numberOfTeams - 1) * 30 - numberOfPlayers * avatarSize) / numberOfTeams;
        barPosition = margin + barLength / 2 + teamPosition * (barLength + 30) + globalPosition * avatarSize;

        if (ally) {
            barPosition += avatarSize * teamSize;
        }

        for (int x = 0; x < teamSize; x++) {
            PlayerClass tempPlayer = player [x];
            avatarPosition = margin + avatarSize / 2 + teamPosition * (barLength + 30) + (globalPosition + x) * avatarSize;
            int avatarNumber = tempPlayer.properties.avatar;

            if (!ally) {
                avatarPosition += barLength;
            }

            Clone = GOUI.CreateSprite ();
            GOUI.SetSprite (Clone, AppDefaults.avatar [avatarNumber]);
            GOUI.SetInPixScale (Clone, avatarSize, avatarSize);
            GOUI.SetInPixPosition (Clone, avatarPosition, 45, 14);
            Clone.GetComponent<UIController> ().player = tempPlayer;
            Clone.name = UIString.InGameAvatar;

            if (numerate) {
                int px = avatarPosition + avatarSize / 4;
                int py = 45 + avatarSize / 4;
                Clone = GOUI.CreateSprite ("UI/White");
                GOUI.SetInPixScale (Clone, avatarSize / 2, avatarSize / 2);
                GOUI.SetInPixPosition (Clone, px, py, 15);
                GameObject.DestroyImmediate (Clone.GetComponent<Collider> ());
                GOUI.SetSpriteColor (Clone, Color.black);

                Clone = GOUI.CreateText (tempPlayer.properties.playerNumber.ToString(), px, py, 16, 0.025f);
                Clone.GetComponent<Renderer>().material.color = Color.white;
            }

            Clone = GOUI.CreateSprite ("UI/White");
            GOUI.SetSpriteColor (Clone, AppDefaults.PlayerColor [tempPlayer.properties.playerNumber]);
            GameObject.DestroyImmediate (Clone.GetComponent<BoxCollider> ());
            HealthBar [x] = Clone;
        }

        
        Clone = GOUI.CreateSprite ("Textures/Other/Selection".ToString ());
        GameObject.DestroyImmediate (Clone.GetComponent<BoxCollider> ());
        Clone.GetComponent<SpriteRenderer> ().color = new Color (0, 1, 0);
        GOUI.SetInPixScale (Clone, avatarSize + 30, avatarSize + 30, false);
        GOUI.SetInPixPosition (Clone, avatarPosition, 45, 12, false);
        Highlight = Clone;
        SetPlayerActive (false);

        Clone = GOUI.CreateSprite ("UI/White");
        GOUI.SetInPixScale (Clone, barLength, 30);
        GOUI.SetInPixPosition (Clone, barPosition, 25, 11);
        Color col;
        if (player.Count == 1) {
            col = AppDefaults.PlayerColor [player [0].properties.playerNumber] * 0.5f;
        } else {
            col = Color.white * 0.5f;
        }
        col.a = 1;
        Clone.GetComponent<UIController> ().visualTeam = this;
        Clone.name = UIString.InGameScoreBar;
        GOUI.SetSpriteColor (Clone, col);

        Clone = GOUI.CreateText ("", barPosition, 25, 13, 0.025f);
        Clone.GetComponent<TextMesh> ().color = Color.black;
        ScoreText = Clone;

        Clone = GOUI.CreateText ("", barPosition, 55, 13, 0.025f);
        Clone.GetComponent<TextMesh> ().color = Color.black;
        if (player.Count == 1) {
            Clone.GetComponent<TextMesh> ().text = player [0].properties.displayName;
        } else {
            Clone.GetComponent<TextMesh> ().text = Language.Team + " " + player[0].properties.team;
        }
        UserNameText = Clone;
    }

    public void DelayedSetActivePlayer (bool active) {
        VisualMatch.instance.SetPlayerActive (this, active);
    }

    public void SetPlayerActive (bool active) {
        Highlight.GetComponent<SpriteRenderer> ().enabled = active;
    }

    public void DelayedUpdateVisuals (MatchClass match) {
        int count = player.Count;
        int [] score = new int [count];
        int [] scoreIncome = new int [count];
        for (int x = 0; x < count; x++) {
            PlayerClass tempPlayer = player [x];
            score [x] = tempPlayer.score;
            scoreIncome [x] = tempPlayer.scoreIncome;
        }
        DelayedSetPlayerHealthBar (score, scoreIncome, match.properties.scoreLimit);
    }

    public void UpdateVisuals (MatchClass match) {
        int count = player.Count;
        int [] score = new int [count];
        int [] scoreIncome = new int [count];
        for (int x = 0; x < count; x++) {
            PlayerClass tempPlayer = player [x];
            score [x] = tempPlayer.score;
            scoreIncome [x] = tempPlayer.scoreIncome;
        }
        SetPlayerHealthBar (score, scoreIncome, match.properties.scoreLimit);
    }

    public void DisplayDetailedHealthBar () {
        ScoreText.GetComponent<TextMesh> ().text = detailedScoreString;
    }

    public void DisplaySimplifiedHealthBar () {
        ScoreText.GetComponent<TextMesh> ().text = simplifiedScoreString;
    }


    public void DelayedSetPlayerHealthBar (int [] score, int [] scoreIncome, int scoreLimit) {
        VisualMatch.instance.SetPlayerHealthBar (this, score, scoreIncome, scoreLimit);
    }

    public void SetPlayerHealthBar (int [] score, int [] scoreIncome, int scoreLimit) {
        float totalScore = 0;
        float totalScoreIncome = 0;
        string totalScoreString = "";
        string totalScoreIncomeString = "";
        for (int x = score.Length - 1; x >= 0; x--) {
            float percentage = Mathf.Clamp (1f * score [x] / scoreLimit, 0, 1);
            totalScore += score [x];
            float totalPercentage = Mathf.Clamp (1f * totalScore / scoreLimit, 0, 1);
            GOUI.SetInPixScale (HealthBar [x], (int) (percentage * barLength), 30);
            if (ally) {
                GOUI.SetInPixPosition (HealthBar [x], barPosition + (int) (barLength * (totalPercentage - percentage / 2 - 0.5f)), 25, 12, false);
            } else {
                GOUI.SetInPixPosition (HealthBar [x], barPosition + (int) ((barLength * (-totalPercentage + percentage / 2 + 0.5f)) + 0.5f), 25, 12, false);
            }
            totalScoreIncome += scoreIncome [x];
        }
        for (int x = 0; x < score.Length; x++) {
            if (x > 0) {
                if (score [x] >= 0) {
                    totalScoreString += " + ";
                } else {
                    totalScoreString += " - ";
                }
                if (scoreIncome [x] >= 0) {
                    totalScoreIncomeString += " + ";
                } else {
                    totalScoreIncomeString += " - ";
                }
            } else {
                if (scoreIncome [x] >= 0) {
                    totalScoreIncomeString += "+";
                } else {
                    totalScoreIncomeString += "-";
                }
            }
            totalScoreString += score [x].ToString ();
            totalScoreIncomeString += Mathf.Abs (scoreIncome [x]).ToString ();
        }
        detailedScoreString = totalScoreString + " (" + totalScoreIncomeString.ToString () + ")";
        if (totalScoreIncome >= 0) {
            simplifiedScoreString = totalScore + " (+" + totalScoreIncome + ")";
        } else {
            simplifiedScoreString = totalScore + " (" + totalScoreIncome + ")";
        }
        ScoreText.GetComponent<TextMesh> ().text = simplifiedScoreString;
    }
    /*
    public void SetPlayerHealthBar (int playerNumber, int score, int scoreIncome, int scoreLimit) {

        float percentage = Mathf.Clamp (1f * score / scoreLimit, 0, 1);
        GOUI.SetInPixScale (HealthBar [playerNumber], (int) (percentage * barLength), 30);
        if (ally) {
            GOUI.SetInPixPosition (HealthBar [playerNumber], barPosition - (int) (barLength / 2 * (1 - percentage)), 25, 12);
        } else {
            GOUI.SetInPixPosition (HealthBar [playerNumber], barPosition + (int) ((barLength * (1 - percentage) + 1) / 2), 25, 12);
        }
        if (scoreIncome >= 0) {
            ScoreText.GetComponent<TextMesh> ().text = score.ToString () + " (+" + scoreIncome.ToString () + ")";
        } else {
            ScoreText.GetComponent<TextMesh> ().text = score.ToString () + " (" + scoreIncome.ToString () + ")";
        }
    }*/

}
