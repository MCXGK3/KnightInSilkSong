using System;
using System.Collections.Generic;
using System.Reflection;
using GlobalEnums;
using UnityEngine;
namespace Knight;

[Serializable]
public class PlayerData
{
	private enum MapBools
	{
		MapDirtmouth,
		MapCrossroads,
		MapGreenpath,
		MapFogCanyon,
		MapRoyalGardens,
		MapFungalWastes,
		MapCity,
		MapWaterways,
		MapMines,
		MapDeepnest,
		MapCliffs,
		MapOutskirts,
		MapRestingGrounds,
		MapAbyss
	}

	public string version;

	public bool awardAllAchievements;

	public int profileID;

	public float playTime;

	public float completionPercent;

	public bool openingCreditsPlayed;

	public int permadeathMode;

	public int health;

	public int maxHealth;

	public int maxHealthBase;

	public int healthBlue;

	public int joniHealthBlue;

	public bool damagedBlue;

	public int heartPieces;

	public bool heartPieceCollected;

	public int maxHealthCap;

	public bool heartPieceMax;

	public int prevHealth;

	public int blockerHits;

	public bool firstGeo;

	public int geo;

	public int maxMP;

	public int MPCharge;

	public int MPReserve;

	public int MPReserveMax;

	public bool soulLimited;

	public int vesselFragments;

	public bool vesselFragmentCollected;

	public int MPReserveCap;

	public bool vesselFragmentMax;

	public int focusMP_amount;

	public bool atBench;

	public string respawnScene;

	public MapZone mapZone;

	public string respawnMarkerName;

	public int respawnType;

	public bool respawnFacingRight;

	[NonSerialized]
	public Vector3 hazardRespawnLocation;

	public bool hazardRespawnFacingRight;
	public HazardRespawnMarker.FacingDirection hazardRespawnFacing;

	public string shadeScene;

	public string shadeMapZone;

	public float shadePositionX;

	public float shadePositionY;

	public int shadeHealth;

	public int shadeMP;

	public int shadeFireballLevel;

	public int shadeQuakeLevel;

	public int shadeScreamLevel;

	public int shadeSpecialType;

	public Vector3 shadeMapPos;

	public Vector3 dreamgateMapPos;

	public int geoPool;

	public int nailDamage;

	public int nailRange;

	public int beamDamage;

	public bool canDash;

	public bool canBackDash;

	public bool canWallJump;

	public bool canSuperDash;

	public bool canShadowDash;

	public bool hasSpell;

	public int fireballLevel;

	public int quakeLevel;

	public int screamLevel;

	public bool hasNailArt;

	public bool hasCyclone;

	public bool hasDashSlash;

	public bool hasUpwardSlash;

	public bool hasAllNailArts;

	public bool hasDreamNail;

	public bool hasDreamGate;

	public bool dreamNailUpgraded;

	public int dreamOrbs;

	public int dreamOrbsSpent;

	public string dreamGateScene;

	public float dreamGateX;

	public float dreamGateY;

	public bool hasDash;

	public bool hasWalljump;

	public bool hasSuperDash;

	public bool hasShadowDash;

	public bool hasAcidArmour;

	public bool hasDoubleJump;

	public bool hasLantern;

	public bool hasTramPass;

	public bool hasQuill;

	public bool hasCityKey;

	public bool hasSlykey;

	public bool gaveSlykey;

	public bool hasWhiteKey;

	public bool usedWhiteKey;

	public bool hasMenderKey;

	public bool hasWaterwaysKey;

	public bool hasSpaKey;

	public bool hasLoveKey;

	public bool hasKingsBrand;

	public bool hasXunFlower;

	public int ghostCoins;

	public int ore;

	public bool foundGhostCoin;

	public int trinket1;

	public bool foundTrinket1;

	public int trinket2;

	public bool foundTrinket2;

	public int trinket3;

	public bool foundTrinket3;

	public int trinket4;

	public bool foundTrinket4;

	public bool noTrinket1;

	public bool noTrinket2;

	public bool noTrinket3;

	public bool noTrinket4;

	public int soldTrinket1;

	public int soldTrinket2;

	public int soldTrinket3;

	public int soldTrinket4;

	public int simpleKeys;

	public int rancidEggs;

	public bool notchShroomOgres;

	public bool notchFogCanyon;

	public bool gotLurkerKey;

	public float gMap_doorX;

	public float gMap_doorY;

	public string gMap_doorScene;

	public string gMap_doorMapZone;

	public float gMap_doorOriginOffsetX;

	public float gMap_doorOriginOffsetY;

	public float gMap_doorSceneWidth;

	public float gMap_doorSceneHeight;

	public int guardiansDefeated;

	public bool lurienDefeated;

	public bool hegemolDefeated;

	public bool monomonDefeated;

	public bool maskBrokenLurien;

	public bool maskBrokenHegemol;

	public bool maskBrokenMonomon;

	public int maskToBreak;

	public int elderbug;

	public bool metElderbug;

	public bool elderbugReintro;

	public int elderbugHistory;

	public bool elderbugHistory1;

	public bool elderbugHistory2;

	public bool elderbugHistory3;

	public bool elderbugSpeechSly;

	public bool elderbugSpeechStation;

	public bool elderbugSpeechEggTemple;

	public bool elderbugSpeechMapShop;

	public bool elderbugSpeechBretta;

	public bool elderbugSpeechJiji;

	public bool elderbugSpeechMinesLift;

	public bool elderbugSpeechKingsPass;

	public bool elderbugSpeechInfectedCrossroads;

	public bool elderbugSpeechFinalBossDoor;

	public bool elderbugRequestedFlower;

	public bool elderbugGaveFlower;

	public bool elderbugFirstCall;

	public bool metQuirrel;

	public int quirrelEggTemple;

	public int quirrelSlugShrine;

	public int quirrelRuins;

	public int quirrelMines;

	public bool quirrelLeftStation;

	public bool quirrelLeftEggTemple;

	public bool quirrelCityEncountered;

	public bool quirrelCityLeft;

	public bool quirrelMinesEncountered;

	public bool quirrelMinesLeft;

	public bool quirrelMantisEncountered;

	public bool enteredMantisLordArea;

	public bool visitedDeepnestSpa;

	public bool quirrelSpaReady;

	public bool quirrelSpaEncountered;

	public bool quirrelArchiveEncountered;

	public bool quirrelEpilogueCompleted;

	public bool metRelicDealer;

	public bool metRelicDealerShop;

	public bool marmOutside;

	public bool marmOutsideConvo;

	public bool marmConvo1;

	public bool marmConvo2;

	public bool marmConvo3;

	public bool marmConvoNailsmith;

	public int cornifer;

	public bool metCornifer;

	public bool corniferIntroduced;

	public bool corniferAtHome;

	public bool corn_crossroadsEncountered;

	public bool corn_crossroadsLeft;

	public bool corn_greenpathEncountered;

	public bool corn_greenpathLeft;

	public bool corn_fogCanyonEncountered;

	public bool corn_fogCanyonLeft;

	public bool corn_fungalWastesEncountered;

	public bool corn_fungalWastesLeft;

	public bool corn_cityEncountered;

	public bool corn_cityLeft;

	public bool corn_waterwaysEncountered;

	public bool corn_waterwaysLeft;

	public bool corn_minesEncountered;

	public bool corn_minesLeft;

	public bool corn_cliffsEncountered;

	public bool corn_cliffsLeft;

	public bool corn_deepnestEncountered;

	public bool corn_deepnestLeft;

	public bool corn_deepnestMet1;

	public bool corn_deepnestMet2;

	public bool corn_outskirtsEncountered;

	public bool corn_outskirtsLeft;

	public bool corn_royalGardensEncountered;

	public bool corn_royalGardensLeft;

	public bool corn_abyssEncountered;

	public bool corn_abyssLeft;

	public bool metIselda;

	public bool iseldaCorniferHomeConvo;

	public bool iseldaConvo1;

	public bool brettaRescued;

	public int brettaPosition;

	public int brettaState;

	public bool brettaSeenBench;

	public bool brettaSeenBed;

	public bool brettaSeenBenchDiary;

	public bool brettaSeenBedDiary;

	public bool brettaLeftTown;

	public bool slyRescued;

	public bool slyBeta;

	public bool metSlyShop;

	public bool gotSlyCharm;

	public bool slyShellFrag1;

	public bool slyShellFrag2;

	public bool slyShellFrag3;

	public bool slyShellFrag4;

	public bool slyVesselFrag1;

	public bool slyVesselFrag2;

	public bool slyVesselFrag3;

	public bool slyVesselFrag4;

	public bool slyNotch1;

	public bool slyNotch2;

	public bool slySimpleKey;

	public bool slyRancidEgg;

	public bool slyConvoNailArt;

	public bool slyConvoMapper;

	public bool slyConvoNailHoned;

	public bool jijiDoorUnlocked;

	public bool jijiMet;

	public bool jijiShadeOffered;

	public bool jijiShadeCharmConvo;

	public bool metJinn;

	public bool jinnConvo1;

	public bool jinnConvo2;

	public bool jinnConvo3;

	public bool jinnConvoKingBrand;

	public bool jinnConvoShadeCharm;

	public int jinnEggsSold;

	public int zote;

	public bool zoteRescuedBuzzer;

	public bool zoteDead;

	public int zoteDeathPos;

	public bool zoteSpokenCity;

	public bool zoteLeftCity;

	public bool zoteTrappedDeepnest;

	public bool zoteRescuedDeepnest;

	public bool zoteDefeated;

	public bool zoteSpokenColosseum;

	public int zotePrecept;

	public int zoteTownConvo;

	public int shaman;

	public bool shamanScreamConvo;

	public bool shamanQuakeConvo;

	public bool shamanFireball2Convo;

	public bool shamanScream2Convo;

	public bool shamanQuake2Convo;

	public bool metMiner;

	public int miner;

	public int minerEarly;

	public int hornetGreenpath;

	public int hornetFung;

	public bool hornet_f19;

	public bool hornetFountainEncounter;

	public bool hornetCityBridge_ready;

	public bool hornetCityBridge_completed;

	public bool hornetAbyssEncounter;

	public bool hornetDenEncounter;

	public bool metMoth;

	public bool ignoredMoth;

	public bool gladeDoorOpened;

	public bool mothDeparted;

	public bool completedRGDreamPlant;

	public bool dreamReward1;

	public bool dreamReward2;

	public bool dreamReward3;

	public bool dreamReward4;

	public bool dreamReward5;

	public bool dreamReward5b;

	public bool dreamReward6;

	public bool dreamReward7;

	public bool dreamReward8;

	public bool dreamReward9;

	public bool dreamMothConvo1;

	public bool bankerAccountPurchased;

	public bool metBanker;

	public int bankerBalance;

	public bool bankerDeclined;

	public bool bankerTheftCheck;

	public int bankerTheft;

	public bool bankerSpaMet;

	public bool metGiraffe;

	public bool metCharmSlug;

	public bool salubraNotch1;

	public bool salubraNotch2;

	public bool salubraNotch3;

	public bool salubraNotch4;

	public bool salubraBlessing;

	public bool salubraConvoCombo;

	public bool salubraConvoOvercharm;

	public bool salubraConvoTruth;

	public bool cultistTransformed;

	public bool metNailsmith;

	public int nailSmithUpgrades;

	public bool honedNail;

	public bool nailsmithCliff;

	public bool nailsmithKilled;

	public bool nailsmithSpared;

	public bool nailsmithKillSpeech;

	public bool nailsmithSheo;

	public bool nailsmithConvoArt;

	public bool metNailmasterMato;

	public bool metNailmasterSheo;

	public bool metNailmasterOro;

	public bool matoConvoSheo;

	public bool matoConvoOro;

	public bool matoConvoSly;

	public bool sheoConvoMato;

	public bool sheoConvoOro;

	public bool sheoConvoSly;

	public bool sheoConvoNailsmith;

	public bool oroConvoSheo;

	public bool oroConvoMato;

	public bool oroConvoSly;

	public bool hunterRoared;

	public bool metHunter;

	public bool hunterRewardOffered;

	public bool huntersMarkOffered;

	public bool hasHuntersMark;

	public bool metLegEater;

	public bool paidLegEater;

	public bool refusedLegEater;

	public bool legEaterConvo1;

	public bool legEaterConvo2;

	public bool legEaterConvo3;

	public bool legEaterBrokenConvo;

	public bool legEaterDungConvo;

	public bool legEaterInfectedCrossroadConvo;

	public bool legEaterBoughtConvo;

	public bool legEaterGoldConvo;

	public bool legEaterLeft;

	public bool tukMet;

	public int tukEggPrice;

	public bool tukDungEgg;

	public bool metEmilitia;

	public bool emilitiaKingsBrandConvo;

	public bool metCloth;

	public bool clothEnteredTramRoom;

	public bool savedCloth;

	public bool clothEncounteredQueensGarden;

	public bool clothKilled;

	public bool clothInTown;

	public bool clothLeftTown;

	public bool clothGhostSpoken;

	public bool bigCatHitTail;

	public bool bigCatHitTailConvo;

	public bool bigCatMeet;

	public bool bigCatTalk1;

	public bool bigCatTalk2;

	public bool bigCatTalk3;

	public bool bigCatKingsBrandConvo;

	public bool bigCatShadeConvo;

	public bool tisoEncounteredTown;

	public bool tisoEncounteredBench;

	public bool tisoEncounteredLake;

	public bool tisoEncounteredColosseum;

	public bool tisoDead;

	public bool tisoShieldConvo;

	public int mossCultist;

	public bool maskmakerMet;

	public bool maskmakerConvo1;

	public bool maskmakerConvo2;

	public bool maskmakerUnmasked1;

	public bool maskmakerUnmasked2;

	public bool maskmakerShadowDash;

	public bool maskmakerKingsBrand;

	public bool dungDefenderConvo1;

	public bool dungDefenderConvo2;

	public bool dungDefenderConvo3;

	public bool dungDefenderCharmConvo;

	public bool dungDefenderIsmaConvo;

	public bool dungDefenderAwoken;

	public bool dungDefenderLeft;

	public bool dungDefenderAwakeConvo;

	public bool midwifeMet;

	public bool midwifeConvo1;

	public bool midwifeConvo2;

	public bool metQueen;

	public bool queenTalk1;

	public bool queenTalk2;

	public bool queenDung1;

	public bool queenDung2;

	public bool queenHornet;

	public bool queenTalkExtra;

	public bool gotQueenFragment;

	public bool queenConvo_grimm1;

	public bool queenConvo_grimm2;

	public bool gotKingFragment;

	public bool metXun;

	public bool xunFailedConvo1;

	public bool xunFailedConvo2;

	public bool xunFlowerBroken;

	public int xunFlowerBrokeTimes;

	public bool xunFlowerGiven;

	public bool xunRewardGiven;

	public int menderState;

	public bool menderSignBroken;

	public bool allBelieverTabletsDestroyed;

	public int mrMushroomState;

	public bool openedMapperShop;

	public bool openedSlyShop;

	public bool metStag;

	public bool travelling;

	public int stagPosition;

	public int stationsOpened;

	public bool stagConvoTram;

	public bool stagConvoTiso;

	public bool stagRemember1;

	public bool stagRemember2;

	public bool stagRemember3;

	public bool stagEggInspected;

	public bool stagHopeConvo;

	public string nextScene;

	public bool littleFoolMet;

	public bool ranAway;

	public bool seenColosseumTitle;

	public bool colosseumBronzeOpened;

	public bool colosseumBronzeCompleted;

	public bool colosseumSilverOpened;

	public bool colosseumSilverCompleted;

	public bool colosseumGoldOpened;

	public bool colosseumGoldCompleted;

	public bool openedTown;

	public bool openedTownBuilding;

	public bool openedCrossroads;

	public bool openedGreenpath;

	public bool openedRuins1;

	public bool openedRuins2;

	public bool openedFungalWastes;

	public bool openedRoyalGardens;

	public bool openedRestingGrounds;

	public bool openedDeepnest;

	public bool openedStagNest;

	public bool openedHiddenStation;

	public string dreamReturnScene;

	public int charmSlots;

	public int charmSlotsFilled;

	public bool hasCharm;

	public List<int> equippedCharms;

	public bool charmBenchMsg;

	public int charmsOwned;

	public bool canOvercharm;

	public bool overcharmed;

	public bool gotCharm_1;

	public bool equippedCharm_1;

	public int charmCost_1;

	public bool newCharm_1;

	public bool gotCharm_2;

	public bool equippedCharm_2;

	public int charmCost_2;

	public bool newCharm_2;

	public bool gotCharm_3;

	public bool equippedCharm_3;

	public int charmCost_3;

	public bool newCharm_3;

	public bool gotCharm_4;

	public bool equippedCharm_4;

	public int charmCost_4;

	public bool newCharm_4;

	public bool gotCharm_5;

	public bool equippedCharm_5;

	public int charmCost_5;

	public bool newCharm_5;

	public bool gotCharm_6;

	public bool equippedCharm_6;

	public int charmCost_6;

	public bool newCharm_6;

	public bool gotCharm_7;

	public bool equippedCharm_7;

	public int charmCost_7;

	public bool newCharm_7;

	public bool gotCharm_8;

	public bool equippedCharm_8;

	public int charmCost_8;

	public bool newCharm_8;

	public bool gotCharm_9;

	public bool equippedCharm_9;

	public int charmCost_9;

	public bool newCharm_9;

	public bool gotCharm_10;

	public bool equippedCharm_10;

	public int charmCost_10;

	public bool newCharm_10;

	public bool gotCharm_11;

	public bool equippedCharm_11;

	public int charmCost_11;

	public bool newCharm_11;

	public bool gotCharm_12;

	public bool equippedCharm_12;

	public int charmCost_12;

	public bool newCharm_12;

	public bool gotCharm_13;

	public bool equippedCharm_13;

	public int charmCost_13;

	public bool newCharm_13;

	public bool gotCharm_14;

	public bool equippedCharm_14;

	public int charmCost_14;

	public bool newCharm_14;

	public bool gotCharm_15;

	public bool equippedCharm_15;

	public int charmCost_15;

	public bool newCharm_15;

	public bool gotCharm_16;

	public bool equippedCharm_16;

	public int charmCost_16;

	public bool newCharm_16;

	public bool gotCharm_17;

	public bool equippedCharm_17;

	public int charmCost_17;

	public bool newCharm_17;

	public bool gotCharm_18;

	public bool equippedCharm_18;

	public int charmCost_18;

	public bool newCharm_18;

	public bool gotCharm_19;

	public bool equippedCharm_19;

	public int charmCost_19;

	public bool newCharm_19;

	public bool gotCharm_20;

	public bool equippedCharm_20;

	public int charmCost_20;

	public bool newCharm_20;

	public bool gotCharm_21;

	public bool equippedCharm_21;

	public int charmCost_21;

	public bool newCharm_21;

	public bool gotCharm_22;

	public bool equippedCharm_22;

	public int charmCost_22;

	public bool newCharm_22;

	public bool gotCharm_23;

	public bool equippedCharm_23;

	public bool brokenCharm_23;

	public int charmCost_23;

	public bool newCharm_23;

	public bool gotCharm_24;

	public bool equippedCharm_24;

	public bool brokenCharm_24;

	public int charmCost_24;

	public bool newCharm_24;

	public bool gotCharm_25;

	public bool equippedCharm_25;

	public bool brokenCharm_25;

	public int charmCost_25;

	public bool newCharm_25;

	public bool gotCharm_26;

	public bool equippedCharm_26;

	public int charmCost_26;

	public bool newCharm_26;

	public bool gotCharm_27;

	public bool equippedCharm_27;

	public int charmCost_27;

	public bool newCharm_27;

	public bool gotCharm_28;

	public bool equippedCharm_28;

	public int charmCost_28;

	public bool newCharm_28;

	public bool gotCharm_29;

	public bool equippedCharm_29;

	public int charmCost_29;

	public bool newCharm_29;

	public bool gotCharm_30;

	public bool equippedCharm_30;

	public int charmCost_30;

	public bool newCharm_30;

	public bool gotCharm_31;

	public bool equippedCharm_31;

	public int charmCost_31;

	public bool newCharm_31;

	public bool gotCharm_32;

	public bool equippedCharm_32;

	public int charmCost_32;

	public bool newCharm_32;

	public bool gotCharm_33;

	public bool equippedCharm_33;

	public int charmCost_33;

	public bool newCharm_33;

	public bool gotCharm_34;

	public bool equippedCharm_34;

	public int charmCost_34;

	public bool newCharm_34;

	public bool gotCharm_35;

	public bool equippedCharm_35;

	public int charmCost_35;

	public bool newCharm_35;

	public bool gotCharm_36;

	public bool equippedCharm_36;

	public int charmCost_36;

	public bool newCharm_36;

	public bool gotCharm_37;

	public bool equippedCharm_37;

	public int charmCost_37;

	public bool newCharm_37;

	public bool gotCharm_38;

	public bool equippedCharm_38;

	public int charmCost_38;

	public bool newCharm_38;

	public bool gotCharm_39;

	public bool equippedCharm_39;

	public int charmCost_39;

	public bool newCharm_39;

	public bool gotCharm_40;

	public bool equippedCharm_40;

	public int charmCost_40;

	public bool newCharm_40;

	public bool fragileHealth_unbreakable;

	public bool fragileGreed_unbreakable;

	public bool fragileStrength_unbreakable;

	public int royalCharmState;

	public bool hasJournal;

	public int lastJournalItem;

	public bool killedDummy;

	public int killsDummy;

	public bool newDataDummy;

	public bool seenJournalMsg;

	public bool seenHunterMsg;

	public bool fillJournal;

	public int journalEntriesCompleted;

	public int journalNotesCompleted;

	public int journalEntriesTotal;

	public bool killedCrawler;

	public int killsCrawler;

	public bool newDataCrawler;

	public bool killedBuzzer;

	public int killsBuzzer;

	public bool newDataBuzzer;

	public bool killedBouncer;

	public int killsBouncer;

	public bool newDataBouncer;

	public bool killedClimber;

	public int killsClimber;

	public bool newDataClimber;

	public bool killedHopper;

	public int killsHopper;

	public bool newDataHopper;

	public bool killedWorm;

	public int killsWorm;

	public bool newDataWorm;

	public bool killedSpitter;

	public int killsSpitter;

	public bool newDataSpitter;

	public bool killedHatcher;

	public int killsHatcher;

	public bool newDataHatcher;

	public bool killedHatchling;

	public int killsHatchling;

	public bool newDataHatchling;

	public bool killedZombieRunner;

	public int killsZombieRunner;

	public bool newDataZombieRunner;

	public bool killedZombieHornhead;

	public int killsZombieHornhead;

	public bool newDataZombieHornhead;

	public bool killedZombieLeaper;

	public int killsZombieLeaper;

	public bool newDataZombieLeaper;

	public bool killedZombieBarger;

	public int killsZombieBarger;

	public bool newDataZombieBarger;

	public bool killedZombieShield;

	public int killsZombieShield;

	public bool newDataZombieShield;

	public bool killedZombieGuard;

	public int killsZombieGuard;

	public bool newDataZombieGuard;

	public bool killedBigBuzzer;

	public int killsBigBuzzer;

	public bool newDataBigBuzzer;

	public bool killedBigFly;

	public int killsBigFly;

	public bool newDataBigFly;

	public bool killedMawlek;

	public int killsMawlek;

	public bool newDataMawlek;

	public bool killedFalseKnight;

	public int killsFalseKnight;

	public bool newDataFalseKnight;

	public bool killedRoller;

	public int killsRoller;

	public bool newDataRoller;

	public bool killedBlocker;

	public int killsBlocker;

	public bool newDataBlocker;

	public bool killedPrayerSlug;

	public int killsPrayerSlug;

	public bool newDataPrayerSlug;

	public bool killedMenderBug;

	public int killsMenderBug;

	public bool newDataMenderBug;

	public bool killedMossmanRunner;

	public int killsMossmanRunner;

	public bool newDataMossmanRunner;

	public bool killedMossmanShaker;

	public int killsMossmanShaker;

	public bool newDataMossmanShaker;

	public bool killedMosquito;

	public int killsMosquito;

	public bool newDataMosquito;

	public bool killedBlobFlyer;

	public int killsBlobFlyer;

	public bool newDataBlobFlyer;

	public bool killedFungifiedZombie;

	public int killsFungifiedZombie;

	public bool newDataFungifiedZombie;

	public bool killedPlantShooter;

	public int killsPlantShooter;

	public bool newDataPlantShooter;

	public bool killedMossCharger;

	public int killsMossCharger;

	public bool newDataMossCharger;

	public bool killedMegaMossCharger;

	public int killsMegaMossCharger;

	public bool newDataMegaMossCharger;

	public bool killedSnapperTrap;

	public int killsSnapperTrap;

	public bool newDataSnapperTrap;

	public bool killedMossKnight;

	public int killsMossKnight;

	public bool newDataMossKnight;

	public bool killedGrassHopper;

	public int killsGrassHopper;

	public bool newDataGrassHopper;

	public bool killedAcidFlyer;

	public int killsAcidFlyer;

	public bool newDataAcidFlyer;

	public bool killedAcidWalker;

	public int killsAcidWalker;

	public bool newDataAcidWalker;

	public bool killedMossFlyer;

	public int killsMossFlyer;

	public bool newDataMossFlyer;

	public bool killedMossKnightFat;

	public int killsMossKnightFat;

	public bool newDataMossKnightFat;

	public bool killedMossWalker;

	public int killsMossWalker;

	public bool newDataMossWalker;

	public bool killedInfectedKnight;

	public int killsInfectedKnight;

	public bool newDataInfectedKnight;

	public bool killedLazyFlyer;

	public int killsLazyFlyer;

	public bool newDataLazyFlyer;

	public bool killedZapBug;

	public int killsZapBug;

	public bool newDataZapBug;

	public bool killedJellyfish;

	public int killsJellyfish;

	public bool newDataJellyfish;

	public bool killedJellyCrawler;

	public int killsJellyCrawler;

	public bool newDataJellyCrawler;

	public bool killedMegaJellyfish;

	public int killsMegaJellyfish;

	public bool newDataMegaJellyfish;

	public bool killedFungoonBaby;

	public int killsFungoonBaby;

	public bool newDataFungoonBaby;

	public bool killedMushroomTurret;

	public int killsMushroomTurret;

	public bool newDataMushroomTurret;

	public bool killedMantis;

	public int killsMantis;

	public bool newDataMantis;

	public bool killedMushroomRoller;

	public int killsMushroomRoller;

	public bool newDataMushroomRoller;

	public bool killedMushroomBrawler;

	public int killsMushroomBrawler;

	public bool newDataMushroomBrawler;

	public bool killedMushroomBaby;

	public int killsMushroomBaby;

	public bool newDataMushroomBaby;

	public bool killedMantisFlyerChild;

	public int killsMantisFlyerChild;

	public bool newDataMantisFlyerChild;

	public bool killedFungusFlyer;

	public int killsFungusFlyer;

	public bool newDataFungusFlyer;

	public bool killedFungCrawler;

	public int killsFungCrawler;

	public bool newDataFungCrawler;

	public bool killedMantisLord;

	public int killsMantisLord;

	public bool newDataMantisLord;

	public bool killedBlackKnight;

	public int killsBlackKnight;

	public bool newDataBlackKnight;

	public bool killedElectricMage;

	public int killsElectricMage;

	public bool newDataElectricMage;

	public bool killedMage;

	public int killsMage;

	public bool newDataMage;

	public bool killedMageKnight;

	public int killsMageKnight;

	public bool newDataMageKnight;

	public bool killedRoyalDandy;

	public int killsRoyalDandy;

	public bool newDataRoyalDandy;

	public bool killedRoyalCoward;

	public int killsRoyalCoward;

	public bool newDataRoyalCoward;

	public bool killedRoyalPlumper;

	public int killsRoyalPlumper;

	public bool newDataRoyalPlumper;

	public bool killedFlyingSentrySword;

	public int killsFlyingSentrySword;

	public bool newDataFlyingSentrySword;

	public bool killedFlyingSentryJavelin;

	public int killsFlyingSentryJavelin;

	public bool newDataFlyingSentryJavelin;

	public bool killedSentry;

	public int killsSentry;

	public bool newDataSentry;

	public bool killedSentryFat;

	public int killsSentryFat;

	public bool newDataSentryFat;

	public bool killedMageBlob;

	public int killsMageBlob;

	public bool newDataMageBlob;

	public bool killedGreatShieldZombie;

	public int killsGreatShieldZombie;

	public bool newDataGreatShieldZombie;

	public bool killedJarCollector;

	public int killsJarCollector;

	public bool newDataJarCollector;

	public bool killedMageBalloon;

	public int killsMageBalloon;

	public bool newDataMageBalloon;

	public bool killedMageLord;

	public int killsMageLord;

	public bool newDataMageLord;

	public bool killedGorgeousHusk;

	public int killsGorgeousHusk;

	public bool newDataGorgeousHusk;

	public bool killedFlipHopper;

	public int killsFlipHopper;

	public bool newDataFlipHopper;

	public bool killedFlukeman;

	public int killsFlukeman;

	public bool newDataFlukeman;

	public bool killedInflater;

	public int killsInflater;

	public bool newDataInflater;

	public bool killedFlukefly;

	public int killsFlukefly;

	public bool newDataFlukefly;

	public bool killedFlukeMother;

	public int killsFlukeMother;

	public bool newDataFlukeMother;

	public bool killedDungDefender;

	public int killsDungDefender;

	public bool newDataDungDefender;

	public bool killedCrystalCrawler;

	public int killsCrystalCrawler;

	public bool newDataCrystalCrawler;

	public bool killedCrystalFlyer;

	public int killsCrystalFlyer;

	public bool newDataCrystalFlyer;

	public bool killedLaserBug;

	public int killsLaserBug;

	public bool newDataLaserBug;

	public bool killedBeamMiner;

	public int killsBeamMiner;

	public bool newDataBeamMiner;

	public bool killedZombieMiner;

	public int killsZombieMiner;

	public bool newDataZombieMiner;

	public bool killedMegaBeamMiner;

	public int killsMegaBeamMiner;

	public bool newDataMegaBeamMiner;

	public bool killedMinesCrawler;

	public int killsMinesCrawler;

	public bool newDataMinesCrawler;

	public bool killedAngryBuzzer;

	public int killsAngryBuzzer;

	public bool newDataAngryBuzzer;

	public bool killedBurstingBouncer;

	public int killsBurstingBouncer;

	public bool newDataBurstingBouncer;

	public bool killedBurstingZombie;

	public int killsBurstingZombie;

	public bool newDataBurstingZombie;

	public bool killedSpittingZombie;

	public int killsSpittingZombie;

	public bool newDataSpittingZombie;

	public bool killedBabyCentipede;

	public int killsBabyCentipede;

	public bool newDataBabyCentipede;

	public bool killedBigCentipede;

	public int killsBigCentipede;

	public bool newDataBigCentipede;

	public bool killedCentipedeHatcher;

	public int killsCentipedeHatcher;

	public bool newDataCentipedeHatcher;

	public bool killedLesserMawlek;

	public int killsLesserMawlek;

	public bool newDataLesserMawlek;

	public bool killedSlashSpider;

	public int killsSlashSpider;

	public bool newDataSlashSpider;

	public bool killedSpiderCorpse;

	public int killsSpiderCorpse;

	public bool newDataSpiderCorpse;

	public bool killedShootSpider;

	public int killsShootSpider;

	public bool newDataShootSpider;

	public bool killedMiniSpider;

	public int killsMiniSpider;

	public bool newDataMiniSpider;

	public bool killedSpiderFlyer;

	public int killsSpiderFlyer;

	public bool newDataSpiderFlyer;

	public bool killedMimicSpider;

	public int killsMimicSpider;

	public bool newDataMimicSpider;

	public bool killedBeeHatchling;

	public int killsBeeHatchling;

	public bool newDataBeeHatchling;

	public bool killedBeeStinger;

	public int killsBeeStinger;

	public bool newDataBeeStinger;

	public bool killedBigBee;

	public int killsBigBee;

	public bool newDataBigBee;

	public bool killedHiveKnight;

	public int killsHiveKnight;

	public bool newDataHiveKnight;

	public bool killedBlowFly;

	public int killsBlowFly;

	public bool newDataBlowFly;

	public bool killedCeilingDropper;

	public int killsCeilingDropper;

	public bool newDataCeilingDropper;

	public bool killedGiantHopper;

	public int killsGiantHopper;

	public bool newDataGiantHopper;

	public bool killedGrubMimic;

	public int killsGrubMimic;

	public bool newDataGrubMimic;

	public bool killedMawlekTurret;

	public int killsMawlekTurret;

	public bool newDataMawlekTurret;

	public bool killedOrangeScuttler;

	public int killsOrangeScuttler;

	public bool newDataOrangeScuttler;

	public bool killedHealthScuttler;

	public int killsHealthScuttler;

	public bool newDataHealthScuttler;

	public bool killedPigeon;

	public int killsPigeon;

	public bool newDataPigeon;

	public bool killedZombieHive;

	public int killsZombieHive;

	public bool newDataZombieHive;

	public bool killedDreamGuard;

	public int killsDreamGuard;

	public bool newDataDreamGuard;

	public bool killedHornet;

	public int killsHornet;

	public bool newDataHornet;

	public bool killedAbyssCrawler;

	public int killsAbyssCrawler;

	public bool newDataAbyssCrawler;

	public bool killedSuperSpitter;

	public int killsSuperSpitter;

	public bool newDataSuperSpitter;

	public bool killedSibling;

	public int killsSibling;

	public bool newDataSibling;

	public bool killedPalaceFly;

	public int killsPalaceFly;

	public bool newDataPalaceFly;

	public bool killedEggSac;

	public int killsEggSac;

	public bool newDataEggSac;

	public bool killedMummy;

	public int killsMummy;

	public bool newDataMummy;

	public bool killedOrangeBalloon;

	public int killsOrangeBalloon;

	public bool newDataOrangeBalloon;

	public bool killedAbyssTendril;

	public int killsAbyssTendril;

	public bool newDataAbyssTendril;

	public bool killedHeavyMantis;

	public int killsHeavyMantis;

	public bool newDataHeavyMantis;

	public bool killedTraitorLord;

	public int killsTraitorLord;

	public bool newDataTraitorLord;

	public bool killedMantisHeavyFlyer;

	public int killsMantisHeavyFlyer;

	public bool newDataMantisHeavyFlyer;

	public bool killedGardenZombie;

	public int killsGardenZombie;

	public bool newDataGardenZombie;

	public bool killedRoyalGuard;

	public int killsRoyalGuard;

	public bool newDataRoyalGuard;

	public bool killedWhiteRoyal;

	public int killsWhiteRoyal;

	public bool newDataWhiteRoyal;

	public bool openedPalaceGrounds;

	public bool killedOblobble;

	public int killsOblobble;

	public bool newDataOblobble;

	public bool killedZote;

	public int killsZote;

	public bool newDataZote;

	public bool killedBlobble;

	public int killsBlobble;

	public bool newDataBlobble;

	public bool killedColMosquito;

	public int killsColMosquito;

	public bool newDataColMosquito;

	public bool killedColRoller;

	public int killsColRoller;

	public bool newDataColRoller;

	public bool killedColFlyingSentry;

	public int killsColFlyingSentry;

	public bool newDataColFlyingSentry;

	public bool killedColMiner;

	public int killsColMiner;

	public bool newDataColMiner;

	public bool killedColShield;

	public int killsColShield;

	public bool newDataColShield;

	public bool killedColWorm;

	public int killsColWorm;

	public bool newDataColWorm;

	public bool killedColHopper;

	public int killsColHopper;

	public bool newDataColHopper;

	public bool killedLobsterLancer;

	public int killsLobsterLancer;

	public bool newDataLobsterLancer;

	public bool killedGhostAladar;

	public int killsGhostAladar;

	public bool newDataGhostAladar;

	public bool killedGhostXero;

	public int killsGhostXero;

	public bool newDataGhostXero;

	public bool killedGhostHu;

	public int killsGhostHu;

	public bool newDataGhostHu;

	public bool killedGhostMarmu;

	public int killsGhostMarmu;

	public bool newDataGhostMarmu;

	public bool killedGhostNoEyes;

	public int killsGhostNoEyes;

	public bool newDataGhostNoEyes;

	public bool killedGhostMarkoth;

	public int killsGhostMarkoth;

	public bool newDataGhostMarkoth;

	public bool killedGhostGalien;

	public int killsGhostGalien;

	public bool newDataGhostGalien;

	public bool killedWhiteDefender;

	public int killsWhiteDefender;

	public bool newDataWhiteDefender;

	public bool killedGreyPrince;

	public int killsGreyPrince;

	public bool newDataGreyPrince;

	public bool killedZotelingBalloon;

	public int killsZotelingBalloon;

	public bool newDataZotelingBalloon;

	public bool killedZotelingHopper;

	public int killsZotelingHopper;

	public bool newDataZotelingHopper;

	public bool killedZotelingBuzzer;

	public int killsZotelingBuzzer;

	public bool newDataZotelingBuzzer;

	public bool killedHollowKnight;

	public int killsHollowKnight;

	public bool newDataHollowKnight;

	public bool killedFinalBoss;

	public int killsFinalBoss;

	public bool newDataFinalBoss;

	public bool killedHunterMark;

	public int killsHunterMark;

	public bool newDataHunterMark;

	public bool killedFlameBearerSmall;

	public int killsFlameBearerSmall;

	public bool newDataFlameBearerSmall;

	public bool killedFlameBearerMed;

	public int killsFlameBearerMed;

	public bool newDataFlameBearerMed;

	public bool killedFlameBearerLarge;

	public int killsFlameBearerLarge;

	public bool newDataFlameBearerLarge;

	public bool killedGrimm;

	public int killsGrimm;

	public bool newDataGrimm;

	public bool killedNightmareGrimm;

	public int killsNightmareGrimm;

	public bool newDataNightmareGrimm;

	public bool killedBindingSeal;

	public int killsBindingSeal;

	public bool newDataBindingSeal;

	public bool killedFatFluke;

	public int killsFatFluke;

	public bool newDataFatFluke;

	public bool killedPaleLurker;

	public int killsPaleLurker;

	public bool newDataPaleLurker;

	public bool killedNailBros;

	public int killsNailBros;

	public bool newDataNailBros;

	public bool killedPaintmaster;

	public int killsPaintmaster;

	public bool newDataPaintmaster;

	public bool killedNailsage;

	public int killsNailsage;

	public bool newDataNailsage;

	public bool killedHollowKnightPrime;

	public int killsHollowKnightPrime;

	public bool newDataHollowKnightPrime;

	public bool killedGodseekerMask;

	public int killsGodseekerMask;

	public bool newDataGodseekerMask;

	public bool killedVoidIdol_1;

	public int killsVoidIdol_1;

	public bool newDataVoidIdol_1;

	public bool killedVoidIdol_2;

	public int killsVoidIdol_2;

	public bool newDataVoidIdol_2;

	public bool killedVoidIdol_3;

	public int killsVoidIdol_3;

	public bool newDataVoidIdol_3;

	public int grubsCollected;

	public int grubRewards;

	public bool finalGrubRewardCollected;

	public bool fatGrubKing;

	public bool falseKnightDefeated;

	public bool falseKnightDreamDefeated;

	public bool falseKnightOrbsCollected;

	public bool mawlekDefeated;

	public bool giantBuzzerDefeated;

	public bool giantFlyDefeated;

	public bool blocker1Defeated;

	public bool blocker2Defeated;

	public bool hornet1Defeated;

	public bool collectorDefeated;

	public bool hornetOutskirtsDefeated;

	public bool mageLordDreamDefeated;

	public bool mageLordOrbsCollected;

	public bool infectedKnightDreamDefeated;

	public bool infectedKnightOrbsCollected;

	public bool whiteDefenderDefeated;

	public bool whiteDefenderOrbsCollected;

	public int whiteDefenderDefeats;

	public int greyPrinceDefeats;

	public bool greyPrinceDefeated;

	public bool greyPrinceOrbsCollected;

	public int aladarSlugDefeated;

	public int xeroDefeated;

	public int elderHuDefeated;

	public int mumCaterpillarDefeated;

	public int noEyesDefeated;

	public int markothDefeated;

	public int galienDefeated;

	public bool XERO_encountered;

	public bool ALADAR_encountered;

	public bool HU_encountered;

	public bool MUMCAT_encountered;

	public bool NOEYES_encountered;

	public bool MARKOTH_encountered;

	public bool GALIEN_encountered;

	public bool xeroPinned;

	public bool aladarPinned;

	public bool huPinned;

	public bool mumCaterpillarPinned;

	public bool noEyesPinned;

	public bool markothPinned;

	public bool galienPinned;

	public int currentInvPane;

	public bool showGeoUI;

	public bool showHealthUI;

	public bool promptFocus;

	public bool seenFocusTablet;

	public bool seenDreamNailPrompt;

	public bool isFirstGame;

	public bool enteredTutorialFirstTime;

	public bool isInvincible;

	public bool infiniteAirJump;

	public bool invinciTest;

	public int currentArea;

	public bool visitedDirtmouth;

	public bool visitedCrossroads;

	public bool visitedGreenpath;

	public bool visitedFungus;

	public bool visitedHive;

	public bool visitedCrossroadsInfected;

	public bool visitedRuins;

	public bool visitedMines;

	public bool visitedRoyalGardens;

	public bool visitedFogCanyon;

	public bool visitedDeepnest;

	public bool visitedRestingGrounds;

	public bool visitedWaterways;

	public bool visitedAbyss;

	public bool visitedOutskirts;

	public bool visitedWhitePalace;

	public bool visitedCliffs;

	public bool visitedAbyssLower;

	public bool visitedGodhome;

	public bool visitedMines10;

	public List<string> scenesVisited;

	public List<string> scenesMapped;

	public List<string> scenesEncounteredBench;

	public List<string> scenesGrubRescued;

	public List<string> scenesFlameCollected;

	public List<string> scenesEncounteredCocoon;

	public List<string> scenesEncounteredDreamPlant;

	public List<string> scenesEncounteredDreamPlantC;

	public bool hasMap;

	public bool mapAllRooms;

	public bool atMapPrompt;

	public bool mapDirtmouth;

	public bool mapCrossroads;

	public bool mapGreenpath;

	public bool mapFogCanyon;

	public bool mapRoyalGardens;

	public bool mapFungalWastes;

	public bool mapCity;

	public bool mapWaterways;

	public bool mapMines;

	public bool mapDeepnest;

	public bool mapCliffs;

	public bool mapOutskirts;

	public bool mapRestingGrounds;

	public bool mapAbyss;

	private Dictionary<string, MapBools> mapZoneBools;

	public bool hasPin;

	public bool hasPinBench;

	public bool hasPinCocoon;

	public bool hasPinDreamPlant;

	public bool hasPinGuardian;

	public bool hasPinBlackEgg;

	public bool hasPinShop;

	public bool hasPinSpa;

	public bool hasPinStag;

	public bool hasPinTram;

	public bool hasPinGhost;

	public bool hasPinGrub;

	public bool hasMarker;

	public bool hasMarker_r;

	public bool hasMarker_b;

	public bool hasMarker_y;

	public bool hasMarker_w;

	public int spareMarkers_r;

	public int spareMarkers_b;

	public int spareMarkers_y;

	public int spareMarkers_w;

	public List<Vector3> placedMarkers_r;

	public List<Vector3> placedMarkers_b;

	public List<Vector3> placedMarkers_y;

	public List<Vector3> placedMarkers_w;

	public int environmentType;

	public int environmentTypeDefault;

	public int previousDarkness;

	public bool openedTramLower;

	public bool openedTramRestingGrounds;

	public int tramLowerPosition;

	public int tramRestingGroundsPosition;

	public bool mineLiftOpened;

	public bool menderDoorOpened;

	public bool vesselFragStagNest;

	public bool shamanPillar;

	public bool crossroadsMawlekWall;

	public bool eggTempleVisited;

	public bool crossroadsInfected;

	public bool falseKnightFirstPlop;

	public bool falseKnightWallRepaired;

	public bool falseKnightWallBroken;

	public bool falseKnightGhostDeparted;

	public bool spaBugsEncountered;

	public bool hornheadVinePlat;

	public bool infectedKnightEncountered;

	public bool megaMossChargerEncountered;

	public bool megaMossChargerDefeated;

	public bool dreamerScene1;

	public bool slugEncounterComplete;

	public bool defeatedDoubleBlockers;

	public bool oneWayArchive;

	public bool defeatedMegaJelly;

	public bool summonedMonomon;

	public bool sawWoundedQuirrel;

	public bool encounteredMegaJelly;

	public bool defeatedMantisLords;

	public bool encounteredGatekeeper;

	public bool deepnestWall;

	public bool queensStationNonDisplay;

	public bool cityBridge1;

	public bool cityBridge2;

	public bool cityLift1;

	public bool cityLift1_isUp;

	public bool liftArrival;

	public bool openedMageDoor;

	public bool openedMageDoor_v2;

	public bool brokenMageWindow;

	public bool brokenMageWindowGlass;

	public bool mageLordEncountered;

	public bool mageLordEncountered_2;

	public bool mageLordDefeated;

	public bool ruins1_5_tripleDoor;

	public bool openedCityGate;

	public bool cityGateClosed;

	public bool bathHouseOpened;

	public bool bathHouseWall;

	public bool cityLift2;

	public bool cityLift2_isUp;

	public bool city2_sewerDoor;

	public bool openedLoveDoor;

	public bool watcherChandelier;

	public bool completedQuakeArea;

	public bool kingsStationNonDisplay;

	public bool tollBenchCity;

	public bool waterwaysGate;

	public bool defeatedDungDefender;

	public bool dungDefenderEncounterReady;

	public bool flukeMotherEncountered;

	public bool flukeMotherDefeated;

	public bool openedWaterwaysManhole;

	public bool waterwaysAcidDrained;

	public bool dungDefenderWallBroken;

	public bool dungDefenderSleeping;

	public bool defeatedMegaBeamMiner;

	public bool defeatedMegaBeamMiner2;

	public bool brokeMinersWall;

	public bool encounteredMimicSpider;

	public bool steppedBeyondBridge;

	public bool deepnestBridgeCollapsed;

	public bool spiderCapture;

	public bool deepnest26b_switch;

	public bool openedRestingGrounds02;

	public bool restingGroundsCryptWall;

	public bool dreamNailConvo;

	public int gladeGhostsKilled;

	public bool openedGardensStagStation;

	public bool extendedGramophone;

	public bool tollBenchQueensGardens;

	public bool blizzardEnded;

	public bool encounteredHornet;

	public bool savedByHornet;

	public bool outskirtsWall;

	public bool abyssGateOpened;

	public bool abyssLighthouse;

	public bool blueVineDoor;

	public bool gotShadeCharm;

	public bool tollBenchAbyss;

	public int fountainGeo;

	public bool fountainVesselSummoned;

	public bool openedBlackEggPath;

	public bool enteredDreamWorld;

	public bool duskKnightDefeated;

	public bool whitePalaceOrb_1;

	public bool whitePalaceOrb_2;

	public bool whitePalaceOrb_3;

	public bool whitePalace05_lever;

	public bool whitePalaceMidWarp;

	public bool whitePalaceSecretRoomVisited;

	public bool tramOpenedDeepnest;

	public bool tramOpenedCrossroads;

	public bool openedBlackEggDoor;

	public bool unchainedHollowKnight;

	public int flamesCollected;

	public int flamesRequired;

	public bool nightmareLanternAppeared;

	public bool nightmareLanternLit;

	public bool troupeInTown;

	public bool divineInTown;

	public int grimmChildLevel;

	public bool elderbugConvoGrimm;

	public bool slyConvoGrimm;

	public bool iseldaConvoGrimm;

	public bool midwifeWeaverlingConvo;

	public bool metGrimm;

	public bool foughtGrimm;

	public bool metBrum;

	public bool defeatedNightmareGrimm;

	public bool grimmchildAwoken;

	public bool gotBrummsFlame;

	public bool brummBrokeBrazier;

	public bool destroyedNightmareLantern;

	public bool gotGrimmNotch;

	public bool nymmInTown;

	public bool nymmSpoken;

	public bool nymmCharmConvo;

	public bool nymmFinalConvo;

	public bool elderbugNymmConvo;

	public bool slyNymmConvo;

	public bool iseldaNymmConvo;

	public bool nymmMissedEggOpen;

	public bool elderbugTroupeLeftConvo;

	public bool elderbugBrettaLeft;

	public bool jijiGrimmConvo;

	public bool metDivine;

	public bool divineFinalConvo;

	public bool gaveFragileHeart;

	public bool gaveFragileGreed;

	public bool gaveFragileStrength;

	public int divineEatenConvos;

	public bool pooedFragileHeart;

	public bool pooedFragileGreed;

	public bool pooedFragileStrength;

	public float completionPercentage;

	public bool disablePause;

	public bool backerCredits;

	public bool unlockedCompletionRate;

	public int mapKeyPref;

	public List<string> playerStory;

	public string playerStoryOutput;

	public bool betaEnd;

	public bool newDatTraitorLord;

	public string bossReturnEntryGate;

	public BossSequenceDoor.Completion bossDoorStateTier1;

	public BossSequenceDoor.Completion bossDoorStateTier2;

	public BossSequenceDoor.Completion bossDoorStateTier3;

	public BossSequenceDoor.Completion bossDoorStateTier4;

	public BossSequenceDoor.Completion bossDoorStateTier5;

	public int bossStatueTargetLevel;

	public string currentBossStatueCompletionKey;

	public BossStatue.Completion statueStateGruzMother;

	public BossStatue.Completion statueStateVengefly;

	public BossStatue.Completion statueStateBroodingMawlek;

	public BossStatue.Completion statueStateFalseKnight;

	public BossStatue.Completion statueStateFailedChampion;

	public BossStatue.Completion statueStateHornet1;

	public BossStatue.Completion statueStateHornet2;

	public BossStatue.Completion statueStateMegaMossCharger;

	public BossStatue.Completion statueStateMantisLords;

	public BossStatue.Completion statueStateOblobbles;

	public BossStatue.Completion statueStateGreyPrince;

	public BossStatue.Completion statueStateBrokenVessel;

	public BossStatue.Completion statueStateLostKin;

	public BossStatue.Completion statueStateNosk;

	public BossStatue.Completion statueStateFlukemarm;

	public BossStatue.Completion statueStateCollector;

	public BossStatue.Completion statueStateWatcherKnights;

	public BossStatue.Completion statueStateSoulMaster;

	public BossStatue.Completion statueStateSoulTyrant;

	public BossStatue.Completion statueStateGodTamer;

	public BossStatue.Completion statueStateCrystalGuardian1;

	public BossStatue.Completion statueStateCrystalGuardian2;

	public BossStatue.Completion statueStateUumuu;

	public BossStatue.Completion statueStateDungDefender;

	public BossStatue.Completion statueStateWhiteDefender;

	public BossStatue.Completion statueStateHiveKnight;

	public BossStatue.Completion statueStateTraitorLord;

	public BossStatue.Completion statueStateGrimm;

	public BossStatue.Completion statueStateNightmareGrimm;

	public BossStatue.Completion statueStateHollowKnight;

	public BossStatue.Completion statueStateElderHu;

	public BossStatue.Completion statueStateGalien;

	public BossStatue.Completion statueStateMarkoth;

	public BossStatue.Completion statueStateMarmu;

	public BossStatue.Completion statueStateNoEyes;

	public BossStatue.Completion statueStateXero;

	public BossStatue.Completion statueStateGorb;

	public BossStatue.Completion statueStateRadiance;

	public BossStatue.Completion statueStateSly;

	public BossStatue.Completion statueStateNailmasters;

	public BossStatue.Completion statueStateMageKnight;

	public BossStatue.Completion statueStatePaintmaster;

	public BossStatue.Completion statueStateZote;

	public BossStatue.Completion statueStateNoskHornet;

	public BossStatue.Completion statueStateMantisLordsExtra;

	public bool godseekerUnlocked;

	public BossSequenceController.BossSequenceData currentBossSequence;

	public bool bossRushMode;

	public bool bossDoorCageUnlocked;

	public bool blueRoomDoorUnlocked;

	public bool blueRoomActivated;

	public bool finalBossDoorUnlocked;

	public bool hasGodfinder;

	public bool unlockedNewBossStatue;

	public bool scaredFlukeHermitEncountered;

	public bool scaredFlukeHermitReturned;

	public bool enteredGGAtrium;

	public bool extraFlowerAppear;

	public bool givenGodseekerFlower;

	public bool givenOroFlower;

	public bool givenWhiteLadyFlower;

	public bool givenEmilitiaFlower;

	public List<string> unlockedBossScenes;

	public bool queuedGodfinderIcon;

	public bool godseekerSpokenAwake;

	public bool nailsmithCorpseAppeared;

	public int godseekerWaterwaysSeenState;

	public bool godseekerWaterwaysSpoken1;

	public bool godseekerWaterwaysSpoken2;

	public bool godseekerWaterwaysSpoken3;

	public int bossDoorEntranceTextSeen;

	public bool seenDoor4Finale;

	public bool zoteStatueWallBroken;

	public bool seenGGWastes;

	public bool ordealAchieved;

	private static PlayerData _instance;

	public int CurrentMaxHealth
	{
		get
		{
			if (BossSequenceController.BoundShell)
			{
				return Mathf.Min(maxHealth, BossSequenceController.BoundMaxHealth);
			}
			return maxHealth;
		}
	}

	public static PlayerData instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new PlayerData();
			}
			return _instance;
		}
		set
		{
			_instance = value;
		}
	}

	private Dictionary<string, MapBools> InitMapBools()
	{
		return new Dictionary<string, MapBools>
		{
			{
				"Town",
				MapBools.MapDirtmouth
			},
			{
				"Tutorial_01",
				MapBools.MapDirtmouth
			},
			{
				"Abyss_03",
				MapBools.MapAbyss
			},
			{
				"Abyss_04",
				MapBools.MapAbyss
			},
			{
				"Abyss_05",
				MapBools.MapAbyss
			},
			{
				"Abyss_06_Core",
				MapBools.MapAbyss
			},
			{
				"Abyss_06_Core_b",
				MapBools.MapAbyss
			},
			{
				"Abyss_08",
				MapBools.MapAbyss
			},
			{
				"Abyss_09",
				MapBools.MapAbyss
			},
			{
				"Abyss_10",
				MapBools.MapAbyss
			},
			{
				"Abyss_12",
				MapBools.MapAbyss
			},
			{
				"Abyss_16",
				MapBools.MapAbyss
			},
			{
				"Abyss_17",
				MapBools.MapAbyss
			},
			{
				"Abyss_18",
				MapBools.MapAbyss
			},
			{
				"Abyss_18_b",
				MapBools.MapAbyss
			},
			{
				"Abyss_19",
				MapBools.MapAbyss
			},
			{
				"Abyss_20",
				MapBools.MapAbyss
			},
			{
				"Abyss_21",
				MapBools.MapAbyss
			},
			{
				"Abyss_22",
				MapBools.MapAbyss
			},
			{
				"Crossroads_49b",
				MapBools.MapCity
			},
			{
				"Ruins1_01",
				MapBools.MapCity
			},
			{
				"Ruins1_02",
				MapBools.MapCity
			},
			{
				"Ruins1_03",
				MapBools.MapCity
			},
			{
				"Ruins1_04",
				MapBools.MapCity
			},
			{
				"Ruins1_05",
				MapBools.MapCity
			},
			{
				"Ruins1_05b",
				MapBools.MapCity
			},
			{
				"Ruins1_05c",
				MapBools.MapCity
			},
			{
				"Ruins1_06",
				MapBools.MapCity
			},
			{
				"Ruins1_09",
				MapBools.MapCity
			},
			{
				"Ruins1_17",
				MapBools.MapCity
			},
			{
				"Ruins1_18",
				MapBools.MapCity
			},
			{
				"Ruins1_18_b",
				MapBools.MapCity
			},
			{
				"Ruins1_23",
				MapBools.MapCity
			},
			{
				"Ruins1_24",
				MapBools.MapCity
			},
			{
				"Ruins1_25",
				MapBools.MapCity
			},
			{
				"Ruins1_27",
				MapBools.MapCity
			},
			{
				"Ruins1_28",
				MapBools.MapCity
			},
			{
				"Ruins1_29",
				MapBools.MapCity
			},
			{
				"Ruins1_30",
				MapBools.MapCity
			},
			{
				"Ruins1_31",
				MapBools.MapCity
			},
			{
				"Ruins1_31b",
				MapBools.MapCity
			},
			{
				"Ruins1_31_top",
				MapBools.MapCity
			},
			{
				"Ruins1_31_top_2",
				MapBools.MapCity
			},
			{
				"Ruins1_32",
				MapBools.MapCity
			},
			{
				"Ruins2_01",
				MapBools.MapCity
			},
			{
				"Ruins2_01_b",
				MapBools.MapCity
			},
			{
				"Ruins2_03",
				MapBools.MapCity
			},
			{
				"Ruins2_03b",
				MapBools.MapCity
			},
			{
				"Ruins2_04",
				MapBools.MapCity
			},
			{
				"Ruins2_05",
				MapBools.MapCity
			},
			{
				"Ruins2_06",
				MapBools.MapCity
			},
			{
				"Ruins2_07",
				MapBools.MapCity
			},
			{
				"Ruins2_07_left",
				MapBools.MapCity
			},
			{
				"Ruins2_07_right",
				MapBools.MapCity
			},
			{
				"Ruins2_08",
				MapBools.MapCity
			},
			{
				"Ruins2_09",
				MapBools.MapCity
			},
			{
				"Ruins2_10_b",
				MapBools.MapCity
			},
			{
				"Ruins2_11",
				MapBools.MapCity
			},
			{
				"Ruins2_11_b",
				MapBools.MapCity
			},
			{
				"Ruins2_Watcher_Room",
				MapBools.MapCity
			},
			{
				"Ruins_Bathhouse",
				MapBools.MapCity
			},
			{
				"Ruins_Elevator",
				MapBools.MapCity
			},
			{
				"Cliffs_01",
				MapBools.MapCliffs
			},
			{
				"Cliffs_01_b",
				MapBools.MapCliffs
			},
			{
				"Cliffs_02",
				MapBools.MapCliffs
			},
			{
				"Cliffs_02_b",
				MapBools.MapCliffs
			},
			{
				"Cliffs_04",
				MapBools.MapCliffs
			},
			{
				"Cliffs_05",
				MapBools.MapCliffs
			},
			{
				"Cliffs_06",
				MapBools.MapCliffs
			},
			{
				"Cliffs_06_b",
				MapBools.MapCliffs
			},
			{
				"Fungus1_28",
				MapBools.MapCliffs
			},
			{
				"Fungus1_28_b",
				MapBools.MapCliffs
			},
			{
				"Crossroads_01",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_02",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_03",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_04",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_05",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_06",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_07",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_08",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_09",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_10",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_11_alt",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_12",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_13",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_14",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_15",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_16",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_18",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_19",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_21",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_22",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_25",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_27",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_30",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_31",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_33",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_35",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_36",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_37",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_38",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_39",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_40",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_42",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_43",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_45",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_46",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_47",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_48",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_49",
				MapBools.MapCrossroads
			},
			{
				"Crossroads_52",
				MapBools.MapCrossroads
			},
			{
				"Mines_01",
				MapBools.MapMines
			},
			{
				"Mines_02",
				MapBools.MapMines
			},
			{
				"Mines_03",
				MapBools.MapMines
			},
			{
				"Mines_04",
				MapBools.MapMines
			},
			{
				"Mines_05",
				MapBools.MapMines
			},
			{
				"Mines_06",
				MapBools.MapMines
			},
			{
				"Mines_07",
				MapBools.MapMines
			},
			{
				"Mines_10",
				MapBools.MapMines
			},
			{
				"Mines_11",
				MapBools.MapMines
			},
			{
				"Mines_13",
				MapBools.MapMines
			},
			{
				"Mines_16",
				MapBools.MapMines
			},
			{
				"Mines_17",
				MapBools.MapMines
			},
			{
				"Mines_18",
				MapBools.MapMines
			},
			{
				"Mines_19",
				MapBools.MapMines
			},
			{
				"Mines_20",
				MapBools.MapMines
			},
			{
				"Mines_20_b",
				MapBools.MapMines
			},
			{
				"Mines_23",
				MapBools.MapMines
			},
			{
				"Mines_24",
				MapBools.MapMines
			},
			{
				"Mines_25",
				MapBools.MapMines
			},
			{
				"Mines_28",
				MapBools.MapMines
			},
			{
				"Mines_28_b",
				MapBools.MapMines
			},
			{
				"Mines_29",
				MapBools.MapMines
			},
			{
				"Mines_30",
				MapBools.MapMines
			},
			{
				"Mines_31",
				MapBools.MapMines
			},
			{
				"Mines_32",
				MapBools.MapMines
			},
			{
				"Mines_34",
				MapBools.MapMines
			},
			{
				"Mines_36",
				MapBools.MapMines
			},
			{
				"Mines_37",
				MapBools.MapMines
			},
			{
				"Abyss_03_b",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_01b",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_02",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_03",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_09",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_10",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_14",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_16",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_17",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_26",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_26b",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_30",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_30_b",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_31",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_32",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_33",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_34",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_35",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_36",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_37",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_38",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_39",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_40",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_41",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_41_b",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_42",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_44",
				MapBools.MapDeepnest
			},
			{
				"Deepnest_44_b",
				MapBools.MapDeepnest
			},
			{
				"Fungus2_25",
				MapBools.MapDeepnest
			},
			{
				"Room_Mask_maker",
				MapBools.MapDeepnest
			},
			{
				"Fungus3_01",
				MapBools.MapFogCanyon
			},
			{
				"Fungus3_02",
				MapBools.MapFogCanyon
			},
			{
				"Fungus3_03",
				MapBools.MapFogCanyon
			},
			{
				"Fungus3_24",
				MapBools.MapFogCanyon
			},
			{
				"Fungus3_25",
				MapBools.MapFogCanyon
			},
			{
				"Fungus3_25b",
				MapBools.MapFogCanyon
			},
			{
				"Fungus3_26",
				MapBools.MapFogCanyon
			},
			{
				"Fungus3_27",
				MapBools.MapFogCanyon
			},
			{
				"Fungus3_28",
				MapBools.MapFogCanyon
			},
			{
				"Fungus3_30",
				MapBools.MapFogCanyon
			},
			{
				"Fungus3_35",
				MapBools.MapFogCanyon
			},
			{
				"Fungus3_44",
				MapBools.MapFogCanyon
			},
			{
				"Fungus3_47",
				MapBools.MapFogCanyon
			},
			{
				"Deepnest_01",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_01",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_02",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_03",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_04",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_05",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_06",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_07",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_08",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_09",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_10",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_11",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_12",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_13",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_14",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_14_b",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_14_c",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_15",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_17",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_18",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_19",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_20",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_21",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_23",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_26",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_28",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_29",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_30",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_31",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_32",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_29_b",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_33",
				MapBools.MapFungalWastes
			},
			{
				"Fungus2_34",
				MapBools.MapFungalWastes
			},
			{
				"Fungus1_01",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_01b",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_02",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_03",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_04",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_05",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_06",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_07",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_08",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_09",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_09_b",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_10",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_11",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_12",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_13",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_14",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_14_b",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_15",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_16_alt",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_17",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_19",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_20_v02",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_21",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_22",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_25",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_26",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_29",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_30",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_31",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_32",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_34",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_37",
				MapBools.MapGreenpath
			},
			{
				"Fungus1_Slug",
				MapBools.MapGreenpath
			},
			{
				"Abyss_03_c",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_01",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_02",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_02b",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_03",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_04",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_06",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_07",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_08",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_09",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_09_b",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_10",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_11",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_12",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_13",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_14",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_15",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_16",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_18",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_Hornet",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_East_Hornet_b",
				MapBools.MapOutskirts
			},
			{
				"Hive_01",
				MapBools.MapOutskirts
			},
			{
				"Hive_02",
				MapBools.MapOutskirts
			},
			{
				"Hive_03",
				MapBools.MapOutskirts
			},
			{
				"Hive_03_b",
				MapBools.MapOutskirts
			},
			{
				"Hive_03_c",
				MapBools.MapOutskirts
			},
			{
				"Hive_04",
				MapBools.MapOutskirts
			},
			{
				"Hive_04_b",
				MapBools.MapOutskirts
			},
			{
				"Hive_05",
				MapBools.MapOutskirts
			},
			{
				"Deepnest_43",
				MapBools.MapRoyalGardens
			},
			{
				"Deepnest_43_b",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus1_23",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus1_24",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_04",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_05",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_08",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_10",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_11",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_13",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_21",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_22",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_22_b",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_23",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_23_b",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_34",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_39",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_40",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_48",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_48_bot",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_48_left",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_48_top",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_49",
				MapBools.MapRoyalGardens
			},
			{
				"Fungus3_50",
				MapBools.MapRoyalGardens
			},
			{
				"Crossroads_46b",
				MapBools.MapRestingGrounds
			},
			{
				"Crossroads_50",
				MapBools.MapRestingGrounds
			},
			{
				"RestingGrounds_02",
				MapBools.MapRestingGrounds
			},
			{
				"RestingGrounds_04",
				MapBools.MapRestingGrounds
			},
			{
				"RestingGrounds_05",
				MapBools.MapRestingGrounds
			},
			{
				"RestingGrounds_06",
				MapBools.MapRestingGrounds
			},
			{
				"RestingGrounds_08",
				MapBools.MapRestingGrounds
			},
			{
				"RestingGrounds_09",
				MapBools.MapRestingGrounds
			},
			{
				"RestingGrounds_10_b",
				MapBools.MapRestingGrounds
			},
			{
				"RestingGrounds_10_c",
				MapBools.MapRestingGrounds
			},
			{
				"RestingGrounds_10_d",
				MapBools.MapRestingGrounds
			},
			{
				"RestingGrounds_12",
				MapBools.MapRestingGrounds
			},
			{
				"RestingGrounds_17",
				MapBools.MapRestingGrounds
			},
			{
				"Ruins2_10",
				MapBools.MapRestingGrounds
			},
			{
				"RestingGrounds_10",
				MapBools.MapRestingGrounds
			},
			{
				"Abyss_01",
				MapBools.MapWaterways
			},
			{
				"Abyss_02",
				MapBools.MapWaterways
			},
			{
				"Waterways_01",
				MapBools.MapWaterways
			},
			{
				"Waterways_02",
				MapBools.MapWaterways
			},
			{
				"Waterways_02b",
				MapBools.MapWaterways
			},
			{
				"Waterways_03",
				MapBools.MapWaterways
			},
			{
				"Waterways_04",
				MapBools.MapWaterways
			},
			{
				"Waterways_04_part_b",
				MapBools.MapWaterways
			},
			{
				"Waterways_04b",
				MapBools.MapWaterways
			},
			{
				"Waterways_05",
				MapBools.MapWaterways
			},
			{
				"Waterways_06",
				MapBools.MapWaterways
			},
			{
				"Waterways_07",
				MapBools.MapWaterways
			},
			{
				"Waterways_08",
				MapBools.MapWaterways
			},
			{
				"Waterways_09",
				MapBools.MapWaterways
			},
			{
				"Waterways_12",
				MapBools.MapWaterways
			},
			{
				"Waterways_13",
				MapBools.MapWaterways
			},
			{
				"Waterways_14",
				MapBools.MapWaterways
			},
			{
				"Waterways_15",
				MapBools.MapWaterways
			}
		};
	}

	private bool HasMapForScene(string sceneName)
	{
		if (mapZoneBools == null)
		{
			mapZoneBools = InitMapBools();
		}
		if (mapZoneBools.ContainsKey(sceneName))
		{
			return mapZoneBools[sceneName] switch
			{
				MapBools.MapDirtmouth => mapDirtmouth,
				MapBools.MapCrossroads => mapCrossroads,
				MapBools.MapGreenpath => mapGreenpath,
				MapBools.MapFogCanyon => mapFogCanyon,
				MapBools.MapRoyalGardens => mapRoyalGardens,
				MapBools.MapFungalWastes => mapFungalWastes,
				MapBools.MapCity => mapCity,
				MapBools.MapWaterways => mapWaterways,
				MapBools.MapMines => mapMines,
				MapBools.MapDeepnest => mapDeepnest,
				MapBools.MapCliffs => mapCliffs,
				MapBools.MapOutskirts => mapOutskirts,
				MapBools.MapRestingGrounds => mapRestingGrounds,
				MapBools.MapAbyss => mapAbyss,
				_ => false,
			};
		}
		return true;
	}

	protected PlayerData()
	{
		SetupNewPlayerData();
	}

	public void PrintStory()
	{
		Debug.Log("combining player story");
		playerStoryOutput = string.Join(",", playerStory.ToArray());
	}

	public void Reset()
	{
		SetupNewPlayerData();
	}

	public bool UpdateGameMap()
	{
		bool result = false;
		if (hasQuill)
		{
			foreach (string item in scenesVisited)
			{
				if (!scenesMapped.Contains(item) && HasMapForScene(item))
				{
					scenesMapped.Add(item);
					result = true;
				}
			}
		}
		return result;
	}

	public void CheckAllMaps()
	{
		if (mapCrossroads && mapGreenpath && mapFogCanyon && mapRoyalGardens && mapFungalWastes && mapCity && mapWaterways && mapMines && mapDeepnest && mapCliffs && mapOutskirts && mapRestingGrounds && mapAbyss)
		{
			corniferAtHome = true;
		}
	}

	public void SetBool(string boolName, bool value)
	{
		FieldInfo field = GetType().GetField(boolName);
		if (field != null)
		{
			field.SetValue(instance, value);
		}
		else
		{
			Debug.Log("PlayerData: Could not find field named " + boolName + ", check variable name exists and FSM variable string is correct.");
		}
	}

	public void SetInt(string intName, int value)
	{
		FieldInfo field = GetType().GetField(intName);
		if (field != null)
		{
			field.SetValue(instance, value);
		}
		else
		{
			Debug.Log("PlayerData: Could not find field named " + intName + ", check variable name exists and FSM variable string is correct.");
		}
	}

	public void IncrementInt(string intName)
	{
		FieldInfo field = GetType().GetField(intName);
		if (field != null)
		{
			int num = (int)field.GetValue(instance);
			field.SetValue(instance, num + 1);
		}
		else
		{
			Debug.Log("PlayerData: Could not find field named " + intName + ", check variable name exists and FSM variable string is correct.");
		}
	}

	public void IntAdd(string intName, int amount)
	{
		FieldInfo field = GetType().GetField(intName);
		if (field != null)
		{
			int num = (int)field.GetValue(instance);
			field.SetValue(instance, num + amount);
		}
		else
		{
			Debug.Log("PlayerData: Could not find field named " + intName + ", check variable name exists and FSM variable string is correct.");
		}
	}

	public void SetFloat(string floatName, float value)
	{
		FieldInfo field = GetType().GetField(floatName);
		if (field != null)
		{
			field.SetValue(instance, value);
		}
		else
		{
			Debug.Log("PlayerData: Could not find field named " + floatName + ", check variable name exists and FSM variable string is correct.");
		}
	}

	public void DecrementInt(string intName)
	{
		FieldInfo field = GetType().GetField(intName);
		if (field != null)
		{
			int num = (int)field.GetValue(instance);
			field.SetValue(instance, num - 1);
		}
	}

	public bool GetBool(string boolName)
	{
		if (string.IsNullOrEmpty(boolName))
		{
			return false;
		}
		FieldInfo field = GetType().GetField(boolName);
		if (field != null)
		{
			return (bool)field.GetValue(instance);
		}
		Debug.Log("PlayerData: Could not find bool named " + boolName + " in PlayerData");
		return false;
	}

	public int GetInt(string intName)
	{
		if (string.IsNullOrEmpty(intName))
		{
			Debug.LogError("PlayerData: Int with an EMPTY name requested.");
			return -9999;
		}
		FieldInfo field = GetType().GetField(intName);
		if (field != null)
		{
			return (int)field.GetValue(instance);
		}
		Debug.LogError("PlayerData: Could not find int named " + intName + " in PlayerData");
		return -9999;
	}

	public float GetFloat(string floatName)
	{
		if (string.IsNullOrEmpty(floatName))
		{
			Debug.LogError("PlayerData: Float with an EMPTY name requested.");
			return -9999f;
		}
		FieldInfo field = GetType().GetField(floatName);
		if (field != null)
		{
			return (float)field.GetValue(instance);
		}
		Debug.LogError("PlayerData: Could not find int named " + floatName + " in PlayerData");
		return -9999f;
	}

	public string GetString(string stringName)
	{
		if (string.IsNullOrEmpty(stringName))
		{
			Debug.LogError("PlayerData: String with an EMPTY name requested.");
			return " ";
		}
		FieldInfo field = GetType().GetField(stringName);
		if (field != null)
		{
			return (string)field.GetValue(instance);
		}
		Debug.LogError("PlayerData: Could not find string named " + stringName + " in PlayerData");
		return " ";
	}

	public void SetString(string stringName, string value)
	{
		FieldInfo field = GetType().GetField(stringName);
		if (field != null)
		{
			field.SetValue(instance, value);
		}
		else
		{
			Debug.Log("PlayerData: Could not find field named " + stringName + ", check variable name exists and FSM variable string is correct.");
		}
	}

	public void SetVector3(string vectorName, Vector3 value)
	{
		FieldInfo field = GetType().GetField(vectorName);
		if (field != null)
		{
			field.SetValue(instance, value);
		}
		else
		{
			Debug.Log("PlayerData: Could not find field named " + vectorName + ", check variable name exists and FSM variable string is correct.");
		}
	}

	public Vector3 GetVector3(string vectorName)
	{
		FieldInfo field = GetType().GetField(vectorName);
		if (field != null)
		{
			return (Vector3)field.GetValue(instance);
		}
		Debug.LogError("PlayerData: Could not find string named " + vectorName + " in PlayerData");
		return Vector3.zero;
	}

	public void SetVariable<T>(string fieldName, T value)
	{
		FieldInfo field = GetType().GetField(fieldName);
		if (field != null)
		{
			if (field.FieldType == typeof(T))
			{
				field.SetValue(this, value);
			}
			else
			{
				Debug.LogError($"PlayerData field: {fieldName}, type {field.FieldType} does not match type: {typeof(T).ToString()}");
			}
		}
		else
		{
			Debug.LogError($"PlayerData field: {fieldName} does not exist!");
		}
	}

	public T GetVariable<T>(string fieldName)
	{
		FieldInfo field = GetType().GetField(fieldName);
		if (field != null)
		{
			if (field.FieldType == typeof(T))
			{
				return (T)field.GetValue(this);
			}
			Debug.LogError($"PlayerData field: {fieldName}, type {field.FieldType} does not match type: {typeof(T).ToString()}");
		}
		else
		{
			Debug.LogError($"PlayerData field: {fieldName} does not exist!");
		}
		return default(T);
	}

	public void AddHealth(int amount)
	{
		if (health + amount >= maxHealth)
		{
			health = maxHealth;
		}
		else
		{
			health += amount;
		}
		if (health >= CurrentMaxHealth)
		{
			health = maxHealth;
		}
	}

	public void TakeHealth(int amount)
	{
		if (amount > 0 && health == maxHealth && health != CurrentMaxHealth)
		{
			health = CurrentMaxHealth;
		}
		if (healthBlue > 0)
		{
			int num = amount - healthBlue;
			damagedBlue = true;
			healthBlue -= amount;
			if (healthBlue < 0)
			{
				healthBlue = 0;
			}
			if (num > 0)
			{
				TakeHealth(num);
			}
		}
		else
		{
			damagedBlue = false;
			if (health - amount <= 0)
			{
				health = 0;
			}
			else
			{
				health -= amount;
			}
		}
	}

	public void MaxHealth()
	{
		prevHealth = health;
		health = CurrentMaxHealth;
		blockerHits = 4;
		UpdateBlueHealth();
	}

	public void ActivateTestingCheats()
	{
		rancidEggs += 3;
		simpleKeys += 3;
		ore += 6;
		AddGeo(50000);
		openedTownBuilding = true;
		openedCrossroads = true;
		openedGreenpath = true;
		openedFungalWastes = true;
		openedRuins1 = true;
		openedRuins2 = true;
		openedRoyalGardens = true;
		openedRestingGrounds = true;
		openedDeepnest = true;
		openedStagNest = true;
		openedHiddenStation = true;
		dreamOrbs = 9999;
	}

	public void GetAllPowerups()
	{
		canDash = true;
		hasDash = true;
		hasWalljump = true;
		canWallJump = true;
		hasSuperDash = true;
		hasDreamNail = true;
		hasShadowDash = true;
		canShadowDash = true;
		dreamNailUpgraded = true;
		hasDoubleJump = true;
		hasLantern = true;
		hasAcidArmour = true;
		hasTramPass = true;
		hasSpell = true;
		if (fireballLevel == 0)
		{
			fireballLevel = 1;
		}
		if (quakeLevel == 0)
		{
			quakeLevel = 1;
		}
		if (screamLevel == 0)
		{
			screamLevel = 1;
		}
		hasLoveKey = true;
		hasWhiteKey = true;
		hasKingsBrand = true;
		hasNailArt = true;
		hasDashSlash = true;
		hasCyclone = true;
		hasUpwardSlash = true;
		hasCharm = true;
		gotCharm_1 = true;
		gotCharm_2 = true;
		gotCharm_3 = true;
		gotCharm_4 = true;
		gotCharm_5 = true;
		gotCharm_6 = true;
		gotCharm_7 = true;
		gotCharm_8 = true;
		gotCharm_9 = true;
		gotCharm_10 = true;
		gotCharm_11 = true;
		gotCharm_12 = true;
		gotCharm_13 = true;
		gotCharm_14 = true;
		gotCharm_15 = true;
		gotCharm_16 = true;
		gotCharm_17 = true;
		gotCharm_18 = true;
		gotCharm_19 = true;
		gotCharm_20 = true;
		gotCharm_21 = true;
		gotCharm_22 = true;
		gotCharm_23 = true;
		gotCharm_24 = true;
		gotCharm_25 = true;
		gotCharm_26 = true;
		gotCharm_27 = true;
		gotCharm_28 = true;
		gotCharm_29 = true;
		gotCharm_30 = true;
		gotCharm_31 = true;
		gotCharm_32 = true;
		gotCharm_33 = true;
		gotCharm_34 = true;
		gotCharm_35 = true;
		gotCharm_37 = true;
		gotCharm_38 = true;
		gotCharm_39 = true;
		charmSlots = 11;
	}

	public void AddToMaxHealth(int amount)
	{
		maxHealthBase += amount;
		if (!equippedCharm_27)
		{
			maxHealth += amount;
		}
		prevHealth = health;
		health = maxHealth;
		if (maxHealthBase == maxHealthCap)
		{
			heartPieceMax = true;
		}
	}

	public void UpdateBlueHealth()
	{
		healthBlue = 0;
		if (equippedCharm_8)
		{
			healthBlue += 2;
		}
		if (equippedCharm_9)
		{
			healthBlue += 4;
		}
	}

	public void AddGeo(int amount)
	{
		geo += amount;
		if (geo > 9999999)
		{
			geo = 9999999;
		}
	}

	public void TakeGeo(int amount)
	{
		geo -= amount;
	}

	public bool WouldDie(int damage)
	{
		if (health - damage <= 0)
		{
			return true;
		}
		return false;
	}

	public bool AddMPCharge(int amount)
	{
		bool result = false;
		if (soulLimited && maxMP != 66)
		{
			maxMP = 66;
		}
		if (!soulLimited && maxMP != 99)
		{
			maxMP = 99;
		}
		if (BossSequenceController.BoundSoul && maxMP != 33)
		{
			maxMP = 33;
		}
		if (MPCharge + amount > maxMP)
		{
			if (MPReserve < MPReserveMax && !BossSequenceController.BoundSoul)
			{
				MPReserve += amount - (maxMP - MPCharge);
				result = true;
				if (MPReserve > MPReserveMax)
				{
					MPReserve = MPReserveMax;
				}
			}
			MPCharge = maxMP;
		}
		else
		{
			MPCharge += amount;
			result = true;
		}
		return result;
	}

	public void TakeMP(int amount)
	{
		if (amount <= MPCharge)
		{
			MPCharge -= amount;
			if (MPCharge < 0)
			{
				MPCharge = 0;
			}
		}
		else
		{
			MPCharge = 0;
		}
	}

	public void TakeReserveMP(int amount)
	{
		MPReserve -= amount;
		if (MPReserve < 0)
		{
			MPReserve = 0;
		}
	}

	public void ClearMP()
	{
		MPCharge = 0;
		MPReserve = 0;
	}

	public void AddToMaxMPReserve(int amount)
	{
		MPReserveMax += amount;
		if (MPReserveMax == MPReserveCap)
		{
			vesselFragmentMax = true;
		}
	}

	public void StartSoulLimiter()
	{
		soulLimited = true;
		maxMP = 66;
	}

	public void EndSoulLimiter()
	{
		soulLimited = false;
		maxMP = 99;
	}

	public void EquipCharm(int charmNum)
	{
		equippedCharms.Add(charmNum);
	}

	public void UnequipCharm(int charmNum)
	{
		equippedCharms.Remove(charmNum);
	}

	public void CalculateNotchesUsed()
	{
		int num = 0;
		if (equippedCharm_1)
		{
			num += charmCost_1;
		}
		if (equippedCharm_2)
		{
			num += charmCost_2;
		}
		if (equippedCharm_3)
		{
			num += charmCost_3;
		}
		if (equippedCharm_4)
		{
			num += charmCost_4;
		}
		if (equippedCharm_5)
		{
			num += charmCost_5;
		}
		if (equippedCharm_6)
		{
			num += charmCost_6;
		}
		if (equippedCharm_7)
		{
			num += charmCost_7;
		}
		if (equippedCharm_8)
		{
			num += charmCost_8;
		}
		if (equippedCharm_9)
		{
			num += charmCost_9;
		}
		if (equippedCharm_10)
		{
			num += charmCost_10;
		}
		if (equippedCharm_11)
		{
			num += charmCost_11;
		}
		if (equippedCharm_12)
		{
			num += charmCost_12;
		}
		if (equippedCharm_13)
		{
			num += charmCost_13;
		}
		if (equippedCharm_14)
		{
			num += charmCost_14;
		}
		if (equippedCharm_15)
		{
			num += charmCost_15;
		}
		if (equippedCharm_16)
		{
			num += charmCost_16;
		}
		if (equippedCharm_17)
		{
			num += charmCost_17;
		}
		if (equippedCharm_18)
		{
			num += charmCost_18;
		}
		if (equippedCharm_19)
		{
			num += charmCost_19;
		}
		if (equippedCharm_20)
		{
			num += charmCost_20;
		}
		if (equippedCharm_21)
		{
			num += charmCost_21;
		}
		if (equippedCharm_22)
		{
			num += charmCost_22;
		}
		if (equippedCharm_23)
		{
			num += charmCost_23;
		}
		if (equippedCharm_24)
		{
			num += charmCost_24;
		}
		if (equippedCharm_25)
		{
			num += charmCost_25;
		}
		if (equippedCharm_26)
		{
			num += charmCost_26;
		}
		if (equippedCharm_27)
		{
			num += charmCost_27;
		}
		if (equippedCharm_28)
		{
			num += charmCost_28;
		}
		if (equippedCharm_29)
		{
			num += charmCost_29;
		}
		if (equippedCharm_30)
		{
			num += charmCost_30;
		}
		if (equippedCharm_31)
		{
			num += charmCost_31;
		}
		if (equippedCharm_32)
		{
			num += charmCost_32;
		}
		if (equippedCharm_33)
		{
			num += charmCost_33;
		}
		if (equippedCharm_34)
		{
			num += charmCost_34;
		}
		if (equippedCharm_35)
		{
			num += charmCost_35;
		}
		if (equippedCharm_36)
		{
			num += charmCost_36;
		}
		if (equippedCharm_37)
		{
			num += charmCost_37;
		}
		if (equippedCharm_38)
		{
			num += charmCost_38;
		}
		if (equippedCharm_39)
		{
			num += charmCost_39;
		}
		if (equippedCharm_40)
		{
			num += charmCost_40;
		}
		charmSlotsFilled = num;
	}

	public void SetBenchRespawn(RespawnMarker spawnMarker, string sceneName, int spawnType)
	{
		respawnMarkerName = spawnMarker.name;
		respawnScene = sceneName;
		respawnType = spawnType;
		respawnFacingRight = spawnMarker.respawnFacingRight;
		GameManager.instance.SetCurrentMapZoneAsRespawn();
	}

	public void SetBenchRespawn(string spawnMarker, string sceneName, bool facingRight)
	{
		respawnMarkerName = spawnMarker;
		respawnScene = sceneName;
		respawnFacingRight = facingRight;
		GameManager.instance.SetCurrentMapZoneAsRespawn();
	}

	public void SetBenchRespawn(string spawnMarker, string sceneName, int spawnType, bool facingRight)
	{
		respawnMarkerName = spawnMarker;
		respawnScene = sceneName;
		respawnType = spawnType;
		respawnFacingRight = facingRight;
		GameManager.instance.SetCurrentMapZoneAsRespawn();
	}

	public void SetHazardRespawn(HazardRespawnMarker location)
	{
		hazardRespawnLocation = location.transform.position;
		hazardRespawnFacing = location.RespawnFacingDirection;
		// hazardRespawnFacingRight = location.respawnFacingRight;//modified
	}

	public void SetHazardRespawn(Vector3 position, bool facingRight)
	{
		hazardRespawnLocation = position;
		hazardRespawnFacingRight = facingRight;//modified
		hazardRespawnFacing = ((!facingRight) ? HazardRespawnMarker.FacingDirection.Left : HazardRespawnMarker.FacingDirection.Right);
	}

	public void CountGameCompletion()
	{
		completionPercentage = 0f;
		CountCharms();
		completionPercentage += charmsOwned;
		if (killedFalseKnight)
		{
			completionPercentage += 1f;
		}
		if (hornet1Defeated)
		{
			completionPercentage += 1f;
		}
		if (hornetOutskirtsDefeated)
		{
			completionPercentage += 1f;
		}
		if (killedMantisLord)
		{
			completionPercentage += 1f;
		}
		if (killedMageLord)
		{
			completionPercentage += 1f;
		}
		if (killedDungDefender)
		{
			completionPercentage += 1f;
		}
		if (killedBlackKnight)
		{
			completionPercentage += 1f;
		}
		if (killedInfectedKnight)
		{
			completionPercentage += 1f;
		}
		if (killedMimicSpider)
		{
			completionPercentage += 1f;
		}
		if (killedMegaJellyfish)
		{
			completionPercentage += 1f;
		}
		if (killedTraitorLord)
		{
			completionPercentage += 1f;
		}
		if (killedJarCollector)
		{
			completionPercentage += 1f;
		}
		if (killedBigFly)
		{
			completionPercentage += 1f;
		}
		if (killedMawlek)
		{
			completionPercentage += 1f;
		}
		if (killedHiveKnight)
		{
			completionPercentage += 1f;
		}
		if (colosseumBronzeCompleted)
		{
			completionPercentage += 1f;
		}
		if (colosseumSilverCompleted)
		{
			completionPercentage += 1f;
		}
		if (colosseumGoldCompleted)
		{
			completionPercentage += 1f;
		}
		if (killedGhostAladar)
		{
			completionPercentage += 1f;
		}
		if (killedGhostHu)
		{
			completionPercentage += 1f;
		}
		if (killedGhostXero)
		{
			completionPercentage += 1f;
		}
		if (killedGhostMarkoth)
		{
			completionPercentage += 1f;
		}
		if (killedGhostNoEyes)
		{
			completionPercentage += 1f;
		}
		if (killedGhostMarmu)
		{
			completionPercentage += 1f;
		}
		if (killedGhostGalien)
		{
			completionPercentage += 1f;
		}
		completionPercentage += fireballLevel;
		completionPercentage += quakeLevel;
		completionPercentage += screamLevel;
		if (hasCyclone)
		{
			completionPercentage += 1f;
		}
		if (hasDashSlash)
		{
			completionPercentage += 1f;
		}
		if (hasUpwardSlash)
		{
			completionPercentage += 1f;
		}
		if (hasDash)
		{
			completionPercentage += 2f;
		}
		if (hasWalljump)
		{
			completionPercentage += 2f;
		}
		if (hasDoubleJump)
		{
			completionPercentage += 2f;
		}
		if (hasAcidArmour)
		{
			completionPercentage += 2f;
		}
		if (hasSuperDash)
		{
			completionPercentage += 2f;
		}
		if (hasShadowDash)
		{
			completionPercentage += 2f;
		}
		if (hasKingsBrand)
		{
			completionPercentage += 2f;
		}
		if (lurienDefeated)
		{
			completionPercentage += 1f;
		}
		if (hegemolDefeated)
		{
			completionPercentage += 1f;
		}
		if (monomonDefeated)
		{
			completionPercentage += 1f;
		}
		if (hasDreamNail)
		{
			completionPercentage += 1f;
		}
		if (dreamNailUpgraded)
		{
			completionPercentage += 1f;
		}
		if (mothDeparted)
		{
			completionPercentage += 1f;
		}
		completionPercentage += nailSmithUpgrades;
		completionPercentage += maxHealthBase - 5;
		switch (MPReserveMax)
		{
			case 33:
				completionPercentage += 1f;
				break;
			case 66:
				completionPercentage += 2f;
				break;
			case 99:
				completionPercentage += 3f;
				break;
		}
		if (killedGrimm)
		{
			completionPercentage += 1f;
		}
		if (killedNightmareGrimm || destroyedNightmareLantern)
		{
			completionPercentage += 1f;
		}
		if (hasGodfinder)
		{
			completionPercentage += 1f;
		}
		if (bossDoorStateTier1.completed)
		{
			completionPercentage += 1f;
		}
		if (bossDoorStateTier2.completed)
		{
			completionPercentage += 1f;
		}
		if (bossDoorStateTier3.completed)
		{
			completionPercentage += 1f;
		}
		if (bossDoorStateTier4.completed)
		{
			completionPercentage += 1f;
		}
	}

	public void CountCharms()
	{
		charmsOwned = 0;
		if (gotCharm_1)
		{
			charmsOwned++;
		}
		if (gotCharm_2)
		{
			charmsOwned++;
		}
		if (gotCharm_3)
		{
			charmsOwned++;
		}
		if (gotCharm_4)
		{
			charmsOwned++;
		}
		if (gotCharm_5)
		{
			charmsOwned++;
		}
		if (gotCharm_6)
		{
			charmsOwned++;
		}
		if (gotCharm_7)
		{
			charmsOwned++;
		}
		if (gotCharm_8)
		{
			charmsOwned++;
		}
		if (gotCharm_9)
		{
			charmsOwned++;
		}
		if (gotCharm_10)
		{
			charmsOwned++;
		}
		if (gotCharm_11)
		{
			charmsOwned++;
		}
		if (gotCharm_12)
		{
			charmsOwned++;
		}
		if (gotCharm_13)
		{
			charmsOwned++;
		}
		if (gotCharm_14)
		{
			charmsOwned++;
		}
		if (gotCharm_15)
		{
			charmsOwned++;
		}
		if (gotCharm_16)
		{
			charmsOwned++;
		}
		if (gotCharm_17)
		{
			charmsOwned++;
		}
		if (gotCharm_18)
		{
			charmsOwned++;
		}
		if (gotCharm_19)
		{
			charmsOwned++;
		}
		if (gotCharm_20)
		{
			charmsOwned++;
		}
		if (gotCharm_21)
		{
			charmsOwned++;
		}
		if (gotCharm_22)
		{
			charmsOwned++;
		}
		if (gotCharm_23)
		{
			charmsOwned++;
		}
		if (gotCharm_24)
		{
			charmsOwned++;
		}
		if (gotCharm_25)
		{
			charmsOwned++;
		}
		if (gotCharm_26)
		{
			charmsOwned++;
		}
		if (gotCharm_27)
		{
			charmsOwned++;
		}
		if (gotCharm_28)
		{
			charmsOwned++;
		}
		if (gotCharm_29)
		{
			charmsOwned++;
		}
		if (gotCharm_30)
		{
			charmsOwned++;
		}
		if (gotCharm_31)
		{
			charmsOwned++;
		}
		if (gotCharm_32)
		{
			charmsOwned++;
		}
		if (gotCharm_33)
		{
			charmsOwned++;
		}
		if (gotCharm_34)
		{
			charmsOwned++;
		}
		if (gotCharm_35)
		{
			charmsOwned++;
		}
		if (royalCharmState > 2)
		{
			charmsOwned++;
		}
		if (gotCharm_37)
		{
			charmsOwned++;
		}
		if (gotCharm_38)
		{
			charmsOwned++;
		}
		if (gotCharm_39)
		{
			charmsOwned++;
		}
		if (gotCharm_40)
		{
			charmsOwned++;
		}
	}

	public void CountJournalEntries()
	{
		journalEntriesCompleted = 0;
		journalNotesCompleted = 0;
		journalEntriesTotal = 146;
		if (killedCrawler)
		{
			journalEntriesCompleted += 2;
		}
		if (killedBuzzer)
		{
			journalEntriesCompleted++;
		}
		if (killedBouncer)
		{
			journalEntriesCompleted++;
		}
		if (killedClimber)
		{
			journalEntriesCompleted++;
		}
		if (killedHopper)
		{
			journalEntriesCompleted++;
		}
		if (killedWorm)
		{
			journalEntriesCompleted++;
		}
		if (killedSpitter)
		{
			journalEntriesCompleted++;
		}
		if (killedHatcher)
		{
			journalEntriesCompleted++;
		}
		if (killedHatchling)
		{
			journalEntriesCompleted++;
		}
		if (killedZombieRunner)
		{
			journalEntriesCompleted++;
		}
		if (killedZombieHornhead)
		{
			journalEntriesCompleted++;
		}
		if (killedZombieLeaper)
		{
			journalEntriesCompleted++;
		}
		if (killedZombieBarger)
		{
			journalEntriesCompleted++;
		}
		if (killedZombieShield)
		{
			journalEntriesCompleted++;
		}
		if (killedZombieGuard)
		{
			journalEntriesCompleted++;
		}
		if (killedBigBuzzer)
		{
			journalEntriesCompleted++;
		}
		if (killedBigFly)
		{
			journalEntriesCompleted++;
		}
		if (killedMawlek)
		{
			journalEntriesCompleted++;
		}
		if (killedFalseKnight)
		{
			journalEntriesCompleted++;
		}
		if (killedRoller)
		{
			journalEntriesCompleted++;
		}
		if (killedBlocker)
		{
			journalEntriesCompleted++;
		}
		if (killedPrayerSlug)
		{
			journalEntriesCompleted++;
		}
		if (killedMenderBug)
		{
			journalEntriesCompleted++;
			journalEntriesTotal++;
		}
		if (killedMossmanRunner)
		{
			journalEntriesCompleted++;
		}
		if (killedMossmanShaker)
		{
			journalEntriesCompleted++;
		}
		if (killedMosquito)
		{
			journalEntriesCompleted++;
		}
		if (killedBlobFlyer)
		{
			journalEntriesCompleted++;
		}
		if (killedFungifiedZombie)
		{
			journalEntriesCompleted++;
		}
		if (killedPlantShooter)
		{
			journalEntriesCompleted++;
		}
		if (killedMossCharger)
		{
			journalEntriesCompleted++;
		}
		if (killedMegaMossCharger)
		{
			journalEntriesCompleted++;
		}
		if (killedSnapperTrap)
		{
			journalEntriesCompleted++;
		}
		if (killedMossKnight)
		{
			journalEntriesCompleted++;
		}
		if (killedGrassHopper)
		{
			journalEntriesCompleted++;
		}
		if (killedAcidFlyer)
		{
			journalEntriesCompleted++;
		}
		if (killedAcidWalker)
		{
			journalEntriesCompleted++;
		}
		if (killedMossFlyer)
		{
			journalEntriesCompleted++;
		}
		if (killedMossKnightFat)
		{
			journalEntriesCompleted++;
		}
		if (killedMossWalker)
		{
			journalEntriesCompleted++;
		}
		if (killedInfectedKnight)
		{
			journalEntriesCompleted++;
		}
		if (killedLazyFlyer)
		{
			journalEntriesCompleted++;
		}
		if (killedZapBug)
		{
			journalEntriesCompleted++;
		}
		if (killedJellyfish)
		{
			journalEntriesCompleted++;
		}
		if (killedJellyCrawler)
		{
			journalEntriesCompleted++;
		}
		if (killedMegaJellyfish)
		{
			journalEntriesCompleted++;
		}
		if (killedFungoonBaby)
		{
			journalEntriesCompleted++;
		}
		if (killedMushroomTurret)
		{
			journalEntriesCompleted++;
		}
		if (killedMantis)
		{
			journalEntriesCompleted++;
		}
		if (killedMushroomRoller)
		{
			journalEntriesCompleted++;
		}
		if (killedMushroomBrawler)
		{
			journalEntriesCompleted++;
		}
		if (killedMushroomBaby)
		{
			journalEntriesCompleted++;
		}
		if (killedMantisFlyerChild)
		{
			journalEntriesCompleted++;
		}
		if (killedFungusFlyer)
		{
			journalEntriesCompleted++;
		}
		if (killedFungCrawler)
		{
			journalEntriesCompleted++;
		}
		if (killedMantisLord)
		{
			journalEntriesCompleted++;
		}
		if (killedBlackKnight)
		{
			journalEntriesCompleted++;
		}
		if (killedMage)
		{
			journalEntriesCompleted++;
		}
		if (killedMageKnight)
		{
			journalEntriesCompleted++;
		}
		if (killedRoyalDandy)
		{
			journalEntriesCompleted++;
		}
		if (killedRoyalCoward)
		{
			journalEntriesCompleted++;
		}
		if (killedRoyalPlumper)
		{
			journalEntriesCompleted++;
		}
		if (killedFlyingSentrySword)
		{
			journalEntriesCompleted++;
		}
		if (killedFlyingSentryJavelin)
		{
			journalEntriesCompleted++;
		}
		if (killedSentry)
		{
			journalEntriesCompleted++;
		}
		if (killedSentryFat)
		{
			journalEntriesCompleted++;
		}
		if (killedMageBlob)
		{
			journalEntriesCompleted++;
		}
		if (killedGreatShieldZombie)
		{
			journalEntriesCompleted++;
		}
		if (killedJarCollector)
		{
			journalEntriesCompleted++;
		}
		if (killedMageBalloon)
		{
			journalEntriesCompleted++;
		}
		if (killedMageLord)
		{
			journalEntriesCompleted++;
		}
		if (killedGorgeousHusk)
		{
			journalEntriesCompleted++;
		}
		if (killedFlipHopper)
		{
			journalEntriesCompleted++;
		}
		if (killedFlukeman)
		{
			journalEntriesCompleted++;
		}
		if (killedInflater)
		{
			journalEntriesCompleted++;
		}
		if (killedFlukefly)
		{
			journalEntriesCompleted++;
		}
		if (killedFlukeMother)
		{
			journalEntriesCompleted++;
		}
		if (killedDungDefender)
		{
			journalEntriesCompleted++;
		}
		if (killedCrystalCrawler)
		{
			journalEntriesCompleted++;
		}
		if (killedCrystalFlyer)
		{
			journalEntriesCompleted++;
		}
		if (killedLaserBug)
		{
			journalEntriesCompleted++;
		}
		if (killedBeamMiner)
		{
			journalEntriesCompleted++;
		}
		if (killedZombieMiner)
		{
			journalEntriesCompleted++;
		}
		if (killedMegaBeamMiner)
		{
			journalEntriesCompleted++;
		}
		if (killedMinesCrawler)
		{
			journalEntriesCompleted++;
		}
		if (killedAngryBuzzer)
		{
			journalEntriesCompleted++;
		}
		if (killedBurstingBouncer)
		{
			journalEntriesCompleted++;
		}
		if (killedBurstingZombie)
		{
			journalEntriesCompleted++;
		}
		if (killedSpittingZombie)
		{
			journalEntriesCompleted++;
		}
		if (killedBabyCentipede)
		{
			journalEntriesCompleted++;
		}
		if (killedBigCentipede)
		{
			journalEntriesCompleted++;
		}
		if (killedCentipedeHatcher)
		{
			journalEntriesCompleted++;
		}
		if (killedLesserMawlek)
		{
			journalEntriesCompleted++;
		}
		if (killedSlashSpider)
		{
			journalEntriesCompleted++;
		}
		if (killedSpiderCorpse)
		{
			journalEntriesCompleted++;
		}
		if (killedShootSpider)
		{
			journalEntriesCompleted++;
		}
		if (killedMiniSpider)
		{
			journalEntriesCompleted++;
		}
		if (killedSpiderFlyer)
		{
			journalEntriesCompleted++;
		}
		if (killedMimicSpider)
		{
			journalEntriesCompleted++;
		}
		if (killedBeeHatchling)
		{
			journalEntriesCompleted++;
		}
		if (killedBeeStinger)
		{
			journalEntriesCompleted++;
		}
		if (killedBigBee)
		{
			journalEntriesCompleted++;
		}
		if (killedHiveKnight)
		{
			journalEntriesCompleted++;
		}
		if (killedBlowFly)
		{
			journalEntriesCompleted++;
		}
		if (killedCeilingDropper)
		{
			journalEntriesCompleted++;
		}
		if (killedGiantHopper)
		{
			journalEntriesCompleted++;
		}
		if (killedGrubMimic)
		{
			journalEntriesCompleted++;
		}
		if (killedMawlekTurret)
		{
			journalEntriesCompleted++;
		}
		if (killedOrangeScuttler)
		{
			journalEntriesCompleted++;
		}
		if (killedHealthScuttler)
		{
			journalEntriesCompleted++;
		}
		if (killedPigeon)
		{
			journalEntriesCompleted++;
		}
		if (killedZombieHive)
		{
			journalEntriesCompleted++;
		}
		if (killedHornet)
		{
			journalEntriesCompleted++;
		}
		if (killedAbyssCrawler)
		{
			journalEntriesCompleted++;
		}
		if (killedSuperSpitter)
		{
			journalEntriesCompleted++;
		}
		if (killedSibling)
		{
			journalEntriesCompleted++;
		}
		if (killedAbyssTendril)
		{
			journalEntriesCompleted++;
		}
		if (killedPalaceFly)
		{
			journalEntriesCompleted++;
		}
		if (killedEggSac)
		{
			journalEntriesCompleted++;
		}
		if (killedMummy)
		{
			journalEntriesCompleted++;
		}
		if (killedOrangeBalloon)
		{
			journalEntriesCompleted++;
		}
		if (killedHeavyMantis)
		{
			journalEntriesCompleted++;
		}
		if (killedTraitorLord)
		{
			journalEntriesCompleted++;
		}
		if (killedMantisHeavyFlyer)
		{
			journalEntriesCompleted++;
		}
		if (killedGardenZombie)
		{
			journalEntriesCompleted++;
		}
		if (killedRoyalGuard)
		{
			journalEntriesCompleted++;
		}
		if (killedWhiteRoyal)
		{
			journalEntriesCompleted++;
		}
		if (killedOblobble)
		{
			journalEntriesCompleted++;
		}
		if (killedZote)
		{
			journalEntriesCompleted++;
			journalEntriesTotal++;
		}
		if (killedBlobble)
		{
			journalEntriesCompleted++;
		}
		if (killedColMosquito)
		{
			journalEntriesCompleted++;
		}
		if (killedColRoller)
		{
			journalEntriesCompleted++;
		}
		if (killedColFlyingSentry)
		{
			journalEntriesCompleted++;
		}
		if (killedColMiner)
		{
			journalEntriesCompleted++;
		}
		if (killedColShield)
		{
			journalEntriesCompleted++;
		}
		if (killedColWorm)
		{
			journalEntriesCompleted++;
		}
		if (killedColHopper)
		{
			journalEntriesCompleted++;
		}
		if (killedElectricMage)
		{
			journalEntriesCompleted++;
		}
		if (killedLobsterLancer)
		{
			journalEntriesCompleted++;
		}
		if (killedGhostAladar)
		{
			journalEntriesCompleted++;
		}
		if (killedGhostXero)
		{
			journalEntriesCompleted++;
		}
		if (killedGhostHu)
		{
			journalEntriesCompleted++;
		}
		if (killedGhostMarmu)
		{
			journalEntriesCompleted++;
		}
		if (killedGhostNoEyes)
		{
			journalEntriesCompleted++;
		}
		if (killedGhostMarkoth)
		{
			journalEntriesCompleted++;
		}
		if (killedGhostGalien)
		{
			journalEntriesCompleted++;
		}
		if (killedWhiteDefender)
		{
			journalEntriesCompleted++;
			journalEntriesTotal++;
		}
		if (killedGreyPrince)
		{
			journalEntriesCompleted++;
			journalEntriesTotal++;
		}
		if (killedZotelingBuzzer)
		{
			journalEntriesCompleted++;
			journalEntriesTotal++;
		}
		if (killedZotelingHopper)
		{
			journalEntriesCompleted++;
			journalEntriesTotal++;
		}
		if (killedZotelingBalloon)
		{
			journalEntriesCompleted++;
			journalEntriesTotal++;
		}
		if (killedHollowKnight)
		{
			journalEntriesCompleted++;
			journalEntriesTotal++;
		}
		if (killedFinalBoss)
		{
			journalEntriesCompleted++;
			journalEntriesTotal++;
		}
		if (killedFlameBearerSmall)
		{
			journalEntriesCompleted++;
			journalEntriesTotal++;
		}
		if (killedFlameBearerMed)
		{
			journalEntriesCompleted++;
			journalEntriesTotal++;
		}
		if (killedFlameBearerLarge)
		{
			journalEntriesCompleted++;
			journalEntriesTotal++;
		}
		if (killedGrimm)
		{
			journalEntriesCompleted++;
			journalEntriesTotal++;
		}
		if (killedNightmareGrimm)
		{
			journalEntriesCompleted++;
			journalEntriesTotal++;
		}
		if (killedFatFluke)
		{
			journalEntriesCompleted++;
		}
		if (killedPaleLurker)
		{
			journalEntriesCompleted++;
		}
		if (killedNailBros)
		{
			journalEntriesCompleted++;
			journalEntriesTotal++;
		}
		if (killedPaintmaster)
		{
			journalEntriesCompleted++;
			journalEntriesTotal++;
		}
		if (killedNailsage)
		{
			journalEntriesCompleted++;
			journalEntriesTotal++;
		}
		if (killedHollowKnightPrime)
		{
			journalEntriesCompleted++;
			journalEntriesTotal++;
		}
		if (killsCrawler == 0)
		{
			journalNotesCompleted += 2;
		}
		if (killsBuzzer == 0)
		{
			journalNotesCompleted++;
		}
		if (killsBouncer == 0)
		{
			journalNotesCompleted++;
		}
		if (killsClimber == 0)
		{
			journalNotesCompleted++;
		}
		if (killsHopper == 0)
		{
			journalNotesCompleted++;
		}
		if (killsWorm == 0)
		{
			journalNotesCompleted++;
		}
		if (killsSpitter == 0)
		{
			journalNotesCompleted++;
		}
		if (killsHatcher == 0)
		{
			journalNotesCompleted++;
		}
		if (killsHatchling == 0)
		{
			journalNotesCompleted++;
		}
		if (killsZombieRunner == 0)
		{
			journalNotesCompleted++;
		}
		if (killsZombieHornhead == 0)
		{
			journalNotesCompleted++;
		}
		if (killsZombieLeaper == 0)
		{
			journalNotesCompleted++;
		}
		if (killsZombieBarger == 0)
		{
			journalNotesCompleted++;
		}
		if (killsZombieShield == 0)
		{
			journalNotesCompleted++;
		}
		if (killsZombieGuard == 0)
		{
			journalNotesCompleted++;
		}
		if (killsBigBuzzer == 0)
		{
			journalNotesCompleted++;
		}
		if (killsBigFly == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMawlek == 0)
		{
			journalNotesCompleted++;
		}
		if (killsFalseKnight == 0)
		{
			journalNotesCompleted++;
		}
		if (killsRoller == 0)
		{
			journalNotesCompleted++;
		}
		if (killsBlocker == 0)
		{
			journalNotesCompleted++;
		}
		if (killsPrayerSlug == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMenderBug == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMossmanRunner == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMossmanShaker == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMosquito == 0)
		{
			journalNotesCompleted++;
		}
		if (killsBlobFlyer == 0)
		{
			journalNotesCompleted++;
		}
		if (killsFungifiedZombie == 0)
		{
			journalNotesCompleted++;
		}
		if (killsPlantShooter == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMossCharger == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMegaMossCharger == 0)
		{
			journalNotesCompleted++;
		}
		if (killsSnapperTrap == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMossKnight == 0)
		{
			journalNotesCompleted++;
		}
		if (killsGrassHopper == 0)
		{
			journalNotesCompleted++;
		}
		if (killsAcidFlyer == 0)
		{
			journalNotesCompleted++;
		}
		if (killsAcidWalker == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMossFlyer == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMossKnightFat == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMossWalker == 0)
		{
			journalNotesCompleted++;
		}
		if (killsInfectedKnight == 0)
		{
			journalNotesCompleted++;
		}
		if (killsLazyFlyer == 0)
		{
			journalNotesCompleted++;
		}
		if (killsZapBug == 0)
		{
			journalNotesCompleted++;
		}
		if (killsJellyfish == 0)
		{
			journalNotesCompleted++;
		}
		if (killsJellyCrawler == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMegaJellyfish == 0)
		{
			journalNotesCompleted++;
		}
		if (killsFungoonBaby == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMushroomTurret == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMantis == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMushroomRoller == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMushroomBrawler == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMushroomBaby == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMantisFlyerChild == 0)
		{
			journalNotesCompleted++;
		}
		if (killsFungusFlyer == 0)
		{
			journalNotesCompleted++;
		}
		if (killsFungCrawler == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMantisLord == 0)
		{
			journalNotesCompleted++;
		}
		if (killsBlackKnight == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMage == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMageKnight == 0)
		{
			journalNotesCompleted++;
		}
		if (killsRoyalDandy == 0)
		{
			journalNotesCompleted++;
		}
		if (killsRoyalCoward == 0)
		{
			journalNotesCompleted++;
		}
		if (killsRoyalPlumper == 0)
		{
			journalNotesCompleted++;
		}
		if (killsFlyingSentrySword == 0)
		{
			journalNotesCompleted++;
		}
		if (killsFlyingSentryJavelin == 0)
		{
			journalNotesCompleted++;
		}
		if (killsSentry == 0)
		{
			journalNotesCompleted++;
		}
		if (killsSentryFat == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMageBlob == 0)
		{
			journalNotesCompleted++;
		}
		if (killsGreatShieldZombie == 0)
		{
			journalNotesCompleted++;
		}
		if (killsJarCollector == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMageBalloon == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMageLord == 0)
		{
			journalNotesCompleted++;
		}
		if (killsGorgeousHusk == 0)
		{
			journalNotesCompleted++;
		}
		if (killsFlipHopper == 0)
		{
			journalNotesCompleted++;
		}
		if (killsFlukeman == 0)
		{
			journalNotesCompleted++;
		}
		if (killsInflater == 0)
		{
			journalNotesCompleted++;
		}
		if (killsFlukefly == 0)
		{
			journalNotesCompleted++;
		}
		if (killsFlukeMother == 0)
		{
			journalNotesCompleted++;
		}
		if (killsDungDefender == 0)
		{
			journalNotesCompleted++;
		}
		if (killsCrystalCrawler == 0)
		{
			journalNotesCompleted++;
		}
		if (killsCrystalFlyer == 0)
		{
			journalNotesCompleted++;
		}
		if (killsLaserBug == 0)
		{
			journalNotesCompleted++;
		}
		if (killsBeamMiner == 0)
		{
			journalNotesCompleted++;
		}
		if (killsZombieMiner == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMegaBeamMiner == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMinesCrawler == 0)
		{
			journalNotesCompleted++;
		}
		if (killsAngryBuzzer == 0)
		{
			journalNotesCompleted++;
		}
		if (killsBurstingBouncer == 0)
		{
			journalNotesCompleted++;
		}
		if (killsBurstingZombie == 0)
		{
			journalNotesCompleted++;
		}
		if (killsSpittingZombie == 0)
		{
			journalNotesCompleted++;
		}
		if (killsBabyCentipede == 0)
		{
			journalNotesCompleted++;
		}
		if (killsBigCentipede == 0)
		{
			journalNotesCompleted++;
		}
		if (killsCentipedeHatcher == 0)
		{
			journalNotesCompleted++;
		}
		if (killsLesserMawlek == 0)
		{
			journalNotesCompleted++;
		}
		if (killsSlashSpider == 0)
		{
			journalNotesCompleted++;
		}
		if (killsSpiderCorpse == 0)
		{
			journalNotesCompleted++;
		}
		if (killsShootSpider == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMiniSpider == 0)
		{
			journalNotesCompleted++;
		}
		if (killsSpiderFlyer == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMimicSpider == 0)
		{
			journalNotesCompleted++;
		}
		if (killsBeeHatchling == 0)
		{
			journalNotesCompleted++;
		}
		if (killsBeeStinger == 0)
		{
			journalNotesCompleted++;
		}
		if (killsBigBee == 0)
		{
			journalNotesCompleted++;
		}
		if (killsHiveKnight == 0)
		{
			journalNotesCompleted++;
		}
		if (killsBlowFly == 0)
		{
			journalNotesCompleted++;
		}
		if (killsCeilingDropper == 0)
		{
			journalNotesCompleted++;
		}
		if (killsGiantHopper == 0)
		{
			journalNotesCompleted++;
		}
		if (killsGrubMimic == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMawlekTurret == 0)
		{
			journalNotesCompleted++;
		}
		if (killsOrangeScuttler == 0)
		{
			journalNotesCompleted++;
		}
		if (killsHealthScuttler == 0)
		{
			journalNotesCompleted++;
		}
		if (killsPigeon == 0)
		{
			journalNotesCompleted++;
		}
		if (killsZombieHive == 0)
		{
			journalNotesCompleted++;
		}
		if (killsHornet == 0)
		{
			journalNotesCompleted++;
		}
		if (killsAbyssCrawler == 0)
		{
			journalNotesCompleted++;
		}
		if (killsSuperSpitter == 0)
		{
			journalNotesCompleted++;
		}
		if (killsSibling == 0)
		{
			journalNotesCompleted++;
		}
		if (killsAbyssTendril == 0)
		{
			journalNotesCompleted++;
		}
		if (killsPalaceFly == 0)
		{
			journalNotesCompleted++;
		}
		if (killsEggSac == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMummy == 0)
		{
			journalNotesCompleted++;
		}
		if (killsOrangeBalloon == 0)
		{
			journalNotesCompleted++;
		}
		if (killsHeavyMantis == 0)
		{
			journalNotesCompleted++;
		}
		if (killsTraitorLord == 0)
		{
			journalNotesCompleted++;
		}
		if (killsMantisHeavyFlyer == 0)
		{
			journalNotesCompleted++;
		}
		if (killsGardenZombie == 0)
		{
			journalNotesCompleted++;
		}
		if (killsRoyalGuard == 0)
		{
			journalNotesCompleted++;
		}
		if (killsWhiteRoyal == 0)
		{
			journalNotesCompleted++;
		}
		if (killsOblobble == 0)
		{
			journalNotesCompleted++;
		}
		if (killsZote == 0)
		{
			journalNotesCompleted++;
		}
		if (killsBlobble == 0)
		{
			journalNotesCompleted++;
		}
		if (killsColMosquito == 0)
		{
			journalNotesCompleted++;
		}
		if (killsColRoller == 0)
		{
			journalNotesCompleted++;
		}
		if (killsColFlyingSentry == 0)
		{
			journalNotesCompleted++;
		}
		if (killsColMiner == 0)
		{
			journalNotesCompleted++;
		}
		if (killsColShield == 0)
		{
			journalNotesCompleted++;
		}
		if (killsColWorm == 0)
		{
			journalNotesCompleted++;
		}
		if (killsColHopper == 0)
		{
			journalNotesCompleted++;
		}
		if (killsElectricMage == 0)
		{
			journalNotesCompleted++;
		}
		if (killsLobsterLancer == 0)
		{
			journalNotesCompleted++;
		}
		if (killsGhostAladar == 0)
		{
			journalNotesCompleted++;
		}
		if (killsGhostXero == 0)
		{
			journalNotesCompleted++;
		}
		if (killsGhostHu == 0)
		{
			journalNotesCompleted++;
		}
		if (killsGhostMarmu == 0)
		{
			journalNotesCompleted++;
		}
		if (killsGhostNoEyes == 0)
		{
			journalNotesCompleted++;
		}
		if (killsGhostMarkoth == 0)
		{
			journalNotesCompleted++;
		}
		if (killsGhostGalien == 0)
		{
			journalNotesCompleted++;
		}
		if (killedWhiteDefender)
		{
			journalNotesCompleted++;
		}
		if (killedGreyPrince)
		{
			journalNotesCompleted++;
		}
		if (killedZotelingBuzzer)
		{
			journalNotesCompleted++;
		}
		if (killedZotelingHopper)
		{
			journalNotesCompleted++;
		}
		if (killedZotelingBalloon)
		{
			journalNotesCompleted++;
		}
		if (killsHollowKnight == 0)
		{
			journalNotesCompleted++;
		}
		if (killsFinalBoss == 0)
		{
			journalNotesCompleted++;
		}
		if (killedFlameBearerSmall)
		{
			journalNotesCompleted++;
		}
		if (killedFlameBearerMed)
		{
			journalNotesCompleted++;
		}
		if (killedFlameBearerLarge)
		{
			journalNotesCompleted++;
		}
		if (killedGrimm)
		{
			journalNotesCompleted++;
		}
		if (killedNightmareGrimm)
		{
			journalNotesCompleted++;
		}
		if (killedFatFluke)
		{
			journalNotesCompleted++;
		}
		if (killedPaleLurker)
		{
			journalNotesCompleted++;
		}
		if (killedNailBros)
		{
			journalNotesCompleted++;
		}
		if (killedPaintmaster)
		{
			journalNotesCompleted++;
		}
		if (killedNailsage)
		{
			journalNotesCompleted++;
		}
		if (killedHollowKnightPrime)
		{
			journalNotesCompleted++;
		}
	}

	private void SetupNewPlayerData()
	{
		openingCreditsPlayed = false;
		playTime = 0f;
		completionPercent = 0f;
		permadeathMode = 0;
		version = "1.5.78.11833";
		awardAllAchievements = false;
		health = 5;
		maxHealth = 5;
		maxHealthBase = 5;
		healthBlue = 0;
		joniHealthBlue = 0;
		damagedBlue = false;
		heartPieces = 0;
		heartPieceCollected = false;
		maxHealthCap = 9;
		heartPieceMax = false;
		prevHealth = health;
		blockerHits = 4;
		firstGeo = false;
		geo = 0;
		maxMP = 99;
		MPCharge = 0;
		MPReserve = 0;
		MPReserveMax = 0;
		soulLimited = false;
		focusMP_amount = 33;
		vesselFragments = 0;
		vesselFragmentCollected = false;
		MPReserveCap = 99;
		vesselFragmentMax = false;
		atBench = false;
		respawnScene = "Tutorial_01";
		respawnMarkerName = "Death Respawn Marker";
		mapZone = MapZone.MOSS_CAVE;
		respawnType = 0;
		respawnFacingRight = false;
		hazardRespawnFacingRight = false;
		shadeScene = "None";
		shadeMapZone = "";
		shadePositionX = -999f;
		shadePositionY = -999f;
		shadeHealth = 0;
		shadeMP = 0;
		shadeFireballLevel = 0;
		shadeQuakeLevel = 0;
		shadeScreamLevel = 0;
		shadeSpecialType = 0;
		shadeMapPos = new Vector3(0f, 0f, 0f);
		dreamgateMapPos = new Vector3(0f, 0f, 0f);
		geoPool = 0;
		nailDamage = 5;
		nailRange = 0;
		beamDamage = 0;
		canDash = false;
		canBackDash = false;
		canWallJump = false;
		canSuperDash = false;
		canShadowDash = false;
		hasSpell = false;
		fireballLevel = 0;
		quakeLevel = 0;
		screamLevel = 0;
		hasNailArt = false;
		hasCyclone = false;
		hasDashSlash = false;
		hasUpwardSlash = false;
		hasAllNailArts = false;
		hasDreamNail = false;
		hasDreamGate = false;
		dreamNailUpgraded = false;
		dreamOrbs = 0;
		dreamOrbsSpent = 0;
		dreamGateScene = "";
		dreamGateX = 0f;
		dreamGateY = 0f;
		hasDash = false;
		hasWalljump = false;
		hasSuperDash = false;
		hasShadowDash = false;
		hasAcidArmour = false;
		hasDoubleJump = false;
		hasLantern = false;
		hasTramPass = false;
		hasQuill = false;
		hasCityKey = false;
		hasSlykey = false;
		gaveSlykey = false;
		hasWhiteKey = false;
		usedWhiteKey = false;
		hasMenderKey = false;
		hasWaterwaysKey = false;
		hasSpaKey = false;
		hasLoveKey = false;
		hasKingsBrand = false;
		hasXunFlower = false;
		ghostCoins = 0;
		ore = 0;
		foundGhostCoin = false;
		trinket1 = 0;
		foundTrinket1 = false;
		trinket2 = 0;
		foundTrinket2 = false;
		trinket3 = 0;
		foundTrinket3 = false;
		trinket4 = 0;
		foundTrinket4 = false;
		noTrinket1 = false;
		noTrinket2 = false;
		noTrinket3 = false;
		noTrinket4 = false;
		soldTrinket1 = 0;
		soldTrinket2 = 0;
		soldTrinket3 = 0;
		soldTrinket4 = 0;
		simpleKeys = 0;
		rancidEggs = 0;
		notchShroomOgres = false;
		notchFogCanyon = false;
		gotLurkerKey = false;
		gMap_doorX = 0f;
		gMap_doorY = 0f;
		gMap_doorScene = "";
		gMap_doorMapZone = "";
		gMap_doorOriginOffsetX = 0f;
		gMap_doorOriginOffsetY = 0f;
		gMap_doorSceneWidth = 0f;
		gMap_doorSceneHeight = 0f;
		guardiansDefeated = 0;
		lurienDefeated = false;
		hegemolDefeated = false;
		monomonDefeated = false;
		maskBrokenLurien = false;
		maskBrokenHegemol = false;
		maskBrokenMonomon = false;
		maskToBreak = 0;
		elderbug = 0;
		metElderbug = false;
		elderbugReintro = false;
		elderbugHistory = 0;
		elderbugHistory1 = false;
		elderbugHistory2 = false;
		elderbugHistory3 = false;
		elderbugSpeechSly = false;
		elderbugSpeechStation = false;
		elderbugSpeechEggTemple = false;
		elderbugSpeechMapShop = false;
		elderbugSpeechBretta = false;
		elderbugSpeechJiji = false;
		elderbugSpeechMinesLift = false;
		elderbugSpeechKingsPass = false;
		elderbugSpeechInfectedCrossroads = false;
		elderbugSpeechFinalBossDoor = false;
		elderbugRequestedFlower = false;
		elderbugGaveFlower = false;
		elderbugFirstCall = false;
		metQuirrel = false;
		quirrelEggTemple = 0;
		quirrelLeftEggTemple = false;
		quirrelSlugShrine = 0;
		quirrelRuins = 0;
		quirrelMines = 0;
		quirrelLeftStation = false;
		quirrelCityEncountered = false;
		quirrelCityLeft = false;
		quirrelMinesEncountered = false;
		quirrelMinesLeft = false;
		visitedDeepnestSpa = false;
		quirrelSpaReady = false;
		quirrelSpaEncountered = false;
		quirrelArchiveEncountered = false;
		quirrelEpilogueCompleted = false;
		quirrelMantisEncountered = false;
		enteredMantisLordArea = false;
		metRelicDealer = false;
		metRelicDealerShop = false;
		marmOutside = false;
		marmOutsideConvo = false;
		marmConvo1 = false;
		marmConvo2 = false;
		marmConvo3 = false;
		marmConvoNailsmith = false;
		cornifer = 0;
		metCornifer = false;
		corniferIntroduced = false;
		corniferAtHome = false;
		corn_crossroadsEncountered = false;
		corn_crossroadsLeft = false;
		corn_greenpathEncountered = false;
		corn_greenpathLeft = false;
		corn_fogCanyonEncountered = false;
		corn_fogCanyonLeft = false;
		corn_fungalWastesEncountered = false;
		corn_fungalWastesLeft = false;
		corn_cityEncountered = false;
		corn_cityLeft = false;
		corn_waterwaysEncountered = false;
		corn_waterwaysLeft = false;
		corn_minesEncountered = false;
		corn_minesLeft = false;
		corn_cliffsEncountered = false;
		corn_cliffsLeft = false;
		corn_deepnestEncountered = false;
		corn_deepnestLeft = false;
		corn_deepnestMet1 = false;
		corn_deepnestMet2 = false;
		corn_outskirtsEncountered = false;
		corn_outskirtsLeft = false;
		corn_royalGardensEncountered = false;
		corn_royalGardensLeft = false;
		corn_abyssEncountered = false;
		corn_abyssLeft = false;
		metIselda = false;
		iseldaCorniferHomeConvo = false;
		iseldaConvo1 = false;
		brettaRescued = false;
		brettaPosition = 0;
		brettaState = 0;
		brettaSeenBench = false;
		brettaSeenBed = false;
		brettaSeenBenchDiary = false;
		brettaSeenBedDiary = false;
		brettaLeftTown = false;
		slyRescued = false;
		slyBeta = false;
		metSlyShop = false;
		gotSlyCharm = false;
		slyShellFrag1 = false;
		slyShellFrag2 = false;
		slyShellFrag3 = false;
		slyShellFrag4 = false;
		slyVesselFrag1 = false;
		slyVesselFrag2 = false;
		slyVesselFrag3 = false;
		slyVesselFrag4 = false;
		slyNotch1 = false;
		slyNotch2 = false;
		slySimpleKey = false;
		slyRancidEgg = false;
		slyConvoNailArt = false;
		slyConvoMapper = false;
		slyConvoNailHoned = false;
		jijiDoorUnlocked = false;
		jijiMet = false;
		jijiShadeOffered = false;
		jijiShadeCharmConvo = false;
		metJinn = false;
		jinnConvo1 = false;
		jinnConvo2 = false;
		jinnConvo3 = false;
		jinnConvoKingBrand = false;
		jinnConvoShadeCharm = false;
		jinnEggsSold = 0;
		zote = 0;
		zoteDead = false;
		zoteDeathPos = 0;
		zoteRescuedBuzzer = false;
		zoteSpokenCity = false;
		zoteLeftCity = false;
		zoteRescuedDeepnest = false;
		zoteDefeated = false;
		zoteTrappedDeepnest = false;
		zoteSpokenColosseum = false;
		zotePrecept = 1;
		zoteTownConvo = 0;
		shaman = 0;
		shamanScreamConvo = false;
		shamanQuakeConvo = false;
		shamanFireball2Convo = false;
		shamanScream2Convo = false;
		shamanQuake2Convo = false;
		metMiner = false;
		miner = 0;
		minerEarly = 0;
		hornetGreenpath = 0;
		hornetFung = 0;
		hornet_f19 = false;
		hornetFountainEncounter = false;
		hornetCityBridge_ready = false;
		hornetCityBridge_completed = false;
		hornetAbyssEncounter = false;
		hornetDenEncounter = false;
		metMoth = false;
		ignoredMoth = false;
		gladeDoorOpened = false;
		mothDeparted = false;
		completedRGDreamPlant = false;
		dreamReward1 = false;
		dreamReward2 = false;
		dreamReward3 = false;
		dreamReward4 = false;
		dreamReward5 = false;
		dreamReward5b = false;
		dreamReward6 = false;
		dreamReward7 = false;
		dreamReward8 = false;
		dreamReward9 = false;
		dreamMothConvo1 = false;
		bankerAccountPurchased = false;
		metBanker = false;
		bankerBalance = 0;
		bankerDeclined = false;
		bankerTheftCheck = false;
		bankerTheft = 0;
		bankerSpaMet = false;
		metGiraffe = false;
		metCharmSlug = false;
		salubraNotch1 = false;
		salubraNotch2 = false;
		salubraNotch3 = false;
		salubraNotch4 = false;
		salubraBlessing = false;
		salubraConvoCombo = false;
		salubraConvoOvercharm = false;
		salubraConvoTruth = false;
		cultistTransformed = false;
		metNailsmith = false;
		honedNail = false;
		nailSmithUpgrades = 0;
		nailsmithCliff = false;
		nailsmithKilled = false;
		nailsmithSpared = false;
		nailsmithKillSpeech = false;
		nailsmithSheo = false;
		nailsmithConvoArt = false;
		metNailmasterMato = false;
		metNailmasterSheo = false;
		metNailmasterOro = false;
		matoConvoSheo = false;
		matoConvoOro = false;
		matoConvoSly = false;
		sheoConvoMato = false;
		sheoConvoOro = false;
		sheoConvoSly = false;
		sheoConvoNailsmith = false;
		oroConvoSheo = false;
		oroConvoMato = false;
		oroConvoSly = false;
		hunterRoared = false;
		metHunter = false;
		hunterRewardOffered = false;
		huntersMarkOffered = false;
		hasHuntersMark = false;
		metLegEater = false;
		paidLegEater = false;
		refusedLegEater = false;
		legEaterBrokenConvo = false;
		legEaterDungConvo = false;
		legEaterInfectedCrossroadConvo = false;
		legEaterBoughtConvo = false;
		legEaterConvo1 = false;
		legEaterConvo2 = false;
		legEaterConvo3 = false;
		legEaterGoldConvo = false;
		legEaterLeft = false;
		tukMet = false;
		tukEggPrice = 0;
		tukDungEgg = false;
		metEmilitia = false;
		emilitiaKingsBrandConvo = false;
		metCloth = false;
		clothEnteredTramRoom = false;
		savedCloth = false;
		clothEncounteredQueensGarden = false;
		clothKilled = false;
		clothInTown = false;
		clothLeftTown = false;
		clothGhostSpoken = false;
		bigCatHitTail = false;
		bigCatHitTailConvo = false;
		bigCatMeet = false;
		bigCatTalk1 = false;
		bigCatTalk2 = false;
		bigCatTalk3 = false;
		bigCatKingsBrandConvo = false;
		bigCatShadeConvo = false;
		tisoEncounteredTown = false;
		tisoEncounteredBench = false;
		tisoEncounteredLake = false;
		tisoEncounteredColosseum = false;
		tisoShieldConvo = false;
		tisoDead = false;
		mossCultist = 0;
		maskmakerMet = false;
		maskmakerConvo1 = false;
		maskmakerConvo2 = false;
		maskmakerUnmasked1 = false;
		maskmakerUnmasked2 = false;
		maskmakerShadowDash = false;
		maskmakerKingsBrand = false;
		dungDefenderConvo1 = false;
		dungDefenderConvo2 = false;
		dungDefenderConvo3 = false;
		dungDefenderCharmConvo = false;
		dungDefenderIsmaConvo = false;
		dungDefenderAwakeConvo = false;
		dungDefenderAwoken = false;
		dungDefenderLeft = false;
		midwifeMet = false;
		midwifeConvo1 = false;
		midwifeConvo2 = false;
		metQueen = false;
		queenTalk1 = false;
		queenTalk2 = false;
		queenDung1 = false;
		queenDung2 = false;
		queenHornet = false;
		queenTalkExtra = false;
		gotQueenFragment = false;
		gotKingFragment = false;
		metXun = false;
		xunFlowerBroken = false;
		xunFlowerBrokeTimes = 0;
		xunFlowerGiven = false;
		xunRewardGiven = false;
		xunFailedConvo1 = false;
		xunFailedConvo2 = true;
		menderState = 0;
		menderSignBroken = false;
		allBelieverTabletsDestroyed = false;
		mrMushroomState = 0;
		openedMapperShop = false;
		openedSlyShop = false;
		metStag = false;
		travelling = false;
		stagPosition = -1;
		stationsOpened = 0;
		stagConvoTram = false;
		stagConvoTiso = false;
		stagRemember1 = false;
		stagRemember2 = false;
		stagRemember3 = false;
		stagEggInspected = false;
		stagHopeConvo = false;
		nextScene = "";
		littleFoolMet = false;
		ranAway = false;
		seenColosseumTitle = false;
		colosseumBronzeOpened = false;
		colosseumBronzeCompleted = false;
		colosseumSilverOpened = false;
		colosseumSilverCompleted = false;
		colosseumGoldOpened = false;
		colosseumGoldCompleted = false;
		openedTown = true;
		openedTownBuilding = false;
		openedCrossroads = false;
		openedGreenpath = false;
		openedFungalWastes = false;
		openedRuins1 = false;
		openedRuins2 = false;
		openedRoyalGardens = false;
		openedRestingGrounds = false;
		openedDeepnest = false;
		openedStagNest = false;
		openedHiddenStation = false;
		dreamReturnScene = "Dream_NailCollection";
		charmSlots = 3;
		charmSlotsFilled = 0;
		hasCharm = false;
		equippedCharms = new List<int>();
		charmBenchMsg = false;
		charmsOwned = 0;
		canOvercharm = false;
		overcharmed = false;
		gotCharm_1 = false;
		equippedCharm_1 = false;
		charmCost_1 = 1;
		newCharm_1 = true;
		gotCharm_2 = false;
		equippedCharm_2 = false;
		charmCost_2 = 1;
		newCharm_2 = true;
		gotCharm_3 = false;
		equippedCharm_3 = false;
		charmCost_3 = 1;
		newCharm_3 = true;
		gotCharm_4 = false;
		equippedCharm_4 = false;
		charmCost_4 = 2;
		newCharm_4 = true;
		gotCharm_5 = false;
		equippedCharm_5 = false;
		charmCost_5 = 2;
		newCharm_5 = true;
		gotCharm_6 = false;
		equippedCharm_6 = false;
		charmCost_6 = 2;
		newCharm_6 = true;
		gotCharm_7 = false;
		equippedCharm_7 = false;
		charmCost_7 = 3;
		newCharm_7 = true;
		gotCharm_8 = false;
		equippedCharm_8 = false;
		charmCost_8 = 2;
		newCharm_8 = true;
		gotCharm_9 = false;
		equippedCharm_9 = false;
		charmCost_9 = 3;
		newCharm_9 = true;
		gotCharm_10 = false;
		equippedCharm_10 = false;
		charmCost_10 = 1;
		newCharm_10 = true;
		gotCharm_11 = false;
		equippedCharm_11 = false;
		charmCost_11 = 3;
		newCharm_11 = true;
		gotCharm_12 = false;
		equippedCharm_12 = false;
		charmCost_12 = 1;
		newCharm_12 = true;
		gotCharm_13 = false;
		equippedCharm_13 = false;
		charmCost_13 = 3;
		newCharm_13 = true;
		gotCharm_14 = false;
		equippedCharm_14 = false;
		charmCost_14 = 1;
		newCharm_14 = true;
		gotCharm_15 = false;
		equippedCharm_15 = false;
		charmCost_15 = 2;
		newCharm_15 = true;
		gotCharm_16 = false;
		equippedCharm_16 = false;
		charmCost_16 = 2;
		newCharm_16 = true;
		gotCharm_17 = false;
		equippedCharm_17 = false;
		charmCost_17 = 1;
		newCharm_17 = true;
		gotCharm_18 = false;
		equippedCharm_18 = false;
		charmCost_18 = 2;
		newCharm_18 = true;
		gotCharm_19 = false;
		equippedCharm_19 = false;
		charmCost_19 = 3;
		newCharm_19 = true;
		gotCharm_20 = false;
		equippedCharm_20 = false;
		charmCost_20 = 2;
		newCharm_20 = true;
		gotCharm_21 = false;
		equippedCharm_21 = false;
		charmCost_21 = 4;
		newCharm_21 = true;
		gotCharm_22 = false;
		equippedCharm_22 = false;
		charmCost_22 = 2;
		newCharm_22 = true;
		gotCharm_23 = false;
		equippedCharm_23 = false;
		brokenCharm_23 = false;
		charmCost_23 = 2;
		newCharm_23 = true;
		gotCharm_24 = false;
		equippedCharm_24 = false;
		brokenCharm_24 = false;
		charmCost_24 = 2;
		newCharm_24 = true;
		gotCharm_25 = false;
		equippedCharm_25 = false;
		brokenCharm_25 = false;
		charmCost_25 = 3;
		newCharm_25 = true;
		gotCharm_26 = false;
		equippedCharm_26 = false;
		charmCost_26 = 1;
		newCharm_26 = true;
		gotCharm_27 = false;
		equippedCharm_27 = false;
		charmCost_27 = 4;
		newCharm_27 = true;
		gotCharm_28 = false;
		equippedCharm_28 = false;
		charmCost_28 = 2;
		newCharm_28 = true;
		gotCharm_29 = false;
		equippedCharm_29 = false;
		charmCost_29 = 4;
		newCharm_29 = true;
		gotCharm_30 = false;
		equippedCharm_30 = false;
		charmCost_30 = 1;
		newCharm_30 = true;
		gotCharm_31 = false;
		equippedCharm_31 = false;
		charmCost_31 = 2;
		newCharm_31 = true;
		gotCharm_32 = false;
		equippedCharm_32 = false;
		charmCost_32 = 3;
		newCharm_32 = true;
		gotCharm_33 = false;
		equippedCharm_33 = false;
		charmCost_33 = 2;
		newCharm_33 = true;
		gotCharm_34 = false;
		equippedCharm_34 = false;
		charmCost_34 = 4;
		newCharm_34 = true;
		gotCharm_35 = false;
		equippedCharm_35 = false;
		charmCost_35 = 3;
		newCharm_35 = true;
		gotCharm_36 = false;
		equippedCharm_36 = false;
		charmCost_36 = 5;
		newCharm_36 = true;
		gotCharm_37 = false;
		equippedCharm_37 = false;
		charmCost_37 = 1;
		newCharm_37 = false;
		gotCharm_38 = false;
		equippedCharm_38 = false;
		charmCost_38 = 3;
		newCharm_38 = false;
		gotCharm_39 = false;
		equippedCharm_39 = false;
		charmCost_39 = 2;
		newCharm_39 = false;
		gotCharm_40 = false;
		equippedCharm_40 = false;
		charmCost_40 = 2;
		newCharm_40 = false;
		fragileHealth_unbreakable = false;
		fragileGreed_unbreakable = false;
		fragileStrength_unbreakable = false;
		royalCharmState = 0;
		hasJournal = false;
		lastJournalItem = 0;
		killedDummy = false;
		killsDummy = 0;
		newDataDummy = false;
		seenJournalMsg = false;
		seenHunterMsg = false;
		fillJournal = false;
		journalEntriesCompleted = 0;
		journalNotesCompleted = 0;
		journalEntriesTotal = 0;
		killedCrawler = true;
		killsCrawler = 0;
		newDataCrawler = false;
		killedBuzzer = false;
		killsBuzzer = 45;
		newDataBuzzer = false;
		killedBouncer = false;
		killsBouncer = 25;
		newDataBouncer = false;
		killedClimber = false;
		killsClimber = 30;
		newDataClimber = false;
		killedHopper = false;
		killsHopper = 25;
		newDataHopper = false;
		killedWorm = false;
		killsWorm = 10;
		newDataWorm = false;
		killedSpitter = false;
		killsSpitter = 20;
		newDataSpitter = false;
		killedHatcher = false;
		killsHatcher = 15;
		newDataHatcher = false;
		killedHatchling = false;
		killsHatchling = 30;
		newDataHatchling = false;
		killedZombieRunner = false;
		killsZombieRunner = 35;
		newDataZombieRunner = false;
		killedZombieHornhead = false;
		killsZombieHornhead = 35;
		newDataZombieHornhead = false;
		killedZombieLeaper = false;
		killsZombieLeaper = 35;
		newDataZombieLeaper = false;
		killedZombieBarger = false;
		killsZombieBarger = 35;
		newDataZombieBarger = false;
		killedZombieShield = false;
		killsZombieShield = 10;
		newDataZombieShield = false;
		killedZombieGuard = false;
		killsZombieGuard = 6;
		newDataZombieGuard = false;
		killedBigBuzzer = false;
		killsBigBuzzer = 2;
		newDataBigBuzzer = false;
		killedBigFly = false;
		killsBigFly = 3;
		newDataBigFly = false;
		killedMawlek = false;
		killsMawlek = 1;
		newDataMawlek = false;
		killedFalseKnight = false;
		killsFalseKnight = 1;
		newDataFalseKnight = false;
		killedRoller = false;
		killsRoller = 20;
		newDataRoller = false;
		killedBlocker = false;
		killsBlocker = 1;
		newDataBlocker = false;
		killedPrayerSlug = false;
		killsPrayerSlug = 2;
		newDataPrayerSlug = false;
		killedMenderBug = false;
		killsMenderBug = 1;
		newDataMenderBug = false;
		killedMossmanRunner = false;
		killsMossmanRunner = 25;
		newDataMossmanRunner = false;
		killedMossmanShaker = false;
		killsMossmanShaker = 25;
		newDataMossmanShaker = false;
		killedMosquito = false;
		killsMosquito = 25;
		newDataMosquito = false;
		killedBlobFlyer = false;
		killsBlobFlyer = 20;
		newDataBlobFlyer = false;
		killedFungifiedZombie = false;
		killsFungifiedZombie = 10;
		newDataFungifiedZombie = false;
		killedPlantShooter = false;
		killsPlantShooter = 15;
		newDataPlantShooter = false;
		killedMossCharger = false;
		killsMossCharger = 15;
		newDataMossCharger = false;
		killedMegaMossCharger = false;
		killsMegaMossCharger = 1;
		newDataMegaMossCharger = false;
		killedSnapperTrap = false;
		killsSnapperTrap = 15;
		newDataSnapperTrap = false;
		killedMossKnight = false;
		killsMossKnight = 8;
		newDataMossKnight = false;
		killedGrassHopper = false;
		killsGrassHopper = 15;
		newDataGrassHopper = false;
		killedAcidFlyer = false;
		killsAcidFlyer = 8;
		newDataAcidFlyer = false;
		killedAcidWalker = false;
		killsAcidWalker = 8;
		newDataAcidWalker = false;
		killedMossFlyer = false;
		killsMossFlyer = 25;
		newDataMossFlyer = false;
		killedMossKnightFat = false;
		killsMossKnightFat = 10;
		newDataMossKnightFat = false;
		killedMossWalker = false;
		killsMossWalker = 30;
		newDataMossWalker = false;
		killedInfectedKnight = false;
		killsInfectedKnight = 1;
		newDataInfectedKnight = false;
		killedLazyFlyer = false;
		killsLazyFlyer = 1;
		newDataLazyFlyer = false;
		killedZapBug = false;
		killsZapBug = 1;
		newDataZapBug = false;
		killedJellyfish = false;
		killsJellyfish = 12;
		newDataJellyfish = false;
		killedJellyCrawler = false;
		killsJellyCrawler = 20;
		newDataJellyCrawler = false;
		killedMegaJellyfish = false;
		killsMegaJellyfish = 1;
		newDataMegaJellyfish = false;
		killedFungoonBaby = false;
		killsFungoonBaby = 30;
		newDataFungoonBaby = false;
		killedMushroomTurret = false;
		killsMushroomTurret = 20;
		newDataMushroomTurret = false;
		killedMantis = false;
		killsMantis = 25;
		newDataMantis = false;
		killedMushroomRoller = false;
		killsMushroomRoller = 20;
		newDataMushroomRoller = false;
		killedMushroomBrawler = false;
		killsMushroomBrawler = 8;
		newDataMushroomBrawler = false;
		killedMushroomBaby = false;
		killsMushroomBaby = 20;
		newDataMushroomBaby = false;
		killedMantisFlyerChild = false;
		killsMantisFlyerChild = 25;
		newDataMantisFlyerChild = false;
		killedFungusFlyer = false;
		killsFungusFlyer = 20;
		newDataFungusFlyer = false;
		killedFungCrawler = false;
		killsFungCrawler = 15;
		newDataFungCrawler = false;
		killedMantisLord = false;
		killsMantisLord = 1;
		newDataMantisLord = false;
		killedBlackKnight = false;
		killsBlackKnight = 10;
		newDataBlackKnight = false;
		killedElectricMage = false;
		killsElectricMage = 6;
		newDataElectricMage = false;
		killedMage = false;
		killsMage = 20;
		newDataMage = false;
		killedMageKnight = false;
		killsMageKnight = 2;
		newDataMageKnight = false;
		killedRoyalDandy = false;
		killsRoyalDandy = 25;
		newDataRoyalDandy = false;
		killedRoyalCoward = false;
		killsRoyalCoward = 25;
		newDataRoyalCoward = false;
		killedRoyalPlumper = false;
		killsRoyalPlumper = 25;
		newDataRoyalPlumper = false;
		killedFlyingSentrySword = false;
		killsFlyingSentrySword = 30;
		newDataFlyingSentrySword = false;
		killedFlyingSentryJavelin = false;
		killsFlyingSentryJavelin = 25;
		newDataFlyingSentryJavelin = false;
		killedSentry = false;
		killsSentry = 25;
		newDataSentry = false;
		killedSentryFat = false;
		killsSentryFat = 20;
		newDataSentryFat = false;
		killedMageBlob = false;
		killsMageBlob = 25;
		newDataMageBlob = false;
		killedGreatShieldZombie = false;
		killsGreatShieldZombie = 10;
		newDataGreatShieldZombie = false;
		killedJarCollector = false;
		killsJarCollector = 1;
		newDataJarCollector = false;
		killedMageBalloon = false;
		killsMageBalloon = 15;
		newDataMageBalloon = false;
		killedMageLord = false;
		killsMageLord = 1;
		newDataMageLord = false;
		killedGorgeousHusk = false;
		killsGorgeousHusk = 1;
		newDataGorgeousHusk = false;
		killedFlipHopper = false;
		killsFlipHopper = 20;
		newDataFlipHopper = false;
		killedFlukeman = false;
		killsFlukeman = 20;
		newDataFlukeman = false;
		killedInflater = false;
		killsInflater = 20;
		newDataInflater = false;
		killedFlukefly = false;
		killsFlukefly = 15;
		newDataFlukefly = false;
		killedFlukeMother = false;
		killsFlukeMother = 1;
		newDataFlukeMother = false;
		killedDungDefender = false;
		killsDungDefender = 1;
		newDataDungDefender = false;
		killedCrystalCrawler = false;
		killsCrystalCrawler = 15;
		newDataCrystalCrawler = false;
		killedCrystalFlyer = false;
		killsCrystalFlyer = 20;
		newDataCrystalFlyer = false;
		killedLaserBug = false;
		killsLaserBug = 15;
		newDataLaserBug = false;
		killedBeamMiner = false;
		killsBeamMiner = 15;
		newDataBeamMiner = false;
		killedZombieMiner = false;
		killsZombieMiner = 20;
		newDataZombieMiner = false;
		killedMegaBeamMiner = false;
		killsMegaBeamMiner = 2;
		newDataMegaBeamMiner = false;
		killedMinesCrawler = false;
		killsMinesCrawler = 15;
		newDataMinesCrawler = false;
		killedAngryBuzzer = false;
		killsAngryBuzzer = 15;
		newDataAngryBuzzer = false;
		killedBurstingBouncer = false;
		killsBurstingBouncer = 15;
		newDataBurstingBouncer = false;
		killedBurstingZombie = false;
		killsBurstingZombie = 15;
		newDataBurstingZombie = false;
		killedSpittingZombie = false;
		killsSpittingZombie = 15;
		newDataSpittingZombie = false;
		killedBabyCentipede = false;
		killsBabyCentipede = 35;
		newDataBabyCentipede = false;
		killedBigCentipede = false;
		killsBigCentipede = 10;
		newDataBigCentipede = false;
		killedCentipedeHatcher = false;
		killsCentipedeHatcher = 15;
		newDataCentipedeHatcher = false;
		killedLesserMawlek = false;
		killsLesserMawlek = 10;
		newDataLesserMawlek = false;
		killedSlashSpider = false;
		killsSlashSpider = 15;
		newDataSlashSpider = false;
		killedSpiderCorpse = false;
		killsSpiderCorpse = 15;
		newDataSpiderCorpse = false;
		killedShootSpider = false;
		killsShootSpider = 20;
		newDataShootSpider = false;
		killedMiniSpider = false;
		killsMiniSpider = 25;
		newDataMiniSpider = false;
		killedSpiderFlyer = false;
		killsSpiderFlyer = 20;
		newDataSpiderFlyer = false;
		killedMimicSpider = false;
		killsMimicSpider = 1;
		newDataMimicSpider = false;
		killedBeeHatchling = false;
		killsBeeHatchling = 30;
		newDataBeeHatchling = false;
		killedBeeStinger = false;
		killsBeeStinger = 15;
		newDataBeeStinger = false;
		killedBigBee = false;
		killsBigBee = 12;
		newDataBigBee = false;
		killedHiveKnight = false;
		killsHiveKnight = 1;
		newDataHiveKnight = false;
		killedBlowFly = false;
		killsBlowFly = 20;
		newDataBlowFly = false;
		killedCeilingDropper = false;
		killsCeilingDropper = 15;
		newDataCeilingDropper = false;
		killedGiantHopper = false;
		killsGiantHopper = 10;
		newDataGiantHopper = false;
		killedGrubMimic = false;
		killsGrubMimic = 5;
		newDataGrubMimic = false;
		killedMawlekTurret = false;
		killsMawlekTurret = 10;
		newDataMawlekTurret = false;
		killedOrangeScuttler = false;
		killsOrangeScuttler = 20;
		newDataOrangeScuttler = false;
		killedHealthScuttler = false;
		killsHealthScuttler = 10;
		newDataHealthScuttler = false;
		killedPigeon = false;
		killsPigeon = 15;
		newDataPigeon = false;
		killedZombieHive = false;
		killsZombieHive = 10;
		newDataZombieHive = false;
		killedDreamGuard = false;
		killsDreamGuard = 20;
		newDataDreamGuard = false;
		killedHornet = false;
		killsHornet = 2;
		newDataHornet = false;
		killedAbyssCrawler = false;
		killsAbyssCrawler = 20;
		newDataAbyssCrawler = false;
		killedSuperSpitter = false;
		killsSuperSpitter = 25;
		newDataSuperSpitter = false;
		killedSibling = false;
		killsSibling = 25;
		newDataSibling = false;
		killedPalaceFly = false;
		killsPalaceFly = 10;
		newDataPalaceFly = false;
		killedEggSac = false;
		killsEggSac = 5;
		newDataEggSac = false;
		killedMummy = false;
		killsMummy = 10;
		newDataMummy = false;
		killedOrangeBalloon = false;
		killsOrangeBalloon = 10;
		newDataOrangeBalloon = false;
		killedAbyssTendril = false;
		killsAbyssTendril = 10;
		newDataAbyssTendril = false;
		killedHeavyMantis = false;
		killsHeavyMantis = 15;
		newDataHeavyMantis = false;
		killedTraitorLord = false;
		killsTraitorLord = 1;
		newDataTraitorLord = false;
		killedMantisHeavyFlyer = false;
		killsMantisHeavyFlyer = 16;
		newDataMantisHeavyFlyer = false;
		killedGardenZombie = false;
		killsGardenZombie = 20;
		newDataGardenZombie = false;
		killedRoyalGuard = false;
		killsRoyalGuard = 2;
		newDataRoyalGuard = false;
		killedWhiteRoyal = false;
		killsWhiteRoyal = 10;
		newDataWhiteRoyal = false;
		killedOblobble = false;
		killsOblobble = 3;
		newDataOblobble = false;
		killedZote = false;
		killsZote = 1;
		newDataZote = false;
		killedBlobble = false;
		killsBlobble = 15;
		newDataBlobble = false;
		killedColMosquito = false;
		killsColMosquito = 15;
		newDataColMosquito = false;
		killedColRoller = false;
		killsColRoller = 20;
		newDataColRoller = false;
		killedColFlyingSentry = false;
		killsColFlyingSentry = 25;
		newDataColFlyingSentry = false;
		killedColMiner = false;
		killsColMiner = 25;
		newDataColMiner = false;
		killedColShield = false;
		killsColShield = 25;
		newDataColShield = false;
		killedColWorm = false;
		killsColWorm = 20;
		newDataColWorm = false;
		killedColHopper = false;
		killsColHopper = 15;
		newDataColHopper = false;
		killedLobsterLancer = false;
		killsLobsterLancer = 1;
		newDataLobsterLancer = false;
		killedGhostAladar = false;
		killsGhostAladar = 1;
		newDataGhostAladar = false;
		killedGhostXero = false;
		killsGhostXero = 1;
		newDataGhostXero = false;
		killedGhostHu = false;
		killsGhostHu = 1;
		newDataGhostHu = false;
		killedGhostMarmu = false;
		killsGhostMarmu = 1;
		newDataGhostMarmu = false;
		killedGhostNoEyes = false;
		killsGhostNoEyes = 1;
		newDataGhostNoEyes = false;
		killedGhostMarkoth = false;
		killsGhostMarkoth = 1;
		newDataGhostMarkoth = false;
		killedGhostGalien = false;
		killsGhostGalien = 1;
		newDataGhostGalien = false;
		killedWhiteDefender = false;
		killsWhiteDefender = 1;
		newDataWhiteDefender = false;
		killedGreyPrince = false;
		killsGreyPrince = 1;
		newDataGreyPrince = false;
		killedZotelingBalloon = false;
		killsZotelingBalloon = 1;
		newDataZotelingBalloon = false;
		killedZotelingHopper = false;
		killsZotelingHopper = 1;
		newDataZotelingHopper = false;
		killedZotelingBuzzer = false;
		killsZotelingBuzzer = 1;
		newDataZotelingBuzzer = false;
		killedHollowKnight = false;
		killsHollowKnight = 1;
		newDataHollowKnight = false;
		killedFinalBoss = false;
		killsFinalBoss = 1;
		newDataFinalBoss = false;
		killedHunterMark = false;
		killsHunterMark = 1;
		newDataHunterMark = false;
		killedFlameBearerSmall = false;
		killsFlameBearerSmall = 3;
		newDataFlameBearerSmall = false;
		killedFlameBearerMed = false;
		killsFlameBearerMed = 4;
		newDataFlameBearerMed = false;
		killedFlameBearerLarge = false;
		killsFlameBearerLarge = 5;
		newDataFlameBearerLarge = false;
		killedGrimm = false;
		killsGrimm = 1;
		newDataGrimm = false;
		killedNightmareGrimm = false;
		killsNightmareGrimm = 1;
		newDataNightmareGrimm = false;
		killedBindingSeal = false;
		killsBindingSeal = 1;
		newDataBindingSeal = false;
		killedFatFluke = false;
		killsFatFluke = 8;
		newDataFatFluke = false;
		killedPaleLurker = false;
		killsPaleLurker = 1;
		newDataPaleLurker = false;
		killedNailBros = false;
		killsNailBros = 1;
		newDataNailBros = false;
		killedPaintmaster = false;
		killsPaintmaster = 1;
		newDataPaintmaster = false;
		killedNailsage = false;
		killsNailsage = 1;
		newDataNailsage = false;
		killedHollowKnightPrime = false;
		killsHollowKnightPrime = 1;
		newDataHollowKnightPrime = false;
		killedGodseekerMask = false;
		killsGodseekerMask = 1;
		newDataGodseekerMask = false;
		killedVoidIdol_1 = false;
		killsVoidIdol_1 = 1;
		newDataVoidIdol_1 = false;
		killedVoidIdol_2 = false;
		killsVoidIdol_2 = 1;
		newDataVoidIdol_2 = false;
		killedVoidIdol_3 = false;
		killsVoidIdol_3 = 1;
		newDataVoidIdol_3 = false;
		grubsCollected = 0;
		grubRewards = 0;
		finalGrubRewardCollected = false;
		fatGrubKing = false;
		falseKnightDefeated = false;
		falseKnightDreamDefeated = false;
		falseKnightOrbsCollected = false;
		mawlekDefeated = false;
		giantBuzzerDefeated = false;
		giantFlyDefeated = false;
		blocker1Defeated = false;
		blocker2Defeated = false;
		hornet1Defeated = false;
		collectorDefeated = false;
		hornetOutskirtsDefeated = false;
		mageLordDreamDefeated = false;
		mageLordOrbsCollected = false;
		infectedKnightDreamDefeated = false;
		infectedKnightOrbsCollected = false;
		whiteDefenderDefeated = false;
		whiteDefenderOrbsCollected = false;
		whiteDefenderDefeats = 0;
		greyPrinceDefeats = 0;
		greyPrinceDefeated = false;
		greyPrinceOrbsCollected = false;
		aladarSlugDefeated = 0;
		xeroDefeated = 0;
		elderHuDefeated = 0;
		mumCaterpillarDefeated = 0;
		noEyesDefeated = 0;
		markothDefeated = 0;
		galienDefeated = 0;
		XERO_encountered = false;
		ALADAR_encountered = false;
		HU_encountered = false;
		MUMCAT_encountered = false;
		NOEYES_encountered = false;
		MARKOTH_encountered = false;
		GALIEN_encountered = false;
		xeroPinned = false;
		aladarPinned = false;
		huPinned = false;
		mumCaterpillarPinned = false;
		noEyesPinned = false;
		markothPinned = false;
		galienPinned = false;
		currentInvPane = 0;
		showGeoUI = false;
		showHealthUI = false;
		promptFocus = false;
		seenFocusTablet = false;
		seenDreamNailPrompt = false;
		isFirstGame = true;
		enteredTutorialFirstTime = false;
		isInvincible = false;
		infiniteAirJump = false;
		invinciTest = false;
		hazardRespawnLocation = Vector3.zero;
		currentArea = 0;
		visitedDirtmouth = false;
		visitedCrossroads = false;
		visitedGreenpath = false;
		visitedFungus = false;
		visitedHive = false;
		visitedCrossroadsInfected = false;
		visitedRuins = false;
		visitedMines = false;
		visitedRoyalGardens = false;
		visitedFogCanyon = false;
		visitedDeepnest = false;
		visitedRestingGrounds = false;
		visitedWaterways = false;
		visitedAbyss = false;
		visitedOutskirts = false;
		visitedWhitePalace = false;
		visitedCliffs = false;
		visitedAbyssLower = false;
		visitedGodhome = false;
		visitedMines10 = false;
		scenesVisited = new List<string>();
		scenesMapped = new List<string>();
		scenesMapped.Add("Cinematic_Stag_travel");
		scenesMapped.Add("Room_Town_Stag_Station");
		scenesMapped.Add("Room_Charm_Shop");
		scenesMapped.Add("Room_Mender_House");
		scenesMapped.Add("Room_mapper");
		scenesMapped.Add("Room_nailmaster");
		scenesMapped.Add("Room_nailmaster_02");
		scenesMapped.Add("Room_nailmaster_03");
		scenesMapped.Add("Room_shop");
		scenesMapped.Add("Room_nailsmith");
		scenesMapped.Add("Room_temple");
		scenesMapped.Add("Room_ruinhouse");
		scenesMapped.Add("Room_Mansion");
		scenesMapped.Add("Room_Tram");
		scenesMapped.Add("Room_Tram_RG");
		scenesMapped.Add("Room_Bretta");
		scenesMapped.Add("Room_Fungus_Shaman");
		scenesMapped.Add("Room_Ouiji");
		scenesMapped.Add("Room_Jinn");
		scenesMapped.Add("Room_Colosseum_01");
		scenesMapped.Add("Room_Colosseum_02");
		scenesMapped.Add("Room_Colosseum_03");
		scenesMapped.Add("Room_Colosseum_Bronze");
		scenesMapped.Add("Room_Colosseum_Silver");
		scenesMapped.Add("Room_Colosseum_Gold");
		scenesMapped.Add("Room_Slug_Shrine");
		scenesMapped.Add("Crossroads_ShamanTemple");
		scenesMapped.Add("Ruins_House_01");
		scenesMapped.Add("Ruins_House_02");
		scenesMapped.Add("Ruins_House_03");
		scenesMapped.Add("Fungus1_35");
		scenesMapped.Add("Fungus1_36");
		scenesMapped.Add("Fungus3_archive");
		scenesMapped.Add("Fungus3_archive_02");
		scenesMapped.Add("Cliffs_03");
		scenesMapped.Add("RestingGrounds_07");
		scenesMapped.Add("Deepnest_45_v02");
		scenesMapped.Add("Deepnest_Spider_Town");
		scenesMapped.Add("Room_spider_small");
		scenesMapped.Add("Room_Wyrm");
		scenesMapped.Add("Abyss_Lighthouse_room");
		scenesMapped.Add("Room_Queen");
		scenesMapped.Add("White_Palace_01");
		scenesMapped.Add("White_Palace_02");
		scenesMapped.Add("White_Palace_03_hub");
		scenesMapped.Add("White_Palace_04");
		scenesMapped.Add("White_Palace_05");
		scenesMapped.Add("White_Palace_06");
		scenesMapped.Add("White_Palace_07");
		scenesMapped.Add("White_Palace_08");
		scenesMapped.Add("White_Palace_09");
		scenesMapped.Add("White_Palace_11");
		scenesMapped.Add("White_Palace_12");
		scenesMapped.Add("White_Palace_13");
		scenesMapped.Add("White_Palace_14");
		scenesMapped.Add("White_Palace_15");
		scenesMapped.Add("White_Palace_16");
		scenesMapped.Add("Dream_Nailcollection");
		scenesMapped.Add("Dream_01_False_Knight");
		scenesMapped.Add("Dream_03_Infected_Knight");
		scenesMapped.Add("Dream_02_Mage_Lord");
		scenesMapped.Add("Dream_Guardian");
		scenesMapped.Add("Dream_Guardian_Hegemol");
		scenesMapped.Add("Dream_Guardian_Lurien");
		scenesMapped.Add("Dream_Guardian_Monomon");
		scenesMapped.Add("Cutscene_Boss_Door");
		scenesMapped.Add("Dream_Backer_Shrine");
		scenesMapped.Add("Dream_Room_Believer_Shrine");
		scenesMapped.Add("Dream_Abyss");
		scenesMapped.Add("Dream_Final_Boss");
		scenesMapped.Add("Room_Final_Boss_Atrium");
		scenesMapped.Add("Room_Final_Boss_Core");
		scenesMapped.Add("Cinematic_Ending_A");
		scenesMapped.Add("Cinematic_Ending_B");
		scenesMapped.Add("Cinematic_Ending_C");
		scenesMapped.Add("Cinematic_Ending_D");
		scenesMapped.Add("Cinematic_Ending_E");
		scenesMapped.Add("End_Credits");
		scenesMapped.Add("Cinematic_MrMushroom");
		scenesMapped.Add("End_Game_Completion");
		scenesMapped.Add("PermaDeath");
		scenesMapped.Add("PermaDeath_Unlock");
		scenesMapped.Add("Deepnest_East_17");
		scenesEncounteredBench = new List<string>();
		scenesEncounteredCocoon = new List<string>();
		scenesGrubRescued = new List<string>();
		scenesFlameCollected = new List<string>();
		scenesEncounteredDreamPlant = new List<string>();
		scenesEncounteredDreamPlantC = new List<string>();
		hasMap = false;
		mapAllRooms = false;
		atMapPrompt = false;
		mapDirtmouth = true;
		mapCrossroads = false;
		mapGreenpath = false;
		mapFogCanyon = false;
		mapRoyalGardens = false;
		mapFungalWastes = false;
		mapCity = false;
		mapWaterways = false;
		mapMines = false;
		mapDeepnest = false;
		mapCliffs = false;
		mapOutskirts = false;
		mapRestingGrounds = false;
		mapAbyss = false;
		hasPin = false;
		hasPinBench = false;
		hasPinCocoon = false;
		hasPinDreamPlant = false;
		hasPinGuardian = false;
		hasPinBlackEgg = false;
		hasPinShop = false;
		hasPinSpa = false;
		hasPinStag = false;
		hasPinTram = false;
		hasPinGhost = false;
		hasPinGrub = false;
		hasMarker = false;
		hasMarker_r = false;
		hasMarker_b = false;
		hasMarker_y = false;
		hasMarker_w = false;
		spareMarkers_r = 6;
		spareMarkers_b = 6;
		spareMarkers_y = 6;
		spareMarkers_w = 6;
		placedMarkers_r = new List<Vector3>();
		placedMarkers_b = new List<Vector3>();
		placedMarkers_y = new List<Vector3>();
		placedMarkers_w = new List<Vector3>();
		environmentType = 0;
		previousDarkness = 0;
		openedTramLower = false;
		openedTramRestingGrounds = false;
		tramLowerPosition = 0;
		tramRestingGroundsPosition = 0;
		mineLiftOpened = false;
		menderDoorOpened = false;
		vesselFragStagNest = false;
		shamanPillar = false;
		crossroadsMawlekWall = false;
		eggTempleVisited = false;
		crossroadsInfected = false;
		falseKnightFirstPlop = false;
		falseKnightWallRepaired = false;
		falseKnightWallBroken = false;
		falseKnightGhostDeparted = false;
		spaBugsEncountered = false;
		hornheadVinePlat = false;
		infectedKnightEncountered = false;
		megaMossChargerEncountered = false;
		megaMossChargerDefeated = false;
		dreamerScene1 = false;
		slugEncounterComplete = false;
		defeatedDoubleBlockers = false;
		oneWayArchive = false;
		defeatedMegaJelly = false;
		summonedMonomon = false;
		sawWoundedQuirrel = false;
		encounteredMegaJelly = false;
		defeatedMantisLords = false;
		encounteredGatekeeper = false;
		deepnestWall = false;
		queensStationNonDisplay = false;
		cityBridge1 = false;
		cityBridge2 = false;
		cityLift1 = false;
		cityLift1_isUp = false;
		liftArrival = false;
		openedMageDoor_v2 = false;
		brokenMageWindow = false;
		brokenMageWindowGlass = false;
		mageLordEncountered = false;
		mageLordEncountered_2 = false;
		mageLordDefeated = false;
		ruins1_5_tripleDoor = false;
		openedWaterwaysManhole = false;
		openedCityGate = false;
		cityGateClosed = false;
		bathHouseOpened = false;
		bathHouseWall = false;
		cityLift2 = true;
		cityLift2_isUp = false;
		city2_sewerDoor = false;
		openedLoveDoor = false;
		watcherChandelier = false;
		completedQuakeArea = false;
		kingsStationNonDisplay = false;
		tollBenchCity = false;
		waterwaysGate = false;
		defeatedDungDefender = false;
		dungDefenderEncounterReady = false;
		flukeMotherEncountered = false;
		flukeMotherDefeated = false;
		waterwaysAcidDrained = false;
		dungDefenderWallBroken = false;
		dungDefenderSleeping = false;
		defeatedMegaBeamMiner = false;
		defeatedMegaBeamMiner2 = false;
		brokeMinersWall = false;
		encounteredMimicSpider = false;
		steppedBeyondBridge = false;
		deepnestBridgeCollapsed = false;
		spiderCapture = false;
		deepnest26b_switch = false;
		openedRestingGrounds02 = false;
		restingGroundsCryptWall = false;
		dreamNailConvo = false;
		gladeGhostsKilled = 0;
		openedGardensStagStation = false;
		extendedGramophone = false;
		tollBenchQueensGardens = false;
		blizzardEnded = false;
		encounteredHornet = false;
		savedByHornet = false;
		outskirtsWall = false;
		abyssGateOpened = false;
		abyssLighthouse = false;
		blueVineDoor = false;
		gotShadeCharm = false;
		tollBenchAbyss = false;
		fountainGeo = 0;
		fountainVesselSummoned = false;
		openedBlackEggPath = false;
		enteredDreamWorld = false;
		duskKnightDefeated = false;
		whitePalaceOrb_1 = false;
		whitePalaceOrb_2 = false;
		whitePalaceOrb_3 = false;
		whitePalace05_lever = false;
		whitePalaceMidWarp = false;
		whitePalaceSecretRoomVisited = false;
		tramOpenedDeepnest = false;
		tramOpenedCrossroads = false;
		openedBlackEggDoor = false;
		unchainedHollowKnight = false;
		flamesCollected = 0;
		flamesRequired = 3;
		nightmareLanternAppeared = false;
		nightmareLanternLit = false;
		troupeInTown = false;
		divineInTown = false;
		grimmChildLevel = 1;
		elderbugConvoGrimm = false;
		slyConvoGrimm = false;
		iseldaConvoGrimm = false;
		midwifeWeaverlingConvo = false;
		metGrimm = false;
		foughtGrimm = false;
		metBrum = false;
		defeatedNightmareGrimm = false;
		grimmchildAwoken = false;
		gotBrummsFlame = false;
		brummBrokeBrazier = false;
		destroyedNightmareLantern = false;
		gotGrimmNotch = false;
		nymmInTown = false;
		nymmSpoken = false;
		elderbugNymmConvo = false;
		slyNymmConvo = false;
		iseldaNymmConvo = false;
		elderbugTroupeLeftConvo = false;
		jijiGrimmConvo = false;
		nymmMissedEggOpen = false;
		elderbugBrettaLeft = false;
		metDivine = false;
		divineFinalConvo = false;
		gaveFragileHeart = false;
		gaveFragileGreed = false;
		gaveFragileStrength = false;
		pooedFragileHeart = false;
		pooedFragileGreed = false;
		pooedFragileStrength = false;
		divineEatenConvos = 0;
		completionPercentage = 0f;
		unlockedCompletionRate = false;
		disablePause = false;
		backerCredits = false;
		mapKeyPref = 0;
		playerStory = new List<string>();
		betaEnd = false;
		bossReturnEntryGate = "";
		bossDoorStateTier1 = BossSequenceDoor.Completion.None;
		bossDoorStateTier2 = BossSequenceDoor.Completion.None;
		bossDoorStateTier3 = BossSequenceDoor.Completion.None;
		bossDoorStateTier4 = BossSequenceDoor.Completion.None;
		bossDoorStateTier5 = BossSequenceDoor.Completion.None;
		bossStatueTargetLevel = -1;
		currentBossStatueCompletionKey = "";
		statueStateGruzMother = BossStatue.Completion.None;
		statueStateVengefly = BossStatue.Completion.None;
		statueStateBroodingMawlek = BossStatue.Completion.None;
		statueStateFalseKnight = BossStatue.Completion.None;
		statueStateFailedChampion = BossStatue.Completion.None;
		statueStateHornet1 = BossStatue.Completion.None;
		statueStateHornet2 = BossStatue.Completion.None;
		statueStateMegaMossCharger = BossStatue.Completion.None;
		statueStateMantisLords = BossStatue.Completion.None;
		statueStateOblobbles = BossStatue.Completion.None;
		statueStateGreyPrince = BossStatue.Completion.None;
		statueStateBrokenVessel = BossStatue.Completion.None;
		statueStateLostKin = BossStatue.Completion.None;
		statueStateNosk = BossStatue.Completion.None;
		statueStateFlukemarm = BossStatue.Completion.None;
		statueStateCollector = BossStatue.Completion.None;
		statueStateWatcherKnights = BossStatue.Completion.None;
		statueStateSoulMaster = BossStatue.Completion.None;
		statueStateSoulTyrant = BossStatue.Completion.None;
		statueStateGodTamer = BossStatue.Completion.None;
		statueStateCrystalGuardian1 = BossStatue.Completion.None;
		statueStateCrystalGuardian2 = BossStatue.Completion.None;
		statueStateUumuu = BossStatue.Completion.None;
		statueStateDungDefender = BossStatue.Completion.None;
		statueStateWhiteDefender = BossStatue.Completion.None;
		statueStateHiveKnight = BossStatue.Completion.None;
		statueStateTraitorLord = BossStatue.Completion.None;
		statueStateGrimm = BossStatue.Completion.None;
		statueStateNightmareGrimm = BossStatue.Completion.None;
		statueStateHollowKnight = BossStatue.Completion.None;
		statueStateElderHu = BossStatue.Completion.None;
		statueStateGalien = BossStatue.Completion.None;
		statueStateMarkoth = BossStatue.Completion.None;
		statueStateMarmu = BossStatue.Completion.None;
		statueStateNoEyes = BossStatue.Completion.None;
		statueStateXero = BossStatue.Completion.None;
		statueStateGorb = BossStatue.Completion.None;
		statueStateRadiance = new BossStatue.Completion
		{
			hasBeenSeen = true
		};
		statueStateSly = BossStatue.Completion.None;
		statueStateNailmasters = BossStatue.Completion.None;
		statueStateMageKnight = BossStatue.Completion.None;
		statueStatePaintmaster = BossStatue.Completion.None;
		statueStateZote = BossStatue.Completion.None;
		statueStateNoskHornet = BossStatue.Completion.None;
		statueStateMantisLordsExtra = BossStatue.Completion.None;
		godseekerUnlocked = false;
		currentBossSequence = null;
		bossRushMode = false;
		bossDoorCageUnlocked = false;
		blueRoomDoorUnlocked = false;
		blueRoomActivated = false;
		finalBossDoorUnlocked = false;
		hasGodfinder = false;
		unlockedNewBossStatue = true;
		scaredFlukeHermitEncountered = false;
		scaredFlukeHermitReturned = false;
		enteredGGAtrium = false;
		extraFlowerAppear = false;
		givenGodseekerFlower = false;
		givenOroFlower = false;
		givenWhiteLadyFlower = false;
		givenEmilitiaFlower = false;
		unlockedBossScenes = new List<string>();
		queuedGodfinderIcon = false;
		godseekerSpokenAwake = false;
		nailsmithCorpseAppeared = false;
		godseekerWaterwaysSeenState = -1;
		godseekerWaterwaysSpoken1 = false;
		godseekerWaterwaysSpoken2 = false;
		godseekerWaterwaysSpoken3 = false;
		bossDoorEntranceTextSeen = -1;
		seenDoor4Finale = false;
		zoteStatueWallBroken = false;
		seenGGWastes = false;
		ordealAchieved = false;
	}

	public void AddGGPlayerDataOverrides()
	{
		bossRushMode = true;
		atBench = false;
		respawnScene = "GG_Entrance_Cutscene";
		respawnMarkerName = "Death Respawn Marker";
		mapZone = MapZone.ABYSS;
		respawnType = 0;
		respawnFacingRight = true;
		hazardRespawnFacingRight = true;
		maxHealthBase = maxHealthCap;
		MPReserveMax = MPReserveCap;
		heartPieceMax = true;
		nailDamage = 21;
		honedNail = true;
		nailSmithUpgrades = 4;
		canDash = true;
		canShadowDash = true;
		hasDash = true;
		hasShadowDash = true;
		hasWalljump = true;
		hasDoubleJump = true;
		hasSuperDash = true;
		hasDreamNail = true;
		hasDreamGate = true;
		dreamOrbs = 1;
		hasNailArt = true;
		hasDashSlash = true;
		hasCyclone = true;
		hasUpwardSlash = true;
		hasSpell = true;
		shadeFireballLevel = 2;
		shadeQuakeLevel = 2;
		shadeScreamLevel = 2;
		fireballLevel = 2;
		quakeLevel = 2;
		screamLevel = 2;
		grimmChildLevel = 4;
		hasAcidArmour = true;
		hasLantern = true;
		hasCharm = true;
		charmSlots = 11;
		equippedCharm_36 = true;
		if (!equippedCharms.Contains(36))
		{
			equippedCharms.Add(36);
		}
		charmCost_36 = 0;
		fragileGreed_unbreakable = true;
		fragileHealth_unbreakable = true;
		fragileStrength_unbreakable = true;
		gotShadeCharm = true;
		hasGodfinder = true;
		greyPrinceDefeats = 1;
		greyPrinceDefeated = true;
		seenDoor4Finale = true;
		FieldInfo[] fields = GetType().GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			if (fieldInfo.FieldType == typeof(BossSequenceDoor.Completion))
			{
				BossSequenceDoor.Completion completion = (BossSequenceDoor.Completion)fieldInfo.GetValue(this);
				completion.unlocked = true;
				fieldInfo.SetValue(this, completion);
				continue;
			}
			if (fieldInfo.FieldType == typeof(BossStatue.Completion))
			{
				if (!fieldInfo.Name.IsAny("statueStateRadiance"))
				{
					BossStatue.Completion completion2 = (BossStatue.Completion)fieldInfo.GetValue(this);
					completion2.isUnlocked = true;
					completion2.hasBeenSeen = true;
					fieldInfo.SetValue(this, completion2);
				}
				continue;
			}
			string text = fieldInfo.Name.Split('_')[0];
			if (text == "gotCharm")
			{
				fieldInfo.SetValue(this, true);
			}
			else if (text == "newCharm")
			{
				fieldInfo.SetValue(this, false);
			}
		}
	}
}
