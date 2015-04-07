@setlocal

@echo OFF
:: BatchGotAdmin
:-------------------------------------
REM  --> Check for permissions
>nul 2>&1 "%SYSTEMROOT%\system32\cacls.exe" "%SYSTEMROOT%\system32\config\system"

REM --> If error flag set, we do not have admin.
if '%errorlevel%' NEQ '0' (
    echo Requesting administrative privileges...
    goto UACPrompt
) else ( goto gotAdmin )

:UACPrompt
    echo Set UAC = CreateObject^("Shell.Application"^) > "%temp%\getadmin.vbs"
    set params = %*:"=""
    echo UAC.ShellExecute "cmd.exe", "/c %~s0 %params%", "", "runas", 1 >> "%temp%\getadmin.vbs"

    "%temp%\getadmin.vbs"
    del "%temp%\getadmin.vbs"
    exit /B

:gotAdmin
    pushd "%CD%"
    CD /D "%~dp0"
:--------------------------------------

echo Attendance Manager Installer Script V1 (c) 2015 
echo Preparing installation...

@SET program=Instructor Attendance Manager
@SET folder=Instructor.Attendance.Manager
@SET renFolder=Instructor Attendance Manager
@SET installPath=C:\Program Files ^(x86^)

IF NOT EXIST %~dp0%folder% GOTO NOEXIST

    @rename "%folder%" "%renFolder%"

    @SET folder=Instructort Attendance Manager
    @SET fullPath=%installPath%\%folder%

    echo Moving Directory into %installPath%
    @move "%~dp0%folder%" "%installPath%"

    echo Granting permissions to Authenticated Users...

    @icacls "%fullPath%" /t /q /grant "Authenticated Users":RXW


    REM Create shortcut using vbscript
    echo Creating shortcut for all users...

    set SCRIPT="%TEMP%\%RANDOM%-%RANDOM%-%RANDOM%-%RANDOM%.vbs"

    echo Set oWS = WScript.CreateObject("WScript.Shell") >> %SCRIPT%
    echo sLinkFile = "%allusersprofile%\Desktop\%program%.lnk" >> %SCRIPT%
    echo Set oLink = oWS.CreateShortcut(sLinkFile) >> %SCRIPT%
    echo oLink.TargetPath = "%fullPath%\%program%.exe" >> %SCRIPT%
    echo oLink.Save >> %SCRIPT%

    cscript /nologo %SCRIPT%
    del %SCRIPT%

    echo Successfully installed
    GOTO END
    
:NOEXIST
    echo %~dp0%folder% doesn't exist


:END
pause

