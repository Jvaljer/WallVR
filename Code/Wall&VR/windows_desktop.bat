@echo off
set EXEC=WAllBase.exe
set WALL=DESKTOP
REM aditional parameters
set PID=p
set MID=m
set PA=2
REM master is alone or not
set MO=0

REM executing for operator first
echo exec for ope
start "" "%EXEC%" -popuwindow -screen-fullscreen 0 -screen-width 1024 -screen-height 256 -wall %WALL% -sw 1024 -sh 256 -r %MID% -pa %PA% -mo %MO% -logfile log_win_m.txt
REM "%EXEC%" -popuwindow -screen-fullscreen 0 -screen-width 1024 -screen-height 512 -wall %WALL% -sw 1024 -sh 512 -r %MID% -pa %PA% -mo %MO% -logfile log_win_m.txt &
ping -n 5 127.0.0.1 >nul

IF %MO%==0 (
    REM executing for all participant
    start "" "%EXEC%" -popuwindow -screen-fullscreen 0 -screen-width 512 -screen-height 256 -wall %WALL% -sw 512 -sh 256 -r %PID% -x 0 -y 0 -logfile log_win_p1.txt
    start "" "%EXEC%" -popuwindow -screen-fullscreen 0 -screen-width 512 -screen-height 256 -wall %WALL% -sw 512 -sh 256 -r %PID% -x 512 -y 0 -logfile log_win_p2.txt
    REM "%EXEC%" -popuwindow -screen-fullscreen 0 -screen-width 512 -screen-height 256 -wall %WALL% -sw 512 -sh 256 -r %PID% -x 0 -y 256 -logfile log_win_p3.txt &
    REM "%EXEC%" -popuwindow -screen-fullscreen 0 -screen-width 512 -screen-height 256 -wall %WALL% -sw 512 -sh 256 -r %PID% -x 512 -y 256 -logfile log_win_p4.txt &
)