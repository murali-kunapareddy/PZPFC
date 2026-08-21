using Microsoft.EntityFrameworkCore;
using NPOI.POIFS.FileSystem;
using PFCRepository.DatabaseContext.Models.CustomTables;
using PFCRepository.DatabaseContext.Models.ExtractionTables;
using PFCRepository.Utilities;

namespace PFCRepository.DatabaseContext
{
    /// <summary>
    /// SEDBContext
    /// </summary>
    public class PFCDBContext : DbContext
    {


        static string connectionString = AppConfig.ConnectionString;
        public PFCDBContext(DbContextOptions options)
            : base(options)
        {
            this.Database.SetCommandTimeout(300); // <-- 300 seconds

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(AppConfig.ConnectionString);
            optionsBuilder.EnableSensitiveDataLogging(true);
        }



        public virtual DbSet<Menu> MST_Menus { get; set; }
        public virtual DbSet<Role> MST_Roles { get; set; }
        public virtual DbSet<RoleMenu> MST_RoleMenus { get; set; }
        public virtual DbSet<UserMaster> MST_UserMasters { get; set; }
        public virtual DbSet<UserRoleMapping> MST_UserRoleMapping { get; set; }
        public virtual DbSet<AppConfigSetting> MST_AppConfigSetting { get; set; }
        public virtual DbSet<ConfigOptions> MST_ConfigOptions { get; set; }
        public virtual DbSet<UserLog> TRN_UserLog { get; set; }
        public virtual DbSet<UserConfigSetting> TRN_UserConfigSetting { get; set; }
        public virtual DbSet<TemplateCategory> MST_TemplateCategory { get; set; }
        public virtual DbSet<TemplateMaster> MST_TemplateMaster { get; set; }
        public virtual DbSet<TemplateStructure> MST_TemplateStructure { get; set; }
        public virtual DbSet<TemplateData> MST_TemplateData { get; set; }
        public virtual DbSet<MaterialMaster> MST_MaterialMaster { get; set; }
        public virtual DbSet<CustomerContacts> MST_CustomerContactDetails { get; set; }
        public virtual DbSet<ReportFormatMaster> MST_ReportFormatMaster { get; set; }
        public virtual DbSet<ReportFormatFieldMaster> MST_ReportFormatFieldMaster { get; set; }
        public virtual DbSet<ReportFormatFieldMapping> MST_ReportFormatFieldMapping { get; set; }
        public virtual DbSet<PriceFileHeader> TRN_PriceFileHeader { get; set; }
        public virtual DbSet<PriceFileDetails> TRN_PriceFileDetails { get; set; }
        public virtual DbSet<PriceFileLocationDetails> TRN_PriceFileLocationDetails { get; set; }
        public virtual DbSet<PriceFileLog> TRN_PriceFileLog { get; set; }
        public virtual DbSet<NLogEntity> TRN_NLogEntity { get; set; }
        public virtual DbSet<NotificationTemplates> MST_NotificationTemplates { get; set; }
        public virtual DbSet<NotificationHistory> TRN_NotificationHistory { get; set; }

        public virtual DbSet<A507> A507 { get; set; }
        public virtual DbSet<A604> A604 { get; set; }
        public virtual DbSet<A606> A606 { get; set; }
        public virtual DbSet<A607> A607 { get; set; }
        public virtual DbSet<A608> A608 { get; set; }
        public virtual DbSet<A609> A609 { get; set; }
        public virtual DbSet<A652> A652 { get; set; }
        public virtual DbSet<A653> A653 { get; set; }
        public virtual DbSet<A655> A655 { get; set; }
        public virtual DbSet<A657> A657 { get; set; }
        public virtual DbSet<A979> A979 { get; set; }
        public virtual DbSet<A996> A996 { get; set; }
        public virtual DbSet<Kna1> KNA1 { get; set; }
        public virtual DbSet<Knvv> KNVV { get; set; }
        public virtual DbSet<KonmSum> KONM_sum { get; set; }
        public virtual DbSet<Konp> KONP { get; set; }
        public virtual DbSet<Makt> MAKT { get; set; }
        public virtual DbSet<Mara> MARA { get; set; }
        public virtual DbSet<Marc> MARC { get; set; }
        public virtual DbSet<MarmSum> MARM_sum { get; set; }
        public virtual DbSet<Mvke> MVKE { get; set; }
        public virtual DbSet<T006a> T006A { get; set; }
        public virtual DbSet<CustomerHierarchyCust> Customer_Hierarchy_cust { get; set; }
        public virtual DbSet<CustomerSettings> MST_CustomerSettings { get; set; }
        public virtual DbSet<QueueModel> TRN_Queue { get; set; }
        public virtual DbSet<QueueHistory> TRN_Queue_History { get; set; }
        public virtual DbSet<MvkeOverride> MvkeOverride { get; set; }
        public virtual DbSet<ETLJobProcessHistory> ETLJobProcessHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region BackOffice

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("MST_Menus", tb => tb.HasTrigger("UTR_MST_Menus_Audit"));
                entity.ToTable("MST_Menus", "dbo");
                //entity.HasKey(e => e.MenuID);
                entity.HasIndex(e => e.MenuName).IsUnique(true);
                entity.Property(e => e.ParentID).HasDefaultValueSql("0");
                entity.Property(e => e.HrefVal).HasDefaultValueSql("('')");
                entity.Property(e => e.CanShowMenu).HasDefaultValueSql("0");
                entity.Property(e => e.SortOrder).HasDefaultValueSql("0");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            });


            // configuration schema seed data
            modelBuilder.Entity<Menu>().HasData(
                new Menu() { MenuID = 1,  MenuName = "PriceList",                    ParentID = 0, ControllerName = "PriceList", ActionName = "GeneratePriceFile",       AliasName = "Price List",               HrefVal = "", CanShowMenu = true,  SortOrder = 1, IsActive = true },
                new Menu() { MenuID = 2,  MenuName = "Templates",                    ParentID = 0, ControllerName = "",          ActionName = "",                        AliasName = "Templates",                HrefVal = "", CanShowMenu = true,  SortOrder = 2, IsActive = true },
                new Menu() { MenuID = 3,  MenuName = "Masters",                      ParentID = 0, ControllerName = "Configure", ActionName = "Masters",                 AliasName = "Masters",                  HrefVal = "", CanShowMenu = true,  SortOrder = 3, IsActive = true },
                new Menu() { MenuID = 4,  MenuName = "UserManagement",               ParentID = 0, ControllerName = "",          ActionName = "",                        AliasName = "User Management",          HrefVal = "", CanShowMenu = true,  SortOrder = 4, IsActive = true },
                new Menu() { MenuID = 5,  MenuName = "Settings",                     ParentID = 0, ControllerName = "Configure", ActionName = "ApplicationSettings",     AliasName = "Settings",                 HrefVal = "", CanShowMenu = true,  SortOrder = 6, IsActive = true },
                
                new Menu() { MenuID = 6,  MenuName = "Roles",                        ParentID = 4, ControllerName = "BackOps",   ActionName = "Roles",                   AliasName = "Roles",                    HrefVal = "", CanShowMenu = true,  SortOrder = 1, IsActive = true },
                new Menu() { MenuID = 7,  MenuName = "Users",                        ParentID = 4, ControllerName = "BackOps",   ActionName = "Users",                   AliasName = "Users",                    HrefVal = "", CanShowMenu = true,  SortOrder = 2, IsActive = true },
                new Menu() { MenuID = 8, MenuName = "Add Users",                     ParentID = 4, ControllerName = "BackOps",    ActionName = "AddUser",                AliasName = "Add Users",                HrefVal = "", CanShowMenu = false, SortOrder = 4, IsActive = true },
                new Menu() { MenuID = 9,  MenuName = "User Role Mapping",            ParentID = 4, ControllerName = "BackOps",   ActionName = "UserRoleMapping",         AliasName = "User Role Mapping",        HrefVal = "", CanShowMenu = true,  SortOrder = 3, IsActive = true },
              

                new Menu() { MenuID = 10,  MenuName = "Trade List Formats",          ParentID = 2, ControllerName = "Configure", ActionName = "ReportFormat",             AliasName = "Trade List Formats",             HrefVal = "", CanShowMenu = true,  SortOrder = 1, IsActive = true },
                new Menu() { MenuID = 11, MenuName = "Trade List Templates",         ParentID = 2, ControllerName = "Configure", ActionName = "ReportContent",            AliasName = "Trade List Templates",           HrefVal = "", CanShowMenu = true,   SortOrder = 2, IsActive = true },
                new Menu() { MenuID = 12, MenuName = "Customer Templates",           ParentID = 2, ControllerName = "Configure", ActionName = "CustomerTemplates",        AliasName = "Customer Templates",       HrefVal = "", CanShowMenu = true,   SortOrder = 3, IsActive = true },
                new Menu() { MenuID = 13, MenuName = "Notifications",                ParentID = 0, ControllerName = "Notification", ActionName = "History", AliasName = "Notifications", HrefVal = "", CanShowMenu = true, SortOrder = 5, IsActive = true }
            );

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("MST_Roles", tb => tb.HasTrigger("UTR_MST_Roles_Audit"));
                entity.ToTable("MST_Roles", "dbo");
                //entity.HasKey(e => e.RoleID);
                entity.HasIndex(e => e.RoleName).IsUnique(true);
                entity.Property(e => e.SortOrder).HasDefaultValueSql("0");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<Role>().HasData(
                new Role() { RoleID = 1, RoleName = "Admin",          SortOrder = 1, IsActive = true },
                new Role() { RoleID = 2, RoleName = "PrivilegedUser", SortOrder = 2, IsActive = true },
                new Role() { RoleID = 3, RoleName = "User",           SortOrder = 3, IsActive = true }

            );

            modelBuilder.Entity<RoleMenu>(entity =>
            {
                entity.ToTable("MST_RoleMenus", tb => tb.HasTrigger("UTR_MST_RoleMenus_Audit"));
                entity.ToTable("MST_RoleMenus", "dbo");
                //entity.HasKey(e => e.RoleMenuID);
                entity.HasIndex(e => new { e.RoleID, e.MenuID }).IsUnique(true);
                entity.HasOne(m => m.Roles).WithMany(t => t.RoleMenus).HasForeignKey(m => m.RoleID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(m => m.Menus).WithMany(t => t.RoleMenus).HasForeignKey(m => m.RoleID).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            });
            modelBuilder.Entity<RoleMenu>().HasData(
                new RoleMenu() { RoleMenuID = 1,  RoleID = 1, MenuID = 1,   IsActive = true },
                new RoleMenu() { RoleMenuID = 2,  RoleID = 1, MenuID = 2,   IsActive = true },
                new RoleMenu() { RoleMenuID = 3,  RoleID = 1, MenuID = 3,   IsActive = true },
                new RoleMenu() { RoleMenuID = 4,  RoleID = 1, MenuID = 4,   IsActive = true },
                new RoleMenu() { RoleMenuID = 5,  RoleID = 1, MenuID = 5,   IsActive = true },
                new RoleMenu() { RoleMenuID = 6,  RoleID = 1, MenuID = 6,   IsActive = true },
                new RoleMenu() { RoleMenuID = 7,  RoleID = 1, MenuID = 7,   IsActive = true },
                new RoleMenu() { RoleMenuID = 8,  RoleID = 1, MenuID = 8,   IsActive = true },
                new RoleMenu() { RoleMenuID = 9,  RoleID = 1, MenuID = 9,   IsActive = true },
                new RoleMenu() { RoleMenuID = 10, RoleID = 1, MenuID = 10, IsActive = true },
                new RoleMenu() { RoleMenuID = 11, RoleID = 1, MenuID = 11, IsActive = true },
                new RoleMenu() { RoleMenuID = 12, RoleID = 1, MenuID = 12, IsActive = true },
                new RoleMenu() { RoleMenuID = 13, RoleID = 1, MenuID = 13, IsActive = true },

                new RoleMenu() { RoleMenuID = 14, RoleID = 2, MenuID = 1,  IsActive = true },
                new RoleMenu() { RoleMenuID = 15, RoleID = 2, MenuID = 2,  IsActive = true },
                new RoleMenu() { RoleMenuID = 16, RoleID = 2, MenuID = 3,  IsActive = true },
                new RoleMenu() { RoleMenuID = 17, RoleID = 2, MenuID = 10,  IsActive = true },
                new RoleMenu() { RoleMenuID = 18, RoleID = 2, MenuID = 11,  IsActive = true },
                new RoleMenu() { RoleMenuID = 19, RoleID = 2, MenuID = 12, IsActive = true },
                new RoleMenu() { RoleMenuID = 20, RoleID = 2, MenuID = 13, IsActive = true },

                new RoleMenu() { RoleMenuID = 21, RoleID = 3, MenuID = 1, IsActive = true },
                new RoleMenu() { RoleMenuID = 22, RoleID = 3, MenuID = 2, IsActive = true },
                new RoleMenu() { RoleMenuID = 23, RoleID = 3, MenuID = 12, IsActive = true },
                new RoleMenu() { RoleMenuID = 24, RoleID = 3, MenuID = 13, IsActive = true }



            );

            modelBuilder.Entity<UserMaster>(entity =>
            {
                entity.ToTable("MST_UserMaster", tb => tb.HasTrigger("UTR_MST_UserMaster_Audit"));
                entity.ToTable("MST_UserMaster", tb => tb.HasTrigger("UTR_MST_UserMaster_Audit"));
                entity.ToTable("MST_UserMaster", "dbo");
                //entity.HasKey(e => e.UserMasterID);
                entity.HasIndex(e => e.UserSESA).IsUnique(true);
                //entity.Property(e => e.UserSESA);
                //entity.Property(e => e.FirstName);
                entity.Property(e => e.LastName).HasDefaultValueSql("('')");
                //entity.Property(e => e.Email);
                //entity.Property(e => e.Company).HasDefaultValueSql("('')");
                entity.Property(e => e.Department).HasDefaultValueSql("('')");
                //entity.Property(e => e.Location).HasDefaultValueSql("('')");
                //entity.Property(e => e.ManagerSESA);
                //entity.Property(e => e.Country);
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
                entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getutcdate())");
                
            });


            modelBuilder.Entity<UserMaster>().HasData(
                
                
                new UserMaster() { UserMasterID = 1, UserSESA = "SESA41591", FirstName = "Robert", LastName = "FUSCO", Email = "robert.fusco@se.com", Department = "Schneider Digital", Country = "AU", IsActive = true },
                new UserMaster() { UserMasterID = 2, UserSESA = "SESA95046", FirstName = "Jeffrey", LastName = "SMITH", Email = "jeff.smith@se.com", Department = "Data Management & Business Analysis", Country = "AU", IsActive = true },
                new UserMaster() { UserMasterID = 3, UserSESA = "SESA121108", FirstName = "Clayton", LastName = "HALL", Email = "Clayton.Hall@se.com", Department = "Price Management", Country = "NZ", IsActive = true },
                
                new UserMaster() { UserMasterID = 4, UserSESA = "SESA432166", FirstName = "Murali", LastName = "KUNAPAREDDY", Email = "murali.kunapareddy@se.com", Department = "Schneider Digital", Country = "IN", IsActive = true },
                new UserMaster() { UserMasterID = 5, UserSESA = "SESA658252", FirstName = "Naresh", LastName = "CHALLA", Email = "nareshkumar.challa@non.se.com", Department = "Schneider Digital", Country = "IN", IsActive = true },
                new UserMaster() { UserMasterID = 6, UserSESA = "SESA512280", FirstName = "Venkata", LastName = "Jayendranath", Email = "VenkataJayendranath.Yelleswarapu@se.com", Department = "Schneider Digital", Country = "IN", IsActive = true },
                new UserMaster() { UserMasterID = 7, UserSESA = "SESA715213", FirstName = "Bhavana", LastName = "Adari", Email = "bhavana.adari@non.se.com", Department = "Schneider Digital", Country = "IN", IsActive = true },
                new UserMaster() { UserMasterID = 8, UserSESA = "SESA715214", FirstName = "Shiva", LastName = "KOMAPARATHI", Email = "shiva.komaparathi@non.se.com", Department = "Schneider Digital", Country = "IN", IsActive = true },
                new UserMaster() { UserMasterID = 9, UserSESA = "SESA497078", FirstName = "Ram", LastName = "SINGH", Email = "Ram.Singh@se.com", Department = "Schneider Digital", Country = "IN", IsActive = true },
                new UserMaster() { UserMasterID = 10, UserSESA = "SESA654946", FirstName = "Prabhu", LastName = "SEKAR", Email = "prabhu.sekar@non.se.com", Department = "Schneider Digital", Country = "IN", IsActive = true },
                new UserMaster() { UserMasterID = 11, UserSESA = "ADM432166", FirstName = "Murali", LastName = "KUNAPAREDDY", Email = "murali.kunapareddy@se.com", Department = "Schneider Digital", Country = "IN", IsActive = true },
                new UserMaster() { UserMasterID = 12, UserSESA = "ADM658252", FirstName = "Naresh", LastName = "Challa", Email = "nareshkumar.challa@non.se.com", Department = "Schneider Digital", Country = "IN", IsActive = true }
                );

            modelBuilder.Entity<UserRoleMapping>(entity =>
            {
                entity.ToTable("MST_UserRoleMapping", tb => tb.HasTrigger("UTR_MST_UserRoleMapping_Audit"));
                entity.ToTable("MST_UserRoleMapping", "dbo");
                //entity.HasKey(e => e.UserRoleMappingID);
                entity.HasIndex(e => new { e.RoleID, e.UserSESA }).IsUnique(true);
                //entity.Property(e => e.UserSESA);
                entity.HasOne(m => m.Roles).WithMany(t => t.UserRoleMapping).HasForeignKey(m => m.RoleID).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
                entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getutcdate())");
                
            });

            modelBuilder.Entity<UserRoleMapping>().HasData(
                new UserRoleMapping() { UserRoleMappingID = 1, UserSESA = "SESA41591", RoleID = 2, IsActive = true},
                new UserRoleMapping() { UserRoleMappingID = 2, UserSESA = "SESA95046", RoleID = 2, IsActive = true},
                new UserRoleMapping() { UserRoleMappingID = 3, UserSESA = "SESA121108", RoleID = 2, IsActive = true},
                new UserRoleMapping() { UserRoleMappingID = 4, UserSESA = "SESA432166", RoleID = 2, IsActive = true},
                new UserRoleMapping() { UserRoleMappingID = 5, UserSESA = "SESA658252", RoleID = 2, IsActive = true },
                new UserRoleMapping() { UserRoleMappingID = 6, UserSESA = "SESA512280", RoleID = 2, IsActive = true },
                new UserRoleMapping() { UserRoleMappingID = 7, UserSESA = "SESA715213", RoleID = 2, IsActive = true },
                new UserRoleMapping() { UserRoleMappingID = 8, UserSESA = "SESA715214", RoleID = 2, IsActive = true },
                new UserRoleMapping() { UserRoleMappingID = 9, UserSESA = "SESA497078", RoleID = 2, IsActive = true },
                new UserRoleMapping() { UserRoleMappingID = 10, UserSESA = "SESA654946", RoleID = 2, IsActive = true },
                new UserRoleMapping() { UserRoleMappingID = 11, UserSESA = "ADM432166", RoleID = 1, IsActive = true },
                new UserRoleMapping() { UserRoleMappingID = 12, UserSESA = "ADM658252", RoleID = 1, IsActive = true }

            );

            modelBuilder.Entity<UserLog>(entity =>
            {
                entity.ToTable("TRN_UserLog", tb => tb.HasTrigger("UTR_TRN_UserLog_Audit"));
                entity.ToTable("TRN_UserLog", "dbo");
                //entity.HasKey(e => e.UserLogID);
                entity.Property(e => e.AttemptedOn).HasDefaultValueSql("(getutcdate())");
                entity.Property(e => e.IsSuccess).HasDefaultValueSql("0"); ;
                entity.Property(e => e.IPAddress).HasDefaultValueSql("('')");
                entity.Property(e => e.MachineName).HasDefaultValueSql("('')");
                entity.Property(e => e.OperatingSystem).HasDefaultValueSql("('')");
                entity.Property(e => e.UserHostAddress).HasDefaultValueSql("('')");
                entity.Property(e => e.UserAgent).HasDefaultValueSql("('')");
                //entity.Property(e => e.IsActive).HasDefaultValueSql("1");
            });
         

            modelBuilder.Entity<ConfigOptions>(entity =>
            {
                entity.ToTable("MST_ConfigOptions", tb => tb.HasTrigger("UTR_MST_ConfigOptions_Audit"));
                entity.ToTable("MST_ConfigOptions", "dbo");
                entity.HasIndex(e => new { e.ConfigType,e.ConfigValue }).IsUnique(true);
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
                entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getutcdate())");

            });

            modelBuilder.Entity<ConfigOptions>().HasData(
              
              new ConfigOptions() { ConfigOptionID = 1, ConfigType = "LogType",ConfigValue = "Trace", SequenceNo = 1, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 2, ConfigType = "LogType",ConfigValue = "Debug", SequenceNo = 2, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 3, ConfigType = "LogType", ConfigValue = "Info", SequenceNo = 3, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 4, ConfigType = "LogType", ConfigValue = "Warn", SequenceNo = 4, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 5, ConfigType = "LogType",ConfigValue = "Error", SequenceNo = 5, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 6, ConfigType = "LogType",ConfigValue = "Fatal", SequenceNo = 6, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 7, ConfigType = "LogType", ConfigValue = "Off", SequenceNo = 7, IsActive = true },

              new ConfigOptions() { ConfigOptionID = 8, ConfigType = "BulkUpdate", ConfigValue = "500", SequenceNo = 1, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 9, ConfigType = "BulkUpdate", ConfigValue = "1000", SequenceNo = 2, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 10, ConfigType = "BulkUpdate", ConfigValue = "5000", SequenceNo = 3, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 11, ConfigType = "BulkUpdate", ConfigValue = "10000", SequenceNo = 4, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 12, ConfigType = "BulkUpdate", ConfigValue = "15000", SequenceNo = 5, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 13,ConfigType = "BulkUpdate", ConfigValue = "20000", SequenceNo = 6, IsActive = true },

              new ConfigOptions() { ConfigOptionID = 14, ConfigType = "DisplayMaxRecords", ConfigValue = "500", SequenceNo = 1, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 15, ConfigType = "DisplayMaxRecords", ConfigValue = "1000", SequenceNo = 2, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 16, ConfigType = "DisplayMaxRecords", ConfigValue = "5000", SequenceNo = 3, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 17, ConfigType = "DisplayMaxRecords", ConfigValue = "10000", SequenceNo = 4, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 18, ConfigType = "DisplayMaxRecords", ConfigValue = "15000", SequenceNo = 5, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 19, ConfigType = "DisplayMaxRecords", ConfigValue = "20000", SequenceNo = 6, IsActive = true },

              new ConfigOptions() { ConfigOptionID = 20, ConfigType = "YesNo", ConfigValue = "Yes", SequenceNo = 1, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 21, ConfigType = "YesNo", ConfigValue = "No", SequenceNo = 2, IsActive = true },

              new ConfigOptions() { ConfigOptionID = 22, ConfigType = "MaxCustomers", ConfigValue = "5", SequenceNo = 1, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 23, ConfigType = "MaxCustomers", ConfigValue = "10", SequenceNo = 2, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 24, ConfigType = "MaxCustomers", ConfigValue = "15", SequenceNo = 3, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 25, ConfigType = "MaxCustomers", ConfigValue = "20", SequenceNo = 4, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 26, ConfigType = "MaxCustomers", ConfigValue = "25", SequenceNo = 5, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 27, ConfigType = "MaxCustomers", ConfigValue = "30", SequenceNo = 6, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 28, ConfigType = "MaxCustomers", ConfigValue = "35", SequenceNo = 7, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 29, ConfigType = "MaxCustomers", ConfigValue = "40", SequenceNo = 8, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 30, ConfigType = "MaxCustomers", ConfigValue = "45", SequenceNo = 9, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 31, ConfigType = "MaxCustomers", ConfigValue = "50", SequenceNo = 10, IsActive = true },


              new ConfigOptions() { ConfigOptionID = 32, ConfigType = "MaxRetry", ConfigValue = "1",  SequenceNo = 1, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 33, ConfigType = "MaxRetry", ConfigValue = "2",  SequenceNo = 2, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 34, ConfigType = "MaxRetry", ConfigValue = "3",  SequenceNo = 3, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 35, ConfigType = "MaxRetry", ConfigValue = "4",  SequenceNo = 4, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 36, ConfigType = "MaxRetry", ConfigValue = "5",  SequenceNo = 5, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 37, ConfigType = "MaxRetry", ConfigValue = "6",  SequenceNo = 6, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 38, ConfigType = "MaxRetry", ConfigValue = "7",  SequenceNo = 7, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 39, ConfigType = "MaxRetry", ConfigValue = "8",  SequenceNo = 8, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 40, ConfigType = "MaxRetry", ConfigValue = "9",  SequenceNo = 9, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 41, ConfigType = "MaxRetry", ConfigValue = "10", SequenceNo = 10, IsActive = true },

              new ConfigOptions() { ConfigOptionID = 42, ConfigType = "ArchivedFileLocationMode", ConfigValue = "ApplicationServer", SequenceNo = 1, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 43, ConfigType = "ArchivedFileLocationMode", ConfigValue = "AWSS3Bucket", SequenceNo = 2, IsActive = true },

              new ConfigOptions() { ConfigOptionID = 44, ConfigType = "ArchivedFileExtractionMode", ConfigValue = "ArchivedFileLocation", SequenceNo = 1, IsActive = true },
              new ConfigOptions() { ConfigOptionID = 45, ConfigType = "ArchivedFileExtractionMode", ConfigValue = "DataBase", SequenceNo = 2, IsActive = true }

          );


            modelBuilder.Entity<AppConfigSetting>(entity =>
            {
                entity.ToTable("MST_AppConfig", tb => tb.HasTrigger("UTR_MST_AppConfig_Audit"));
                entity.ToTable("MST_AppConfig", "dbo");
                entity.HasIndex(e => new { e.ConfigName }).IsUnique(true);
                entity.Property(e => e.AliasName).HasDefaultValueSql("('')");
                entity.Property(e => e.Description).HasDefaultValueSql("('')");
                entity.Property(e => e.ConfigValue).HasDefaultValueSql("('')");
                entity.Property(e => e.ConfigDataType).HasDefaultValueSql("('')");
                entity.Property(e => e.ConfigUIType).HasDefaultValueSql("('')");
                entity.Property(e => e.ConfigType).HasDefaultValueSql("('')");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
                entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getutcdate())");
                
            });




            modelBuilder.Entity<AppConfigSetting>().HasData(
               new AppConfigSetting() { AppConfigID = 1, ConfigName = "BulkUpdateRecordCount", AliasName = "Bulk Update Records Count", Description = "Bulk Update while saving data into Database", ConfigDataType = "NUMBER", ConfigValue = "5000", ConfigMinLength = 4, ConfigMaxLength = 6,ConfigUIType ="DropDownList",ConfigType= "BulkUpdate",SequenceNo =1, IsActive = true },
               new AppConfigSetting() { AppConfigID = 2, ConfigName = "DisplayMaxRecords", AliasName = "Display Max Records", Description = "Display Max Records in a Grid", ConfigDataType = "NUMBER", ConfigValue = "5000", ConfigMinLength = 4, ConfigMaxLength = 6, ConfigUIType = "DropDownList", ConfigType = "DisplayMaxRecords", SequenceNo = 2, IsActive = true },
               new AppConfigSetting() { AppConfigID = 3, ConfigName = "SelectMaxCustomers", AliasName = "Select Max Customers", Description = "Select Max Customers for Price File Generation", ConfigDataType = "NUMBER", ConfigValue = "10", ConfigMinLength = 1, ConfigMaxLength = 3, ConfigUIType = "DropDownList", ConfigType = "MaxCustomers", SequenceNo = 3, IsActive = true },
               //new AppConfigSetting() { AppConfigID = 4, ConfigName = "MaxRetry", AliasName = "Max Retry", Description = "Max Retry Count", ConfigDataType = "NUMBER", ConfigValue = "3", ConfigMinLength = 1, ConfigMaxLength = 2, ConfigUIType = "DropDownList", ConfigType = "MaxRetry", SequenceNo = 4, IsActive = true },
               new AppConfigSetting() { AppConfigID = 4, ConfigName = "ArchivedFileLocationMode", AliasName = "Archived File Location", Description = "Archived File Location Mode", ConfigDataType = "STRING", ConfigValue = "ApplicationServer", ConfigMinLength = 3, ConfigMaxLength = 50, ConfigUIType = "DropDownList", ConfigType = "ArchivedFileLocationMode", SequenceNo = 4, IsActive = true },
               new AppConfigSetting() { AppConfigID = 5, ConfigName = "ArchivedFileExtractionMode", AliasName = "Archived File Extraction", Description = "Archived File Location Mode", ConfigDataType = "STRING", ConfigValue = "ArchivedFileLocation", ConfigMinLength = 3, ConfigMaxLength = 50, ConfigUIType = "DropDownList", ConfigType = "ArchivedFileExtractionMode", SequenceNo = 5, IsActive = true },              
               new AppConfigSetting() { AppConfigID = 6, ConfigName = "ViewUserLog", AliasName = "View UserLog", Description = "View UserLog information", ConfigDataType = "STRING", ConfigValue = "Yes", ConfigMinLength = 2, ConfigMaxLength = 3, ConfigUIType = "DropDownList", ConfigType = "YesNo", SequenceNo = 6, IsActive = true }
          
               );


            modelBuilder.Entity<UserConfigSetting>(entity =>
            {
                entity.ToTable("TRN_UserConfigSetting", tb => tb.HasTrigger("UTR_TRN_UserConfigSetting_Audit"));
                entity.ToTable("TRN_UserConfigSetting", "dbo");
                //entity.HasKey(e => e.UserConfigSettingID);
                entity.HasIndex(e => e.UserSESA).IsUnique(false);
                entity.HasIndex(e => e.UserSESA);
                entity.Property(e => e.SalesOrganization).HasDefaultValueSql("('')");
                entity.Property(e => e.SelectedCustomers).HasDefaultValueSql("('')");
                entity.Property(e => e.CanUseAutoReportContent).HasDefaultValueSql("0");
                entity.Property(e => e.ReportContentTemplateID).HasDefaultValueSql("0");
                entity.Property(e => e.ReportFormatTemplateID).HasDefaultValueSql("0");
                entity.Property(e => e.SelectedCustomersTemplateID).HasDefaultValueSql("0");

                entity.Property(e => e.CanIncludeTradePrices).HasDefaultValueSql("0");
                entity.Property(e => e.CanIncludeCustomerNetPrices).HasDefaultValueSql("0");
                entity.Property(e => e.CanIncludeCustomerHierarchyNetPrices).HasDefaultValueSql("0");
                entity.Property(e => e.CanIncludeOverallNetPrices).HasDefaultValueSql("0");
                entity.Property(e => e.CanIncludePriceGroupNets).HasDefaultValueSql("0");

                entity.Property(e => e.CanIncludeSellOffPrices).HasDefaultValueSql("0");
                entity.Property(e => e.CanIncludeDiscount1).HasDefaultValueSql("0");
                entity.Property(e => e.CanIncludeDiscount2).HasDefaultValueSql("0");
                entity.Property(e => e.CanIncludeDiscount3).HasDefaultValueSql("0");
                entity.Property(e => e.CanIncludeDiscount4).HasDefaultValueSql("0");
                entity.Property(e => e.CanIncludeDiscount5).HasDefaultValueSql("0");
                entity.Property(e => e.CanIncludeDiscount6).HasDefaultValueSql("0");
                entity.Property(e => e.CanIncludeDiscount7).HasDefaultValueSql("0");
                entity.Property(e => e.CanIncludeDiscount8).HasDefaultValueSql("0");
                entity.Property(e => e.CanIncludePromoPrice).HasDefaultValueSql("0");

                entity.Property(e => e.CanUseShiftBreaks).HasDefaultValueSql("0");
                entity.Property(e => e.CanUseMOQAsBrk1).HasDefaultValueSql("0");
                entity.Property(e => e.CanUseGlobalCOSForProductHierarchy).HasDefaultValueSql("0");
                entity.Property(e => e.CanUseLocalCOSForProductHierarchy).HasDefaultValueSql("0");
                entity.Property(e => e.CanAddSODInFinalPrice).HasDefaultValueSql("0");
                entity.Property(e => e.SODInFinalPriceValue).HasDefaultValueSql("0");
                entity.Property(e => e.CanUseAlternateValidFromDate).HasDefaultValueSql("0");
                entity.Property(e => e.CanShowTemplateMaterialOnly).HasDefaultValueSql("0");             
                entity.Property(e => e.CanSendEmail).HasDefaultValueSql("0");
                entity.Property(e => e.CanShowNotFoundTemplateMaterials).HasDefaultValueSql("0");

                //entity.Property(e => e.Discount1).HasDefaultValueSql("('')");
                //entity.Property(e => e.Discount2).HasDefaultValueSql("('')");
                //entity.Property(e => e.Discount3).HasDefaultValueSql("('')");
                //entity.Property(e => e.Discount4).HasDefaultValueSql("('')");
                //entity.Property(e => e.Discount5).HasDefaultValueSql("('')");
                //entity.Property(e => e.Discount6).HasDefaultValueSql("('')");
                //entity.Property(e => e.Discount7).HasDefaultValueSql("('')");
                //entity.Property(e => e.Discount8).HasDefaultValueSql("('')");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
                entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getutcdate())");
                
            });


            modelBuilder.Entity<NLogEntity>(entity =>
            {
                entity.ToTable("TRN_NLog", tb => tb.HasTrigger("UTR_TRN_NLog_Audit"));
                entity.ToTable("TRN_NLog", "dbo");
                entity.Property(e => e.MachineName).HasDefaultValueSql("('')");
                entity.Property(e => e.Logged).HasDefaultValueSql("(getutcdate())");
                entity.Property(e => e.MachineName).HasDefaultValueSql("('')");
                entity.Property(e => e.Level).HasDefaultValueSql("('')");
                entity.Property(e => e.Message).HasDefaultValueSql("('')");
                entity.Property(e => e.Logger).HasDefaultValueSql("('')");
                entity.Property(e => e.Properties).HasDefaultValueSql("('')");
                entity.Property(e => e.Callsite).HasDefaultValueSql("('')");
                entity.Property(e => e.Exception).HasDefaultValueSql("('')");
                entity.Property(e => e.StackTrace).HasDefaultValueSql("('')");
                entity.Property(e => e.ThreadID).HasDefaultValueSql("('')");

            });

            #endregion

            #region Templates & Report Masters

            modelBuilder.Entity<TemplateCategory>(entity =>
            {
                entity.ToTable("MST_TemplateCategory", tb => tb.HasTrigger("UTR_MST_TemplateCategory_Audit"));
                entity.ToTable("MST_TemplateCategory", "dbo");
                //entity.HasKey(e => e.TemplateCategoryID);
                entity.HasIndex(e => e.CategoryName).IsUnique(true);
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<TemplateCategory>().HasData(
               new TemplateCategory() { TemplateCategoryID = 1, CategoryName = "ReportTradeListContent", IsActive = true },
               new TemplateCategory() { TemplateCategoryID = 2, CategoryName = "MasterReferences", IsActive = true },
               new TemplateCategory() { TemplateCategoryID = 3, CategoryName = "CustomerTemplates", IsActive = true }
           );

            modelBuilder.Entity<TemplateMaster>(entity =>
            {
                entity.ToTable("MST_TemplateMaster", tb => tb.HasTrigger("UTR_MST_TemplateMaster_Audit"));
                entity.ToTable("MST_TemplateMaster", "dbo");
                //entity.HasKey(e => e.TemplateMasterID);
                entity.HasIndex(e => e.TemplateName).IsUnique(true);
                entity.HasOne(m => m.TemplateCategory).WithMany(t => t.TemplateMaster).HasForeignKey(m => m.TemplateCategoryID).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.AliasName).HasDefaultValueSql("('')");
                entity.Property(e => e.TemplateDataModel).HasDefaultValueSql("('JSON')");
                entity.Property(e => e.CountryCode).HasDefaultValueSql("('00')");
                entity.Property(e => e.CanDuplicate).HasDefaultValueSql("0");
                entity.Property(e => e.CanUpload).HasDefaultValueSql("1");
                entity.Property(e => e.CanEdit).HasDefaultValueSql("1");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<TemplateMaster>().HasData(
              new TemplateMaster() { TemplateMasterID = 1, TemplateCategoryID = 2, TemplateName = "VRGDescriptions",    AliasName = "VRG Descriptions",     CountryCode  ="00", TemplateDataModel = "JSON",  CanDuplicate  = false, CanUpload  =true, IsActive = true },
              new TemplateMaster() { TemplateMasterID = 2, TemplateCategoryID = 2, TemplateName = "MaterialStatus",     AliasName = "Material Status",      CountryCode = "00", TemplateDataModel = "JSON",  CanDuplicate = false,  CanUpload =true, IsActive = true },
              new TemplateMaster() { TemplateMasterID = 3, TemplateCategoryID = 2, TemplateName = "MOQ",                AliasName = "MOQ",                  CountryCode = "00", TemplateDataModel = "JSON",  CanDuplicate = false, CanUpload = true, IsActive = true },
              new TemplateMaster() { TemplateMasterID = 4, TemplateCategoryID = 2, TemplateName = "RRPReferences",             AliasName = "RRP References", CountryCode = "00", TemplateDataModel = "JSON",  CanDuplicate = false, CanUpload = true, IsActive = true },
              new TemplateMaster() { TemplateMasterID = 5, TemplateCategoryID = 2, TemplateName = "MaterialMasterList", AliasName = "Material Master List", CountryCode = "00", TemplateDataModel = "Table", CanDuplicate = false, CanUpload = true, IsActive = true },
              new TemplateMaster() { TemplateMasterID = 6, TemplateCategoryID = 2, TemplateName = "GSTConfigurations",  AliasName = "GST Configurations",   CountryCode = "00", TemplateDataModel = "JSON",  CanDuplicate = false, CanUpload = true, IsActive = true },
              new TemplateMaster() { TemplateMasterID = 7, TemplateCategoryID = 2, TemplateName = "DiscountParameters", AliasName = "Discount Parameters",  CountryCode = "00", TemplateDataModel = "JSON", CanDuplicate = false, CanUpload = true, IsActive = true },
              new TemplateMaster() { TemplateMasterID = 8, TemplateCategoryID = 2, TemplateName = "CustomerContacts", AliasName = "Customer Contacts", CountryCode = "00", TemplateDataModel = "Table", CanDuplicate = false, CanUpload = true, IsActive = true }
          );


            modelBuilder.Entity<TemplateStructure>(entity =>
            {
                entity.ToTable("MST_TemplateStructure", tb => tb.HasTrigger("UTR_MST_TemplateStructure_Audit"));
                entity.ToTable("MST_TemplateStructure", "dbo");
                //entity.HasKey(e => e.TemplateStructureID);
                entity.HasIndex(e => new { e.TemplateMasterID, e.PropertyName }).IsUnique(true);
                entity.HasOne(m => m.TemplateMaster).WithMany(t => t.TemplateStructure).HasForeignKey(m => m.TemplateMasterID).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.PropertyDescription).HasDefaultValueSql("('')");
                entity.Property(e => e.PropertyDataType).HasDefaultValueSql("('JSON')");
                entity.Property(e => e.SequenceNo).HasDefaultValueSql("0");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            });
            modelBuilder.Entity<TemplateStructure>().HasData(
               new TemplateStructure() { TemplateStructureID = 1,  TemplateMasterID = 1, PropertyName = "VRG",                                PropertyDescription = "VRG",                        PropertyDataType = "VARCHAR", SequenceNo =1, IsActive = true },
               new TemplateStructure() { TemplateStructureID = 2, TemplateMasterID = 1, PropertyName = "VRGDescription",                     PropertyDescription = "VRG Description",            PropertyDataType = "VARCHAR", SequenceNo = 2, IsActive = true },

               new TemplateStructure() { TemplateStructureID = 3, TemplateMasterID = 2, PropertyName = "St",                                 PropertyDescription = "St",                         PropertyDataType = "VARCHAR", SequenceNo = 1, IsActive = true },
               new TemplateStructure() { TemplateStructureID = 4, TemplateMasterID = 2, PropertyName = "Description",                        PropertyDescription = "Description",                PropertyDataType = "VARCHAR", SequenceNo = 2, IsActive = true },
               new TemplateStructure() { TemplateStructureID = 5, TemplateMasterID = 2, PropertyName = "Status",                             PropertyDescription = "Status",                     PropertyDataType = "VARCHAR", SequenceNo = 3, IsActive = true },

               new TemplateStructure() { TemplateStructureID = 6, TemplateMasterID = 3, PropertyName = "SchneiderElectricMaterialReference", PropertyDescription = "Schneider Electric Material Reference", PropertyDataType = "VARCHAR", SequenceNo = 1, IsActive = true },
               new TemplateStructure() { TemplateStructureID = 7, TemplateMasterID = 3, PropertyName = "MOQa",                               PropertyDescription = "MOQa",                                  PropertyDataType = "VARCHAR", SequenceNo = 2, IsActive = true },

               new TemplateStructure() { TemplateStructureID = 8, TemplateMasterID = 4, PropertyName = "LCOS1To4",                           PropertyDescription = "LCOS1-4",                    PropertyDataType = "VARCHAR", SequenceNo = 1, IsActive = true },
               new TemplateStructure() { TemplateStructureID = 9, TemplateMasterID = 4, PropertyName = "Collection",                         PropertyDescription = "Collection",                 PropertyDataType = "VARCHAR", SequenceNo = 2, IsActive = true },
               new TemplateStructure() { TemplateStructureID = 10, TemplateMasterID = 4, PropertyName = "SubCollection",                      PropertyDescription = "Sub Collection",             PropertyDataType = "VARCHAR", SequenceNo = 3, IsActive = true },
               new TemplateStructure() { TemplateStructureID = 11, TemplateMasterID = 4, PropertyName = "DiscountGroup",                      PropertyDescription = "Discount Group",             PropertyDataType = "VARCHAR", SequenceNo = 4, IsActive = true },
               new TemplateStructure() { TemplateStructureID = 12, TemplateMasterID = 4, PropertyName = "RRPMarkup",                          PropertyDescription = "Description",                PropertyDataType = "float",   SequenceNo = 5, IsActive = true },


               new TemplateStructure() { TemplateStructureID = 13, TemplateMasterID = 5, PropertyName = "Prefix",                             PropertyDescription = "Prefix",                     PropertyDataType = "VARCHAR", SequenceNo = 1, IsActive = true },
               new TemplateStructure() { TemplateStructureID = 14, TemplateMasterID = 5, PropertyName = "CatNo",                              PropertyDescription = "CatNo",                      PropertyDataType = "VARCHAR", SequenceNo = 2, IsActive = true },
               new TemplateStructure() { TemplateStructureID = 15, TemplateMasterID = 5, PropertyName = "ColourCode",                         PropertyDescription = "ColourCode",                 PropertyDataType = "VARCHAR", SequenceNo = 3, IsActive = true },
               new TemplateStructure() { TemplateStructureID = 16, TemplateMasterID = 5, PropertyName = "ItemNo",                             PropertyDescription = "ItemNo",                     PropertyDataType = "VARCHAR", SequenceNo = 4, IsActive = true },
               new TemplateStructure() { TemplateStructureID = 17, TemplateMasterID = 5, PropertyName = "InternalSAPItemNo",                  PropertyDescription = "InternalSAPItemNo",          PropertyDataType = "VARCHAR", SequenceNo = 5, IsActive = true },
               new TemplateStructure() { TemplateStructureID = 18, TemplateMasterID = 5, PropertyName = "SplitPackQty",                       PropertyDescription = "SplitPackQty",               PropertyDataType = "INT",     SequenceNo = 6, IsActive = true },

               new TemplateStructure() { TemplateStructureID = 19, TemplateMasterID = 6, PropertyName = "CountryCode",                        PropertyDescription = "CountryCode",                PropertyDataType = "VARCHAR", SequenceNo = 1, IsActive = true },
               new TemplateStructure() { TemplateStructureID = 20, TemplateMasterID = 6, PropertyName = "GSTPercentage",                      PropertyDescription = "GST Percentage",                      PropertyDataType = "float",   SequenceNo = 2, IsActive = true },

               new TemplateStructure() { TemplateStructureID = 21, TemplateMasterID = 7, PropertyName = "DiscountName",     PropertyDescription = "DiscountName",   PropertyDataType = "VARCHAR",     SequenceNo = 1, IsActive = true },
               new TemplateStructure() { TemplateStructureID = 22, TemplateMasterID = 7, PropertyName = "DiscountValue",    PropertyDescription = "DiscountValue",  PropertyDataType = "VARCHAR",     SequenceNo = 2, IsActive = true },

                new TemplateStructure() { TemplateStructureID = 23, TemplateMasterID =  8, PropertyName = "AccountNumber", PropertyDescription = "AccountNumber", PropertyDataType = "VARCHAR", SequenceNo = 1, IsActive = true },
                new TemplateStructure() { TemplateStructureID =  24, TemplateMasterID =  8, PropertyName = "AccountName", PropertyDescription = "AccountName", PropertyDataType = "VARCHAR", SequenceNo = 2, IsActive = true },
                new TemplateStructure() { TemplateStructureID =  25, TemplateMasterID =  8, PropertyName = "ContactPerson", PropertyDescription = "ContactPerson", PropertyDataType = "VARCHAR", SequenceNo = 3, IsActive = true },
                new TemplateStructure() { TemplateStructureID = 26, TemplateMasterID = 8, PropertyName = "ToEmailID", PropertyDescription = "ToEmailID", PropertyDataType = "VARCHAR", SequenceNo = 4, IsActive = true },
                new TemplateStructure() { TemplateStructureID = 27, TemplateMasterID = 8, PropertyName = "CcEmailID", PropertyDescription = "CcEmailID", PropertyDataType = "VARCHAR", SequenceNo = 5, IsActive = true },
                new TemplateStructure() { TemplateStructureID = 28, TemplateMasterID = 8, PropertyName = "BccEmailID", PropertyDescription = "BccEmailID", PropertyDataType = "VARCHAR", SequenceNo = 6, IsActive = true }




           );

            modelBuilder.Entity<TemplateData>(entity =>
            {
                entity.ToTable("MST_TemplateData", tb => tb.HasTrigger("UTR_MST_TemplateData_Audit"));
                entity.ToTable("MST_TemplateData", "dbo");
                //entity.HasKey(e => e.TemplateDataID);
                entity.HasIndex(e => e.TemplateMasterID).IsUnique(true);
                entity.HasOne(m => m.TemplateMaster).WithMany(t => t.TemplateData).HasForeignKey(m => m.TemplateMasterID).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<MaterialMaster>(entity =>
            {
                entity.ToTable("MST_MaterialMaster", tb => tb.HasTrigger("UTR_MST_MaterialMaster_Audit"));
                entity.ToTable("MST_MaterialMaster", "dbo");
                //entity.HasKey(e => e.MaterialMasterID);
                entity.HasIndex(e => e.InternalSAPItemNo).IsUnique(true);
                entity.Property(e => e.ColourCode).HasDefaultValueSql("('')");
                entity.Property(e => e.SplitPackQty).HasDefaultValueSql("0");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<CustomerContacts>(entity =>
            {
                entity.ToTable("MST_CustomerContact", tb => tb.HasTrigger("UTR_MST_CustomerContact_Audit"));
                entity.ToTable("MST_CustomerContact", "dbo");
                entity.HasIndex(e => e.AccountNumber).IsUnique(false);
                entity.Property(e => e.AccountNumber).HasDefaultValueSql("('')");
                entity.Property(e => e.AccountName).HasDefaultValueSql("('')");
                entity.Property(e => e.ContactPerson).HasDefaultValueSql("('')");
                entity.Property(e => e.ToEmailID).HasDefaultValueSql("('')");
                entity.Property(e => e.CcEmailID).HasDefaultValueSql("('')");
                entity.Property(e => e.BccEmailID).HasDefaultValueSql("('')");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ReportFormatMaster>(entity =>
            {
                entity.ToTable("MST_ReportFormatMaster", tb => tb.HasTrigger("UTR_MST_ReportFormatMaster_Audit"));
                entity.ToTable("MST_ReportFormatMaster", "dbo");
                //entity.HasKey(e => e.ReportFormatMasterID);
                entity.HasIndex(e => e.FormatName).IsUnique(true);
                entity.Property(e => e.AliasName).HasDefaultValueSql("('')");
                entity.Property(e => e.CountryCode).HasDefaultValueSql("('00')");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ReportFormatMaster>().HasData(
               new ReportFormatMaster() { ReportFormatMasterID = 1, FormatName = "AUWholesalerOutputTemplate",    AliasName = "AU Wholesaler Output Template",       CountryCode = "AU", IsActive = true },
               new ReportFormatMaster() { ReportFormatMasterID = 2, FormatName = "AUOtherChannelOutputTemplate", AliasName = "AU Other Channel Output Template",    CountryCode = "AU", IsActive = true },
               new ReportFormatMaster() { ReportFormatMasterID = 3, FormatName = "NZRebateCustomerOutputTemplate",      AliasName = "NZ Rebate Customer Output Template",         CountryCode = "NZ", IsActive = true },
               new ReportFormatMaster() { ReportFormatMasterID = 4, FormatName = "NZNonRebateCustomerOutputTemplate",  AliasName = "NZ Non-Rebate Customer Output Template",     CountryCode = "NZ", IsActive = true }
           );

            modelBuilder.Entity<ReportFormatFieldMaster>(entity =>
            {
                entity.ToTable("MST_ReportFormatFieldMaster", tb => tb.HasTrigger("UTR_MST_ReportFormatFieldMaster_Audit"));
                entity.ToTable("MST_ReportFormatFieldMaster", "dbo");
                //entity.HasKey(e => e.ReportFormatFieldMasterID);
                entity.HasIndex(e => e.FieldName).IsUnique(true);
                
                entity.Property(e => e.FieldDescription).HasDefaultValueSql("('')");
                entity.Property(e => e.DataType).HasDefaultValueSql("('')");
                entity.Property(e => e.AlignmentType).HasDefaultValueSql("('')");
                entity.Property(e => e.ColorCode).HasDefaultValueSql("('')");
                entity.Property(e => e.SequenceNo).HasDefaultValueSql("0");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            });


            modelBuilder.Entity<PriceFileHeader>(entity =>
            {
                entity.ToTable("TRN_PriceFileHeader", tb => tb.HasTrigger("UTR_TRN_PriceFileHeader_Audit"));
                entity.ToTable("TRN_PriceFileHeader", "dbo");
                entity.HasIndex(e => e.UserConfigSettingID).IsUnique(true);
                entity.Property(e => e.Status).HasDefaultValueSql("('')");
                entity.Property(e => e.StatusText).HasDefaultValueSql("('')");
                entity.Property(e => e.PercentCompleted).HasDefaultValueSql("0");
                entity.Property(e => e.IsCompleted).HasDefaultValueSql("0");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PriceFileDetails>(entity =>
            {
                entity.ToTable("TRN_PriceFileDetails", tb => tb.HasTrigger("UTR_TRN_PriceFileDetails_Audit"));
                entity.ToTable("TRN_PriceFileDetails", "dbo");
                entity.HasOne(m => m.PriceFileHeader).WithMany(t => t.PriceFileDetails).HasForeignKey(m => m.PriceFileHeaderID).OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => new {e.PriceFileHeaderID, e.CustomerNo}).IsUnique(false);
                entity.Property(e => e.CustomerNo).HasDefaultValueSql("('')");
                entity.Property(e => e.Prefix).HasDefaultValueSql("('')");
                entity.Property(e => e.CustomerCatNo).HasDefaultValueSql("('')");
                entity.Property(e => e.ColourCode).HasDefaultValueSql("('')");
                entity.Property(e => e.CustomerItemNo).HasDefaultValueSql("('')");
                entity.Property(e => e.SchneiderElectricMaterialReference).HasDefaultValueSql("('')");
                entity.Property(e => e.MaterialDescription).HasDefaultValueSql("('')");
                entity.Property(e => e.UOM).HasDefaultValueSql("('')");
                entity.Property(e => e.PriceDerivedFrom).HasDefaultValueSql("('')");
                entity.Property(e => e.Barcode).HasDefaultValueSql("('')");
                entity.Property(e => e.SAPCOS).HasDefaultValueSql("('')");
                entity.Property(e => e.ProductHierarchy).HasDefaultValueSql("('')");
                entity.Property(e => e.CartonQty).HasDefaultValueSql("('')");
                entity.Property(e => e.StockStatus).HasDefaultValueSql("('')");
                entity.Property(e => e.FileReferenceData).HasDefaultValueSql("('')");
                entity.Property(e => e.Currency).HasDefaultValueSql("('')");
                entity.Property(e => e.VRG).HasDefaultValueSql("('')");
                entity.Property(e => e.VRGDescription).HasDefaultValueSql("('')");
                entity.Property(e => e.MaterialStatus).HasDefaultValueSql("('')");
                entity.Property(e => e.MainGroup).HasDefaultValueSql("('')");
                entity.Property(e => e.MainGroupDescription).HasDefaultValueSql("('')");
                entity.Property(e => e.Group).HasDefaultValueSql("('')");
                entity.Property(e => e.GroupDescription).HasDefaultValueSql("('')");
                entity.Property(e => e.SubGroup).HasDefaultValueSql("('')");
                entity.Property(e => e.SubGroupDescription).HasDefaultValueSql("('')");
                entity.Property(e => e.IsFound).HasDefaultValueSql("1");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.WholesaleListPriceExclGST).HasDefaultValueSql("0");
                entity.Property(e => e.WholesaleListPriceInclGST).HasDefaultValueSql("0");

                entity.Property(e => e.Per).HasDefaultValueSql("0");
                entity.Property(e => e.UOM).HasDefaultValueSql("0");
                entity.Property(e => e.OrderMultiple).HasDefaultValueSql("0");
                entity.Property(e => e.RecommendedRetailPrice).HasDefaultValueSql("0");
                entity.Property(e => e.AdvertisedRecommendedRetailPrice).HasDefaultValueSql("0");

                entity.Property(e => e.PriceBreak1CustomerQty).HasDefaultValueSql("0");
                entity.Property(e => e.PriceBreak1CustomerDiscount).HasDefaultValueSql("0");
                entity.Property(e => e.PriceBreak1CustomerCostExclGST).HasDefaultValueSql("0");
                entity.Property(e => e.PriceBreak1CustomerCostInclGST).HasDefaultValueSql("0");

                entity.Property(e => e.PriceBreak2CustomerQty).HasDefaultValueSql("0");
                entity.Property(e => e.PriceBreak2CustomerDiscount).HasDefaultValueSql("0");
                entity.Property(e => e.PriceBreak2CustomerCostExclGST).HasDefaultValueSql("0");
                entity.Property(e => e.PriceBreak2CustomerCostInclGST).HasDefaultValueSql("0");

                entity.Property(e => e.PriceBreak3CustomerQty).HasDefaultValueSql("0");
                entity.Property(e => e.PriceBreak3CustomerDiscount).HasDefaultValueSql("0");
                entity.Property(e => e.PriceBreak3CustomerCostExclGST).HasDefaultValueSql("0");
                entity.Property(e => e.PriceBreak3CustomerCostInclGST).HasDefaultValueSql("0");

                entity.Property(e => e.PriceBreak4CustomerQty).HasDefaultValueSql("0");
                entity.Property(e => e.PriceBreak4CustomerDiscount).HasDefaultValueSql("0");
                entity.Property(e => e.PriceBreak4CustomerCostExclGST).HasDefaultValueSql("0");
                entity.Property(e => e.PriceBreak4CustomerCostInclGST).HasDefaultValueSql("0");


                entity.Property(e => e.PriceBreak5CustomerQty).HasDefaultValueSql("0");
                entity.Property(e => e.PriceBreak5CustomerDiscount).HasDefaultValueSql("0");
                entity.Property(e => e.PriceBreak5CustomerCostExclGST).HasDefaultValueSql("0");
                entity.Property(e => e.PriceBreak5CustomerCostInclGST).HasDefaultValueSql("0");
            });


            modelBuilder.Entity<PriceFileLocationDetails>(entity =>
            {
                entity.ToTable("TRN_PriceFileLocationDetails", tb => tb.HasTrigger("UTR_TRN_PriceFileLocationDetails_Audit"));
                entity.ToTable("TRN_PriceFileLocationDetails", "dbo");
                entity.HasOne(m => m.PriceFileHeader).WithMany(t => t.PriceFileLocationDetails).HasForeignKey(m => m.PriceFileHeaderID).OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => new {e.PriceFileHeaderID, e.CustomerNo}).IsUnique(true);
                entity.Property(e => e.CustomerNo).HasDefaultValueSql("('')");
                entity.Property(e => e.PFCActualFileName).HasDefaultValueSql("('')");
                entity.Property(e => e.PFCEncryptedFileName).HasDefaultValueSql("('')");
                entity.Property(e => e.PFCFileLocationMode).HasDefaultValueSql("('')");
                entity.Property(e => e.PFCFileType).HasDefaultValueSql("('')");               
                entity.Property(e => e.PFCFilePath).HasDefaultValueSql("('')");
                entity.Property(e => e.PFCFileSize).HasDefaultValueSql("('')");
                entity.Property(e => e.Status).HasDefaultValueSql("('')");
                entity.Property(e => e.StatusText).HasDefaultValueSql("('')");
                entity.Property(e => e.PercentCompleted).HasDefaultValueSql("0");
                entity.Property(e => e.IsCompleted).HasDefaultValueSql("0");
                entity.Property(e => e.ReDownloadCount).HasDefaultValueSql("0");
                entity.Property(e => e.ReDownloadStatus).HasDefaultValueSql("('')");
                entity.Property(e => e.ReDownloadStatusText).HasDefaultValueSql("('')");
                entity.Property(e => e.ReDownloadPercentCompleted).HasDefaultValueSql("0");
                entity.Property(e => e.IsReDownloadCompleted).HasDefaultValueSql("0");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PriceFileLog>(entity =>
            {
                entity.ToTable("TRN_PriceFileLog", tb => tb.HasTrigger("UTR_TRN_PriceFileLog_Audit"));
                entity.ToTable("TRN_PriceFileLog", "dbo");
                entity.Property(e => e.LogType).HasDefaultValueSql("('')");
                entity.Property(e => e.FunctionName).HasDefaultValueSql("('')");
                entity.Property(e => e.LogInformation).HasDefaultValueSql("('')");
                entity.Property(e => e.LogReference1).HasDefaultValueSql("('')");
                entity.Property(e => e.LogReference2).HasDefaultValueSql("('')");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            });



            modelBuilder.Entity<ReportFormatFieldMaster>().HasData(
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 1, FieldName = "Prefix", FieldDescription = "Prefix",                                      DataType = "VARCHAR", SequenceNo =1, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 2, FieldName = "CustomerCatNo", FieldDescription = "CustomerCatNo",                        DataType = "VARCHAR", SequenceNo = 2, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 3, FieldName = "ColourCode", FieldDescription = "ColourCode",                              DataType = "VARCHAR", SequenceNo = 3, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 4, FieldName = "CustomerItemNo", FieldDescription = "CustomerItemNo",                      DataType = "VARCHAR", SequenceNo = 4, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 5, FieldName = "SchneiderElectricMaterialReference", FieldDescription = "SchneiderElectricMaterialReference", DataType = "VARCHAR", SequenceNo = 5, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 6, FieldName = "MaterialDescription", FieldDescription = "MaterialDescription",                                DataType = "VARCHAR", SequenceNo = 6, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 7, FieldName = "WholesaleListPriceExclGST", FieldDescription = "WholesaleListPriceExclGST",  DataType = "float", SequenceNo = 7, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 8, FieldName = "WholesaleListPriceInclGST", FieldDescription = "WholesaleListPriceInclGST",  DataType = "float", SequenceNo = 8, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 9, FieldName = "Per", FieldDescription = "Per",                                                DataType = "float", SequenceNo = 9, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 10, FieldName = "UOM", FieldDescription = "UOM",                                               DataType = "VARCHAR", SequenceNo = 10, IsActive = true },

               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 11, FieldName = "MOQ", FieldDescription = "MOQ_MinimumOrderQuantity",     DataType = "int", SequenceNo = 11, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 12, FieldName = "OrderMultiple", FieldDescription = "OrderMultiple",                           DataType = "float", SequenceNo = 12, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 13, FieldName = "RecommendedRetailPrice", FieldDescription = "RRP_RecommendedRetailPrice", DataType = "float", SequenceNo = 13, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 14, FieldName = "AdvertisedRecommendedRetailPrice", FieldDescription = "ARRP_AdvertisedRecommendedRetailPrice", DataType = "float", SequenceNo = 14, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 15, FieldName = "PriceDerivedFrom", FieldDescription = "PriceDerivedFrom",                     DataType = "VARCHAR", SequenceNo = 15, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 16, FieldName = "PriceBreak1CustomerQty", FieldDescription = "PriceBreak1CustomerQty",       DataType = "int", SequenceNo = 16, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 17, FieldName = "PriceBreak1CustomerDiscount", FieldDescription = "PriceBreak1CustomerDiscount", DataType = "float", SequenceNo = 17, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 18, FieldName = "PriceBreak1CustomerCostExclGST", FieldDescription = "PriceBreak1CustomerCostExclGST", DataType = "float", SequenceNo = 18, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 19, FieldName = "PriceBreak1CustomerCostInclGST", FieldDescription = "PriceBreak1CustomerCostInclGST", DataType = "float", SequenceNo = 19, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 20, FieldName = "PriceBreak2CustomerQty", FieldDescription = "PriceBreak2CustomerQty",       DataType = "int", SequenceNo = 20, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 21, FieldName = "PriceBreak2CustomerDiscount", FieldDescription = "PriceBreak2CustomerDiscount", DataType = "float", SequenceNo = 21, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 22, FieldName = "PriceBreak2CustomerCostExclGST", FieldDescription = "PriceBreak2CustomerCostExclGST", DataType = "float", SequenceNo = 22, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 23, FieldName = "PriceBreak2CustomerCostInclGST", FieldDescription = "PriceBreak2CustomerCostInclGST", DataType = "float", SequenceNo = 23, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 24, FieldName = "PriceBreak3CustomerQty", FieldDescription = "PriceBreak3CustomerQty",       DataType = "int", SequenceNo = 24, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 25, FieldName = "PriceBreak3CustomerDiscount", FieldDescription = "PriceBreak3CustomerDiscount", DataType = "float", SequenceNo = 25, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 26, FieldName = "PriceBreak3CustomerCostExclGST", FieldDescription = "PriceBreak3CustomerCostExclGST", DataType = "float", SequenceNo = 26, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 27, FieldName = "PriceBreak3CustomerCostInclGST", FieldDescription = "PriceBreak3CustomerCostInclGST", DataType = "float", SequenceNo = 27, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 28, FieldName = "PriceBreak4CustomerQty", FieldDescription = "PriceBreak4CustomerQty", DataType = "int", SequenceNo = 28, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 29, FieldName = "PriceBreak4CustomerDiscount", FieldDescription = "PriceBreak4CustomerDiscount", DataType = "float", SequenceNo = 29, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 30, FieldName = "PriceBreak4CustomerCostExclGST", FieldDescription = "PriceBreak4CustomerCostExclGST", DataType = "float", SequenceNo = 30, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 31, FieldName = "PriceBreak4CustomerCostInclGST", FieldDescription = "PriceBreak4CustomerCostInclGST", DataType = "float", SequenceNo = 31, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 32, FieldName = "PriceBreak5CustomerQty", FieldDescription = "PriceBreak5CustomerQty", DataType = "int", SequenceNo = 32, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 33, FieldName = "PriceBreak5CustomerDiscount", FieldDescription = "PriceBreak5CustomerDiscount", DataType = "float", SequenceNo = 33, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 34, FieldName = "PriceBreak5CustomerCostExclGST", FieldDescription = "PriceBreak5CustomerCostExclGST", DataType = "float", SequenceNo = 34, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 35, FieldName = "PriceBreak5CustomerCostInclGST", FieldDescription = "PriceBreak5CustomerCostInclGST", DataType = "float", SequenceNo = 35, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 36, FieldName = "Barcode", FieldDescription = "Barcode",                   DataType = "VARCHAR", SequenceNo = 36, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 37, FieldName = "SAP COS", FieldDescription = "SAP COS",           DataType = "VARCHAR", SequenceNo = 37, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 38, FieldName = "CartonQty", FieldDescription = "CartonQty",               DataType = "VARCHAR", SequenceNo = 38, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 39, FieldName = "StockStatus", FieldDescription = "StockStatus",           DataType = "VARCHAR", SequenceNo = 39, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 40, FieldName = "ValidFrom", FieldDescription = "ValidFrom",               DataType = "Date", SequenceNo = 40, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 41, FieldName = "ValidTo", FieldDescription = "ValidTo",                   DataType = "Date", SequenceNo = 41, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 42, FieldName = "FileReferenceData", FieldDescription = "FileReferenceData", DataType = "VARCHAR", SequenceNo = 42, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 43, FieldName = "Currency", FieldDescription = "Currency",                     DataType = "VARCHAR", SequenceNo = 43, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 44, FieldName = "VRG", FieldDescription = "VRG",                               DataType = "VARCHAR", SequenceNo = 44, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 45, FieldName = "VRGDescription", FieldDescription = "VRGDescription",         DataType = "VARCHAR", SequenceNo = 45, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 46, FieldName = "MaterialStatus", FieldDescription = "MaterialStatus", DataType = "VARCHAR", SequenceNo = 46, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 47, FieldName = "MainGroup", FieldDescription = "MainGroup",                   DataType = "VARCHAR", SequenceNo = 47, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 48, FieldName = "MainGroupDescription", FieldDescription = "MainGroupDescription", DataType = "VARCHAR", SequenceNo = 48, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 49, FieldName = "Group", FieldDescription = "Group",                               DataType = "VARCHAR", SequenceNo = 49, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 50, FieldName = "GroupDescription", FieldDescription = "GroupDescription",         DataType = "VARCHAR", SequenceNo = 50, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 51, FieldName = "SubGroup", FieldDescription = "SubGroup",   DataType = "VARCHAR", SequenceNo = 51, IsActive = true },
               new ReportFormatFieldMaster() { ReportFormatFieldMasterID = 52, FieldName = "SubGroupDescription", FieldDescription = "SubGroupDescription", DataType = "VARCHAR", SequenceNo = 52, IsActive = true }
           );

            modelBuilder.Entity<ReportFormatFieldMapping>(entity =>
            {
                entity.ToTable("MST_ReportFormatFieldMapping", tb => tb.HasTrigger("UTR_MST_ReportFormatFieldMapping_Audit"));
                entity.ToTable("MST_ReportFormatFieldMapping", "dbo");
                //entity.HasKey(e => e.ReportFormatFieldMappingID);
                entity.HasIndex(e => new { e.ReportFormatMasterID, e.ReportFormatFieldMasterID }).IsUnique(true);
                entity.HasOne(m => m.ReportFormatMaster).WithMany(t => t.ReportFormatFieldMapping).HasForeignKey(m => m.ReportFormatMasterID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(m => m.ReportFormatFieldMaster).WithMany(t => t.ReportFormatFieldMapping).HasForeignKey(m => m.ReportFormatFieldMasterID).OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.AliasName).HasDefaultValueSql("('')");
                entity.Property(e => e.SequenceNo).HasDefaultValueSql("0");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
                entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<ReportFormatFieldMapping>().HasData(
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 1, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 1, AliasName = "Prefix", SequenceNo = 1, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 2, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 2, AliasName = "Customer Cat No", SequenceNo = 2, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 3, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 3, AliasName = "Colour Code", SequenceNo = 3, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 4, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 4, AliasName = "Customer Item No", SequenceNo = 4, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 5, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 5, AliasName = "Schneider Electric Material Reference", SequenceNo = 5, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 6, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 6, AliasName = "Description", SequenceNo = 6, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 7, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 7, AliasName = "Wholesale List Price (excl GST)", SequenceNo = 7, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 8, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 8, AliasName = "Wholesale List Price (incl GST)", SequenceNo = 8, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 9, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 9, AliasName = "Per", SequenceNo = 9, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 10, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 10, AliasName = "UOM", SequenceNo = 10, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 11, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 11, AliasName = "MOQ (Minimum Order Quantity)", SequenceNo = 11, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 12, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 12, AliasName = "Order Multiple", SequenceNo = 12, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 13, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 13, AliasName = "RRP (Recommended Retail Price)", SequenceNo = 13, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 14, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 14, AliasName = "ARRP (Advertised Recommended Retail Price, displayed on Clipsal.com)", SequenceNo = 14, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 15, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 15, AliasName = "Price derived from", SequenceNo = 15, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 16, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 16, AliasName = "Price Break 1 - CUSTOMER QTY", SequenceNo = 16, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 17, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 17, AliasName = "Price Break 1 - CUSTOMER Discount", SequenceNo = 17, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 18, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 18, AliasName = "Price Break 1 - CUSTOMER Cost (excl GST)", SequenceNo = 18, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 19, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 19, AliasName = "Price Break 1 - CUSTOMER Cost (incl GST)", SequenceNo = 19, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 20, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 20, AliasName = "Price Break 2 - CUSTOMER QTY", SequenceNo = 20, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 21, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 21, AliasName = "Price Break 2 - CUSTOMER Discount", SequenceNo = 21, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 22, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 22, AliasName = "Price Break 2 - CUSTOMER Cost (excl GST)", SequenceNo = 22, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 23, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 23, AliasName = "Price Break 2 - CUSTOMER Cost (incl GST)", SequenceNo = 23, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 24, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 24, AliasName = "Price Break 3 - CUSTOMER QTY", SequenceNo = 24, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 25, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 25, AliasName = "Price Break 3 - CUSTOMER Discount", SequenceNo = 25, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 26, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 26, AliasName = "Price Break 3 - CUSTOMER Cost (excl GST)", SequenceNo = 26, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 27, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 27, AliasName = "Price Break 3 - CUSTOMER Cost (incl GST)", SequenceNo = 27, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 28, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 28, AliasName = "Price Break 4 - CUSTOMER QTY", SequenceNo = 28, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 29, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 29, AliasName = "Price Break 4 - CUSTOMER Discount", SequenceNo = 29, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 30, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 30, AliasName = "Price Break 4 - CUSTOMER Cost (excl GST)", SequenceNo = 30, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 31, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 31, AliasName = "Price Break 4 - CUSTOMER Cost (incl GST)", SequenceNo = 31, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 32, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 32, AliasName = "Price Break 5 - CUSTOMER QTY", SequenceNo = 32, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 33, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 33, AliasName = "Price Break 5 - CUSTOMER Discount", SequenceNo = 33, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 34, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 34, AliasName = "Price Break 5 - CUSTOMER Cost (excl GST)", SequenceNo = 34, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 35, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 35, AliasName = "Price Break 5 - CUSTOMER Cost (incl GST)", SequenceNo = 35, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 36, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 36, AliasName = "Barcode", SequenceNo = 36, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 37, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 37, AliasName = "SAP Local COS", SequenceNo = 37, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 38, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 38, AliasName = "Carton Qty", SequenceNo = 38, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 39, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 39, AliasName = "Stock Status (S = Stockable, * = Not normally stocked in Australia)", SequenceNo = 39, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 40, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 40, AliasName = "Valid From", SequenceNo = 40, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 41, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 41, AliasName = "Valid To", SequenceNo = 41, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 42, ReportFormatMasterID = 1, ReportFormatFieldMasterID = 42, AliasName = "File Reference Data", SequenceNo = 42, IsActive = true },


               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 43, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 5, AliasName = "Schneider Electric Material Reference", SequenceNo = 1, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 44, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 6, AliasName = "Description", SequenceNo = 2, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 45, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 7, AliasName = "Wholesale List Price (excl GST)", SequenceNo = 3, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 46, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 8, AliasName = "Wholesale List Price (incl GST)", SequenceNo = 4, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 47, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 9, AliasName = "Per", SequenceNo = 5, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 48, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 10, AliasName = "UOM", SequenceNo = 6, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 49, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 11, AliasName = "MOQ (Minimum Order Quantity)", SequenceNo = 7, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 50, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 12, AliasName = "Order Multiple", SequenceNo = 8, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 51, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 15, AliasName = "Price derived from", SequenceNo = 9, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 52, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 16, AliasName = "Price Break 1 - CUSTOMER QTY", SequenceNo = 10, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 53, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 17, AliasName = "Price Break 1 - CUSTOMER Discount", SequenceNo = 11, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 54, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 18, AliasName = "Price Break 1 - CUSTOMER Cost (excl GST)", SequenceNo = 12, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 55, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 19, AliasName = "Price Break 1 - CUSTOMER Cost (incl GST)", SequenceNo = 13, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 56, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 20, AliasName = "Price Break 2 - CUSTOMER QTY", SequenceNo = 14, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 57, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 21, AliasName = "Price Break 2 - CUSTOMER Discount", SequenceNo = 15, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 58, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 22, AliasName = "Price Break 2 - CUSTOMER Cost (excl GST)", SequenceNo = 16, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 59, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 23, AliasName = "Price Break 2 - CUSTOMER Cost (incl GST)", SequenceNo = 17, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 60, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 24, AliasName = "Price Break 3 - CUSTOMER QTY", SequenceNo = 18, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 61, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 25, AliasName = "Price Break 3 - CUSTOMER Discount", SequenceNo = 19, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 62, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 26, AliasName = "Price Break 3 - CUSTOMER Cost (excl GST)", SequenceNo = 20, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 63, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 27, AliasName = "Price Break 3 - CUSTOMER Cost (incl GST)", SequenceNo = 21, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 64, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 28, AliasName = "Price Break 4 - CUSTOMER QTY", SequenceNo = 22, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 65, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 29, AliasName = "Price Break 4 - CUSTOMER Discount", SequenceNo = 23, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 66, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 30, AliasName = "Price Break 4 - CUSTOMER Cost (excl GST)", SequenceNo = 24, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 67, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 31, AliasName = "Price Break 4 - CUSTOMER Cost (incl GST)", SequenceNo = 25, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 68, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 32, AliasName = "Price Break 5 - CUSTOMER QTY", SequenceNo = 26, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 69, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 33, AliasName = "Price Break 5 - CUSTOMER Discount", SequenceNo = 27, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 70, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 34, AliasName = "Price Break 5 - CUSTOMER Cost (excl GST)", SequenceNo = 28, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 71, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 35, AliasName = "Price Break 5 - CUSTOMER Cost (incl GST)", SequenceNo = 29, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 72, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 36, AliasName = "Barcode", SequenceNo = 30, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 73, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 37, AliasName = "SAP Local COS", SequenceNo = 31, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 74, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 38, AliasName = "Carton Qty", SequenceNo = 32, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 75, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 39, AliasName = "Stock Status (S = Stockable, * = Not normally stocked in Australia)", SequenceNo = 33, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 76, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 40, AliasName = "Valid From", SequenceNo = 34, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 77, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 41, AliasName = "Valid To", SequenceNo = 35, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 78, ReportFormatMasterID = 2, ReportFormatFieldMasterID = 42, AliasName = "File Reference Data", SequenceNo = 36, IsActive = true },

               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 79, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 5, AliasName = "Schneider Electric Material Reference", SequenceNo = 1, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 80, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 6, AliasName = "Material Description", SequenceNo = 2, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 81, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 7, AliasName = "List Price", SequenceNo = 3, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 82, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 43, AliasName = "Currency", SequenceNo = 4, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 83, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 9, AliasName = "Per", SequenceNo = 5, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 84, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 10, AliasName = "Price Unit", SequenceNo = 6, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 85, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 12, AliasName = "Order in Multiples of", SequenceNo = 7, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 86, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 44, AliasName = "VRG", SequenceNo = 8, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 87, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 45, AliasName = "VRG Description", SequenceNo = 9, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 88, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 46, AliasName = "Material Status", SequenceNo = 10, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 89, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 16, AliasName = "Quantity Break", SequenceNo = 11, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 90, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 17, AliasName = "Qty Discount or Price", SequenceNo = 12, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 91, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 18, AliasName = "Qty Buy Price", SequenceNo = 13, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 92, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 20, AliasName = "Quantity Break 2", SequenceNo = 14, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 93, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 21, AliasName = "Qty Discount or Price 2", SequenceNo = 15, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 94, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 22, AliasName = "Qty Buy Price 2", SequenceNo = 16, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 95, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 24, AliasName = "Qty Break 3", SequenceNo = 17, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 96, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 25, AliasName = "Qty Discount or Price 3", SequenceNo = 18, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 97, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 26, AliasName = "Qty Buy Price 3", SequenceNo = 19, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 98, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 28, AliasName = "Qty Break 4", SequenceNo = 20, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 99, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 29, AliasName = "Qty Discount or Price 4", SequenceNo = 21, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 100, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 30, AliasName = "Qty Buy Price 4", SequenceNo = 22, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 101, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 32, AliasName = "Qty Break 5", SequenceNo = 23, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 102, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 33, AliasName = "Qty Discount or Price 5", SequenceNo = 24, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 103, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 34, AliasName = "Qty Buy Price 5", SequenceNo = 25, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 104, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 36, AliasName = "EAN/UPC", SequenceNo = 26, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 105, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 47, AliasName = "Main Group", SequenceNo = 27, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 106, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 48, AliasName = "Main Group Description", SequenceNo = 28, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 107, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 49, AliasName = "Group", SequenceNo = 29, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 108, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 50, AliasName = "Group Description", SequenceNo = 30, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 109, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 51, AliasName = "SubGroup", SequenceNo = 31, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 110, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 52, AliasName = "SubGroup Description", SequenceNo = 32, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 111, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 38, AliasName = "CartonQty", SequenceNo = 33, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 112, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 39, AliasName = "Stock or Non Stock", SequenceNo = 34, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 113, ReportFormatMasterID = 3, ReportFormatFieldMasterID = 40, AliasName = "Effective Date", SequenceNo = 35, IsActive = true },


               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 114, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 5, AliasName = "Schneider Electric Material Reference", SequenceNo = 1, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 115, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 6, AliasName = "Material Description", SequenceNo = 2, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 116, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 7, AliasName = "List Price", SequenceNo = 3, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 117, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 43, AliasName = "Currency", SequenceNo = 4, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 118, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 9, AliasName = "Per", SequenceNo = 5, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 119, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 10, AliasName = "Price Unit", SequenceNo = 6, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 120, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 12, AliasName = "Order in Multiples of", SequenceNo = 7, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 121, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 46, AliasName = "Material Status", SequenceNo = 8, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 122, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 16, AliasName = "Quantity Break", SequenceNo = 9, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 123, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 17, AliasName = "Qty Discount or Price", SequenceNo = 10, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 124, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 18, AliasName = "Qty Buy Price", SequenceNo = 11, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 125, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 20, AliasName = "Quantity Break 2", SequenceNo = 12, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 126, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 21, AliasName = "Qty Discount or Price 2", SequenceNo = 13, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 127, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 22, AliasName = "Qty Buy Price 2", SequenceNo = 14, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 128, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 24, AliasName = "Qty Break 3", SequenceNo = 15, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 129, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 25, AliasName = "Qty Discount or Price 3", SequenceNo = 16, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 130, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 26, AliasName = "Qty Buy Price 3", SequenceNo = 17, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 131, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 28, AliasName = "Qty Break 4", SequenceNo = 18, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 132, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 29, AliasName = "Qty Discount or Price 4", SequenceNo = 19, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 133, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 30, AliasName = "Qty Buy Price 4", SequenceNo = 20, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 134, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 32, AliasName = "Qty Break 5", SequenceNo = 21, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 135, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 33, AliasName = "Qty Discount or Price 5", SequenceNo = 22, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 136, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 34, AliasName = "Qty Buy Price 5", SequenceNo = 23, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 137, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 36, AliasName = "EAN/UPC", SequenceNo = 24, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 138, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 47, AliasName = "Main Group", SequenceNo = 25, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 139, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 48, AliasName = "Main Group Description", SequenceNo = 26, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 140, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 49, AliasName = "Group", SequenceNo = 27, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 141, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 50, AliasName = "Group Description", SequenceNo = 28, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 142, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 51, AliasName = "SubGroup", SequenceNo = 29, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 143, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 52, AliasName = "SubGroup Description", SequenceNo = 30, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 144, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 38, AliasName = "CartonQty", SequenceNo = 31, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 145, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 39, AliasName = "Stock or Non Stock", SequenceNo = 32, IsActive = true },
               new ReportFormatFieldMapping() { ReportFormatFieldMappingID = 146, ReportFormatMasterID = 4, ReportFormatFieldMasterID = 40, AliasName = "Effective Date", SequenceNo = 33, IsActive = true }




           );


            modelBuilder.Entity<NotificationTemplates>(entity =>
            {
                entity.ToTable("MST_NotificationTemplate", tb => tb.HasTrigger("UTR_MST_NotificationTemplate_Audit"));
                entity.ToTable("MST_NotificationTemplate", "dbo");
                entity.HasIndex(e => new { e.SalesOrganization, e.TemplateName }).IsUnique(true);
                entity.Property(e => e.SalesOrganization).HasDefaultValueSql("('')");
                entity.Property(e => e.TemplateName).HasDefaultValueSql("('')");
                entity.Property(e => e.TemplateSubject).HasDefaultValueSql("('')");
                entity.Property(e => e.TemplateBody).HasDefaultValueSql("('')");
                entity.Property(e => e.TemplateVars).HasDefaultValueSql("('')");
                entity.Property(e => e.DefaultSentTo).HasDefaultValueSql("('')");
                entity.Property(e => e.DefaultCcTo).HasDefaultValueSql("('')");
                entity.Property(e => e.DefaultBccTo).HasDefaultValueSql("('')");
                entity.Property(e => e.Priority).HasDefaultValueSql("0");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
                entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<NotificationTemplates>().HasData(
              new NotificationTemplates() { NotificationTemplateID =1, SalesOrganization = "AU01", TemplateName = "AU01PriceFileDistribution", TemplateSubject = "[NEW Price File] Customer_No: {{Customer_No}}  // Customer_Name: {{Customer_Name}}", TemplateBody = "<p>Dear Customer,</p><p>The attached Price File have been digitally approved.</p><p>PLEASE DO NOT REPLY TO THIS AUTOMATICALLY GENERATED EMAIL. If you wish to reply, please remove this email address.</p><p>From <em>Notification Services</em><br /><strong>Price File Creator</strong></p>", TemplateVars = "Customer_No, Customer_Name", DefaultSentTo ="", DefaultCcTo="", DefaultBccTo="",IsActive = true, Priority =2 },
              new NotificationTemplates() { NotificationTemplateID =2, SalesOrganization = "NZ01", TemplateName = "NZ01PriceFileDistribution", TemplateSubject = "[NEW Price File] Customer_No: {{Customer_No}}  // Customer_Name: {{Customer_Name}}", TemplateBody = "<p>Dear Customer,</p><p>The attached Price File have been digitally approved.</p><p>PLEASE DO NOT REPLY TO THIS AUTOMATICALLY GENERATED EMAIL. If you wish to reply, please remove this email address.</p><p>From <em>Notification Services</em><br /><strong>Price File Creator</strong></p>", TemplateVars = "Customer_No, Customer_Name", DefaultSentTo = "", DefaultCcTo = "", DefaultBccTo = "", IsActive = true, Priority=2 }

              );

            modelBuilder.Entity<NotificationHistory>(entity =>
            {
                entity.ToTable("TRN_NotificationHistory", tb => tb.HasTrigger("UTR_TRN_NotificationHistory_Audit"));
                entity.ToTable("TRN_NotificationHistory", "dbo");
                entity.Property(e => e.NotificationDate).HasDefaultValueSql("(getutcdate())");
                entity.Property(e => e.Subject).HasDefaultValueSql("('')");
                entity.Property(e => e.Body).HasDefaultValueSql("('')");
                entity.Property(e => e.SentTo).HasDefaultValueSql("('')");
                entity.Property(e => e.CcTo).HasDefaultValueSql("('')");
                entity.Property(e => e.BccTo).HasDefaultValueSql("('')");
                entity.Property(e => e.Priority).HasDefaultValueSql("0");
                entity.Property(e => e.Status).HasDefaultValueSql("('')");
                entity.Property(e => e.StatusDate).HasDefaultValueSql("(getutcdate())");
                entity.Property(e => e.AttachmentPath).HasDefaultValueSql("('')");
                entity.Property(e => e.ActualFileName).HasDefaultValueSql("('')");
                entity.Property(e => e.EncryptedFileName).HasDefaultValueSql("('')");
                entity.Property(e => e.NotificationTemplateID).HasDefaultValueSql("0");
                entity.Property(e => e.PriceFileHeaderID).HasDefaultValueSql("0");
                entity.Property(e => e.PriceFileLocationID).HasDefaultValueSql("0");
                entity.Property(e => e.ResendCount).HasDefaultValueSql("0");
                entity.Property(e => e.ResendStatus).HasDefaultValueSql("('')");
                entity.Property(e => e.IsActive).HasDefaultValueSql("1");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("0");
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("('')");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
                entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getutcdate())");
            });

            #endregion

        }
    }
}
