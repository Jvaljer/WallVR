#!/bin/bash
EXEC="WallBase_exe.x86_64"
WALL="DESKTOP"
PART_ID="p"

./$EXEC -popupwindow -screen-fullscreen 0 -screen-width 512 -screen-height 256 -0 -wall $WALL -sw 512 -sh 256 -r $PART_ID -x 0 -y 0 -logfile log_p1.txt &
./$EXEC -popupwindow -screen-fullscreen 0 -screen-width 512 -screen-height 256 -0 -wall $WALL -sw 512 -sh 256 -r $PART_ID -x 512 -y 0 -logfile log_p2.txt &
