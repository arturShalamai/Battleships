﻿// <auto-generated />
using Battleships.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Battleships.Migrations.Migrations
{
    [DbContext(typeof(BattleshipsContext))]
    [Migration("20180802153544_AddedPlayersReadyInfo")]
    partial class AddedPlayersReadyInfo
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Battleships.DAL.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Status");

                    b.Property<bool>("Winner");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Battleships.DAL.GameInfo", b =>
                {
                    b.Property<Guid>("GameId");

                    b.Property<string>("FirstUserField");

                    b.Property<bool>("FirstUserReady");

                    b.Property<string>("SecondUserField");

                    b.Property<bool>("SecondUserReady");

                    b.Property<bool>("Turn");

                    b.HasKey("GameId");

                    b.ToTable("GamesInfo");
                });

            modelBuilder.Entity("Battleships.DAL.GamePlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("GameId");

                    b.Property<Guid>("PlayerId");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayerId");

                    b.ToTable("GamePlayer");
                });

            modelBuilder.Entity("Battleships.DAL.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName");

                    b.Property<string>("NickName")
                        .IsRequired();

                    b.Property<string>("Password");

                    b.Property<double>("Score");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Battleships.DAL.GameInfo", b =>
                {
                    b.HasOne("Battleships.DAL.Game", "Game")
                        .WithOne("GameInfo")
                        .HasForeignKey("Battleships.DAL.GameInfo", "GameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Battleships.DAL.GamePlayer", b =>
                {
                    b.HasOne("Battleships.DAL.Game", "Game")
                        .WithMany("PlayersInfo")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Battleships.DAL.Player", "Player")
                        .WithMany("GamesInfo")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
