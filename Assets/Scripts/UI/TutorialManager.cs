using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : GOUI {

    static public int newTurnState = 1;
    static public int selectCardState = 2;
    static public int beginMatch = 3;
    static public int inspectCard = 4;
    static public int inspectToken = 5;
    static public int inspectStack = 6;

    static public int tutorialNumber;
    static public int state;

    static public bool blockActions;

    static public void Reset () {
        tutorialNumber = 0;
        Tooltip.permanent = false;
        blockActions = false;
    }

    static public void SetTutorialNumber (int newTutorialNumber) {
        tutorialNumber = newTutorialNumber;
        state = 0;
    }

    static public void NewState (int newState) {
        switch (tutorialNumber) {
            case 1:
                switch (state) {
                    case 0:
                        if (newState == beginMatch) {
                            DelayedTooltip (720, 800, Language.TutorialTooltip [0]);
                            state++;
                        }
                        break;
                    case 1:
                        if (newState == selectCardState) {
                            DelayedTooltip (695, 385, Language.TutorialTooltip [1]);
                            state++;
                        }
                        break;
                    case 2:
                        if (newState == newTurnState) {
                            DelayedTooltip (800, 25, Language.TutorialTooltip [2]);
                            state++;
                        }
                        break;
                    case 3:
                        Tooltip.permanent = false;
                        break;
                }
                break;
            case 2:
                switch (state) {
                    case 0:
                        if (newState == beginMatch) {
                            DelayedTooltip (840, 25, -1, Language.TutorialTooltip [3]);
                            state++;
                        }
                        break;
                    case 1:
                        Tooltip.DestroyPermanentTooltip ();
                        state++;
                        break;
                    case 2:
                        if (newState == newTurnState) {
                            DelayedTooltip (720, 800, Language.TutorialTooltip [4]);
                            blockActions = true;
                            state++;
                        }
                        break;
                    case 3:
                        if (newState == inspectCard) {
                            Tooltip.DestroyPermanentTooltip ();
                            blockActions = false;
                            state++;
                        }
                        break;
                    case 4:
                        if (newState == newTurnState) {
                            DelayedTooltip (685, 300, Language.TutorialTooltip [5]);
                            state++;
                        }
                        break;
                    case 5:
                        Tooltip.permanent = false;
                        break;
                }
                break;
            case 3:
                switch (state) {
                    case 0:
                        if (newState == beginMatch) {
                            DelayedTooltip (830, 800, Language.TutorialTooltip [6]);
                            state++;
                        }
                        break;
                    case 1:
                        if (newState == selectCardState) {
                            Tooltip.DestroyPermanentTooltip ();
                            Tooltip.permanent = false;
                            state++;
                        }
                        break;
                    case 2:
                        if (newState == newTurnState) {
                            state++;
                        }
                        break;
                    case 3:
                        if (newState == newTurnState) {
                            DelayedTooltip (1260, 150, Language.TutorialTooltip [7]);
                            blockActions = true;
                            state++;
                        }
                        break;
                    case 4:
                        if (newState == inspectCard) {
                            blockActions = false;
                            Tooltip.permanent = false;
                        }
                        break;
                }
                break;
            case 4:
                switch (state) {
                    case 0:
                        if (newState == beginMatch) {
                            DelayedTooltip (695, 385, Language.TutorialTooltip [9]);
                            blockActions = true;
                            state++;
                        }
                        break;
                    case 1:
                        if (newState == inspectToken) {
                            Tooltip.DestroyPermanentTooltip ();
                            blockActions = false;
                            state++;
                        }
                        break;
                    case 2:
                        if (newState == newTurnState) {
                            state++;
                        }
                        break;
                    case 3:
                        if (newState == newTurnState) {
                            DelayedTooltip (150, 40, Language.TutorialTooltip [8]);
                            state++;
                        }
                        break;
                    case 4:
                        if (newState == newTurnState) {
                            state++;
                        }
                        break;
                    case 5:
                        if (newState == newTurnState) {
                            DelayedTooltip (1050, 800, Language.TutorialTooltip [10]);
                            blockActions = true;
                            state++;
                        }
                        break;
                    case 6:
                        if (newState == inspectStack) {
                            Tooltip.DestroyPermanentTooltip ();
                            blockActions = false;
                            state++;
                        }
                        break;
                    case 7:
                        if (newState == inspectCard) {
                            Tooltip.permanent = false;
                        }
                        break;
                }
                break;
        }
    }

    static public void DelayedTooltip (int px, int py, string s) {
        DelayedTooltip (px, py, 1, s);
    }

    static public void DelayedTooltip (int px, int py, int side, string s) {
        VisualMatch.instance.DelayedTooltip (px, py, side, s);
    }

}
