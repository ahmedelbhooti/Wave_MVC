﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wave.DataAccess.Data;

#nullable disable

namespace Wave.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231030223746_AddProductToDb")]
    partial class AddProductToDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Wave.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DisplayOrder = 1,
                            Name = "First"
                        },
                        new
                        {
                            Id = 2,
                            DisplayOrder = 2,
                            Name = "Second"
                        },
                        new
                        {
                            Id = 3,
                            DisplayOrder = 3,
                            Name = "Third"
                        });
                });

            modelBuilder.Entity("Wave.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ListPrice")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 9,
                            Description = "Die bequeme, verspiegelte Unisex Schwimmbrille aus Silikon von wave-plus ist perfekt für das Training im Wasser. Die Dichtungen passen sich dem Gesicht an, reduzieren den Druck um die Augen und sorgen für ein leichtes Tragegefühl. Die breiten Gläser gewährleisten eine optimale seitliche Sicht. Das elastische Kopfband lässt sich gut am Hinterkopf dem individuellen Kopfumfang anpassen und garantiert einen sehr stabilen Sitz während dem Training. Die Anti-Fog Beschichtung der Gläser schützt vor Beschlagen.",
                            ListPrice = 16.899999999999999,
                            Name = "SILIKON UV SPEED SCHWIMMBRILLE \"ANTI-FOG\"",
                            Price = 16.899999999999999
                        },
                        new
                        {
                            Id = 7,
                            Description = "Die bequeme, verspiegelte Unisex Schwimmbrille aus Silikon von wave-plus ist perfekt für das Training im Wasser. Die Dichtungen passen sich dem Gesicht an, reduzieren den Druck um die Augen und sorgen für ein leichtes Tragegefühl. Die breiten Gläser gewährleisten eine optimale seitliche Sicht. Das elastische Kopfband lässt sich gut am Hinterkopf dem individuellen Kopfumfang anpassen und garantiert einen sehr stabilen Sitz während dem Training. Die Anti-Fog Beschichtung der Gläser schützt vor Beschlagen.",
                            ListPrice = 16.899999999999999,
                            Name = "SILIKON Klare SCHWIMMBRILLE \"ANTI-FOG\" ( Schwarz,Waiss,Blau)",
                            Price = 16.899999999999999
                        },
                        new
                        {
                            Id = 5,
                            Description = "Die komfortable Silikon Kinder Schwimmbrille  wave-plus ist perfekt für die Kleinen Wasserratten und es gibt sie in folgenden Farben: Königsblau, Blau und Gelb-Rosa. Die weichen Augenschalen passen sich dem Gesicht an und reduzieren den Druck um die Augen. Das elastische Kopfband lässt sich gut am Hinterkopf anpassen und garantiert einen sehr stabilen Sitz während dem Training. Die Anti-Fog Beschichtung der Gläser schützt vor Beschlagen.",
                            ListPrice = 12.9,
                            Name = "SILIKON KINDER SCHWIMMBRILLE \"ANTI-FOG\" (Königsblau, Hellblau, Gelb-Rosa)",
                            Price = 12.9
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
