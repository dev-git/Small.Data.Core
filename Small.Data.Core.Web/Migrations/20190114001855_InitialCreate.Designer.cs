﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Small.Data.Core.Web.Models;

namespace Small.Data.Core.Web.Migrations
{
    [DbContext(typeof(NotesContext))]
    [Migration("20190114001855_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity("Small.Data.Core.Web.Models.Note", b =>
                {
                    b.Property<int>("NoteID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category");

                    b.Property<string>("Detail");

                    b.Property<string>("Name");

                    b.HasKey("NoteID");

                    b.ToTable("Notes");
                });
#pragma warning restore 612, 618
        }
    }
}
