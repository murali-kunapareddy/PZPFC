using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PFCWebAPP.Migrations
{
    /// <inheritdoc />
    public partial class CreatingCustomTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "MST_AppConfig",
                schema: "dbo",
                columns: table => new
                {
                    AppConfigID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfigName = table.Column<string>(type: "varchar(50)", nullable: false),
                    AliasName = table.Column<string>(type: "varchar(50)", nullable: false, defaultValueSql: "('')"),
                    Description = table.Column<string>(type: "varchar(200)", nullable: false, defaultValueSql: "('')"),
                    ConfigValue = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    ConfigType = table.Column<string>(type: "varchar(50)", nullable: false, defaultValueSql: "('')"),
                    ConfigDataType = table.Column<string>(type: "varchar(10)", nullable: false, defaultValueSql: "('')"),
                    ConfigUIType = table.Column<string>(type: "varchar(50)", nullable: false, defaultValueSql: "('')"),
                    ConfigMinLength = table.Column<int>(type: "int", nullable: false),
                    ConfigMaxLength = table.Column<int>(type: "int", nullable: false),
                    SequenceNo = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MST_AppConfig", x => x.AppConfigID);
                });

            migrationBuilder.CreateTable(
                name: "MST_ConfigOptions",
                schema: "dbo",
                columns: table => new
                {
                    ConfigOptionID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfigType = table.Column<string>(type: "varchar(50)", nullable: false),
                    ConfigValue = table.Column<string>(type: "varchar(100)", nullable: false),
                    SequenceNo = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MST_ConfigOptions", x => x.ConfigOptionID);
                });

            migrationBuilder.CreateTable(
                name: "MST_CustomerContact",
                schema: "dbo",
                columns: table => new
                {
                    CustomerContactID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    AccountName = table.Column<string>(type: "varchar(250)", nullable: false, defaultValueSql: "('')"),
                    ContactPerson = table.Column<string>(type: "varchar(250)", nullable: false, defaultValueSql: "('')"),
                    ToEmailID = table.Column<string>(type: "varchar(1024)", nullable: false, defaultValueSql: "('')"),
                    CcEmailID = table.Column<string>(type: "varchar(1024)", nullable: false, defaultValueSql: "('')"),
                    BccEmailID = table.Column<string>(type: "varchar(1024)", nullable: false, defaultValueSql: "('')"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MST_CustomerContact", x => x.CustomerContactID);
                });

            migrationBuilder.CreateTable(
                name: "MST_MaterialMaster",
                schema: "dbo",
                columns: table => new
                {
                    MaterialMasterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Prefix = table.Column<string>(type: "varchar(3)", nullable: false),
                    ColourCode = table.Column<string>(type: "varchar(50)", nullable: false, defaultValueSql: "('')"),
                    CatNo = table.Column<string>(type: "varchar(50)", nullable: false),
                    ItemNo = table.Column<string>(type: "varchar(50)", nullable: false),
                    InternalSAPItemNo = table.Column<string>(type: "varchar(50)", nullable: false),
                    SplitPackQty = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MST_MaterialMaster", x => x.MaterialMasterID);
                });

            migrationBuilder.CreateTable(
                name: "MST_Menus",
                schema: "dbo",
                columns: table => new
                {
                    MenuID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ParentID = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    ControllerName = table.Column<string>(type: "varchar(50)", nullable: false),
                    ActionName = table.Column<string>(type: "varchar(50)", nullable: false),
                    AliasName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    HrefVal = table.Column<string>(type: "varchar(50)", nullable: false, defaultValueSql: "('')"),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    CanShowMenu = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MST_Menus", x => x.MenuID);
                });

            migrationBuilder.CreateTable(
                name: "MST_NotificationTemplate",
                schema: "dbo",
                columns: table => new
                {
                    NotificationTemplateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesOrganization = table.Column<string>(type: "varchar(10)", nullable: false, defaultValueSql: "('')"),
                    TemplateName = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    TemplateSubject = table.Column<string>(type: "varchar(1024)", nullable: false, defaultValueSql: "('')"),
                    TemplateBody = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValueSql: "('')"),
                    TemplateVars = table.Column<string>(type: "varchar(1000)", nullable: false, defaultValueSql: "('')"),
                    DefaultSentTo = table.Column<string>(type: "varchar(max)", nullable: false, defaultValueSql: "('')"),
                    DefaultCcTo = table.Column<string>(type: "varchar(max)", nullable: false, defaultValueSql: "('')"),
                    DefaultBccTo = table.Column<string>(type: "varchar(max)", nullable: false, defaultValueSql: "('')"),
                    Priority = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MST_NotificationTemplate", x => x.NotificationTemplateID);
                });

            migrationBuilder.CreateTable(
                name: "MST_ReportFormatFieldMaster",
                schema: "dbo",
                columns: table => new
                {
                    ReportFormatFieldMasterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    FieldDescription = table.Column<string>(type: "nvarchar(100)", nullable: false, defaultValueSql: "('')"),
                    DataType = table.Column<string>(type: "varchar(50)", nullable: false, defaultValueSql: "('')"),
                    AlignmentType = table.Column<string>(type: "varchar(50)", nullable: false, defaultValueSql: "('')"),
                    ColorCode = table.Column<string>(type: "varchar(50)", nullable: false, defaultValueSql: "('')"),
                    SequenceNo = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MST_ReportFormatFieldMaster", x => x.ReportFormatFieldMasterID);
                });

            migrationBuilder.CreateTable(
                name: "MST_ReportFormatMaster",
                schema: "dbo",
                columns: table => new
                {
                    ReportFormatMasterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormatName = table.Column<string>(type: "varchar(50)", nullable: false),
                    AliasName = table.Column<string>(type: "varchar(50)", nullable: false, defaultValueSql: "('')"),
                    CountryCode = table.Column<string>(type: "varchar(2)", nullable: false, defaultValueSql: "('00')"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MST_ReportFormatMaster", x => x.ReportFormatMasterID);
                });

            migrationBuilder.CreateTable(
                name: "MST_Roles",
                schema: "dbo",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MST_Roles", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "MST_TemplateCategory",
                schema: "dbo",
                columns: table => new
                {
                    TemplateCategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "varchar(50)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MST_TemplateCategory", x => x.TemplateCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "MST_UserMaster",
                schema: "dbo",
                columns: table => new
                {
                    UserMasterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserSESA = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    FirstName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, defaultValueSql: "('')"),
                    Email = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false, defaultValueSql: "('')"),
                    Country = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MST_UserMaster", x => x.UserMasterID);
                });

            migrationBuilder.CreateTable(
                name: "TRN_NLog",
                schema: "dbo",
                columns: table => new
                {
                    NLogID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValueSql: "('')"),
                    Logged = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    Level = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false, defaultValueSql: "('')"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValueSql: "('')"),
                    Logger = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false, defaultValueSql: "('')"),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValueSql: "('')"),
                    Callsite = table.Column<string>(type: "nvarchar(300)", nullable: false, defaultValueSql: "('')"),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValueSql: "('')"),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValueSql: "('')"),
                    ThreadID = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRN_NLog", x => x.NLogID);
                });

            migrationBuilder.CreateTable(
                name: "TRN_NotificationHistory",
                schema: "dbo",
                columns: table => new
                {
                    NotificationHistoryID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    Subject = table.Column<string>(type: "varchar(1024)", nullable: false, defaultValueSql: "('')"),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValueSql: "('')"),
                    SentTo = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValueSql: "('')"),
                    CcTo = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValueSql: "('')"),
                    BccTo = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValueSql: "('')"),
                    Priority = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    Status = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    StatusDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    AttachmentPath = table.Column<string>(type: "nvarchar(1024)", nullable: false, defaultValueSql: "('')"),
                    ActualFileName = table.Column<string>(type: "nvarchar(250)", nullable: false, defaultValueSql: "('')"),
                    EncryptedFileName = table.Column<string>(type: "nvarchar(100)", nullable: false, defaultValueSql: "('')"),
                    NotificationTemplateID = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    PriceFileHeaderID = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "0"),
                    PriceFileLocationID = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "0"),
                    ResendCount = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "0"),
                    ResendStatus = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRN_NotificationHistory", x => x.NotificationHistoryID);
                });

            migrationBuilder.CreateTable(
                name: "TRN_PriceFileHeader",
                schema: "dbo",
                columns: table => new
                {
                    PriceFileHeaderID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserConfigSettingID = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    StatusText = table.Column<string>(type: "varchar(512)", nullable: false, defaultValueSql: "('')"),
                    PercentCompleted = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRN_PriceFileHeader", x => x.PriceFileHeaderID);
                });

            migrationBuilder.CreateTable(
                name: "TRN_PriceFileLog",
                schema: "dbo",
                columns: table => new
                {
                    PriceFileLogID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriceFileHeaderID = table.Column<int>(type: "int", nullable: false),
                    LogType = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    FunctionName = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    LogInformation = table.Column<string>(type: "varchar(max)", nullable: false, defaultValueSql: "('')"),
                    LogReference1 = table.Column<string>(type: "varchar(max)", nullable: false, defaultValueSql: "('')"),
                    LogReference2 = table.Column<string>(type: "varchar(max)", nullable: false, defaultValueSql: "('')"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRN_PriceFileLog", x => x.PriceFileLogID);
                });

            migrationBuilder.CreateTable(
                name: "TRN_UserConfigSetting",
                schema: "dbo",
                columns: table => new
                {
                    UserConfigSettingID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserSESA = table.Column<string>(type: "varchar(30)", nullable: false),
                    SalesOrganization = table.Column<string>(type: "varchar(10)", nullable: false, defaultValueSql: "('')"),
                    SelectedCustomers = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValueSql: "('')"),
                    PricesActiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    CanUseAutoReportContent = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    ReportContentTemplateID = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "0"),
                    ReportFormatTemplateID = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "0"),
                    SelectedCustomersTemplateID = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "0"),
                    CanIncludeTradePrices = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanIncludeCustomerNetPrices = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanIncludeCustomerHierarchyNetPrices = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanIncludeOverallNetPrices = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanIncludePriceGroupNets = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanIncludeSellOffPrices = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanIncludeDiscount1 = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanIncludeDiscount2 = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanIncludeDiscount3 = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanIncludeDiscount4 = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanIncludeDiscount5 = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanIncludeDiscount6 = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanIncludeDiscount7 = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanIncludeDiscount8 = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanIncludePromoPrice = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanUseShiftBreaks = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanUseMOQAsBrk1 = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanUseGlobalCOSForProductHierarchy = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanUseLocalCOSForProductHierarchy = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanAddSODInFinalPrice = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    SODInFinalPriceValue = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    CanUseAlternateValidFromDate = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    AlternateValidFromDate = table.Column<DateTime>(type: "date", nullable: true),
                    CanShowTemplateMaterialOnly = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanSendEmail = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanShowNotFoundTemplateMaterials = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRN_UserConfigSetting", x => x.UserConfigSettingID);
                });

            migrationBuilder.CreateTable(
                name: "TRN_UserLog",
                schema: "dbo",
                columns: table => new
                {
                    UserLogID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserSESA = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    AttemptedOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    IPAddress = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false, defaultValueSql: "('')"),
                    MachineName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValueSql: "('')"),
                    OperatingSystem = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValueSql: "('')"),
                    UserHostAddress = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, defaultValueSql: "('')"),
                    UserAgent = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRN_UserLog", x => x.UserLogID);
                });

            migrationBuilder.CreateTable(
                name: "MST_ReportFormatFieldMapping",
                schema: "dbo",
                columns: table => new
                {
                    ReportFormatFieldMappingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportFormatMasterID = table.Column<int>(type: "int", nullable: false),
                    ReportFormatFieldMasterID = table.Column<int>(type: "int", nullable: false),
                    AliasName = table.Column<string>(type: "nvarchar(100)", nullable: false, defaultValueSql: "('')"),
                    SequenceNo = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MST_ReportFormatFieldMapping", x => x.ReportFormatFieldMappingID);
                    table.ForeignKey(
                        name: "FK_MST_ReportFormatFieldMapping_MST_ReportFormatFieldMaster_ReportFormatFieldMasterID",
                        column: x => x.ReportFormatFieldMasterID,
                        principalSchema: "dbo",
                        principalTable: "MST_ReportFormatFieldMaster",
                        principalColumn: "ReportFormatFieldMasterID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MST_ReportFormatFieldMapping_MST_ReportFormatMaster_ReportFormatMasterID",
                        column: x => x.ReportFormatMasterID,
                        principalSchema: "dbo",
                        principalTable: "MST_ReportFormatMaster",
                        principalColumn: "ReportFormatMasterID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MST_RoleMenus",
                schema: "dbo",
                columns: table => new
                {
                    RoleMenuID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    MenuID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MST_RoleMenus", x => x.RoleMenuID);
                    table.ForeignKey(
                        name: "FK_MST_RoleMenus_MST_Menus_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "dbo",
                        principalTable: "MST_Menus",
                        principalColumn: "MenuID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MST_RoleMenus_MST_Roles_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "dbo",
                        principalTable: "MST_Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MST_UserRoleMapping",
                schema: "dbo",
                columns: table => new
                {
                    UserRoleMappingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserSESA = table.Column<string>(type: "varchar(15)", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MST_UserRoleMapping", x => x.UserRoleMappingID);
                    table.ForeignKey(
                        name: "FK_MST_UserRoleMapping_MST_Roles_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "dbo",
                        principalTable: "MST_Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MST_TemplateMaster",
                schema: "dbo",
                columns: table => new
                {
                    TemplateMasterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateCategoryID = table.Column<int>(type: "int", nullable: false),
                    TemplateName = table.Column<string>(type: "varchar(50)", nullable: false),
                    AliasName = table.Column<string>(type: "varchar(50)", nullable: false, defaultValueSql: "('')"),
                    TemplateDataModel = table.Column<string>(type: "varchar(5)", nullable: false, defaultValueSql: "('JSON')"),
                    CountryCode = table.Column<string>(type: "varchar(2)", nullable: false, defaultValueSql: "('00')"),
                    CanDuplicate = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CanUpload = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    CanEdit = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MST_TemplateMaster", x => x.TemplateMasterID);
                    table.ForeignKey(
                        name: "FK_MST_TemplateMaster_MST_TemplateCategory_TemplateCategoryID",
                        column: x => x.TemplateCategoryID,
                        principalSchema: "dbo",
                        principalTable: "MST_TemplateCategory",
                        principalColumn: "TemplateCategoryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TRN_PriceFileDetails",
                schema: "dbo",
                columns: table => new
                {
                    PriceFileDetailID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriceFileHeaderID = table.Column<long>(type: "bigint", nullable: false),
                    CustomerNo = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    Prefix = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CustomerCatNo = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    ColourCode = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CustomerItemNo = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    SchneiderElectricMaterialReference = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    MaterialDescription = table.Column<string>(type: "varchar(500)", nullable: false, defaultValueSql: "('')"),
                    WholesaleListPriceExclGST = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    WholesaleListPriceInclGST = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    Per = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    UOM = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "0"),
                    MOQ = table.Column<int>(type: "int", nullable: false),
                    OrderMultiple = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    RecommendedRetailPrice = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    AdvertisedRecommendedRetailPrice = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    PriceDerivedFrom = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    PriceBreak1CustomerQty = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    PriceBreak1CustomerDiscount = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    PriceBreak1CustomerCostExclGST = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    PriceBreak1CustomerCostInclGST = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    PriceBreak2CustomerQty = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    PriceBreak2CustomerDiscount = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    PriceBreak2CustomerCostExclGST = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    PriceBreak2CustomerCostInclGST = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    PriceBreak3CustomerQty = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    PriceBreak3CustomerDiscount = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    PriceBreak3CustomerCostExclGST = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    PriceBreak3CustomerCostInclGST = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    PriceBreak4CustomerQty = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    PriceBreak4CustomerDiscount = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    PriceBreak4CustomerCostExclGST = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    PriceBreak4CustomerCostInclGST = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    PriceBreak5CustomerQty = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    PriceBreak5CustomerDiscount = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    PriceBreak5CustomerCostExclGST = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    PriceBreak5CustomerCostInclGST = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    Barcode = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    ProductHierarchy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    SAPCOS = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CartonQty = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    StockStatus = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    ValidFrom = table.Column<DateTime>(type: "datetime", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    FileReferenceData = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    Currency = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    VRG = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    VRGDescription = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    MaterialStatus = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    MainGroup = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    MainGroupDescription = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    Group = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    GroupDescription = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    SubGroup = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    SubGroupDescription = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    IsFound = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRN_PriceFileDetails", x => x.PriceFileDetailID);
                    table.ForeignKey(
                        name: "FK_TRN_PriceFileDetails_TRN_PriceFileHeader_PriceFileHeaderID",
                        column: x => x.PriceFileHeaderID,
                        principalSchema: "dbo",
                        principalTable: "TRN_PriceFileHeader",
                        principalColumn: "PriceFileHeaderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TRN_PriceFileLocationDetails",
                schema: "dbo",
                columns: table => new
                {
                    PriceFileLocationID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriceFileHeaderID = table.Column<long>(type: "bigint", nullable: false),
                    CustomerNo = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    PFCActualFileName = table.Column<string>(type: "nvarchar(250)", nullable: false, defaultValueSql: "('')"),
                    PFCEncryptedFileName = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    PFCFileLocationMode = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    PFCFileType = table.Column<string>(type: "varchar(20)", nullable: false, defaultValueSql: "('')"),
                    PFCFilePath = table.Column<string>(type: "varchar(250)", nullable: false, defaultValueSql: "('')"),
                    PFCFileSize = table.Column<string>(type: "varchar(50)", nullable: false, defaultValueSql: "('')"),
                    Status = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    StatusText = table.Column<string>(type: "varchar(512)", nullable: false, defaultValueSql: "('')"),
                    PercentCompleted = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    ReDownloadCount = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    ReDownloadStatus = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    ReDownloadStatusText = table.Column<string>(type: "varchar(512)", nullable: false, defaultValueSql: "('')"),
                    ReDownloadPercentCompleted = table.Column<double>(type: "float", nullable: false, defaultValueSql: "0"),
                    IsReDownloadCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRN_PriceFileLocationDetails", x => x.PriceFileLocationID);
                    table.ForeignKey(
                        name: "FK_TRN_PriceFileLocationDetails_TRN_PriceFileHeader_PriceFileHeaderID",
                        column: x => x.PriceFileHeaderID,
                        principalSchema: "dbo",
                        principalTable: "TRN_PriceFileHeader",
                        principalColumn: "PriceFileHeaderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MST_TemplateData",
                schema: "dbo",
                columns: table => new
                {
                    TemplateDataID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateMasterID = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    ModifiedBy = table.Column<string>(type: "varchar(100)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MST_TemplateData", x => x.TemplateDataID);
                    table.ForeignKey(
                        name: "FK_MST_TemplateData_MST_TemplateMaster_TemplateMasterID",
                        column: x => x.TemplateMasterID,
                        principalSchema: "dbo",
                        principalTable: "MST_TemplateMaster",
                        principalColumn: "TemplateMasterID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MST_TemplateStructure",
                schema: "dbo",
                columns: table => new
                {
                    TemplateStructureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateMasterID = table.Column<int>(type: "int", nullable: false),
                    PropertyName = table.Column<string>(type: "varchar(50)", nullable: false),
                    PropertyDescription = table.Column<string>(type: "varchar(250)", nullable: false, defaultValueSql: "('')"),
                    PropertyDataType = table.Column<string>(type: "varchar(50)", nullable: false, defaultValueSql: "('JSON')"),
                    SequenceNo = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false, defaultValueSql: "('')"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MST_TemplateStructure", x => x.TemplateStructureID);
                    table.ForeignKey(
                        name: "FK_MST_TemplateStructure_MST_TemplateMaster_TemplateMasterID",
                        column: x => x.TemplateMasterID,
                        principalSchema: "dbo",
                        principalTable: "MST_TemplateMaster",
                        principalColumn: "TemplateMasterID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "MST_AppConfig",
                columns: new[] { "AppConfigID", "AliasName", "ConfigDataType", "ConfigMaxLength", "ConfigMinLength", "ConfigName", "ConfigType", "ConfigUIType", "ConfigValue", "Description", "IsActive", "SequenceNo" },
                values: new object[,]
                {
                    { 1, "Bulk Update Records Count", "NUMBER", 6, 4, "BulkUpdateRecordCount", "BulkUpdate", "DropDownList", "5000", "Bulk Update while saving data into Database", true, 1 },
                    { 2, "Display Max Records", "NUMBER", 6, 4, "DisplayMaxRecords", "DisplayMaxRecords", "DropDownList", "5000", "Display Max Records in a Grid", true, 2 },
                    { 3, "Select Max Customers", "NUMBER", 3, 1, "SelectMaxCustomers", "MaxCustomers", "DropDownList", "10", "Select Max Customers for Price File Generation", true, 3 },
                    { 4, "Archived File Location", "STRING", 50, 3, "ArchivedFileLocationMode", "ArchivedFileLocationMode", "DropDownList", "ApplicationServer", "Archived File Location Mode", true, 4 },
                    { 5, "Archived File Extraction", "STRING", 50, 3, "ArchivedFileExtractionMode", "ArchivedFileExtractionMode", "DropDownList", "ArchivedFileLocation", "Archived File Location Mode", true, 5 },
                    { 6, "View UserLog", "STRING", 3, 2, "ViewUserLog", "YesNo", "DropDownList", "Yes", "View UserLog information", true, 6 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "MST_ConfigOptions",
                columns: new[] { "ConfigOptionID", "ConfigType", "ConfigValue", "IsActive", "SequenceNo" },
                values: new object[,]
                {
                    { 1L, "LogType", "Trace", true, 1 },
                    { 2L, "LogType", "Debug", true, 2 },
                    { 3L, "LogType", "Info", true, 3 },
                    { 4L, "LogType", "Warn", true, 4 },
                    { 5L, "LogType", "Error", true, 5 },
                    { 6L, "LogType", "Fatal", true, 6 },
                    { 7L, "LogType", "Off", true, 7 },
                    { 8L, "BulkUpdate", "500", true, 1 },
                    { 9L, "BulkUpdate", "1000", true, 2 },
                    { 10L, "BulkUpdate", "5000", true, 3 },
                    { 11L, "BulkUpdate", "10000", true, 4 },
                    { 12L, "BulkUpdate", "15000", true, 5 },
                    { 13L, "BulkUpdate", "20000", true, 6 },
                    { 14L, "DisplayMaxRecords", "500", true, 1 },
                    { 15L, "DisplayMaxRecords", "1000", true, 2 },
                    { 16L, "DisplayMaxRecords", "5000", true, 3 },
                    { 17L, "DisplayMaxRecords", "10000", true, 4 },
                    { 18L, "DisplayMaxRecords", "15000", true, 5 },
                    { 19L, "DisplayMaxRecords", "20000", true, 6 },
                    { 20L, "YesNo", "Yes", true, 1 },
                    { 21L, "YesNo", "No", true, 2 },
                    { 22L, "MaxCustomers", "5", true, 1 },
                    { 23L, "MaxCustomers", "10", true, 2 },
                    { 24L, "MaxCustomers", "15", true, 3 },
                    { 25L, "MaxCustomers", "20", true, 4 },
                    { 26L, "MaxCustomers", "25", true, 5 },
                    { 27L, "MaxCustomers", "30", true, 6 },
                    { 28L, "MaxCustomers", "35", true, 7 },
                    { 29L, "MaxCustomers", "40", true, 8 },
                    { 30L, "MaxCustomers", "45", true, 9 },
                    { 31L, "MaxCustomers", "50", true, 10 },
                    { 32L, "MaxRetry", "1", true, 1 },
                    { 33L, "MaxRetry", "2", true, 2 },
                    { 34L, "MaxRetry", "3", true, 3 },
                    { 35L, "MaxRetry", "4", true, 4 },
                    { 36L, "MaxRetry", "5", true, 5 },
                    { 37L, "MaxRetry", "6", true, 6 },
                    { 38L, "MaxRetry", "7", true, 7 },
                    { 39L, "MaxRetry", "8", true, 8 },
                    { 40L, "MaxRetry", "9", true, 9 },
                    { 41L, "MaxRetry", "10", true, 10 },
                    { 42L, "ArchivedFileLocationMode", "ApplicationServer", true, 1 },
                    { 43L, "ArchivedFileLocationMode", "AWSS3Bucket", true, 2 },
                    { 44L, "ArchivedFileExtractionMode", "ArchivedFileLocation", true, 1 },
                    { 45L, "ArchivedFileExtractionMode", "DataBase", true, 2 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "MST_Menus",
                columns: new[] { "MenuID", "ActionName", "AliasName", "CanShowMenu", "ControllerName", "HrefVal", "IsActive", "MenuName", "SortOrder" },
                values: new object[,]
                {
                    { 1, "GeneratePriceFile", "Price List", true, "PriceList", "", true, "PriceList", 1 },
                    { 2, "", "Templates", true, "", "", true, "Templates", 2 },
                    { 3, "Masters", "Masters", true, "Configure", "", true, "Masters", 3 },
                    { 4, "", "User Management", true, "", "", true, "UserManagement", 4 },
                    { 5, "ApplicationSettings", "Settings", true, "Configure", "", true, "Settings", 6 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "MST_Menus",
                columns: new[] { "MenuID", "ActionName", "AliasName", "CanShowMenu", "ControllerName", "HrefVal", "IsActive", "MenuName", "ParentID", "SortOrder" },
                values: new object[,]
                {
                    { 6, "Roles", "Roles", true, "BackOps", "", true, "Roles", 4, 1 },
                    { 7, "Users", "Users", true, "BackOps", "", true, "Users", 4, 2 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "MST_Menus",
                columns: new[] { "MenuID", "ActionName", "AliasName", "ControllerName", "HrefVal", "IsActive", "MenuName", "ParentID", "SortOrder" },
                values: new object[] { 8, "AddUser", "Add Users", "BackOps", "", true, "Add Users", 4, 4 });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "MST_Menus",
                columns: new[] { "MenuID", "ActionName", "AliasName", "CanShowMenu", "ControllerName", "HrefVal", "IsActive", "MenuName", "ParentID", "SortOrder" },
                values: new object[,]
                {
                    { 9, "UserRoleMapping", "User Role Mapping", true, "BackOps", "", true, "User Role Mapping", 4, 3 },
                    { 10, "ReportFormat", "Trade List Formats", true, "Configure", "", true, "Trade List Formats", 2, 1 },
                    { 11, "ReportContent", "Trade List Templates", true, "Configure", "", true, "Trade List Templates", 2, 2 },
                    { 12, "CustomerTemplates", "Customer Templates", true, "Configure", "", true, "Customer Templates", 2, 3 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "MST_Menus",
                columns: new[] { "MenuID", "ActionName", "AliasName", "CanShowMenu", "ControllerName", "HrefVal", "IsActive", "MenuName", "SortOrder" },
                values: new object[] { 13, "History", "Notifications", true, "Notification", "", true, "Notifications", 5 });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "MST_NotificationTemplate",
                columns: new[] { "NotificationTemplateID", "DefaultBccTo", "DefaultCcTo", "DefaultSentTo", "IsActive", "Priority", "SalesOrganization", "TemplateBody", "TemplateName", "TemplateSubject", "TemplateVars" },
                values: new object[,]
                {
                    { 1, "", "", "", true, 2, "AU01", "<p>Dear Customer,</p><p>The attached Price File have been digitally approved.</p><p>PLEASE DO NOT REPLY TO THIS AUTOMATICALLY GENERATED EMAIL. If you wish to reply, please remove this email address.</p><p>From <em>Notification Services</em><br /><strong>Price File Creator</strong></p>", "AU01PriceFileDistribution", "[NEW Price File] Customer_No: {{Customer_No}}  // Customer_Name: {{Customer_Name}}", "Customer_No, Customer_Name" },
                    { 2, "", "", "", true, 2, "NZ01", "<p>Dear Customer,</p><p>The attached Price File have been digitally approved.</p><p>PLEASE DO NOT REPLY TO THIS AUTOMATICALLY GENERATED EMAIL. If you wish to reply, please remove this email address.</p><p>From <em>Notification Services</em><br /><strong>Price File Creator</strong></p>", "NZ01PriceFileDistribution", "[NEW Price File] Customer_No: {{Customer_No}}  // Customer_Name: {{Customer_Name}}", "Customer_No, Customer_Name" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "MST_ReportFormatFieldMaster",
                columns: new[] { "ReportFormatFieldMasterID", "DataType", "FieldDescription", "FieldName", "IsActive", "SequenceNo" },
                values: new object[,]
                {
                    { 1, "VARCHAR", "Prefix", "Prefix", true, 1 },
                    { 2, "VARCHAR", "CustomerCatNo", "CustomerCatNo", true, 2 },
                    { 3, "VARCHAR", "ColourCode", "ColourCode", true, 3 },
                    { 4, "VARCHAR", "CustomerItemNo", "CustomerItemNo", true, 4 },
                    { 5, "VARCHAR", "SchneiderElectricMaterialReference", "SchneiderElectricMaterialReference", true, 5 },
                    { 6, "VARCHAR", "MaterialDescription", "MaterialDescription", true, 6 },
                    { 7, "float", "WholesaleListPriceExclGST", "WholesaleListPriceExclGST", true, 7 },
                    { 8, "float", "WholesaleListPriceInclGST", "WholesaleListPriceInclGST", true, 8 },
                    { 9, "float", "Per", "Per", true, 9 },
                    { 10, "VARCHAR", "UOM", "UOM", true, 10 },
                    { 11, "int", "MOQ_MinimumOrderQuantity", "MOQ", true, 11 },
                    { 12, "float", "OrderMultiple", "OrderMultiple", true, 12 },
                    { 13, "float", "RRP_RecommendedRetailPrice", "RecommendedRetailPrice", true, 13 },
                    { 14, "float", "ARRP_AdvertisedRecommendedRetailPrice", "AdvertisedRecommendedRetailPrice", true, 14 },
                    { 15, "VARCHAR", "PriceDerivedFrom", "PriceDerivedFrom", true, 15 },
                    { 16, "int", "PriceBreak1CustomerQty", "PriceBreak1CustomerQty", true, 16 },
                    { 17, "float", "PriceBreak1CustomerDiscount", "PriceBreak1CustomerDiscount", true, 17 },
                    { 18, "float", "PriceBreak1CustomerCostExclGST", "PriceBreak1CustomerCostExclGST", true, 18 },
                    { 19, "float", "PriceBreak1CustomerCostInclGST", "PriceBreak1CustomerCostInclGST", true, 19 },
                    { 20, "int", "PriceBreak2CustomerQty", "PriceBreak2CustomerQty", true, 20 },
                    { 21, "float", "PriceBreak2CustomerDiscount", "PriceBreak2CustomerDiscount", true, 21 },
                    { 22, "float", "PriceBreak2CustomerCostExclGST", "PriceBreak2CustomerCostExclGST", true, 22 },
                    { 23, "float", "PriceBreak2CustomerCostInclGST", "PriceBreak2CustomerCostInclGST", true, 23 },
                    { 24, "int", "PriceBreak3CustomerQty", "PriceBreak3CustomerQty", true, 24 },
                    { 25, "float", "PriceBreak3CustomerDiscount", "PriceBreak3CustomerDiscount", true, 25 },
                    { 26, "float", "PriceBreak3CustomerCostExclGST", "PriceBreak3CustomerCostExclGST", true, 26 },
                    { 27, "float", "PriceBreak3CustomerCostInclGST", "PriceBreak3CustomerCostInclGST", true, 27 },
                    { 28, "int", "PriceBreak4CustomerQty", "PriceBreak4CustomerQty", true, 28 },
                    { 29, "float", "PriceBreak4CustomerDiscount", "PriceBreak4CustomerDiscount", true, 29 },
                    { 30, "float", "PriceBreak4CustomerCostExclGST", "PriceBreak4CustomerCostExclGST", true, 30 },
                    { 31, "float", "PriceBreak4CustomerCostInclGST", "PriceBreak4CustomerCostInclGST", true, 31 },
                    { 32, "int", "PriceBreak5CustomerQty", "PriceBreak5CustomerQty", true, 32 },
                    { 33, "float", "PriceBreak5CustomerDiscount", "PriceBreak5CustomerDiscount", true, 33 },
                    { 34, "float", "PriceBreak5CustomerCostExclGST", "PriceBreak5CustomerCostExclGST", true, 34 },
                    { 35, "float", "PriceBreak5CustomerCostInclGST", "PriceBreak5CustomerCostInclGST", true, 35 },
                    { 36, "VARCHAR", "Barcode", "Barcode", true, 36 },
                    { 37, "VARCHAR", "SAP COS", "SAP COS", true, 37 },
                    { 38, "VARCHAR", "CartonQty", "CartonQty", true, 38 },
                    { 39, "VARCHAR", "StockStatus", "StockStatus", true, 39 },
                    { 40, "Date", "ValidFrom", "ValidFrom", true, 40 },
                    { 41, "Date", "ValidTo", "ValidTo", true, 41 },
                    { 42, "VARCHAR", "FileReferenceData", "FileReferenceData", true, 42 },
                    { 43, "VARCHAR", "Currency", "Currency", true, 43 },
                    { 44, "VARCHAR", "VRG", "VRG", true, 44 },
                    { 45, "VARCHAR", "VRGDescription", "VRGDescription", true, 45 },
                    { 46, "VARCHAR", "MaterialStatus", "MaterialStatus", true, 46 },
                    { 47, "VARCHAR", "MainGroup", "MainGroup", true, 47 },
                    { 48, "VARCHAR", "MainGroupDescription", "MainGroupDescription", true, 48 },
                    { 49, "VARCHAR", "Group", "Group", true, 49 },
                    { 50, "VARCHAR", "GroupDescription", "GroupDescription", true, 50 },
                    { 51, "VARCHAR", "SubGroup", "SubGroup", true, 51 },
                    { 52, "VARCHAR", "SubGroupDescription", "SubGroupDescription", true, 52 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "MST_ReportFormatMaster",
                columns: new[] { "ReportFormatMasterID", "AliasName", "CountryCode", "FormatName", "IsActive" },
                values: new object[,]
                {
                    { 1, "AU Wholesaler Output Template", "AU", "AUWholesalerOutputTemplate", true },
                    { 2, "AU Other Channel Output Template", "AU", "AUOtherChannelOutputTemplate", true },
                    { 3, "NZ Rebate Customer Output Template", "NZ", "NZRebateCustomerOutputTemplate", true },
                    { 4, "NZ Non-Rebate Customer Output Template", "NZ", "NZNonRebateCustomerOutputTemplate", true }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "MST_Roles",
                columns: new[] { "RoleID", "IsActive", "RoleName", "SortOrder" },
                values: new object[,]
                {
                    { 1, true, "Admin", 1 },
                    { 2, true, "PrivilegedUser", 2 },
                    { 3, true, "User", 3 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "MST_TemplateCategory",
                columns: new[] { "TemplateCategoryID", "CategoryName", "IsActive" },
                values: new object[,]
                {
                    { 1, "ReportTradeListContent", true },
                    { 2, "MasterReferences", true },
                    { 3, "CustomerTemplates", true }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "MST_UserMaster",
                columns: new[] { "UserMasterID", "Country", "Department", "Email", "FirstName", "IsActive", "LastName", "UserSESA" },
                values: new object[,]
                {
                    { 1, "AU", "Schneider Digital", "robert.fusco@se.com", "Robert", true, "FUSCO", "SESA41591" },
                    { 2, "AU", "Data Management & Business Analysis", "jeff.smith@se.com", "Jeffrey", true, "SMITH", "SESA95046" },
                    { 3, "NZ", "Price Management", "Clayton.Hall@se.com", "Clayton", true, "HALL", "SESA121108" },
                    { 4, "IN", "Schneider Digital", "murali.kunapareddy@se.com", "Murali", true, "KUNAPAREDDY", "SESA432166" },
                    { 5, "IN", "Schneider Digital", "nareshkumar.challa@non.se.com", "Naresh", true, "CHALLA", "SESA658252" },
                    { 6, "IN", "Schneider Digital", "VenkataJayendranath.Yelleswarapu@se.com", "Venkata", true, "Jayendranath", "SESA512280" },
                    { 7, "IN", "Schneider Digital", "bhavana.adari@non.se.com", "Bhavana", true, "Adari", "SESA715213" },
                    { 8, "IN", "Schneider Digital", "shiva.komaparathi@non.se.com", "Shiva", true, "KOMAPARATHI", "SESA715214" },
                    { 9, "IN", "Schneider Digital", "Ram.Singh@se.com", "Ram", true, "SINGH", "SESA497078" },
                    { 10, "IN", "Schneider Digital", "prabhu.sekar@non.se.com", "Prabhu", true, "SEKAR", "SESA654946" },
                    { 11, "IN", "Schneider Digital", "murali.kunapareddy@se.com", "Murali", true, "KUNAPAREDDY", "ADM432166" },
                    { 12, "IN", "Schneider Digital", "nareshkumar.challa@non.se.com", "Naresh", true, "Challa", "ADM658252" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "MST_ReportFormatFieldMapping",
                columns: new[] { "ReportFormatFieldMappingID", "AliasName", "IsActive", "ReportFormatFieldMasterID", "ReportFormatMasterID", "SequenceNo" },
                values: new object[,]
                {
                    { 1, "Prefix", true, 1, 1, 1 },
                    { 2, "Customer Cat No", true, 2, 1, 2 },
                    { 3, "Colour Code", true, 3, 1, 3 },
                    { 4, "Customer Item No", true, 4, 1, 4 },
                    { 5, "Schneider Electric Material Reference", true, 5, 1, 5 },
                    { 6, "Description", true, 6, 1, 6 },
                    { 7, "Wholesale List Price (excl GST)", true, 7, 1, 7 },
                    { 8, "Wholesale List Price (incl GST)", true, 8, 1, 8 },
                    { 9, "Per", true, 9, 1, 9 },
                    { 10, "UOM", true, 10, 1, 10 },
                    { 11, "MOQ (Minimum Order Quantity)", true, 11, 1, 11 },
                    { 12, "Order Multiple", true, 12, 1, 12 },
                    { 13, "RRP (Recommended Retail Price)", true, 13, 1, 13 },
                    { 14, "ARRP (Advertised Recommended Retail Price, displayed on Clipsal.com)", true, 14, 1, 14 },
                    { 15, "Price derived from", true, 15, 1, 15 },
                    { 16, "Price Break 1 - CUSTOMER QTY", true, 16, 1, 16 },
                    { 17, "Price Break 1 - CUSTOMER Discount", true, 17, 1, 17 },
                    { 18, "Price Break 1 - CUSTOMER Cost (excl GST)", true, 18, 1, 18 },
                    { 19, "Price Break 1 - CUSTOMER Cost (incl GST)", true, 19, 1, 19 },
                    { 20, "Price Break 2 - CUSTOMER QTY", true, 20, 1, 20 },
                    { 21, "Price Break 2 - CUSTOMER Discount", true, 21, 1, 21 },
                    { 22, "Price Break 2 - CUSTOMER Cost (excl GST)", true, 22, 1, 22 },
                    { 23, "Price Break 2 - CUSTOMER Cost (incl GST)", true, 23, 1, 23 },
                    { 24, "Price Break 3 - CUSTOMER QTY", true, 24, 1, 24 },
                    { 25, "Price Break 3 - CUSTOMER Discount", true, 25, 1, 25 },
                    { 26, "Price Break 3 - CUSTOMER Cost (excl GST)", true, 26, 1, 26 },
                    { 27, "Price Break 3 - CUSTOMER Cost (incl GST)", true, 27, 1, 27 },
                    { 28, "Price Break 4 - CUSTOMER QTY", true, 28, 1, 28 },
                    { 29, "Price Break 4 - CUSTOMER Discount", true, 29, 1, 29 },
                    { 30, "Price Break 4 - CUSTOMER Cost (excl GST)", true, 30, 1, 30 },
                    { 31, "Price Break 4 - CUSTOMER Cost (incl GST)", true, 31, 1, 31 },
                    { 32, "Price Break 5 - CUSTOMER QTY", true, 32, 1, 32 },
                    { 33, "Price Break 5 - CUSTOMER Discount", true, 33, 1, 33 },
                    { 34, "Price Break 5 - CUSTOMER Cost (excl GST)", true, 34, 1, 34 },
                    { 35, "Price Break 5 - CUSTOMER Cost (incl GST)", true, 35, 1, 35 },
                    { 36, "Barcode", true, 36, 1, 36 },
                    { 37, "SAP Local COS", true, 37, 1, 37 },
                    { 38, "Carton Qty", true, 38, 1, 38 },
                    { 39, "Stock Status (S = Stockable, * = Not normally stocked in Australia)", true, 39, 1, 39 },
                    { 40, "Valid From", true, 40, 1, 40 },
                    { 41, "Valid To", true, 41, 1, 41 },
                    { 42, "File Reference Data", true, 42, 1, 42 },
                    { 43, "Schneider Electric Material Reference", true, 5, 2, 1 },
                    { 44, "Description", true, 6, 2, 2 },
                    { 45, "Wholesale List Price (excl GST)", true, 7, 2, 3 },
                    { 46, "Wholesale List Price (incl GST)", true, 8, 2, 4 },
                    { 47, "Per", true, 9, 2, 5 },
                    { 48, "UOM", true, 10, 2, 6 },
                    { 49, "MOQ (Minimum Order Quantity)", true, 11, 2, 7 },
                    { 50, "Order Multiple", true, 12, 2, 8 },
                    { 51, "Price derived from", true, 15, 2, 9 },
                    { 52, "Price Break 1 - CUSTOMER QTY", true, 16, 2, 10 },
                    { 53, "Price Break 1 - CUSTOMER Discount", true, 17, 2, 11 },
                    { 54, "Price Break 1 - CUSTOMER Cost (excl GST)", true, 18, 2, 12 },
                    { 55, "Price Break 1 - CUSTOMER Cost (incl GST)", true, 19, 2, 13 },
                    { 56, "Price Break 2 - CUSTOMER QTY", true, 20, 2, 14 },
                    { 57, "Price Break 2 - CUSTOMER Discount", true, 21, 2, 15 },
                    { 58, "Price Break 2 - CUSTOMER Cost (excl GST)", true, 22, 2, 16 },
                    { 59, "Price Break 2 - CUSTOMER Cost (incl GST)", true, 23, 2, 17 },
                    { 60, "Price Break 3 - CUSTOMER QTY", true, 24, 2, 18 },
                    { 61, "Price Break 3 - CUSTOMER Discount", true, 25, 2, 19 },
                    { 62, "Price Break 3 - CUSTOMER Cost (excl GST)", true, 26, 2, 20 },
                    { 63, "Price Break 3 - CUSTOMER Cost (incl GST)", true, 27, 2, 21 },
                    { 64, "Price Break 4 - CUSTOMER QTY", true, 28, 2, 22 },
                    { 65, "Price Break 4 - CUSTOMER Discount", true, 29, 2, 23 },
                    { 66, "Price Break 4 - CUSTOMER Cost (excl GST)", true, 30, 2, 24 },
                    { 67, "Price Break 4 - CUSTOMER Cost (incl GST)", true, 31, 2, 25 },
                    { 68, "Price Break 5 - CUSTOMER QTY", true, 32, 2, 26 },
                    { 69, "Price Break 5 - CUSTOMER Discount", true, 33, 2, 27 },
                    { 70, "Price Break 5 - CUSTOMER Cost (excl GST)", true, 34, 2, 28 },
                    { 71, "Price Break 5 - CUSTOMER Cost (incl GST)", true, 35, 2, 29 },
                    { 72, "Barcode", true, 36, 2, 30 },
                    { 73, "SAP Local COS", true, 37, 2, 31 },
                    { 74, "Carton Qty", true, 38, 2, 32 },
                    { 75, "Stock Status (S = Stockable, * = Not normally stocked in Australia)", true, 39, 2, 33 },
                    { 76, "Valid From", true, 40, 2, 34 },
                    { 77, "Valid To", true, 41, 2, 35 },
                    { 78, "File Reference Data", true, 42, 2, 36 },
                    { 79, "Schneider Electric Material Reference", true, 5, 3, 1 },
                    { 80, "Material Description", true, 6, 3, 2 },
                    { 81, "List Price", true, 7, 3, 3 },
                    { 82, "Currency", true, 43, 3, 4 },
                    { 83, "Per", true, 9, 3, 5 },
                    { 84, "Price Unit", true, 10, 3, 6 },
                    { 85, "Order in Multiples of", true, 12, 3, 7 },
                    { 86, "VRG", true, 44, 3, 8 },
                    { 87, "VRG Description", true, 45, 3, 9 },
                    { 88, "Material Status", true, 46, 3, 10 },
                    { 89, "Quantity Break", true, 16, 3, 11 },
                    { 90, "Qty Discount or Price", true, 17, 3, 12 },
                    { 91, "Qty Buy Price", true, 18, 3, 13 },
                    { 92, "Quantity Break 2", true, 20, 3, 14 },
                    { 93, "Qty Discount or Price 2", true, 21, 3, 15 },
                    { 94, "Qty Buy Price 2", true, 22, 3, 16 },
                    { 95, "Qty Break 3", true, 24, 3, 17 },
                    { 96, "Qty Discount or Price 3", true, 25, 3, 18 },
                    { 97, "Qty Buy Price 3", true, 26, 3, 19 },
                    { 98, "Qty Break 4", true, 28, 3, 20 },
                    { 99, "Qty Discount or Price 4", true, 29, 3, 21 },
                    { 100, "Qty Buy Price 4", true, 30, 3, 22 },
                    { 101, "Qty Break 5", true, 32, 3, 23 },
                    { 102, "Qty Discount or Price 5", true, 33, 3, 24 },
                    { 103, "Qty Buy Price 5", true, 34, 3, 25 },
                    { 104, "EAN/UPC", true, 36, 3, 26 },
                    { 105, "Main Group", true, 47, 3, 27 },
                    { 106, "Main Group Description", true, 48, 3, 28 },
                    { 107, "Group", true, 49, 3, 29 },
                    { 108, "Group Description", true, 50, 3, 30 },
                    { 109, "SubGroup", true, 51, 3, 31 },
                    { 110, "SubGroup Description", true, 52, 3, 32 },
                    { 111, "CartonQty", true, 38, 3, 33 },
                    { 112, "Stock or Non Stock", true, 39, 3, 34 },
                    { 113, "Effective Date", true, 40, 3, 35 },
                    { 114, "Schneider Electric Material Reference", true, 5, 4, 1 },
                    { 115, "Material Description", true, 6, 4, 2 },
                    { 116, "List Price", true, 7, 4, 3 },
                    { 117, "Currency", true, 43, 4, 4 },
                    { 118, "Per", true, 9, 4, 5 },
                    { 119, "Price Unit", true, 10, 4, 6 },
                    { 120, "Order in Multiples of", true, 12, 4, 7 },
                    { 121, "Material Status", true, 46, 4, 8 },
                    { 122, "Quantity Break", true, 16, 4, 9 },
                    { 123, "Qty Discount or Price", true, 17, 4, 10 },
                    { 124, "Qty Buy Price", true, 18, 4, 11 },
                    { 125, "Quantity Break 2", true, 20, 4, 12 },
                    { 126, "Qty Discount or Price 2", true, 21, 4, 13 },
                    { 127, "Qty Buy Price 2", true, 22, 4, 14 },
                    { 128, "Qty Break 3", true, 24, 4, 15 },
                    { 129, "Qty Discount or Price 3", true, 25, 4, 16 },
                    { 130, "Qty Buy Price 3", true, 26, 4, 17 },
                    { 131, "Qty Break 4", true, 28, 4, 18 },
                    { 132, "Qty Discount or Price 4", true, 29, 4, 19 },
                    { 133, "Qty Buy Price 4", true, 30, 4, 20 },
                    { 134, "Qty Break 5", true, 32, 4, 21 },
                    { 135, "Qty Discount or Price 5", true, 33, 4, 22 },
                    { 136, "Qty Buy Price 5", true, 34, 4, 23 },
                    { 137, "EAN/UPC", true, 36, 4, 24 },
                    { 138, "Main Group", true, 47, 4, 25 },
                    { 139, "Main Group Description", true, 48, 4, 26 },
                    { 140, "Group", true, 49, 4, 27 },
                    { 141, "Group Description", true, 50, 4, 28 },
                    { 142, "SubGroup", true, 51, 4, 29 },
                    { 143, "SubGroup Description", true, 52, 4, 30 },
                    { 144, "CartonQty", true, 38, 4, 31 },
                    { 145, "Stock or Non Stock", true, 39, 4, 32 },
                    { 146, "Effective Date", true, 40, 4, 33 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "MST_RoleMenus",
                columns: new[] { "RoleMenuID", "IsActive", "MenuID", "RoleID" },
                values: new object[,]
                {
                    { 1, true, 1, 1 },
                    { 2, true, 2, 1 },
                    { 3, true, 3, 1 },
                    { 4, true, 4, 1 },
                    { 5, true, 5, 1 },
                    { 6, true, 6, 1 },
                    { 7, true, 7, 1 },
                    { 8, true, 8, 1 },
                    { 9, true, 9, 1 },
                    { 10, true, 10, 1 },
                    { 11, true, 11, 1 },
                    { 12, true, 12, 1 },
                    { 13, true, 13, 1 },
                    { 14, true, 1, 2 },
                    { 15, true, 2, 2 },
                    { 16, true, 3, 2 },
                    { 17, true, 10, 2 },
                    { 18, true, 11, 2 },
                    { 19, true, 12, 2 },
                    { 20, true, 13, 2 },
                    { 21, true, 1, 3 },
                    { 22, true, 2, 3 },
                    { 23, true, 12, 3 },
                    { 24, true, 13, 3 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "MST_TemplateMaster",
                columns: new[] { "TemplateMasterID", "AliasName", "CanUpload", "CountryCode", "IsActive", "TemplateCategoryID", "TemplateDataModel", "TemplateName" },
                values: new object[,]
                {
                    { 1, "VRG Descriptions", true, "00", true, 2, "JSON", "VRGDescriptions" },
                    { 2, "Material Status", true, "00", true, 2, "JSON", "MaterialStatus" },
                    { 3, "MOQ", true, "00", true, 2, "JSON", "MOQ" },
                    { 4, "RRP References", true, "00", true, 2, "JSON", "RRPReferences" },
                    { 5, "Material Master List", true, "00", true, 2, "Table", "MaterialMasterList" },
                    { 6, "GST Configurations", true, "00", true, 2, "JSON", "GSTConfigurations" },
                    { 7, "Discount Parameters", true, "00", true, 2, "JSON", "DiscountParameters" },
                    { 8, "Customer Contacts", true, "00", true, 2, "Table", "CustomerContacts" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "MST_UserRoleMapping",
                columns: new[] { "UserRoleMappingID", "CreatedBy", "CreatedDate", "IsActive", "RoleID", "UserSESA" },
                values: new object[,]
                {
                    { 1, "", new DateTime(2023, 9, 20, 10, 27, 38, 573, DateTimeKind.Utc).AddTicks(6194), true, 2, "SESA41591" },
                    { 2, "", new DateTime(2023, 9, 20, 10, 27, 38, 573, DateTimeKind.Utc).AddTicks(6198), true, 2, "SESA95046" },
                    { 3, "", new DateTime(2023, 9, 20, 10, 27, 38, 573, DateTimeKind.Utc).AddTicks(6199), true, 2, "SESA121108" },
                    { 4, "", new DateTime(2023, 9, 20, 10, 27, 38, 573, DateTimeKind.Utc).AddTicks(6200), true, 2, "SESA432166" },
                    { 5, "", new DateTime(2023, 9, 20, 10, 27, 38, 573, DateTimeKind.Utc).AddTicks(6201), true, 2, "SESA658252" },
                    { 6, "", new DateTime(2023, 9, 20, 10, 27, 38, 573, DateTimeKind.Utc).AddTicks(6202), true, 2, "SESA512280" },
                    { 7, "", new DateTime(2023, 9, 20, 10, 27, 38, 573, DateTimeKind.Utc).AddTicks(6203), true, 2, "SESA715213" },
                    { 8, "", new DateTime(2023, 9, 20, 10, 27, 38, 573, DateTimeKind.Utc).AddTicks(6203), true, 2, "SESA715214" },
                    { 9, "", new DateTime(2023, 9, 20, 10, 27, 38, 573, DateTimeKind.Utc).AddTicks(6204), true, 2, "SESA497078" },
                    { 10, "", new DateTime(2023, 9, 20, 10, 27, 38, 573, DateTimeKind.Utc).AddTicks(6205), true, 2, "SESA654946" },
                    { 11, "", new DateTime(2023, 9, 20, 10, 27, 38, 573, DateTimeKind.Utc).AddTicks(6205), true, 1, "ADM432166" },
                    { 12, "", new DateTime(2023, 9, 20, 10, 27, 38, 573, DateTimeKind.Utc).AddTicks(6206), true, 1, "ADM658252" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "MST_TemplateStructure",
                columns: new[] { "TemplateStructureID", "IsActive", "PropertyDataType", "PropertyDescription", "PropertyName", "SequenceNo", "TemplateMasterID" },
                values: new object[,]
                {
                    { 1, true, "VARCHAR", "VRG", "VRG", 1, 1 },
                    { 2, true, "VARCHAR", "VRG Description", "VRGDescription", 2, 1 },
                    { 3, true, "VARCHAR", "St", "St", 1, 2 },
                    { 4, true, "VARCHAR", "Description", "Description", 2, 2 },
                    { 5, true, "VARCHAR", "Status", "Status", 3, 2 },
                    { 6, true, "VARCHAR", "Schneider Electric Material Reference", "SchneiderElectricMaterialReference", 1, 3 },
                    { 7, true, "VARCHAR", "MOQa", "MOQa", 2, 3 },
                    { 8, true, "VARCHAR", "LCOS1-4", "LCOS1To4", 1, 4 },
                    { 9, true, "VARCHAR", "Collection", "Collection", 2, 4 },
                    { 10, true, "VARCHAR", "Sub Collection", "SubCollection", 3, 4 },
                    { 11, true, "VARCHAR", "Discount Group", "DiscountGroup", 4, 4 },
                    { 12, true, "float", "Description", "RRPMarkup", 5, 4 },
                    { 13, true, "VARCHAR", "Prefix", "Prefix", 1, 5 },
                    { 14, true, "VARCHAR", "CatNo", "CatNo", 2, 5 },
                    { 15, true, "VARCHAR", "ColourCode", "ColourCode", 3, 5 },
                    { 16, true, "VARCHAR", "ItemNo", "ItemNo", 4, 5 },
                    { 17, true, "VARCHAR", "InternalSAPItemNo", "InternalSAPItemNo", 5, 5 },
                    { 18, true, "INT", "SplitPackQty", "SplitPackQty", 6, 5 },
                    { 19, true, "VARCHAR", "CountryCode", "CountryCode", 1, 6 },
                    { 20, true, "float", "GST Percentage", "GSTPercentage", 2, 6 },
                    { 21, true, "VARCHAR", "DiscountName", "DiscountName", 1, 7 },
                    { 22, true, "VARCHAR", "DiscountValue", "DiscountValue", 2, 7 },
                    { 23, true, "VARCHAR", "AccountNumber", "AccountNumber", 1, 8 },
                    { 24, true, "VARCHAR", "AccountName", "AccountName", 2, 8 },
                    { 25, true, "VARCHAR", "ContactPerson", "ContactPerson", 3, 8 },
                    { 26, true, "VARCHAR", "ToEmailID", "ToEmailID", 4, 8 },
                    { 27, true, "VARCHAR", "CcEmailID", "CcEmailID", 5, 8 },
                    { 28, true, "VARCHAR", "BccEmailID", "BccEmailID", 6, 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MST_AppConfig_ConfigName",
                schema: "dbo",
                table: "MST_AppConfig",
                column: "ConfigName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MST_ConfigOptions_ConfigType_ConfigValue",
                schema: "dbo",
                table: "MST_ConfigOptions",
                columns: new[] { "ConfigType", "ConfigValue" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MST_CustomerContact_AccountNumber",
                schema: "dbo",
                table: "MST_CustomerContact",
                column: "AccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_MST_MaterialMaster_InternalSAPItemNo",
                schema: "dbo",
                table: "MST_MaterialMaster",
                column: "InternalSAPItemNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MST_Menus_MenuName",
                schema: "dbo",
                table: "MST_Menus",
                column: "MenuName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MST_NotificationTemplate_SalesOrganization_TemplateName",
                schema: "dbo",
                table: "MST_NotificationTemplate",
                columns: new[] { "SalesOrganization", "TemplateName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MST_ReportFormatFieldMapping_ReportFormatFieldMasterID",
                schema: "dbo",
                table: "MST_ReportFormatFieldMapping",
                column: "ReportFormatFieldMasterID");

            migrationBuilder.CreateIndex(
                name: "IX_MST_ReportFormatFieldMapping_ReportFormatMasterID_ReportFormatFieldMasterID",
                schema: "dbo",
                table: "MST_ReportFormatFieldMapping",
                columns: new[] { "ReportFormatMasterID", "ReportFormatFieldMasterID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MST_ReportFormatFieldMaster_FieldName",
                schema: "dbo",
                table: "MST_ReportFormatFieldMaster",
                column: "FieldName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MST_ReportFormatMaster_FormatName",
                schema: "dbo",
                table: "MST_ReportFormatMaster",
                column: "FormatName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MST_RoleMenus_RoleID_MenuID",
                schema: "dbo",
                table: "MST_RoleMenus",
                columns: new[] { "RoleID", "MenuID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MST_Roles_RoleName",
                schema: "dbo",
                table: "MST_Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MST_TemplateCategory_CategoryName",
                schema: "dbo",
                table: "MST_TemplateCategory",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MST_TemplateData_TemplateMasterID",
                schema: "dbo",
                table: "MST_TemplateData",
                column: "TemplateMasterID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MST_TemplateMaster_TemplateCategoryID",
                schema: "dbo",
                table: "MST_TemplateMaster",
                column: "TemplateCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_MST_TemplateMaster_TemplateName",
                schema: "dbo",
                table: "MST_TemplateMaster",
                column: "TemplateName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MST_TemplateStructure_TemplateMasterID_PropertyName",
                schema: "dbo",
                table: "MST_TemplateStructure",
                columns: new[] { "TemplateMasterID", "PropertyName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MST_UserMaster_UserSESA",
                schema: "dbo",
                table: "MST_UserMaster",
                column: "UserSESA",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MST_UserRoleMapping_RoleID_UserSESA",
                schema: "dbo",
                table: "MST_UserRoleMapping",
                columns: new[] { "RoleID", "UserSESA" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TRN_PriceFileDetails_PriceFileHeaderID_CustomerNo",
                schema: "dbo",
                table: "TRN_PriceFileDetails",
                columns: new[] { "PriceFileHeaderID", "CustomerNo" });

            migrationBuilder.CreateIndex(
                name: "IX_TRN_PriceFileHeader_UserConfigSettingID",
                schema: "dbo",
                table: "TRN_PriceFileHeader",
                column: "UserConfigSettingID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TRN_PriceFileLocationDetails_PriceFileHeaderID_CustomerNo",
                schema: "dbo",
                table: "TRN_PriceFileLocationDetails",
                columns: new[] { "PriceFileHeaderID", "CustomerNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TRN_UserConfigSetting_UserSESA",
                schema: "dbo",
                table: "TRN_UserConfigSetting",
                column: "UserSESA");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MST_AppConfig",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MST_ConfigOptions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MST_CustomerContact",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MST_MaterialMaster",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MST_NotificationTemplate",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MST_ReportFormatFieldMapping",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MST_RoleMenus",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MST_TemplateData",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MST_TemplateStructure",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MST_UserMaster",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MST_UserRoleMapping",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TRN_NLog",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TRN_NotificationHistory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TRN_PriceFileDetails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TRN_PriceFileLocationDetails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TRN_PriceFileLog",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TRN_UserConfigSetting",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TRN_UserLog",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MST_ReportFormatFieldMaster",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MST_ReportFormatMaster",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MST_Menus",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MST_TemplateMaster",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MST_Roles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TRN_PriceFileHeader",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MST_TemplateCategory",
                schema: "dbo");
        }
    }
}
