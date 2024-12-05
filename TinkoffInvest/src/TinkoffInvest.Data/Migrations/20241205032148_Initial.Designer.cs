﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TinkoffInvest.Data;

#nullable disable

namespace TinkoffInvest.Data.Migrations
{
    [DbContext(typeof(TinkoffInvestDbContext))]
    [Migration("20241205032148_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TinkoffInvest.Data.Entities.SecurityEntity", b =>
                {
                    b.Property<string>("Uid")
                        .HasColumnType("text");

                    b.Property<string>("ClassCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CountryOfRisk")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Figi")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("InstrumentType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Isin")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("LiquidityFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Sector")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("isFavorite")
                        .HasColumnType("boolean");

                    b.Property<bool>("isLastPriceSubscribed")
                        .HasColumnType("boolean");

                    b.HasKey("Uid");

                    b.ToTable("Securities");
                });
#pragma warning restore 612, 618
        }
    }
}
