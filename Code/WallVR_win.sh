EXEC="WallVR.exe"
PART_VR_ID="pvr"

#must check how to exec for windows
./$EXEC -popupwindow -screen-fullscreen 0 -screen-width 512 -screen-height 256 -wall $WALL -sw 512 -sh 256 -r $PART_VR_ID -x 0 -y 0 -vr 0 -logfile log_vr.txt &