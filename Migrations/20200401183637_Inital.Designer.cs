﻿// <auto-generated />
using CoronaBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CovidBot.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200401183637_Inital")]
    partial class Inital
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3");

            modelBuilder.Entity("CoronaBot.Models.SelfCertification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DateOfBorn")
                        .HasColumnType("TEXT");

                    b.Property<string>("DomicileAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("DomicileCity")
                        .HasColumnType("TEXT");

                    b.Property<string>("EndPlace")
                        .HasColumnType("TEXT");

                    b.Property<string>("EndRegion")
                        .HasColumnType("TEXT");

                    b.Property<string>("IdentificationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("IdentificationNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("IdentificationReleased")
                        .HasColumnType("TEXT");

                    b.Property<string>("IdentificationType")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("PlaceOfBorn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Reason")
                        .HasColumnType("TEXT");

                    b.Property<string>("ResidenceAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("ResidenceCity")
                        .HasColumnType("TEXT");

                    b.Property<string>("StartPlace")
                        .HasColumnType("TEXT");

                    b.Property<string>("StartRegion")
                        .HasColumnType("TEXT");

                    b.Property<int>("Step")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("X1Work")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("X2Urgency")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("X3Necessary")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("X4Health")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("SelfCertifications");
                });

            modelBuilder.Entity("CoronaBot.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Users");
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
