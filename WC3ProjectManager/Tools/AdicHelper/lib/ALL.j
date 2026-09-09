#guard Anti_Leak_Library
library ALL {
    include "cj_types_priv.j"
    native int GetPlayerUnitTypeCount(player p, int unitid)
    //Убираем конфликты с AntiBJ base
    setdef GetPlayerStartLocationLoc(p) = ALL_RemoveLocation(GetStartLocation##Loc(GetPlayerStartLocation(p)))
    setdef GetUnitsInRangeOfLocAll(r, l) = ALL_DestroyGroup(ALL_GetUnitsInRangeOfLocMatching(r, l, null))
    setdef GetUnitsInRectAll(r) = ALL_DestroyGroup(ALL_GetUnitsInRectMatching(r, null))
    setdef GetUnitsOfPlayerAll(p) = ALL_DestroyGroup(ALL_GetUnitsOfPlayerMatching(p, null))
    setdef LoadLocationHandleBJ(childKey, parentKey, table) = ALL_RemoveLocation(LoadLocation##Handle(table, parentKey, childKey))
    setdef LoadRectHandleBJ(childKey, parentKey, table) = ALL_RemoveRect(LoadRect##Handle(table, parentKey, childKey))
    setdef LoadGroupHandleBJ(childKey, parentKey, table) = ALL_DestroyGroup(LoadGroup##Handle(table, parentKey, childKey))
    setdef LoadForceHandleBJ(childKey, parentKey, table) = ALL_DestroyForce(LoadForce##Handle(table, parentKey, childKey))
    setdef <call AddSpecialEffectLocBJ>(l, s) = call ALL_AddSpecialEffectLocBJ(l, s)
    setdef <call AddSpecialEffectTargetUnitBJ>(a, w, s) = call ALL_AddSpecialEffectTargetUnitBJ(a, w, s)
    //Области, созданные в функции CreateRegions, не будут прогоняться через RemoveRect
    setdef Rect(minX, minY, maxX, maxY) = ALL_RemoveRect(Re##ct(minX, minY, maxX, maxY))
    define {
        <InitCustomTriggers()> = { InitCustom##Triggers(); ALL_Remove = true }
        GetPlayerStartLocationLoc(p) = GetStartLocationLoc(GetPlayerStartLocation(p))
        GetUnitsInRangeOfLocAll(r, l) = GetUnitsInRangeOfLocMatching(r, l, null)
        GetUnitsInRectAll(r) = GetUnitsInRectMatching(r, null)
        GetUnitsOfPlayerAll(p) = GetUnitsOfPlayerMatching(p, null)
        LoadLocationHandleBJ(a, b, h) = LoadLocationHandle(h, b, a)
        LoadRectHandleBJ(a, b, h) = LoadRectHandle(h, b, a)
        LoadGroupHandleBJ(a, b, h) = LoadGroupHandle(h, b, a)
        LoadForceHandleBJ(a, b, h) = LoadForceHandle(h, b, a)
        <call AddSpecialEffectLocBJ>(l, s) = bj_lastCreatedEffect = AddSpecialEffectLoc(s, l)
        <call AddSpecialEffectTargetUnitBJ>(a, w, s) = bj_lastCreatedEffect = AddSpecialEffectTarget(a, w, s)

        <GetCameraEyePositionLoc()> = ALL_RemoveLocation(GetCameraEyePosition##Loc())
        <GetCameraTargetPositionLoc()> = ALL_RemoveLocation(GetCameraTargetPosition##Loc())
        GetDestructableLoc(d) = ALL_RemoveLocation(ALL_GetWidgetLoc(d))
        GetItemLoc(i) = ALL_RemoveLocation(ALL_GetWidgetLoc(i))
        <GetOrderPointLoc()> = ALL_RemoveLocation(GetOrderPoint##Loc())
        GetRandomLocInRect(r) = ALL_RemoveLocation(GetRandom##LocInRect(r))
        GetRectCenter(r) = ALL_RemoveLocation(GetRect##Center(r))
        <GetSpellTargetLoc()> = ALL_RemoveLocation(GetSpellTarget##Loc())
        GetStartLocationLoc(i) = ALL_RemoveLocation(GetStartLocation##Loc(i))
        GetUnitLoc(u) = ALL_RemoveLocation(GetUnit##Loc(u))
        Location(x, y) = ALL_RemoveLocation(Loc##ation(x, y))
        MeleeGetLocWithinRect(l, r) = ALL_RemoveLocation(ALL_MeleeGetLocWithinRect(l, r))
        MeleeGetProjectedLoc(l, targ, dist, a) = ALL_RemoveLocation(MeleeGet##ProjectedLoc(l, targ, dist, a))
        OffsetLocation(l, x, y) = ALL_RemoveLocation(OffsetLoc##ation(l, x, y))
        PolarProjectionBJ(l, r, a) = ALL_RemoveLocation(ALL_PolarProjectionBJ(l, r, a))
        WaygateGetDestinationLocBJ(u) = ALL_RemoveLocation(Waygate##GetDestinationLocBJ(u))
        GetRectFromCircleBJ(l, r) = ALL_RemoveRect(ALL_RectFromCenterSizeBJ_GetRectFromCircleBJ(l, r, .0, false))
        OffsetRectBJ(r, x, y) = ALL_RemoveRect(OffsetRect##BJ(r, x, y))
        Rect(minX, minY, maxX, maxY) = Re##ct(minX, minY, maxX, maxY)
        RectFromCenterSizeBJ(l, width, heigth) = ALL_RemoveRect(ALL_RectFromCenterSizeBJ_GetRectFromCircleBJ(l, width, heigth, true))
        RectFromLoc(min, max) = ALL_RemoveRect(RectFrom##Loc(min, max))
        CountLivingPlayerUnitsOfTypeId(id, p) = GetPlayerUnitTypeCount(p, id)
        GetUnitsInRangeOfLocMatching(r, l, b) = ALL_DestroyGroup(ALL_GetUnitsInRangeOfLocMatching(r, l, b))
        GetUnitsInRectMatching(r, b) = ALL_DestroyGroup(ALL_GetUnitsInRectMatching(r, b))
        GetUnitsInRectOfPlayer(r, p) = ALL_DestroyGroup(ALL_GetUnitsInRectOfPlayer(r, p))
        GetUnitsOfPlayerAndTypeId(p, id) = ALL_DestroyGroup(ALL_GetUnitsOfPlayerAndTypeId(p, id))
        GetUnitsOfPlayerMatching(p, b) = ALL_DestroyGroup(ALL_GetUnitsOfPlayerMatching(p, b))
        GetUnitsOfTypeIdAll(id) = ALL_DestroyGroup(ALL_GetUnitsOfTypeIdAll(id))
        GetUnitsSelectedAll(p) = ALL_DestroyGroup(ALL_GetUnitsSelectedAll(p))
        GetForceOfPlayer(p) = ALL_DestroyForce(ALL_GetForceOfPlayer(p))
        GetPlayersAllies(p) = ALL_DestroyForce(ALL_GetPlayersAllies(p))
        GetPlayersByMapControl(mc) = ALL_DestroyForce(ALL_GetPlayersByMapControl(mc))
        GetPlayersEnemies(p) = ALL_DestroyForce(ALL_GetPlayersEnemies(p))
        GetPlayersMatching(b) = ALL_DestroyForce(ALL_GetPlayersMatching(b))
        <call ALL_AddSpecialEffectLocBJ>(l, s) = {
            if ALL_Remove {
                bj_lastCreatedEffect = null
                DestroyEffect(AddSpecialEffectLoc(s, l))
            } else {
                bj_lastCreatedEffect = AddSpecialEffectLoc(s, l)
            }
        }
        <call ALL_AddSpecialEffectTargetUnitBJ>(a, w, s) = {
            if ALL_Remove {
                bj_lastCreatedEffect = null
                DestroyEffect(AddSpecialEffectTarget(s, w, a))
            } else {
                bj_lastCreatedEffect = AddSpecialEffectTarget(s, w, a)
            }
        }
    }

    enum (ALL_Leak) {
        private LocationLeak
        private RectLeak
        private GroupLeak
        private ForceLeak
    }

    private constant timer Timer = CreateTimer()
    private int MaxVar = -1
    private int array LeakNum
    private location array LocationVar
    private rect array RectVar
    private group array GroupVar
    private force array ForceVar
    private group Group
    private force Force
    public bool Remove = false

    private void RemoveLeak() {
        do {
            if MaxVar:LeakNum < GroupLeak {
                if MaxVar:LeakNum < RectLeak {
                    RemoveLocation(MaxVar:LocationVar)
                } else {
                    RemoveRect(MaxVar:RectVar)
                }
            } elseif MaxVar:LeakNum < ForceLeak {
                DestroyGroup(MaxVar:GroupVar)
            } else {
                DestroyForce(MaxVar:ForceVar)
            }
        } whilenot --MaxVar < 0
    }

    public rect RemoveRect(rect leak) {
        if Remove {
            MaxVar++
            MaxVar:RectVar = leak
            MaxVar:LeakNum = RectLeak
            if MaxVar == 0 { TimerStart(Timer, .0, false, function RemoveLeak) }
        }
        return leak
    }
    public rect RectFromCenterSizeBJ_GetRectFromCircleBJ(location l, real width, real height, bool b) {
        real x = GetLocationX(l), y = GetLocationY(l)
        if b {
            width = width * .5
            height = height * .5
            return Re##ct(x - width, y - height, x + width, y + height)
        }
        return Re##ct(x - width, y - width, x + width, y + width)
    }

    public location RemoveLocation(location leak) {
        if Remove {
            MaxVar++
            MaxVar:LocationVar = leak
            MaxVar:LeakNum = LocationLeak
            if MaxVar == 0 { TimerStart(Timer, .0, false, function RemoveLeak) }
        }
        return leak
    }
    public location GetWidgetLoc(widget w) {
        return Loc##ation(GetWidgetX(w), GetWidgetY(w))
    }
    public location PolarProjection##BJ(location l, real r, real a) {
        a = a * .017
        return Loc##ation(GetLocationX(l) + Cos(a) * r, GetLocationY(l) + Sin(a) * r)
    }
    public location MeleeGetLoc##WithinRect(location l, rect r) {
        real minX = GetRectMinX(r), maxX = GetRectMaxX(r), \
            minY = GetRectMinY(r), maxY = GetRectMaxY(r)
        real x = GetLocationX(l), y = GetLocationY(l)
        if x < minX {
            x = minX
        } elseif x > maxX {
            x = maxX
        }
        if y < minY {
            return Loc##ation(x, minY)
        } elseif y > maxY {
            return Loc##ation(x, maxY)
        }
        return Loc##ation(x, y)
    }

    public group DestroyGroup(group leak) {
        if Remove {
            MaxVar++
            MaxVar:GroupVar = leak
            MaxVar:LeakNum = GroupLeak
            if MaxVar == 0 { TimerStart(Timer, .0, false, function RemoveLeak) }
        }
        return leak
    }
    public group GetUnitsInRange##OfLocMatching(real r, location l, boolexpr b) {
        Group = CreateGroup()
        GroupEnumUnitsInRangeOfLoc(Group, l, r, b)
        DestroyBoolExpr(b)
        return Group
    }
    public group GetUnitsInRect##Matching(rect r, boolexpr b) {
        Group = CreateGroup()
        GroupEnumUnitsInRect(Group, r, b)
        DestroyBoolExpr(b)
        return Group
    }
    public group GetUnitsInRect##OfPlayer(rect r, player p) {
        Group = CreateGroup()
        bj_groupEnumOwningPlayer = p
        GroupEnumUnitsInRect(Group, r, filterGetUnitsInRectOfPlayer)
        return Group
    }
    public group GetUnitsOfPlayer##Matching(player p, boolexpr b) {
        Group = CreateGroup()
        GroupEnumUnitsOfPlayer(Group, p, b)
        DestroyBoolExpr(b)
        return Group
    }
    public group GetUnitsOfPlayer##AndTypeId(player p, int id) {
        Group = CreateGroup()
        bj_groupEnumTypeId = id
        GroupEnumUnitsOfPlayer(Group, p, filterGetUnitsOfPlayerAndTypeId)
        return Group
    }
    public group GetUnitsOfTypeId##All(int id) {
        group g = CreateGroup()
        int i = 0
        bj_groupAddGroupDest = CreateGroup()
        bj_groupEnumTypeId = id
        loop {
            GroupEnumUnitsOfPlayer(g, Player(i), filterGetUnitsOfTypeIdAll)
            ForGroup(g, function GroupAddGroupEnum)
            exitwhen i == 15
            i++
        }
        Destroy##Group(g)
        g = null
        return bj_groupAddGroupDest
    }
    public group GetUnits##SelectedAll(player p) {
        Group = CreateGroup()
        SyncSelections()
        GroupEnumUnitsSelected(Group, p, null)
        return Group
    }

    public force DestroyForce(force leak) {
        if Remove {
            MaxVar++
            MaxVar:ForceVar = leak
            MaxVar:LeakNum = ForceLeak
            if MaxVar == 0 { TimerStart(Timer, .0, false, function RemoveLeak) }
        }
        return leak
    }
    public force GetForce##OfPlayer(player p) {
        Force = CreateForce()
        ForceAddPlayer(Force, p)
        return Force
    }
    public force GetPlay##ersAllies(player p) {
        Force = CreateForce()
        ForceEnumAllies(Force, p, null)
        return Force
    }
    public force GetPlayersByMap##Control(mapcontrol mc) {
        player p = Player(0)
        int i = 0
        Force = CreateForce()
        loop {
            if GetPlayerController(p) == mc {
                ForceAddPlayer(Force, p)
            }
            exitwhen i == 15
            p = Player(++i)
        }
        p = null
        return Force
    }
    public force GetPlay##ersEnemies(player p) {
        Force = CreateForce()
        ForceEnumEnemies(Force, p, null)
        return Force
    }
    public force GetPlay##ersMatching(boolexpr b) {
        Force = CreateForce()
        ForceEnumPlayers(Force, b)
        DestroyBoolExpr(b)
        return Force
    }
}
