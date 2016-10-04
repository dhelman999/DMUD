namespace _8th_Circle_Server
{
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

    public enum MobFlags
    {
        FLAG_START,
        FLAG_OPENABLE = FLAG_START,
        FLAG_CLOSEABLE,
        FLAG_LOCKED,
        FLAG_LOCKABLE,
        FLAG_UNLOCKED,
        FLAG_UNLOCKABLE,
        FLAG_HIDDEN,
        FLAG_INVISIBLE,
        FLAG_GETTABLE,
        FLAG_DROPPABLE,
        FLAG_PUSHABLE,
        FLAG_STORABLE,
        FLAG_USEABLE,
        FLAG_INSPECTABLE,
        FLAG_WEARABLE,
        FLAG_IDENTIFYABLE,
        FLAG_STEALABLE,
        FLAG_DUPLICATABLE,
        FLAG_SEARCHING,
        FLAG_COMBATABLE,
        FLAG_INCOMBAT,
        FLAG_RESTING,
        FLAG_END
    }// MobFlags

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

    public enum Grammar
    {
        GRAMMAR_START,
        VERB = GRAMMAR_START,
        PREP,
        PREDICATE,
        GRAMMAR_END
    }// Grammar

    public enum predicateType
    {
        PREDICATE_START,
        PREDICATE_OBJECT = PREDICATE_START,
        PREDICATE_PLAYER,
        PREDICATE_NPC,
        PREDICATE_PLAYER_OR_NPC,
        PREDICATE_OBJECT_OR_PLAYER,
        PREDICATE_OBJECT_OR_NPC,
        PREDICATE_ALL,
        PREDICATE_CUSTOM,
        PREDICATE_SPELL,
        PREDICATE_END
    }// predicateType

    public enum validityType
    {
        VALID_START,
        VALID_INVENTORY = VALID_START,
        VALID_LOCAL,
        VALID_INVLOCAL,
        VALID_EQUIP,
        VALID_AREA,
        VALID_GLOBAL,
        VALID_END
    }// validityType

    public enum PrepositionType
    {
        PREP_START,
        PREP_IN = PREP_START,
        PREP_ON,
        PREP_WITH,
        PREP_AT,
        PREP_FROM,
        PREP_OFF,
        PREP_END
    }// PrepositionType

    public enum commandName
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
    }// commandName

    public enum errorCode
    {
        E_START,
        E_OK = E_START,
        E_INVALID_SYNTAX,
        E_INVALID_COMMAND_USAGE,
        E_END
    }// errorCode

    public enum CommandType
    {
        COMTYPE_START,
        GENERAL,
        ABILITY,
        SPELL,
        COMTYPE_END
    }// CommandType

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

    public enum AreaID
    {
        AID_PROTOAREA,
        AID_START = AID_PROTOAREA,
        AID_GERALDINEMANOR,
        AID_NEWBIEAREA,
        AID_END
    }// AreaID

    public enum MOBLIST
    {
        MOB_START = -1,
        // Reserved for players
        PLAYER = MOB_START,

        // Objects
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
    }// MOBLIST

    public enum MementoType
    {
        CONTAINER,
        MTYPE_START = CONTAINER,
        DOORWAY,
        EQUIPMENT,
        MTYPE_END = EQUIPMENT
    }// Direction

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
        DIRECTION_END = NORTHWEST
    }// Direction

    public enum ResType
    {
        OBJECT,
        RESOURCE_START = OBJECT,
        NPC,
        PLAYER,
        DOORWAY,
        RESOURCE_END
    }// ResType

    public enum EventFlag
    {
        EVENT_START,
        EVENT_TELL_PLAYER = EVENT_START,
        EVENT_TELEPORT,
        EVENT_GPG_WALL_REMOVE,
        EVENT_GPG_WALL_ADD,
        EVENT_END
    }// EventFlag

    public enum ActionType
    {
        ACTIONTYPE_START,
        ABILITY,
        SPELL,
        ACTIONTYPE_END
    }// ActionType

    public enum DamageScaling
    {
        DAMAGESCALING_START,
        PERLEVEL,
        DAMAGEMULT,
        DAMAGEMULTPERLEVEL,
        DAMAGEBONUS,
        DAMAGESCALING_END
    }// DamageScaling

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

    public enum Useable
    {
        ALL,
        USEABLE_START = ALL,
        USEABLE_END
    }// Useable

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

    public enum DamageType
    {
        PHYSICAL,
        DAMAGETYPE_START = PHYSICAL,
        MAGICAL,
        PURE,
        HEALING,
        DAMAGETYPE_END
    }// DamageType

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

}// _8th_Circle_Server
