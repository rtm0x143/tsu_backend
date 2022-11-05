﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieCatalogBackend.Data.Tokens;
using Oracle.EntityFrameworkCore.Metadata;

#nullable disable

namespace MovieCatalogBackend.Migrations.TokenList
{
    [DbContext(typeof(TokenListContext))]
    partial class TokenListContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            OracleModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MovieCatalogBackend.Data.Tokens.BlackedToken", b =>
                {
                    b.Property<string>("Token")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<DateTime>("Expiretion")
                        .HasColumnType("TIMESTAMP(7)");

                    b.HasKey("Token");

                    b.ToTable("Tokens");
                });
#pragma warning restore 612, 618
        }
    }
}