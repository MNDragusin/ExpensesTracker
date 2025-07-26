@echo off
setlocal enabledelayedexpansion

set "BASE=src"

if not exist "%BASE%" (
    echo Folder '%BASE%' does not exist.
    goto :eof
)

echo Looking for 'bin' and 'obj' folders inside immediate subfolders of '%BASE%'...

for /d %%D in ("%BASE%\*") do (
    if exist "%%D\bin" (
        echo Deleting: %%D\bin
        rmdir /s /q "%%D\bin"
    )
    if exist "%%D\obj" (
        echo Deleting: %%D\obj
        rmdir /s /q "%%D\obj"
    )
)

echo Done.
pause