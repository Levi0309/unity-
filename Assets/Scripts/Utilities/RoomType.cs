using System;
[Flags]
public enum RoomType
{
    MinorEnemy=1,//��ͨ����1 2 4 8
    EliteEnemy=2,//��Ӣ����
    Shop=4,
    Treasure=8,//����
    RestRoom=16,//��Ϣ����
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
