﻿using System.ComponentModel.DataAnnotations;

namespace MovieCatalogBackend.Data.MovieCatalog.Dtos;

public class ProfileModel
{
    public Guid id { get; set; }
    public string? nickName { get; set; }
    
    [Required] 
    [DataType(DataType.EmailAddress)]
    public string email { get; set; }
    
    [DataType(DataType.ImageUrl)] 
    public string avatarLink { get; set; }
    
    [Required] public string name { get; set; }
    [Required] public DateTime birthDate { get; set; }
    public Gender gender { get; set; }
}