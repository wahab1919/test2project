;#DropDB MEIEasyTest

#Application MEI.Win
#Application MEI.Web

*FillForm
 User Name = Admin
*Action Log In

#IfDef MEI.Win

*Action About...
 
*Action OK

#EndIf

#IfDef MEI.Web
*Action Navigation(My Details)
#EndIf
