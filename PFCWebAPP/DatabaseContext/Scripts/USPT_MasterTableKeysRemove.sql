

--EXEC USPT_MasterTableKeysRemove
CREATE or Alter PROCEDURE [dbo].[USPT_MasterTableKeysRemove]
AS
BEGIN
	CREATE TABLE #TempTablesNames(
		TableName varchar(150)
	)
	INSERT INTO #TempTablesNames(TableName) 
		SELECT * FROM (select 'A507' union all select 'A604' union all select 'A606' union all select 'A607' union all select 'A608' 
			union all select 'A609' union all select 'A652' union all select 'A653' union all select 'A655' union all select 'A657' 
			union all select 'A979' union all select 'A996' union all select 'Customer_Hierarchy_cust' union all  select 'KNA1' union all select 'KNVV' union all select 'KONM_Sum' 
			union all select 'KONP' union all select 'MAKT' union all select 'MARA' union all select 'MARC' union all select 'MARM_Sum' 
			union all select 'MVKE' union all select 'T006A') t(Col)


	DECLARE @TableName varchar(150)
	DECLARE db_cursor CURSOR FOR 
		SELECT TableName from #TempTablesNames

	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO @TableName  

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		DECLARE @IsPrimary INT

		SELECT @IsPrimary=COUNT(1)
		FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
		WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsPrimaryKey') = 1
		AND TABLE_NAME = @TableName

		IF @IsPrimary > 0
		BEGIN
			PRINT 'PK exists in table ' + @TableName + ' and deleting'
			DECLARE @sqlcmd VARCHAR(MAX)
			SET @sqlcmd = 'ALTER TABLE ' + QUOTENAME(@TableName) + '  DROP CONSTRAINT ' + @TableName + '_PK;';
			EXEC (@sqlcmd);
		END
		ELSE
		BEGIN
			PRINT 'PK not exists in table ' + @TableName
		END
		FETCH NEXT FROM db_cursor INTO @TableName 
	END 

	CLOSE db_cursor  
	DEALLOCATE db_cursor 
	DROP TABLE #TempTablesNames;
END
GO


