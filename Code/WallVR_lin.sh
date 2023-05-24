#!/bin/bash
EXEC_LIN="WallVR_exe.x86_64"
EXEC_WIN="WallVR.exe"
WALL="DESKTOP"
PART_VR_ID="pvr"
PART_2D_ID="p2d"
MASTER_ID="m"

PART_AMOUNT=3 #2 2D + 1 VR
MASTER_ONLY=0

#first executing for operator
echo "executing for operator -> 2D linux"
./$EXEC_LIN -popupwindow -screen-fullscreen 0 -screen-width 1024 -screen-height 256 -wall $WALL -sw 1024 -sh 256 -r $MASTER_ID -pa $PART_AMOUNT -mo $MASTER_ONLY -logfile log_m.txt &

sleep 5

if [ $MASTER_ONLY == 0 ]; then
    #executing for 2 participant
    ./$EXEC -popupwindow -screen-fullscreen 0 -screen-width 512 -screen-height 256 -wall $WALL -sw 512 -sh 256 -r $PART_ID -x 0 -y 0 -logfile log_p1.txt &
    ./$EXEC -popupwindow -screen-fullscreen 0 -screen-width 512 -screen-height 256 -wall $WALL -sw 512 -sh 256 -r $PART_ID -x 512 -y 0 -logfile log_p2.txt &
fi