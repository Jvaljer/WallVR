#!/bin/bash
EXEC="Single_WallVR.x86_64"
WALL="DESKTOP"

MID="m"
PID="p"

PA=1
MASTER_ONLY=0
LOGS=0
VR=0

SW=1024
SH=512

if [ LOGS==1 ]; then
	./$EXEC -vr $VR -popupwindow -screen-fullscreen 0 -screen-width $SW -screen-height $SH -sw $SW -sh $SH -wall $WALL -r $MID -pa $PA -logfile log_m.txt & 
else
	./$EXEC -vr $VR -popupwindow -screen-fullscreen 0 -screen-width $SW -screen-height $SH -sw $SW -sh $SH -wall $WALL -r $MID -pa $PA & 
fi
