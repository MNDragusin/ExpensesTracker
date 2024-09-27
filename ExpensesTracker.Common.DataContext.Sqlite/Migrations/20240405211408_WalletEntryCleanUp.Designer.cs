﻿// <auto-generated />
using System;
using ExpensesTracker.Common.DataContext.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExpensesTracker.Common.DataContext.Sqlite.Migrations
{
    [DbContext(typeof(ExpensesContext))]
    [Migration("20240405211408_WalletEntryCleanUp")]
    partial class WalletEntryCleanUp
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("ExpensesTracker.Common.EntityModel.Sqlite.Category", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ExpensesTracker.Common.EntityModel.Sqlite.Label", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("ExpensesTracker.Common.EntityModel.Sqlite.Owner", b =>
                {
                    b.Property<string>("OwnerId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("OwnerId");

                    b.ToTable("Owners");
                });

            modelBuilder.Entity("ExpensesTracker.Common.EntityModel.Sqlite.Wallet", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("ExpensesTracker.Common.EntityModel.Sqlite.WalletEntry", b =>
                {
                    b.Property<string>("EntryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<float>("Amount")
                        .HasColumnType("REAL");

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("LabelId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("WalletId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("EntryId");

                    b.ToTable("WalletEntries");
                });

            modelBuilder.Entity("ExpensesTracker.Common.EntityModel.Sqlite.Category", b =>
                {
                    b.HasOne("ExpensesTracker.Common.EntityModel.Sqlite.Owner", null)
                        .WithMany("Categories")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExpensesTracker.Common.EntityModel.Sqlite.Label", b =>
                {
                    b.HasOne("ExpensesTracker.Common.EntityModel.Sqlite.Owner", null)
                        .WithMany("Labels")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExpensesTracker.Common.EntityModel.Sqlite.Wallet", b =>
                {
                    b.HasOne("ExpensesTracker.Common.EntityModel.Sqlite.Owner", null)
                        .WithMany("Wallets")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExpensesTracker.Common.EntityModel.Sqlite.Owner", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Labels");

                    b.Navigation("Wallets");
                });
#pragma warning restore 612, 618
        }
    }
}
