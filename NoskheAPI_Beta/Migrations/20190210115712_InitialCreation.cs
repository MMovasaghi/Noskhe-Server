using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NoskheAPI_Beta.Migrations
{
    public partial class InitialCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cosmetics",
                columns: table => new
                {
                    CosmeticId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    ProductPictureUrl = table.Column<string>(nullable: true),
                    ProductPictureUploadDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cosmetics", x => x.CosmeticId);
                });

            migrationBuilder.CreateTable(
                name: "Couriers",
                columns: table => new
                {
                    CourierId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    RegisterationDate = table.Column<DateTime>(nullable: false),
                    NameOfFather = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    Birthday = table.Column<DateTime>(nullable: false),
                    MelliNumber = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    ProfilePictureUrl = table.Column<string>(nullable: true),
                    ProfilePictureUploadDate = table.Column<DateTime>(nullable: false),
                    IsAvailableNow = table.Column<bool>(nullable: false),
                    IsBusy = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Couriers", x => x.CourierId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    RegisterationDate = table.Column<DateTime>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    Birthday = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    ProfilePictureUrl = table.Column<string>(nullable: true),
                    ProfilePictureUploadDate = table.Column<DateTime>(nullable: false),
                    Money = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    ManagerId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    NameOfFather = table.Column<string>(nullable: true),
                    MelliNumber = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    ProfilePictureUrl = table.Column<string>(nullable: true),
                    ProfilePictureUploadDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.ManagerId);
                });

            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    MedicineId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    ProductPictureUrl = table.Column<string>(nullable: true),
                    ProductPictureUploadDate = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.MedicineId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerNotificationMap",
                columns: table => new
                {
                    CustomerNotificationMapId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConnectionID = table.Column<string>(nullable: true),
                    Connected = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerNotificationMap", x => x.CustomerNotificationMapId);
                    table.ForeignKey(
                        name: "FK_CustomerNotificationMap_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerTokens",
                columns: table => new
                {
                    CustomerTokenId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Token = table.Column<string>(nullable: true),
                    IsValid = table.Column<bool>(nullable: false),
                    TokenRefreshRequests = table.Column<uint>(nullable: false),
                    LoginRequests = table.Column<uint>(nullable: false),
                    ValidFrom = table.Column<DateTime>(nullable: false),
                    ValidTo = table.Column<DateTime>(nullable: false),
                    IsAvailableInSignalR = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTokens", x => x.CustomerTokenId);
                    table.ForeignKey(
                        name: "FK_CustomerTokens_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    ShoppingCartId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    USCI = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    AddressLongitude = table.Column<double>(nullable: false),
                    AddressLatitude = table.Column<double>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    HasBeenRequested = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.ShoppingCartId);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TextMessages",
                columns: table => new
                {
                    TextMessageId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(nullable: false),
                    VerificationCode = table.Column<string>(nullable: true),
                    HasBeenLocked = table.Column<bool>(nullable: false),
                    NumberOfAttempts = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextMessages", x => x.TextMessageId);
                    table.ForeignKey(
                        name: "FK_TextMessages_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pharmacies",
                columns: table => new
                {
                    PharmacyId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    UPI = table.Column<string>(nullable: true),
                    RegisterationDate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    ProfilePictureUrl = table.Column<string>(nullable: true),
                    ProfilePictureUploadDate = table.Column<DateTime>(nullable: false),
                    AddressLongitude = table.Column<double>(nullable: false),
                    AddressLatitude = table.Column<double>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    IsAvailableNow = table.Column<bool>(nullable: false),
                    Credit = table.Column<decimal>(nullable: false),
                    PendingRequests = table.Column<uint>(nullable: false),
                    ManagerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pharmacies", x => x.PharmacyId);
                    table.ForeignKey(
                        name: "FK_Pharmacies_Managers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Managers",
                        principalColumn: "ManagerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CosmeticShoppingCarts",
                columns: table => new
                {
                    CosmeticId = table.Column<int>(nullable: false),
                    ShoppingCartId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CosmeticShoppingCarts", x => new { x.CosmeticId, x.ShoppingCartId });
                    table.ForeignKey(
                        name: "FK_CosmeticShoppingCarts_Cosmetics_CosmeticId",
                        column: x => x.CosmeticId,
                        principalTable: "Cosmetics",
                        principalColumn: "CosmeticId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CosmeticShoppingCarts_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "ShoppingCartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicineShoppingCarts",
                columns: table => new
                {
                    MedicineId = table.Column<int>(nullable: false),
                    ShoppingCartId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineShoppingCarts", x => new { x.MedicineId, x.ShoppingCartId });
                    table.ForeignKey(
                        name: "FK_MedicineShoppingCarts_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "MedicineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicineShoppingCarts_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "ShoppingCartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notations",
                columns: table => new
                {
                    NotationId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BrandPreference = table.Column<int>(nullable: false),
                    HasPregnancy = table.Column<bool>(nullable: false),
                    HasOtherDiseases = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ShoppingCartId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notations", x => x.NotationId);
                    table.ForeignKey(
                        name: "FK_Notations_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "ShoppingCartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    PrescriptionId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HasBeenAcceptedByPharmacy = table.Column<bool>(nullable: false),
                    PictureUrl_1 = table.Column<string>(nullable: true),
                    PictureUrl_2 = table.Column<string>(nullable: true),
                    PictureUrl_3 = table.Column<string>(nullable: true),
                    PicturesUploadDate = table.Column<DateTime>(nullable: false),
                    ShoppingCartId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.PrescriptionId);
                    table.ForeignKey(
                        name: "FK_Prescriptions_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "ShoppingCartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceMappings",
                columns: table => new
                {
                    ServiceMappingId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FoundPharmacies = table.Column<string>(nullable: true),
                    PrimativePharmacyId = table.Column<int>(nullable: false),
                    PharmacyServiceStatus = table.Column<int>(nullable: false),
                    PharmacyCancellationReason = table.Column<int>(nullable: false),
                    PharmacyCancellationDate = table.Column<DateTime>(nullable: false),
                    CustomerCancellation = table.Column<bool>(nullable: false),
                    CustomerCancellationReason = table.Column<int>(nullable: false),
                    CustomerCancellationDate = table.Column<DateTime>(nullable: false),
                    ShoppingCartId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceMappings", x => x.ServiceMappingId);
                    table.ForeignKey(
                        name: "FK_ServiceMappings_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "ShoppingCartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IBAN = table.Column<string>(nullable: true),
                    AccountOwnerName = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    PharmacyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Accounts_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "PharmacyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UOI = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    HasPrescription = table.Column<bool>(nullable: false),
                    HasBeenSetToACourier = table.Column<bool>(nullable: false),
                    HasBeenAcceptedByCustomer = table.Column<bool>(nullable: false),
                    HasBeenPaid = table.Column<bool>(nullable: false),
                    HasBeenReceivedByCustomer = table.Column<bool>(nullable: false),
                    HasBeenDeliveredToCustomer = table.Column<bool>(nullable: false),
                    HasBeenSettled = table.Column<bool>(nullable: false),
                    SettlementDate = table.Column<DateTime>(nullable: false),
                    PackingTime = table.Column<int>(nullable: false),
                    ShoppingCartId = table.Column<int>(nullable: false),
                    CourierId = table.Column<int>(nullable: false),
                    PharmacyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Couriers_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Couriers",
                        principalColumn: "CourierId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "PharmacyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "ShoppingCartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PharmacyNotificationMap",
                columns: table => new
                {
                    PharmacyNotificationMapId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConnectionID = table.Column<string>(nullable: true),
                    Connected = table.Column<bool>(nullable: false),
                    PharmacyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyNotificationMap", x => x.PharmacyNotificationMapId);
                    table.ForeignKey(
                        name: "FK_PharmacyNotificationMap_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "PharmacyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PharmacyTokens",
                columns: table => new
                {
                    PharmacyTokenId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Token = table.Column<string>(nullable: true),
                    IsValid = table.Column<bool>(nullable: false),
                    TokenRefreshRequests = table.Column<uint>(nullable: false),
                    LoginRequests = table.Column<uint>(nullable: false),
                    ValidFrom = table.Column<DateTime>(nullable: false),
                    ValidTo = table.Column<DateTime>(nullable: false),
                    IsAvailableInSignalR = table.Column<bool>(nullable: false),
                    PharmacyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyTokens", x => x.PharmacyTokenId);
                    table.ForeignKey(
                        name: "FK_PharmacyTokens_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "PharmacyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    ScoreId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerSatisfaction = table.Column<int>(nullable: false),
                    RankAmongPharmacies = table.Column<int>(nullable: false),
                    PackingAverageTimeInSeconds = table.Column<int>(nullable: false),
                    PharmacyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.ScoreId);
                    table.ForeignKey(
                        name: "FK_Scores_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "PharmacyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Settles",
                columns: table => new
                {
                    SettleId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    USI = table.Column<string>(nullable: true),
                    CommissionCoefficient = table.Column<double>(nullable: false),
                    NumberOfOrders = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    PharmacyId = table.Column<int>(nullable: false),
                    HasBeenSettled = table.Column<bool>(nullable: false),
                    Credit = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settles", x => x.SettleId);
                    table.ForeignKey(
                        name: "FK_Settles_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "PharmacyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionItems",
                columns: table => new
                {
                    PrescriptionItemId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    PrescriptionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionItems", x => x.PrescriptionItemId);
                    table.ForeignKey(
                        name: "FK_PrescriptionItems_Prescriptions_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescriptions",
                        principalColumn: "PrescriptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Occurrences",
                columns: table => new
                {
                    OccurrenceId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerHasRequestedOrderOn = table.Column<DateTime>(nullable: false),
                    PharmacyHasAcceptedOrderOn = table.Column<DateTime>(nullable: false),
                    CustomerHasFinallyAcceptedOrderOn = table.Column<DateTime>(nullable: false),
                    PharmacyHasPackedOrderOn = table.Column<DateTime>(nullable: false),
                    CourierHasReceivedOrderFromPharmacyOn = table.Column<DateTime>(nullable: false),
                    CourierHasDeliveredOrderToCustomerOn = table.Column<DateTime>(nullable: false),
                    CustomerHasReceivedOrderFromCourierOn = table.Column<DateTime>(nullable: false),
                    OrderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Occurrences", x => x.OccurrenceId);
                    table.ForeignKey(
                        name: "FK_Occurrences_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_PharmacyId",
                table: "Accounts",
                column: "PharmacyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CosmeticShoppingCarts_ShoppingCartId",
                table: "CosmeticShoppingCarts",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerNotificationMap_CustomerId",
                table: "CustomerNotificationMap",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTokens_CustomerId",
                table: "CustomerTokens",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicineShoppingCarts_ShoppingCartId",
                table: "MedicineShoppingCarts",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_Notations_ShoppingCartId",
                table: "Notations",
                column: "ShoppingCartId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Occurrences_OrderId",
                table: "Occurrences",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CourierId",
                table: "Orders",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PharmacyId",
                table: "Orders",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShoppingCartId",
                table: "Orders",
                column: "ShoppingCartId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_ManagerId",
                table: "Pharmacies",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyNotificationMap_PharmacyId",
                table: "PharmacyNotificationMap",
                column: "PharmacyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyTokens_PharmacyId",
                table: "PharmacyTokens",
                column: "PharmacyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionItems_PrescriptionId",
                table: "PrescriptionItems",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_ShoppingCartId",
                table: "Prescriptions",
                column: "ShoppingCartId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scores_PharmacyId",
                table: "Scores",
                column: "PharmacyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceMappings_ShoppingCartId",
                table: "ServiceMappings",
                column: "ShoppingCartId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Settles_PharmacyId",
                table: "Settles",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_CustomerId",
                table: "ShoppingCarts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TextMessages_CustomerId",
                table: "TextMessages",
                column: "CustomerId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "CosmeticShoppingCarts");

            migrationBuilder.DropTable(
                name: "CustomerNotificationMap");

            migrationBuilder.DropTable(
                name: "CustomerTokens");

            migrationBuilder.DropTable(
                name: "MedicineShoppingCarts");

            migrationBuilder.DropTable(
                name: "Notations");

            migrationBuilder.DropTable(
                name: "Occurrences");

            migrationBuilder.DropTable(
                name: "PharmacyNotificationMap");

            migrationBuilder.DropTable(
                name: "PharmacyTokens");

            migrationBuilder.DropTable(
                name: "PrescriptionItems");

            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropTable(
                name: "ServiceMappings");

            migrationBuilder.DropTable(
                name: "Settles");

            migrationBuilder.DropTable(
                name: "TextMessages");

            migrationBuilder.DropTable(
                name: "Cosmetics");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropTable(
                name: "Couriers");

            migrationBuilder.DropTable(
                name: "Pharmacies");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
