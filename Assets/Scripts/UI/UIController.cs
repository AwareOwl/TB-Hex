﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {


    public int id;
    public int number;
    public int x;
    public int y;

    public Sprite NormalSprite;
    public Sprite OnMouseOverSprite;
    public Sprite OnMouseClickSprite;
    public GameObject Text;
    public GameObject HoverObject;

    public PageUI pageUI;
    public VisualTeam visualTeam;
    public PlayerClass player;
    public CardClass card;
    public TileClass tile;
    public AbilityType abilityType = AbilityType.NULL;
    public TokenType tokenType = TokenType.NULL;

    public List<GameObject> references = new List<GameObject> ();

    public float timer = 0f;
    public float timeToTooltip = 0.25f;

    public bool Over = false;
    public bool interactible = true;
    public bool Pressed;
    public bool Pressed1;
    public bool PressedAndLocked;

    // Use this for initialization
    void Start () {
		
	}

    public void PressAndLock () {
        PressedAndLocked = true;
        SetOnMouseClickSprite ();
    }

    public void FreeAndUnlcok () {
        PressedAndLocked = false;
        if (!Over) {
            SetNormalSprite ();
        } else {
            SetOnMouseOverSprite ();
        }
    }

    public void OnPointerEnter (PointerEventData eventData) {
        Over = true;
        Entered = true;
    }

    public void OnPointerExit (PointerEventData eventData) {
        if (!Pressed && interactible) {
            SetNormalSprite ();
        }
        Over = false;
        Entered = false;
    }

    public void OnPointerDown (PointerEventData eventData) {
        if (!interactible) {
            SetOnMouseClickSprite ();
            Pressed = true;
        }
    }

    bool Entered;


    private void OnMouseEnter () {
        timer = 0;
        Entered = true;
    }

    void OnEnter () {
        //Debug.Log (this, gameObject);
        SetOnMouseOverSprite ();
        switch (name) {
            // Exceptions
            case UIString.InGameAvatar:
            case UIString.InGameScoreBar:
            case UIString.TurnCounter:
                break;
            case UIString.Tile:
                if (tile.token == null) {
                    SoundManager.PlayAudioClip (MyAudioClip.OnTileOver);
                } else {
                    //SoundManager.PlayAudioClip (MyAudioClip.OnTileOver);
                }
                break;
            case UIString.SetEditorCollectionCard:
                if (SetEditor.Collection [x, y] != null) {
                    SoundManager.PlayAudioClip (MyAudioClip.OnButtonOver);
                }
                break;
            case UIString.SetEditorSetCard:
                if (SetEditor.Set [x, y] != null) {
                    SoundManager.PlayAudioClip (MyAudioClip.OnButtonOver);
                }
                break;
            default:
                SoundManager.PlayAudioClip (MyAudioClip.OnButtonOver);
                break;
        }
    }

    private void OnMouseExit () {
        Tooltip.DestroyTooltip ();
        switch (name) {
            case "Tile":
                OnMouseLeaveTileAction ();
                break;
        }
        if (!Pressed) {
            SetNormalSprite ();
        }
        Over = false;
        Entered = false;
    }

    public void SetNormalSprite () {
        if (!PressedAndLocked) {
            SetSprite (NormalSprite);
            SetTextColor (Color.black);
            SetHoverObjectColor (new Color (1, 1, 1, 0f));
        }
    }

    public void SetOnMouseOverSprite () {
        if (!PressedAndLocked) {
            SetSprite (OnMouseOverSprite);
            SetTextColor (Color.white);
            SetHoverObjectColor (new Color (1, 1, 1, 0.25f));
        }
    }

    public void SetOnMouseClickSprite () {
        SetSprite (OnMouseClickSprite);
        SetTextColor (Color.white);
        SetHoverObjectColor (new Color (1, 1, 1, 0.25f));
    }

    public void SetHoverObjectColor (Color color) {
        if (HoverObject != null){
            if (BoardEditorMenu.instance != null) {
                HoverObject.GetComponent<VisualEffectScript> ().SetColor (color);
            } else {
                bool highlighted = color.a != 0;
                VisualEffectScript VEScript = tile.visualTile.Tile.GetComponent<VisualEffectScript> ();
                if (VEScript) {
                    VEScript.highlighted = highlighted;
                }
            }
        }
    }

    public void SetTextColor (Color color) {
        if (Text != null) {
            if (Text.GetComponent<Renderer> ()) {
                Text.GetComponent<Renderer> ().material.color = color;
            } else if (Text.GetComponent <Text> () != null) {
                Text.GetComponent<Text> ().color = color;
            }
        }
    }

    public void CreateTooltip () {
        if (card != null) {
            TutorialManager.NewState (TutorialManager.inspectCard);
            Tooltip.NewTooltip (transform, card);
        }
        if (tile != null && tile.token != null) {
            TutorialManager.NewState (TutorialManager.inspectToken);
            Tooltip.NewTooltip (transform, tile.token);
        }
        if (abilityType != AbilityType.NULL) {
            Tooltip.NewAbilityTypeTooltip (transform, abilityType);
        }
        if (tokenType != TokenType.NULL) {
            TutorialManager.NewState (TutorialManager.inspectToken);
            Tooltip.NewTokenTypeTooltip (transform, tokenType);
        }
        switch (name) {
            case "StartHost":
                Tooltip.NewTooltip (transform, Language.CreateLocalNetworkTooltip);
                break;
            case "StartClient":
                Tooltip.NewTooltip (transform, Language.JoinLocalNetworkTooltip);
                break;
            case UIString.ProfileSettingsAvatar:
            case UIString.Avatar:
                Tooltip.NewTooltip (transform, Language.AvatarName [number]);
                break;

            case UIString.InGameAvatar:
                Tooltip.NewTooltip (transform, player.properties.displayName + System.Environment.NewLine + System.Environment.NewLine + Language.StatusDescription [player.properties.specialStatus]);
                break;

            case UIString.ShowCodeMenu:
                Tooltip.NewTooltip (transform, Language.EnterCode);
                break;

            case UIString.ExitApp:
                Tooltip.NewTooltip (transform, Language.ExitApp);
                break;
            case UIString.ShowSettingsMenu:
                Tooltip.NewTooltip (transform, Language.ShowSettingsMenu);
                break;
            case UIString.MainMenuStartGameVsAI:
                Tooltip.NewTooltip (transform, Language.BeginGameAgainstAI);
                break;
            case UIString.ShowSetList:
                Tooltip.NewTooltip (transform, Language.SelectCardSetTooltip);
                break;
            case UIString.ShowGameModeMenu:
                Tooltip.NewTooltip (transform, Language.ChangeGameVersion);
                break;

            case UIString.SetEditorGenerateRandomSet:
                Tooltip.NewTooltip (transform, Language.GenerateRandomSet);
                break;
            case UIString.SetEditorSaveSet:
                Tooltip.NewTooltip (transform, Language.SaveSet);
                break;
            case UIString.ShowMainMenu:
                Tooltip.NewTooltip (transform, Language.GoBackToMenu);
                break;
            case UIString.SetEditorChangeSetProperties:
                Tooltip.NewTooltip (transform, Language.ChangeSetNameOrIcon);
                break;
            case UIString.SetEditorFilterMenu:
                switch (number) {
                    case 0:
                        Tooltip.NewTooltip (transform, Language.ShowTokenValueFilters);
                        break;
                    case 1:
                        Tooltip.NewTooltip (transform, Language.ShowTokenTypeFilters);
                        break;
                    case 2:
                        Tooltip.NewTooltip (transform, Language.ShowAbilityTypeFilters);
                        break;
                    case 3:
                        Tooltip.NewTooltip (transform, Language.ShowAbilityAreaFilters);
                        break;
                }
                break;

            case UIString.ShowSetEditor:
                Tooltip.NewTooltip (transform, Language.EditSet);
                break;
            case UIString.DeleteSet:
                Tooltip.NewTooltip (transform, Language.DeleteSet);
                break;
            case UIString.CreateNewSet:
                Tooltip.NewTooltip (transform, Language.CreateNewSet);
                break;
            case UIString.MoreToUnlock:
                Tooltip.NewTooltip (transform, Language.YouCanUnlockMoreCardsByFinishingPuzzlesAndIncreasingYourLevel);
                break;


            case UIString.TurnCounter:
                Tooltip.NewTooltip (transform, Language.NumberOfTurnsLeftToTheEndOfGame);
                break;

            case UIString.ShowGameModeEditor:
                if (GameModeMenu.currentGroup < 2) {
                    Tooltip.NewTooltip (transform, Language.ViewGameModeEditor);
                } else {
                    Tooltip.NewTooltip (transform, Language.EditThisGameVersion);
                }
                break;
            case UIString.CreateNewGameMode:
                Tooltip.NewTooltip (transform, Language.AddNewGameVersion);
                break;
            case UIString.DeleteGameMode:
                Tooltip.NewTooltip (transform, Language.DeleteThisGameVersion);
                break;
            case UIString.GoBackToGameModeSelection:
                Tooltip.NewTooltip (transform, Language.GoBackToGameVersionSelection);
                break;

            case UIString.GameModeEditorCreateNewBoard:
                Tooltip.NewTooltip (transform, Language.AddNewBoard);
                break;
            case UIString.GameModeEditorDeleteBoard:
                Tooltip.NewTooltip (transform, Language.DeleteThisBoard);
                break;
            case UIString.GameModeEditorEditBoard:
                if (GameModeEditor.editMode) {
                    Tooltip.NewTooltip (transform, Language.EditThisBoard);
                } else {
                    Tooltip.NewTooltip (transform, Language.ViewBoardEditor);
                }
                break;
            case UIString.GameModeEditorEditCardPool:
                if (GameModeEditor.editMode) {
                    Tooltip.NewTooltip (transform, Language.EditAvailableCardPool);
                } else {
                    Tooltip.NewTooltip (transform, Language.ViewCardPoolEditor);
                }
                break;
            case UIString.GameModeEditorSettings:
                if (GameModeEditor.editMode) {
                    Tooltip.NewTooltip (transform, Language.EditGameModeSettings);
                } else {
                    Tooltip.NewTooltip (transform, Language.ViewGameModeSettings);
                }
                break;
            case UIString.GameModeEditorChangeName:
                Tooltip.NewTooltip (transform, Language.ChangeGameVersionName);
                break;
            case UIString.RefreshCustomGameLobby:
                Tooltip.NewTooltip (transform, Language.Refresh);
                break;

            case UIString.SaveBoard:
                Tooltip.NewTooltip (transform, Language.SaveThisBoard);
                break;
            case UIString.ChangeBoardName:
                Tooltip.NewTooltip (transform, Language.ChangeBoardName);
                break;
            case UIString.BoardEditorSettings:
                Tooltip.NewTooltip (transform, Language.ChangeSettings);
                break;
            case UIString.GoBackToGameModeEditor:
                Tooltip.NewTooltip (transform, Language.GoBackToGameVersionEditor);
                break;
            case UIString.CustomGameKickPlayer:
                Tooltip.NewTooltip (transform, Language.KickPlayer);
                break;
            case UIString.CustomGameLobbyApply:
                Tooltip.NewTooltip (transform, Language.JoinToSelectedGame);
                break;
            case UIString.ShowChat:
                if (ChatUI.instance == null) {
                    Tooltip.NewTooltip (transform, Language.ShowChat);
                } else {
                    Tooltip.NewTooltip (transform, Language.HideChat);
                }
                break;

            case UIString.ShowUnlockedContent:
                Tooltip.NewTooltip (transform, Language.ShowUnlockedContent);
                break;

            case UIString.CardPoolEditorNextAbilityPage:
            case UIString.CardPoolEditorNextTokenPage:
                Tooltip.NewTooltip (transform, Language.NextPage);
                break;
            case UIString.CardPoolEditorPrevAbilityPage:
            case UIString.CardPoolEditorPrevTokenPage:
                Tooltip.NewTooltip (transform, Language.PrevPage);
                break;


            case UIString.PuzzleMenuApply:
                Tooltip.NewTooltip (transform, Language.BeginPuzzle);
                break;
            case UIString.BossMenuApply:
                Tooltip.NewTooltip (transform, Language.GoToTheSetEditor);
                break;
            case UIString.BossMenuLocked:
                Tooltip.NewTooltip (transform, Language.BossUnlockTooltip);
                break;
            case UIString.SetEditorStartBossFight:
                Tooltip.NewTooltip (transform, Language.BeginBossFight);
                break;
            case UIString.TutorialMenuApply:
                Tooltip.NewTooltip (transform, Language.BeginMission);
                break;

            case UIString.RestartPuzzle:
                Tooltip.NewTooltip (transform, Language.RestartPuzzle);
                break;
            case UIString.RestartBoss:
                Tooltip.NewTooltip (transform, Language.RestartBoss);
                break;
            case UIString.PuzzleMenuLocked:
                Tooltip.NewTooltip (transform, Language.PuzzleUnlockTooltip);
                break;
            case UIString.RestartTutorial:
                Tooltip.NewTooltip (transform, Language.RestartMission);
                break;

            case UIString.ShowOptionsMenu:
                Tooltip.NewTooltip (transform, Language.Options);
                break;

            case UIString.SetEditorAbout:
            case UIString.GameModeEditorAbout:
            case UIString.BoardEditorAbout:
            case UIString.CardPoolEditorAbout:
            case UIString.BossMenuAbout:
            case UIString.PuzzleMenuAbout:
            case UIString.LibraryMenuAbout:
                Tooltip.NewTooltip (transform, Language.ClickToLearnMore);
                break;

            case UIString.SaveSelectedSet:
            case UIString.SetEditorApplySetProperties:
            case UIString.CardPoolEditorSaveCardPool:
            case UIString.GameModeMenuApply:
            case UIString.AvailableMatchTypesEditorApply:
            case UIString.SaveGameModeSettings:
                Tooltip.NewTooltip (transform, Language.Apply);
                break;
            case UIString.DestroySubMenu:
                Tooltip.NewTooltip (transform, Language.Close);
                break;

        }
    }

    public void OnClickAction () {
        if (!interactible) {
            return;
        }
        if (pageUI != null) {
            //pageUI.SelectPage (number);
        }
        switch (name) {
            case UIString.InGameAvatar:
            case UIString.InGameScoreBar:
            case UIString.TurnCounter:
            case UIString.Tile:
                break;
            case UIString.SetEditorCollectionCard:
                if (SetEditor.Collection [x, y] != null) {
                    SoundManager.PlayAudioClip (MyAudioClip.SetClick);
                }
                break;
            case UIString.SetEditorSetCard:
                if (SetEditor.Set [x, y] != null) {
                    //Debug.Log (SetEditor.Set [x, y]);
                    SoundManager.PlayAudioClip (MyAudioClip.SetClick);
                }
                break;
            case UIString.InGameHandCard:
                SoundManager.PlayAudioClip (MyAudioClip.SetClick);
                break;
            default:
                SoundManager.PlayAudioClip (MyAudioClip.OnButtonClick);
                break;
        }
        switch (name) {
            case "SelectPL":
                Language.SetLanguage (Language.Polish);
                break;
            case "SelectENG":
                Language.SetLanguage (Language.English);
                break;

            case "StartHost":
                MyNetworkManager.StartNewHost ();
                break;
            case "StartClient":
                MyNetworkManager.StartNewClient ();
                break;
            case "StartServer":
                MyNetworkManager.StartNewServer ();
                break;
            case "Tile":
                TileAction ();
                //transform.parent.GetComponent<VisualEffectScript> ().PushItDown (-1);
                break;
            case "LogUserIn":
                LoginMenu.LogIn ();
                break;
            case "RegisterMenu":
                RegisterMenu.ShowRegisterMenu ();
                break;
            case "RegisterUser":
                RegisterMenu.Register ();
                break;
            case "LoginMenu":
                LoginMenu.ShowLoginMenu ();
                break;
            case "CloseMessage":
                DestroyReferences ();
                break;
                /*
            case "LoadBoard":
                BoardEditorMenu.LoadBoard (2);
                break;*/
            case "BoardEditorTileType":
                BoardEditorMenu.SelectButton (1, number);
                break;
            case "BoardEditorOwner":
                BoardEditorMenu.SelectButton (2, number);
                break;
            case "BoardEditorValue":
                BoardEditorMenu.SelectButton (3, number);
                break;
            case "BoardEditorTokenType":
                BoardEditorMenu.SelectButton (4, number);
                break;

            // CardPoolEditor
            case UIString.CardPoolEditorValue:
                CardPoolEditor.SelectButton (1, number);
                break;
            case UIString.CardPoolEditorTokenType:
                CardPoolEditor.SelectButton (2, number);
                break;
            case UIString.CardPoolEditorAbilityArea:
                CardPoolEditor.SelectButton (3, number);
                break;
            case UIString.CardPoolEditorAbilityType:
                CardPoolEditor.SelectButton (4, number);
                break;
            case UIString.CardPoolEditorNextAbilityPage:
                CardPoolEditor.NextAbilityPage ();
                break;
            case UIString.CardPoolEditorPrevAbilityPage:
                CardPoolEditor.PrevAbilityPage ();
                break;
            case UIString.CardPoolEditorNextTokenPage:
                CardPoolEditor.NextTokenPage ();
                break;
            case UIString.CardPoolEditorPrevTokenPage:
                CardPoolEditor.PrevTokenPage ();
                break;
            case UIString.CardPoolEditorCard:
                CardPoolEditor.CardAction (number);
                break;
            case UIString.CardPoolEditorAbout:
                if (GameModeEditor.editMode) {
                    GOUI.ShowMessage (Language.CardPoolEditorDescription);
                } else {
                    GOUI.ShowMessage (Language.CardPoolEditorReadOnlyDescription);
                }
                break;
            case UIString.CardPoolEditorPageButton:
                CardPoolEditor.SelectPage (number);
                break;

            case UIString.CardPoolEditorSaveCardPool:
                CardPoolEditor.SaveCardPool ();
                break;

            // Set editor
            case UIString.SetEditorGenerateRandomSet:
                SetEditor.LoadRandomSet ();
                break;
            case UIString.SetEditorSaveSet:
                SetEditor.SaveSet ();
                break;
            case UIString.SetEditorCollectionCard:
                SetEditor.SelectCardInCollection (x, y);
                break;
            case UIString.SetEditorSetCard:
                SetEditor.LoadCardInSet (x, y);
                break;
            case UIString.SetEditorPageButton:
                SetEditor.LoadPage (number);
                break;
            case UIString.SetEditorAbout:
                GOUI.ShowMessage (Language.GetSetEditorDescription ());
                break;
            case UIString.SetEditorFilterMenu:
                SetEditor.ShowFilterMenu (number);
                break;
            case UIString.SetEditorFilterButton:
                SetEditor.SelectFilterButton (number);
                break;

            case UIString.ShowSetList:
                SetList.ShowSetList ();
                break;
            case UIString.ShowMainMenu:
                MainMenu.ShowMainMenu ();
                break;
            case UIString.SetListPageButton:
                SetList.SelectPage (number);
                break;

            case UIString.CreateNewSet:
                ClientLogic.MyInterface.CmdCreateNewSet (Language.NewSet);
                break;

            case UIString.DeleteSet:
                SetList.DeleteSet (id);
                break;

            case UIString.SelectSet:
                SetList.SelectSet (id);
                break;

            case UIString.AvailableMatchTypesEditorButton:
                AvailableMatchTypesEditor.ButtonAction (number);
                break;
            case UIString.AvailableMatchTypesEditorApply:
                AvailableMatchTypesEditor.ApplyChanges ();
                break;

            case UIString.SaveSelectedSet:
                SetList.SaveSelection ();
                break;
            case UIString.SetEditorChangeSetProperties:
            case UIString.GameModeEditorChangeName:
            case UIString.ChangeBoardName:
                GOUI.CurrentGOUI.ShowPropertiesMenu ();
                break;
            case UIString.BoardEditorSettings:
                AvailableMatchTypesEditor.ShowAvailableMatchTypesEditor ();
                break;
            case UIString.SetEditorApplySetProperties:
                PropertiesMenu.ApplySetProperties ();
                break;
            case UIString.SetEditorSelectIcon:
                PropertiesMenu.SelectIcon (x, y, number);
                break;

            // Main menu
            case UIString.MainMenuStartGameVsAI:
                ClientLogic.MyInterface.CmdJoinGameAgainstAI ();
                break;
            case UIString.MainMenuStartQuickMatch:
                ClientLogic.MyInterface.CmdJoinQuickMatchQueue ();
                break;
            case UIString.ShowCustomGameLobby:
            case UIString.RefreshCustomGameLobby:
                CustomGameLobby.ShowCustomGameLobby ();
                break;

            case UIString.CustomGameLobbyPageButton:
                CustomGameLobby.ShowPage (number);
                break;
            case UIString.CustomGameLobbyRow:
                CustomGameLobby.SelectRow (id);
                break;
            case UIString.CustomGameLobbyApply:
                CustomGameLobby.JoinCustomGameRoom ();
                break;
            case UIString.ShowCustomGameRoom:
                CustomGameRoom.ShowCustomGameRoom ();
                break;
            case UIString.ShowCustomGameEditor:
                CustomGameEditor.ShowCustomGameEditor ();
                break;
            case UIString.CustomGameEditorTypeButton:
                CustomGameEditor.SelectType (number);
                break;
            case UIString.CustomGameRoomAddAI:
                CustomGameRoom.AddAIOpponent (id);
                break;
            case UIString.CustomGameKickPlayer:
                CustomGameRoom.KickPlayer (id);
                break;
            case UIString.CustomGameEditorApply:
                CustomGameEditor.CreateGame ();
                break;
            case UIString.CustomGameRoomSelectRow:
                ClientLogic.MyInterface.CmdCustomGameMoveToDifferentSlot (id);
                break;
            case UIString.CustomGameRoomAvatar:
                ClientLogic.MyInterface.CmdCustomGameChangeTeam (number);
                break;
            case UIString.CustomGameRoomStartMatch:
                CustomGameRoom.StartMatch ();
                break;
            case UIString.ShowSetEditor:
                SetEditor.ShowSetEditorMenu (id);
                break;
            case UIString.ShowChat:
                ChatUI.ToggleChatUI ();
                break;
            case UIString.ChatSendButton:
                ChatUI.SendMessage ();
                break;

            case UIString.InGameHandCard:
                InGameUI.SelectStack (x);
                break;
            case UIString.PlayedCard:
                UsedCardPreview.ExtendPreview (x);
                break;

            case UIString.RestartPuzzle:
                InGameUI.RestartPuzzle ();
                break;
            case UIString.RestartBoss:
                InGameUI.RestartBoss ();
                break;
            case UIString.RestartTutorial:
                InGameUI.RestartTutorial ();
                break;

            case UIString.LibraryMenu:
                LibraryMenu.ShowLibrary ();
                break;
            case UIString.LibraryMenuTokenType:
                LibraryMenu.SelectButton (0, (int) tokenType);
                break;
            case UIString.LibraryMenuAbilityType:
                LibraryMenu.SelectButton (1, (int) abilityType);
                break;
            case UIString.LibraryMenuNextAbilityPage:
                LibraryMenu.NextAbilityPage ();
                break;
            case UIString.LibraryMenuPrevAbilityPage:
                LibraryMenu.PrevAbilityPage ();
                break;
            case UIString.LibraryMenuAbout:
                GOUI.ShowMessage (Language.LibraryMenuAbout);
                break;

            case UIString.ShowGameModePlayerSettingsMenu:
                GameModePlayerSettingsMenu.ShowGameModePlayerSettingsMenu ();
                break;
            case UIString.GameModePlayerSettingsApply:
                GameModePlayerSettingsMenu.Apply ();
                break;

            case UIString.ShowGameModeMenu:
            case UIString.GoBackToGameModeSelection:
                GameModeMenu.ShowGameModeMenu ();
                break;
            case UIString.GameModeMenuOfficialGameModes:
                GameModeMenu.SelectGroup (0);
                break;
            case UIString.GameModeMenuPublicGameModes:
                GameModeMenu.SelectGroup (1);
                break;
            case UIString.GameModeMenuYourGameModes:
                GameModeMenu.SelectGroup (2);
                break;
            case UIString.CreateNewGameMode:
                ClientLogic.MyInterface.CmdCreateNewGameMode (Language.NewGameVersion);
                break;
            case UIString.DeleteGameMode:
                ClientLogic.MyInterface.CmdDeleteGameMode (id);
                break;
            case UIString.GameModeListPageButton:
                GameModeMenu.SelectPage (number);
                break;
            case UIString.SelectGameMode:
                GameModeMenu.ClickOnGameMode (id);
                break;
            case UIString.ShowGameModeEditor:
                GameModeEditor.ShowGameModeEditor (id);
                break;
            case UIString.GameModeEditorAbout:
                if (GameModeEditor.editMode) {
                    GOUI.ShowMessage (Language.GameModeEditorDescription);
                } else {
                    GOUI.ShowMessage (Language.GameModeEditorReadOnlyDescription);
                }
                break;

            case UIString.GameModeEditorCreateNewBoard:
                GameModeEditor.CreateNewBoard ();
                break;
            case UIString.GameModeEditorDeleteBoard:
                GameModeEditor.DeleteBoard (id);
                break;
            case UIString.GameModeEditorEditCardPool:
                GameModeEditor.ShowCardPoolEditor ();
                break;
            case UIString.GameModeEditorSettings:
                GameModeSettingsEditor.ShowGameModeSettingsEditor ();
                break;
            case UIString.SaveGameModeSettings:
                GameModeSettingsEditor.SaveGameModeSettings ();
                break;
            case UIString.GameModeEditorEditBoard:
                BoardEditorMenu.ShowBoardEditorMenu (id);
                break;
            case UIString.GameModeEditorPageButton:
                GameModeEditor.ClickOnPageButton (number);
                break;
            case UIString.GameModeEditorSelectBoard:
                GameModeEditor.ClickOnRow (id);
                break;


            case UIString.ShowPuzzleMenu:
                PuzzleMenu.ShowPuzzleMenu ();
                break;
            case UIString.PuzzleMenuRow:
                PuzzleMenu.SelectRow (number);
                break;
            case UIString.PuzzleMenuApply:
                PuzzleMenu.ApplySelection ();
                break;
            case UIString.PuzzleMenuType:
                PuzzleMenu.SelectType (number);
                break;
            case UIString.PuzzleMenuAbout:
                GOUI.ShowMessage (Language.PuzzleAbout);
                break;
            case UIString.PuzzleMenuPageButton:
                PuzzleMenu.SelectPage (number);
                break;

            case UIString.ShowProfileMenu:
                ProfileMenu.ShowProfileMenu ();
                break;
            case UIString.ShowProfileSettings:
                ProfileSettings.ShowProfileSettings ();
                break;
            case UIString.GoBackToProfileMenu:
                ProfileMenu.GoBackToProfileMenu ();
                break;
            case UIString.ProfileSettingsApply:
                ProfileSettings.ApplySettings ();
                break;
            case UIString.ProfileSettingsAvatar:
                ProfileSettings.SelectAvatar (number);
                break;
            case UIString.InputMenuApplyInput:
                InputMenu.Apply ();
                break;

            case UIString.ShowTutorialMenu:
                TutorialMenu.ShowTutorialMenu ();
                break;
            case UIString.TutorialMenuRow:
                TutorialMenu.SelectMission (id);
                break;
            case UIString.TutorialMenuApply:
                TutorialMenu.Apply ();
                break;


            case UIString.TooltipDestroy:
                Tooltip.DestroyPermanentTooltip ();
                break;


            case UIString.ShowUnlockedContent:
                UnlockedContent.ShowUnlockedContent ();
                break;
            case UIString.ShowCodeMenu:
                InputMenu.ShowInputMenu (UIString.PromoCode);
                break;

            case UIString.ShowBossMenu:
                BossMenu.ShowBossMenu ();
                break;

            case UIString.BossMenuApply:
                BossMenu.Apply ();
                break;
            case UIString.BossMenuAbout:
                GOUI.ShowMessage (Language.BossMenuAbout);
                break;
            case UIString.BossMenuType:
                BossMenu.SelectType (number);
                break;
            case UIString.BossMenuRow:
                BossMenu.SelectRow (number);
                break;
            case UIString.SetEditorStartBossFight:
                SetEditor.StartBossFight ();
                break;
            case UIString.BossMenuPageButton:
                BossMenu.SelectPage (number);
                break;

            case UIString.ShowSettingsMenu:
                SettingsMenu.ShowSettingsMenu ();
                break;
            case UIString.SettingsMenuApply:
                SettingsMenu.Instance.Apply ();
                break;
            case UIString.SettingsMenuExit:
                SettingsMenu.Instance.Exit ();
                break;


            // BoardEditor
            case UIString.SaveBoard:
                BoardEditorMenu.SaveBoard ();
                break;
            case UIString.GoBackToGameModeEditor:
                GameModeEditor.ShowGameModeEditor ();
                break;
            case UIString.BoardEditorAbout:
                if (BoardEditorMenu.editMode) {
                    GOUI.ShowMessage (Language.BoardEditorDescription);
                } else {
                    GOUI.ShowMessage (Language.BoardEditorReadOnlyDescription);
                }
                break;

            case UIString.GameModeMenuApply:
                GameModeMenu.ApplyGameMode ();
                break;

            case UIString.ShowInGameOptionsMenu:
                OptionsMenu.ShowOptionsMenu (OptionsMenu.OptionsMenuMode.inGame);
                break;
            case UIString.DestroySubMenu:
                OptionsMenu.DestroySubMenu ();
                break;
            case UIString.ExitApp:
                Application.Quit ();
                break;

            case UIString.ShowOptionsMenu:
                OptionsMenu.ShowOptionsMenu (OptionsMenu.OptionsMenuMode.normal);
                break;

            // Other
            default:
                Debug.Log (name, gameObject);
                break;
        }
    }

    void OnMouseOverAction () {

    }

    private void OnMouseOver () {
        if (!EventSystem.current.IsPointerOverGameObject ()) {
            if (Over) {
                if (timer < timeToTooltip && timer + Time.deltaTime >= timeToTooltip) {
                    CreateTooltip ();
                }
                timer += Time.deltaTime;
            }
            if (Input.GetMouseButtonDown (0)) {
                if (interactible) {
                    SetOnMouseClickSprite ();
                    Pressed = true;
                }
            }
            if (Input.GetAxis ("Mouse ScrollWheel") > 0f) {
                switch (name) {
                    case UIString.InGameHandCard:
                        CardAnimation.SetStackZoomed (x, true);
                        break;
                }
            }
            if (Input.GetAxis ("Mouse ScrollWheel") < 0f) {
                switch (name) {
                    case UIString.InGameHandCard:
                        CardAnimation.SetStackZoomed (x, false);
                        break;
                }
            }
            if (Input.GetMouseButtonDown (1)) {
                Pressed1 = true;
            }
            if (Input.GetMouseButtonUp (1)) {
                if (Pressed1) {
                    switch (name) {
                        case UIString.SetEditorSetCard:
                            SetEditor.RotateCardInSet (x, y);
                            break;
                        case UIString.SetEditorCollectionCard:
                            SetEditor.RotateCardInCollection (x, y);
                            break;

                        case UIString.LibraryMenuTokenType:
                            LibraryMenu.EditElementToggle (0, (int) tokenType);
                            break;
                        case UIString.LibraryMenuAbilityType:
                            LibraryMenu.EditElementToggle (1, (int) abilityType);
                            break;

                        case UIString.InGameHandCard:
                            InGameUI.RotateAbilityAreaOnServer (x);
                            break;
                        case UIString.PlayedCard:
                            UsedCardPreview.DestoryPreview (x);
                            break;
                    }
                }
                Pressed1 = false;
            }

            if (Input.GetMouseButtonUp (0)) {
                if (Pressed) {
                    OnClickAction ();
                }
                Pressed = false;
            }
        } else {
            //Debug.Log (name);
        }
    }

    public void OnMouseEnterTileAction () {
        if (BoardEditorMenu.instance != null) {
        }
        if (InGameUI.instance != null) {
            InGameUI.SetAreaHovers (x, y);
        }
    }

    public void OnMouseOverTileAction () {
        if (InGameUI.instance != null) {
            InGameUI.CheckHideAreaCovers (x, y);
        }
    }

    public void TileAction () {
        if (BoardEditorMenu.instance != null) {
            BoardEditorMenu.TileAction (x, y);
        }
        if (InGameUI.instance != null) {
            InGameUI.TileAction (x, y);
        }
    }

    public void OnMouseLeaveTileAction () {
        if (InGameUI.instance != null) {
            InGameUI.HideAreaHovers ();
        }
    }

    void DestroyReferences () {
        foreach (GameObject Ref in references) {
            DestroyImmediate (Ref);
        }
        DestroyImmediate (gameObject);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonUp (0)) {
            FreeButton ();
        }
        if (Entered){// && !EventSystem.current.IsPointerOverGameObject ()) {
            Over = true;
            Entered = false;
            if (!Pressed && interactible) {
                OnEnter ();
            }
            switch (name) {
                case UIString.Tile:
                    OnMouseEnterTileAction ();
                    break;
            }
        }
        if (Over) {
            if (!EventSystem.current.IsPointerOverGameObject ()) {
                switch (name) {
                    case "Tile":
                        OnMouseOverTileAction ();
                        break;
                }
            }
            switch (name) {
                case UIString.InGameScoreBar:
                    visualTeam.DisplayDetailedHealthBar ();
                    break;
            }
        } else {
            switch (name) {
                case UIString.InGameScoreBar:
                    visualTeam.DisplaySimplifiedHealthBar ();
                    break;
            }
        }
    }

    public void FreeButton () {
        Pressed = false;
        if (Over) {
            SetOnMouseOverSprite ();
        } else {
            SetNormalSprite ();
        }
    }

    void SetSprite (Sprite sprite) {
        if (sprite != null){
            if (GetComponent<SpriteRenderer> () != null) {
                GetComponent<SpriteRenderer> ().sprite = sprite;
            } else if (GetComponent<Image> () != null) {
                GetComponent<Image> ().sprite = sprite;
            }
        }
    }
}
