using System;
using System.Collections.Generic;
using Realms;
using MongoDB.Bson;

public partial class PlayerData : IRealmObject
{
    [MapTo("_id")]
    [PrimaryKey]
    public ObjectId Id { get; set; }
    [Required]
    public string PlayerName { get; set; }
    [Required]
    public string Role { get; set; }

    public int Tokens { get; set; }

    public int TopupGems { get; set; }

    [MapTo("isRoom")]
    public bool? IsRoom { get; set; }

    [MapTo("isRoom_Color")]
    public string IsRoomColor { get; set; }

    [MapTo("isRoom_Percent")]
    public int? IsRoomPercent { get; set; }

    [MapTo("isRoom_Roomname")]
    public string IsRoomRoomname { get; set; }

    [MapTo("owner_id")]
    [Required]
    public string OwnerId { get; set; }
}
