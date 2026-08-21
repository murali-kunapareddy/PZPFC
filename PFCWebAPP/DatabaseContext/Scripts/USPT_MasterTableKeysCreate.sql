

--EXEC USPT_MasterTableKeysCreate

CREATE or Alter PROCEDURE [dbo].[USPT_MasterTableKeysCreate]
AS
BEGIN
	CREATE TABLE #TempTablesNames(
		TableName varchar(150),
		KeyColumns varchar(500)
	)
	INSERT INTO #TempTablesNames(TableName, KeyColumns) 
		SELECT * FROM (select 'A507', 'KAPPL,   KSCHL,   VKORG,   VTWEG,   SPART,   PLTYP,   MATNR,   KFRST,   DATBI' union all 
					   select 'A604','KAPPL,   KSCHL,   VKORG,   VTWEG,   SPART,   PLTYP,   KONDA,   MATNR,   KFRST,   DATBI' union all 
					   select 'A606', 'KSCHL,   VKORG,   VTWEG,   SPART,   KUNNR,   PRODH1,   PRODH2,   PRODH3,   KFRST,   DATBI' union all 
					   select 'A607' ,'KSCHL,   VKORG,   VTWEG,   SPART,   HIENR,   PRODH1,   PRODH2,   PRODH3,   KFRST,   DATBI' union all 
					   select 'A608' ,'KSCHL,   VKORG,   VTWEG,   SPART,   ZKVGR1,   YYKVGR2,   YYKVGR3,   PRODH1,   PRODH2,   PRODH3' union all 
					   select 'A609' ,'KAPPL,   KSCHL,   VKORG,   VTWEG,   SPART,   PRODH1,   PRODH2,   PRODH3,   KFRST,   DATBI' union all 
					   select 'A652' ,'KAPPL,   KSCHL,   VKORG,   VTWEG,   SPART,   KUNNR,   MATNR,   KFRST,   DATBI' union all 
					   select 'A653', 'KSCHL,   VKORG,   VTWEG,   SPART,   ZKVGR1,   YYKVGR2,   YYKVGR3,   MATNR,   KFRST,   DATBI' union all 
					   select 'A655', 'KAPPL,   KSCHL,   VKORG,   VTWEG,   SPART,   MATNR,   KFRST,   DATBI' union all 
					   select 'A657', 'KAPPL,   KSCHL,   VKORG,   VTWEG,   SPART,   HIENR,   MATNR,   KFRST,   DATBI' union all 
					   select 'A979', 'KAPPL,   KSCHL,   VKORG,   VTWEG,   SPART,   YYKVGR3,   PRODH1,   PRODH2,   PRODH3,   KFRST' union all 
					   select 'A996', 'KSCHL,   VKORG,   VTWEG,   SPART,   YYKVGR3,   KONDA,   PRODH1,   PRODH2,   PRODH3,   DATBI' union all 
					   select 'Customer_Hierarchy_cust', 'KUNNR, VKORG, VTWEG, SPART' union all
					   select 'KNA1', 'KUNNR' union all 
					   select 'KNVV', 'KUNNR,   VKORG,   VTWEG,   SPART' union all 
					   select 'KONM_Sum', 'KNUMH' union all 
					   select 'KONP' ,'KNUMH,   KOPOS' union all 
					   select 'MAKT', 'MATNR'union all 
					   select 'MARA', 'MATNR' union all 
					   select 'MARC', 'MATNR,   WERKS' union all 
					   select 'MARM_Sum', 'MATNR' union all 
					   select 'MVKE','MATNR,   VKORG,   VTWEG' union all 
					   select 'T006A','MSEHI') t(Col1, col2)


	DECLARE @TableName varchar(150)
	DECLARE @KeyCols varchar(500)
	DECLARE db_cursor CURSOR FOR 
		SELECT TableName,KeyColumns  from #TempTablesNames

	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO @TableName , @KeyCols

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		DECLARE @IsPrimary INT

		SELECT @IsPrimary=COUNT(1)
		FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
		WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsPrimaryKey') = 1
		AND TABLE_NAME = @TableName

		IF @IsPrimary > 0
		BEGIN
			PRINT 'PK exists in table ' + @TableName
		END
		ELSE
		BEGIN
			PRINT 'PK not exists in table ' + @TableName + ' creating'
			DECLARE @sqlcmd VARCHAR(MAX)
			SET @sqlcmd = 'ALTER TABLE ' + @TableName + ' ADD CONSTRAINT ' + @TableName + '_PK PRIMARY KEY CLUSTERED (' + @KeyCols + ');';
			EXEC (@sqlcmd);
		END
		FETCH NEXT FROM db_cursor INTO @TableName , @KeyCols
	END 

	CLOSE db_cursor  
	DEALLOCATE db_cursor 
	DROP TABLE #TempTablesNames;
END
GO


