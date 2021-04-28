Attribute VB_Name = "Module1"
'#Reference {15571441-435B-11D7-B412-00105A5689F9}#1.0#1#
Dim lvsup As cLVSupport       'Verweis auf LV Support DLL
Dim atl As cAtlas             'Verweis auf die allg. Atlas Klasse



Sub Main()
  Dim N As Integer                        'Lokale Hilfsvariable
  Dim MyKap As Integer                    'Kapitel Nr im Makro
  Dim A$                                  'Hilsfvariable
  Dim tempFolder As String


  Set lvsup = New LVManage8.cLVSupport    'Hole Verweis auf Support Klasse
  Set atl = lvsup.GetObject("LvStress")           'Hole aktulelle Atlas Klasse


'----------

Dim kapName As String
Dim actKap As Integer
Dim kapAnz As Integer
Dim bauform(10) As Integer
Dim kanal As Integer

Call atl.status(actKap, kapAnz, kapName)
For N = 0 To 10
    bauform(N) = atl.CalcType(N, actKap, 1)
Next
tempFolder = CreateObject("WScript.Shell").ExpandEnvironmentStrings("%Temp%") + "\mySource.txt"

Close
kanal = FreeFile
Open tempFolder For Output As #kanal

Print #kanal, "string module =" + kapName + ";"
Print #kanal, "short kap = atlas.CreateModul(module);"
For N = 0 To 10
    Print #kanal, "atlas.CalcType[" + CStr(N) + ", kap, 1] =" + CStr(bauform(N)) + ";"
Next

Print #kanal, "atlas.activateModulNr(kap);"

Close #kanal

MsgBox "Printed to: " + tempFolder

End Sub
