using System;

namespace _8th_Circle_Server
{
    // All rooms have a unique room identifier for easy access.
    public enum RoomID
    {
        ROOMID_START,

        // Geraldine Manor
        GERALD_1ST_ENT = ROOMID_START,
        GERALD_1ST_HALLWAY,
        GERALD_1ST_KITCHEN,
        GERALD_1ST_BATHROOM,
        GERALD_1ST_DININGROOM,
        GERALD_1ST_LIVINGROOM,
        GERALD_2ND_HALLWAY,
        GERALD_2ND_BATHROOM,
        GERALD_2ND_KITTYROOM,
        GERALD_2ND_KITTYCLOSET,
        GERALD_2ND_BLUEROOM,
        GERALD_2ND_BEDROOM,
        GERALD_BASE_PART1,
        GERALD_BASE_PART2,
        GERALD_BASE_PART3,
        GERALD_BASE_PART4,
        GERALD_BASE_PART5,
        GERALD_BASE_LAUNDRYROOM,
        GERALD_BASE_CLOSET,
        GERALD_BASE_SUMPROOM,
        GERALD_BASE_BATHROOM,

        // Goblin Prooving Grounds
        GPG_PLAYER_START,
        GPG_ROOM_1,
        GPG_ROOM_2,
        GPG_ROOM_3,
        GPG_ROOM_4,
        GPG_ROOM_5,
        GPG_ROOM_6,
        GPG_ROOM_7,
        GPG_ROOM_8,
        GPG_ROOM_9,
        GPG_ROOM_10,
        GPG_ROOM_11,
        GPG_ROOM_12,
        GPG_ROOM_13,
        GPG_ROOM_14,
        GPG_ROOM_15,
        GPG_ROOM_16,
        GPG_ROOM_17,
        GPG_ROOM_18,
        GPG_ROOM_19,
        GPG_ROOM_21,
        GPG_ROOM_22,
        GPG_ROOM_23,
        GPG_ROOM_24,
        GPG_ROOM_25,
        GPG_ROOM_26,
        GPG_ROOM_27,
        GPG_ROOM_28,
        GPG_ROOM_29,
        GPG_ROOM_30,
        GPG_ROOM_31,
        GPG_ROOM_32,
        GPG_ROOM_33,
        GPG_ROOM_34,
        GPG_ROOM_35,
        GPG_ROOM_36,
        GPG_ROOM_37,
        GPG_ROOM_38,
        GPG_ROOM_39,
        GPG_ROOM_40,
        GPG_ROOM_41,
        GPG_ROOM_42,
        GPG_ROOM_43,
        GPG_ROOM_44,
        GPG_ROOM_45,
        GPG_ROOM_46,
        GPG_ROOM_47,
        GPG_ROOM_48,
        GPG_ROOM_49,
        GPG_ROOM_50,
        GPG_ROOM_51,
        GPG_ROOM_52,
        GPG_ROOM_53,
        GPG_ROOM_54,
        GPG_ROOM_55,
        GPG_ROOM_56,
        GPG_ROOM_57,
        GPG_ROOM_58,
        GPG_ROOM_59,
        GPG_ROOM_60,
        GPG_ROOM_61,
        GPG_ROOM_62,
        GPG_ROOM_63,
        GPG_ROOM_64,
        GPG_ROOM_65,
        GPG_ROOM_66,
        GPG_ROOM_67,
        GPG_ROOM_68,
        GPG_ROOM_69,
        GPG_ROOM_70,
        GPG_ROOM_71,
        GPG_ROOM_72,
        GPG_ROOM_73,
        GPG_ROOM_74,
        GPG_ROOM_75,
        GPG_ROOM_76,
        GPG_ROOM_77,
        GPG_ROOM_78,

        // Proto Area
        PROTO_1,
        PROTO_2,
        PROTO_3,
        PROTO_4,
        PROTO_5,
        PROTO_6,
        PROTO_7,
        PROTO_8,
        PROTO_9,
        PROTO_10,
        PROTO_11,
        PROTO_12,
        PROTO_13,
        PROTO_14,
        PROTO_15,
        PROTO_16,
        PROTO_17,
        PROTO_18,
        PROTO_19,
        PROTO_20,
        PROTO_21,
        PROTO_22,
        PROTO_23,
        PROTO_24,
        PROTO_25,
        PROTO_26,
        PROTO_27,

        // Last Room
        ROOMID_END
    }// RoomID

    // MobFlags represent every 'property or attribute' that mobs can have.
    [Flags]
    public enum MobFlags
    {
        NONE = 0,
        OPENABLE = 1,
        OPEN = 2,
        CLOSEABLE = 4,
        LOCKED = 8,
        LOCKABLE = 16,
        UNLOCKABLE = 32,
        HIDDEN = 64,
        INVISIBLE = 128,
        GETTABLE = 256,
        DROPPABLE = 512,
        PUSHABLE = 1024,
        STORABLE = 2048,
        USEABLE = 4096,
        INSPECTABLE = 8192,
        WEARABLE = 16384,
        IDENTIFYABLE = 32768,
        STEALABLE = 65536,
        DUPLICATABLE = 131072,
        SEARCHING = 262144,
        COMBATABLE = 524288,
        INCOMBAT = 1048576,
        RESTING = 2097152,
        END = 4194304
    }// MobFlags

    // The different grammar combinations that is allowed by the parser for commands.
    public enum GrammarType
    {
        GRAMMAR_START,
        GRAMMAR_VERB = GRAMMAR_START,
        GRAMMAR_VERB_PRED,
        GRAMMAR_VERB_PRED_PRED,
        GRAMMAR_VERB_PREP_PRED,
        GRAMMAR_VERB_PRED_PREP_PRED,
        GRAMMAR_END
    }// GrammarType

    // A particular grammar type
    public enum Grammar
    {
        GRAMMAR_START,
        VERB = GRAMMAR_START,
        PREP,
        PREDICATE,
        GRAMMAR_END
    }// Grammar

    // Types of predicates in the game, generally represent mobs
    [Flags]
    public enum PredicateType
    {
        NONE = 0,
        OBJECT = 1,
        PLAYER = 2,
        NPC = 4,
        DOORWAY = 8,
        CUSTOM = 16,
        SPELL = 32,
        END = 64
    }// PredicateType

    // Where the parser should look to find a particular predicate
    [Flags]
    public enum ValidityType
    {
        NONE = 0,
        INVENTORY = 1,
        LOCAL = 2,
        DOORWAY = 4,
        EQUIPMENT = 8,
        AREA = 16,
        GLOBAL = 32,
        END = 64
    }// ValidityType

    // Valid prepisitions for the parser
    public enum PrepositionType
    {
        PREP_START,
        PREP_IN,
        PREP_ON,
        PREP_WITH,
        PREP_AT,
        PREP_FROM,
        PREP_END
    }// PrepositionType

    // Enum representation of commands
    public enum CommandName
    {
        COMMAND_START,
        COMMAND_LOOK = COMMAND_START,
        COMMAND_EXIT,
        COMMAND_NORTH,
        COMMAND_SOUTH,
        COMMAND_EAST,
        COMMAND_WEST,
        COMMAND_UP,
        COMMAND_DOWN,
        COMMAND_NORTHEAST,
        COMMAND_NORTHWEST,
        COMMAND_SOUTHEAST,
        COMMAND_SOUTHWEST,
        COMMAND_SAY,
        COMMAND_YELL,
        COMMAND_TELL,
        COMMAND_OPEN,
        COMMAND_CLOSE,
        COMMAND_GET,
        COMMAND_GETALL,
        COMMAND_INVENTORY,
        COMMAND_EQUIPMENT,
        COMMAND_LOCK,
        COMMAND_UNLOCK,
        COMMAND_DROP,
        COMMAND_DROPALL,
        COMMAND_SPAWN,
        COMMAND_DESTROY,
        COMMAND_USE,
        COMMAND_ATTACK,
        COMMAND_SEARCH,
        COMMAND_WHO,
        COMMAND_FULLHEAL,
        COMMAND_TELEPORT,
        COMMAND_WEAR,
        COMMAND_WEARALL,
        COMMAND_REMOVE,
        COMMAND_REMOVEALL,
        COMMAND_REST,
        COMMAND_BACKSTAB,
        COMMAND_BASH,
        COMMAND_CAST,
        COMMAND_END
    }// CommandName

    // Various error codes returned by commands or the parser
    public enum errorCode
    {
        E_START,
        E_OK = E_START,
        E_INVALID_SYNTAX,
        E_INVALID_COMMAND_USAGE,
        E_END
    }// errorCode

    // Types of abilities and spells
    public enum AbilitySpell
    {
        ABILITY_SPELL_START,

        // Abilities
        ABILITY_BASH = ABILITY_SPELL_START,
        ABILITY_BACKSTAB,

        // Spells
        SPELL_MYSTIC_SHOT,
        SPELL_CURE,

        ABILITY_SPELL_END
    }// AbilitySpell

    // Each area has a unique area id for easy access.
    public enum AreaID
    {
        AID_PROTOAREA,
        AID_START = AID_PROTOAREA,
        AID_GERALDINEMANOR,
        AID_gpgArea,
        AID_END
    }// AreaID

    // Every mob in the game must have a unique itentifier for easy access
    public enum MobList
    {
        MOB_START = -1,
        // Reserved for players
        PLAYER = MOB_START,

        // Objects
        STRANGE_LUMP,
        BASIC_KEY,
        EVENT_CHEST1,
        EVENT_CHEST2,
        FIRST_CIRCLE,
        BASIC_CHEST,
        SWITCH,
        BASIC_SWORD,

        // NPCs
        GOBLIN_RUNT,
        MAX,

        MOB_END
    }// MobList

    // Cardinal directions for movement and doorways
    public enum Direction
    {
        UP,
        DIRECTION_START = UP,
        NORTH,
        NORTHEAST,
        EAST,
        SOUTHEAST,
        DOWN,
        SOUTH,
        SOUTHWEST,
        WEST,
        NORTHWEST,
        DIRECTION_END
    }// Direction

    // Resource types for the resource handler
    public enum ResType
    {
        OBJECT,
        RESOURCE_START = OBJECT,
        NPC,
        PLAYER,
        DOORWAY,
        RESOURCE_END
    }// ResType

    // Each event is mapped to a a unique itentifier for easy access
    public enum EventID
    {
        EVENT_START,
        EVENT_TELL_PLAYER = EVENT_START,
        EVENT_TELEPORT,
        EVENT_GPG_WALL_REMOVE,
        EVENT_GPG_WALL_ADD,
        EVENT_END
    }// EventID

    // Commands with actions have a type to differentiate them
    public enum ActionType
    {
        ACTIONTYPE_START,
        ABILITY,
        SPELL,
        ACTIONTYPE_END
    }// ActionType

    // Damage can scale with multiple formulas
    public enum DamageScaling
    {
        DAMAGESCALING_START,
        PERLEVEL,
        DAMAGEMULT,
        DAMAGEMULTPERLEVEL,
        DAMAGEBONUS,
        DAMAGESCALING_END
    }// DamageScaling

    // All the gear slots for CombatMobs
    public enum EQSlot
    {
        EQSLOT_START,
        HEAD = EQSLOT_START,
        NECK,
        BACK,
        CHEST,
        ARMS,
        HANDS,
        WAIST,
        LEGS,
        FEET,
        RING1,
        RING2,
        PRIMARY,
        SECONDARY,
        EQSLOT_END
    }// EQSlot

    // Various types of gear for the different classes
    public enum EQType
    {
        LIGHT_ARMOR,
        EQTYPE_START = LIGHT_ARMOR,
        MEDIUM_ARMOR,
        HEAVY_ARMOR,
        MAGIC,
        ACCESSORY,
        WEAPON,
        EQTYPE_END
    }// EQType

    // Various types of damage, also maps to their resistances
    public enum DamageType
    {
        PHYSICAL,
        DAMAGETYPE_START = PHYSICAL,
        MAGICAL,
        PURE,
        HEALING,
        DAMAGETYPE_END
    }// DamageType

    // Various types of mobs
    public enum MobType
    {
        NONHEROIC,
        MOBTYPE_START = NONHEROIC,
        WARRIOR,
        ROGUE,
        WIZARD,
        CLERIC,
        SPELLCASTER,
        ALL,
        MOBTYPE_END
    }// MobType

    // All combat stats
    public enum STAT
    {
        STAT_START,

        LEVEL,
        CURRENTHP,
        BASEMAXHP,
        MAXHPMOD,
        BASEMINDAM,
        BASEMAXDAM,
        BASEDAMBONUSMOD,
        DAMBONUSMOD,
        BASEHIT,
        HITMOD,
        BASEEVADE,
        EVADEMOD,
        BASEARMOR,
        ARMORMOD,
        BASEPHYRES,
        PHYRESMOD,
        BASEMAGICRES,
        MAGICRESMOD,
        BASEMAXMANA,
        MAXMANAMOD,
        CURRENTMANA,

        STAT_EMD
    }// STAT

    // Used to operate pre and post executes for CommandClasses
    public enum CmdOps
    {
        OPS_START,

        CHECK_TO_PASS,
        CHECK_TO_FAIL,
        SET,
        UNSET,

        OPS_END
    }// CmdOpsk

}// _8th_Circle_Server
