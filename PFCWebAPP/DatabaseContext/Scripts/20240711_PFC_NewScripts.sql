--------Create a new Table MST_CustomerSettings
CREATE TABLE MST_CustomerSettings (
    CustomerNumber nvarchar(100) PRIMARY KEY,
    SalesOrganization VARCHAR(10) NOT NULL,
    CanUseAutoReportContent BIT NULL,
    ReportContentTemplateID BIGINT NOT NULL,
	ReportFormatTemplateID BIGINT NOT NULL,
	SelectedCustomersTemplateID BIGINT NOT NULL,
	CanIncludeTradePrices BIT NOT NULL,
	CanIncludeCustomerNetPrices BIT NOT NULL,
	CanIncludeCustomerHierarchyNetPrices BIT NOT NULL,
	CanIncludeOverallNetPrices BIT NOT NULL,
	CanIncludePriceGroupNets BIT NOT NULL,
	CanIncludeSellOffPrices BIT NOT NULL,
	CanIncludeDiscount1 BIT NOT NULL,
	CanIncludeDiscount2 BIT NOT NULL,
	CanIncludeDiscount3 BIT NOT NULL,
	CanIncludeDiscount4 BIT NOT NULL,
	CanIncludeDiscount5 BIT NOT NULL,
	CanIncludeDiscount6 BIT NOT NULL,
	CanIncludeDiscount7 BIT NOT NULL,
	CanIncludeDiscount8 BIT NOT NULL,
	CanIncludePromoPrice BIT NOT NULL,
	CanUseShiftBreaks BIT NOT NULL,
	CanUseMOQAsBrk1 BIT NOT NULL,
	CanUseGlobalCOSForProductHierarchy BIT NOT NULL,
	CanUseLocalCOSForProductHierarchy BIT NOT NULL,
	CanAddSODInFinalPrice BIT NOT NULL,
	SODInFinalPriceValue FLOAT NOT NULL,
	CanUseAlternateValidFromDate BIT NOT NULL,
	AlternateValidFromDate DATE,
	CanShowTemplateMaterialOnly BIT NOT NULL,
	CanShowNotFoundTemplateMaterials BIT NOT NULL,
	CanSendEmail BIT NOT NULL,
	IsActive BIT NOT NULL,
	IsDeleted BIT NOT NULL,
	CreatedBy VARCHAR(100) NOT NULL,
	ModifiedBy VARCHAR(100) NOT NULL,
	CreatedDate DATETIME NOT NULL,
	ModifiedDate DATETIME NOT NULL
);


------Insert into TemplateMaster

DECALRE @temp_masterId int;
DECLARE @UserSESA varchar(10);

set @UserSESA = 'SESA715213';

INSERT INTO [dbo].[MST_TemplateMaster]
VALUES(2,'CustomerSettings','Customer Settings','Table','00',0,1,1,1,0,@UserSESA,'',GetDate(),'')

set @temp_masterId = (SELECT Max(TemplateMasterId) from [dbo].[MST_TemplateMaster]);


---------Insert into TemplateStructure
INSERT INTO TemplateStructure
VALUES(@temp_masterId, 'CustomerNumber','CustomerNumber','VARCHAR',1,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'SalesOrganization','SalesOrganization','VARCHAR',2,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'CanIncludeTradePrices','CanIncludeTradePrices','BIT',3,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'CanIncludeCustomerNetPrices','CanIncludeCustomerNetPrices','BIT',4,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'CanIncludeCustomerHierarchyNetPrices','CanIncludeCustomerHierarchyNetPrices','BIT',5,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'CanIncludeOverallNetPrices','CanIncludeOverallNetPrices','BIT',6,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'CanIncludePriceGroupNets','CanIncludePriceGroupNets','BIT',7,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'CanIncludeSellOffPrices','CanIncludeSellOffPrices','BIT',8,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'CanIncludePromoPrice','CanIncludePromoPrice','BIT',9,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'CanUseShiftBreaks','CanUseShiftBreaks','BIT',10,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'CanUseMOQAsBrk1','CanUseMOQAsBrk1','BIT',11,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'CanShowTemplateMaterialOnly','CanShowTemplateMaterialOnly','BIT',12,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'CanShowNotFoundTemplateMaterials','CanShowNotFoundTemplateMaterials','BIT',13,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'CanIncludeDiscount1','CanIncludeDiscount1','BIT',14,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'CanIncludeDiscount2','CanIncludeDiscount2','BIT',15,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'CanIncludeDiscount3','CanIncludeDiscount3','BIT',16,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'CanIncludeDiscount4','CanIncludeDiscount4','BIT',17,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'CanIncludeDiscount5','CanIncludeDiscount5','BIT',18,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'CanIncludeDiscount6','CanIncludeDiscount6','BIT',19,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'CanIncludeDiscount7','CanIncludeDiscount7','BIT',202,1,0,@UserSESA,GetDate()),
(@temp_masterId, 'CanIncludeDiscount8','CanIncludeDiscount8','BIT',21,1,0,@UserSESA,GetDate());



CREATE TABLE TRN_Queue
(
    QueueId INT IDENTITY (1, 1) PRIMARY KEY,
    SalesOrganization VARCHAR(10) NOT NULL,
    DistributionChannel VARCHAR(100) NOT NULL CONSTRAINT [DF_Queue_DistributionChannel] DEFAULT('OG'),
    CustomerId VARCHAR(100) NOT NULL,
    PricingFileType VARCHAR(100) NOT NULL CONSTRAINT [DF_Queue_PricingFileType] DEFAULT('Negotiated'),
    PriceStatus VARCHAR(100) NOT NULL CONSTRAINT [DF_Queue_PriceStatus] DEFAULT(''),
    PricingDate DATE NOT NULL CONSTRAINT [DF_Queue_PricingDate]  DEFAULT (getdate()),
    CustomerEmail NVARCHAR(255) NOT NULL,
	IsActive BIT NOT NULL CONSTRAINT [DF_Queue_IsActive] DEFAULT(1),
	CreatedBy VARCHAR(100) NOT NULL CONSTRAINT [DF_Queue_CreatedBy] DEFAULT(''),
	CreatedDate DATETIME NOT NULL CONSTRAINT [DF_Queue_CreatedDate]  DEFAULT (getutcdate()),
);


CREATE TABLE TRN_Queue_History
(
	QueueHistoryId INT IDENTITY (1, 1) PRIMARY KEY,
    SalesOrganization VARCHAR(10) NOT NULL,
	UserConfigId BIGINT NOT NULL  CONSTRAINT [DF_QueueHistory_UserConfigId] DEFAULT(0),
    DistributionChannel VARCHAR(100) NOT NULL CONSTRAINT [DF_QueueHistory_DistributionChannel] DEFAULT('OG'),
    CustomerId VARCHAR(100) NOT NULL,
    PricingFileType VARCHAR(100) NOT NULL CONSTRAINT [DF_QueueHistory_PricingFileType] DEFAULT('Negotiated'),
    PriceStatus VARCHAR(100) NOT NULL CONSTRAINT [DF_QueueHistory_PriceStatus] DEFAULT(''),
    PricingDate DATE NOT NULL CONSTRAINT [DF_QueueHistory_PricingDate]  DEFAULT (getdate()),
    CustomerEmail NVARCHAR(255) NOT NULL,
	QueueStatus NVARCHAR(MAX) NOT NULL  CONSTRAINT [DF_QueueHistory_QueueStatus] DEFAULT(''),
	QueueMessage NVARCHAR(MAX) NOT NULL  CONSTRAINT [DF_QueueHistory_QueueMessage] DEFAULT(''),
	IsActive BIT NOT NULL CONSTRAINT [DF_QueueHistory_IsActive] DEFAULT(1),
	IsDeleted BIT NOT NULL CONSTRAINT [DF_QueueHistory_IsDeleted] DEFAULT(0),
	CreatedBy VARCHAR(100) NOT NULL CONSTRAINT [DF_QueueHistory_CreatedBy] DEFAULT(''),
	CreatedDate DATETIME NOT NULL CONSTRAINT [DF_QueueHistory_CreatedDate]  DEFAULT (getutcdate()),
	ModifiedBy VARCHAR(100) NOT NULL CONSTRAINT [DF_QueueHistory_ModifiedBy] DEFAULT(''),
	ModifiedDate DATETIME NOT NULL CONSTRAINT [DF_QueueHistory_ModifiedDate]  DEFAULT (getutcdate())
);

insert into [dbo].[MST_AppConfig]
Values('ApiRequestLimitPerDay','API Request Limit per Day','Max API Requests that a user can request per Day','1','RateLimitPerDay','NUMBER','DropDownList',1,2,8,1,0,'SESA715213','',GETUTCDATE(),GETUTCDATE()),
	  ('PricingFutureDate','Pricing Future Date','Intermediate Date for API requests','2024-12-31','PricingDate','DATE','Date',10,20,7,1,0,'SESA715213','',GETUTCDATE(),GETUTCDATE()),
      ('ApiRequestLimitPerMonth','API Request Limit per Month','Max API Requests that a user can request per Month','5','RateLimitPerMonth','NUMBER','DropDownList',1,2,8,1,0,'SESA715213','',GETUTCDATE(),GETUTCDATE())



insert into MST_ConfigOptions
values
('RateLimitPerDay',1,1,1,         0,'SESA715213','',GetUTCDate(),GETUTCDATE()),
('RateLimitPerDay',2,2,1,		  0,'SESA715213','',GetUTCDate(),GETUTCDATE()),
('RateLimitPerDay',3,3,1,		  0,'SESA715213','',GetUTCDate(),GETUTCDATE()),
('RateLimitPerDay',4,4,1,		  0,'SESA715213','',GetUTCDate(),GETUTCDATE()),
('RateLimitPerDay',5,5,1,		  0,'SESA715213','',GetUTCDate(),GETUTCDATE()),
('RateLimitPerDay',6,6,1,		  0,'SESA715213','',GetUTCDate(),GETUTCDATE()),
('RateLimitPerDay',7,7,1,		  0,'SESA715213','',GetUTCDate(),GETUTCDATE()),
('RateLimitPerDay',8,8,1,		  0,'SESA715213','',GetUTCDate(),GETUTCDATE()),
('RateLimitPerDay',9,9,1,		  0,'SESA715213','',GetUTCDate(),GETUTCDATE()),
('RateLimitPerDay',10,10,1,	      0,'SESA715213','',GetUTCDate(),GETUTCDATE()),
('RateLimitPerMonth',5,1,1,       0,'SESA715213','',GetUTCDate(),GETUTCDATE()),
('RateLimitPerMonth',10,2,1,      0,'SESA715213','',GetUTCDate(),GETUTCDATE()),
('RateLimitPerMonth',15,3,1,      0,'SESA715213','',GetUTCDate(),GETUTCDATE()),
('RateLimitPerMonth',20,4,1,      0,'SESA715213','',GetUTCDate(),GETUTCDATE()),
('RateLimitPerMonth',25,5,1,      0,'SESA715213','',GetUTCDate(),GETUTCDATE()),
('RateLimitPerMonth',30,6,1,      0,'SESA715213','',GetUTCDate(),GETUTCDATE()),
('RateLimitPerMonth',35,7,1,      0,'SESA715213','',GetUTCDate(),GETUTCDATE()),
('RateLimitPerMonth',40,8,1,      0,'SESA715213','',GetUTCDate(),GETUTCDATE())



insert into MST_NotificationTemplate
values
('00','CurrentPriceFileDistribution',
'Schneider Electric Price File (Current) - [{{Customer_No}}] - {{Customer_Name}}',
'<p>CONFIDENTIAL</p>
<p>Dear Customer</p>
<p>Please find attached your current Schneider Electric company specific Price file, as requested via the mySchneider platform, or your account manager.</p>
<p>The file details your company pricing for the SE account number provided in the file name, valid from the date as shown.</p>
<p>The Net prices included are based on all compounded standard discounts or continued Special Price Agreements, however contract &/or post invoice discounts are not included.</p>
<br/>
<p>This is a system generated email based on a specific request via the mySchneider platform or an SE account manager.</p>
<p>If you did not request this file please disregard and delete.</p>
<p>Please notify your SE Account Manager or Representative of any issues.</p>
<br/><br/><p><em>Best regards</em></p>
<p>Pricing Operations and mySchneider teams</p>
<p>Schneider Electric Pacific Zone</p>',
'Customer_No, Customer_Name','','','Partner.PriceOperations@schneider-electric.com',2,1,0,'SESA715213','',GETUTCDATE(),GETUTCDATE()),


('00','PendingPriceFileDistribution',
'Schneider Electric Price File (Pending) - [{{Customer_No}}] - {{Customer_Name}}',
'<p>CONFIDENTIAL</p>
<p>Dear Customer</p>
<p>Please find attached your pending Schneider Electric company specific Price file, as requested via the mySchneider platform, or your account manager.</p>
<p>The file details your company pricing for the SE account number provided in the file name, valid from the date as shown.</p>
<p>The Net prices included are based on all compounded standard discounts or continued Special Price Agreements, however contract &/or post invoice discounts are not included.</p>
<br/><p>This is a system generated email based on a specific request via the mySchneider platform or an SE account manager.</p>
<p>If you did not request this file please disregard and delete.</p>
<p>Please notify your SE Account Manager or Representative of any issues.</p>
<br/><br/><p><em>Best regards</em></p>
<p>Pricing Operations and mySchneider teams</p>
<p>Schneider Electric Pacific Zone</p>',
'Customer_No, Customer_Name','','','Partner.PriceOperations@schneider-electric.com',2,1,0,'SESA715213','',GETUTCDATE(),GETUTCDATE()),


('00','CurrentPendingPriceFileDistribution',
'Schneider Electric Price File (Current and Pending) - [{{Customer_No}}] - {{Customer_Name}}',
'<p>CONFIDENTIAL</p>
<p>Dear Customer</p>
<p>Please find attached your current & pending Schneider Electric company specific Price file, as requested via the mySchneider platform, or your account manager.</p>
<p>The file details your company pricing for the SE account number provided in the file name, valid from the date as shown.</p>
<p>The Net prices included are based on all compounded standard discounts or continued Special Price Agreements, however contract &/or post invoice discounts are not included.</p>
<br/>
<p>This is a system generated email based on a specific request via the mySchneider platform or an SE account manager.</p>
<p>If you did not request this file please disregard and delete.</p>
<p>Please notify your SE Account Manager or Representative of any issues.</p>
<br/><br/>
<p><em>Best regards</em></p>
<p>Pricing Operations and mySchneider teams</p>
<p>Schneider Electric Pacific Zone</p>',
'Customer_No, Customer_Name','','','Partner.PriceOperations@schneider-electric.com',2,1,0,'SESA715213','',GETUTCDATE(),GETUTCDATE()),


('00','DailyLimitExceeded',
'Schneider Electric Price File (daily Quota exceeded) - [{{Customer_No}}] - {{Customer_Name}}',
'<p>CONFIDENTIAL</p>
<p>Dear Customer</p>
<p>A requested price file via the mySchneider platform was unable to be created due to the daily quota limit being exceeded.</p>
<p>To provide all our valued mySchneider accounts with fast performance Schneider Electric allows for 1 daily, and 5 monthly price file requests, per account/email combination. </p>
<p>Please allow 24 hours before initiating another request, ensuring not to exceed 5 requests within a monthly period.</p>
<br/>
<p>This is a system generated email based on a specific request via the mySchneider platform or an SE account manager.</p>
<p>If you did not request this file please disregard and delete.</p>
<p>Please notify your SE Account Manager or Representative of any issues.</p>
<br/><br/>
<p><em>Best regards</em></p>
<p>Pricing Operations and mySchneider teams</p>
<p>Schneider Electric Pacific Zone</p>',
'Customer_No, Customer_Name','','','Partner.PriceOperations@schneider-electric.com',2,1,0,'SESA715213','',GETUTCDATE(),GETUTCDATE()),


('00','MonthlyLimitExceeded',
'Schneider Electric Price File (monthly Quota exceeded) - [{{Customer_No}}] - {{Customer_Name}}',
'<p>CONFIDENTIAL</p>
<p>Dear Customer</p>
<p>A requested price file via the mySchneider platform was unable to be created due to the monthly quota limit being exceeded.</p>
<p>To provide all our valued mySchneider accounts with fast performance Schneider Electric allows for 1 daily, and 5 monthly price file requests, per account/email combination. </p>
<p>Please allow 30 days from the last successful request before initiating another request, ensuring not to exceed 5 requests within a monthly period.</p>
<br/>
<p>This is a system generated email based on a specific request via the mySchneider platform or an SE account manager.</p>
<p>If you did not request this file please disregard and delete.</p>
<p>Please notify your SE Account Manager or Representative of any issues.</p>
<br/><br/>
<p><em>Best regards</em></p>
<p>Pricing Operations and mySchneider teams</p>
<p>Schneider Electric Pacific Zone</p>',
'Customer_No, Customer_Name','','','Partner.PriceOperations@schneider-electric.com',2,1,0,'SESA715213','',GETUTCDATE(),GETUTCDATE()),


('00','MissingCustomerSettings',
'Schneider Electric Price File (Request issue) - [{{Customer_No}}] - {{Customer_Name}}',
'<p>CONFIDENTIAL</p>
<p>Dear Customer</p>
<p>A requested price file via the mySchneider platform was unable to be created due to incomplete data.</p>
<p>There may be various reasons for this issue including system settings or account maintenance issues.</p>
<p>The SE Pricing Operations Team has been notified via this email and will review at their earliest convenience.</p>
<p>Please notify your SE Account Manager or Representative of this issue if no response is received within 7 days.</p>
<br/>
<p>This is a system generated email based on a specific request via the mySchneider platform or an SE account manager.</p>
<p>If you did not request this file please disregard and delete.</p>
<p>Please notify your SE Account Manager or Representative of any issues.</p>
<br/><br/>
<p><em>Best regards</em></p>
<p>Pricing Operations and mySchneider teams</p>
<p>Schneider Electric Pacific Zone</p>',
'Customer_No, Customer_Name','','','Partner.PriceOperations@schneider-electric.com',2,1,0,'SESA715213','',GETUTCDATE(),GETUTCDATE()),


('00','MissingCustomer',
'Schneider Electric Price File (Request issue) - [{{Customer_No}}] - {{Customer_Name}}',
'<p>CONFIDENTIAL</p>
<p>Dear Customer</p>
<p>A requested price file via the mySchneider platform was unable to be created due to incomplete data.</p>
<p>There may be various reasons for this issue including system settings or account maintenance issues.</p>
<p>The SE Pricing Operations Team has been notified via this email and will review at their earliest convenience.</p>
<p>Please notify your SE Account Manager or Representative of this issue if no response is received within 7 days.</p>
<br/>
<p>This is a system generated email based on a specific request via the mySchneider platform or an SE account manager.</p>
<p>If you did not request this file please disregard and delete.</p>
<p>Please notify your SE Account Manager or Representative of any issues.</p>
<br/><br/>
<p><em>Best regards</em></p>
<p>Pricing Operations and mySchneider teams</p>
<p>Schneider Electric Pacific Zone</p>',
'Customer_No, Customer_Name','','','Partner.PriceOperations@schneider-electric.com',2,1,0,'SESA715213','',GETUTCDATE(),GETUTCDATE())



update MST_NotificationTemplate
set DefaultBccTo = 'Partner.PriceOperations@schneider-electric.com'
where NotificationTemplateID > 12



Insert into MST_AppConfig
Select 'WhitelistIPAddresses', 'Whitelist IP Addresses', 'Whitelist IP Addresses are only Authorized to use API', '10.252.155.44','WhitelistIPAddresses','STRING','DropDownList',7,39,10,0,1,'SESA624164','SESA624164',GETUTCDATE(),GETUTCDATE()
Select 'ApiRequestLimitPerDay', 'API Request Limit per Day', 'Max API Requests that a user can request per Day', '15', 'RateLimitPerDay', 'NUMBER','DropDownList',1,2,8,1,0,'SESA715213','', GETUTCDATE(),GETUTCDATE()

Insert into MST_ConfigOptions
Select 'WhitelistIPAddresses','10.236.35.57',1,1,0,'SESA624164','SESA624164',GETUTCDATE(),GETUTCDATE()

Insert into MST_NotificationTemplate
Select 'ADMIN','UnAuthorized','UnAuthorized Price File Request','<p>Dear Admin,<br></br><p>This is to inform you that we have received an Unauthorized Request for Price File via API</p></p><br/><p><em>Best regards</em></p><p>Pricing Operations and mySchneider teams</p><p>Schneider Electric Pacific Zone</p>','','Murali.Kunapareddy@se.com;bhavana.adari@non.se.com;sreeranga.vinjamuri@non.se.com','','',2,1,0,'SESA624164','SESA624164',GETUTCDATE(),GETUTCDATE()





------Alter SP [dbo].[USPM_GetTemplateDatabyTemplateID] Script Alter Date: 24/07/11 10:50:16 AM 


/****** Object:  StoredProcedure [dbo].[USPM_GetTemplateDatabyTemplateID]    Script Date: 7/11/2024 10:50:16 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--exec USPM_GetTemplateDatabyTemplateID 1 ,2,'VRGDescriptions',5000    
--exec USPM_GetTemplateDatabyTemplateID 2 ,2,'MaterialStatus',5000     
--exec USPM_GetTemplateDatabyTemplateID 3 ,2,'MOQ',5000     
--exec USPM_GetTemplateDatabyTemplateID 4 ,2,'RRPReferences',5000    
--exec USPM_GetTemplateDatabyTemplateID 5 ,2,'MaterialMasterList',5000   
--exec USPM_GetTemplateDatabyTemplateID 6 ,2,'GSTConfigurations',5000   
--exec USPM_GetTemplateDatabyTemplateID 7 ,2,'DiscountParameters',5000   
--exec USPM_GetTemplateDatabyTemplateID 8 ,1,'Template1212',5000   
--exec USPM_GetTemplateDatabyTemplateID 12 ,3,'testcusttemplate1',5000   
  
  
ALTER   Procedure [dbo].[USPM_GetTemplateDatabyTemplateID](
@TemplateMasterID bigint,     
@TemplateCategoryID bigint,    
@TemplateName varchar(100),    
@DisplayMaxRecords int,    
@ResultFlag NVARCHAR(25) = NULL OUTPUT,          
@Result  NVARCHAR(250) = NULL OUTPUT          
)                                                      
As                                                      
BEGIN           
BEGIN TRY           
Print 'Start: USPM_GetTemplateDataByTemplateID'   
  
IF OBJECT_ID(N'tempdb..#Temp_TradeListTemplate') IS NOT NULL DROP TABLE #Temp_TradeListTemplate;  
IF OBJECT_ID(N'tempdb..#Temp_CustomerTemplate') IS NOT NULL DROP TABLE #Temp_CustomerTemplate;  
IF OBJECT_ID(N'tempdb..#Temp_DiscountParameters') IS NOT NULL DROP TABLE #Temp_DiscountParameters;  
IF OBJECT_ID(N'tempdb..#Temp_RRPReferences') IS NOT NULL DROP TABLE #Temp_RRPReferences;  
IF OBJECT_ID(N'tempdb..#Temp_MOQ') IS NOT NULL DROP TABLE #Temp_MOQ;  
IF OBJECT_ID(N'tempdb..#Temp_GSTConfigurations') IS NOT NULL DROP TABLE #Temp_GSTConfigurations;  
IF OBJECT_ID(N'tempdb..#Temp_VRGDescriptions') IS NOT NULL DROP TABLE #Temp_VRGDescriptions;  
IF OBJECT_ID(N'tempdb..#Temp_MaterialStatus') IS NOT NULL DROP TABLE #Temp_MaterialStatus;  
IF OBJECT_ID(N'tempdb..#Temp_MaterialMaster') IS NOT NULL DROP TABLE #Temp_MaterialMaster;  
  
  
  
begin  
    
if(@TemplateCategoryID = 1)    
begin    
   
SELECT    D.InternalSAPItemNo    
into #Temp_TradeListTemplate  
FROM MST_TemplateData TB WITH(NOLOCK)                                                      
    CROSS APPLY OPENJSON (TB.[Data]) WITH                                                      
    (                                                      
        InternalSAPItemNo NVarchar(100) '$.InternalSAPItemNo'                                                      
    ) as  D                                       
 Where TB.TemplateMasterID = @TemplateMasterID   and D.InternalSAPItemNo is not null     
   
 Select Count(1) as TotalRecordsCount from #Temp_TradeListTemplate  
 if(@DisplayMaxRecords > 0)  
 begin  
  Select Top(@DisplayMaxRecords) * from #Temp_TradeListTemplate  
  end  
  else  
  begin  
  Select * from #Temp_TradeListTemplate  
  End  
    
end    
else if(@TemplateCategoryID = 3)    
begin   
  
SELECT    D.CustomerNo   
into #Temp_CustomerTemplate   
FROM MST_TemplateData TB WITH(NOLOCK)                                                      
    CROSS APPLY OPENJSON (TB.[Data]) WITH                                                      
    (                                                      
        CustomerNo NVarchar(100) '$.CustomerNo'                                                      
    ) as  D                                       
 Where TB.TemplateMasterID = @TemplateMasterID       
  Select Count(1) as TotalRecordsCount from #Temp_CustomerTemplate  
 if(@DisplayMaxRecords > 0)  
 begin  
  Select Top(@DisplayMaxRecords) * from #Temp_CustomerTemplate  
  end  
  else  
  begin  
  Select * from #Temp_CustomerTemplate  
  End  
end    
else if(@TemplateCategoryID = 2)    
Begin    
if(@TemplateName ='DiscountParameters')    
begin   
   
SELECT     D.DiscountName, D.DiscountValue   
into #Temp_DiscountParameters   
from                                              
 MST_TemplateData TB  WITH(NOLOCK)                                                   
    CROSS APPLY OPENJSON (TB.[Data]) WITH                            
    (           
     DiscountName NVarchar(100) '$.DiscountName',        
     DiscountValue NVarchar(100) '$.DiscountValue'        
    ) as  D          
 where TB.TemplateMasterID = @TemplateMasterID    
   
  Select Count(1) as TotalRecordsCount from #Temp_DiscountParameters  
 if(@DisplayMaxRecords > 0)  
 begin  
  Select Top(@DisplayMaxRecords) * from #Temp_DiscountParameters  
  end  
  else  
  begin  
  Select * from #Temp_DiscountParameters  
  End  
  
End      
else if(@TemplateName ='RRPReferences')    
begin    
   
SELECT                                                       
     D.[LCOS1To4],D.[Collection],D.[SubCollection],D.[DiscountGroup],D.[RRPMarkup]     
  into #Temp_RRPReferences   
FROM MST_TemplateData TB  WITH(NOLOCK)                                                   
    CROSS APPLY OPENJSON (TB.[Data]) WITH                                                    
    (                                                    
       [LCOS1To4] NVarchar(100) '$.LCOS1To4' ,                                            
    [Collection] NVarchar(100) '$.Collection' ,                                            
    [SubCollection] NVarchar(100) '$.SubCollection' ,                                            
    [DiscountGroup] NVarchar(100) '$.DiscountGroup' ,                                            
    [RRPMarkup] float '$.RRPMarkup'                                              
    ) as  D                                              
  Where TB.TemplateMasterID = @TemplateMasterID      
  
  
  Select Count(1) as TotalRecordsCount from #Temp_RRPReferences  
 if(@DisplayMaxRecords > 0)  
 begin  
  Select Top(@DisplayMaxRecords) * from #Temp_RRPReferences  
  end  
  else  
  begin  
  Select * from #Temp_RRPReferences  
  End  
  
End    
else if(@TemplateName ='MOQ')    
begin    
   
SELECT                                                      
     D.[SchneiderElectricMaterialReference],D.[MOQa]        
  into #Temp_MOQ   
FROM MST_TemplateData TB WITH(NOLOCK)                                     
    CROSS APPLY OPENJSON (TB.[Data]) WITH                                                    
    (                                                    
        [SchneiderElectricMaterialReference] NVarchar(100) '$.SchneiderElectricMaterialReference' ,              
  [MOQa] NVarchar(100) '$.MOQa'                                             
    ) as  D                                              
  Where TB.TemplateMasterID = @TemplateMasterID    
    
  
   Select Count(1) as TotalRecordsCount from #Temp_MOQ  
 if(@DisplayMaxRecords > 0)  
 begin  
  Select Top(@DisplayMaxRecords) * from #Temp_MOQ  
  end  
  else  
  begin  
  Select * from #Temp_MOQ  
  End  
  
End    
    
else if(@TemplateName ='GSTConfigurations')    
begin    
   
SELECT                                                      
     D.[CountryCode],D.[GSTPercentage]    
  into #Temp_GSTConfigurations   
FROM MST_TemplateData TB WITH(NOLOCK)                                                    
    CROSS APPLY OPENJSON (TB.[Data]) WITH                                                    
    (                                                    
        [CountryCode] NVarchar(100) '$.CountryCode' ,                                            
        [GSTPercentage] Float '$.GSTPercentage'                                             
    ) as  D                                              
  Where TB.TemplateMasterID = @TemplateMasterID    
    
     Select Count(1) as TotalRecordsCount from #Temp_GSTConfigurations  
 if(@DisplayMaxRecords > 0)  
 begin  
  Select Top(@DisplayMaxRecords) * from #Temp_GSTConfigurations  
  end  
  else  
  begin  
  Select * from #Temp_GSTConfigurations  
  End  
  
End    
    
else if(@TemplateName ='VRGDescriptions')    
begin    
   
SELECT                                                      
     D.[VRG],D.[VRGDescription]     
  into #Temp_VRGDescriptions   
FROM MST_TemplateData TB WITH(NOLOCK)                                                    
    CROSS APPLY OPENJSON (TB.[Data]) WITH                                                    
    (                                                    
        [VRG] NVarchar(100) '$.VRG' ,                                            
        [VRGDescription] NVarchar(1000) '$.VRGDescription'                                             
    ) as  D                                              
  Where TB.TemplateMasterID = @TemplateMasterID    
    
    Select Count(1) as TotalRecordsCount from #Temp_VRGDescriptions  
 if(@DisplayMaxRecords > 0)  
 begin  
  Select Top(@DisplayMaxRecords) * from #Temp_VRGDescriptions  
  end  
  else  
  begin  
  Select * from #Temp_VRGDescriptions  
  End  
  
End    
    
else if(@TemplateName ='MaterialStatus')    
begin  
   
SELECT                                                      
       D.[St],D.[Description],D.[Status]      
    Into #Temp_MaterialStatus  
FROM MST_TemplateData TB  WITH(NOLOCK)                                                   
    CROSS APPLY OPENJSON (TB.[Data]) WITH                                                    
    (                                                    
        [St] NVarchar(100) '$.St' ,                                            
        [Description] NVarchar(100) '$.Description' ,                                            
        [Status] NVarchar(100) '$.Status'                                         
    ) as  D                                              
  Where TB.TemplateMasterID = @TemplateMasterID   
    
    
    Select Count(1) as TotalRecordsCount from #Temp_MaterialStatus  
 if(@DisplayMaxRecords > 0)  
 begin  
  Select Top(@DisplayMaxRecords) * from #Temp_MaterialStatus  
  end  
  else  
  begin  
  Select * from #Temp_MaterialStatus  
  End  
  
End    
else if(@TemplateName ='MaterialMasterList')    
begin  
  
    
    
    Select Count(1) as TotalRecordsCount from dbo.MST_MaterialMaster  
 if(@DisplayMaxRecords > 0)  
 begin  
  Select Top(@DisplayMaxRecords) Prefix,CatNo,ColourCode,ItemNo,InternalSAPItemNo,SplitPackQty from dbo.MST_MaterialMaster  
  end  
  else  
  begin  
  Select Prefix,CatNo,ColourCode,ItemNo,InternalSAPItemNo,SplitPackQty from dbo.MST_MaterialMaster  
  End  
  
End    
else if(@TemplateName ='CustomerContacts')    
begin  
  
    
    
    Select Count(1) as TotalRecordsCount from dbo.MST_CustomerContact  
 if(@DisplayMaxRecords > 0)  
 begin  
  Select Top(@DisplayMaxRecords) AccountNumber,AccountName,ContactPerson,ToEmailID,CcEmailID,BccEmailID from dbo.MST_CustomerContact  
  end  
  else  
  begin  
  Select AccountNumber,AccountName,ContactPerson,ToEmailID,CcEmailID,BccEmailID from dbo.MST_CustomerContact  
  End  
  
End  
else if(@TemplateName ='CustomerSettings')    
begin 
	Select Count(1) as TotalRecordsCount from dbo.MST_CustomerSettings  
		if(@DisplayMaxRecords > 0)  
			begin  
				Select Top(@DisplayMaxRecords) CustomerNumber,SalesOrganization,CanIncludeTradePrices,CanIncludeCustomerNetPrices,CanIncludeCustomerHierarchyNetPrices,CanIncludeOverallNetPrices,CanIncludePriceGroupNets,CanIncludeSellOffPrices
,CanIncludePromoPrice,CanUseShiftBreaks,CanUseMOQAsBrk1,CanShowTemplateMaterialOnly,CanShowNotFoundTemplateMaterials,CanIncludeDiscount1,CanIncludeDiscount2
,CanIncludeDiscount3,CanIncludeDiscount4,CanIncludeDiscount5,CanIncludeDiscount6,CanIncludeDiscount7,CanIncludeDiscount8
 from dbo.MST_CustomerSettings  
			end  
		else  
			begin  
				Select CustomerNumber,SalesOrganization,CanIncludeTradePrices,CanIncludeCustomerNetPrices,CanIncludeCustomerHierarchyNetPrices,CanIncludeOverallNetPrices,CanIncludePriceGroupNets,CanIncludeSellOffPrices
,CanIncludePromoPrice,CanUseShiftBreaks,CanUseMOQAsBrk1,CanShowTemplateMaterialOnly,CanShowNotFoundTemplateMaterials,CanIncludeDiscount1,CanIncludeDiscount2
,CanIncludeDiscount3,CanIncludeDiscount4,CanIncludeDiscount5,CanIncludeDiscount6,CanIncludeDiscount7,CanIncludeDiscount8
 from dbo.MST_CustomerSettings  
			End 
End 
else  
begin  
  
Select 1 as TotalRecordsCount  
  
Select 'Invalid Payload' InvalidPayload  
  
end  
  
    
End    
  
  
  
  
End  
  
  
      
          
 SET @ResultFlag = 'Success'        
 SET @Result = 'Data Returned back !!!'            
END TRY                                                      
 BEGIN CATCH                                                      
 print ERROR_MESSAGE()           
    
SET @ResultFlag = 'Failed'          
SET @Result = ERROR_MESSAGE()          
--Some Error Occured !!!          
 END CATCH            
           
 PRINT @ResultFlag          
 PRINT @Result      
          
END 

GO


