using System;
[Flags]
public enum RoomType
{
    MinorEnemy=1,//普通敌人1 2 4 8
    EliteEnemy=2,//精英敌人
    Shop=4,
    Treasure=8,//宝箱
    RestRoom=16,//休息房间
    Boss=32
}

public enum RoomState 
{
    Locked,
    Attainable,
    Visited,
   
}

public enum CardType 
{
    Attack,
    Deffend,
    Abilities
}
public enum EffectExcuteType 
{
    self,
    Target,
    All
}
