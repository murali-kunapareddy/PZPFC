

-- EXEC USPT_NullMasterTableKeyFieldsRemove
CREATE or Alter PROCEDURE [dbo].[USPT_NotNullMasterTableKeyFieldsRemove]
AS
BEGIN
	Alter table A507 alter column    KAPPL NVARCHAR(100) null
	Alter table A507 alter column    KSCHL NVARCHAR(100) null
	Alter table A507 alter column    VKORG NVARCHAR(100) null
	Alter table A507 alter column    VTWEG NVARCHAR(100) null
	Alter table A507 alter column    SPART NVARCHAR(100) null
	Alter table A507 alter column    PLTYP NVARCHAR(100) null
	Alter table A507 alter column    MATNR NVARCHAR(100) null
	Alter table A507 alter column    KFRST NVARCHAR(100) null
	Alter table A507 alter column    DATBI DATETIME null

	Alter table A604 alter column    KAPPL NVARCHAR(100) null
	Alter table A604 alter column    KSCHL NVARCHAR(100) null
	Alter table A604 alter column    VKORG NVARCHAR(100) null
	Alter table A604 alter column    VTWEG NVARCHAR(100) null
	Alter table A604 alter column    SPART NVARCHAR(100) null
	Alter table A604 alter column    PLTYP NVARCHAR(100) null
	Alter table A604 alter column    KONDA NVARCHAR(100) null
	Alter table A604 alter column    MATNR NVARCHAR(100) null
	Alter table A604 alter column    KFRST NVARCHAR(100) null
	Alter table A604 alter column    DATBI DATETIME null

	Alter table A606 alter column    KSCHL NVARCHAR(100)  null
	Alter table A606 alter column    VKORG NVARCHAR(100)  null
	Alter table A606 alter column    VTWEG NVARCHAR(100)  null
	Alter table A606 alter column    SPART NVARCHAR(100)  null
	Alter table A606 alter column    KUNNR NVARCHAR(100)  null
	Alter table A606 alter column    PRODH1 NVARCHAR(100)  null
	Alter table A606 alter column    PRODH2 NVARCHAR(100)  null
	Alter table A606 alter column    PRODH3 NVARCHAR(100)  null
	Alter table A606 alter column    KFRST NVARCHAR(100)  null
	Alter table A606 alter column    DATBI DATETIME null

	Alter table A607 alter column    KSCHL NVARCHAR(100)  null
	Alter table A607 alter column    VKORG NVARCHAR(100)  null
	Alter table A607 alter column    VTWEG NVARCHAR(100)  null
	Alter table A607 alter column    SPART NVARCHAR(100)  null
	Alter table A607 alter column    HIENR NVARCHAR(100)  null
	Alter table A607 alter column    PRODH1 NVARCHAR(100)  null
	Alter table A607 alter column    PRODH2 NVARCHAR(100)  null
	Alter table A607 alter column    PRODH3 NVARCHAR(100)  null
	Alter table A607 alter column    KFRST NVARCHAR(100)  null
	Alter table A607 alter column    DATBI DATETIME null

	Alter table A608 alter column    KSCHL NVARCHAR(100)  null
	Alter table A608 alter column    VKORG NVARCHAR(100)  null
	Alter table A608 alter column    VTWEG NVARCHAR(100)  null
	Alter table A608 alter column    SPART NVARCHAR(100)  null
	Alter table A608 alter column    ZKVGR1 NVARCHAR(100)  null
	Alter table A608 alter column    YYKVGR2 NVARCHAR(100)  null
	Alter table A608 alter column    YYKVGR3 NVARCHAR(100)  null
	Alter table A608 alter column    PRODH1 NVARCHAR(100)  null
	Alter table A608 alter column    PRODH2 NVARCHAR(100)  null
	Alter table A608 alter column    PRODH3 NVARCHAR(100)  null

	Alter table A609 alter column    KAPPL NVARCHAR(100)  null
	Alter table A609 alter column    KSCHL NVARCHAR(100)  null
	Alter table A609 alter column    VKORG NVARCHAR(100)  null
	Alter table A609 alter column    VTWEG NVARCHAR(100)  null
	Alter table A609 alter column    SPART NVARCHAR(100)  null
	Alter table A609 alter column    PRODH1 NVARCHAR(100)  null
	Alter table A609 alter column    PRODH2 NVARCHAR(100)  null
	Alter table A609 alter column    PRODH3 NVARCHAR(100)  null
	Alter table A609 alter column    KFRST NVARCHAR(100)  null
	Alter table A609 alter column    DATBI DATETIME null

	Alter table A652 alter column    KAPPL NVARCHAR(100)  null
	Alter table A652 alter column    KSCHL NVARCHAR(100)  null
	Alter table A652 alter column    VKORG NVARCHAR(100)  null
	Alter table A652 alter column    VTWEG NVARCHAR(100)  null
	Alter table A652 alter column    SPART NVARCHAR(100)  null
	Alter table A652 alter column    KUNNR NVARCHAR(100)  null
	Alter table A652 alter column    MATNR NVARCHAR(100)  null
	Alter table A652 alter column    KFRST NVARCHAR(100)  null
	Alter table A652 alter column    DATBI DATETIME null

	Alter table A653 alter column    KSCHL NVARCHAR(100)  null
	Alter table A653 alter column    VKORG NVARCHAR(100)  null
	Alter table A653 alter column    VTWEG NVARCHAR(100)  null
	Alter table A653 alter column    SPART NVARCHAR(100)  null
	Alter table A653 alter column    ZKVGR1 NVARCHAR(100)  null
	Alter table A653 alter column    YYKVGR2 NVARCHAR(100)  null
	Alter table A653 alter column    YYKVGR3 NVARCHAR(100)  null
	Alter table A653 alter column    MATNR NVARCHAR(100)  null
	Alter table A653 alter column    KFRST NVARCHAR(100)  null
	Alter table A653 alter column    DATBI DATETIME null

	Alter table A655 alter column    KAPPL NVARCHAR(100)  null
	Alter table A655 alter column    KSCHL NVARCHAR(100)  null
	Alter table A655 alter column    VKORG NVARCHAR(100)  null
	Alter table A655 alter column    VTWEG NVARCHAR(100)  null
	Alter table A655 alter column    SPART NVARCHAR(100)  null
	Alter table A655 alter column    MATNR NVARCHAR(100)  null
	Alter table A655 alter column    KFRST NVARCHAR(100)  null
	Alter table A655 alter column    DATBI DATETIME null

	Alter table A657 alter column    KAPPL NVARCHAR(100)  null
	Alter table A657 alter column    KSCHL NVARCHAR(100)  null
	Alter table A657 alter column    VKORG NVARCHAR(100)  null
	Alter table A657 alter column    VTWEG NVARCHAR(100)  null
	Alter table A657 alter column    SPART NVARCHAR(100)  null
	Alter table A657 alter column    HIENR NVARCHAR(100)  null
	Alter table A657 alter column    MATNR NVARCHAR(100)  null
	Alter table A657 alter column    KFRST NVARCHAR(100)  null
	Alter table A657 alter column    DATBI DATETIME null

	Alter table A979 alter column    KAPPL NVARCHAR(100)  null
	Alter table A979 alter column    KSCHL NVARCHAR(100)  null
	Alter table A979 alter column    VKORG NVARCHAR(100)  null
	Alter table A979 alter column    VTWEG NVARCHAR(100)  null
	Alter table A979 alter column    SPART NVARCHAR(100)  null
	Alter table A979 alter column    YYKVGR3 NVARCHAR(100)  null
	Alter table A979 alter column    PRODH1 NVARCHAR(100)  null
	Alter table A979 alter column    PRODH2 NVARCHAR(100)  null
	Alter table A979 alter column    PRODH3 NVARCHAR(100)  null
	Alter table A979 alter column    KFRST NVARCHAR(100)  null

	Alter table A996 alter column    KSCHL NVARCHAR(100)  null
	Alter table A996 alter column    VKORG NVARCHAR(100)  null
	Alter table A996 alter column    VTWEG NVARCHAR(100)  null
	Alter table A996 alter column    SPART NVARCHAR(100)  null
	Alter table A996 alter column    YYKVGR3 NVARCHAR(100)  null
	Alter table A996 alter column    KONDA NVARCHAR(100)  null
	Alter table A996 alter column    PRODH1 NVARCHAR(100)  null
	Alter table A996 alter column    PRODH2 NVARCHAR(100)  null
	Alter table A996 alter column    PRODH3 NVARCHAR(100)  null
	Alter table A996 alter column    DATBI NVARCHAR(100)  null

	Alter table Customer_Hierarchy_cust alter column KUNNR NVARCHAR(100) null
	Alter table Customer_Hierarchy_cust alter column VKORG NVARCHAR(100) null
	Alter table Customer_Hierarchy_cust alter column VTWEG NVARCHAR(100) null
	Alter table Customer_Hierarchy_cust alter column SPART NVARCHAR(100) null

	Alter table KNA1 alter column    KUNNR NVARCHAR(100)  null

	Alter table KNVV alter column    KUNNR NVARCHAR(100)  null
	Alter table KNVV alter column    VKORG NVARCHAR(100)  null
	Alter table KNVV alter column    VTWEG NVARCHAR(100)  null
	Alter table KNVV alter column    SPART NVARCHAR(100)  null

	Alter table KONM_sum alter column    KNUMH NVARCHAR(100)  null

	Alter table KONP alter column    KNUMH NVARCHAR(100)  null
	Alter table KONP alter column    KOPOS NVARCHAR(100)  null

	Alter table MAKT alter column    MATNR NVARCHAR(100)  null

	Alter table MARA alter column    MATNR NVARCHAR(100)  null	

	Alter table MARC alter column    MATNR NVARCHAR(100)  null
	Alter table MARC alter column    WERKS NVARCHAR(100)  null

	Alter table MARM_sum alter column    MATNR NVARCHAR(100)  null	

	Alter table MVKE alter column    MATNR NVARCHAR(100)  null
	Alter table MVKE alter column    VKORG NVARCHAR(100)  null
	Alter table MVKE alter column    VTWEG NVARCHAR(100)  null

	Alter table T006A alter column    MSEHI NVARCHAR(100)  null

END
GO


