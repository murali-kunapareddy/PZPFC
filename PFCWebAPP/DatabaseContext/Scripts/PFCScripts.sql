
-- ==================== Functions ====================
-- UFN_GetBrkVal
CREATE or Alter FUNCTION [dbo].[UFN_GetBrkVal] (
    @BrkQty int, @KSTBM1 int, @KSTBM2 int, @KSTBM3 int, @KSTBM4 int, @KSTBM5 int, @KSTBM6 int, 
				 @KBETR1 float, @KBETR2 float,@KBETR3 float,@KBETR4 float, @KBETR5 float,@KBETR6 float
)
RETURNS Float
AS  

BEGIN


   declare @getBrkVal float =0 
   if(Convert(int, isnull(@BrkQty,0)) >= @KSTBM1) 
   begin
   set @getBrkVal  =@KBETR1
   end

    if(Convert(int, isnull(@BrkQty,0)) >= @KSTBM2) 
   begin
   set @getBrkVal  =@KBETR2
   end

    if(Convert(int, isnull(@BrkQty,0)) >= @KSTBM3) 
   begin
   set @getBrkVal  =@KBETR3
   end

    if(Convert(int, isnull(@BrkQty,0)) >= @KSTBM4) 
   begin
   set @getBrkVal  =@KBETR4
   end

    if(Convert(int, isnull(@BrkQty,0)) >= @KSTBM5) 
   begin
   set @getBrkVal  =@KBETR5
   end

    if(Convert(int, isnull(@BrkQty,0)) >= @KSTBM6) 
   begin
   set @getBrkVal  =@KBETR6
   end
                              

    --
    RETURN @getBrkVal 
END
GO
-- UFN_GetDiscPrice
--select [dbo].[UFN_GetDiscPrice](193.4,-48,-3.85,0,0,0,0,0,0,0.1,1,'A507') as A
CREATE or Alter FUNCTION [dbo].[UFN_GetDiscPrice] (
    @Price float, @Disc1 float, @Disc2 float, @Disc3 float, @Disc4 float, 
	@Disc5 float, @Disc6 float, @Disc7 float, @Disc8 float,@SODVal float,@CanAddSODInFinalPrice bit, @PriceTable Varchar(100) 
)
RETURNS Float
AS  

BEGIN
set @Price =isnull(@Price,0)
set @Disc1 =isnull(@Disc1,0)
set @Disc2 =isnull(@Disc2,0)
set @Disc3 =isnull(@Disc3,0)
set @Disc4 =isnull(@Disc4,0)
set @Disc5 =isnull(@Disc5,0)
set @Disc6 =isnull(@Disc6,0)
set @Disc7 =isnull(@Disc7,0)
set @Disc8 =isnull(@Disc8,0)
set @SODVal =isnull(@SODVal,0)
set @CanAddSODInFinalPrice =isnull(@CanAddSODInFinalPrice,0)
set @PriceTable =isnull(@PriceTable,'')

DECLARE @P1  float, @P2  float, @P3  float, @P4  float, @P5 float, @P6 float, @P7 float, @P8 float
DECLARE @Disc9 As float, @FPrcNoSOD As float, @FprcSOD As float, @Fprc As float

If(isnull(@Price,0) = 0) RETURN 0

If @PriceTable = 'A507' 
begin
   set @Disc9 = @SODVal
    set @P1 = Round(@Price * (@Disc1 / -100) + 0.0000001, 2)
    set @P2 = Round(Round(@Price - @P1, 2) * (@Disc2 / -100) + 0.000001, 2)
    set @P3 = Round(Round(@Price - (@P1 + @P2), 2) * (@Disc3 / -100) + 0.000001, 2)
    set @P4 = Round(Round(@Price - (@P1 + @P2 + @P3), 2) * (@Disc4 / -100) + 0.000001, 2)
    set @P5 = Round(Round(@Price - (@P1 + @P2 + @P3 + @P4), 2) * (@Disc5 / -100) + 0.000001, 2)
    set @P6 = Round(Round(@Price - (@P1 + @P2 + @P3 + @P4 + @P5), 2) * (@Disc6 / -100) + 0.000001, 2)
    set @P7 = Round(Round(@Price - (@P1 + @P2 + @P3 + @P4 + @P5 + @P6), 2) * (@Disc7 / -100) + 0.000001, 2)
    set @P8 = Round(Round(@Price - (@P1 + @P2 + @P3 + @P4 + @P5 + @P6 + @P7), 2) * (@Disc8 / -100) + 0.000001, 2)

    -- 
    DECLARE @DiscPrice float;
    SET @DiscPrice=0;
    --
	--'Define price with or without SOD
    set @FPrcNoSOD = Round(@Price - (@P1 + @P2 + @P3 + @P4 + @P5 + @P6 + @P7 + @P8), 2)
    set @FprcSOD = Round((@FPrcNoSOD - (@FPrcNoSOD * @Disc9) + 0.000001), 2)

	if(@CanAddSODInFinalPrice =1 )
	begin
	set @Fprc = @FprcSOD
	end
	else
	begin
	set @Fprc = @FPrcNoSOD
	end

	If (isnull(@Price,0) != 0 And (isnull(@Disc1,0) != 0 Or @Disc2 != 0 Or @Disc3 != 0 Or @Disc4 != 0 Or @Disc5 != 0 Or @Disc6 != 0 Or @Disc7 != 0 Or @Disc8 != 0)) 
	begin
	If @PriceTable = 'A507'  return @Fprc
	else  return @Price
	end

	else
	 return @Price

end
else
	 return @Price

    --
    RETURN 0 
END
GO

-- UFN_GetDiscTR
--select [dbo].[UFN_GetDiscTR](20,10,10,10,10,10,10,10,10,10,10,10,10,10) as A
CREATE or Alter FUNCTION [dbo].[UFN_GetDiscTR] (
@Price float, @Quantity float, @QB1 float, @QB2 float, @QB3 float, @QB4 float, @QB5 float, @QB6 float, @TP1 float, @TP2 float, @TP3 float, @TP4 float, @TP5 float, @TP6 float

)
RETURNS Float
AS  

BEGIN
Declare @getDiscTR float =0
Declare @TradePrice float =0

if(isnull(@Price,0) != 0)
begin
if(isnull(@TP1,0) != 0)
begin
if @QB1 is null  set @QB1 =0
if @QB2 is null  set @QB2 =0
if @QB3 is null  set @QB3 =0
if @QB4 is null  set @QB4 =0
if @QB5 is null  set @QB5 =0
if @QB6 is null  set @QB6 =0

if @TP1 is null  set @TP1 =0
if @TP2 is null  set @TP2 =0
if @TP3 is null  set @TP3 =0
if @TP4 is null  set @TP4 =0
if @TP5 is null  set @TP5 =0
if @TP6 is null  set @TP6 =0

If (@Quantity >= @QB1 And @QB1 != 0)  set @TradePrice = @TP1  
If (@Quantity >= @QB2 And @QB2 != 0)  set @TradePrice = @TP2
If (@Quantity >= @QB3 And @QB3 != 0)  set @TradePrice = @TP3 
If (@Quantity >= @QB4 And @QB4 != 0)  set @TradePrice = @TP4 
If (@Quantity >= @QB5 And @QB5 != 0)  set @TradePrice = @TP5 
--If (@Quantity >= @QB6 And @QB6 != 0)  set @TradePrice = @TP6 

set @getDiscTR =  cast(Round((@TradePrice - @Price) / @TradePrice, 4) as float) 

end
end

    --
    RETURN @getDiscTR
END
GO

-- UFN_GetDiscTR
--select [dbo].[UFN_GetIncGSTPrice](10,10) as A
CREATE or Alter FUNCTION [dbo].[UFN_GetIncGSTPrice] (
    @Price float, @SelectedSalesOrganizationGSTPercentage float 
)
RETURNS Float
AS 

BEGIN
Declare @IncGSTPrice  decimal(38,10)
set @IncGSTPrice = cast(cast(isnull(@Price,0) as float) * (cast(1 +  cast((cast( (CAST (@SelectedSalesOrganizationGSTPercentage as float) / CAST (100 as float)) as float)) as float) as float)) as float)
return cast(Round(@IncGSTPrice, 2) as float)
    --
    RETURN 0 
END
GO


--UFN_GetRetailPrice

--select [dbo].[UFN_GetRetailPrice](10,'') as A
CREATE or Alter FUNCTION [dbo].[UFN_GetRetailPrice] (  
    @RRP_Non_Rounded float, @LCOS Varchar(100)   
)  
RETURNS Float  
AS    
  
Begin  
Declare @RetailPrice float =0  
if @RRP_Non_Rounded is null  
begin   
set @RetailPrice =0   
end  
else if @RRP_Non_Rounded < 0.05    
begin  
--Less than.05c then = RRP Non Rounded  
set @RetailPrice = @RRP_Non_Rounded  
end  
else if @RRP_Non_Rounded < 1    
begin  
--Less than $1.00 then roundup to next 0.05c  
set @RetailPrice = floor(-20 * @RRP_Non_Rounded) / -20 --0.07 --0.05, 0.08 - 0.1  
end  
else if @RRP_Non_Rounded < 300   
begin  
--Less than (<) $300.00 then roundup to next $1.00  
--set @RetailPrice = CEILING( -1 *  (@RRP_Non_Rounded / -1 ) ) --191.1 -- 192
set @RetailPrice =  -1 *  floor(@RRP_Non_Rounded / -1 ) 
end  
  
  
if(@RRP_Non_Rounded > 300 And (@LCOS) = '03GEN1REWIS')  
begin  
--Greater then(>) $300.00 and is wiser item then roundup to next $5.00  
set @RetailPrice =  -5 * floor(@RRP_Non_Rounded / -5)  -- 399 means --295   
end  
    --  
    RETURN @RetailPrice   
END  
GO
  
                                            
                                            
--exec USPT_GeneratePriceFileCreation 28                                           
CREATE or Alter Procedure [dbo].[USPT_GeneratePriceFileCreation](  
@UserConfigSettingID bigint,    
@ResultFlag NVARCHAR(25) = NULL OUTPUT,    
@Result  NVARCHAR(250) = NULL OUTPUT    
)                                                
As                                                
BEGIN     
BEGIN TRY     
Print 'Start: USPT_GeneratePriceFileCreation'      


--delete from dbo.TRN_PriceFileLog 
--    where PriceFileHeaderID in ( select PriceFileHeaderID from dbo.TRN_PriceFileHeader    where UserConfigSettingID =2 )
--delete from dbo.TRN_PriceFileDetails  
--    where PriceFileHeaderID in ( select PriceFileHeaderID from dbo.TRN_PriceFileHeader    where UserConfigSettingID =2 )  
--delete from dbo.TRN_PriceFileLocationDetails  
--   where PriceFileHeaderID in ( select PriceFileHeaderID from dbo.TRN_PriceFileHeader    where UserConfigSettingID =2 ) 
--delete from dbo.TRN_PriceFileHeader    where UserConfigSettingID =2  




 
SET @ResultFlag = ''    
SET @Result = ''    
    
Begin---====================Validations  ================    
    
if(@UserConfigSettingID <= 0)    
Begin    
 SET @ResultFlag = 'Failed'    
SET @Result = 'Invalid UserConfigSettingID'    
end    
    
IF exists (select Top 1 1 from Dbo.TRN_PriceFileHeader where UserConfigSettingID=@UserConfigSettingID)    
 begin    
 SET @ResultFlag = 'Failed'    
 SET @Result = 'Invalid UserConfigSettingID'    
 end    
 IF not exists (select Top 1 1 from Dbo.TRN_UserConfigSetting where UserConfigSettingID=@UserConfigSettingID)    
 begin    
 SET @ResultFlag = 'Failed'    
 SET @Result = 'Invalid UserConfigSettingID'    
 end    
    
 IF ( @ResultFlag = 'Failed' )    
 BEGIN    
  PRINT @ResultFlag; PRINT @Result; RETURN    
 END    
End    
    
Begin    
    
Begin---====================Declaring Variables for User Settings  ================       
   Declare @PriceFileHeaderID bigint =0;    
   Declare @RequestedUserSESA Varchar(100) ='-';    
   Declare @SelectedCustomers nvarchar(max)    
   Declare @PricesActiveDate DateTime    
   Declare @SalesOrganization Nvarchar(10)                                                
   Declare @DefaultPriceList Varchar(10)      
   Declare @CanUseAutoReportContent bit    
   Declare @ReportContentTemplateID bigint    
   Declare @ReportFormatTemplateID bigint    
   Declare @CanIncludeTradePrices bit     
   Declare @SelectedProductHierarchy int--Selected product hierarchy (1=Global, 2=Local)                                                
   Declare @CanIncludeSellOffPrices bit                                                
   Declare @CanIncludeOverallNetPrices bit                                                
   Declare @CanIncludeCustomerNetPrices bit                                                
   Declare @CanIncludeCustomerHierarchyNetPrices bit                                                
   Declare @CanIncludePriceGroupNets bit                                                
   Declare @CanIncludePromoPrice bit                                                                                           
   Declare @CanIncludeDiscount1 bit                                               
   Declare @CanIncludeDiscount2 bit                                               
   Declare @CanIncludeDiscount3 bit                                                
   Declare @CanIncludeDiscount4 bit                                               
   Declare @CanIncludeDiscount5 bit                                                
   Declare @CanIncludeDiscount6 bit                                                
   Declare @CanIncludeDiscount7 bit                                                
   Declare @CanIncludeDiscount8 bit                                               
   Declare @CanUseShiftBreaks bit                                      
   Declare @SelectedSalesOrganizationGSTPercentage float                                    
   Declare @CanUseMOQAsBrk1 bit                                  
   Declare @CanAddSODInFinalPrice bit                                  
   Declare @SODVal Float  --Disc9        
   Declare @CanShowTemplateMaterialOnly bit      
End    
    
begin---====================Get Required User Settings By UserConfigSettingID=================================     
                                    
             
select    
  @SelectedCustomers = SelectedCustomers,    
  @RequestedUserSESA  = UserSESA,    
  @SalesOrganization = SalesOrganization,     
  --@PricesActiveDate =case when Convert(date,[PricesActiveDate]) >= getdate() then [PricesActiveDate] else getdate() end,  
  @PricesActiveDate = isnull(PricesActiveDate,getdate()), 
  @CanUseAutoReportContent = CanUseAutoReportContent,    
  @ReportContentTemplateID =ReportContentTemplateID,    
  @ReportFormatTemplateID =ReportFormatTemplateID,    
  @CanIncludeTradePrices = CanIncludeTradePrices,    
  @SelectedProductHierarchy = case when  [CanUseGlobalCOSForProductHierarchy] =1 then 1                         
                                   when  [CanUseLocalCOSForProductHierarchy] =1 then 2  else 2 end,-- adding default value as LocalCOS if both are zero                                               
  @CanIncludeSellOffPrices = CanIncludeSellOffPrices,                                                
  @CanIncludeOverallNetPrices = CanIncludeOverallNetPrices,                 
  @CanIncludeCustomerNetPrices =CanIncludeCustomerNetPrices,                                                
  @CanIncludeCustomerHierarchyNetPrices =CanIncludeCustomerHierarchyNetPrices,                                                
  @CanIncludePriceGroupNets = CanIncludePriceGroupNets,                                                
  @CanIncludePromoPrice = CanIncludePromoPrice,                                                
  @CanIncludeDiscount1 = CanIncludeDiscount1,                              
  @CanIncludeDiscount2 = CanIncludeDiscount2,                                          
  @CanIncludeDiscount3 = CanIncludeDiscount3,                                               
  @CanIncludeDiscount4 = CanIncludeDiscount4,                                               
  @CanIncludeDiscount5 = CanIncludeDiscount5,                                               
  @CanIncludeDiscount6 = CanIncludeDiscount6,                                                
  @CanIncludeDiscount7 = CanIncludeDiscount7,                                                
  @CanIncludeDiscount8 = CanIncludeDiscount8,                                               
  @CanUseShiftBreaks = CanUseShiftBreaks  ,                                  
  @CanUseMOQAsBrk1 = CanUseMOQAsBrk1  ,                                
  @CanAddSODInFinalPrice =CanAddSODInFinalPrice,                                
  @SODVal =SODInFinalPriceValue ,      
  @CanShowTemplateMaterialOnly =CanShowTemplateMaterialOnly      
from dbo.TRN_UserConfigSetting WITH(NOLOCK)  where UserConfigSettingID =@UserConfigSettingID --@RequestedUserSESA                                                
                                                
                                                
                                                
--Default price list if blank                               
if(@SalesOrganization = 'AU01')                     
begin                                                
set @DefaultPriceList ='Z1'                                                
end                                                
else if(@SalesOrganization = 'NZ01')                                                
begin                                                
set @DefaultPriceList ='Z1'                                                
end     
    
      
    
End    
    
Insert into dbo.TRN_PriceFileHeader([UserConfigSettingID],[Status],[StatusText],[PercentCompleted],[CreatedBy]) Values(@UserConfigSettingID,'In-Progress','Start GeneratePriceFileCreation',5,@RequestedUserSESA)    
set @PriceFileHeaderID = SCOPE_IDENTITY()    
--select @PriceFileHeaderID    
update dbo.TRN_PriceFileHeader set [StatusText] ='Clearing Temp Tables...' ,     
           [PercentCompleted] =5     
           where PriceFileHeaderID =@PriceFileHeaderID    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)  
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','User Settings','Get Required User Settings')              
     
Begin---===================Clean Temp Tables====================================       
   Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
   Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Drop Temp Tables','Clearing All Temp Tables')                                                
                     
   IF OBJECT_ID(N'tempdb..#Temp_SelectedCustomers') IS NOT NULL DROP TABLE #Temp_SelectedCustomers;                                                                                                                                        
   IF OBJECT_ID(N'tempdb..#Temp_CustomerList') IS NOT NULL DROP TABLE #Temp_CustomerList;                                                
   IF OBJECT_ID(N'tempdb..#Temp_MaterialList') IS NOT NULL DROP TABLE #Temp_MaterialList;                                                                                                                                        
   IF OBJECT_ID(N'tempdb..#Temp_Prices') IS NOT NULL DROP TABLE #Temp_Prices;                                                
   IF OBJECT_ID(N'tempdb..#Temp_DiscountTypeList') IS NOT NULL DROP TABLE #Temp_DiscountTypeList;                                                
   IF OBJECT_ID(N'tempdb..#Temp_Discounts') IS NOT NULL DROP TABLE #Temp_Discounts;                                                
   IF OBJECT_ID(N'tempdb..#Temp_Discounts_PH') IS NOT NULL DROP TABLE #Temp_Discounts_PH;                                                
   IF OBJECT_ID(N'tempdb..#Temp_Discounts_brk') IS NOT NULL DROP TABLE #Temp_Discounts_brk;                                                
   IF OBJECT_ID(N'tempdb..#Temp_Prices_brk') IS NOT NULL DROP TABLE #Temp_Prices_brk;                                                
   IF OBJECT_ID(N'tempdb..#Temp_Qty_Brks_tmp') IS NOT NULL DROP TABLE #Temp_Qty_Brks_tmp;                                                
   IF OBJECT_ID(N'tempdb..#Temp_Qty_Brks') IS NOT NULL DROP TABLE #Temp_Qty_Brks;                                                                                
   IF OBJECT_ID(N'tempdb..#Temp_get_A507_Trade') IS NOT NULL DROP TABLE #Temp_get_A507_Trade;     
       
   IF OBJECT_ID(N'tempdb..#Temp_RRP') IS NOT NULL DROP TABLE #Temp_RRP;                                        
   IF OBJECT_ID(N'tempdb..#Temp_MOQ') IS NOT NULL DROP TABLE #Temp_MOQ;     
   IF OBJECT_ID(N'tempdb..#Temp_GST') IS NOT NULL DROP TABLE #Temp_GST;     
   IF OBJECT_ID(N'tempdb..#Temp_VRGDescriptions') IS NOT NULL DROP TABLE #Temp_VRGDescriptions;  
   IF OBJECT_ID(N'tempdb..#Temp_MaterialStatus') IS NOT NULL DROP TABLE #Temp_MaterialStatus;  
   --  
       
   IF OBJECT_ID(N'tempdb..#Temp_get_Stock_Status') IS NOT NULL DROP TABLE #Temp_get_Stock_Status;                        
   IF OBJECT_ID(N'tempdb..#Temp_get_Price_brks') IS NOT NULL DROP TABLE #Temp_get_Price_brks;                                           
   IF OBJECT_ID(N'tempdb..#Temp_get_Discount1_brks') IS NOT NULL DROP TABLE #Temp_get_Discount1_brks;                                         
   IF OBJECT_ID(N'tempdb..#Temp_get_Discount2_brks') IS NOT NULL DROP TABLE #Temp_get_Discount2_brks;                                        
   IF OBJECT_ID(N'tempdb..#Temp_get_Discount3_brks') IS NOT NULL DROP TABLE #Temp_get_Discount3_brks;                                        
   IF OBJECT_ID(N'tempdb..#Temp_get_Discount4_brks') IS NOT NULL DROP TABLE #Temp_get_Discount4_brks;                                        
   IF OBJECT_ID(N'tempdb..#Temp_get_Discount5_brks') IS NOT NULL DROP TABLE #Temp_get_Discount5_brks;                                        
   IF OBJECT_ID(N'tempdb..#Temp_get_Discount6_brks') IS NOT NULL DROP TABLE #Temp_get_Discount6_brks;                                        
   IF OBJECT_ID(N'tempdb..#Temp_get_Discount7_brks') IS NOT NULL DROP TABLE #Temp_get_Discount7_brks;                                        
   IF OBJECT_ID(N'tempdb..#Temp_get_Discount8_brks') IS NOT NULL DROP TABLE #Temp_get_Discount8_brks; 
   IF OBJECT_ID(N'tempdb..#Temp_Qty_Brks_tmp_RowNumber') IS NOT NULL DROP TABLE #Temp_Qty_Brks_tmp_RowNumber; 

   IF OBJECT_ID(N'tempdb..#Temp_MissingMaterials') IS NOT NULL DROP TABLE #Temp_MissingMaterials; 
                            
   IF OBJECT_ID(N'tempdb..#Temp_Cust_Prices') IS NOT NULL DROP TABLE #Temp_Cust_Prices;                           
                                     
                                                
End      
    
Begin---====================Declaring Variables while loop ====================================     
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Variable Declaration for loops','Declaring required variables for While Loop')    
  Declare @TotalCnt int = 10    
   Declare @Endno int = 1  
  Declare @Startno int = 1                                                
  Declare @SQL nvarchar(max)                                                
  Declare @ParmDefinition varchar(max)                            
End     
    
    
    
Begin---====================Creating Required Temp Tables ====================================      
                                                
--Select * From  Tempdb.Sys.Columns Where Object_ID = Object_ID('tempdb..#Temp_SelectedCustomersSettings')                                                
                                                
                                            
  Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp Tables Creation','Creating Required Temp Tables')           
            
  --===============================#Temp_SelectedCustomers================================================================                                                
    Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp Tables Creation','#Temp_SelectedCustomers')                                                
                                                
  CREATE TABLE #Temp_SelectedCustomers(                                                
    CustomerSNO        INT identity(1,1),                                                
    CustomerNumber        NVARCHAR(100)  NOT NULL DEFAULT(''),                                                
    CustomerName          NVARCHAR(100)  NOT NULL DEFAULT(''),                                                
    zKUNNR                NVARCHAR(100)    NOT NULL DEFAULT(''),                                                
    PC1                   NVARCHAR(100)    NOT NULL DEFAULT(''),                                                
    PC2                   NVARCHAR(100)    NOT NULL DEFAULT(''),                                                
    PC3                   NVARCHAR(100)    NOT NULL DEFAULT('')                                          
 --CONSTRAINT PK_Temp_SelectedCustomers_CustomerNumber PRIMARY KEY(CustomerNumber)                                                
); 
 CREATE UNIQUE CLUSTERED INDEX UK_SelectedCustomers ON #Temp_SelectedCustomers(CustomerNumber) 
                                                       
                                              
                                                
 --===============================#Temp_CustomerList================================================================ 
 
    Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp Tables Creation','#Temp_CustomerList') 
                                                
   Create TABLE  #Temp_CustomerList  (                                                
   KUNNR [nvarchar](100) NOT NULL ,                                                
   VKORG [nvarchar](100) NOT NULL  ,                                                
   VTWEG [nvarchar](100) NOT NULL  ,                                                
   SPART [nvarchar](100) NOT NULL  ,                                                
   BZIRK [nvarchar](100) ,                                                 
   PLTYP [nvarchar](100) ,                                                 
   KVGR1 [nvarchar](100) ,                                                 
   KVGR2 [nvarchar](100) ,                                                 
   KVGR3 [nvarchar](100),                                              
   KONDA [nvarchar](100) ,                                                 
   Level1 [nvarchar](100) ,                                        
   Level2 [nvarchar](100) ,                                                
   Level3 [nvarchar](100) ,                                                
   Level4 [nvarchar](100) ,                                                
   Level5 [nvarchar](100) ,                                                 
   Level6 [nvarchar](100),                                                
   Level7 [nvarchar](100) ,                                                 
   Level8 [nvarchar](100),                                         
   Level9 [nvarchar](100) ,                                             
   Level10 [nvarchar](100) ,                                                 
   SelDate [datetime] NULL,                                                 
   SelTrade [bit],                                                 
   SelHier [bit] ,                                                
   --CONSTRAINT PK_Temp_CustomerList_KUNNR_VKORG_VTWEG_SPART PRIMARY KEY(KUNNR, VKORG, VTWEG, SPART)                                                
   )                                                
   CREATE UNIQUE CLUSTERED INDEX UK_Temp_CustomerList ON #Temp_CustomerList(KUNNR, VKORG, VTWEG, SPART)                                                
   Create NONCLUSTERED Index IX_Temp_CustomerList_KUNNR On #Temp_CustomerList (KUNNR)      
   Create NONCLUSTERED Index IX_Temp_CustomerList_VKORG_VTWEG_SPART On #Temp_CustomerList (VKORG, VTWEG, SPART)     
                                                
                                                
                                              
--===============================#Temp_MaterialList================================================================                                      

  Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp Tables Creation','#Temp_MaterialList') 
  
                                                
  Create TABLE  #Temp_MaterialList (                                                
 MATNR [nvarchar](100),                                                
 VKORG [nvarchar](100),                                                
 VTWEG [nvarchar](100),                                                
 Prefix [nvarchar](100),                                               
 Cat_No [nvarchar](100),                    
 Colour_Code [nvarchar](100),                                                
 Item_No [nvarchar](100),                                                
 Split_Pack_Qty [nvarchar](100),                                                 
 MaterialSource [nvarchar](100),                                                
 PRDHA1 [nvarchar](100),                                                
 PRDHA2 [nvarchar](100),        
 PRDHA3 [nvarchar](100),                                                
 PRDHA4 [nvarchar](100),                                                
 PRDHA5 [nvarchar](100),                                                
 PRDHA6 [nvarchar](100),                                                
 SelProdHier int,   
 MainGroupPRODH [nvarchar](100),   
 MainGroupPRODHDescription [nvarchar](100),   
 GroupPRODH [nvarchar](100),   
 GroupPRODHDescription [nvarchar](100),   
 SubGroupPRODH [nvarchar](100),   
 SubGroupPRODHDescription [nvarchar](100),   
-- CONSTRAINT PK_Temp_MaterialList_MATNR_VKORG_VTWEG PRIMARY KEY(MATNR, VKORG, VTWEG)                                                
  )                                                 
     CREATE UNIQUE CLUSTERED INDEX UK_Temp_MaterialList ON #Temp_MaterialList(MATNR, VKORG, VTWEG)                                                
                                                
--===============================#Temp_Prices================================================================   


  Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp Tables Creation','#Temp_Prices') 

                                                
  Create TABLE  #Temp_Prices (                                                
    CustNo [nvarchar](100),                                  
 CustPriceList [nvarchar](100),                                                
 CustPriceGroup [nvarchar](100),                                                
 [Table] [nvarchar](100),                                                
 KAPPL [nvarchar](100),                                                
 KSCHL [nvarchar](100),                                                
 VKORG [nvarchar](100),                           
 VTWEG [nvarchar](100),                                         
 SPART [nvarchar](100),                                                
 KUNNR [nvarchar](100),                                                
 HIENR [nvarchar](100),                                                
KVGR1 [nvarchar](100),                                                
 KVGR2 [nvarchar](100),                                                
 KVGR3 [nvarchar](100),                                                
 PLTYP [nvarchar](100),                                                
 KONDA [nvarchar](100),                                                
 MATNR [nvarchar](100),                                                
 KFRST [nvarchar](100),                                                
 DATBI DateTime,                                                
 DATAB DateTime,                                                
 KBSTAT [nvarchar](100),                                           
 KNUMH [nvarchar](100),                                                
 KSTBM int,                                                
 KBETR float,                                                
 --CONSTRAINT PK_Temp_Prices_CustNo_MATNR PRIMARY KEY(CustNo, MATNR)                                                
 --CONSTRAINT PK_Temp_Prices_CustNo_MATNR PRIMARY KEY(CustNo, MATNR,[Table])-- For Time Being adding [Table] in PK to avoid Issue                                                
  )                                                 
  CREATE UNIQUE CLUSTERED INDEX UK_Temp_Prices ON #Temp_Prices(CustNo, MATNR)                                          
                                                
--===============================#Temp_DiscountTypeList================================================================   


  Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp Tables Creation','#Temp_DiscountTypeList') 

   Create TABLE  #Temp_DiscountTypeList (                                                
    [DiscountName] [nvarchar](100) NOT NULL,                                                
 [DiscountValue] [nvarchar](100) NOT NULL,                                                
 [Status] bit                                                
 --CONSTRAINT PK_Temp_DiscountTypeList_DiscountType PRIMARY KEY(DiscountName)                                                
  ) 
  CREATE UNIQUE CLUSTERED INDEX UK_Temp_DiscountTypeList ON #Temp_DiscountTypeList(DiscountName)
  --===============================#Temp_Discounts================================================================  
  
  
  Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp Tables Creation','#Temp_Discounts') 

   Create TABLE  #Temp_Discounts (                                                
    CustNo [nvarchar](100),                                                
 [Table] [nvarchar](100),                             
 KAPPL [nvarchar](100),                                                
 KSCHL [nvarchar](100),                                                
 VKORG [nvarchar](100),                                                
 VTWEG [nvarchar](100),                                                
 SPART [nvarchar](100),                                                
 HIENR [nvarchar](100),                                                 
 KVGR1 [nvarchar](100),                                                
 KVGR2 [nvarchar](100),                                                
 KVGR3 [nvarchar](100),                                                
 PRODH1 [nvarchar](100),                           
 PRODH2 [nvarchar](100),                                                
 PRODH3 [nvarchar](100),    
 PRODH4 [nvarchar](100),                      
 PRODH5 [nvarchar](100),                                                
 PRODH6 [nvarchar](100),                                                
 MATNR [nvarchar](100),                                                
 KFRST [nvarchar](100),                                                
 DATBI DateTime,                                                
 DATAB DateTime,                                                
 KBSTAT [nvarchar](100),                                                
 KNUMH [nvarchar](100),                                        
 KSTBM int,                                                
 KBETR float,                                                
 --CONSTRAINT PK_Temp_Discounts_CustNo_KSCHL_MATNR PRIMARY KEY(CustNo,KSCHL, MATNR)              
 --Violation of PRIMARY KEY constraint 'PK_Temp_Discounts_CustNo_KSCHL_MATNR'. Cannot insert duplicate key in object 'dbo.#Temp_Discounts'. The duplicate key value is (0000044327, ZD01, 3105SS/PK4).          
  )  
  CREATE UNIQUE CLUSTERED INDEX UK_Temp_Discounts ON #Temp_Discounts(CustNo,KSCHL, MATNR)
  

                                             
  --===============================#Temp_Discounts_PH================================================================ 
  
    Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp Tables Creation','#Temp_Discounts_PH') 


  Create TABLE  #Temp_Discounts_PH (                                                
    CustNo [nvarchar](100),                                                
 [Table] [nvarchar](100),                                                
 KAPPL [nvarchar](100),                                                
 KSCHL [nvarchar](100),                                                
 VKORG [nvarchar](100),                                                
 VTWEG [nvarchar](100),                                                
 SPART [nvarchar](100),                                                
 HIENR [nvarchar](100),                             
 KVGR1 [nvarchar](100),                                                
 KVGR2 [nvarchar](100),                                                
 KVGR3 [nvarchar](100),                                                
 PRODH1 [nvarchar](100),                                                
 PRODH2 [nvarchar](100),                                                
 PRODH3 [nvarchar](100),                                                
 PRODH4 [nvarchar](100),                                                
 PRODH5 [nvarchar](100),                                                
 PRODH6 [nvarchar](100),                                                
 KFRST [nvarchar](100),                                                
 DATBI DateTime,                                                
 DATAB DateTime,                                                
 KBSTAT [nvarchar](100),                                                
 KNUMH [nvarchar](100),                                                
 KSTBM int,                                                
 KBETR float,                        
  --CONSTRAINT UK_Temp_Discounts_PH_CustNo_KSCHL_PRODH123456 Unique (CustNo,KSCHL,PRODH1, PRODH2,PRODH3,PRODH4,PRODH5,PRODH6)                         
 --CONSTRAINT PK_Temp_Discounts_PH_CustNo_PRODH123456 PRIMARY KEY(CustNo,PRODH1, PRODH2,PRODH3,PRODH4,PRODH5,PRODH6)                                              
 --commented due to  The maximum key length for a clustered index is 900 bytes. The index 'PK_Temp_Discounts_PH_CustNo_PRODH123456'                                               
 --has maximum length of 1400 bytes. For some combination of large values, the insert/update operation will fail                                                
                                                 
  )    
  
  CREATE UNIQUE CLUSTERED INDEX UK_Temp_Discounts_PH_CustNo_KSCHL_PRODH123456 ON #Temp_Discounts_PH(CustNo,KSCHL,PRODH1, PRODH2,PRODH3,PRODH4,PRODH5,PRODH6) 
                                                
    --===============================#Temp_Discounts_brk================================================================ 
	    Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp Tables Creation','#Temp_Discounts_brk') 

  Create TABLE #Temp_Discounts_brk  (                                             
   CustNo [nvarchar](100) NOT NULL ,                                                
   MATNR [nvarchar](100)  ,                                   
   DiscTable [nvarchar](100)  ,                                                
   KSCHL [nvarchar](100)  ,                                                
  Qty int ,                                                 
   Disc float ,                                                 
   KSTBM1 int ,                                                 
   KSTBM2 int ,                                                 
   KSTBM3 int,                                                 
   KSTBM4 int ,                                                 
   KSTBM5 int ,                                                 
   KSTBM6 int ,                                                
   KBETR1 float ,                                                
   KBETR2 float ,                                       
   KBETR3 float,                                          
   KBETR4 float,                                                
   KBETR5 float ,                                
   KBETR6 float,                                                 
   CondType  [nvarchar](100)                   
                                                
   )                                                
                                                
      --===============================#Temp_Prices_brk================================================================ 
	  
	  	    Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp Tables Creation','#Temp_Prices_brk') 

    Create TABLE  #Temp_Prices_brk  (                                                
   CustNo [nvarchar](100) NOT NULL ,                                                
   MATNR [nvarchar](100)  ,                                                
   VKORG [nvarchar](100)  ,                                                
   VTWEG [nvarchar](100)  ,                                                 
   KSTBM1 int ,                                                 
   KSTBM2 int ,                                                 
   KSTBM3 int,                                                 
   KSTBM4 int ,                                                 
   KSTBM5 int ,                                                 
   KSTBM6 int ,                                                
   KBETR1 float ,                                                
   KBETR2 float ,                                                
   KBETR3 float,                                                 
   KBETR4 float,                                                
   KBETR5 float ,                                                 
   KBETR6 float,                                                 
   PriceTable [nvarchar](100)  ,                                                 
   CondType   [nvarchar](100)  ,                                                 
   DATBI DateTime,                                                
   DATAB DateTime,                                                
                                                
   )                                                
   --===============================#Temp_Qty_Brks_tmp================================================================ 
   
     Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp Tables Creation','#Temp_Qty_Brks_tmp') 
                                                
    Create TABLE  #Temp_Qty_Brks_tmp  (                                                
   CustNo [nvarchar](100) NOT NULL ,                                                
   MATNR [nvarchar](100)  ,                                                 
   QtyBrk int ,      
   --QtyBrkType [nvarchar](100),     
   KSCHL [nvarchar](100)                                                  
  -- CONSTRAINT PK_Temp_Qty_Brks_tmp_CustNo_MATNR_QtyBrk PRIMARY KEY(CustNo,MATNR, QtyBrk)                                                
    --Violation of PRIMARY KEY constraint 'PK_Temp_Qty_Brks_tmp_CustNo_MATNR_QtyBrk'.                                               
 --Cannot insert duplicate key in object 'dbo.#Temp_Qty_Brks_tmp'. The duplicate key value is (0000044327, WSCF227/2X-RG, 1).                                                
   ) 
    CREATE UNIQUE CLUSTERED INDEX UK_Temp_Qty_Brks_tmp_CustNo_MATNR_QtyBrk ON #Temp_Qty_Brks_tmp(CustNo,MATNR, QtyBrk)
                                                
     --===============================#Temp_Qty_Brks================================================================   
	 
	      Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp Tables Creation','#Temp_Qty_Brks') 

  CREATE TABLE #Temp_Qty_Brks(                                                
  CustNo varchar(100),                                                
  MATNR varchar(100),                                                
  QtyBrk1 varchar(100),                                                
  QtyBrk2 varchar(100),                                                
  QtyBrk3 varchar(100),                                                
  QtyBrk4 varchar(100),                                                
  QtyBrk5 varchar(100),                                                
  QtyBrk6 varchar(100)                                     
--CONSTRAINT PK_Temp_Qty_Brks_CustNo_MATNR PRIMARY KEY(CustNo,MATNR)                                                
  --Violation of PRIMARY KEY constraint 'PK_Temp_Qty_Brks_CustNo_MATNR'.                         
  --Cannot insert duplicate key in object 'dbo.#Temp_Qty_Brks'. The duplicate key value is (0000044327, WSCF227/2X-RG).            
  --Violation of PRIMARY KEY constraint 'PK_Temp_Qty_Brks_CustNo_MATNR'. Cannot insert duplicate key in object 'dbo.#Temp_Qty_Brks'. The duplicate key value is (0000044327, CLITPWP2).          
                                                 
  ); 
   CREATE UNIQUE CLUSTERED INDEX UK_Temp_Qty_Brks_CustNo_MATNR ON #Temp_Qty_Brks(CustNo,MATNR)
                                                
  --==================================#Temp_get_Stock_Status=====================================================================================      
       Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp Tables Creation','#Temp_get_Stock_Status') 
  
  CREATE TABLE #Temp_get_Stock_Status(                                                
  MATNR varchar(100),                                                
  [Status] varchar(100)                
  );
--==================================================================================================================================================
    
                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp Tables Creation','Temp Tables Creation Completed')                                                
                                                
End     
    
update dbo.TRN_PriceFileHeader set [StatusText] ='Retriving Selected Customers from Payload' ,     
           [PercentCompleted] =10     
           where PriceFileHeaderID =@PriceFileHeaderID    
    
begin---====================Get Customer List from @SelectedCustomers Json String==============     
                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Customer List','Get Customer List from @SelectedCustomers')                                              
    
    
--if(isnull(@SelectedCustomers,'') = '')    
--begin    
--return    
--end    
    
insert into #Temp_SelectedCustomers(CustomerNumber,CustomerName,zKUNNR,PC1,PC2,PC3)                                                
SELECT Distinct CustomerNumber,CustomerName,zKUNNR,PC1,PC2,PC3                                                
  FROM OPENJSON(@SelectedCustomers)                                                
  WITH (                                                 
           --CustomerSNO           INT    '$.CustomerSNO',                                                
           CustomerNumber        NVARCHAR(100)    '$.CustomerNumber',                                     
           CustomerName          NVARCHAR(100)    '$.CustomerName',                                                
           zKUNNR                NVARCHAR(100)    '$.zKUNNR',  --'$.zKUNNR' later need to replace CustomerNumber with zKUNNR                                           
           PC1                   NVARCHAR(100)    '$.PC1',                                                
           PC2                   NVARCHAR(100) '$.PC2',                                                
           PC3                   NVARCHAR(100)    '$.PC3'            
    ) ;            
 --select * from #Temp_SelectedCustomers          
                                                
end      
    
update dbo.TRN_PriceFileHeader set [StatusText] ='Loading Customers Additinal Information' ,     
           [PercentCompleted] =15     
           where PriceFileHeaderID =@PriceFileHeaderID    
                                                                                         
Begin---====================load_Customers====================================================     
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','load_Customers','Load selected Customer(s) with CustomerHierarchy')                                       
           
     
      
    
INSERT INTO #Temp_CustomerList ( KUNNR, VKORG, VTWEG, SPART, BZIRK, PLTYP, KVGR1, KVGR2, KVGR3, KONDA,                                                
                                 Level1, Level2, Level3, Level4, Level5, Level6, Level7, Level8, Level9, Level10,                                   
         SelDate, SelTrade, SelHier )                                                
 SELECT SC.zKUNNR as KUNNR, KNVV_clean.VKORG, KNVV_clean.VTWEG, KNVV_clean.SPART,                                                 
           KNVV_clean.BZIRK, 
		   --isnull([KNVV_clean].[PLTYP],@DefaultPriceList) AS PLTYPx, 
    case when isnull([KNVV_clean].[PLTYP],'') = '' then @DefaultPriceList else [KNVV_clean].[PLTYP] end as PLTYPx, 
     KNVV_clean.KVGR1, KNVV_clean.KVGR2, KNVV_clean.KVGR3, KNVV_clean.KONDA ,                                                
     IIf(@CanIncludeCustomerHierarchyNetPrices = 1,isnull(Level1, SC.[zKUNNR]),SC.[zKUNNR] ) AS Level1x,                                                
     IIf(@CanIncludeCustomerHierarchyNetPrices = 1,[Level2],Null) AS Level2x,                
     IIf(@CanIncludeCustomerHierarchyNetPrices = 1,[Level3],Null) AS Level3x,                                                
     IIf(@CanIncludeCustomerHierarchyNetPrices = 1,[Level4],Null) AS Level4x,                                                
     IIf(@CanIncludeCustomerHierarchyNetPrices = 1,[Level5],Null) AS Level5x,                                                
     IIf(@CanIncludeCustomerHierarchyNetPrices = 1,[Level6],Null) AS Level6x,                                                
     IIf(@CanIncludeCustomerHierarchyNetPrices = 1,[Level7],Null) AS Level7x,                                                
     IIf(@CanIncludeCustomerHierarchyNetPrices = 1,[Level8],Null) AS Level8x,                                                
     IIf(@CanIncludeCustomerHierarchyNetPrices = 1,[Level9],Null) AS Level9x,                                                
     IIf(@CanIncludeCustomerHierarchyNetPrices = 1,[Level10],Null) AS Level10x,                                                
         @PricesActiveDate as SelDate, @CanIncludeTradePrices as SelTrade, @CanIncludeCustomerHierarchyNetPrices  as SelHier      
   FROM #Temp_SelectedCustomers SC  WITH(NOLOCK)     
LEFT JOIN KNVV KNVV_clean WITH(NOLOCK)  on (SC.zKUNNR = KNVV_clean.KUNNR)  AND     
           (KNVV_clean.VTWEG='OG' AND     
        KNVV_clean.VKORG = @SalesOrganization)    
LEFT JOIN Customer_Hierarchy_cust CHC WITH(NOLOCK)  ON (KNVV_clean.SPART = CHC.SPART) AND     
            (KNVV_clean.VTWEG = CHC.VTWEG) AND     
            (KNVV_clean.VKORG = CHC.VKORG) AND     
            (KNVV_clean.KUNNR = CHC.KUNNR)AND     
            (CHC.VTWEG='OG' and CHC.VKORG =@SalesOrganization)     
    
                                                
    
     
                                                
End     
    
update dbo.TRN_PriceFileHeader set [StatusText] ='Loading Materials' ,     
           [PercentCompleted] =20     
           where PriceFileHeaderID =@PriceFileHeaderID    
     
Begin---====================load_MaterialList=================================================     
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','load_MaterialList','Load Material List from selected trade list template/MVKE')                           
                       
    
Begin----==========================================Load MVKEAuto or Temp_MaterialList=============    
if(@CanUseAutoReportContent =1)---==========================================@CanUseAutoReportContent = 1===========================                                                
Begin---==========================================Load #Temp_MaterialList with MVKEAuto============================                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load #Temp_MaterialList','Load #Temp_MaterialList from MVKE_for_Auto ')                                   
               
                                                
INSERT INTO #Temp_MaterialList ( MATNR, VKORG, VTWEG, MaterialSource )                                                
select Distinct  MVKEAuto.MATNR, @SalesOrganization, 'OG', 'T' AS Expr1      
from MVKE as MVKEAuto WITH(NOLOCK)     
WHERE MVKEAuto.MATNR Is Not Null and     
  (MVKEAuto.VKORG)=@SalesOrganization AND     
  (MVKEAuto.VTWEG)='OG' AND     
  (MVKEAuto.VMSTA) not in ('00','02','04','06','19','20') AND     
  (MVKEAuto.PRAT6)='X'     
                              
END     
Else---==========================================@IsAutoReportContent = 0===========================      
begin    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load Material List','Load Material List from ReportContentTemplate')      
 INSERT INTO #Temp_MaterialList ( MATNR, VKORG, VTWEG, MaterialSource )     
SELECT Distinct  D.InternalSAPItemNo, @SalesOrganization, 'OG', 'T'                                             
FROM MST_TemplateData TB WITH(NOLOCK)                                                
    CROSS APPLY OPENJSON (TB.[Data]) WITH                                                
    (                                                
        InternalSAPItemNo NVarchar(100) '$.InternalSAPItemNo'                                                
    ) as  D                                 
 Where TB.TemplateMasterID = @ReportContentTemplateID   and D.InternalSAPItemNo is not null    
    
End         
End    
    
Begin----====================================IncludeSellOffPrices or OverallNetPrices============= 
--load_MaterialList_P_A655_ZNT1+ load_MaterialList_P_A655_ZNTP
if(@CanIncludeSellOffPrices = 1 and @CanIncludeOverallNetPrices = 1)    
Begin    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load Material List','Load Material List from SellOffPrices && OverallNetPrices')      
INSERT INTO #Temp_MaterialList ( MATNR, VKORG, VTWEG, MaterialSource )                                                
SELECT Distinct A655.MATNR, A655.VKORG, A655.VTWEG,'N' AS MaterialSource                                                
FROM (A655 WITH(NOLOCK)      
 LEFT JOIN #Temp_MaterialList MaterialList WITH(NOLOCK) ON (A655.MATNR = MaterialList.MATNR) AND     
           (A655.VKORG = MaterialList.VKORG) AND     
           (A655.VTWEG = MaterialList.VTWEG))                                                                                             
WHERE (((MaterialList.MATNR) Is Null) AND ((A655.KSCHL) in ('ZNT1','ZNTP','ZPAF')))    
--and not EXISTS(select 1 from #Temp_MaterialList where MATNR =A655.MATNR and VKORG =A655.VKORG and A655.VTWEG = VTWEG )     
    
    
    
    
End    
else if(@CanIncludeSellOffPrices = 1 )    
begin    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load Material List','Load Material List from SellOffPrices')      
INSERT INTO #Temp_MaterialList ( MATNR, VKORG, VTWEG, MaterialSource )                                                
SELECT Distinct A655.MATNR, A655.VKORG, A655.VTWEG,'N' AS MaterialSource                                                
FROM (A655 WITH(NOLOCK)     
 LEFT JOIN #Temp_MaterialList MaterialList  WITH(NOLOCK) ON (A655.MATNR = MaterialList.MATNR) AND     
           (A655.VKORG = MaterialList.VKORG) AND     
           (A655.VTWEG = MaterialList.VTWEG))                                                                                             
WHERE (((MaterialList.MATNR) Is Null) AND ((A655.KSCHL) in ('ZNT1')))    
--and not EXISTS(select 1 from #Temp_MaterialList where MATNR =A655.MATNR and VKORG =A655.VKORG and A655.VTWEG = VTWEG )     
end    
else if(@CanIncludeOverallNetPrices = 1 )    
begin    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load Material List','Load Material List from OverallNetPrices')      
INSERT INTO #Temp_MaterialList ( MATNR, VKORG, VTWEG, MaterialSource )                                                
SELECT Distinct A655.MATNR, A655.VKORG, A655.VTWEG,'N' AS MaterialSource                                                
FROM (A655 WITH(NOLOCK)     
 LEFT JOIN #Temp_MaterialList MaterialList  WITH(NOLOCK) ON (A655.MATNR = MaterialList.MATNR) AND     
           (A655.VKORG = MaterialList.VKORG) AND     
           (A655.VTWEG = MaterialList.VTWEG))                                                                                             
WHERE (((MaterialList.MATNR) Is Null) AND ((A655.KSCHL) in ('ZNTP','ZPAF')))    
--and not EXISTS(select 1 from #Temp_MaterialList where MATNR =A655.MATNR and VKORG =A655.VKORG and A655.VTWEG = VTWEG )     
end    
End    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load Material List','Completed')                                                                          
                              
END      
    
update dbo.TRN_PriceFileHeader set [StatusText] ='Loading Prices' ,     
           [PercentCompleted] =25     
           where PriceFileHeaderID =@PriceFileHeaderID    
    
Begin---====================load_Prices=======================================================     
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load prices','Load customer/hierarchy/material net price entries')     
if(@CanIncludeSellOffPrices = 1)--load_P_A655_ZNT1  
Begin    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load prices','@CanIncludeSellOffPrices  =1 load_P_A655_ZNT1 : If requested, load material sell-off price  
s')       
    
INSERT INTO #Temp_Prices ( CustNo, CustPriceList, CustPriceGroup, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART,                                              
PLTYP, MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS Custno, CustomerList.PLTYP AS CustPriceList, CustomerList.KONDA AS CustPriceGroup,                                                
'A655' AS [Table], A655.KAPPL, A655.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                                                
CustomerList.PLTYP, A655.MATNR, A655.KFRST, A655.DATBI, A655.DATAB, A655.KBSTAT, A655.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM (                                                
(#Temp_CustomerList as CustomerList  WITH(NOLOCK)                                                
INNER JOIN A655 WITH(NOLOCK)  ON (CustomerList.SPART = A655.SPART) AND                                                 
                   (CustomerList.VTWEG = A655.VTWEG) AND     
       (CustomerList.VKORG = A655.VKORG))                                              
INNER JOIN #Temp_MaterialList MaterialList  WITH(NOLOCK) ON (A655.MATNR = MaterialList.MATNR) AND                                                 
                                              (A655.VKORG = MaterialList.VKORG) AND     
             (A655.VTWEG = MaterialList.VTWEG))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A655.KNUMH = KONP.KNUMH                                                
WHERE (((A655.KAPPL)='V') AND     
  ((A655.KSCHL)='ZNT1') AND     
  ((A655.DATBI)>=[SelDate]) AND     
  ((A655.DATAB)<=[SelDate]) AND     
  ((KONP.LOEVM_KO)<>'X'))                        
    and not EXISTS(select 1 from #Temp_Prices  WITH(NOLOCK) where MATNR =A655.MATNR and CustNo =CustomerList.KUNNR )                        
                        
     
    
     
End    
if(@CanIncludeCustomerNetPrices = 1)--load_P_A652   
Begin    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load prices','@CanIncludeCustomerNetPrices  =1load_P_A652: If requested, load Material net by Customer')  
                                                
      
INSERT INTO #Temp_Prices ( CustNo, CustPriceList, CustPriceGroup, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART,                                        
KUNNR, MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )  
SELECT CustomerList.KUNNR AS CustNo, CustomerList.PLTYP AS CustPriceList, CustomerList.KONDA AS CustPriceGroup,                                                
'A652' AS [Table], A652.KAPPL, A652.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                                                 
A652.KUNNR, A652.MATNR, A652.KFRST, A652.DATBI, A652.DATAB, A652.KBSTAT, A652.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM                                                
(#Temp_CustomerList as CustomerList  WITH(NOLOCK)                                                
INNER JOIN A652 WITH(NOLOCK)  ON (CustomerList.KUNNR = A652.KUNNR) AND     
     (CustomerList.SPART = A652.SPART) AND     
     (CustomerList.VTWEG = A652.VTWEG) AND     
     (CustomerList.VKORG = A652.VKORG))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A652.KNUMH = KONP.KNUMH                                                
WHERE (((A652.KAPPL)='V') AND     
  ((A652.KSCHL) in ('ZNTP','ZPAF')) AND                                                 
       ((A652.DATBI)>=[SelDate]) AND     
    ((A652.DATAB)<=[SelDate]) AND     
    ((KONP.LOEVM_KO)<>'X'))-- OR (((A652.KSCHL)='ZPAF'))                        
      and not EXISTS(select 1 from #Temp_Prices  WITH(NOLOCK) where MATNR =A652.MATNR and CustNo =CustomerList.KUNNR )              
                                                
     
    
    
End    
    
if(@CanIncludeCustomerHierarchyNetPrices =1)---=============@CanIncludeCustomerHierarchyNetPrices = 1==      
Begin    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load prices','@CanIncludeCustomerHierarchyNetPrices  =1')        
Begin    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load prices','load_P_A657, load_P_A657_1')     
 
 set @Endno  = 1                                                
  set @Startno  = 10                               
  set @SQL =''                                                
  set @ParmDefinition =''                                                
  while(@Startno >= @Endno)                                                
  Begin                                                
  set @SQL = 'insert into #Temp_Prices ( CustNo, CustPriceList, CustPriceGroup, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, HIENR, MATNR,                                                
              KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                        
              Select CustomerList.KUNNR as CustNo, CustomerList.PLTYP as CustPriceList, CustomerList.KONDA as CustPriceGroup,                                              
     ''A657-'+Convert(varchar(100),@Startno) +''' as [Table],    
     A657.KAPPL, A657.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,    
     CustomerList.level'+Convert(varchar(100),@Startno) +',    
     A657.MATNR, A657.KFRST, A657.DATBI, A657.DATAB,                                                
     A657.KBSTAT, A657.KNUMH, KONP.KSTBM, KONP.KBETR                                  
         from (#Temp_CustomerList as CustomerList  WITH(NOLOCK)                                                 
         inner join A657 WITH(NOLOCK)  on (CustomerList.VKORG = A657.VKORG)                                                 
                        and (CustomerList.VTWEG = A657.VTWEG)     
      and (CustomerList.SPART = A657.SPART)                                                 
      and (CustomerList.level'+Convert(varchar(100),@Startno) +' = A657.HIENR))                                                
         inner join KONP WITH(NOLOCK)  on A657.KNUMH = KONP.KNUMH                                                 
         where (((A657.KAPPL)=''V'')                                                
          and ((A657.KSCHL) in (''ZNTP'',''ZPAF''))                                                 
          and ((CustomerList.level'+Convert(varchar(100),@Startno) +') Is Not Null)                                                 
          and ((A657.DATBI)>=[SelDate])                                             
          and ((A657.DATAB)<=[SelDate])                                                 
          and ((KONP.LOEVM_KO)<>''X''))  and not EXISTS(select 1 from #Temp_Prices  WITH(NOLOCK) where MATNR =A657.MATNR and CustNo =CustomerList.KUNNR )';                                                
   EXECUTE sp_executesql @SQL                                                
   --select @SQL                                                
  set @Startno = @Startno-1                                                
                                                
                                                
  End       
    
End    
    
    
End    
    
                                             
if(@CanIncludePriceGroupNets = 1) ---====================@CanIncludePriceGroupNets = 1================       
Begin --load_P_A604_Net   
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load prices','@CanIncludePriceGroupNets =load_P_A604_Net 1- load price group nets')    
    
                                         
INSERT INTO #Temp_Prices ( CustNo, CustPriceList, CustPriceGroup, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART,                                              
PLTYP, KONDA, MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS CustNo, CustomerList.PLTYP AS CustPriceList, CustomerList.KONDA AS CustPriceGroup,                                                
    'A604' AS [Table], A604.KAPPL, A604.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                                                
    CustomerList.PLTYP, CustomerList.KONDA, A604.MATNR, A604.KFRST, A604.DATBI, A604.DATAB, A604.KBSTAT,    
    A604.KNUMH, KONP.KSTBM, KONP.KBETR                            
FROM ((#Temp_CustomerList as CustomerList  WITH(NOLOCK)                                                 
INNER JOIN A604 WITH(NOLOCK)  ON (CustomerList.PLTYP = A604.PLTYP) AND                                                 
       (CustomerList.KONDA = A604.KONDA) AND                                                 
       (CustomerList.SPART = A604.SPART) AND                                                 
       (CustomerList.VTWEG = A604.VTWEG) AND                                                 
       (CustomerList.VKORG = A604.VKORG))                                        
INNER JOIN #Temp_MaterialList as MaterialList  WITH(NOLOCK) ON (A604.MATNR = MaterialList.MATNR) AND                                                 
             (A604.VKORG = MaterialList.VKORG) AND                                                 
             (A604.VTWEG = MaterialList.VTWEG))                                             
INNER JOIN KONP WITH(NOLOCK)  ON A604.KNUMH = KONP.KNUMH                                                
WHERE (((A604.KAPPL)='V') AND                                                 
       ((A604.KSCHL) in ('ZNTP','ZPAF')) AND                                                 
       ((A604.DATBI)>=[SelDate]) AND     
    ((A604.DATAB)<=[SelDate]) AND     
    ((KONP.LOEVM_KO)<>'X')) and     
    not EXISTS(select 1 from #Temp_Prices  WITH(NOLOCK) where MATNR =A604.MATNR and CustNo =CustomerList.KUNNR )      
    
    
End    
if(@CanIncludeOverallNetPrices = 1) --load_P_A655_ZNTP    
Begin    
  Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load prices','@CanIncludeOverallNetPrices = 1 load_P_A655_ZNTP If requested, load material overall net prices if requested')                                                
  INSERT INTO #Temp_Prices ( CustNo, CustPriceList, CustPriceGroup, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART,                                              
  PLTYP, MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                          
  SELECT CustomerList.KUNNR AS Custno, CustomerList.PLTYP AS CustPriceList, CustomerList.KONDA AS CustPriceGroup,                                                
  'A655' AS [Table], A655.KAPPL, A655.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                                                
  CustomerList.PLTYP, A655.MATNR, A655.KFRST, A655.DATBI, A655.DATAB, A655.KBSTAT, A655.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM ((#Temp_CustomerList as CustomerList WITH(NOLOCK)                                                 
INNER JOIN A655 WITH(NOLOCK)  ON (CustomerList.SPART = A655.SPART) AND                                                 
                   (CustomerList.VTWEG = A655.VTWEG) AND                                             
       (CustomerList.VKORG = A655.VKORG))                                                 
INNER JOIN #Temp_MaterialList as  MaterialList WITH(NOLOCK) ON (A655.MATNR = MaterialList.MATNR) AND                                                 
              (A655.VKORG = MaterialList.VKORG) AND                                                 
              (A655.VTWEG = MaterialList.VTWEG))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A655.KNUMH = KONP.KNUMH                                                
WHERE (((A655.KAPPL)='V') AND                                                 
       ((A655.KSCHL)in ('ZNTP','ZPAF')) AND                                        
       ((A655.DATBI)>=[SelDate]) AND                                                 
       ((A655.DATAB)<=[SelDate]) AND                                                 
       ((KONP.LOEVM_KO)<>'X'))                         
  and not EXISTS(select 1 from #Temp_Prices  WITH(NOLOCK) where MATNR =A655.MATNR and CustNo =CustomerList.KUNNR )     
End    
    
Begin--===ZPNP+ZNTP=============     
if(@CanIncludePromoPrice = 1)---====================@CanIncludePromoPrice = 1===ZPNP+ZNTP=============       
  begin    
  Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load prices','@CanIncludePromoPrice = 1 Promo Prices+ZNTP')     
 begin---====================load_P_A653_PC123_ZPNP=========================================================                                                
  Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load prices','load_P_A653_PC123_ZPNP+ZNTP-Promo Prices for Price Class 1-3 and Material')              
                                    
                                                
  INSERT INTO #Temp_Prices ( CustNo, CustPriceList, CustPriceGroup, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART,                                            
  KVGR1, KVGR2, KVGR3, PLTYP, MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS Custno, CustomerList.PLTYP AS CustPriceList, CustomerList.KONDA AS CustPriceGroup,                                                
'A653' AS [Table], A653.KAPPL, A653.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                                     
A653.ZKVGR1 AS KVGR1, A653.YYKVGR2 AS KVGR2, A653.YYKVGR3 AS KVGR3, CustomerList.PLTYP, A653.MATNR, A653.KFRST,                                               
A653.DATBI, A653.DATAB, A653.KBSTAT, A653.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM #Temp_CustomerList as CustomerList  WITH(NOLOCK)                                                
INNER JOIN (                                                
(A653 WITH(NOLOCK)                                                
INNER JOIN     
#Temp_MaterialList as MaterialList  WITH(NOLOCK) ON (A653.VKORG = MaterialList.VKORG) AND                                                 
                                      (A653.VTWEG = MaterialList.VTWEG) AND                                                 
           (A653.MATNR = MaterialList.MATNR))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A653.KNUMH = KONP.KNUMH    
) ON (CustomerList.VKORG = A653.VKORG) AND     
     (CustomerList.VTWEG = A653.VTWEG) AND                       
              (CustomerList.SPART = A653.SPART) AND                                                 
              (CustomerList.KVGR1 = A653.ZKVGR1) AND                           
              (CustomerList.KVGR2 = A653.YYKVGR2) AND                                                 
              (CustomerList.KVGR3 = A653.YYKVGR3)                                                
WHERE (((A653.KAPPL)='V') AND     
  ((A653.KSCHL) in ('ZPNP','ZNTP','ZPAF')) AND                         
  ((A653.DATBI)>=[SelDate]) AND                         
  ((A653.DATAB)<=[SelDate]) AND     
  ((KONP.LOEVM_KO)<>'X'))                        
and not EXISTS(select 1 from #Temp_Prices  WITH(NOLOCK) where MATNR =A653.MATNR and CustNo =CustomerList.KUNNR )                        
End                                                
                                                
 begin---====================load_P_A653_PC12X_ZPNP=========================================================                                                
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load prices','load_P_A653_PC12X_ZPNP+ZNTP-Promo Prices for Price Class 1-2 and Material')               
                                   
                             
 INSERT INTO #Temp_Prices ( CustNo, CustPriceList, CustPriceGroup, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART,                                              
 KVGR1, KVGR2, KVGR3, PLTYP, MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS Custno, CustomerList.PLTYP AS CustPriceList, CustomerList.KONDA AS CustPriceGroup,                                                
'A653' AS [Table], A653.KAPPL, A653.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                                                
A653.ZKVGR1 AS KVGR1, A653.YYKVGR2 AS KVGR2, A653.YYKVGR3 AS KVGR3, CustomerList.PLTYP, A653.MATNR, A653.KFRST,                                              
A653.DATBI, A653.DATAB, A653.KBSTAT, A653.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM #Temp_CustomerList as CustomerList  WITH(NOLOCK)                                               
INNER JOIN (                                                
 (A653 WITH(NOLOCK)                                                 
 INNER JOIN #Temp_MaterialList as MaterialList WITH(NOLOCK) ON (A653.VKORG = MaterialList.VKORG) AND                                                 
   (A653.VTWEG = MaterialList.VTWEG) AND                                                 
            (A653.MATNR = MaterialList.MATNR))                                                 
  INNER JOIN KONP WITH(NOLOCK)  ON A653.KNUMH = KONP.KNUMH    
  ) ON (CustomerList.VKORG = A653.VKORG) AND                                                 
             (CustomerList.VTWEG = A653.VTWEG) AND                                                 
             (CustomerList.SPART = A653.SPART) AND                                                 
             (CustomerList.KVGR1 = A653.ZKVGR1) AND                                                 
             (CustomerList.KVGR2 = A653.YYKVGR2)                                                
WHERE (((A653.KAPPL)='V') AND     
   ((A653.KSCHL) in ('ZPNP','ZNTP','ZPAF')) AND     
   ((A653.YYKVGR3) Is Null Or (A653.YYKVGR3)='') AND     
   ((A653.DATBI)>=[SelDate]) AND     
   ((A653.DATAB)<=[SelDate]) AND     
   ((KONP.LOEVM_KO)<>'X'))                        
and not EXISTS(select 1 from #Temp_Prices WITH(NOLOCK) where MATNR =A653.MATNR and CustNo =CustomerList.KUNNR )                        
                                                
 End                                                
                                                
 begin---====================load_P_A653_PC1XX_ZPNP=========================================================                                                
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load prices','load_P_A653_PC1XX_ZPNP+ZNTP-Promo Prices for Price Class 1 and Material')                  
                                
                                                
 INSERT INTO #Temp_Prices ( CustNo, CustPriceList, CustPriceGroup, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART,                                              
 KVGR1, KVGR2, KVGR3, PLTYP, MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS Custno, CustomerList.PLTYP AS CustPriceList, CustomerList.KONDA AS CustPriceGroup,                                                
'A653' AS [Table], A653.KAPPL, A653.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                                                 
A653.ZKVGR1 AS KVGR1, A653.YYKVGR2 AS KVGR2, A653.YYKVGR3 AS KVGR3, CustomerList.PLTYP, A653.MATNR, A653.KFRST,                                              
A653.DATBI, A653.DATAB, A653.KBSTAT, A653.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM #Temp_CustomerList as CustomerList WITH(NOLOCK)                                            
INNER JOIN (                                                
  (A653 WITH(NOLOCK)  
  INNER JOIN #Temp_MaterialList as  MaterialList WITH(NOLOCK) ON (A653.MATNR = MaterialList.MATNR) AND                                                
                                                          (A653.VTWEG = MaterialList.VTWEG) AND                                                 
                (A653.VKORG = MaterialList.VKORG))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A653.KNUMH = KONP.KNUMH    
) ON (CustomerList.KVGR1 = A653.ZKVGR1) AND                                                 
              (CustomerList.SPART = A653.SPART) AND                                                 
              (CustomerList.VTWEG = A653.VTWEG) AND                                                 
              (CustomerList.VKORG = A653.VKORG)                                                
WHERE (((A653.KAPPL)='V') AND     
 ((A653.KSCHL) in ('ZPNP','ZNTP','ZPAF')) AND     
  ((A653.YYKVGR2) Is Null Or (A653.YYKVGR2)='') AND     
  ((A653.YYKVGR3) Is Null Or (A653.YYKVGR3)='') AND     
  ((A653.DATBI)>=[SelDate]) AND ((A653.DATAB)<=[SelDate]) AND     
  ((KONP.LOEVM_KO)<>'X'))                        
and not EXISTS(select 1 from #Temp_Prices  WITH(NOLOCK) where MATNR =A653.MATNR and CustNo =CustomerList.KUNNR )                        
                                                
                                              
                                                
 End                                                
     
  End    
  else--==================load_P_A653==============================================================    
  Begin--==================load_P_A653==============================================================    
      
 Begin---====================load_P_A653_PC123=============================================================                                                
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load prices','load_P_A653_PC123- Prices for Price Class 1-3 and Material')                               
                   
 INSERT INTO #Temp_Prices ( CustNo, CustPriceList, CustPriceGroup, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART,                                              
 KVGR1, KVGR2, KVGR3, PLTYP, MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS Custno, CustomerList.PLTYP AS CustPriceList, CustomerList.KONDA AS CustPriceGroup,                                                
'A653' AS [Table], A653.KAPPL, A653.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                              
A653.ZKVGR1 AS KVGR1, A653.YYKVGR2 AS KVGR2, A653.YYKVGR3 AS KVGR3, CustomerList.PLTYP, A653.MATNR, A653.KFRST,                          
A653.DATBI, A653.DATAB, A653.KBSTAT, A653.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM #Temp_CustomerList as CustomerList WITH(NOLOCK)                                                 
INNER JOIN (                                                
     (A653 WITH(NOLOCK)     
   INNER JOIN                                                 
   #Temp_MaterialList as MaterialList WITH(NOLOCK) ON (A653.VKORG = MaterialList.VKORG) AND                                                
            (A653.VTWEG = MaterialList.VTWEG) AND                                                 
            (A653.MATNR = MaterialList.MATNR))                                                 
  INNER JOIN KONP WITH(NOLOCK)  ON A653.KNUMH = KONP.KNUMH    
   ) ON (CustomerList.VKORG = A653.VKORG) AND                                                 
           (CustomerList.VTWEG = A653.VTWEG) AND                                                 
     (CustomerList.SPART = A653.SPART) AND                                  
     (CustomerList.KVGR1 = A653.ZKVGR1) AND                                                 
     (CustomerList.KVGR2 = A653.YYKVGR2) AND                                                 
     (CustomerList.KVGR3 = A653.YYKVGR3)                                                
WHERE (((A653.KAPPL)='V') AND                         
   ((A653.KSCHL) in ('ZNTP','ZPAF')) AND                         
   ((A653.DATBI)>=[SelDate]) AND                         
   ((A653.DATAB)<=[SelDate]) AND                         
   ((KONP.LOEVM_KO)<>'X'))      
   and not EXISTS(select 1 from #Temp_Prices WITH(NOLOCK) where MATNR =A653.MATNR and CustNo =CustomerList.KUNNR )                                                
                                     
End     
    
 Begin---====================load_P_A653_PC12X=============================================================                                                
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load prices','load_P_A653_PC12X-Prices for Price Class 1-2 and Material')                               
                   
 INSERT INTO #Temp_Prices ( CustNo, CustPriceList, CustPriceGroup, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART,                                              
 KVGR1, KVGR2, KVGR3, PLTYP, MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS Custno, CustomerList.PLTYP AS CustPriceList, CustomerList.KONDA AS CustPriceGroup,                                                
'A653' AS [Table], A653.KAPPL, A653.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                                                
A653.ZKVGR1 AS KVGR1, A653.YYKVGR2 AS KVGR2, A653.YYKVGR3 AS KVGR3, CustomerList.PLTYP, A653.MATNR, A653.KFRST,                                              
A653.DATBI, A653.DATAB, A653.KBSTAT, A653.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM #Temp_CustomerList as CustomerList WITH(NOLOCK)                                     
INNER JOIN (                                                
  (A653 WITH(NOLOCK)                                                 
  INNER JOIN #Temp_MaterialList as MaterialList WITH(NOLOCK) ON (A653.VKORG = MaterialList.VKORG) AND                                                
             (A653.VTWEG = MaterialList.VTWEG) AND                                                 
             (A653.MATNR = MaterialList.MATNR))                                                 
  INNER JOIN KONP WITH(NOLOCK)  ON A653.KNUMH = KONP.KNUMH    
  ) ON (CustomerList.VKORG = A653.VKORG) AND                                                 
             (CustomerList.VTWEG = A653.VTWEG) AND                                                 
             (CustomerList.SPART = A653.SPART) AND                                                 
             (CustomerList.KVGR1 = A653.ZKVGR1) AND                                                 
             (CustomerList.KVGR2 = A653.YYKVGR2)                                                
WHERE (((A653.KAPPL)='V') AND                         
  ((A653.KSCHL) in ('ZNTP','ZPAF')) AND                         
  ((A653.YYKVGR3) Is Null Or (A653.YYKVGR3)='') AND                         
  ((A653.DATBI)>=[SelDate]) AND                         
  ((A653.DATAB)<=[SelDate]) AND     
  ((KONP.LOEVM_KO)<>'X'))                        
    and not EXISTS(select 1 from #Temp_Prices WITH(NOLOCK) where MATNR =A653.MATNR and CustNo =CustomerList.KUNNR )                        
                                                
 End     
     
 Begin---====================load_P_A653_PC1XX=============================================================                                                
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load prices','load_P_A653_PC1XX-Prices for Price Class 1 and Material')                                  
                
 INSERT INTO #Temp_Prices ( CustNo, CustPriceList, CustPriceGroup, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, KVGR1,                                              
 KVGR2, KVGR3, PLTYP, MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                            
SELECT CustomerList.KUNNR AS Custno, CustomerList.PLTYP AS CustPriceList, CustomerList.KONDA AS CustPriceGroup,                                                
'A653' AS [Table], A653.KAPPL, A653.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                                                
A653.ZKVGR1 AS KVGR1, A653.YYKVGR2 AS KVGR2, A653.YYKVGR3 AS KVGR3, CustomerList.PLTYP, A653.MATNR, A653.KFRST,                                              
A653.DATBI, A653.DATAB, A653.KBSTAT, A653.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM #Temp_CustomerList as CustomerList  WITH(NOLOCK)                                                
INNER JOIN (                                                
 (A653 WITH(NOLOCK)                                                  
 INNER JOIN #Temp_MaterialList as MaterialList WITH(NOLOCK) ON (A653.MATNR = MaterialList.MATNR) AND                                                 
                                                  (A653.VTWEG = MaterialList.VTWEG) AND                                                 
              (A653.VKORG = MaterialList.VKORG))                                                 
 INNER JOIN KONP WITH(NOLOCK)  ON A653.KNUMH = KONP.KNUMH    
 ) ON (CustomerList.KVGR1 = A653.ZKVGR1) AND                                                 
   (CustomerList.SPART = A653.SPART) AND                                                 
   (CustomerList.VTWEG = A653.VTWEG) AND                                                 
   (CustomerList.VKORG = A653.VKORG)                                                
WHERE (((A653.KAPPL)='V') AND     
  ((A653.KSCHL)in ('ZNTP','ZPAF')) AND     
  ((A653.YYKVGR2) Is Null Or (A653.YYKVGR2)='') AND     
  ((A653.YYKVGR3) Is Null Or (A653.YYKVGR3)='') AND     
  ((A653.DATBI)>=[SelDate]) AND     
  ((A653.DATAB)<=[SelDate]) AND     
  ((KONP.LOEVM_KO)<>'X'))                        
and not EXISTS(select 1 from #Temp_Prices WITH(NOLOCK) where MATNR =A653.MATNR and CustNo =CustomerList.KUNNR )                        
                                                
                                                
 End                                                
     
  End    
End    
    
Begin---====================load_P_A604====================================================================       
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load prices','load_P_A604-Material by Price Group and Price List')                                        
          
                                   
INSERT INTO #Temp_Prices ( CustNo, CustPriceList, CustPriceGroup, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART,                                              
PLTYP, KONDA, MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS CustNo, CustomerList.PLTYP AS CustPriceList, CustomerList.KONDA AS CustPriceGroup,                                                
'A604' AS [Table], A604.KAPPL, A604.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                                                
CustomerList.PLTYP, CustomerList.KONDA, A604.MATNR, A604.KFRST, A604.DATBI, A604.DATAB, A604.KBSTAT, A604.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM (                                                
(#Temp_CustomerList as CustomerList WITH(NOLOCK)                                                 
INNER JOIN A604 WITH(NOLOCK)  ON (CustomerList.PLTYP = A604.PLTYP) AND                                                 
                   (CustomerList.KONDA = A604.KONDA) AND                                                 
       (CustomerList.SPART = A604.SPART) AND                                                 
       (CustomerList.VTWEG = A604.VTWEG) AND                                                 
       (CustomerList.VKORG = A604.VKORG))                                                 
INNER JOIN #Temp_MaterialList as MaterialList WITH(NOLOCK) ON (A604.MATNR = MaterialList.MATNR) AND                                                 
                                                 (A604.VKORG = MaterialList.VKORG) AND                                                 
             (A604.VTWEG = MaterialList.VTWEG))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A604.KNUMH = KONP.KNUMH                                                
WHERE (((A604.KAPPL)='V') AND                         
   ((A604.KSCHL)='ZPR0') AND                         
   ((A604.DATBI)>=[SelDate]) AND                         
   ((A604.DATAB)<=[SelDate]) AND ((KONP.LOEVM_KO)<>'X'))                        
 and not EXISTS(select 1 from #Temp_Prices WITH(NOLOCK) where MATNR =A604.MATNR and CustNo =CustomerList.KUNNR )                        
                                                
End      
    
Begin---====================load_P_A507===================================================================        
--Violation of PRIMARY KEY constraint 'PK_Temp_Prices_CustNo_MATNR'. Cannot insert duplicate key in object 'dbo.#Temp_Prices'. The duplicate key value is (0000044327, WSCF227/2X-RG).                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load prices','load_P_A507-Material by Sales Area Price List (trade)')                                     
             
INSERT INTO #Temp_Prices ( CustNo, CustPriceList, CustPriceGroup, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART,                                              
PLTYP, MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS Custno, CustomerList.PLTYP AS CustPriceList, CustomerList.KONDA AS CustPriceGroup,                                                
'A507' AS [Table], A507.KAPPL, A507.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                                                
CustomerList.PLTYP, A507.MATNR, A507.KFRST, A507.DATBI, A507.DATAB, A507.KBSTAT, A507.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM (                                                
( #Temp_CustomerList as CustomerList WITH(NOLOCK)                                                 
INNER JOIN A507 WITH(NOLOCK)  ON (CustomerList.PLTYP = A507.PLTYP) AND                                                 
                   (CustomerList.SPART = A507.SPART) AND                                                 
       (CustomerList.VTWEG = A507.VTWEG) AND                                                 
       (CustomerList.VKORG = A507.VKORG))                                                 
INNER JOIN #Temp_MaterialList as MaterialList WITH(NOLOCK) ON (A507.MATNR = MaterialList.MATNR) AND                                   
                                                (A507.VKORG = MaterialList.VKORG) AND                                                 
            (A507.VTWEG = MaterialList.VTWEG))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A507.KNUMH = KONP.KNUMH                        
WHERE (((A507.KAPPL)='V') AND                         
   ((A507.KSCHL)='ZPR0') AND                         
   ((A507.DATBI)>=[SelDate]) AND                         
   ((A507.DATAB)<=[SelDate]) AND                         
   ((KONP.LOEVM_KO)<>'X'))                         
  and not EXISTS(select 1 from #Temp_Prices WITH(NOLOCK) where MATNR =A507.MATNR and CustNo =CustomerList.KUNNR )                         
                          
                                                
--select * from #Temp_Prices                                                
                                                
End                                                
     
    
     
    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Load prices','Completed')     
End    
    
Begin---====================load_MaterialList_P_Net===========================================     
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','load_MaterialList_P_Net','load_MaterialList_P_A65x: Add material from customer and sell-off net prices to material list')                                                
                                                
INSERT INTO #Temp_MaterialList ( MATNR, VKORG, VTWEG, MaterialSource )                                                
SELECT Distinct Prices.MATNR, Prices.VKORG, Prices.VTWEG, 'N' AS MaterialSource                                                
FROM                                                 
(#Temp_Prices as Prices WITH(NOLOCK)                                                 
LEFT JOIN #Temp_MaterialList as MaterialList WITH(NOLOCK) ON (Prices.VTWEG = MaterialList.VTWEG) AND                                                 
                                                (Prices.VKORG = MaterialList.VKORG) AND                                                 
            (Prices.MATNR = MaterialList.MATNR))                                                                                         
WHERE (((MaterialList.MATNR) Is Null) AND ((Left([Table],4)) In ('A652','A653','A655','A657','A604','A507')));                                                
                                                
                       
End                                                
                                                
Begin---====================load_MaterialList_hier===========================================      
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','load_MaterialList_hier','Add Product Hierarchy to Material List')                               
if(@SelectedProductHierarchy = 1)---====================@SelectedProductHierarchy = 1================                                                
begin                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','load_MaterialList_hier','@SelectedProductHierarchy = 1-load_MaterialList_PRDHA_MARA: Load standard produ  
ct hierarchy from MARA')                                                
                                                
   UPDATE ML                                         
   SET ML.PRDHA1 = SUBSTRING([PRDHA],1,2),                                                
       ML.PRDHA2 = SUBSTRING([PRDHA],3,3),                                                
       ML.PRDHA3 = SUBSTRING([PRDHA],6,3),                                                
    ML.PRDHA4 = SUBSTRING([PRDHA],9,3),                                                 
       ML.PRDHA5 = SUBSTRING([PRDHA],12,3),                                                
       ML.PRDHA6 = SUBSTRING([PRDHA],15,4),--Mid([PRDHA],15,4)                                         
       ML.SelProdHier = 1                                                
   From                                                
   #Temp_MaterialList ML  WITH(NOLOCK) INNER JOIN MARA WITH(NOLOCK)  ON ML.MATNR = MARA.MATNR                                                
    WHERE (((ML.PRDHA1) Is Null Or (ML.PRDHA1)='')) and [PRDHA] is not null;     
   
  
  UPDATE ML                                         
   SET ML.MainGroupPRODH = SUBSTRING([PRDHA],1,2),   
    ML.MainGroupPRODHDescription = MainGroupDescription.VTEXT,  
       ML.[GroupPRODH] = SUBSTRING([PRDHA],1,5),        
    ML.GroupPRODHDescription = MainGroupDescription.VTEXT,  
       ML.SubGroupPRODH = SUBSTRING([PRDHA],1,8),     
    ML.SubGroupPRODHDescription = MainGroupDescription.VTEXT,  
       ML.SelProdHier = 1                                                
   From                                                
   #Temp_MaterialList ML  WITH(NOLOCK) INNER JOIN MARA WITH(NOLOCK)  ON ML.MATNR = MARA.MATNR   
 LEFT JOIN T179T as MainGroupDescription WITH(NOLOCK) on MainGroupDescription.PRODH = SUBSTRING([PRDHA],1,2)  
LEFT JOIN T179T as GroupDescription WITH(NOLOCK) on GroupDescription.PRODH = SUBSTRING([PRDHA],1,5)  
LEFT JOIN T179T as SubGroupDescription WITH(NOLOCK) on SubGroupDescription.PRODH = SUBSTRING([PRDHA],1,8)  
   Where  [PRDHA] is not null  
                                                
End                                              
                                                
if(@SelectedProductHierarchy = 2)---====================@SelectedProductHierarchy = 2================                                                
begin                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','load_MaterialList_hier','@SelectedProductHierarchy = 2-load_MaterialList_PRDHA_MVKE:Load local product h  
ierarchy from MVKE')                                                
                                                
                                                
                                                
 UPDATE ML                                                
                                                 
   SET ML.PRDHA1 = SUBSTRING([PRODH],1,2),                                    
          ML.PRDHA2 = SUBSTRING([PRODH],3,3),                                                  
          ML.PRDHA3 = SUBSTRING([PRODH],6,3),                                                 
          ML.PRDHA4 = SUBSTRING([PRODH],9,3),                                                 
          ML.PRDHA5 = SUBSTRING([PRODH],12,3),                                                 
          ML.PRDHA6 = SUBSTRING([PRODH],15,4),                                                
          ML.SelProdHier = 2                                                
From                                                
 #Temp_MaterialList ML WITH(NOLOCK)                                                
      INNER JOIN MVKE WITH(NOLOCK)  ON (ML.MATNR = MVKE.MATNR) AND (ML.VKORG = MVKE.VKORG) AND (ML.VTWEG = MVKE.VTWEG)                             
   WHERE (((ML.PRDHA1) Is Null Or (ML.PRDHA1)='')) and  [PRODH] is not null;        
     
  
 if(@SalesOrganization = 'AU01')
 begin
  
     UPDATE ML                                                  
                                                   
   SET ML.MainGroupPRODH = SUBSTRING(MVKE.[PRODH],1,5),   
       ML.MainGroupPRODHDescription = MainGroupDescription.VTEXT,   
       ML.[GroupPRODH] = SUBSTRING(MVKE.[PRODH],1,8),     
    ML.GroupPRODHDescription = MainGroupDescription.VTEXT,     
       ML.SubGroupPRODH = SUBSTRING(MVKE.[PRODH],1,11),     
    ML.SubGroupPRODHDescription = MainGroupDescription.VTEXT,   
       ML.SelProdHier = 2                                                  
From                                                  
 #Temp_MaterialList ML WITH(NOLOCK)                                                 
      INNER JOIN MVKE WITH(NOLOCK)  ON (ML.MATNR = MVKE.MATNR) AND (ML.VKORG = MVKE.VKORG) AND (ML.VTWEG = MVKE.VTWEG)     
   LEFT JOIN T179T as MainGroupDescription WITH(NOLOCK) on MainGroupDescription.PRODH = SUBSTRING(MVKE.[PRODH],1,5)    
   LEFT JOIN T179T as GroupDescription WITH(NOLOCK) on GroupDescription.PRODH = SUBSTRING(MVKE.[PRODH],1,8)    
  LEFT JOIN T179T as SubGroupDescription WITH(NOLOCK) on SubGroupDescription.PRODH = SUBSTRING(MVKE.[PRODH],1,11)    
   where MVKE.[PRODH] is not null    

   end
   else
   begin
     UPDATE ML                                                  
                                                   
   SET ML.MainGroupPRODH = SUBSTRING(MVKE.[PRODH],1,2),   
       ML.MainGroupPRODHDescription = MainGroupDescription.VTEXT,   
       ML.[GroupPRODH] = SUBSTRING(MVKE.[PRODH],1,5),     
    ML.GroupPRODHDescription = MainGroupDescription.VTEXT,     
       ML.SubGroupPRODH = SUBSTRING(MVKE.[PRODH],1,8),     
    ML.SubGroupPRODHDescription = MainGroupDescription.VTEXT,   
       ML.SelProdHier = 2                                                  
From                                                  
 #Temp_MaterialList ML WITH(NOLOCK)                                                 
      INNER JOIN MVKE WITH(NOLOCK)  ON (ML.MATNR = MVKE.MATNR) AND (ML.VKORG = MVKE.VKORG) AND (ML.VTWEG = MVKE.VTWEG)     
   LEFT JOIN T179T as MainGroupDescription WITH(NOLOCK) on MainGroupDescription.PRODH = SUBSTRING(MVKE.[PRODH],1,2)    
   LEFT JOIN T179T as GroupDescription WITH(NOLOCK) on GroupDescription.PRODH = SUBSTRING(MVKE.[PRODH],1,5)    
  LEFT JOIN T179T as SubGroupDescription WITH(NOLOCK) on SubGroupDescription.PRODH = SUBSTRING(MVKE.[PRODH],1,8)    
   where MVKE.[PRODH] is not null  
   end
  
                                                
End                                                
                                                
                                               
                                                
End       
    
update dbo.TRN_PriceFileHeader set [StatusText] ='Loading Discounts' ,     
           [PercentCompleted] =30     
           where PriceFileHeaderID =@PriceFileHeaderID    
    
Begin---====================Discounts=========================================================     
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','Start')     
Begin    
    
insert into #Temp_DiscountTypeList(DiscountName,DiscountValue,[Status])     
SELECT Distinct  D.DiscountName as DiscountTypeID, trim(D.DiscountValue) as DiscountType, case     
             when DiscountName = 'Discount1' then @CanIncludeDiscount1    
             when DiscountName = 'Discount2' then @CanIncludeDiscount2    
             when DiscountName = 'Discount3' then @CanIncludeDiscount3    
             when DiscountName = 'Discount4' then @CanIncludeDiscount4    
             when DiscountName = 'Discount5' then @CanIncludeDiscount5    
             when DiscountName = 'Discount6' then @CanIncludeDiscount6    
             when DiscountName = 'Discount7' then @CanIncludeDiscount7    
             when DiscountName = 'Discount8' then @CanIncludeDiscount8 else cast(0 as bit) end as [Status]    
    
from                                          
 MST_TemplateData TB  WITH(NOLOCK)                                               
    CROSS APPLY OPENJSON (TB.[Data]) WITH                                                
    (       
     DiscountName NVarchar(100) '$.DiscountName',    
  DiscountValue NVarchar(100) '$.DiscountValue'    
    ) as  D      
 where TB.TemplateMasterID = (select top 1 TemplateMasterID from [dbo].[MST_TemplateMaster] WITH(NOLOCK)  where TemplateName ='DiscountParameters')    
End    
                                               
set @SQL =''                                                
set @ParmDefinition =''                                                
     
    
begin    
    
Begin--====================load_D_A652=================================================================     
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_A652 - Load A652 discounts for Sales Area, Customer, Material, and condition type- '  
)                                                
    
 INSERT INTO #Temp_Discounts ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, KVGR1, KVGR2, KVGR3,                                               
PRODH1, PRODH2, PRODH3, PRODH4, PRODH5, PRODH6, MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS CustNo, 'A652' AS [Table], A652.KAPPL, A652.KSCHL,                                                
CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART, '' AS KVGR1, '' AS KVGR2, '' AS KVGR3, ''                                                
AS PRODH1, '' AS PRODH2, '' AS PRODH3, '' AS PRODH4, '' AS PRODH5, '' AS PRODH6, A652.MATNR,                    
A652.KFRST, A652.DATBI, A652.DATAB, A652.KBSTAT, A652.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM (#Temp_CustomerList as CustomerList WITH(NOLOCK)                                                
  INNER JOIN A652 WITH(NOLOCK)  ON (CustomerList.VKORG = A652.VKORG) AND                                                 
                     (CustomerList.VTWEG = A652.VTWEG) AND                                                 
      (CustomerList.SPART = A652.SPART) AND                                                 
      (CustomerList.KUNNR = A652.KUNNR))                                                 
  INNER JOIN KONP WITH(NOLOCK)  ON A652.KNUMH = KONP.KNUMH                                                
WHERE ((((A652.KAPPL)='V') AND     
  ((A652.KSCHL) in (select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where Status =1)) AND     
  ((A652.DATBI)>=[SelDate]) AND     
  ((A652.DATAB)<=[SelDate]) AND     
  ((KONP.LOEVM_KO)<>'X')))         
and not EXISTS(select 1 from #Temp_Discounts WITH(NOLOCK) where CustNo =CustomerList.KUNNR and MATNR =A652.MATNR and KSCHL = A652.KSCHL )         
End     
    
Begin--====================Load A996 discounts==========================================================    
  Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','Load A996 discounts for Sales Area, Price Class 3, Price Group, Product Hierarchy 1-3, and  
 condition type')                                                
  Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','Clear Discounts_PH')                                                
  truncate Table #Temp_Discounts_PH                                     
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_A996_PH-Discounts for Sales Area, Price Class 3 and Price Group')                     
                             
                                          
INSERT INTO #Temp_Discounts_PH ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, HIENR, KVGR1, KVGR2, KVGR3,                                              
PRODH1, PRODH2, PRODH3, PRODH4, PRODH5, PRODH6, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS CustNo, 'A996' AS [Table], A996.KAPPL, A996.KSCHL,                                                 
CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART, '' AS HIENR, '' AS KVGR1, '' AS KVGR2, A996.YYKVGR3 AS KVGR3,                                         
A996.PRODH1, A996.PRODH2, A996.PRODH3, '' AS PRODH4, '' AS PRODH5, '' AS PRODH6, '' AS KFRST, A996.DATBI, A996.DATAB,                                              
'' AS KBSTAT, A996.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM (A996  WITH(NOLOCK)                       
INNER JOIN #Temp_CustomerList as  CustomerList WITH(NOLOCK) ON (A996.KONDA = CustomerList.KONDA) AND                                                 
              (A996.YYKVGR3 = CustomerList.KVGR3) AND                                                 
              (A996.SPART = CustomerList.SPART) AND                                                 
              (A996.VTWEG = CustomerList.VTWEG) AND                                                 
              (A996.VKORG = CustomerList.VKORG))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A996.KNUMH = KONP.KNUMH                                                
WHERE (((A996.KAPPL)='V') AND     
  ((A996.KSCHL) in (select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where Status =1)) AND     
  ((A996.DATBI)>=[SelDate]) AND     
  ((A996.DATAB)<=[SelDate]) AND     
  ((KONP.LOEVM_KO)<>'X'));                                                
                                   
                                                
                                                
 Declare @Level int                                                
 Declare @SQLDiscounts_PH nvarchar(max)                                                
 Declare @SQLDiscounts_PH_ParmDefinition nvarchar(100)                                                
 SET @SQLDiscounts_PH_ParmDefinition = N'@Level int';                                            
                                                 
Begin--====================load_D_PH 3=================================================================                                                
                                                
  Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_PH 3Load Discounts table using product hierarchy 1 to 3')                           
                       
   --Need to write this code in seperate function or procedure                                                
                                                  
                                                  
                                                
SET @SQLDiscounts_PH =  'if  exists(select top 1 1 from #Temp_Discounts_PH)                                                
  begin                                                
  If (@Level >= 6)                                                 
  begin                                                
                                             
   INSERT INTO #Temp_Discounts ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, HIENR, KVGR1, KVGR2, KVGR3,                                              
   PRODH1, PRODH2, PRODH3, PRODH4, PRODH5, PRODH6, MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT Discounts_PH.CustNo, Discounts_PH.[Table], Discounts_PH.KAPPL, Discounts_PH.KSCHL, Discounts_PH.VKORG, Discounts_PH.VTWEG, Discounts_PH.SPART,                                      
Discounts_PH.HIENR, Discounts_PH.KVGR1, Discounts_PH.KVGR2, Discounts_PH.KVGR3, Discounts_PH.PRODH1, Discounts_PH.PRODH2, Discounts_PH.PRODH3, Discounts_PH.PRODH4,                                                
Discounts_PH.PRODH5, Discounts_PH.PRODH6, MaterialList.MATNR, Discounts_PH.KFRST, Discounts_PH.DATBI, Discounts_PH.DATAB, Discounts_PH.KBSTAT, Discounts_PH.KNUMH,                                
Discounts_PH.KSTBM, Discounts_PH.KBETR                                                
FROM #Temp_Discounts_PH as Discounts_PH WITH(NOLOCK)                                                
INNER JOIN #Temp_MaterialList as MaterialList WITH(NOLOCK) ON (Discounts_PH.PRODH6 = MaterialList.PRDHA6) AND                                                
                                               (Discounts_PH.PRODH5 = MaterialList.PRDHA5) AND                                                 
             (Discounts_PH.PRODH4 = MaterialList.PRDHA4) AND                                                 
             (Discounts_PH.PRODH3 = MaterialList.PRDHA3) AND                                                 
             (Discounts_PH.PRODH2 = MaterialList.PRDHA2) AND                                           
             (Discounts_PH.PRODH1 = MaterialList.PRDHA1)        
     where  not EXISTS(select 1 from #Temp_Discounts WITH(NOLOCK) where CustNo =Discounts_PH.CustNo and MATNR =MaterialList.MATNR and KSCHL = Discounts_PH.KSCHL )         
                                                
  end                                                
   If (@Level >= 5)                                                 
  begin                                                
                                          
  INSERT INTO #Temp_Discounts ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, HIENR, KVGR1, KVGR2, KVGR3, PRODH1, PRODH2, PRODH3, PRODH4, PRODH5, PRODH6,                                               
  MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT Discounts_PH.CustNo, Discounts_PH.[Table], Discounts_PH.KAPPL, Discounts_PH.KSCHL, Discounts_PH.VKORG, Discounts_PH.VTWEG, Discounts_PH.SPART,                                                
Discounts_PH.HIENR, Discounts_PH.KVGR1, Discounts_PH.KVGR2, Discounts_PH.KVGR3, Discounts_PH.PRODH1, Discounts_PH.PRODH2, Discounts_PH.PRODH3, Discounts_PH.PRODH4,                                                
Discounts_PH.PRODH5, Discounts_PH.PRODH6, MaterialList.MATNR, Discounts_PH.KFRST, Discounts_PH.DATBI, Discounts_PH.DATAB, Discounts_PH.KBSTAT, Discounts_PH.KNUMH,                        
Discounts_PH.KSTBM, Discounts_PH.KBETR                                                
FROM #Temp_Discounts_PH as  Discounts_PH WITH(NOLOCK)                                   
INNER JOIN #Temp_MaterialList as  MaterialList WITH(NOLOCK) ON (Discounts_PH.PRODH5 = MaterialList.PRDHA5) AND                                                 
                                                  (Discounts_PH.PRODH4 = MaterialList.PRDHA4) AND                                          
              (Discounts_PH.PRODH3 = MaterialList.PRDHA3) AND                                                 
              (Discounts_PH.PRODH2 = MaterialList.PRDHA2) AND                                                 
              (Discounts_PH.PRODH1 = MaterialList.PRDHA1)                                                
WHERE ((((Discounts_PH.PRODH6) Is Null Or (Discounts_PH.PRODH6)='''')) )          
 and not EXISTS(select 1 from #Temp_Discounts WITH(NOLOCK) where CustNo =Discounts_PH.CustNo and MATNR =MaterialList.MATNR and KSCHL = Discounts_PH.KSCHL )         
                                                
  end                                                
   If (@Level >= 4)                                                 
   begin                                                
                               
  INSERT INTO #Temp_Discounts ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, HIENR, KVGR1, KVGR2, KVGR3, PRODH1, PRODH2, PRODH3, PRODH4, PRODH5, PRODH6,                                              
  MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT Discounts_PH.CustNo, Discounts_PH.[Table], Discounts_PH.KAPPL, Discounts_PH.KSCHL, Discounts_PH.VKORG, Discounts_PH.VTWEG, Discounts_PH.SPART,                                                
Discounts_PH.HIENR, Discounts_PH.KVGR1, Discounts_PH.KVGR2, Discounts_PH.KVGR3, Discounts_PH.PRODH1, Discounts_PH.PRODH2, Discounts_PH.PRODH3, Discounts_PH.PRODH4,                                                 
Discounts_PH.PRODH5, Discounts_PH.PRODH6, MaterialList.MATNR, Discounts_PH.KFRST, Discounts_PH.DATBI, Discounts_PH.DATAB, Discounts_PH.KBSTAT, Discounts_PH.KNUMH,                                                
Discounts_PH.KSTBM, Discounts_PH.KBETR                                                
FROM #Temp_Discounts_PH as  Discounts_PH WITH(NOLOCK)                                                  
INNER JOIN #Temp_MaterialList as  MaterialList WITH(NOLOCK) ON (Discounts_PH.PRODH4 = MaterialList.PRDHA4) AND                                                 
                                                  (Discounts_PH.PRODH3 = MaterialList.PRDHA3) AND           
              (Discounts_PH.PRODH2 = MaterialList.PRDHA2) AND                                                 
              (Discounts_PH.PRODH1 = MaterialList.PRDHA1)                                                
WHERE ((((Discounts_PH.PRODH5) Is Null Or (Discounts_PH.PRODH5)='''') AND ((Discounts_PH.PRODH6) Is Null Or (Discounts_PH.PRODH6)='''')))         
and not EXISTS(select 1 from #Temp_Discounts WITH(NOLOCK) where CustNo =Discounts_PH.CustNo and MATNR =MaterialList.MATNR and KSCHL = Discounts_PH.KSCHL )         
                                                
                                                
                                                
  end                                                
   If (@Level >= 3)                                                 
   begin                                                
                                                
 INSERT INTO #Temp_Discounts ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, HIENR, KVGR1, KVGR2, KVGR3, PRODH1, PRODH2, PRODH3, PRODH4, PRODH5, PRODH6,                                              
 MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT Discounts_PH.CustNo, Discounts_PH.[Table], Discounts_PH.KAPPL, Discounts_PH.KSCHL, Discounts_PH.VKORG, Discounts_PH.VTWEG, Discounts_PH.SPART,                                                
Discounts_PH.HIENR, Discounts_PH.KVGR1, Discounts_PH.KVGR2, Discounts_PH.KVGR3, Discounts_PH.PRODH1, Discounts_PH.PRODH2, Discounts_PH.PRODH3, Discounts_PH.PRODH4,                                                
Discounts_PH.PRODH5, Discounts_PH.PRODH6, MaterialList.MATNR, Discounts_PH.KFRST, Discounts_PH.DATBI, Discounts_PH.DATAB, Discounts_PH.KBSTAT, Discounts_PH.KNUMH,                                                
Discounts_PH.KSTBM, Discounts_PH.KBETR                                                
FROM #Temp_Discounts_PH as  Discounts_PH WITH(NOLOCK)                                                  
INNER JOIN #Temp_MaterialList as  MaterialList WITH(NOLOCK) ON (Discounts_PH.PRODH3 = MaterialList.PRDHA3) AND                                                
                                                  (Discounts_PH.PRODH2 = MaterialList.PRDHA2) AND                                
              (Discounts_PH.PRODH1 = MaterialList.PRDHA1)                                                
WHERE ((((Discounts_PH.PRODH4) Is Null Or (Discounts_PH.PRODH4)='''') AND                                   
((Discounts_PH.PRODH5) Is Null Or (Discounts_PH.PRODH5)='''') AND ((Discounts_PH.PRODH6) Is Null Or (Discounts_PH.PRODH6)='''')))        
and not EXISTS(select 1 from #Temp_Discounts WITH(NOLOCK) where CustNo =Discounts_PH.CustNo and MATNR =MaterialList.MATNR and KSCHL = Discounts_PH.KSCHL )         
                                                
                                                
  end                                                
   If (@Level >= 2)                                                 
   begin                                                
                                                
 INSERT INTO #Temp_Discounts ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, HIENR, KVGR1, KVGR2, KVGR3, PRODH1, PRODH2, PRODH3, PRODH4, PRODH5, PRODH6,                                               
 MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT Discounts_PH.CustNo, Discounts_PH.[Table], Discounts_PH.KAPPL, Discounts_PH.KSCHL, Discounts_PH.VKORG, Discounts_PH.VTWEG, Discounts_PH.SPART,                                                
Discounts_PH.HIENR, Discounts_PH.KVGR1, Discounts_PH.KVGR2, Discounts_PH.KVGR3, Discounts_PH.PRODH1, Discounts_PH.PRODH2, Discounts_PH.PRODH3, Discounts_PH.PRODH4,                                                
Discounts_PH.PRODH5, Discounts_PH.PRODH6, MaterialList.MATNR, Discounts_PH.KFRST, Discounts_PH.DATBI, Discounts_PH.DATAB, Discounts_PH.KBSTAT, Discounts_PH.KNUMH,                                                
Discounts_PH.KSTBM, Discounts_PH.KBETR                                                
FROM #Temp_Discounts_PH as  Discounts_PH WITH(NOLOCK)                                                  
INNER JOIN #Temp_MaterialList as MaterialList WITH(NOLOCK) ON (Discounts_PH.PRODH2 = MaterialList.PRDHA2) AND                                                 
                                                  (Discounts_PH.PRODH1 = MaterialList.PRDHA1)                                                
WHERE ((((Discounts_PH.PRODH3) Is Null Or (Discounts_PH.PRODH3)='''') AND ((Discounts_PH.PRODH4) Is Null Or (Discounts_PH.PRODH4)='''') AND                                              
((Discounts_PH.PRODH5) Is Null Or (Discounts_PH.PRODH5)='''') AND ((Discounts_PH.PRODH6) Is Null Or (Discounts_PH.PRODH6)='''')))        
and not EXISTS(select 1 from #Temp_Discounts WITH(NOLOCK) where CustNo =Discounts_PH.CustNo and MATNR =MaterialList.MATNR and KSCHL = Discounts_PH.KSCHL )         
                                                
                                                
  end                                                
    If (@Level >= 1)                                                 
   begin                           
                                               
 INSERT INTO #Temp_Discounts ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, HIENR, KVGR1, KVGR2, KVGR3, PRODH1, PRODH2, PRODH3, PRODH4, PRODH5, PRODH6,                                               
 MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT Discounts_PH.CustNo, Discounts_PH.[Table], Discounts_PH.KAPPL, Discounts_PH.KSCHL, Discounts_PH.VKORG, Discounts_PH.VTWEG, Discounts_PH.SPART,                                                
Discounts_PH.HIENR, Discounts_PH.KVGR1, Discounts_PH.KVGR2, Discounts_PH.KVGR3, Discounts_PH.PRODH1, Discounts_PH.PRODH2, Discounts_PH.PRODH3, Discounts_PH.PRODH4,                                                
Discounts_PH.PRODH5, Discounts_PH.PRODH6, MaterialList.MATNR, Discounts_PH.KFRST, Discounts_PH.DATBI, Discounts_PH.DATAB, Discounts_PH.KBSTAT, Discounts_PH.KNUMH,                                                
Discounts_PH.KSTBM, Discounts_PH.KBETR                                                
FROM #Temp_Discounts_PH as  Discounts_PH WITH(NOLOCK)                                                 
INNER JOIN #Temp_MaterialList as  MaterialList ON Discounts_PH.PRODH1 = MaterialList.PRDHA1                                                
WHERE ((((Discounts_PH.PRODH2) Is Null Or (Discounts_PH.PRODH2)='''') AND     
       ((Discounts_PH.PRODH3) Is Null Or (Discounts_PH.PRODH3)='''') AND     
    ((Discounts_PH.PRODH4) Is Null Or (Discounts_PH.PRODH4)='''') AND     
    ((Discounts_PH.PRODH5) Is Null Or (Discounts_PH.PRODH5)='''') AND     
    ((Discounts_PH.PRODH6) Is Null Or (Discounts_PH.PRODH6)='''')))        
and not EXISTS(select 1 from #Temp_Discounts WITH(NOLOCK) where CustNo =Discounts_PH.CustNo and MATNR =MaterialList.MATNR and KSCHL = Discounts_PH.KSCHL )         
                                      
                                                
  end                                                
                                                
  End'                                                
  --select @SQLDiscounts_PH                                                
                                                  
  Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_PH 3-A996-Before executing')                                          
  set @Level = 3                                                
 EXECUTE sp_executesql @SQLDiscounts_PH,@SQLDiscounts_PH_ParmDefinition ,@Level = @Level                                                
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_PH 3-Alter executing')                                                
  End                                                
 END                                                
                                                
Begin---====================load_D_A703_PH==============================================================    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_A703_PH-Discounts for Sales Area and Customer')                                       
     
  truncate Table #Temp_Discounts_PH                               
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','Discounts_PH-Clear Discounts_PH')                                                
                                                
INSERT INTO #Temp_Discounts_PH (CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, KVGR1, KVGR2, KVGR3,                                               
PRODH1, PRODH2, PRODH3, PRODH4, PRODH5, PRODH6, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS CustNo, 'A703' AS [Table], A703.KAPPL, A703.KSCHL,                                                
CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART, '' AS KVGR1, '' AS KVGR2, '' AS KVGR3,                                                
A703.PRODH1, A703.PRODH2, A703.PRODH3, A703.YPRODH4, '' AS PRODH5, '' AS PRODH6, A703.KFRST, A703.DATBI, A703.DATAB, A703.KBSTAT,                                              
A703.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM (                                                
#Temp_CustomerList as CustomerList WITH(NOLOCK)                                                 
INNER JOIN A703 WITH(NOLOCK)  ON (CustomerList.KUNNR = A703.KUNNR) AND                                                 
                   (CustomerList.VKORG = A703.VKORG) AND                                                 
       (CustomerList.VTWEG = A703.VTWEG) AND                                                 
       (CustomerList.SPART = A703.SPART))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A703.KNUMH = KONP.KNUMH                                                
WHERE (((A703.KAPPL)='V') AND     
  ((A703.KSCHL) in (select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where Status =1)) AND     
  ((A703.DATBI)>=[SelDate]) AND     
  ((A703.DATAB)<=[SelDate]) AND     
  ((KONP.LOEVM_KO)<>'X'));                                                
                                                
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_PH 6-A703-Before executing')                                                
  set @Level = 6                                                
 EXECUTE sp_executesql @SQLDiscounts_PH,@SQLDiscounts_PH_ParmDefinition ,@Level = @Level                                                
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_PH 6-Alter executing')                                                
        
End                               
                                                
Begin---====================load_D_A606_PH==============================================================    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_A606_PH-Discounts for Sales Area and Customer')                                        
          
  truncate Table #Temp_Discounts_PH                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','Clear Discounts_PH')                                                
                                                
INSERT INTO #Temp_Discounts_PH ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, KVGR1, KVGR2, KVGR3,                                              
PRODH1, PRODH2, PRODH3, PRODH4, PRODH5, PRODH6, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS CustNo, 'A606' AS [Table], A606.KAPPL, A606.KSCHL,                                                
CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART, '' AS KVGR1, '' AS KVGR2, '' AS KVGR3,                                                
A606.PRODH1, A606.PRODH2, A606.PRODH3, '' AS PRODH4, '' AS PRODH5, '' AS PRODH6, A606.KFRST, A606.DATBI,                           
A606.DATAB, A606.KBSTAT, A606.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM (#Temp_CustomerList as CustomerList WITH(NOLOCK)                                                 
INNER JOIN A606 WITH(NOLOCK)  ON (CustomerList.KUNNR = A606.KUNNR) AND                                                 
       (CustomerList.VKORG = A606.VKORG) AND                                 
 (CustomerList.VTWEG = A606.VTWEG) AND                                                 
       (CustomerList.SPART = A606.SPART))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A606.KNUMH = KONP.KNUMH                                                
WHERE (((A606.KAPPL)='V') AND     
  ((A606.KSCHL) in (select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where Status =1)) AND     
  ((A606.DATBI)>=[SelDate]) AND     
  ((A606.DATAB)<=[SelDate]) AND     
  ((KONP.LOEVM_KO)<>'X'));                                                
                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_PH 3-A606-Before executing')                                                
  set @Level = 3                                                
 EXECUTE sp_executesql @SQLDiscounts_PH,@SQLDiscounts_PH_ParmDefinition ,@Level = @Level                                                
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_PH 3-Alter executing')                                        
End                                                
                                                
Begin---====================load_D_A657=================================================================    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_A657-Load A657 discounts for Sales Area, Customer Hierarchy, Material, and condition type')                                                
                                                
set @Endno  = 1                                                
set @Startno  = 10                                                
set @SQL =''                                             
set @ParmDefinition =''                                                
                            
while(@Startno >= @Endno)                                                
 Begin                                                
  set @SQL ='insert into #Temp_Discounts ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, HIENR, KVGR1, KVGR2, KVGR3,                                                 
       PRODH1, PRODH2, PRODH3, PRODH4, PRODH5, PRODH6,MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                 
       select CustomerList.KUNNR as CustNo, ''A657-'+Convert(varchar(100),@Startno) +'''  as [Table],                                                
       A657.KAPPL, A657.KSCHL,CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                                                
       CustomerList.level'+Convert(varchar(100),@Startno) +' as HIENR,                                                
       '''' AS KVGR1, '''' as KVGR2, '''' as KVGR3, '''' as PRODH1, '''' as PRODH2, '''' as PRODH3, '''' as PRODH4,                                                 
       '''' as PRODH5, '''' as PRODH6, A657.MATNR, A657.KFRST,                                                
       A657.DATBI, A657.DATAB, A657.KBSTAT, A657.KNUMH, KONP.KSTBM, KONP.KBETR                                                 
       from                                                
       (#Temp_CustomerList as CustomerList WITH(NOLOCK)                                                
       inner join A657 WITH(NOLOCK)  on (CustomerList.VKORG = A657.VKORG) and                                                 
                          (CustomerList.VTWEG = A657.VTWEG) and                                                 
      (CustomerList.SPART = A657.SPART) and                                                 
           (CustomerList.Level'+Convert(varchar(100),@Startno) +' = A657.HIENR))                                                 
       inner join KONP WITH(NOLOCK)  on A657.KNUMH = KONP.KNUMH                                                 
       where ((((A657.KAPPL)=''V'') and ((A657.KSCHL) in (select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where Status =1)) and                                                 
       ((CustomerList.level'+Convert(varchar(100),@Startno) +') Is Not Null And                                                 
       (CustomerList.level'+Convert(varchar(100),@Startno) +')<>'''') and                                                 
       ((A657.DATBI)>=[SelDate]) and ((A657.DATAB)<=[SelDate]) and ((KONP.LOEVM_KO)<>''X'')) )        
    and not EXISTS(select 1 from #Temp_Discounts WITH(NOLOCK) where CustNo =CustomerList.KUNNR and MATNR =A657.MATNR and KSCHL = A657.KSCHL )  '                                                
   --select @SQL                                                
   EXECUTE sp_executesql @SQL                                 
   set @Startno = @Startno-1                                                
 End                                                
         
END                                                
                                                
                                                
Begin---====================load_D_A704================================================================    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_A704-Load A704 discounts for Sales Area, Customer Hierarchy, Product Hierarchy 1-6')   
                                               
  truncate Table #Temp_Discounts_PH                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','Clear Discounts_PH')                                     
                                                
set @Endno  = 1                                                
set @Startno  = 10                                               
set @SQL =''                                                
set @ParmDefinition =''                                                
while(@Startno >= @Endno)                                                
Begin                                                
set @SQL ='insert into #Temp_Discounts_PH ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, HIENR, KVGR1, KVGR2, KVGR3,    
   PRODH1, PRODH2, PRODH3,PRODH4, PRODH5, PRODH6, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                   
   select CustomerList.KUNNR as CustNo, ''A704-'+Convert(varchar(100),@Startno) +''' as [Table],                                                
   A704.KAPPL, A704.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                                                 
   CustomerList.level'+Convert(varchar(100),@Startno) +' as HIENR,                                                
   '''' AS KVGR1, '''' as KVGR2, '''' as KVGR3,                                               
   A704.PRODH1, A704.PRODH2, A704.PRODH3, A704.YPRODH4, '''' as YPRODH5, '''' as YPRODH6,                                                
   A704.KFRST, A704.DATBI, A704.DATAB, A704.KBSTAT, A704.KNUMH, KONP.KSTBM, KONP.KBETR                                                 
   from (                                                
   #Temp_CustomerList as CustomerList WITH(NOLOCK)                                                 
   inner join A704 WITH(NOLOCK)  on (CustomerList.Level'+Convert(varchar(100),@Startno) +' = A704.HIENR) and                                                 
          (CustomerList.SPART = A704.SPART) and (CustomerList.VTWEG = A704.VTWEG) and                                                 
                      (CustomerList.VKORG = A704.VKORG))                                                 
   inner join KONP WITH(NOLOCK)  on A704.KNUMH = KONP.KNUMH                                                 
where (((A704.KAPPL)=''V'') and                                                 
      ((A704.KSCHL) in (select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where Status =1)) and                                                 
      ((CustomerList.level'+Convert(varchar(100),@Startno) +') Is Not Null and                                                 
      (CustomerList.level'+Convert(varchar(100),@Startno) +')<>'''') and                                                 
      ((A704.DATBI)>=[SelDate]) and ((A704.DATAB)<=[SelDate]) and ((KONP.LOEVM_KO)<>''X''))     
   and not EXISTS(select 1 from #Temp_Discounts_PH WITH(NOLOCK) where CustNo =CustomerList.KUNNR and    
                KSCHL =A704.KSCHL and     
                PRODH1 =A704.PRODH1 and     
                PRODH2 = A704.PRODH2 and     
                PRODH3 = A704.PRODH3 and     
                PRODH4 = A704.YPRODH4 and     
                PRODH5 = '''' and     
                PRODH6 = '''' )'                                                
                            
 EXECUTE sp_executesql @SQL                                                
   set @Startno = @Startno-1                           
End                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_PH 6 -A704-Before executing')                          
  set @Level = 6                                                
 EXECUTE sp_executesql @SQLDiscounts_PH,@SQLDiscounts_PH_ParmDefinition ,@Level = @Level                                                
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_PH 6-Alter executing')                                                
                                                
End                         
                                                
Begin---====================load_D_A607================================================================    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_A607-Load A607 discounts for Sales Area, Customer Hierarchy, Product Hierarchy 1-3, a  
nd condition type ')                                                
  truncate Table #Temp_Discounts_PH                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','Clear Discounts_PH')                                                
set @Endno  = 1                                                
set @Startno  = 10                                               
set @SQL =''                                                
set @ParmDefinition =''                                                
while(@Startno >= @Endno)                                                
Begin                                                
set @SQL ='insert into #Temp_Discounts_PH ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, HIENR, KVGR1, KVGR2, KVGR3, PRODH1, PRODH2,                                     
          PRODH3, PRODH4, PRODH5, PRODH6, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
     select CustomerList.KUNNR as CustNo, ''A607-'+Convert(varchar(100),@Startno) +''' as [Table], A607.KAPPL, A607.KSCHL,                                                
     CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART, CustomerList.level'+Convert(varchar(100),@Startno) +' as HIENR,                                                
     '''' AS KVGR1, '''' as KVGR2, '''' as KVGR3, A607.PRODH1, A607.PRODH2,                                                
     A607.PRODH3, '''' as PRODH4, '''' as PRODH5, '''' as PRODH6, A607.KFRST, A607.DATBI, A607.DATAB,                                                
     A607.KBSTAT, A607.KNUMH, KONP.KSTBM, KONP.KBETR                                                 
     from (                                                
    #Temp_CustomerList as CustomerList WITH(NOLOCK)                                                 
     inner join A607 WITH(NOLOCK)  on (CustomerList.Level'+Convert(varchar(100),@Startno) +' = A607.HIENR) and                                                 
         (CustomerList.SPART = A607.SPART) and                                                 
         (CustomerList.VTWEG = A607.VTWEG) and                                                 
         (CustomerList.VKORG = A607.VKORG))                                                 
    inner join KONP WITH(NOLOCK)  on A607.KNUMH = KONP.KNUMH                                                 
    where (((A607.KAPPL)=''V'') and                                                 
          ((A607.KSCHL) in (select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where Status =1)) and                                                 
    ((CustomerList.level'+Convert(varchar(100),@Startno) +') Is Not Null and                                                 
    (CustomerList.level'+Convert(varchar(100),@Startno) +')<>'''') and ((A607.DATBI)>=[SelDate]) and                                                 
    ((A607.DATAB)<=[SelDate]) and ((KONP.LOEVM_KO)<>''X''))     
 and not EXISTS(select 1 from #Temp_Discounts_PH WITH(NOLOCK) where CustNo =CustomerList.KUNNR and     
                KSCHL =A607.KSCHL and     
                PRODH1 =A607.PRODH1 and     
                PRODH2 = A607.PRODH2 and     
                PRODH3 = A607.PRODH3 and     
                PRODH4 = '''' and     
                PRODH5 = '''' and     
                PRODH6 = '''' )'      
     
    
EXECUTE sp_executesql @SQL                                                
   set @Startno = @Startno-1                                                
End                                                
                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_PH 3 -A607-Before executing')                      
  set @Level = 3                                                
 EXECUTE sp_executesql @SQLDiscounts_PH,@SQLDiscounts_PH_ParmDefinition ,@Level = @Level                                                
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_PH 3 -Alter executing')                                                
                                    
                                                
End                                                
                                                
Begin---====================load_A653==================================================================    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_A653_PC123-Discounts for Sales Area, Price Class 1-3 and Material')                   
                               
                                                
                                                
INSERT INTO #Temp_Discounts ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, KVGR1, KVGR2, KVGR3, PRODH1, PRODH2, PRODH3, PRODH4, PRODH5,                                             
         PRODH6, MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS CustNo, 'A653' AS [Table], A653.KAPPL, A653.KSCHL, CustomerList.VKORG,                                                
CustomerList.VTWEG, CustomerList.SPART, A653.ZKVGR1 AS KVGR1, A653.YYKVGR2 AS KVGR2, A653.YYKVGR3 AS KVGR3,                                                
'' AS PRODH1, '' AS PRODH2, '' AS PRODH3, '' AS PRODH4, '' AS PRODH5, '' AS PRODH6,                                                
A653.MATNR, A653.KFRST, A653.DATBI, A653.DATAB, A653.KBSTAT, A653.KNUMH, KONP.KSTBM, KONP.KBETR                   
FROM (#Temp_CustomerList as CustomerList WITH(NOLOCK)                                                 
INNER JOIN A653 WITH(NOLOCK)  ON (CustomerList.KVGR3 = A653.YYKVGR3) AND                                                 
       (CustomerList.KVGR2 = A653.YYKVGR2) AND                                                 
       (CustomerList.KVGR1 = A653.ZKVGR1) AND                                                 
       (CustomerList.SPART = A653.SPART) AND                                
       (CustomerList.VTWEG = A653.VTWEG) AND                                                 
       (CustomerList.VKORG = A653.VKORG))                                                
INNER JOIN KONP WITH(NOLOCK)  ON A653.KNUMH = KONP.KNUMH                                             
WHERE ((((A653.KAPPL)='V') AND                                                 
  ((A653.KSCHL) in (select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where Status =1)) AND                                                 
  ((A653.DATBI)>=[SelDate]) AND                                       
  ((A653.DATAB)<=[SelDate]) AND     
  ((KONP.LOEVM_KO)<>'X')))        
  and not EXISTS(select 1 from #Temp_Discounts WITH(NOLOCK) where CustNo =CustomerList.KUNNR and MATNR =A653.MATNR and KSCHL = A653.KSCHL )         
                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_A653_PC12X-Discounts for Sales Area, Price Class 1-2 and Material')                    
                              
                                                
                                                
INSERT INTO #Temp_Discounts ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, KVGR1, KVGR2, KVGR3, PRODH1, PRODH2, PRODH3, PRODH4, PRODH5,                                                
         PRODH6, MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS CustNo, 'A653' AS [Table], A653.KAPPL, A653.KSCHL, CustomerList.VKORG,                                        
CustomerList.VTWEG, CustomerList.SPART, A653.ZKVGR1 AS KVGR1, A653.YYKVGR2 AS KVGR2, A653.YYKVGR3 AS KVGR3,                                                 
'' AS PRODH1, '' AS PRODH2, '' AS PRODH3, '' AS PRODH4, '' AS PRODH5, '' AS PRODH6,                                                 
A653.MATNR, A653.KFRST, A653.DATBI, A653.DATAB, A653.KBSTAT, A653.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM (#Temp_CustomerList as CustomerList WITH(NOLOCK)                                                 
INNER JOIN A653 WITH(NOLOCK)  ON (CustomerList.VKORG = A653.VKORG) AND           
       (CustomerList.VTWEG = A653.VTWEG) AND                                                 
       (CustomerList.SPART = A653.SPART) AND                                                 
       (CustomerList.KVGR1 = A653.ZKVGR1) AND                                                 
       (CustomerList.KVGR2 = A653.YYKVGR2))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A653.KNUMH = KONP.KNUMH                                                
WHERE ((((A653.KAPPL)='V') AND                                                 
   ((A653.KSCHL) in (select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where Status =1)) AND                                                 
   ((A653.YYKVGR3) Is Null Or (A653.YYKVGR3)='') AND                                                 
   ((A653.DATBI)>=[SelDate]) AND                                                 
   ((A653.DATAB)<=[SelDate]) AND     
   ((KONP.LOEVM_KO)<>'X')) )        
   and not EXISTS(select 1 from #Temp_Discounts WITH(NOLOCK) where CustNo =CustomerList.KUNNR and MATNR =A653.MATNR and KSCHL = A653.KSCHL )         
                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_A653_PC1XX-Discounts for Sales Area, Price Class 1 and Material')                      
                            
                                                
                                                
INSERT INTO #Temp_Discounts ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, KVGR1, KVGR2, KVGR3, PRODH1, PRODH2, PRODH3, PRODH4, PRODH5,                                                
         PRODH6, MATNR, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS CustNo, 'A653' AS [Table], A653.KAPPL, A653.KSCHL, CustomerList.VKORG,                                                
CustomerList.VTWEG, CustomerList.SPART, A653.ZKVGR1 AS KVGR1, A653.YYKVGR2 AS KVGR2, A653.YYKVGR3 AS KVGR3,                                                
'' AS PRODH1, '' AS PRODH2, '' AS PRODH3, '' AS PRODH4, '' AS PRODH5, '' AS PRODH6,                                                
A653.MATNR, A653.KFRST, A653.DATBI, A653.DATAB, A653.KBSTAT, A653.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM (#Temp_CustomerList as CustomerList WITH(NOLOCK) INNER JOIN                                                 
A653 WITH(NOLOCK)  ON (CustomerList.VKORG = A653.VKORG) AND                                                 
  (CustomerList.VTWEG = A653.VTWEG) AND                                                 
  (CustomerList.SPART = A653.SPART) AND                                                 
  (CustomerList.KVGR1 = A653.ZKVGR1))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A653.KNUMH = KONP.KNUMH                                                
WHERE ((((A653.KAPPL)='V') AND                                                 
  ((A653.KSCHL) in (select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where Status =1)) AND                                                 
  ((A653.YYKVGR2) Is Null Or (A653.YYKVGR2)='') AND                                                 
  ((A653.YYKVGR3) Is Null Or (A653.YYKVGR3)='') AND                                                 
  ((A653.DATBI)>=[SelDate]) AND                                               
  ((A653.DATAB)<=[SelDate]) AND                                                 
  ((KONP.LOEVM_KO)<>'X')))        
  and not EXISTS(select 1 from #Temp_Discounts WITH(NOLOCK) where CustNo =CustomerList.KUNNR and MATNR =A653.MATNR and KSCHL = A653.KSCHL )         
                                                
                                                
                                                
                                           
End                                                
                                                
Begin---====================load_D_A705===============================================================    
                                                
 truncate Table #Temp_Discounts_PH                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','Clear Discounts_PH')                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_A705_PH_PC123-Discounts for Sales Area, Price Class 1-3')                             
                     
INSERT INTO #Temp_Discounts_PH ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, HIENR, KVGR1, KVGR2, KVGR3,                                               
PRODH1, PRODH2, PRODH3, PRODH4, PRODH5, PRODH6, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS CustNo, 'A705' AS [Table], A705.KAPPL, A705.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                                                
'' AS HIENR, A705.ZKVGR1 AS KVGR1, A705.YYKVGR2 AS KVGR2, A705.YYKVGR3 AS KVGR3, A705.PRODH1, A705.PRODH2, A705.PRODH3,                            
A705.YPRODH4 AS PRODH4, A705.YPRODH5 AS PRODH5, A705.YPRODH6 AS PRODH6, '' AS KFRST, A705.DATBI, A705.DATAB,                                                
'' AS KBSTAT, A705.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM (#Temp_CustomerList as CustomerList WITH(NOLOCK)                                             
INNER JOIN A705 WITH(NOLOCK)  ON (CustomerList.KVGR1 = A705.ZKVGR1) AND                
     (CustomerList.KVGR2 = A705.YYKVGR2) AND                                                 
     (CustomerList.KVGR3 = A705.YYKVGR3) AND                              
     (CustomerList.SPART = A705.SPART) AND                                                 
     (CustomerList.VTWEG = A705.VTWEG) AND                                                 
     (CustomerList.VKORG = A705.VKORG))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A705.KNUMH = KONP.KNUMH                                                
WHERE (((A705.KAPPL)='V') AND                                                 
      ((A705.KSCHL) in (select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where Status =1)) AND                                                 
   ((A705.DATBI)>=[SelDate]) AND                                                 
     ((A705.DATAB)<=[SelDate]) AND                                                 
     ((KONP.LOEVM_KO)<>'X'))     
  and not EXISTS(select 1 from #Temp_Discounts_PH WITH(NOLOCK) where CustNo =CustomerList.KUNNR and     
                KSCHL =A705.KSCHL and     
                PRODH1 =A705.PRODH1 and     
                PRODH2 = A705.PRODH2 and     
                PRODH3 = A705.PRODH3 and     
                PRODH4 = A705.YPRODH4 and     
                PRODH5 = A705.YPRODH5 and     
                PRODH6 = A705.YPRODH6 )                                                
                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_A705_PH_PC12X-Discounts for Sales Area, Price Class 1-2')                              
                    
                                                
                                                
INSERT INTO #Temp_Discounts_PH ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, HIENR, KVGR1, KVGR2, KVGR3,                                              
PRODH1, PRODH2, PRODH3, PRODH4, PRODH5, PRODH6, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS CustNo, 'A705' AS [Table], A705.KAPPL, A705.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                      
'' AS HIENR, A705.ZKVGR1 AS KVGR1, A705.YYKVGR2 AS KVGR2, A705.YYKVGR3 AS KVGR3, A705.PRODH1, A705.PRODH2, A705.PRODH3,                                                
A705.YPRODH4 AS PRODH4, A705.YPRODH5 AS PRODH5, A705.YPRODH6 AS PRODH6, '' AS KFRST, A705.DATBI, A705.DATAB,                                                
'' AS KBSTAT, A705.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM (#Temp_CustomerList as CustomerList WITH(NOLOCK)                                                 
INNER JOIN A705 WITH(NOLOCK)  ON (CustomerList.KVGR1 = A705.ZKVGR1) AND                                                 
       (CustomerList.KVGR2 = A705.YYKVGR2) AND                                                 
       (CustomerList.SPART = A705.SPART) AND                                                 
       (CustomerList.VTWEG = A705.VTWEG) AND                                                 
       (CustomerList.VKORG = A705.VKORG))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A705.KNUMH = KONP.KNUMH                                                
WHERE (((A705.KAPPL)='V') AND                                                 
    ((A705.KSCHL) in (select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where Status =1)) AND     
 ((A705.DATBI)>=[SelDate]) AND                                                 
    ((A705.DATAB)<=[SelDate]) AND    
 ((KONP.LOEVM_KO)<>'X')) AND    
  ((A705.YYKVGR3) Is Null Or (A705.YYKVGR3)='')           
 and not EXISTS(select 1 from #Temp_Discounts_PH WITH(NOLOCK) where CustNo =CustomerList.KUNNR and     
                KSCHL =A705.KSCHL and      
                PRODH1 =A705.PRODH1 and     
                PRODH2 = A705.PRODH2 and     
                PRODH3 = A705.PRODH3 and     
                PRODH4 = A705.YPRODH4 and     
                PRODH5 = A705.YPRODH5 and     
                PRODH6 = A705.YPRODH6 )                                               
                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_A705_PH_PC1XX-Discounts for Sales Area, Price Class 1')                                
                  
                                                
                                                
INSERT INTO #Temp_Discounts_PH ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, HIENR, KVGR1, KVGR2, KVGR3,                                               
PRODH1, PRODH2, PRODH3, PRODH4, PRODH5, PRODH6, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS CustNo, 'A705' AS [Table], A705.KAPPL, A705.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART, '' AS HIENR,                                              
A705.ZKVGR1 AS KVGR1, A705.YYKVGR2 AS KVGR2, A705.YYKVGR3 AS KVGR3, A705.PRODH1, A705.PRODH2,                                              
A705.PRODH3, A705.YPRODH4 AS PRODH4, A705.YPRODH5 AS PRODH5, A705.YPRODH6 AS PRODH6, '' AS KFRST, A705.DATBI,                                  
A705.DATAB, '' AS KBSTAT, A705.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM (#Temp_CustomerList as CustomerList WITH(NOLOCK)                                                  
INNER JOIN A705 WITH(NOLOCK)  ON (CustomerList.VKORG = A705.VKORG) AND                                                 
       (CustomerList.VTWEG = A705.VTWEG) AND                                                 
       (CustomerList.SPART = A705.SPART) AND                                                 
       (CustomerList.KVGR1 = A705.ZKVGR1))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A705.KNUMH = KONP.KNUMH                                                
WHERE (((A705.KAPPL)='V') AND                                                 
       ((A705.KSCHL) in (select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where Status =1)) AND                                                                                            
      ((A705.DATBI)>=[SelDate]) AND                                                 
      ((A705.DATAB)<=[SelDate]) AND                                                 
      ((KONP.LOEVM_KO)<>'X')) AND    
   ((A705.YYKVGR2) Is Null Or (A705.YYKVGR2)='') AND                                                 
      ((A705.YYKVGR3) Is Null Or (A705.YYKVGR3)='')          
   and not EXISTS(select 1 from #Temp_Discounts_PH WITH(NOLOCK) where CustNo =CustomerList.KUNNR and     
                KSCHL =A705.KSCHL and      
                PRODH1 =A705.PRODH1 and     
                PRODH2 = A705.PRODH2 and     
                PRODH3 = A705.PRODH3 and     
                PRODH4 = A705.YPRODH4 and     
                PRODH5 = A705.YPRODH5 and     
                PRODH6 = A705.YPRODH6 )    
                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_PH 6-A705-Before executing: Load Discounts table using product hierarchy 1 to 6')      
                                            
  set @Level = 6                                                
 EXECUTE sp_executesql @SQLDiscounts_PH,@SQLDiscounts_PH_ParmDefinition ,@Level = @Level                                                
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_PH 6-Alter executing')                                                
                                                
                                       
End                                                
                  
Begin---====================load_D_A608_PH============================================================    
truncate Table #Temp_Discounts_PH                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','Clear Discounts_PH')                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_A608_PH_PC123-Discounts for Sales Area, Price Class 1-3')                              
                    
                                                
                                                
INSERT INTO #Temp_Discounts_PH ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, HIENR, KVGR1, KVGR2, KVGR3,                                               
PRODH1, PRODH2, PRODH3, PRODH4, PRODH5, PRODH6, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                               
SELECT CustomerList.KUNNR AS CustNo, 'A608' AS [Table], A608.KAPPL, A608.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                                                
'' AS HIENR, A608.ZKVGR1 AS KVGR1, A608.YYKVGR2 AS KVGR2, A608.YYKVGR3 AS KVGR3, A608.PRODH1, A608.PRODH2, A608.PRODH3,                                                
'' AS PRODH4, '' AS PRODH5, '' AS PRODH6, A608.KFRST, A608.DATBI, A608.DATAB, '' AS KBSTAT, A608.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM (#Temp_CustomerList as CustomerList WITH(NOLOCK)             
INNER JOIN A608 WITH(NOLOCK)  ON (CustomerList.VKORG = A608.VKORG) AND                               
     (CustomerList.VTWEG = A608.VTWEG) AND                                                
     (CustomerList.SPART = A608.SPART) AND                                                 
     (CustomerList.KVGR3 = A608.YYKVGR3) AND                                                 
     (CustomerList.KVGR2 = A608.YYKVGR2) AND                                                 
     (CustomerList.KVGR1 = A608.ZKVGR1))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A608.KNUMH = KONP.KNUMH                                                
WHERE (((A608.KAPPL)='V') AND                                                 
       ((A608.KSCHL) in (select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where Status =1)) AND                                                 
       ((A608.DATBI)>=[SelDate]) AND                                                 
       ((A608.DATAB)<=[SelDate]) AND     
    ((KONP.LOEVM_KO)<>'X'))    
    and not EXISTS(select 1 from #Temp_Discounts_PH WITH(NOLOCK) where CustNo =CustomerList.KUNNR and      
                KSCHL =A608.KSCHL and     
                PRODH1 =A608.PRODH1 and     
                PRODH2 = A608.PRODH2 and     
                PRODH3 = A608.PRODH3 and     
                PRODH4 = '' and     
                PRODH5 = '' and     
                PRODH6 = '' )    
                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_A608_PH_PC12X-Discounts for Sales Area, Price Class 1-2')                             
                     
                                                
INSERT INTO #Temp_Discounts_PH ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, HIENR, KVGR1, KVGR2, KVGR3,                                              
PRODH1, PRODH2, PRODH3, PRODH4, PRODH5, PRODH6, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS CustNo, 'A608' AS [Table], A608.KAPPL, A608.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                                                
'' AS HIENR, A608.ZKVGR1 AS KVGR1, A608.YYKVGR2 AS KVGR2, A608.YYKVGR3 AS KVGR3, A608.PRODH1, A608.PRODH2, A608.PRODH3,                                                
'' AS PRODH4, '' AS PRODH5, '' AS PRODH6, A608.KFRST, A608.DATBI, A608.DATAB, '' AS KBSTAT, A608.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM (#Temp_CustomerList as CustomerList WITH(NOLOCK)                                                 
INNER JOIN A608 WITH(NOLOCK)  ON (CustomerList.KVGR1 = A608.ZKVGR1) AND                                                 
       (CustomerList.KVGR2 = A608.YYKVGR2) AND                                                 
       (CustomerList.SPART = A608.SPART) AND                                                 
       (CustomerList.VTWEG = A608.VTWEG) AND                                                 
       (CustomerList.VKORG = A608.VKORG))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A608.KNUMH = KONP.KNUMH                                                
WHERE (((A608.KAPPL)='V') AND                                                 
    ((A608.KSCHL) in (select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where Status =1)) AND                                                 
    ((A608.YYKVGR3) Is Null Or (A608.YYKVGR3)='') AND                                                 
    ((A608.DATBI)>=[SelDate]) AND                   
    ((A608.DATAB)<=[SelDate]) AND                                                 
    ((KONP.LOEVM_KO)<>'X')) and not EXISTS(select 1 from #Temp_Discounts_PH WITH(NOLOCK) where CustNo =CustomerList.KUNNR and      
                KSCHL =A608.KSCHL and      
                PRODH1 =A608.PRODH1 and     
                PRODH2 = A608.PRODH2 and     
                PRODH3 = A608.PRODH3 and     
                PRODH4 = '' and     
                PRODH5 = '' and     
                PRODH6 = '' )                                     
                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_A608_PH_PC1XX-Discounts for Sales Area, Price Class 1')                               
                   
                                                
                                                
INSERT INTO #Temp_Discounts_PH ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, HIENR, KVGR1, KVGR2, KVGR3,                                              
PRODH1, PRODH2, PRODH3, PRODH4, PRODH5, PRODH6, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                   
SELECT CustomerList.KUNNR AS CustNo, 'A608' AS [Table], A608.KAPPL, A608.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART, '' AS HIENR,                                               
A608.ZKVGR1 AS KVGR1, A608.YYKVGR2 AS KVGR2, A608.YYKVGR3 AS KVGR3,                                             
A608.PRODH1, A608.PRODH2, A608.PRODH3, '' AS PRODH4, '' AS PRODH5, '' AS PRODH6, A608.KFRST, A608.DATBI, A608.DATAB, '' AS KBSTAT, A608.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM (#Temp_CustomerList as CustomerList WITH(NOLOCK)                                                  
INNER JOIN A608 WITH(NOLOCK)  ON (CustomerList.VKORG = A608.VKORG) AND                                                 
                   (CustomerList.VTWEG = A608.VTWEG) AND                                                 
       (CustomerList.SPART = A608.SPART) AND                                                 
       (CustomerList.KVGR1 = A608.ZKVGR1))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A608.KNUMH = KONP.KNUMH                                                
WHERE (((A608.KAPPL)='V') AND ((A608.KSCHL) in (select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where Status =1)) AND                                                 
  ((A608.YYKVGR2) Is Null Or (A608.YYKVGR2)='') AND                                                 
  ((A608.YYKVGR3) Is Null Or (A608.YYKVGR3)='') AND                 
  ((A608.DATBI)>=[SelDate]) AND                                                 
  ((A608.DATAB)<=[SelDate]) AND                                   
  ((KONP.LOEVM_KO)<>'X'))     
  and not EXISTS(select 1 from #Temp_Discounts_PH WITH(NOLOCK) where CustNo =CustomerList.KUNNR and       
                KSCHL =A608.KSCHL and     
                PRODH1 =A608.PRODH1 and     
                PRODH2 = A608.PRODH2 and     
                PRODH3 = A608.PRODH3 and     
                PRODH4 = '' and     
                PRODH5 = '' and     
                PRODH6 = '' )                                              
                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_PH 3-A608-Before executing: Load Discounts table using product hierarchy 1 to 3')      
                              
  set @Level = 3                                                
 EXECUTE sp_executesql @SQLDiscounts_PH,@SQLDiscounts_PH_ParmDefinition ,@Level = @Level                                  
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_PH 3-Alter executing')                                          
      
        
          
                                                
                                                
                                                
                                                
End                                                
                                                
Begin---====================load_D_A609_PH===========================================================    
truncate Table #Temp_Discounts_PH                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','Clear Discounts_PH')                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_A609_PH-Discounts for Sales Area')                                                
                                                
                                                
INSERT INTO #Temp_Discounts_PH ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, KVGR1, KVGR2, KVGR3,                                               
PRODH1, PRODH2, PRODH3, PRODH4, PRODH5, PRODH6, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS CustNo, 'A609' AS [Table], A609.KAPPL, A609.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                                                
'' AS KVGR1, '' AS KVGR2, '' AS KVGR3, A609.PRODH1, A609.PRODH2, A609.PRODH3, '' AS PRODH4, '' AS PRODH5, '' AS PRODH6,                                                
A609.KFRST, A609.DATBI, A609.DATAB, A609.KBSTAT, A609.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM (#Temp_CustomerList as CustomerList  WITH(NOLOCK)                                
INNER JOIN A609 WITH(NOLOCK)  ON (CustomerList.VKORG = A609.VKORG) AND                                                 
       (CustomerList.VTWEG = A609.VTWEG) AND                                                 
       (CustomerList.SPART = A609.SPART))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A609.KNUMH = KONP.KNUMH                                                
WHERE (((A609.KAPPL)='V') AND     
  ((A609.KSCHL) in (select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where Status =1)) AND                                                 
  ((A609.DATBI)>=[SelDate]) AND                                                 
  ((A609.DATAB)<=[SelDate]) AND                 
  ((KONP.LOEVM_KO)<>'X'));                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_PH 3-A609-Before executing: Load Discounts table using product hierarchy 1 to 3')      
                                            
  set @Level = 3                                                
 EXECUTE sp_executesql @SQLDiscounts_PH,@SQLDiscounts_PH_ParmDefinition ,@Level = @Level                                                
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_PH 3-Alter executing')                                                
                                                
                                             
End                                                
                                                
Begin---====================load_D_A979_PH===========================================================    
truncate Table #Temp_Discounts_PH                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','Clear Discounts_PH')                                        
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_A979_PH-Discounts for Sales Area and Price Class 3')                                   
               
                                                
                                                
                         
INSERT INTO #Temp_Discounts_PH ( CustNo, [Table], KAPPL, KSCHL, VKORG, VTWEG, SPART, HIENR, KVGR1, KVGR2, KVGR3,        
PRODH1, PRODH2, PRODH3, PRODH4, PRODH5, PRODH6, KFRST, DATBI, DATAB, KBSTAT, KNUMH, KSTBM, KBETR )                                                
SELECT CustomerList.KUNNR AS CustNo, 'A979' AS [Table], A979.KAPPL, A979.KSCHL, CustomerList.VKORG, CustomerList.VTWEG, CustomerList.SPART,                                                
'' AS HIENR, '' AS KVGR1, '' AS KVGR2, A979.YYKVGR3 AS KVGR3, A979.PRODH1, A979.PRODH2, A979.PRODH3,                                                
'' AS PRODH4, '' AS PRODH5, '' AS PRODH6, A979.KFRST, A979.DATBI, A979.DATAB, A979.KBSTAT, A979.KNUMH, KONP.KSTBM, KONP.KBETR                                                
FROM (#Temp_CustomerList as CustomerList WITH(NOLOCK)                                        
INNER JOIN A979 WITH(NOLOCK)  ON (CustomerList.VKORG = A979.VKORG) AND                                                 
       (CustomerList.VTWEG = A979.VTWEG) AND                                                 
       (CustomerList.SPART = A979.SPART) AND                                                 
       (CustomerList.KVGR3 = A979.YYKVGR3))                                                 
INNER JOIN KONP WITH(NOLOCK)  ON A979.KNUMH = KONP.KNUMH                                                
WHERE (((A979.KAPPL)='V') AND                                                 
       ((A979.KSCHL) in (select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where Status =1)) AND                                                 
    ((A979.DATBI)>=[SelDate]) AND                                                 
    ((A979.DATAB)<=[SelDate]) AND                                                 
    ((KONP.LOEVM_KO)<>'X'));                                                
                                                
                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_PH 3-A979-Before executing: Load Discounts table using product hierarchy 1 to 3')     
                                             
  set @Level = 3                                        
 EXECUTE sp_executesql @SQLDiscounts_PH,@SQLDiscounts_PH_ParmDefinition ,@Level = @Level                                                
                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','load_D_PH 3-Alter executing')                                                
                                                
                          
                                                
End      
    
    
    
 --select @ConditionType    
    --set @DiscountStartno = @DiscountStartno+1       
end    
    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts','Completed')     
    
End    
    
update dbo.TRN_PriceFileHeader set [StatusText] ='Loading Discount Breaks' ,     
           [PercentCompleted] =45    
           where PriceFileHeaderID =@PriceFileHeaderID    
    
Begin---========================Discounts_brk========================================================    
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts_brk','Start')     
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts_brk','Load discount breaks, converting SAP values to actual discounts (ie SAP/10)')             
                                     
Begin---====================load_Discounts_brk================================================================    
INSERT INTO #Temp_Discounts_brk ( CustNo, MATNR, DiscTable, KSCHL, Qty, Disc, KSTBM1, KSTBM2, KSTBM3, KSTBM4, KSTBM5, KSTBM6,    
KBETR1, KBETR2, KBETR3, KBETR4, KBETR5, KBETR6 )                    
SELECT Prices.CustNo, Prices.MATNR, Discounts.[table] AS DiscTable, Discounts.KSCHL, Discounts.KSTBM AS Qty, [Discounts].[KBETR]/10 AS Disc,                                                
isnull(KONM_sum.KSTBM1,Discounts.KSTBM) as KSTBM1, KONM_sum.KSTBM2, KONM_sum.KSTBM3, KONM_sum.KSTBM4, KONM_sum.KSTBM5, KONM_sum.KSTBM6,    
case when KONM_sum.KSTBM1 is null then [Discounts].[KBETR]/10 else [KONM_sum].[KBETR1]/10 end AS DISCP1,                                                
[KONM_sum].[KBETR2]/10 AS DISCP2, [KONM_sum].[KBETR3]/10 AS DISCP3, [KONM_sum].[KBETR4]/10 AS DISCP4, [KONM_sum].[KBETR5]/10 AS DISCP5,    
[KONM_sum].[KBETR6]/10 AS DISCP6                                                
FROM (                                                
#Temp_Prices  as Prices WITH(NOLOCK)                                                
LEFT JOIN #Temp_Discounts as Discounts WITH(NOLOCK) ON (Prices.MATNR = Discounts.MATNR) AND                                                 
            (Prices.CustNo = Discounts.CustNo))                                                 
LEFT JOIN KONM_sum WITH(NOLOCK)  ON Discounts.KNUMH = KONM_sum.KNUMH                                             
WHERE (((Discounts.KSCHL) Is Not Null));       
    
END       
    
--Begin---====================load_Discounts_brk================================================================    
--Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation) Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts_brk','upd_Discounts_brk_1-If no discount breaks, use the unit discount (from KONP)')       
   
                                        
--UPDATE Discounts_brk  SET Discounts_brk.KSTBM1 = [Qty], Discounts_brk.KBETR1 = [Disc]                                                
--from #Temp_Discounts_brk as Discounts_brk                                        
--WHERE (((Discounts_brk.KSTBM1) Is Null));                                                
               
                                                
--End                                                
                                                
Begin---============================@CanUseShiftBreaks========================================================    
if(@CanUseShiftBreaks =1 )                                                
begin                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts_brk','@CanUseShiftBreaks =1 ')                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts_brk','upd_Discounts_brk_2(need Clarification)-If only 1 discount break and discount% is 0, del  
ete entry')                                                
--If only 1 discount break and discount% is 0, delete entry                                                
--DELETE Discounts_brk.KSTBM2, Discounts_brk.KBETR1                                                
--FROM Discounts_brk                                                
--WHERE (((Discounts_brk.KSTBM2) Is Null) AND ((Discounts_brk.KBETR1) Is Null Or (Discounts_brk.KBETR1)=0));                                                
                                                
DELETE FROM #Temp_Discounts_brk           
WHERE (((#Temp_Discounts_brk.KSTBM2) Is Null) AND ((#Temp_Discounts_brk.KBETR1) Is Null Or (#Temp_Discounts_brk.KBETR1)=0));                                                
                                                
                                                
end                                                
End                                                
     
    
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Discounts_brk','Completed')      
                                                
End    
    
update dbo.TRN_PriceFileHeader set [StatusText] ='Loading Price Breaks' ,     
           [PercentCompleted] =50     
           where PriceFileHeaderID =@PriceFileHeaderID    
    
Begin---========================Prices_brk========================================================    
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','load_Prices_brk','Start')     
 --Clear "Prices_brk_ZLOG_After"    
  Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','load_Prices_brk','load_Prices_brk')                                                
                                      
 begin                                                
 INSERT INTO #Temp_Prices_brk ( CustNo, MATNR, VKORG, VTWEG, KSTBM1, KSTBM2, KSTBM3, KSTBM4, KSTBM5, KSTBM6,                                                
 KBETR1, KBETR2, KBETR3, KBETR4, KBETR5, KBETR6, PriceTable, CondType )                                                
SELECT Prices.CustNo, Prices.MATNR, Prices.VKORG, Prices.VTWEG,                                                 
case when [KONM_sum].KNUMH is null then [KSTBM] else [KONM_sum].KSTBM1 end  as KSTBM1,                                                
--IIf(IsNull([KONM_sum].[KNUMH]),[KSTBM],[KONM_sum].[KSTBM1]) AS KSTBM1,                                          
KONM_sum.KSTBM2, KONM_sum.KSTBM3, KONM_sum.KSTBM4, KONM_sum.KSTBM5, KONM_sum.KSTBM6,                                                
case when [KONM_sum].KNUMH is null then [KBETR] else [KONM_sum].[KBETR1] end  as KBETR1,                                                
--IIf(IsNull([KONM_sum].[KNUMH]),[KBETR],[KONM_sum].[KBETR1]) AS KBETR1,                                                 
KONM_sum.KBETR2, KONM_sum.KBETR3, KONM_sum.KBETR4, KONM_sum.KBETR5, KONM_sum.KBETR6, Prices.[Table], Prices.KSCHL                                                
FROM #Temp_Prices Prices WITH(NOLOCK)     
LEFT JOIN KONM_sum WITH(NOLOCK)  ON Prices.KNUMH = KONM_sum.KNUMH;                                                
                                                
 End     
  Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','load_Prices_brk','Completed')     
End    
    
update dbo.TRN_PriceFileHeader set [StatusText] ='Loading Quantity Breaks' ,     
           [PercentCompleted] =60     
           where PriceFileHeaderID =@PriceFileHeaderID    
    
Begin---========================load_Qty_Brks========================================================    
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','load_Qty_Brks','Start')     
  Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
  Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','load_Qty_Brks','Determine break quantities to be used in report')    
  insert into #Temp_Qty_Brks_tmp ( CustNo, MATNR, QtyBrk )    
  (select CustNo, MATNR, KSTBM1  from #Temp_Prices_brk WITH(NOLOCK)  where isnull(KBETR1,0) <>0    
  Union    
  select CustNo, MATNR, KSTBM2  from #Temp_Prices_brk WITH(NOLOCK)  where isnull(KBETR2,0) <>0    
  Union    
  select CustNo, MATNR, KSTBM3  from #Temp_Prices_brk WITH(NOLOCK)  where isnull(KBETR3,0) <>0    
  Union    
  select CustNo, MATNR, KSTBM4  from #Temp_Prices_brk WITH(NOLOCK)  where isnull(KBETR4,0) <>0    
  Union    
  select CustNo, MATNR, KSTBM5  from #Temp_Prices_brk WITH(NOLOCK)  where isnull(KBETR5,0) <>0    
  Union    
  select CustNo, MATNR, KSTBM6  from #Temp_Prices_brk WITH(NOLOCK)  where isnull(KBETR6,0) <>0    
  Union    
  select CustNo, MATNR, isnull(KSTBM1,0)  from #Temp_Discounts_brk WITH(NOLOCK)  --where isnull(KBETR1,0) <>0    
  Union    
  select CustNo, MATNR, isnull(KSTBM2,0)  from #Temp_Discounts_brk WITH(NOLOCK)  --where isnull(KBETR2,0) <>0    
  Union    
  select CustNo, MATNR, isnull(KSTBM3,0)  from #Temp_Discounts_brk WITH(NOLOCK)  --where isnull(KBETR3,0) <>0    
  Union    
  select CustNo, MATNR, isnull(KSTBM4,0)  from #Temp_Discounts_brk WITH(NOLOCK)  --where isnull(KBETR4,0) <>0    
  Union    
  select CustNo, MATNR, isnull(KSTBM5,0)  from #Temp_Discounts_brk WITH(NOLOCK)  --where isnull(KBETR5,0) <>0    
  Union    
  select CustNo, MATNR, isnull(KSTBM6,0)  from #Temp_Discounts_brk WITH(NOLOCK)  --where isnull(KBETR6,0) <>0
  )    
    
  select  CustNo,MATNR,QtyBrk,ROW_NUMBER() OVER(Partition by CustNo,MATNR order by QtyBrk) as RowNo
       into #Temp_Qty_Brks_tmp_RowNumber  
     from #Temp_Qty_Brks_tmp where  QtyBrk > 0  
    
  insert into #Temp_Qty_Brks(CustNo,MATNR,QtyBrk1,QtyBrk2,QtyBrk3,QtyBrk4,QtyBrk5,QtyBrk6)    
   select  CustNo, MATNR,QtyBrk,0,0,0,0,0  from #Temp_Qty_Brks_tmp_RowNumber  WITH(NOLOCK) where RowNo = 1    
    
   update T set QtyBrk2 = QtyBrk from  #Temp_Qty_Brks T      
   inner join #Temp_Qty_Brks_tmp_RowNumber TR  WITH(NOLOCK) on T.CustNo = TR.CustNo and TR.MATNR = T.MATNR    
   where RowNo = 2    
    
    update T set QtyBrk3 = QtyBrk from  #Temp_Qty_Brks T     
   inner join #Temp_Qty_Brks_tmp_RowNumber TR  WITH(NOLOCK) on T.CustNo = TR.CustNo and TR.MATNR = T.MATNR    
   where RowNo = 3    
    
   update T set QtyBrk4 = QtyBrk from  #Temp_Qty_Brks T     
   inner join #Temp_Qty_Brks_tmp_RowNumber TR  WITH(NOLOCK) on T.CustNo = TR.CustNo and TR.MATNR = T.MATNR    
   where RowNo = 4    
    
   update T set QtyBrk5 = QtyBrk from  #Temp_Qty_Brks T     
   inner join #Temp_Qty_Brks_tmp_RowNumber TR  WITH(NOLOCK) on T.CustNo = TR.CustNo and TR.MATNR = T.MATNR    
   where RowNo = 5    
    
     update T set QtyBrk6 = QtyBrk from  #Temp_Qty_Brks T     
   inner join #Temp_Qty_Brks_tmp_RowNumber TR  WITH(NOLOCK) on T.CustNo = TR.CustNo and TR.MATNR = T.MATNR    
   where RowNo = 6    
    
       
    
 Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
 Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','load_Qty_Brks','Completed')     
End    
    
update dbo.TRN_PriceFileHeader set [StatusText] ='Loading Customer Prices' ,     
           [PercentCompleted] =70     
           where PriceFileHeaderID =@PriceFileHeaderID    
    
Begin---========================load_Cust_Prices===========================================================     
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','load_Cust_Prices','Start')     
    
begin---==========================================Load prefix, Cat_No, col, Item_No & SplitPackQty===                                                
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','load_MaterialList_codes from MST_MaterialMaster ','Load prefix, cat no, col, item no, and split pack qua  
ntity from trade price template')                                                
                                                
 UPDATE ML                                                
   SET                                                 
   ML.Prefix = MML.[Prefix],                                                
   ML.Cat_No = MML.[CatNo],                                                 
   ML.Colour_Code = MML.[ColourCode],                                                
   ML.Item_No = MML.[ItemNo],                                                
   ML.Split_Pack_Qty = MML.[SplitPackQty]                                                
   from                                                
  #Temp_MaterialList ML  WITH(NOLOCK)                                               
           INNER JOIN [MST_MaterialMaster] MML WITH(NOLOCK)                                                 
   ON ML.MATNR = MML.[InternalSAPItemNo]                                                
                                                
End     
    
    
Begin---========================#Temp_RRP====================================================================                                        
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','RRR','')                                        
SELECT Distinct                                                
     D.[LCOS1To4],D.[Collection],D.[SubCollection],D.[DiscountGroup],D.[RRPMarkup]                                            
  into #Temp_RRP                   
FROM MST_TemplateData TB  WITH(NOLOCK)                                               
    CROSS APPLY OPENJSON (TB.[Data]) WITH                                                
    (                                                
        [LCOS1To4] NVarchar(1000) '$.LCOS1To4' ,                                        
  [Collection] NVarchar(1000) '$.Collection' ,                                        
  [SubCollection] NVarchar(1000) '$.SubCollection' ,                                        
  [DiscountGroup] NVarchar(1000) '$.DiscountGroup' ,                                        
  [RRPMarkup] float '$.RRPMarkup'                                          
    ) as  D                                          
  Where TB.TemplateMasterID = (select top 1 TemplateMasterID from [dbo].[MST_TemplateMaster] WITH(NOLOCK)  where TemplateName ='RRPReferences')                                         
End                                       
                                      
Begin---========================MOQ============================================================================                                      
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','MOQ','')                                      
SELECT Distinct                                                 
     D.[SchneiderElectricMaterialReference],D.[MOQa]                                           
  into #Temp_MOQ                                        
FROM MST_TemplateData TB WITH(NOLOCK)                                 
    CROSS APPLY OPENJSON (TB.[Data]) WITH                                                
    (                                                
        [SchneiderElectricMaterialReference] NVarchar(100) '$.SchneiderElectricMaterialReference' ,                                        
  [MOQa] NVarchar(100) '$.MOQa'                                         
    ) as  D                                          
  Where TB.TemplateMasterID = (select top 1 TemplateMasterID from [dbo].[MST_TemplateMaster] WITH(NOLOCK)  where TemplateName ='MOQ')                              
                                      
                                      
END     
    
Begin---========================GST============================================================================                                      
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','GST','')                                       
SELECT  Distinct                                               
     D.[CountryCode],D.[GSTPercentage]                                           
  into #Temp_GST                                       
FROM MST_TemplateData TB WITH(NOLOCK)                                                
    CROSS APPLY OPENJSON (TB.[Data]) WITH                                                
    (                                                
        [CountryCode] NVarchar(100) '$.CountryCode' ,                                        
        [GSTPercentage] Float '$.GSTPercentage'                                         
    ) as  D                                          
  Where TB.TemplateMasterID = (select top 1 TemplateMasterID from [dbo].[MST_TemplateMaster] WITH(NOLOCK)  where TemplateName ='GSTConfigurations')             
                                    
                                  
   select top 1 @SelectedSalesOrganizationGSTPercentage = GSTPercentage from #Temp_GST  where  CountryCode = @SalesOrganization                                  
                                  
  -- select @SelectedSalesOrganizationGSTPercentage as SelectedSalesOrganizationGSTPercentage                                   
END    
    
Begin---========================VRGDescriptions============================================================================                          
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','VRGDescriptions','')                                       
SELECT Distinct                                                 
     D.[VRG],D.[VRGDescription]                                           
  into #Temp_VRGDescriptions                                       
FROM MST_TemplateData TB WITH(NOLOCK)                                                
    CROSS APPLY OPENJSON (TB.[Data]) WITH                                                
    (                                                
        [VRG] NVarchar(100) '$.VRG' ,                                        
        [VRGDescription] NVarchar(1000) '$.VRGDescription'                                         
    ) as  D                                          
  Where TB.TemplateMasterID = (select top 1 TemplateMasterID from [dbo].[MST_TemplateMaster] WITH(NOLOCK)  where TemplateName ='VRGDescriptions')             
                                    
                                 
END    
  
Begin---========================#Temp_MaterialStatus===================================================================                                        
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','MaterialStatus','')                                        
SELECT  Distinct                                               
     D.[St],D.[Description],D.[Status] AS MaterialStatus                            
  into #Temp_MaterialStatus                   
FROM MST_TemplateData TB  WITH(NOLOCK)                                               
    CROSS APPLY OPENJSON (TB.[Data]) WITH                                                
    (                                                
        [St] NVarchar(100) '$.St' ,                                        
        [Description] NVarchar(1000) '$.Description' ,                                        
        [Status] NVarchar(100) '$.Status'                                     
    ) as  D                                          
  Where TB.TemplateMasterID = (select top 1 TemplateMasterID from [dbo].[MST_TemplateMaster] WITH(NOLOCK)  where TemplateName ='MaterialStatus')                                         
End                                       
   
    
    
Begin---========================Temp_get_Price_brks=========================================================                                         
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp_get_Price_brks','')                                       
                                    
SELECT Prices_brk.CustNo, Prices_brk.MATNR, Prices_brk.CondType AS KSCHL,                                  
[dbo].[UFN_GetBrkVal]([QtyBrk1],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk1Prc,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk2],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk2Prc,                                     
[dbo].[UFN_GetBrkVal]([QtyBrk3],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk3Prc,                                     
[dbo].[UFN_GetBrkVal]([QtyBrk4],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk4Prc,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk5],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk5Prc,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk6],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk6Prc     
 into #Temp_get_Price_brks                                    
FROM #Temp_Prices_brk as Prices_brk WITH(NOLOCK)     
INNER JOIN #Temp_Qty_Brks as Qty_Brks WITH(NOLOCK) ON (Prices_brk.MATNR = Qty_Brks.MATNR) AND (Prices_brk.CustNo = Qty_Brks.CustNo);                                    
                                    
                                    
End     
    
Begin---========================Temp_get_Discountx_brks====================================================     
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp_get_Discountx_brks','')     
Begin---========================Temp_get_Discount1_brks====================================================                                          
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp_get_Discount1_brks','')                                       
SELECT Discounts_brk.CustNo, Discounts_brk.MATNR, Discounts_brk.KSCHL, Discounts_brk.DiscTable AS Disc1Table,                                    
                                   
[dbo].[UFN_GetBrkVal]([QtyBrk1],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk1Disc1,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk2],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk2Disc1,            
[dbo].[UFN_GetBrkVal]([QtyBrk3],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk3Disc1,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk4],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk4Disc1,                                     
[dbo].[UFN_GetBrkVal]([QtyBrk5],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk5Disc1,                                     
[dbo].[UFN_GetBrkVal]([QtyBrk6],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk6Disc1                                    
into #Temp_get_Discount1_brks                                    
FROM #Temp_Discounts_brk as Discounts_brk WITH(NOLOCK)     
INNER JOIN #Temp_Qty_Brks as Qty_Brks WITH(NOLOCK) ON (Discounts_brk.CustNo = Qty_Brks.CustNo) AND (Discounts_brk.MATNR = Qty_Brks.MATNR)                                    
WHERE (((Discounts_brk.KSCHL) in ( select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where DiscountName = 'Discount1')  ));                                    
                               
                                    
End     
    
    
Begin---========================Temp_get_Discount2_brks====================================================                                          
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation) Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp_get_Discount2_brks','')                 
                                    
SELECT Discounts_brk.CustNo, Discounts_brk.MATNR, Discounts_brk.KSCHL, Discounts_brk.DiscTable AS Disc2Table,                                    
                                  
                                    
[dbo].[UFN_GetBrkVal]([QtyBrk1],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk1Disc2,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk2],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk2Disc2,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk3],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk3Disc2,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk4],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk4Disc2,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk5],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk5Disc2,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk6],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk6Disc2                                    
into #Temp_get_Discount2_brks                                    
                                    
FROM #Temp_Discounts_brk Discounts_brk WITH(NOLOCK) 
INNER JOIN #Temp_Qty_Brks Qty_Brks WITH(NOLOCK) ON (Discounts_brk.CustNo = Qty_Brks.CustNo) AND (Discounts_brk.MATNR = Qty_Brks.MATNR)                                    
WHERE (((Discounts_brk.KSCHL) in ( select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where DiscountName = 'Discount2') ))                       
                                    
End                                     
                                    
Begin---========================Temp_get_Discount3_brks====================================================                                          
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp_get_Discount3_brks','')                         
                                    
SELECT Discounts_brk.CustNo, Discounts_brk.MATNR, Discounts_brk.KSCHL, Discounts_brk.DiscTable AS Disc3Table,      
    
[dbo].[UFN_GetBrkVal]([QtyBrk1],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk1Disc3,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk2],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk2Disc3,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk3],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk3Disc3,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk4],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk4Disc3,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk5],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk5Disc3,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk6],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk6Disc3    
                                    
              
                                    
into #Temp_get_Discount3_brks                                    
                                    
FROM #Temp_Discounts_brk Discounts_brk WITH(NOLOCK) 
INNER JOIN #Temp_Qty_Brks Qty_Brks WITH(NOLOCK) ON (Discounts_brk.CustNo = Qty_Brks.CustNo) AND (Discounts_brk.MATNR = Qty_Brks.MATNR)                                    
WHERE (((Discounts_brk.KSCHL) in ( select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where DiscountName = 'Discount3')))                                    
                                    
End                                     
                                    
Begin---========================Temp_get_Discount4_brks====================================================                                 
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp_get_Discount4_brks','')                                        
                                    
SELECT Discounts_brk.CustNo, Discounts_brk.MATNR, Discounts_brk.KSCHL, Discounts_brk.DiscTable AS Disc4Table,                                    
                                    
[dbo].[UFN_GetBrkVal]([QtyBrk1],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk1Disc4,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk2],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk2Disc4,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk3],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk3Disc4,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk4],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk4Disc4,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk5],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk5Disc4,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk6],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk6Disc4                                 
                                    
into #Temp_get_Discount4_brks                                    
                                    
FROM #Temp_Discounts_brk Discounts_brk WITH(NOLOCK) 
INNER JOIN #Temp_Qty_Brks Qty_Brks WITH(NOLOCK) ON (Discounts_brk.CustNo = Qty_Brks.CustNo) AND (Discounts_brk.MATNR = Qty_Brks.MATNR)                                    
WHERE (((Discounts_brk.KSCHL) in ( select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where DiscountName = 'Discount4')))                       
           
End                                      
                                    
Begin---========================Temp_get_Discount5_brks====================================================                                          
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp_get_Discount5_brks','')                                       
SELECT Discounts_brk.CustNo, Discounts_brk.MATNR, Discounts_brk.KSCHL, Discounts_brk.DiscTable AS Disc5Table,                                    
 [dbo].[UFN_GetBrkVal]([QtyBrk1],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk1Disc5,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk2],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk2Disc5,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk3],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk3Disc5,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk4],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk4Disc5,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk5],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk5Disc5,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk6],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk6Disc5                                   
    
                                    
into #Temp_get_Discount5_brks                                    
                                    
FROM #Temp_Discounts_brk Discounts_brk WITH(NOLOCK) 
INNER JOIN #Temp_Qty_Brks Qty_Brks WITH(NOLOCK) ON (Discounts_brk.CustNo = Qty_Brks.CustNo) AND (Discounts_brk.MATNR = Qty_Brks.MATNR)                                    
WHERE (((Discounts_brk.KSCHL) in ( select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where DiscountName = 'Discount5')))                                    
                  
End                                     
                                    
Begin---========================Temp_get_Discount6_brks====================================================                                          
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp_get_Discount6_brks','')                                        
                          
SELECT Discounts_brk.CustNo, Discounts_brk.MATNR, Discounts_brk.KSCHL, Discounts_brk.DiscTable AS Disc6Table,                                    
                                    
[dbo].[UFN_GetBrkVal]([QtyBrk1],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk1Disc6,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk2],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk2Disc6,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk3],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk3Disc6,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk4],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk4Disc6,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk5],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk5Disc6,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk6],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk6Disc6               
                                    
into #Temp_get_Discount6_brks                                    
                                    
FROM #Temp_Discounts_brk Discounts_brk  WITH(NOLOCK) INNER JOIN    
#Temp_Qty_Brks Qty_Brks  WITH(NOLOCK) ON (Discounts_brk.CustNo = Qty_Brks.CustNo) AND (Discounts_brk.MATNR = Qty_Brks.MATNR)        
WHERE (((Discounts_brk.KSCHL) in ( select DiscountValue from #Temp_DiscountTypeList  WITH(NOLOCK) where DiscountName = 'Discount6')))                                 
      
       
                                    
                                    
End                                     
                                    
Begin---========================Temp_get_Discount7_brks====================================================                                          
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp_get_Discount7_brks','')                                        
                                    
SELECT Discounts_brk.CustNo, Discounts_brk.MATNR, Discounts_brk.KSCHL, Discounts_brk.DiscTable AS Disc7Table,                                    
                                    
[dbo].[UFN_GetBrkVal]([QtyBrk1],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk1Disc7,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk2],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk2Disc7,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk3],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk3Disc7,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk4],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk4Disc7,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk5],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk5Disc7,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk6],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk6Disc7                                 
                                    
into #Temp_get_Discount7_brks                                    
                                    
FROM #Temp_Discounts_brk Discounts_brk  WITH(NOLOCK) 
INNER JOIN #Temp_Qty_Brks Qty_Brks WITH(NOLOCK) ON (Discounts_brk.CustNo = Qty_Brks.CustNo) AND (Discounts_brk.MATNR = Qty_Brks.MATNR)                                    
WHERE (((Discounts_brk.KSCHL) in ( select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where DiscountName = 'Discount7')))                                    
                                    
                                    
End                                      
                                    
Begin---========================Temp_get_Discount8_brks====================================================                                          
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp_get_Discount8_brks','')                                       
                                    
SELECT Discounts_brk.CustNo, Discounts_brk.MATNR, Discounts_brk.KSCHL, Discounts_brk.DiscTable AS Disc8Table,                                    
                             
[dbo].[UFN_GetBrkVal]([QtyBrk1],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk1Disc8,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk2],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk2Disc8,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk3],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk3Disc8,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk4],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk4Disc8,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk5],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk5Disc8,                                    
[dbo].[UFN_GetBrkVal]([QtyBrk6],[KSTBM1],[KSTBM2],[KSTBM3],[KSTBM4],[KSTBM5],[KSTBM6],[KBETR1],[KBETR2],[KBETR3],[KBETR4],[KBETR5],[KBETR6]) AS Brk6Disc8                               
                                    
into #Temp_get_Discount8_brks                                    
                                    
FROM #Temp_Discounts_brk Discounts_brk WITH(NOLOCK) 
INNER JOIN #Temp_Qty_Brks Qty_Brks WITH(NOLOCK) ON (Discounts_brk.CustNo = Qty_Brks.CustNo) AND (Discounts_brk.MATNR = Qty_Brks.MATNR)                                    
WHERE (((Discounts_brk.KSCHL) in ( select DiscountValue from #Temp_DiscountTypeList WITH(NOLOCK) where DiscountName = 'Discount8')))                                    
                     
                                    
End                                     
     
    
End    
      
                                        
Begin---========================get_Stock_Status===========================================================                                          
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','get_Stock_Status','')        
                                    
--SELECT MARC.MATNR, 'S' AS [Status]                                    
--into #Temp_get_Stock_Status                                    
--FROM MARC WITH(NOLOCK)     
--inner join #Temp_Prices as Prices WITH(NOLOCK) on Prices.MATNR = MARC.MATNR     
--WHERE (((MARC.WERKS) In ('AU10','AU12','NZ10')) AND ((MARC.DISGR) In ('ALF0','ZSF0','ZSM0','ZSR0')))                                    
--GROUP BY MARC.MATNR;   

if(@SalesOrganization = 'AU01')
begin
Insert into #Temp_get_Stock_Status(MATNR,[Status]) 
SELECT Distinct MARC.MATNR, 'S' AS [Status]                                                                      
FROM MARC WITH(NOLOCK)     
inner join #Temp_Prices as Prices WITH(NOLOCK) on Prices.MATNR = MARC.MATNR     
WHERE (((MARC.WERKS) In ('AU10','AU12')) AND ((MARC.DISGR) In ('ALF0','ZSF0','ZSM0','ZSR0')))                                    
GROUP BY MARC.MATNR;
End
else
begin
Insert into #Temp_get_Stock_Status(MATNR,[Status]) 
SELECT Distinct MARC.MATNR, 'S' AS [Status]                                                                      
FROM MARC WITH(NOLOCK)     
inner join #Temp_Prices as Prices WITH(NOLOCK) on Prices.MATNR = MARC.MATNR     
WHERE (((MARC.WERKS) In ('NZ10')) AND ((MARC.DISGR) In ('ALF0','ZSF0','ZSM0','ZSR0')))                                    
GROUP BY MARC.MATNR;
End
                                    
                                    
End                                          
     
    
    
Begin---========================A507A+get_A507+get_Trade=======================================================================                                         
                                          
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','A507A','A507A+get_A507+get_Trade')                                         
                                        
SELECT Prices.CustNo,    
MaterialList.Prefix,    
MaterialList.Cat_No AS [CustomerCatNo],    
MaterialList.Colour_Code as [ColourCode],                                                               
MaterialList.Item_No AS [CustomerItemNo],    
Prices.MATNR AS [SchneiderElectricMaterialReference],    
MAKT.MAKTX AS [MaterialDescription],     
case when [KONM_sum].[KNUMH] is null then KONP.KBETR else [KONM_sum].KBETR1 end as TradeExclGST,--TrdPrc1    
[dbo].[UFN_GetIncGSTPrice] (cast(isnull(case when [KONM_sum].[KNUMH] is null then KONP.KBETR else [KONM_sum].KBETR1 end ,0) as float),@SelectedSalesOrganizationGSTPercentage) as [TradeInclGST],      
    
KONP.KPEIN AS Per,    
TRIM(T006A.MSEH3) AS UOM,    
case when MOQOverride.MOQa is not null then MOQOverride.MOQa                                       
     when isnull(MVKE.[AUMNG],0) = 0 then 1 else MVKE.[AUMNG] end as MOQ,     
    
case when MOQOverride.MOQa is not null then MOQOverride.MOQa                                       
     when isnull([MVKE].[SCMNG],0) = 0 then 1 else [MVKE].[SCMNG] end as [OrderMultiple],     
    
--Convert(float,case when (case when [KONM_sum].[KNUMH] is null then KONP.KBETR else [KONM_sum].KBETR1 end) is null then 0               
--       when RRP.[RRPMarkup] is null then 0                                     
--     else Round(Convert(float,((Convert(float,case when [KONM_sum].[KNUMH] is null then isnull(KONP.KBETR,0)                                     
--              else isnull([KONM_sum].KBETR1,0) end)*1.1)  *((1+ isnull(RRP.[RRPMarkup],0))+0.00001))),2)                                    
--  end) as RRPNonRounded, 

  Convert(float,case when (case when [KONM_sum].[KNUMH] is null then KONP.KBETR else [KONM_sum].KBETR1 end) is null then 0               
       when RRP.[RRPMarkup] is null then 0                                     
     else Round(
	 (
					Convert(float,
						case when [KONM_sum].[KNUMH] is null then isnull(KONP.KBETR,0)                                     
							else isnull([KONM_sum].KBETR1,0) end)
			  
			  *1.1)  *(1+ isnull(RRP.[RRPMarkup],0))+0.00001,2)                                    
  end) as RRPNonRounded, 

-- IIf(IsNull([TrdPrc1]),0,IIf(Not IsNull([RRP Markup]),Round(([TrdPrc1]*1.1)*(1+[RRP Markup])+0.00001,2),0)) AS RRP_Non_Rounded,

  --RRP_Recommended_Retail_Price    
    
    
--KONM_sum.KSTBM2 AS [QtyBreak2],                                     
--KONM_sum.KBETR2 AS [TradePriceBreak2ExclGST],        
--[dbo].[UFN_GetIncGSTPrice] (cast(isnull(KONM_sum.KBETR2,0) as float),@SelectedSalesOrganizationGSTPercentage) as [TradePriceBreak2InclGST],     
    
--KONM_sum.KSTBM3 AS [QtyBreak3],                                    
--KONM_sum.KBETR3 AS [TradePriceBreak3ExclGST],          
--[dbo].[UFN_GetIncGSTPrice] (cast(isnull(KONM_sum.KBETR3,0) as float),@SelectedSalesOrganizationGSTPercentage) as [TradePriceBreak3InclGST],      
    
 case when  Left([Prices].[KSCHL], 4) in ('ZNTP', 'ZPAF', 'ZPNP', 'ZNT1') then 'Special Net'                                   
      when Left([Prices].[KSCHL], 4) = 'ZPR0' and                                    
     (isnull(Disc1Table,'') != '' or isnull(Disc2Table,'') != ''  or                                   
      isnull(Disc3Table,'') != '' or isnull(Disc4Table,'') != '' or                                   
      isnull(Disc5Table,'') != '' or isnull(Disc6Table,'') != '' or                                   
      isnull(Disc7Table,'') != '' or isnull(Disc8Table,'') != '' ) then 'Discount'                                  
    when Left([Prices].[KSCHL], 4) = 'ZPR0' then 'Trade'  end as [PriceDerivedFrom],     
    
case when @CanUseMOQAsBrk1 = 1 then  (case when MOQOverride.MOQa is not null then MOQOverride.MOQa                                       
              when isnull(MVKE.[AUMNG],0) = 0 then 1 else MVKE.[AUMNG] end) else [Qty_Brks].[QtyBrk1] end as [PriceBreak1CustomerQty],     
                  
[dbo].[UFN_GetDiscPrice](case when isnull(get_Price_brks.Brk1Prc,0) != 0 then  Brk1Prc else     
             (case when [KONM_sum].[KNUMH] is null then KONP.KBETR else [KONM_sum].KBETR1 end) end ,                            
         Brk1Disc1,Brk1Disc2,Brk1Disc3,Brk1Disc4,Brk1Disc5,Brk1Disc6,Brk1Disc7,Brk1Disc8,@SODVal,@CanAddSODInFinalPrice,[Table])  AS [PriceBreak1CustomerCostExclGST],    
    
Qty_Brks.QtyBrk2 AS [PriceBreak2CustomerQty],     
[dbo].[UFN_GetDiscPrice](Brk2Prc ,Brk2Disc1,Brk2Disc2,Brk2Disc3,Brk2Disc4,Brk2Disc5,Brk2Disc6,Brk2Disc7,Brk2Disc8,@SODVal,@CanAddSODInFinalPrice,[Table])  AS [PriceBreak2CustomerCostExclGST],     
Qty_Brks.QtyBrk3 AS [PriceBreak3CustomerQty],    
[dbo].[UFN_GetDiscPrice](Brk3Prc ,Brk3Disc1,Brk3Disc2,Brk3Disc3,Brk3Disc4,Brk3Disc5,Brk3Disc6,Brk3Disc7,Brk3Disc8,@SODVal,@CanAddSODInFinalPrice,[Table])  AS [PriceBreak3CustomerCostExclGST],                               
Qty_Brks.QtyBrk4 AS [PriceBreak4CustomerQty],     
[dbo].[UFN_GetDiscPrice](Brk4Prc ,Brk4Disc1,Brk4Disc2,Brk4Disc3,Brk4Disc4,Brk4Disc5,Brk4Disc6,Brk4Disc7,Brk4Disc8,@SODVal,@CanAddSODInFinalPrice,[Table])  AS [PriceBreak4CustomerCostExclGST],                               
Qty_Brks.QtyBrk5 AS [PriceBreak5CustomerQty],      
[dbo].[UFN_GetDiscPrice](Brk5Prc ,Brk5Disc1,Brk5Disc2,Brk5Disc3,Brk5Disc4,Brk5Disc5,Brk5Disc6,Brk5Disc7,Brk5Disc8,@SODVal,@CanAddSODInFinalPrice,[Table])  AS [PriceBreak5CustomerCostExclGST],                               
Qty_Brks.QtyBrk6 AS [PriceBreak6CustomerQty],     
[dbo].[UFN_GetDiscPrice](Brk6Prc ,Brk6Disc1,Brk6Disc2,Brk6Disc3,Brk6Disc4,Brk6Disc5,Brk6Disc6,Brk6Disc7,Brk6Disc8,@SODVal,@CanAddSODInFinalPrice,[Table])  AS [PriceBreak6CustomerCostExclGST],                               
MARA.EAN11 AS Barcode,     
(isnull([PRDHA1],'')+''+isnull([PRDHA2],'')+''+isnull([PRDHA3],'')+''+isnull([PRDHA4],'')) as  [SAPCOS],      
MARM_sum.UoM_Qty_Text AS [CartonQty],    
case when [get_Stock_Status].[MATNR] is null then '* ' else [Status] end as StockStatus,     
Prices.DATAB AS [ValidFrom], Prices.DATBI AS [ValidTo],    
MaterialList.MaterialSource,    
    
A507.VKORG, A507.VTWEG,A507.PLTYP,A507.MATNR,A507.DATBI, A507.DATAB,    
case when [KONM_sum].[KNUMH] is null then KONP.KSTBM else [KONM_sum].[KSTBM1] end as TrdQty1,     
KONM_sum.KSTBM2 AS TrdQty2, KONM_sum.KSTBM3 AS TrdQty3, KONM_sum.KSTBM4 AS TrdQty4,     
KONM_sum.KSTBM5 AS TrdQty5, KONM_sum.KSTBM6 AS TrdQty6,      
case when [KONM_sum].[KNUMH] is null then KONP.KBETR else [KONM_sum].KBETR1 end as TrdPrc1,    
KONM_sum.KBETR2 AS TrdPrc2, KONM_sum.KBETR3 AS TrdPrc3, KONM_sum.KBETR4 AS TrdPrc4,     
KONM_sum.KBETR5 AS TrdPrc5, KONM_sum.KBETR6 AS TrdPrc6, Left([PRODH],11) AS LCOS,     
RRP.[RRPMarkup],--RRP_Non_Rounded    
KONP.KONWA as Currency,    
Temp_VRGDescriptions.VRG,    
Temp_VRGDescriptions.VRGDescription,    
'' as FileReferenceData,    
MaterialList.MainGroupPRODH,   
MaterialList.MainGroupPRODHDescription,  
MaterialList.[GroupPRODH],    
MaterialList.GroupPRODHDescription,   
MaterialList.SubGroupPRODH,  
MaterialList.SubGroupPRODHDescription,    
Temp_MaterialStatus.MaterialStatus, 

[Qty_Brks].[QtyBrk1],
    
    
--[dbo].[UFN_GetRetailPrice]([RRP_Non_Rounded],[LCOS]) as [RRP (Recommended Retail Price)],     
    
-- Extra Columns    
A507.KAPPL, A507.KSCHL,  A507.SPART,A507.KFRST,  A507.KBSTAT,A507.KNUMH,-- Left([PRODH],11) AS LCOS,    
[PRODH],KONP.KSTBM, KONP.KBETR, KONP.KMEIN                                       
into #Temp_get_A507_Trade FROM #Temp_Prices as Prices     
INNER JOIN A507 WITH(NOLOCK)  ON (Prices.VKORG = A507.VKORG) AND                                   
            (Prices.VTWEG = A507.VTWEG) AND                                   
            (Prices.MATNR = A507.MATNR) AND                                   
            (Prices.CustPriceList = A507.PLTYP)    
INNER JOIN KONP WITH(NOLOCK)  ON (A507.KNUMH = KONP.KNUMH) -- AND (A507.KNUMH = KONP.KNUMH)    
    
LEFT JOIN MVKE WITH(NOLOCK)  ON (A507.MATNR = MVKE.MATNR) AND (A507.VTWEG = MVKE.VTWEG) AND (A507.VKORG = MVKE.VKORG)    
LEFT JOIN KONM_sum WITH(NOLOCK)  ON A507.KNUMH = KONM_sum.KNUMH                                    
LEFT JOIN T006A WITH(NOLOCK)  ON KONP.KMEIN = T006A.MSEHI    
    
LEFT JOIN #Temp_MaterialList as MaterialList WITH(NOLOCK) ON (Prices.VTWEG = MaterialList.VTWEG) AND                                   
            (Prices.VKORG = MaterialList.VKORG) AND                                   
            (Prices.MATNR = MaterialList.MATNR)     
    
LEFT JOIN MAKT WITH(NOLOCK)  ON Prices.MATNR = MAKT.MATNR    
LEFT JOIN MARM_sum WITH(NOLOCK)  ON Prices.MATNR = MARM_sum.MATNR    
LEFT JOIN MARA WITH(NOLOCK)  ON Prices.MATNR = MARA.MATNR    
    
LEFT JOIN #Temp_RRP as RRP WITH(NOLOCK) ON Left([PRODH],11) = RRP.[LCOS1To4]     
LEFT JOIN #Temp_MOQ MOQOverride WITH(NOLOCK) ON A507.MATNR = MOQOverride.[SchneiderElectricMaterialReference]     
LEFT JOIN #Temp_MaterialStatus Temp_MaterialStatus WITH(NOLOCK) ON Temp_MaterialStatus.St = MVKE.VMSTA     
LEFT JOIN #Temp_VRGDescriptions Temp_VRGDescriptions WITH(NOLOCK) ON Temp_VRGDescriptions.VRG = MVKE.BONUS   
    
LEFT JOIN #Temp_get_Stock_Status as get_Stock_Status WITH(NOLOCK) ON Prices.MATNR = get_Stock_Status.MATNR    
    
LEFT JOIN #Temp_Qty_Brks as Qty_Brks WITH(NOLOCK) ON (Prices.CustNo = Qty_Brks.CustNo) AND (Prices.MATNR = Qty_Brks.MATNR)    
LEFT JOIN #Temp_get_Price_brks as get_Price_brks WITH(NOLOCK) ON (Prices.CustNo = get_Price_brks.CustNo) AND (Prices.MATNR = get_Price_brks.MATNR)    
LEFT JOIN #Temp_get_Discount1_brks as get_Discount1_brks WITH(NOLOCK) ON (Prices.MATNR = get_Discount1_brks.MATNR) AND (Prices.CustNo = get_Discount1_brks.CustNo)                                    
LEFT JOIN #Temp_get_Discount2_brks as get_Discount2_brks WITH(NOLOCK) ON (Prices.MATNR = get_Discount2_brks.MATNR) AND (Prices.CustNo = get_Discount2_brks.CustNo)                                     
LEFT JOIN #Temp_get_Discount3_brks as get_Discount3_brks WITH(NOLOCK) ON (Prices.MATNR = get_Discount3_brks.MATNR) AND (Prices.CustNo = get_Discount3_brks.CustNo)                                    
LEFT JOIN #Temp_get_Discount4_brks as get_Discount4_brks WITH(NOLOCK) ON (Prices.CustNo = get_Discount4_brks.CustNo) AND (Prices.MATNR = get_Discount4_brks.MATNR)                                     
LEFT JOIN #Temp_get_Discount5_brks as get_Discount5_brks WITH(NOLOCK) ON (Prices.CustNo = get_Discount5_brks.CustNo) AND (Prices.MATNR = get_Discount5_brks.MATNR)    
LEFT JOIN #Temp_get_Discount6_brks as get_Discount6_brks WITH(NOLOCK) ON (Prices.MATNR = get_Discount6_brks.MATNR) AND (Prices.CustNo = get_Discount6_brks.CustNo)                                    
LEFT JOIN #Temp_get_Discount7_brks as get_Discount7_brks WITH(NOLOCK) ON (Prices.CustNo = get_Discount7_brks.CustNo) AND (Prices.MATNR = get_Discount7_brks.MATNR)                                   
LEFT JOIN #Temp_get_Discount8_brks as get_Discount8_brks WITH(NOLOCK) ON (Prices.CustNo = get_Discount8_brks.CustNo) AND (Prices.MATNR = get_Discount8_brks.MATNR)    
  
  
--LEFT JOIN #Temp_CustomerList as CustomerList ON (Prices.CustNo = CustomerList.KUNNR) AND                                   
--            (Prices.VKORG = CustomerList.VKORG) AND                                   
--            (Prices.VTWEG = CustomerList.VTWEG) AND                                   
--            (Prices.SPART = CustomerList.SPART)     
    
    
    
--LEFT JOIN MVKE ON (A507.MATNR = MVKE.MATNR) AND (A507.VTWEG = MVKE.VTWEG) AND (A507.VKORG = MVKE.VKORG)                                         
--LEFT JOIN KONP ON get_A507.KNUMH = KONP.KNUMH     
WHERE (((A507.KAPPL)='V') AND ((A507.KSCHL)='ZPR0') AND ((A507.DATBI)>=@PricesActiveDate) AND                                         
((A507.DATAB)<=@PricesActiveDate) AND ((KONP.LOEVM_KO)<>'X')) and  A507.VKORG=@SalesOrganization    
    
--select '#Temp_get_A507', * from #Temp_get_A507_Trade    
End    
    
    
begin    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','load_Cust_Prices','Final Query')     
select     
CustNo,    
Prefix,    
CustomerCatNo,    
ColourCode,                                                               
CustomerItemNo,    
SchneiderElectricMaterialReference,    
MaterialDescription,     
TradeExclGST as WholesaleListPriceExclGST,    
TradeInclGST as WholesaleListPriceInclGST,      
Per,    
UOM,    
MOQ as MOQ_MinimumOrderQuantity,     
OrderMultiple,     
RRPNonRounded as RRP_RecommendedRetailPrice,    
[dbo].[UFN_GetRetailPrice](RRPNonRounded,LCOS) as ARRP_AdvertisedRecommendedRetailPrice,    
    
    
PriceDerivedFrom,     
    
PriceBreak1CustomerQty,     
PriceBreak1CustomerCostExclGST,    
[dbo].[UFN_GetIncGSTPrice] (PriceBreak1CustomerCostExclGST,@SelectedSalesOrganizationGSTPercentage) as [PriceBreak1CustomerCostInclGST],     
[dbo].[UFN_GetDiscTR] (PriceBreak1CustomerCostExclGST,QtyBrk1,[TrdQty1],[TrdQty2],[TrdQty3],[TrdQty4],[TrdQty5],[TrdQty6],[TrdPrc1],[TrdPrc2],[TrdPrc3],[TrdPrc4],[TrdPrc5],[TrdPrc6]) AS [PriceBreak1CustomerDiscount],                       
  
      
    
PriceBreak2CustomerQty,     
PriceBreak2CustomerCostExclGST,    
[dbo].[UFN_GetIncGSTPrice] (PriceBreak2CustomerCostExclGST,@SelectedSalesOrganizationGSTPercentage) as [PriceBreak2CustomerCostInclGST],                            
[dbo].[UFN_GetDiscTR] (PriceBreak2CustomerCostExclGST,PriceBreak2CustomerQty,[TrdQty1],[TrdQty2],[TrdQty3],[TrdQty4],[TrdQty5],[TrdQty6],[TrdPrc1],[TrdPrc2],[TrdPrc3],[TrdPrc4],[TrdPrc5],[TrdPrc6]) AS [PriceBreak2CustomerDiscount],                        
  
     
PriceBreak3CustomerQty,    
PriceBreak3CustomerCostExclGST,      
[dbo].[UFN_GetIncGSTPrice] (PriceBreak3CustomerCostExclGST,@SelectedSalesOrganizationGSTPercentage) as [PriceBreak3CustomerCostInclGST],                            
[dbo].[UFN_GetDiscTR] (PriceBreak3CustomerCostExclGST,PriceBreak3CustomerQty,[TrdQty1],[TrdQty2],[TrdQty3],[TrdQty4],[TrdQty5],[TrdQty6],[TrdPrc1],[TrdPrc2],[TrdPrc3],[TrdPrc4],[TrdPrc5],[TrdPrc6]) AS [PriceBreak3CustomerDiscount],                        
  
     
     
[PriceBreak4CustomerQty],     
[PriceBreak4CustomerCostExclGST],     
[dbo].[UFN_GetIncGSTPrice] (PriceBreak4CustomerCostExclGST,@SelectedSalesOrganizationGSTPercentage) as [PriceBreak4CustomerCostInclGST],                            
[dbo].[UFN_GetDiscTR] (PriceBreak4CustomerCostExclGST,PriceBreak4CustomerQty,[TrdQty1],[TrdQty2],[TrdQty3],[TrdQty4],[TrdQty5],[TrdQty6],[TrdPrc1],[TrdPrc2],[TrdPrc3],[TrdPrc4],[TrdPrc5],[TrdPrc6]) AS [PriceBreak4CustomerDiscount],                        
  
     
      
[PriceBreak5CustomerQty],      
[PriceBreak5CustomerCostExclGST],    
[dbo].[UFN_GetIncGSTPrice] (PriceBreak5CustomerCostExclGST,@SelectedSalesOrganizationGSTPercentage) as [PriceBreak5CustomerCostInclGST],                            
[dbo].[UFN_GetDiscTR] (PriceBreak5CustomerCostExclGST,PriceBreak5CustomerQty,[TrdQty1],[TrdQty2],[TrdQty3],[TrdQty4],[TrdQty5],[TrdQty6],[TrdPrc1],[TrdPrc2],[TrdPrc3],[TrdPrc4],[TrdPrc5],[TrdPrc6]) AS [PriceBreak5CustomerDiscount],                        
  
     
      
[PriceBreak6CustomerQty],     
[PriceBreak6CustomerCostExclGST],     
[dbo].[UFN_GetIncGSTPrice] (PriceBreak6CustomerCostExclGST,@SelectedSalesOrganizationGSTPercentage) as [PriceBreak6CustomerCostInclGST],                            
[dbo].[UFN_GetDiscTR] (PriceBreak6CustomerCostExclGST,PriceBreak6CustomerQty,[TrdQty1],[TrdQty2],[TrdQty3],[TrdQty4],[TrdQty5],[TrdQty6],[TrdPrc1],[TrdPrc2],[TrdPrc3],[TrdPrc4],[TrdPrc5],[TrdPrc6]) AS [PriceBreak6CustomerDiscount],                        
  
     
       
Barcode,     
[SAPCOS],
case when @SelectedProductHierarchy = 1 then 'GlobalCOS'
	 when @SelectedProductHierarchy = 2 then 'LocalCOS'
	 else '' end as ProductHierarchy ,
[CartonQty],    
StockStatus,     
[ValidFrom],    
[ValidTo],    
MaterialSource,    
Currency,    
VRG,    
VRGDescription,    
FileReferenceData,    
MainGroupPRODH,    
MainGroupPRODHDescription,    
[GroupPRODH],    
GroupPRODHDescription,   
SubGroupPRODH,  
SubGroupPRODHDescription,  
MaterialStatus  
--,    
    
--VKORG,    
--VTWEG,    
--PLTYP,    
--MATNR,    
--DATBI,    
--DATAB,    
--TrdQty1,     
--TrdQty2,     
--TrdQty3,     
--TrdQty4,     
--TrdQty5,     
--TrdQty6,      
--TrdPrc1,    
--TrdPrc2,     
--TrdPrc3,     
--TrdPrc4,     
--TrdPrc5,    
--TrdPrc6,     
--LCOS,     
--RRPMarkup,    
--KAPPL,    
--KSCHL,    
--SPART,    
--KFRST,    
--KBSTAT,    
--KNUMH,    
--PRODH,    
--KSTBM,    
--KBETR,     
--KMEIN       
into #Temp_Cust_Prices    
from #Temp_get_A507_Trade WITH(NOLOCK)   
  
  
  
Begin---========================#CanUseShiftBreaks============================================================================                         
if(@CanUseShiftBreaks =1)                        
begin        
  
UPDATE   Cust_Prices SET                         
Cust_Prices.PriceBreak1CUSTOMERQTY = PriceBreak2CUSTOMERQTY,                        
Cust_Prices.PriceBreak1CustomerDiscount = PriceBreak2CustomerDiscount,                        
Cust_Prices.PriceBreak1CustomerCostExclGST = PriceBreak2CustomerCostExclGST,                        
Cust_Prices.PriceBreak1CustomerCostInclGST = PriceBreak2CustomerCostInclGST,    
  
Cust_Prices.PriceBreak2CUSTOMERQTY = PriceBreak3CUSTOMERQTY,                        
Cust_Prices.PriceBreak2CustomerDiscount = PriceBreak3CustomerDiscount,                        
Cust_Prices.PriceBreak2CustomerCostExclGST = PriceBreak3CustomerCostExclGST,                        
Cust_Prices.PriceBreak2CustomerCostInclGST = PriceBreak3CustomerCostInclGST,                        
                        
Cust_Prices.PriceBreak3CUSTOMERQTY = PriceBreak4CUSTOMERQTY,                        
Cust_Prices.PriceBreak3CustomerDiscount = PriceBreak4CustomerDiscount,                        
Cust_Prices.PriceBreak3CustomerCostExclGST = PriceBreak4CustomerCostExclGST,                        
Cust_Prices.PriceBreak3CustomerCostInclGST = PriceBreak4CustomerCostInclGST,                    
                        
Cust_Prices.PriceBreak4CUSTOMERQTY = PriceBreak5CUSTOMERQTY,                        
Cust_Prices.PriceBreak4CustomerDiscount = PriceBreak5CustomerDiscount,                        
Cust_Prices.PriceBreak4CustomerCostExclGST = PriceBreak5CustomerCostExclGST,                        
Cust_Prices.PriceBreak4CustomerCostInclGST = PriceBreak5CustomerCostInclGST,                        
                        
Cust_Prices.PriceBreak5CUSTOMERQTY = PriceBreak6CUSTOMERQTY,                        
Cust_Prices.PriceBreak5CustomerDiscount = PriceBreak6CustomerDiscount,                        
Cust_Prices.PriceBreak5CustomerCostExclGST = PriceBreak6CustomerCostExclGST,                        
Cust_Prices.PriceBreak5CustomerCostInclGST = PriceBreak6CustomerCostInclGST,                        
                        
Cust_Prices.PriceBreak6CUSTOMERQTY = '',                        
Cust_Prices.PriceBreak6CustomerDiscount = '',                        
Cust_Prices.PriceBreak6CustomerCostExclGST = '',                        
Cust_Prices.PriceBreak6CustomerCostInclGST = ''                        
from #Temp_Cust_Prices Cust_Prices WITH(NOLOCK)                        
WHERE (((Cust_Prices.PriceBreak1CUSTOMERQTY)>0) AND ((Cust_Prices.PriceBreak1CustomerDiscount)=0) AND                        
   ((Cust_Prices.WholesaleListPriceExclGST) Is Not Null And (Cust_Prices.WholesaleListPriceExclGST)>0));           
           
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Shift Customer Prices','Shift Customer Prices if discount is 0"')          
                        
end   
End     
--select * from #Temp_Cust_Prices    
End    
    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','load_Cust_Prices','End')     
    
end    
    
 update dbo.TRN_PriceFileHeader set [StatusText] ='Loading CustomerPrices Completed' ,        
           [PercentCompleted] =80 ,    
           IsCompleted =1    
           where PriceFileHeaderID =@PriceFileHeaderID      
               
    
 --select   * from #Temp_Cust_Prices  
   if(@CanShowTemplateMaterialOnly = 1)      
  begin      
   IF OBJECT_ID(N'tempdb..#Temp_Cust_Prices') IS NOT NULL    
   begin    
   insert into [dbo].[TRN_PriceFileDetails]([PriceFileHeaderID],[CustomerNo],[Prefix],[CustomerCatNo],[ColourCode]    
   ,[CustomerItemNo],[SchneiderElectricMaterialReference],[MaterialDescription],[WholesaleListPriceExclGST],[WholesaleListPriceInclGST]    
      ,[Per],[UOM],[MOQ],[OrderMultiple],[RecommendedRetailPrice],[AdvertisedRecommendedRetailPrice],[PriceDerivedFrom]    
      ,[PriceBreak1CustomerQty],[PriceBreak1CustomerDiscount],[PriceBreak1CustomerCostExclGST],[PriceBreak1CustomerCostInclGST]    
      ,[PriceBreak2CustomerQty],[PriceBreak2CustomerDiscount] ,[PriceBreak2CustomerCostExclGST],[PriceBreak2CustomerCostInclGST]    
   ,[PriceBreak3CustomerQty],[PriceBreak3CustomerDiscount],[PriceBreak3CustomerCostExclGST],[PriceBreak3CustomerCostInclGST]    
      ,[PriceBreak4CustomerQty],[PriceBreak4CustomerDiscount],[PriceBreak4CustomerCostExclGST],[PriceBreak4CustomerCostInclGST]    
   ,[PriceBreak5CustomerQty],[PriceBreak5CustomerDiscount],[PriceBreak5CustomerCostExclGST],[PriceBreak5CustomerCostInclGST]    
      ,[Barcode],ProductHierarchy,[SAPCOS],[CartonQty],[StockStatus],[ValidFrom],[ValidTo],[FileReferenceData],[Currency],[VRG],[VRGDescription]    
      ,[MaterialStatus],[MainGroup],[MainGroupDescription],[Group],[GroupDescription],SubGroup,[SubGroupDescription],[CreatedBy])     
select  @PriceFileHeaderID,isnull(CustNo,''),isnull(Prefix,''),isnull(CustomerCatNo,''),isnull(ColourCode,''),                                                               
  isnull(CustomerItemNo,''),isnull(SchneiderElectricMaterialReference,''),isnull(MaterialDescription,''),
  isnull(WholesaleListPriceExclGST,0),isnull(WholesaleListPriceInclGST,0),      
  isnull(Per,0),isnull(UOM,''),isnull(MOQ_MinimumOrderQuantity,0),isnull(OrderMultiple,0), 
  isnull(RRP_RecommendedRetailPrice,0),isnull(ARRP_AdvertisedRecommendedRetailPrice,0),isnull(PriceDerivedFrom,''),     
  isnull(PriceBreak1CustomerQty,0),isnull([PriceBreak1CustomerDiscount],0),
  isnull(PriceBreak1CustomerCostExclGST,0),isnull([PriceBreak1CustomerCostInclGST],0),      
  isnull(PriceBreak2CustomerQty,0),isnull([PriceBreak2CustomerDiscount],0),
  isnull(PriceBreak2CustomerCostExclGST,0),isnull([PriceBreak2CustomerCostInclGST],0),     
  isnull(PriceBreak3CustomerQty,0),isnull([PriceBreak3CustomerDiscount],0),
  isnull(PriceBreak3CustomerCostExclGST,0),isnull([PriceBreak3CustomerCostInclGST],0),    
  isnull(PriceBreak4CustomerQty,0),isnull([PriceBreak4CustomerDiscount],0),
  isnull(PriceBreak4CustomerCostExclGST,0),isnull([PriceBreak4CustomerCostInclGST],0),     
  isnull(PriceBreak5CustomerQty,0),isnull([PriceBreak5CustomerDiscount],0),
  isnull(PriceBreak5CustomerCostExclGST,0),isnull([PriceBreak5CustomerCostInclGST],0),    
  isnull(Barcode,''),isnull(ProductHierarchy,''), isnull(SAPCOS,''),isnull([CartonQty],''),
  isnull(StockStatus,''), [ValidFrom],[ValidTo],isnull(FileReferenceData,''),isnull(Currency,''),
  isnull(VRG,''),isnull(VRGDescription,''),isnull(MaterialStatus,''),isnull(MainGroupPRODH,''),
  isnull(MainGroupPRODHDescription,''),isnull([GroupPRODH],''),isnull(GroupPRODHDescription,''),
  isnull(SubGroupPRODH,''),isnull(SubGroupPRODHDescription,''),isnull(@RequestedUserSESA,'')  
from #Temp_Cust_Prices WITH(NOLOCK) where MaterialSource ='T'     
   end    
   End      
   else    
   begin    
   IF OBJECT_ID(N'tempdb..#Temp_Cust_Prices') IS NOT NULL      
    begin    
   insert into [dbo].[TRN_PriceFileDetails]([PriceFileHeaderID],[CustomerNo],[Prefix],[CustomerCatNo],[ColourCode]    
   ,[CustomerItemNo],[SchneiderElectricMaterialReference],[MaterialDescription],[WholesaleListPriceExclGST],[WholesaleListPriceInclGST]    
      ,[Per],[UOM],[MOQ],[OrderMultiple],[RecommendedRetailPrice],[AdvertisedRecommendedRetailPrice],[PriceDerivedFrom]    
      ,[PriceBreak1CustomerQty],[PriceBreak1CustomerDiscount],[PriceBreak1CustomerCostExclGST],[PriceBreak1CustomerCostInclGST]    
      ,[PriceBreak2CustomerQty],[PriceBreak2CustomerDiscount] ,[PriceBreak2CustomerCostExclGST],[PriceBreak2CustomerCostInclGST]    
   ,[PriceBreak3CustomerQty],[PriceBreak3CustomerDiscount],[PriceBreak3CustomerCostExclGST],[PriceBreak3CustomerCostInclGST]    
      ,[PriceBreak4CustomerQty],[PriceBreak4CustomerDiscount],[PriceBreak4CustomerCostExclGST],[PriceBreak4CustomerCostInclGST]    
   ,[PriceBreak5CustomerQty],[PriceBreak5CustomerDiscount],[PriceBreak5CustomerCostExclGST],[PriceBreak5CustomerCostInclGST]    
      ,[Barcode],ProductHierarchy,[SAPCOS],[CartonQty],[StockStatus],[ValidFrom],[ValidTo],[FileReferenceData],[Currency],[VRG],[VRGDescription]    
      ,[MaterialStatus],[MainGroup],[MainGroupDescription],[Group],[GroupDescription],SubGroup,[SubGroupDescription],[CreatedBy])     
select  @PriceFileHeaderID,isnull(CustNo,''),isnull(Prefix,''),isnull(CustomerCatNo,''),isnull(ColourCode,''),                                                               
  isnull(CustomerItemNo,''),isnull(SchneiderElectricMaterialReference,''),isnull(MaterialDescription,''),
  isnull(WholesaleListPriceExclGST,0),isnull(WholesaleListPriceInclGST,0),      
  isnull(Per,0),isnull(UOM,''),isnull(MOQ_MinimumOrderQuantity,0),isnull(OrderMultiple,0), 
  isnull(RRP_RecommendedRetailPrice,0),isnull(ARRP_AdvertisedRecommendedRetailPrice,0),isnull(PriceDerivedFrom,''),     
  isnull(PriceBreak1CustomerQty,0),isnull([PriceBreak1CustomerDiscount],0),
  isnull(PriceBreak1CustomerCostExclGST,0),isnull([PriceBreak1CustomerCostInclGST],0),      
  isnull(PriceBreak2CustomerQty,0),isnull([PriceBreak2CustomerDiscount],0),
  isnull(PriceBreak2CustomerCostExclGST,0),isnull([PriceBreak2CustomerCostInclGST],0),     
  isnull(PriceBreak3CustomerQty,0),isnull([PriceBreak3CustomerDiscount],0),
  isnull(PriceBreak3CustomerCostExclGST,0),isnull([PriceBreak3CustomerCostInclGST],0),    
  isnull(PriceBreak4CustomerQty,0),isnull([PriceBreak4CustomerDiscount],0),
  isnull(PriceBreak4CustomerCostExclGST,0),isnull([PriceBreak4CustomerCostInclGST],0),     
  isnull(PriceBreak5CustomerQty,0),isnull([PriceBreak5CustomerDiscount],0),
  isnull(PriceBreak5CustomerCostExclGST,0),isnull([PriceBreak5CustomerCostInclGST],0),    
  isnull(Barcode,''),isnull(ProductHierarchy,''), isnull(SAPCOS,''),isnull([CartonQty],''),
  isnull(StockStatus,''), [ValidFrom],[ValidTo],isnull(FileReferenceData,''),isnull(Currency,''),
  isnull(VRG,''),isnull(VRGDescription,''),isnull(MaterialStatus,''),isnull(MainGroupPRODH,''),
  isnull(MainGroupPRODHDescription,''),isnull([GroupPRODH],''),isnull(GroupPRODHDescription,''),
  isnull(SubGroupPRODH,''),isnull(SubGroupPRODHDescription,''),isnull(@RequestedUserSESA,'')    
from #Temp_Cust_Prices WITH(NOLOCK)     
   end    
   end    


    update dbo.TRN_PriceFileHeader set [StatusText] ='Updated Customer Prices into PriceFile Table' ,     
           [Status] ='Completed',    
           [PercentCompleted] =100 ,    
           IsCompleted =1 ,
		   ModifiedDate = GetUTCDate()
           where PriceFileHeaderID =@PriceFileHeaderID 

Begin------=========================Find Missing Materials 
Begin try
   select Distinct ML.MATNR
   into #Temp_MissingMaterials 
   from #Temp_MaterialList ML WITH(NOLOCK) 
   left join #Temp_Prices P WITH(NOLOCK) on P.MATNR = ML.MATNR 
   where  P.MATNR is null and ML.MaterialSource ='T'


insert into [dbo].[TRN_PriceFileDetails]([PriceFileHeaderID],[CustomerNo],
    [SchneiderElectricMaterialReference], IsFound,[CreatedBy],
    [Prefix],[CustomerCatNo],[ColourCode]    
   ,[CustomerItemNo],[MaterialDescription],[WholesaleListPriceExclGST],[WholesaleListPriceInclGST]    
   ,[Per],[UOM],[MOQ],[OrderMultiple],[RecommendedRetailPrice],[AdvertisedRecommendedRetailPrice],[PriceDerivedFrom]    
   ,[PriceBreak1CustomerQty],[PriceBreak1CustomerDiscount],[PriceBreak1CustomerCostExclGST],[PriceBreak1CustomerCostInclGST]    
   ,[PriceBreak2CustomerQty],[PriceBreak2CustomerDiscount] ,[PriceBreak2CustomerCostExclGST],[PriceBreak2CustomerCostInclGST]    
   ,[PriceBreak3CustomerQty],[PriceBreak3CustomerDiscount],[PriceBreak3CustomerCostExclGST],[PriceBreak3CustomerCostInclGST]    
   ,[PriceBreak4CustomerQty],[PriceBreak4CustomerDiscount],[PriceBreak4CustomerCostExclGST],[PriceBreak4CustomerCostInclGST]    
   ,[PriceBreak5CustomerQty],[PriceBreak5CustomerDiscount],[PriceBreak5CustomerCostExclGST],[PriceBreak5CustomerCostInclGST]    
   ,[Barcode],ProductHierarchy,[SAPCOS],[CartonQty],[StockStatus],[ValidFrom],[ValidTo],[FileReferenceData],[Currency],[VRG],[VRGDescription]    
   ,[MaterialStatus],[MainGroup],[MainGroupDescription],[Group],[GroupDescription],SubGroup,[SubGroupDescription]) 
select Distinct @PriceFileHeaderID, isnull(CL.KUNNR,''),isnull(MM.MATNR,''),0,isnull(@RequestedUserSESA,''),
		'','','',
		'','',0,0,
		0,'',0,0,0,0,'',
		0,0,0,0,
		0,0,0,0,
		0,0,0,0,
		0,0,0,0,
		0,0,0,0,
		'','','','','',GetUTCDate(),GetUTCDate(),'','','','',
		'','','','','','',''
from #Temp_CustomerList CL WITH(NOLOCK) cross apply #Temp_MissingMaterials MM WITH(NOLOCK)


Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'Info','Temp_MissingMaterials','End')   

End Try
 BEGIN CATCH                                                  
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'ERROR','Catch Block',ERROR_MESSAGE())  

 END CATCH   
End

		   
insert into [dbo].[TRN_PriceFileLocationDetails]([PriceFileHeaderID],[CustomerNo],[Status],[StatusText],[PercentCompleted],CreatedBy)
select Distinct @PriceFileHeaderID,zKUNNR,'In-Progress','Uploading PFC Data into Excel File',5,@RequestedUserSESA from #Temp_SelectedCustomers


   

       
    
    
    
    
     --select * from #Temp_MaterialList                                               
  --select top 1  '#Temp_SelectedCustomers' as Temp_SelectedCustomers, * from  #Temp_SelectedCustomers;                                                                                            
  --select top 1  '#Temp_CustomerList' as Temp_CustomerList, * from #Temp_CustomerList;                                                
  --select top 1  '#Temp_MaterialList' as Temp_MaterialList, * from #Temp_MaterialList;       
  --select top 1  '#Temp_Prices' as Temp_Prices, * from #Temp_Prices;                                                                                            
  --select top 1  '#Temp_Discounts' as Temp_Discounts, * from #Temp_Discounts;     
  --select top 1  '#Temp_Prices_brk' as Temp_Prices_brk, * from #Temp_Prices_brk;    
  --select top 1  '#Temp_Qty_Brks_tmp' as Temp_Qty_Brks_tmp, * from #Temp_Qty_Brks_tmp;     
  --select top 1 '#Temp_Qty_Brks' as Temp_Qty_Brks, * from #Temp_Qty_Brks    
  --select  top 1  '#Temp_get_Price_brks', * from #Temp_get_Price_brks    
  --select top 1  '#Temp_get_Discount1_brks' as Temp_get_Discount1_brks, * from  #Temp_get_Discount1_brks;     
  --select top 1 '#Temp_get_Stock_Status' as Temp_get_Stock_Status, * from #Temp_get_Stock_Status    
  --select '#Temp_get_A507_Trade', * from #Temp_get_A507_Trade    
  --select '#Temp_VRGDescriptions', * from #Temp_VRGDescriptions    
  --select '#Temp_get_Discount1_brks', * from #Temp_get_Discount1_brks    
  --  select  '#Temp_get_Discount2_brks' as Temp_get_Discount2_brks, * from  #Temp_get_Discount2_brks;                                        
  --select   '#Temp_get_Discount3_brks' as Temp_get_Discount3_brks, * from  #Temp_get_Discount3_brks;                                        
  --select  '#Temp_get_Discount4_brks' as Temp_get_Discount4_brks, * from #Temp_get_Discount4_brks;                                        
  --select '#Temp_get_Discount5_brks' as Temp_get_Discount5_brks, * from  #Temp_get_Discount5_brks;                                        
  --select  '#Temp_get_Discount6_brks' as Temp_get_Discount6_brks, * from  #Temp_get_Discount6_brks;                                    
  --select   '#Temp_get_Discount7_brks' as Temp_get_Discount7_brks, * from  #Temp_get_Discount7_brks;                                        
  --select   '#Temp_get_Discount8_brks' as Temp_get_Discount8_brks, * from  #Temp_get_Discount8_brks;     
       
    --select top 1  '#Temp_DiscountTypeList' as Temp_DiscountTypeList, * from #Temp_DiscountTypeList;    
  --select top 1  '#Temp_Discounts_PH' as Temp_Discounts_PH, * from #Temp_Discounts_PH;                                                
  --select '#Temp_Discounts_brk' as Temp_Discounts_brk, * from #Temp_Discounts_brk;                                                                                             
  --select top 1  '#Temp_Qty_Brks_tmp' as Temp_Qty_Brks_tmp, * from #Temp_Qty_Brks_tmp;                                                
  --select top 1  '#Temp_Qty_Brks' as Temp_Qty_Brks, * from #Temp_Qty_Brks;                                                                          
  --select top 1  '#Temp_get_A507' as Temp_get_A507, * from  #Temp_get_A507;                                     
  --select top 1  '#Temp_RRP' as Temp_RRP, * from  #Temp_RRP;                                       
  --select top 1  '#Temp_MOQ' as Temp_RRP, * from  #Temp_MOQ;                                    
  --select top 1  '#Temp_get_Stock_Status' as Temp_get_Stock_Status, * from  #Temp_get_Stock_Status;                                    
                                    
  --select top 1  '#Temp_get_Price_brks' as Temp_get_Price_brks, * from  #Temp_get_Price_brks;                                     
  --select top 1  '#Temp_get_Discount1_brks' as Temp_get_Discount1_brks, * from  #Temp_get_Discount1_brks;                                         
  --select top 1  '#Temp_get_Discount2_brks' as Temp_get_Discount2_brks, * from  #Temp_get_Discount2_brks;                                        
  --select top 1  '#Temp_get_Discount3_brks' as Temp_get_Discount3_brks, * from  #Temp_get_Discount3_brks;                                        
  --select top 1  '#Temp_get_Discount4_brks' as Temp_get_Discount4_brks, * from #Temp_get_Discount4_brks;                                        
  --select top 1  '#Temp_get_Discount5_brks' as Temp_get_Discount5_brks, * from  #Temp_get_Discount5_brks;                                        
  --select top 1  '#Temp_get_Discount6_brks' as Temp_get_Discount6_brks, * from  #Temp_get_Discount6_brks;                                    
  --select top 1  '#Temp_get_Discount7_brks' as Temp_get_Discount7_brks, * from  #Temp_get_Discount7_brks;                                        
  --select top 1  '#Temp_get_Discount8_brks' as Temp_get_Discount8_brks, * from  #Temp_get_Discount8_brks;                                   
  --select top 1  '#Temp_GST' as Temp_GST, * from  #Temp_GST;                           
  --select top 1  '#Temp_Cust_Prices' as Temp_Cust_Prices, * from #Temp_Cust_Prices     
    
    
 SET @ResultFlag = 'Success'    
 SET @Result = 'Data Returned back !!!'    
    
 END    
    
     
END TRY                                                
 BEGIN CATCH                                                
 print ERROR_MESSAGE()     
 if(@PriceFileHeaderID > 0)    
 begin    
  update dbo.TRN_PriceFileHeader set [StatusText] = ERROR_MESSAGE() ,     
           [Status] ='Failed',    
           [PercentCompleted] =0 ,    
           IsCompleted =1 ,
		   ModifiedDate = GetUTCDate()
           where PriceFileHeaderID =@PriceFileHeaderID       
End    
Insert into dbo.TRN_PriceFileLog(PriceFileHeaderID,CreatedBy,LogType,FunctionName,LogInformation)   
Values(@PriceFileHeaderID,@RequestedUserSESA,'ERROR','Catch Block',ERROR_MESSAGE())       
SET @ResultFlag = 'Failed'    
SET @Result = ERROR_MESSAGE()    
--Some Error Occured !!!    
 END CATCH 
 

--  Begin---===================Finally Deleting Temp Tables====================================       
                 
--   IF OBJECT_ID(N'tempdb..#Temp_SelectedCustomers') IS NOT NULL DROP TABLE #Temp_SelectedCustomers;                                                                                                                                        
--   IF OBJECT_ID(N'tempdb..#Temp_CustomerList') IS NOT NULL DROP TABLE #Temp_CustomerList;                                                
--   IF OBJECT_ID(N'tempdb..#Temp_MaterialList') IS NOT NULL DROP TABLE #Temp_MaterialList;                                                                                                                                        
--   IF OBJECT_ID(N'tempdb..#Temp_Prices') IS NOT NULL DROP TABLE #Temp_Prices;                                                
--   IF OBJECT_ID(N'tempdb..#Temp_DiscountTypeList') IS NOT NULL DROP TABLE #Temp_DiscountTypeList;                                                
--   IF OBJECT_ID(N'tempdb..#Temp_Discounts') IS NOT NULL DROP TABLE #Temp_Discounts;                                                
--   IF OBJECT_ID(N'tempdb..#Temp_Discounts_PH') IS NOT NULL DROP TABLE #Temp_Discounts_PH;                                                
--   IF OBJECT_ID(N'tempdb..#Temp_Discounts_brk') IS NOT NULL DROP TABLE #Temp_Discounts_brk;                                                
--   IF OBJECT_ID(N'tempdb..#Temp_Prices_brk') IS NOT NULL DROP TABLE #Temp_Prices_brk;                                                
--   IF OBJECT_ID(N'tempdb..#Temp_Qty_Brks_tmp') IS NOT NULL DROP TABLE #Temp_Qty_Brks_tmp;                                                
--   IF OBJECT_ID(N'tempdb..#Temp_Qty_Brks') IS NOT NULL DROP TABLE #Temp_Qty_Brks;                                                                                
--   IF OBJECT_ID(N'tempdb..#Temp_get_A507_Trade') IS NOT NULL DROP TABLE #Temp_get_A507_Trade;     
       
--   IF OBJECT_ID(N'tempdb..#Temp_RRP') IS NOT NULL DROP TABLE #Temp_RRP;                                        
--   IF OBJECT_ID(N'tempdb..#Temp_MOQ') IS NOT NULL DROP TABLE #Temp_MOQ;     
--   IF OBJECT_ID(N'tempdb..#Temp_GST') IS NOT NULL DROP TABLE #Temp_GST;     
--   IF OBJECT_ID(N'tempdb..#Temp_VRGDescriptions') IS NOT NULL DROP TABLE #Temp_VRGDescriptions;  
--   IF OBJECT_ID(N'tempdb..#Temp_MaterialStatus') IS NOT NULL DROP TABLE #Temp_MaterialStatus;  
--   --  
       
--   IF OBJECT_ID(N'tempdb..#Temp_get_Stock_Status') IS NOT NULL DROP TABLE #Temp_get_Stock_Status;                        
--   IF OBJECT_ID(N'tempdb..#Temp_get_Price_brks') IS NOT NULL DROP TABLE #Temp_get_Price_brks;                                           
--   IF OBJECT_ID(N'tempdb..#Temp_get_Discount1_brks') IS NOT NULL DROP TABLE #Temp_get_Discount1_brks;                                         
--   IF OBJECT_ID(N'tempdb..#Temp_get_Discount2_brks') IS NOT NULL DROP TABLE #Temp_get_Discount2_brks;                                        
--   IF OBJECT_ID(N'tempdb..#Temp_get_Discount3_brks') IS NOT NULL DROP TABLE #Temp_get_Discount3_brks;                                        
--   IF OBJECT_ID(N'tempdb..#Temp_get_Discount4_brks') IS NOT NULL DROP TABLE #Temp_get_Discount4_brks;                                        
--   IF OBJECT_ID(N'tempdb..#Temp_get_Discount5_brks') IS NOT NULL DROP TABLE #Temp_get_Discount5_brks;                                        
--   IF OBJECT_ID(N'tempdb..#Temp_get_Discount6_brks') IS NOT NULL DROP TABLE #Temp_get_Discount6_brks;                                        
--   IF OBJECT_ID(N'tempdb..#Temp_get_Discount7_brks') IS NOT NULL DROP TABLE #Temp_get_Discount7_brks;                                        
--   IF OBJECT_ID(N'tempdb..#Temp_get_Discount8_brks') IS NOT NULL DROP TABLE #Temp_get_Discount8_brks; 
--   IF OBJECT_ID(N'tempdb..#Temp_Qty_Brks_tmp_RowNumber') IS NOT NULL DROP TABLE #Temp_Qty_Brks_tmp_RowNumber; 

--   IF OBJECT_ID(N'tempdb..#Temp_MissingMaterials') IS NOT NULL DROP TABLE #Temp_MissingMaterials; 
                            
--   IF OBJECT_ID(N'tempdb..#Temp_Cust_Prices') IS NOT NULL DROP TABLE #Temp_Cust_Prices;                           
                                     
                                                
--End      
     
 PRINT @ResultFlag    
 PRINT @Result    




  
     
    
    
     
    
      
    
    
    
END   

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
  
  
CREATE or Alter Procedure [dbo].[USPM_GetTemplateDataByTemplateID](
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

Go


IF OBJECT_ID(N'dbo.TRN_AuditLog',N'U')		IS  NULL 
Begin
CREATE TABLE [dbo].[TRN_AuditLog] (
    [AuditLogID] bigint NOT NULL IDENTITY (1, 1) CONSTRAINT [PK_AuditLog_AuditLogID]  PRIMARY KEY ,
    [TableName] varchar(50) NOT NULL,
    [AuditAction] CHAR(1) NOT NULL CONSTRAINT [DF_AuditLog_AuditAction] DEFAULT (('')),--'I', 'U', 'D'
    [OldRowData] nvarchar(max) NOT NULL CONSTRAINT [DF_AuditLog_OldRowData] DEFAULT (('')),
	[NewRowData] nvarchar(max) NOT NULL CONSTRAINT [DF_AuditLog_NewRowData] DEFAULT (('')),
	[AuditDate] datetime NOT NULL CONSTRAINT [DF_AuditLog_AuditDate] DEFAULT ((getutcdate())),
	[AuditUser] varchar(100) NOT NULL CONSTRAINT [DF_AuditLog_AuditUser]  DEFAULT (''),
    [AuditUserRole] varchar(100) NOT NULL CONSTRAINT [DF_AuditLog_AuditUserRole]  DEFAULT (''),
    [AuditSQLUser] varchar(1000) NOT NULL CONSTRAINT [DF_AuditLog_AuditSQLUser]  DEFAULT (suser_sname()),
    [AuditAPP] varchar(128) CONSTRAINT [DF_AuditLog_AuditAPP]  DEFAULT (('App=('+rtrim(isnull(app_name(),'')))+') ')
);
End

Go




--exec USPT_CreateTrigger 'MST_AppConfig'
--exec USPT_CreateTrigger 'MST_TemplateData'
--exec USPT_CreateTrigger 'MST_TemplateMaster'
--exec USPT_CreateTrigger 'MST_UserMaster'
--exec USPT_CreateTrigger 'MST_UserRoleMapping'


----exec USPT_CreateTrigger 'MST_CustomerContact'
----exec USPT_CreateTrigger 'MST_MaterialMaster'

--ALTER TABLE MST_AppConfig ENABLE TRIGGER UTR_MST_AppConfig_Audit
--ALTER TABLE MST_TemplateData ENABLE TRIGGER UTR_MST_TemplateData_Audit
--ALTER TABLE MST_TemplateMaster ENABLE TRIGGER UTR_MST_TemplateMaster_Audit
--ALTER TABLE MST_UserMaster ENABLE TRIGGER UTR_MST_UserMaster_Audit
--ALTER TABLE MST_UserRoleMapping ENABLE TRIGGER UTR_MST_UserRoleMapping_Audit
--ALTER TABLE TRN_NotificationHistory ENABLE TRIGGER UTR_TRN_NotificationHistory_Audit


CREATE or Alter Procedure [dbo].[USPT_CreateTrigger](  
@TableName NVARCHAR(500) 
)                                                
As                                                
BEGIN    
Declare @sql as nvarchar(MAX)  
   
set @sql = 'CREATE OR ALTER TRIGGER UTR_'+@TableName+'_Audit ON '+@TableName+'  
FOR INSERT, UPDATE, DELETE  
AS  
  
DECLARE @AuditAction CHAR(1)  
Declare @TableName nvarchar(500)  
Declare @PrimaryKeyColumnName nvarchar(500)  
  
Declare @OLDData nvarchar(max) =''''  
Declare @NewData nvarchar(max) =''''  
Declare @AuditUser nvarchar(100) =''''  
Declare @AuditUserRole nvarchar(1000) =''''  
  
SELECT @TableName       =   OBJECT_NAME(parent_id)  
        FROM sys.triggers  
        WHERE object_id         =   @@PROCID;  
  
if @TableName is null return  
  
IF EXISTS (SELECT 0 FROM inserted)  
BEGIN  
   IF EXISTS (SELECT 0 FROM deleted)  
   BEGIN   
      SET @AuditAction = ''U''  
   set @OLDData = (SELECT * FROM Deleted FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)  
    set @NewData =  (SELECT * FROM Inserted FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)  
     if(@NewData is null) return  
  if(@OLDData is null) return  
  
  IF COL_LENGTH(@TableName, ''ModifiedBy'') IS NOT NULL  
      BEGIN  
    -- Column Exists  
    SELECT  Top 1 @AuditUser = ModifiedBy  FROM Inserted   
      END  
  
   END ELSE  
   BEGIN  
      SET @AuditAction = ''I''  
     set @NewData =  (SELECT * FROM Inserted FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)  
   if(@NewData is null) return  
    IF COL_LENGTH(@TableName, ''CreatedBy'') IS NOT NULL  
      BEGIN  
    -- Column Exists  
    SELECT Top 1 @AuditUser = CreatedBy  FROM Inserted   
      END  
   END  
END   
  
ELSE   
BEGIN  
   SET @AuditAction = ''D''  
      set @OLDData = (SELECT * FROM Deleted FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)  
   if(@OLDData is null) return  
  
   --SET @AuditUser = ''PFCSupportTeam''  
   -- SET @AuditUserRole = ''''  
     
  
  
END   
  
IF @AuditAction IS NULL RETURN;  
  
If(Isnull(@AuditUser,'''') != '''')  
begin  
SELECT   
@AuditUserRole = STRING_AGG(R.RoleName, '', '')  
from  dbo.MST_UserRoleMapping UM  
inner join dbo.MST_Roles R on R.RoleID = UM.RoleID  
where UserSESA = @AuditUser and UM.isActive =1 and R.isActive=1  
End  
else  
begin  
SET @AuditUser = ''PFCSupportTeam''  
SET @AuditUserRole = ''''  
end  
  
 INSERT INTO [TRN_AuditLog] (  
        [TableName],  
  [AuditAction],  
        OldRowData,  
        NewRowData,  
  AuditUser,  
  AuditUserRole  
    )  
    VALUES(  
        (@TableName),  
  @AuditAction,  
        @OLDData,  
        @NewData,  
  isnull(@AuditUser,''''),  
  isnull(@AuditUserRole,'''')  
    );'  
  
  
  
  
EXEC(@sql)  
  
  
               
END  



















