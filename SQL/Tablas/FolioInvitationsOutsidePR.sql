USE ORIGOSVCPALACE
GO

IF OBJECT_ID ('dbo.FolioInvitationsOutsidePR') IS NOT NULL
	DROP TABLE dbo.FolioInvitationsOutsidePR
GO

CREATE TABLE dbo.FolioInvitationsOutsidePR
	(
	fipID    INT IDENTITY NOT NULL,
	fippe    VARCHAR (10) NOT NULL,
	fipSerie VARCHAR (5) NOT NULL,
	fipFrom  INT NOT NULL,
	fipTo    INT NOT NULL,
	CONSTRAINT PK__FolioInvitations__7D3A4473 PRIMARY KEY (fipID)
	WITH (FILLFACTOR = 100),
	CONSTRAINT fk_peID FOREIGN KEY (fippe) REFERENCES dbo.Personnel (peID)
	)
GO

--Descripcion de columnas
EXEC dbo.USP_OR_AddColumnDescription 'dbo', 'FolioInvitationsOutsidePR','fipID','ID de FolioIncitationOUTPR'
EXEC dbo.USP_OR_AddColumnDescription 'dbo', 'FolioInvitationsOutsidePR','fippe','llave foranea de FolioInvtOutPR con PR'
EXEC dbo.USP_OR_AddColumnDescription 'dbo', 'FolioInvitationsOutsidePR','fipSerie','Serie de los folios'
EXEC dbo.USP_OR_AddColumnDescription 'dbo', 'FolioInvitationsOutsidePR','fipFrom','Inicio de rango de folios'
EXEC dbo.USP_OR_AddColumnDescription 'dbo', 'FolioInvitationsOutsidePR','fipTo','Fin de rango de folios'
GO