﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(OrderContext))]
    [Migration("20240411105801_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Data.Models.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .HasColumnType("int")
                        .HasColumnName("company_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nchar(128)")
                        .HasColumnName("name")
                        .IsFixedLength();

                    b.HasKey("CompanyId")
                        .HasName("PK__Company__3E267235D89E42FC");

                    b.ToTable("Company", (string)null);
                });

            modelBuilder.Entity("Data.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("order_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("int")
                        .HasColumnName("company_id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)")
                        .HasColumnName("description");

                    b.HasKey("OrderId")
                        .HasName("PK__Order__46596229DAD3555B");

                    b.HasIndex("CompanyId");

                    b.ToTable("Order", (string)null);
                });

            modelBuilder.Entity("Data.Models.Orderproduct", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("int")
                        .HasColumnName("order_id");

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18, 2)")
                        .HasColumnName("price");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("quantity");

                    b.HasKey("OrderId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("orderproduct", (string)null);
                });

            modelBuilder.Entity("Data.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("name");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18, 2)")
                        .HasColumnName("price");

                    b.HasKey("ProductId")
                        .HasName("PK__Product__47027DF569E694B0");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("Data.Models.Order", b =>
                {
                    b.HasOne("Data.Models.Company", "Company")
                        .WithMany("Orders")
                        .HasForeignKey("CompanyId")
                        .IsRequired()
                        .HasConstraintName("FK_order_to_company");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Data.Models.Orderproduct", b =>
                {
                    b.HasOne("Data.Models.Order", "Order")
                        .WithMany("Orderproducts")
                        .HasForeignKey("OrderId")
                        .IsRequired()
                        .HasConstraintName("FK_orderproduct_order");

                    b.HasOne("Data.Models.Product", "Product")
                        .WithMany("Orderproducts")
                        .HasForeignKey("ProductId")
                        .IsRequired()
                        .HasConstraintName("FK_orderproduct_product");

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Data.Models.Company", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Data.Models.Order", b =>
                {
                    b.Navigation("Orderproducts");
                });

            modelBuilder.Entity("Data.Models.Product", b =>
                {
                    b.Navigation("Orderproducts");
                });
#pragma warning restore 612, 618
        }
    }
}
