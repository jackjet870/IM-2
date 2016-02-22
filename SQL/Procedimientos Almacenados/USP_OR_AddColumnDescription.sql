if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_AddColumnDescription]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_AddColumnDescription]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** [LorMartinez] 29/10/2015 Created, Anexa nota de descripcion a una columna de una tabla
*/

CREATE PROCEDURE dbo.USP_OR_AddColumnDescription(
@Schema varchar(15),
@Table varchar(30), 
@Column varchar(30),
@Description varchar(100)
)
AS
BEGIN


EXEC sp_ADDextendedproperty 
@name = N'MS_Description', @value = @Description,
@level0type = N'Schema', @level0name = @Schema,
@level1type = N'Table',  @level1name = @Table,
@level2type = N'Column', @level2name = @Column


Select 'Note description added to the column.'

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

