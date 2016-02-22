USE OrigosVCPalace
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Layout del carta de rembolsos (Refund)
** 
** [LorMartinez]	25/Nov/2015
**
*/

declare @Report varchar(40)
set @Report = 'RefundLetter'
delete from ReportsTexts where reReport = @Report

set quoted_identifier off
GO

exec dbo.USP_OR_AddReportText @Report, "Header", 
"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fswiss\fprq2\fcharset0 Calibri;}}
{\*\generator Msftedit 5.41.21.2510;}\viewkind4\uc1\pard\sa200\sl276\slmult1\lang10\f0\fs22 Sala de Ventas:  \b [SaleRoom] \b0\par
Rembolso No. \b [RefundID]\tab\b0\tab\tab\tab\tab\tab Fecha:\b  [RefundDate]\b0\par
Folio: \b [RefundFolio] \par
\b0 Guest ID:\b  [GuestID]\par
\lang2058\b0\f1\fs24 Folio de invitaci\'f3n Outhouse\lang10\f0\fs22 : \b [OutInvt]   \par
\b0\par
A quien corresponda:\par
Por medio del presente escrito y toda vez de que a nuestros intereses as\'ed convienen, nosotros: \b [GuestNames]\b0  solicitamos a PALACE ELITE RESORTS S.A. DE C.V. el reembolso de $[TotalAmount] d\'f3lares que dejamos como dep\'f3sito en el Aeropuerto con el promotor de nombre [PRName].\par
Deacuerdo a lo anterior se hace contatar que en este acto, nos ha(n) sido rembolsada(s) la(s) siguiente(s) cantidad(es):\par
}",
"{\rtf1\ansi\ansicpg1252\deff0\deflang2058\deflangfe2058{\fonttbl{\f0\fswiss\fprq2\fcharset0 Calibri;}{\f1\fnil\fcharset0 Calibri;}}
{\*\generator Msftedit 5.41.21.2510;}\viewkind4\uc1\pard\nowidctlpar\sa200\sl276\slmult1\lang10\f0\fs22 Sale Room:  \b [SaleRoom] \par
\b0 Refund ID:\b  [RefundID]\tab\b0\tab\tab\tab\tab\tab\tab Date:\b  [RefundDate]\par
\b0 Folio: \b [RefundFolio] \par
\b0 Guest ID: \b [GuestID]\par
\lang2058\b0 Outhouse Invitation Folio\lang1033 : \b [OutInvt]   \par
\b0\par
To whom it may concern,\par
By this means, and being in our own best interest, me/ we \b [GuestNames]\b0  require from PALACE ELITE RESORTS S.A. DE C.V., the refund of my deposit in the amount of [TotalAmount] . This deposit was given to the representative [PRName] at the Airport.   \par
\pard\lang2058\f1 Based on the previous information, It is confirmed in this act that the following amount has been refunded\lang1033\f0\par
}",
"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fprq2\fcharset0 Calibri;}{\f1\fnil\fcharset0 Calibri;}}
{\*\generator Msftedit 5.41.21.2510;}\viewkind4\uc1\pard\sa200\sl276\slmult1\lang2058\f0\fs22 Sala de exposi\'e7\'e3o: \lang10\b\f1 [SaleRoom] \b0\par
 \lang2058\f0 N\'famero reembolso:\lang10\b\f1  [RefundID]\b0\tab\tab\tab\tab\tab Data: \b [RefundDate]\par
\b0 Folio: \b [RefundFolio] \par
\b0 Guest ID: \b [GuestID]\par
\lang2058\b0\f0 Outhouse convite Folio\lang10\f1 n \'ba: \b [OutInvt]\par
\b0\par
A quien corresponda:\par
Por meio deste documento e de acordo com nosso interesse, n\'f3s: \b [GuestNames]\b0 , solicitamos a PALACE ELITE RESORTS S.A. DE C.V. o reembolso de $[TotalAmount] d\'f3lares deixados como dep\'f3sito no Aeropuerto com o promotor [PRName].\par
\lang1046 De acordo ao anterior se faz constar que neste ato, nos foi reembolsado os seguinte(s) valore(s)\lang10\par
}", 1
GO

exec dbo.USP_OR_AddReportText "RefundLetter", "Footer", 
"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Calibri;}}
{\*\generator Msftedit 5.41.21.2510;}\viewkind4\uc1\pard\sa200\sl276\slmult1\lang10\f0\fs22 Esta cantidad se vera reflejada en nuestra cuenta en aproximadamente 15 d\'edas h\'e1biles a partir de la presente fecha. \par
Por lo anteriormente se\'f1alado, manifestamos nuestra total satisfacci\'f3n, sin que de modo alguno nos reservemos alguna acci\'f3n presente o futura en contra de PALACE ELITE RESORTS, S.A DE C.V.\par
\par
ATENTAMENTE:\par
\par
}",
"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fprq2\fcharset0 Calibri;}{\f1\fnil\fcharset0 Calibri;}}
{\*\generator Msftedit 5.41.21.2510;}\viewkind4\uc1\pard\nowidctlpar\sa200\sl276\slmult1\f0\fs22 We understand that this amount will be reflected in our account in approximately 15 business days from this date.\par
\pard\sa200\sl276\slmult1\lang10\f1 According to this request our declining each and every action present or future against Palace Elite Resorts, S.A. de C.V. by this issue.\par
\par
\tab\tab\tab\tab We here by protest truly:\par
\par
}",
"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Calibri;}}
{\*\generator Msftedit 5.41.21.2510;}\viewkind4\uc1\pard\sa200\sl276\slmult1\lang10\f0\fs22 Entendemos o qual ser\'e1 depositado em nossa conta em ate 15 dias \'fateis a partir de esta data. \par
Pelo anteriormente mencionado, manifestamos nossa satisfa\'e7\'e3o plena, e de modo algum nos reservamos qualquer a\'e7\'e3o presente ou futura contra ELITE PALACE RESORTS, SA DE CV. \par
\par
\tab\tab\tab\tab Atenciosamente,\par
\par
}", 1
GO

set quoted_identifier on
GO

