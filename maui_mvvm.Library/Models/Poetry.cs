﻿using SQLite;
namespace maui_mvvm.Models;


[Table("works")]
public class Poetry
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("author_name")]
    public string Author { get; set; } = string.Empty;

    [Column("dynasty")]
    public string Dynasty { get; set; } = string.Empty;

    [Column("content")]
    public string Content { get; set; } = string.Empty;

    private string? _snippet;

    [Ignore]  //用这个来告诉数据库，这个字段不在数据库里
    public string Snippet => _snippet ??= Content.Split('。')[0];
}