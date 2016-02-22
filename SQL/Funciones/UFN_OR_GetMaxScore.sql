if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetMaxScore]') and xtype in (N'FN', N'if', N'TF'))
drop function [dbo].[UFN_OR_GetMaxScore]
GO

set QUOTED_IDENTIFIER ON 
GO
set ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene la regla que tiene la puntuación más alta de una lista de puntuaciones
**
** [wtorres]	09/Dic/2010 Created
** [wtorres]	22/Ene/2011 Agregué las puntuaciones 7 y 8
** [lchairez]	19/Mar/2014 Agregué las puntuaciones 9,10 Y 11
*/
create function [dbo].[UFN_OR_GetMaxScore] (
	@Score1 money,
	@Score2 money,
	@Score3 money,
	@Score4 money,
	@Score5 money,
	@Score6 money,
	@Score7 money,
	@Score8 money,
	@Score9 money,
	@Score10 money,
	@Score11 money
)
returns money
as
begin

declare @MaxScore money

-- Tabla de puntuaciones
declare @TableScores table (
	Score money
)

-- Agrega las puntuaciones
insert into @TableScores (Score) values (@Score1)
insert into @TableScores (Score) values (@Score2)
insert into @TableScores (Score) values (@Score3)
insert into @TableScores (Score) values (@Score4)
insert into @TableScores (Score) values (@Score5)
insert into @TableScores (Score) values (@Score6)
insert into @TableScores (Score) values (@Score7)
insert into @TableScores (Score) values (@Score8)
insert into @TableScores (Score) values (@Score9)
insert into @TableScores (Score) values (@Score10)
insert into @TableScores (Score) values (@Score11)

set @MaxScore = (select Max(Score) from @TableScores)

return @MaxScore
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

