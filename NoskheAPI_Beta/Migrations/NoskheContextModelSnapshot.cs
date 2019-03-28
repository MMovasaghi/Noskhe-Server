﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NoskheAPI_Beta.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace NoskheAPI_Beta.Migrations
{
    [DbContext(typeof(NoskheContext))]
    partial class NoskheContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("NoskheAPI_Beta.Models.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountOwnerName");

                    b.Property<string>("BankName");

                    b.Property<string>("IBAN");

                    b.Property<int>("PharmacyId");

                    b.HasKey("AccountId");

                    b.HasIndex("PharmacyId")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Cosmetic", b =>
                {
                    b.Property<int>("CosmeticId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("Price");

                    b.Property<DateTime>("ProductPictureUploadDate");

                    b.Property<string>("ProductPictureUrl");

                    b.HasKey("CosmeticId");

                    b.ToTable("Cosmetics");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.CosmeticShoppingCart", b =>
                {
                    b.Property<int>("CosmeticId");

                    b.Property<int>("ShoppingCartId");

                    b.Property<int>("Quantity");

                    b.HasKey("CosmeticId", "ShoppingCartId");

                    b.HasIndex("ShoppingCartId");

                    b.ToTable("CosmeticShoppingCarts");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Courier", b =>
                {
                    b.Property<int>("CourierId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Birthday");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<int>("Gender");

                    b.Property<bool>("IsAvailableNow");

                    b.Property<bool>("IsBusy");

                    b.Property<string>("LastName");

                    b.Property<string>("MelliNumber");

                    b.Property<string>("NameOfFather");

                    b.Property<string>("Password");

                    b.Property<string>("Phone");

                    b.Property<DateTime>("ProfilePictureUploadDate");

                    b.Property<string>("ProfilePictureUrl");

                    b.Property<DateTime>("RegisterationDate");

                    b.HasKey("CourierId");

                    b.ToTable("Couriers");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.CourierTextMessage", b =>
                {
                    b.Property<int>("CourierTextMessageId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CourierId");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Message");

                    b.Property<int>("Type");

                    b.HasKey("CourierTextMessageId");

                    b.HasIndex("CourierId");

                    b.ToTable("CourierTextMessages");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Birthday");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<int>("Gender");

                    b.Property<bool>("IsEmailValidated");

                    b.Property<bool>("IsPhoneValidated");

                    b.Property<string>("LastName");

                    b.Property<int>("Money");

                    b.Property<string>("Password");

                    b.Property<string>("Phone");

                    b.Property<DateTime>("ProfilePictureUploadDate");

                    b.Property<string>("ProfilePictureUrl");

                    b.Property<DateTime>("RegisterationDate");

                    b.HasKey("CustomerId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.CustomerHubMap", b =>
                {
                    b.Property<int>("CustomerHubMapId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Connected");

                    b.Property<string>("ConnectionID");

                    b.Property<int>("CustomerId");

                    b.HasKey("CustomerHubMapId");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.ToTable("CustomerHubMap");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.CustomerNotification", b =>
                {
                    b.Property<int>("CustomerNotificationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Arg1");

                    b.Property<string>("Arg2");

                    b.Property<string>("Arg3");

                    b.Property<string>("Arg4");

                    b.Property<string>("Arg5");

                    b.Property<string>("Arg6");

                    b.Property<int>("CustomerId");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("HasRecieved");

                    b.Property<int>("Type");

                    b.HasKey("CustomerNotificationId");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.ToTable("CustomerNotifications");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.CustomerResetPasswordToken", b =>
                {
                    b.Property<int>("CustomerResetPasswordTokenId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<bool>("IsValid");

                    b.Property<string>("Token");

                    b.Property<long>("TokenRefreshRequests");

                    b.Property<DateTime>("ValidFrom");

                    b.Property<DateTime>("ValidTo");

                    b.HasKey("CustomerResetPasswordTokenId");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.ToTable("CustomerResetPasswordToken");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.CustomerTextMessage", b =>
                {
                    b.Property<int>("CustomerTextMessageId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Message");

                    b.Property<int>("NumberOfAttempts");

                    b.Property<int>("Type");

                    b.Property<bool>("Validated");

                    b.HasKey("CustomerTextMessageId");

                    b.HasIndex("CustomerId");

                    b.ToTable("CustomerTextMessages");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.CustomerToken", b =>
                {
                    b.Property<int>("CustomerTokenId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<bool>("IsValid");

                    b.Property<long>("LoginRequests");

                    b.Property<string>("Token");

                    b.Property<long>("TokenRefreshRequests");

                    b.Property<DateTime>("ValidFrom");

                    b.Property<DateTime>("ValidTo");

                    b.HasKey("CustomerTokenId");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.ToTable("CustomerTokens");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Manager", b =>
                {
                    b.Property<int>("ManagerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<int>("Gender");

                    b.Property<string>("LastName");

                    b.Property<string>("MelliNumber");

                    b.Property<string>("NameOfFather");

                    b.Property<string>("Password");

                    b.Property<string>("Phone");

                    b.Property<DateTime>("ProfilePictureUploadDate");

                    b.Property<string>("ProfilePictureUrl");

                    b.HasKey("ManagerId");

                    b.ToTable("Managers");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Medicine", b =>
                {
                    b.Property<int>("MedicineId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("Price");

                    b.Property<DateTime>("ProductPictureUploadDate");

                    b.Property<string>("ProductPictureUrl");

                    b.Property<int>("Type");

                    b.HasKey("MedicineId");

                    b.ToTable("Medicines");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.MedicineShoppingCart", b =>
                {
                    b.Property<int>("MedicineId");

                    b.Property<int>("ShoppingCartId");

                    b.Property<int>("Quantity");

                    b.HasKey("MedicineId", "ShoppingCartId");

                    b.HasIndex("ShoppingCartId");

                    b.ToTable("MedicineShoppingCarts");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Notation", b =>
                {
                    b.Property<int>("NotationId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BrandPreference");

                    b.Property<string>("Description");

                    b.Property<bool>("HasOtherDiseases");

                    b.Property<bool>("HasPregnancy");

                    b.Property<int>("ShoppingCartId");

                    b.HasKey("NotationId");

                    b.HasIndex("ShoppingCartId")
                        .IsUnique();

                    b.ToTable("Notations");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Occurrence", b =>
                {
                    b.Property<int>("OccurrenceId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CourierHasDeliveredOrderToCustomerOn");

                    b.Property<DateTime>("CourierHasReceivedOrderFromPharmacyOn");

                    b.Property<DateTime>("CustomerHasFinallyAcceptedOrderOn");

                    b.Property<DateTime>("CustomerHasReceivedOrderFromCourierOn");

                    b.Property<DateTime>("CustomerHasRequestedOrderOn");

                    b.Property<int>("OrderId");

                    b.Property<DateTime>("PharmacyHasAcceptedOrderOn");

                    b.Property<DateTime>("PharmacyHasPackedOrderOn");

                    b.HasKey("OccurrenceId");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("Occurrences");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CourierId");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("HasBeenAcceptedByCustomer");

                    b.Property<bool>("HasBeenDeliveredToCustomer");

                    b.Property<bool>("HasBeenPaid");

                    b.Property<bool>("HasBeenReceivedByCustomer");

                    b.Property<bool>("HasBeenSetToACourier");

                    b.Property<bool>("HasBeenSettled");

                    b.Property<bool>("HasPrescription");

                    b.Property<int>("PackingTime");

                    b.Property<int>("PharmacyId");

                    b.Property<int>("Price");

                    b.Property<DateTime>("SettlementDate");

                    b.Property<int>("ShoppingCartId");

                    b.Property<string>("UOI");

                    b.HasKey("OrderId");

                    b.HasIndex("CourierId");

                    b.HasIndex("PharmacyId");

                    b.HasIndex("ShoppingCartId")
                        .IsUnique();

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Pharmacy", b =>
                {
                    b.Property<int>("PharmacyId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<double>("AddressLatitude");

                    b.Property<double>("AddressLongitude");

                    b.Property<decimal>("Credit");

                    b.Property<string>("Email");

                    b.Property<bool>("IsAvailableNow");

                    b.Property<int>("ManagerId");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<long>("PendingRequests");

                    b.Property<string>("Phone");

                    b.Property<DateTime>("ProfilePictureUploadDate");

                    b.Property<string>("ProfilePictureUrl");

                    b.Property<DateTime>("RegisterationDate");

                    b.Property<string>("UPI");

                    b.HasKey("PharmacyId");

                    b.HasIndex("ManagerId");

                    b.ToTable("Pharmacies");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.PharmacyHubMap", b =>
                {
                    b.Property<int>("PharmacyHubMapId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Connected");

                    b.Property<string>("ConnectionID");

                    b.Property<int>("PharmacyId");

                    b.HasKey("PharmacyHubMapId");

                    b.HasIndex("PharmacyId")
                        .IsUnique();

                    b.ToTable("PharmacyHubMap");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.PharmacyNotification", b =>
                {
                    b.Property<int>("PharmacyNotificationId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<bool>("HasRecieved");

                    b.Property<int>("PharmacyId");

                    b.Property<int>("Type");

                    b.HasKey("PharmacyNotificationId");

                    b.HasIndex("PharmacyId")
                        .IsUnique();

                    b.ToTable("PharmacyNotifications");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.PharmacyToken", b =>
                {
                    b.Property<int>("PharmacyTokenId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsValid");

                    b.Property<long>("LoginRequests");

                    b.Property<int>("PharmacyId");

                    b.Property<string>("Token");

                    b.Property<long>("TokenRefreshRequests");

                    b.Property<DateTime>("ValidFrom");

                    b.Property<DateTime>("ValidTo");

                    b.HasKey("PharmacyTokenId");

                    b.HasIndex("PharmacyId")
                        .IsUnique();

                    b.ToTable("PharmacyTokens");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Prescription", b =>
                {
                    b.Property<int>("PrescriptionId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("HasBeenAcceptedByPharmacy");

                    b.Property<string>("PictureUrl_1");

                    b.Property<string>("PictureUrl_2");

                    b.Property<string>("PictureUrl_3");

                    b.Property<DateTime>("PicturesUploadDate");

                    b.Property<int>("ShoppingCartId");

                    b.HasKey("PrescriptionId");

                    b.HasIndex("ShoppingCartId")
                        .IsUnique();

                    b.ToTable("Prescriptions");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.PrescriptionItem", b =>
                {
                    b.Property<int>("PrescriptionItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("PrescriptionId");

                    b.Property<int>("Price");

                    b.Property<int>("Quantity");

                    b.HasKey("PrescriptionItemId");

                    b.HasIndex("PrescriptionId");

                    b.ToTable("PrescriptionItems");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Score", b =>
                {
                    b.Property<int>("ScoreId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerSatisfaction");

                    b.Property<int>("PackingAverageTimeInSeconds");

                    b.Property<int>("PharmacyId");

                    b.Property<int>("RankAmongPharmacies");

                    b.HasKey("ScoreId");

                    b.HasIndex("PharmacyId")
                        .IsUnique();

                    b.ToTable("Scores");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.ServiceMapping", b =>
                {
                    b.Property<int>("ServiceMappingId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("CustomerCancellation");

                    b.Property<DateTime>("CustomerCancellationDate");

                    b.Property<int>("CustomerCancellationReason");

                    b.Property<string>("FoundPharmacies");

                    b.Property<DateTime>("PharmacyCancellationDate");

                    b.Property<int>("PharmacyCancellationReason");

                    b.Property<int>("PharmacyServiceStatus");

                    b.Property<int>("PrimativePharmacyId");

                    b.Property<int>("ShoppingCartId");

                    b.HasKey("ServiceMappingId");

                    b.HasIndex("ShoppingCartId")
                        .IsUnique();

                    b.ToTable("ServiceMappings");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Settle", b =>
                {
                    b.Property<int>("SettleId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("CommissionCoefficient");

                    b.Property<decimal>("Credit");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("HasBeenSettled");

                    b.Property<int>("NumberOfOrders");

                    b.Property<int>("PharmacyId");

                    b.Property<string>("USI");

                    b.HasKey("SettleId");

                    b.HasIndex("PharmacyId");

                    b.ToTable("Settles");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.ShoppingCart", b =>
                {
                    b.Property<int>("ShoppingCartId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<double>("AddressLatitude");

                    b.Property<double>("AddressLongitude");

                    b.Property<int>("CustomerId");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("HasBeenRequested");

                    b.Property<string>("USCI");

                    b.HasKey("ShoppingCartId");

                    b.HasIndex("CustomerId");

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.WalletTransactionHistory", b =>
                {
                    b.Property<int>("WalletTransactionHistoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("IsSuccessful");

                    b.Property<int>("Price");

                    b.HasKey("WalletTransactionHistoryId");

                    b.HasIndex("CustomerId");

                    b.ToTable("WalletTransactionHistories");
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Account", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Pharmacy", "Pharmacy")
                        .WithOne("Account")
                        .HasForeignKey("NoskheAPI_Beta.Models.Account", "PharmacyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.CosmeticShoppingCart", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Cosmetic", "Cosmetic")
                        .WithMany("CosmeticShoppingCarts")
                        .HasForeignKey("CosmeticId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NoskheAPI_Beta.Models.ShoppingCart", "ShoppingCart")
                        .WithMany("CosmeticShoppingCarts")
                        .HasForeignKey("ShoppingCartId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.CourierTextMessage", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Courier", "Courier")
                        .WithMany("CourierTextMessages")
                        .HasForeignKey("CourierId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.CustomerHubMap", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Customer", "Customer")
                        .WithOne("CustomerHubMap")
                        .HasForeignKey("NoskheAPI_Beta.Models.CustomerHubMap", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.CustomerNotification", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Customer", "Customer")
                        .WithOne("CustomerNotification")
                        .HasForeignKey("NoskheAPI_Beta.Models.CustomerNotification", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.CustomerResetPasswordToken", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Customer", "Customer")
                        .WithOne("CustomerResetPasswordToken")
                        .HasForeignKey("NoskheAPI_Beta.Models.CustomerResetPasswordToken", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.CustomerTextMessage", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Customer", "Customer")
                        .WithMany("CustomerTextMessages")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.CustomerToken", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Customer", "Customer")
                        .WithOne("CustomerToken")
                        .HasForeignKey("NoskheAPI_Beta.Models.CustomerToken", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.MedicineShoppingCart", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Medicine", "Medicine")
                        .WithMany("MedicineShoppingCarts")
                        .HasForeignKey("MedicineId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NoskheAPI_Beta.Models.ShoppingCart", "ShoppingCart")
                        .WithMany("MedicineShoppingCarts")
                        .HasForeignKey("ShoppingCartId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Notation", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.ShoppingCart", "ShoppingCart")
                        .WithOne("Notation")
                        .HasForeignKey("NoskheAPI_Beta.Models.Notation", "ShoppingCartId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Occurrence", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Order", "Order")
                        .WithOne("Occurrence")
                        .HasForeignKey("NoskheAPI_Beta.Models.Occurrence", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Order", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Courier", "Courier")
                        .WithMany("Orders")
                        .HasForeignKey("CourierId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NoskheAPI_Beta.Models.Pharmacy", "Pharmacy")
                        .WithMany("Orders")
                        .HasForeignKey("PharmacyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NoskheAPI_Beta.Models.ShoppingCart", "ShoppingCart")
                        .WithOne("Order")
                        .HasForeignKey("NoskheAPI_Beta.Models.Order", "ShoppingCartId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Pharmacy", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Manager", "Manager")
                        .WithMany("Pharmacies")
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.PharmacyHubMap", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Pharmacy", "Pharmacy")
                        .WithOne("PharmacyHubMap")
                        .HasForeignKey("NoskheAPI_Beta.Models.PharmacyHubMap", "PharmacyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.PharmacyNotification", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Pharmacy", "Pharmacy")
                        .WithOne("PharmacyNotification")
                        .HasForeignKey("NoskheAPI_Beta.Models.PharmacyNotification", "PharmacyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.PharmacyToken", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Pharmacy", "Pharmacy")
                        .WithOne("PharmacyToken")
                        .HasForeignKey("NoskheAPI_Beta.Models.PharmacyToken", "PharmacyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Prescription", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.ShoppingCart", "ShoppingCart")
                        .WithOne("Prescription")
                        .HasForeignKey("NoskheAPI_Beta.Models.Prescription", "ShoppingCartId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.PrescriptionItem", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Prescription", "Prescription")
                        .WithMany("PrescriptionItems")
                        .HasForeignKey("PrescriptionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Score", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Pharmacy", "Pharmacy")
                        .WithOne("Score")
                        .HasForeignKey("NoskheAPI_Beta.Models.Score", "PharmacyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.ServiceMapping", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.ShoppingCart", "ShoppingCart")
                        .WithOne("PharmacyMapping")
                        .HasForeignKey("NoskheAPI_Beta.Models.ServiceMapping", "ShoppingCartId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.Settle", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Pharmacy", "Pharmacy")
                        .WithMany("Settles")
                        .HasForeignKey("PharmacyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.ShoppingCart", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Customer", "Customer")
                        .WithMany("ShoppingCarts")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NoskheAPI_Beta.Models.WalletTransactionHistory", b =>
                {
                    b.HasOne("NoskheAPI_Beta.Models.Customer", "Customer")
                        .WithMany("WalletTransactionHistories")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
