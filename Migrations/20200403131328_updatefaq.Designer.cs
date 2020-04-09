﻿// <auto-generated />
using System;
using CoronaBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CovidBot.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200403131328_updatefaq")]
    partial class updatefaq
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CoronaBot.Models.FAQIntent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Answer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Intent")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Intent")
                        .IsUnique()
                        .HasFilter("[Intent] IS NOT NULL");

                    b.ToTable("FAQIntents");
                });

            modelBuilder.Entity("CoronaBot.Models.FAQQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("FAQIntentId")
                        .HasColumnType("int");

                    b.Property<string>("Question")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FAQIntentId");

                    b.ToTable("FAQQuestions");
                });

            modelBuilder.Entity("CoronaBot.Models.SelfCertification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DateOfBorn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DomicileAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DomicileCity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EndPlace")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EndRegion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentificationDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentificationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentificationReleased")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentificationType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlaceOfBorn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResidenceAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResidenceCity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StartPlace")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StartRegion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Step")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("X1Work")
                        .HasColumnType("bit");

                    b.Property<bool>("X2Urgency")
                        .HasColumnType("bit");

                    b.Property<bool>("X3Necessary")
                        .HasColumnType("bit");

                    b.Property<bool>("X4Health")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("SelfCertifications");
                });

            modelBuilder.Entity("CoronaBot.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CoronaBot.Models.FAQQuestion", b =>
                {
                    b.HasOne("CoronaBot.Models.FAQIntent", null)
                        .WithMany("FAQQuestions")
                        .HasForeignKey("FAQIntentId");
                });

            modelBuilder.Entity("CoronaBot.Models.SelfCertification", b =>
                {
                    b.HasOne("CoronaBot.Models.User", null)
                        .WithMany("SelfCertifications")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
