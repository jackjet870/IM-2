/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Textos del reporte de recibo de regalos
** 
** [wtorres]	19/Jun/2013 Creado
**
*/
use OrigosVCPalace

declare @Report varchar(50)
set @Report = 'GiftsReceipt'
delete from ReportsTexts where reReport = @Report
-- Todos los regalos
exec USP_OR_AddReportText @Report, 'AllGifts',
'{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang3082\deflangfe3082\deftab708{\fonttbl{\f0\fswiss\fprq2\fcharset0 Arial;}{\f1\fnil\fprq2\fcharset2 Wingdings;}}
{\colortbl ;\red0\green0\blue0;}
{\*\generator Riched20 6.3.9600}\viewkind4\uc1 
\pard\cf1\b\f0\fs16 ESTE CUPON ES UNA FORMA DE PAGO. SERA SOLICITADO PARA ACLARAR EL CARGO CORRESPONDIENTE RELACIONADO CON ESTE CUPON. EN CASO DE EXTRAVIO NO ES REEMPLAZABLE.\b0\f1\par
}',
'{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang3082\deflangfe3082\deftab708{\fonttbl{\f0\fswiss\fprq2\fcharset0 Arial;}{\f1\fnil\fprq2\fcharset2 Wingdings;}}
{\colortbl ;\red0\green0\blue0;}
{\*\generator Riched20 6.3.9600}\viewkind4\uc1 
\pard\cf1\b\f0\fs16 THIS COUPON IS A FORM OF PAYMENT. IT WILL BE REQUIRED TO CLEAR CHARGES RELATED TO THIS COUPON. IN CASE OF LOSS WILL NOT BE REPLACED.\b0\f1\par
}'
-- No se cambia por dinero en efectivo. Aplica durante la estancia actual
exec USP_OR_AddReportText @Report, 'NotExchangeableCash_CurrentStay',
'{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang3082\deflangfe3082\deftab708{\fonttbl{\f0\fnil\fprq2\fcharset2 Wingdings;}{\f1\fswiss\fprq2\fcharset0 Arial;}{\f2\froman\fprq2\fcharset0 Times New Roman;}}
{\colortbl ;\red0\green0\blue0;}
{\*\generator Riched20 6.3.9600}\viewkind4\uc1 
\pard\fi-284\li284\tx284\cf1\f0\fs16 o\f1\tab No se cambia por dinero en efectivo\par
\f0 o\f1\tab Aplica durante la estancia actual\cf0\f2\fs24\par
}',
'{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang3082\deflangfe3082\deftab708{\fonttbl{\f0\fnil\fprq2\fcharset2 Wingdings;}{\f1\fswiss\fprq2\fcharset0 Arial;}{\f2\froman\fprq2\fcharset0 Times New Roman;}}
{\colortbl ;\red0\green0\blue0;}
{\*\generator Riched20 6.3.9600}\viewkind4\uc1 
\pard\fi-284\li284\tx284\cf1\f0\fs16 o\f1\tab It is not exchangeable on cash\par
\f0 o\tab\f1 This coupon applies only for the current stay\cf0\f2\fs24\par
}'
-- Reservacion en escritorios de PVP
exec USP_OR_AddReportText @Report, 'ReservationInPVPDesks',
'{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang3082\deflangfe3082\deftab708{\fonttbl{\f0\fnil\fprq2\fcharset2 Wingdings;}{\f1\fswiss\fprq2\fcharset0 Arial;}}
{\colortbl ;\red0\green0\blue0;}
{\*\generator Riched20 6.3.9600}\viewkind4\uc1 
\pard\fi-284\li284\tx284\cf1\f0\fs16 o\f1\tab La reserva se hace directamente en el escritorio de PVP\f0\par
}',
'{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang3082\deflangfe3082\deftab708{\fonttbl{\f0\fnil\fprq2\fcharset2 Wingdings;}{\f1\fswiss\fprq2\fcharset0 Arial;}}
{\colortbl ;\red0\green0\blue0;}
{\*\generator Riched20 6.3.9600}\viewkind4\uc1 
\pard\fi-284\li284\tx284\cf1\f0\fs16 o\f1\tab Applies only at the Palace Vacation Planners desks\f0\par
}'
-- Promocion
exec USP_OR_AddReportText @Report, 'Promotion',
'Tu código de promoción es: [Promotions]',
'Your promotion code is: [Promotions]'
-- Promociones
exec USP_OR_AddReportText @Report, 'Promotions',
'Tus códigos de promoción son: [Promotions]',
'Your promotion codes are: [Promotions]'
-- Noche adicional
exec USP_OR_AddReportText @Report, 'ComplimentaryNight',
'CERTIFICADO PARA UNA NOCHE DE HOSPEDAJE TODO INCLUIDO GRATIS',
'CERTIFICATE FOR ONE (1) COMPLIMENTARY NIGHT'