/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Textos de las leyendas de productos
** 
** [wtorres]	12/Jun/2013 Creado
**
*/
use OrigosVCPalace

delete from ProductsLegends
-- Spa Credit
exec USP_OR_AddProductLegend 'SPACRED',
'{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang3082\deflangfe3082\deftab708{\fonttbl{\f0\fnil\fprq2\fcharset2 Wingdings;}{\f1\fswiss\fprq2\fcharset0 Arial;}{\f2\froman\fprq2\fcharset0 Times New Roman;}}
{\colortbl ;\red0\green0\blue0;}
{\*\generator Riched20 6.3.9600}\viewkind4\uc1 
\pard\fi-284\li284\tx284\cf1\f0\fs16 o\f1\tab Se utiliza solo para tratamientos de Spa\par
\f0 o\f1\tab No aplican para cargos extras a la habitaci\''f3n\par
\f0 o\f1\tab No aplica para tratamientos de Spa que ya hayan sido utilizados antes de la presentaci\''f3n\cf0\lang2058\par
\cf1\f0\lang3082 o\f1\tab\cf0 El Spa credit aplica en tratamientos de spa reservados directamente en los escritorios de PVP\par
\cf1\f0 o\f1\tab\cf0 No aplica para tratamientos reservados o disfrutados antes de la presentaci\''f3n\par
\cf1\f0 o\f1\tab\cf0 Este cup\''f3n aplica en una sola exhibici\''f3n\par
\cf1\f0 o\f1\tab No aplica para reservaciones hechas a trav\''e9s de telemarketing antes de la llegada\cf0\f2\fs24\par
}',
'{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang3082\deflangfe3082\deftab708{\fonttbl{\f0\fnil\fprq2\fcharset2 Wingdings;}{\f1\fswiss\fprq2\fcharset0 Arial;}}
{\colortbl ;\red0\green0\blue0;}
{\*\generator Riched20 6.3.9600}\viewkind4\uc1 
\pard\fi-284\li284\tx284\cf1\f0\fs16 o\f1\tab Only applies for spa treatments\par
\f0 o\f1\tab Does not apply for room charges\par
\f0 o\f1\tab Does not apply for spa treatment already used\par
\f0 o\f1\tab\cf0 Spa credit applies to treatments reserved directly at the PVP desk\par
\cf1\f0 o\f1\tab\cf0 It does not apply to treatments reserved or used before attending the presentation\par
\cf1\f0 o\tab\cf0\f1 This coupon must be used in a single purchase\par
\cf1\f0 o\f1\tab Does not apply for reservations made trough telemarketing before arrival date\f0\fs20\par
}'
-- Hotel Shop
exec USP_OR_AddProductLegend 'HOTELSHOP',
'{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang3082\deflangfe3082\deftab708{\fonttbl{\f0\fswiss\fprq2\fcharset0 Arial;}{\f1\fnil\fprq2\fcharset2 Wingdings;}{\f2\froman\fprq2\fcharset0 Times New Roman;}}
{\colortbl ;\red0\green0\blue0;}
{\*\generator Riched20 6.3.9600}\viewkind4\uc1 
\pard\cf1\b\f0\fs16 SERA SOLICITADO AL MOMENTO DE LA COMPRA. VALIDO SOLO EN LA TIENDA "HOTEL SHOP".\par
\pard\fi-284\li284\tx284\b0\f1 o\f0\tab Este cup\''f3n aplica en una sola exhibici\''f3n\cf0\f2\fs24\par
}',
'{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang3082\deflangfe3082\deftab708{\fonttbl{\f0\fswiss\fprq2\fcharset0 Arial;}{\f1\fnil\fprq2\fcharset2 Wingdings;}{\f2\froman\fprq2\fcharset0 Times New Roman;}}
{\colortbl ;\red0\green0\blue0;}
{\*\generator Riched20 6.3.9600}\viewkind4\uc1 
\pard\cf1\b\f0\fs16\lang1033 IT WILL BE REQUIRED AT THE MOMENT OF THE TRANSACTION. THIS COUPON IS VALID ONLY AT "THE HOTEL SHOP" STORE.\par
\pard\fi-284\li284\tx284\b0\f1\lang3082 o\f0\tab This coupon must be used in one single payment\cf0\f2\fs24\par
}'
-- Golf Round
exec USP_OR_AddProductLegend 'GOLFROUND',
'{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang3082\deflangfe3082\deftab708{\fonttbl{\f0\fnil\fprq2\fcharset2 Wingdings;}{\f1\fswiss\fprq2\fcharset0 Arial;}}
{\colortbl ;\red0\green0\blue0;}
{\*\generator Riched20 6.3.9600}\viewkind4\uc1 
\pard\fi-284\li284\tx284\cf1\f0\fs16 o\f1\tab\lang1033 Para recibir este servicio ser\''e1 necesario presentar la confirmaci\''f3n de Palace Vacation Planners, misma que ser\''e1 requerida en la recepci\''f3n del campo de golf a su llegada\par
\f0\lang3082 o\f1\tab\lang1033 No aplica para cargos a la habitaci\''f3n\par
\f0\lang3082 o\f1\tab\lang1033 No aplica para reservaciones hechas atrav\''e9s de telemarketing antes de la llegada\f0\lang3082\par
}',
'{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang3082\deflangfe3082\deftab708{\fonttbl{\f0\fnil\fprq2\fcharset2 Wingdings;}{\f1\fswiss\fprq2\fcharset0 Arial;}{\f2\froman\fprq2\fcharset0 Times New Roman;}}
{\colortbl ;\red0\green0\blue0;}
{\*\generator Riched20 6.3.9600}\viewkind4\uc1 
\pard\fi-284\li284\tx284\cf1\f0\fs16 o\f1\tab In order to receive the service a Palace Vacation Planners confirmation will be required upon arrival to the front desk Golf Course\par
\f0 o\f1\tab It does not apply for room charges\par
\f0 o\f1\tab It does not apply for reservations made through telemarketing before arrival date\cf0\f2\fs24\par
}'
-- Store Money
exec USP_OR_AddProductLegend 'STOREMONEY',
'{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang3082\deflangfe3082\deftab708{\fonttbl{\f0\fswiss\fprq2\fcharset0 Arial;}{\f1\fnil\fprq2\fcharset2 Wingdings;}{\f2\froman\fprq2\fcharset0 Times New Roman;}}
{\colortbl ;\red0\green0\blue0;}
{\*\generator Riched20 6.3.9600}\viewkind4\uc1 
\pard\cf1\b\f0\fs16 SERA SOLICITADO AL MOMENTO DE LA COMPRA. VALIDO SOLO EN LA TIENDA "HOTEL SHOP".\par
\pard\fi-284\li284\tx284\b0\f1 o\f0\tab Este cup\''f3n aplica en una sola exhibici\''f3n\cf0\f2\fs24\par
}',
'{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang3082\deflangfe3082\deftab708{\fonttbl{\f0\fswiss\fprq2\fcharset0 Arial;}{\f1\fnil\fprq2\fcharset2 Wingdings;}{\f2\froman\fprq2\fcharset0 Times New Roman;}}
{\colortbl ;\red0\green0\blue0;}
{\*\generator Riched20 6.3.9600}\viewkind4\uc1 
\pard\cf1\b\f0\fs16\lang1033 IT WILL BE REQUIRED AT THE MOMENT OF THE TRANSACTION. THIS COUPON IS VALID ONLY AT "THE HOTEL SHOP" STORE.\par
\pard\fi-284\li284\tx284\b0\f1\lang3082 o\f0\tab This coupon must be used in one single payment\cf0\f2\fs24\par
}'
-- Noche Adicional
exec USP_OR_AddProductLegend 'NREG',
'{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang3082\deflangfe3082\deftab708{\fonttbl{\f0\fnil\fprq2\fcharset2 Wingdings;}{\f1\fswiss\fprq2\fcharset0 Arial;}{\f2\fswiss\fprq2 Arial;}{\f3\froman\fprq2\fcharset0 Times New Roman;}}
{\colortbl ;\red0\green0\blue0;\red0\green0\blue255;}
{\*\generator Riched20 6.3.9600}\viewkind4\uc1 
\pard\fi-284\li284\qj\tx284\cf1\f0\fs16 o\f1\tab Este certificado aplica en un Suite Standard de Luxe con Jacuzzi para dos personas en cualquier Hotel de la cadena Palace Resorts y Le Blanc.\par
\f0 o\f1\tab La ocupaci\''f3n en la Suite Standard de Luxe con Jacuzzi es de m\''ednimo de dos (2) \f2\endash  m\f1\''e1ximo cuatro (4) personas. Adultos o menores adicionales en la misma suite ser\''e1 de acuerdo a la tarifa correspondiente proporcionada al momento de su solicitud por el Departamento de Reservaciones.\par
\f0 o\f1\tab Este certificado ser\''e1 v\''e1lido en cualquier \''e9poca del a\''f1o, excepto la semana de A\''f1o Nuevo, (la semana que incluye las noches del 24 al 31 de diciembre). \par
\f0 o\f1\tab Las reservaciones est\''e1n sujetas a la disponibilidad de espacio.\par
\f0 o\f1\tab No incluye el transporte terrestre desde el Aeropuerto / Hotel / Aeropuerto.\par
\f0 o\f1\tab El certificado original deber\''e1 ser presentado junto con la confirmaci\''f3n del hotel a su llegada en la Recepci\''f3n.\par
\f0 o\f1\tab Este certificado no es transferible a terceros y no se puede canjear por cualquier otro servicio proporcionado por Palace Resorts o Palace Elite.\par
\f0 o\f1\tab La vigencia de este certificado es de un (1) a\''f1o a partir de la fecha de su emisi\''f3n.\par
\f0 o\f1\tab Las reservaciones pagadas deben hacerse a trav\''e9s de la p\''e1gina web: \cf2\ul www.palaceresorts.com\cf1\ulnone .\par
\f0 o\f1\tab Se requiere una estancia minima de tres (3) noches, para validar la noche gratis adicional al final de la estancia. \par
\f0 o\f1\tab La reservaci\''f3n de la noche adicional deber\''e1 solicitarla a la siguiente direcci\''f3n : \cf2\ul ReservacionesEspeciales@palaceresorts.com\cf1\ulnone .\par
\f0 o\f1\tab Si necesita asistencia, le agradecemos nos contacte al correo electr\''f3nico antes mencionado.\cf0\f3\fs24\par
}',
'{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang3082\deflangfe3082\deftab708{\fonttbl{\f0\fnil\fprq2\fcharset2 Wingdings;}{\f1\fswiss\fprq2\fcharset0 Arial;}{\f2\froman\fprq2\fcharset0 Times New Roman;}}
{\colortbl ;\red0\green0\blue0;\red0\green0\blue255;}
{\*\generator Riched20 6.3.9600}\viewkind4\uc1 
\pard\fi-284\li284\qj\tx284\cf1\f0\fs16 o\f1\tab This certificate applies in a Standard de Luxe with Jacuzzi Suite free for two persons any of the Palace Resorts Hotel Chain and Le Blanc.\par
\f0 o\f1\tab The occupancy at the Standard Jacuzzi Suite is minimum two (2) - maximum four (4). Additional adult or children in the same suite will be according to the applicable rate provided by the Reservations Department.\par
\f0 o\f1\tab This certificate will be valid at any time during the year except New\''b4s Year week, (the week which includes the nights from 24th to 31st of December).\par
\f0 o\f1\tab Reservation is subject to the availability of space.\par
\f0 o\f1\tab It does not includes ground transportation from the Airport /Hotel /Airport.\par
\f0 o\f1\tab The original certificate must be presented along with hotel confirmation upon arrival at the front desk.\par
\f0 o\f1\tab This certificate is not transferable to a third party and cannot be exchanged for cash of any other service provided by Palace Resorts or Palace Elite.\par
\f0 o\f1\tab This is valid for one (1) year from the date it was issued.\par
\f0 o\f1\tab Paid reservations must be made thru the web page: {\cf0{\field{\*\fldinst{HYPERLINK www.palaceresorts.com }}{\fldrslt{www.palaceresorts.com\ul0\cf0}}}}\f1\fs16 . A three (3) nights stay is required as minimum to validate the additional complimentary night at the end of the stay.\par
\f0 o\f1\tab Reservation for the additional night must be requested at the following e-mail: \cf2\ul ReservacionesEspeciales@palaceresorts.com\cf1\ulnone .\par
\f0 o\f1\tab If you need assistance , please contact us at the above mentioned e-mail.\cf0\f2\fs24\par
}'