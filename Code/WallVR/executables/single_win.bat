@echo off

set EXEC=Single_WallVR/Single_WallVR.exe
set WALL=DESKTOP
set PID=p
set SW=1980
set SH=1080

set LOG=0

if %LOG%==0
	(start "" "%EXEC%" -vr 1 -popupwindow -screen-fullscreen 0 -screen-width %SW% -screen-height %SH% -sw %SW% -sh %SH% -r %PID%)
else 
	(start "" "%EXEC%" -vr 1 -popupwindow -screen-fullscreen 0 -screen-width %SW% -screen-height %SH% -sw %SW% -sh %SH% -r %PID% -logfile log_vr.txt)
