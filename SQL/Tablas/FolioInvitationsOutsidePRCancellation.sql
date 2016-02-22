USE OrigosVCPalace
GO

IF OBJECT_ID ('dbo.FolioInvitationsOutsidePRCancellation') IS NOT NULL
	DROP TABLE dbo.FolioInvitationsOutsidePRCancellation
GO

CREATE TABLE dbo.FolioInvitationsOutsidePRCancellation
	(
	ficID    INT IDENTITY NOT NULL,
	ficpe    VARCHAR (10) NOT NULL,
	ficSerie VARCHAR (5) NOT NULL,
	ficFrom  INT NOT NULL,
	ficTo    INT NOT NULL,
	ficrcf   VARCHAR (3) NOT NULL,
	CONSTRAINT PK__FolioInvitations__03E74202 PRIMARY KEY (ficID)
	WITH (FILLFACTOR = 100),
	CONSTRAINT fk_FolioInvOutCnx_peID FOREIGN KEY (ficpe) REFERENCES dbo.Personnel (peID),
	CONSTRAINT fk_FolioCnxReason FOREIGN KEY (ficrcf) REFERENCES dbo.ReasonCancellationFolios (rcfID)
	)
GO

EXEC sp_ADDextendedproperty 
@name = N'MS_Description', @value = 'ID de FolioIncitationOUTPR Cancelados',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'FolioInvitationsOutsidePRCancellation',
@level2type = N'Column', @level2name = 'ficID'
GO

EXEC sp_ADDextendedproperty 
@name = N'MS_Description', @value = 'llave foranea de FolioInvtOutPRCancellation con PR',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'FolioInvitationsOutsidePRCancellation',
@level2type = N'Column', @level2name = 'ficpe'
GO


EXEC sp_ADDextendedproperty 
@name = N'MS_Description', @value = 'Serie de los folios',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'FolioInvitationsOutsidePRCancellation',
@level2type = N'Column', @level2name = 'ficSerie'
GO

EXEC sp_ADDextendedproperty 
@name = N'MS_Description', @value = 'Inicio de rango de folios',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'FolioInvitationsOutsidePRCancellation',
@level2type = N'Column', @level2name = 'ficFrom'
GO

EXEC sp_ADDextendedproperty 
@name = N'MS_Description', @value = 'Fin de rango de folios',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'FolioInvitationsOutsidePRCancellation',
@level2type = N'Column', @level2name = 'ficTo'
GO

EXEC sp_ADDextendedproperty 
@name = N'MS_Description', @value = 'llave foranera de FolioInvtOutPRCancellation con ReasonCancellationFolios',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'FolioInvitationsOutsidePRCancellation',
@level2type = N'Column', @level2name = 'ficrcf'
GO