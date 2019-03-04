using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RatingData : MonoBehaviour {

    static public string RatingPath () {
        string path = ServerData.ServerPath () + "Rating/";
        if (!Directory.Exists (path)) {
            Directory.CreateDirectory (path);
        }
        return path;
    }
    
    static public string SaveRatingPlayerWinRatio (string [] lines) {
        string path = RatingPath () + "PlayerWinRatio.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingCardNumberWinRatio (string [] lines) {
        string path = RatingPath () + "CardNumberWinRatio.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string RatingPopularityPath () {
        return RatingPath () + "CardPopularity.txt";
    }


    static public string SaveRatingCardPopularity (string [] lines) {
        string path = RatingPopularityPath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string [] GetRatingPopularity () {
        string path = RatingPopularityPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return null;
    }

    static public string SaveRatingWinnerScore (string [] lines) {
        string path = RatingPath () + "WinnerScore.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingLoserScore (string [] lines) {
        string path = RatingPath () + "LoserScore.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingAbilityOnStack (string [] lines) {
        string path = RatingPath () + "AbilityOnStack.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string RatingAbilityOnRowPath () {
        return RatingPath () + "AbilityOnRow.txt";
    }

    static public string SaveRatingAbilityOnRow (string [] lines) {
        string path = RatingAbilityOnRowPath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string [] GetRatingAbilityOnRow () {
        string path = RatingAbilityOnRowPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return null;
    }

    static public string RatingTokenOnRowPath () {
        return RatingPath () + "TokenOnRow.txt";
    }

    static public string SaveRatingTokenOnRow (string [] lines) {
        string path = RatingTokenOnRowPath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string [] GetRatingTokenOnRow () {
        string path = RatingTokenOnRowPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return null;
    }

    static public string RatingAbilityTokenOnRowPath () {
        return RatingPath () + "AbilityTokenOnRow.txt";
    }

    static public string SaveRatingAbilityTokenOnRow (string [] lines) {
        string path = RatingAbilityTokenOnRowPath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string [] GetRatingAbilityTokenOnRow () {
        string path = RatingAbilityTokenOnRowPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return new string [0];
    }

    static public string SaveRatingAISettings (string [] lines) {
        string path = RatingPath () + "AISettings.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingSurroundDanger (string [] lines) {
        string path = RatingPath () + "SurroundDanger.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingMultiTargetDanger (string [] lines) {
        string path = RatingPath () + "MultiTargetDanger.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingEdgeDanger (string [] lines) {
        string path = RatingPath () + "EdgeDanger.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingTurn (string [] lines) {
        string path = RatingPath () + "Turn.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingMapPlayer (string [] lines) {
        string path = RatingPath () + "MapPlayer.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string SaveRatingNumberOfCards (string [] lines) {
        string path = RatingPath () + "NumberOfCards.txt";
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string RatingAbility_AbilitySynergyPath () {
        return RatingPath () + "Ability_AbilitySynergy.txt";
    }

    static public string [] GetRatingAbility_AbilitySynergy () {
        string path = RatingAbility_AbilitySynergyPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return new string [0];
    }

    static public string SaveRatingAbility_AbilitySynergy (string [] lines) {
        string path = RatingAbility_AbilitySynergyPath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string RatingAbility_TokenSynergyPath () {
        return RatingPath () + "Ability_TokenSynergy.txt";
    }

    static public string [] GetRatingAbility_TokenSynergy () {
        string path = RatingAbility_TokenSynergyPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return new string [0];
    }

    static public string SaveRatingAbility_TokenSynergy (string [] lines) {
        string path = RatingAbility_TokenSynergyPath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string RatingToken_TokenSynergyPath () {
        return RatingPath () + "Token_TokenSynergy.txt";
    }

    static public string [] GetRatingToken_TokenSynergy () {
        string path = RatingToken_TokenSynergyPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return new string [0];
    }

    static public string SaveRatingToken_TokenSynergy (string [] lines) {
        string path = RatingToken_TokenSynergyPath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string RatingTokenAfterTokenPath () {
        return RatingPath () + "TokenAfterToken.txt";
    }

    static public string SaveRatingTokenAfterToken (string [] lines) {
        string path = RatingTokenAfterTokenPath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string [] GetRatingTokenAfterToken () {
        string path = RatingTokenAfterTokenPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return new string [0];
    }

    static public string RatingAbilityStackSizePath () {
        return RatingPath () + "AbilityStackSize.txt";
    }

    static public string SaveRatingAbilityStackSize (string [] lines) {
        string path = RatingAbilityStackSizePath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string [] GetRatingAbilityStackSize () {
        string path = RatingAbilityStackSizePath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return new string [0];
    }

    static public string RatingAbilityAgainstAbilityPath () {
        return RatingPath () + "AbilityAgainstAbility.txt";
    }

    static public string SaveRatingAbilityAgainstAbility (string [] lines) {
        string path = RatingAbilityAgainstAbilityPath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string [] GetRatingAbilityAgainstAbility () {
        string path = RatingAbilityAgainstAbilityPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return new string [0];
    }

    static public string RatingAbilityAgainstTokenPath () {
        return RatingPath () + "AbilityAgainstToken.txt";
    }

    static public string SaveRatingAbilityAgainstToken (string [] lines) {
        string path = RatingAbilityAgainstTokenPath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string [] GetRatingAbilityAgainstToken () {
        string path = RatingAbilityAgainstTokenPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return new string [0];
    }

    static public string RatingTokenAgainstAbilityPath () {
        return RatingPath () + "TokenAgainstAbility.txt";
    }

    static public string SaveRatingTokenAgainstAbility (string [] lines) {
        string path = RatingTokenAgainstAbilityPath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string [] GetRatingTokenAgainstAbility () {
        string path = RatingTokenAgainstAbilityPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return new string [0];
    }

    static public string RatingTokenAgainstTokenPath () {
        return RatingPath () + "TokenAgainstToken.txt";
    }

    static public string SaveRatingTokenAgainstToken (string [] lines) {
        string path = RatingTokenAgainstTokenPath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string [] GetRatingTokenAgainstToken () {
        string path = RatingTokenAgainstTokenPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return new string [0];
    }

    static public string RatingTokenStackSizePath () {
        return RatingPath () + "TokenStackSize.txt";
    }

    static public string SaveRatingTokenStackSize (string [] lines) {
        string path = RatingTokenStackSizePath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string [] GetRatingTokenStackSize () {
        string path = RatingTokenStackSizePath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return new string [0];
    }

    static public string RatingAbilityTokenStackSizePath () {
        return RatingPath () + "AbilityTokenStackSize.txt";
    }

    static public string SaveRatingAbilityTokenStackSize (string [] lines) {
        string path = RatingAbilityTokenStackSizePath ();
        File.WriteAllLines (path, lines);
        return path;
    }

    static public string [] GetRatingAbilityTokenStackSize () {
        string path = RatingAbilityTokenStackSizePath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return new string [0];
    }

    static public string RatingAbilityAfterAbilityPath () {
        return RatingPath () + "AbilityAfterAbility.txt";
    }

    static public string [] GetRatingAbilityAfterAbility () {
        string path = RatingAbilityAfterAbilityPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return null;
    }

    static public string SaveRatingAbilityAfterAbility (string [] lines) {
        string path = RatingAbilityAfterAbilityPath ();
        File.WriteAllLines (path, lines);
        return path;
    }


    static public string RatingAbilityAfterTokenPath () {
        return RatingPath () + "AbilityAfterToken.txt";
    }

    static public string [] GetRatingAbilityAfterToken () {
        string path = RatingAbilityAfterTokenPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return null;
    }

    static public string SaveRatingAbilityAfterToken (string [] lines) {
        string path = RatingAbilityAfterTokenPath ();
        File.WriteAllLines (path, lines);
        return path;
    }


    static public string RatingTokenAfterAbilityPath () {
        return RatingPath () + "TokenAfterAbility.txt";
    }

    static public string [] GetRatingTokenAfterAbility () {
        string path = RatingTokenAfterAbilityPath ();
        if (File.Exists (path)) {
            string [] lines = File.ReadAllLines (path);
            return lines;
        }
        return null;
    }

    static public string SaveRatingTokenAfterAbility (string [] lines) {
        string path = RatingTokenAfterAbilityPath ();
        File.WriteAllLines (path, lines);
        return path;
    }

}
