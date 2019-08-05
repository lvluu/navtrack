﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Navtrack.DataAccess.Model;

namespace Navtrack.DataAccess.Migrations
{
    [DbContext(typeof(NavtrackContext))]
    partial class NavtrackContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0-preview7.19362.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Navtrack.DataAccess.Model.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("IMEI")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.HasKey("Id");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("Navtrack.DataAccess.Model.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Altitude");

                    b.Property<DateTime>("DateTime");

                    b.Property<int>("DeviceId");

                    b.Property<int>("Heading");

                    b.Property<decimal>("Latitude");

                    b.Property<decimal>("Longitude");

                    b.Property<int>("ObjectId");

                    b.Property<int>("Speed");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("ObjectId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Navtrack.DataAccess.Model.Object", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DeviceId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("DeviceId")
                        .IsUnique();

                    b.ToTable("Objects");
                });

            modelBuilder.Entity("Navtrack.DataAccess.Model.Location", b =>
                {
                    b.HasOne("Navtrack.DataAccess.Model.Device", "Device")
                        .WithMany("Locations")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Navtrack.DataAccess.Model.Object", "Object")
                        .WithMany("Locations")
                        .HasForeignKey("ObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Navtrack.DataAccess.Model.Object", b =>
                {
                    b.HasOne("Navtrack.DataAccess.Model.Device", "Device")
                        .WithOne("Object")
                        .HasForeignKey("Navtrack.DataAccess.Model.Object", "DeviceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
