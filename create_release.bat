@echo off 
cls
if "%1"=="" goto ERR1
md "Release-%1"
md "Release-%1\PGORMWizard"
md "Release-%1\PGORM"

xcopy .\PGORMWizard\bin\Release\*.* "Release-%1\PGORMWizard" /s
xcopy .\PGORM\bin\Release\*.* "Release-%1\PGORM" /s

del ".\Release-%1\PGORMWizard\*.pdb"
del ".\Release-%1\PGORM\*.pdb"

del ".\Release-%1\*vshost*" /F/S



goto END1
:ERR1
echo provide the release number.
:END1