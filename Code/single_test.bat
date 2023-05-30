@echo off
set EXEC=WallVR/Single_WallVR.exe
set WALL=DESKTOP

set PID=p

echo executing for the last part (in VR)
start "" "%EXEC%" -vr 1 -popuwindow -screen-fullscreen 0 -screen-width 1980 -screen-height 780 -wall %WALL% -sw 1980 -sh 780 -r %PID% -logfile log_vr.txt
