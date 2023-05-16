#!/bin/bash
EXEC="../WallBase_exe.x86_64"
WALL="DESKTOP"
#additional parameters
PART_ID="p"
MASTER_ID="m"
PART_AMOUNT=2

#executing for operator
echo "executing for ope"
./$EXEC -popupwindow -screen-fullscreen 0 -screen-width 1024 -screen-height 256 -wall $WALL -sw 1024 -sh 256 -r $MASTER_ID -pa $PART_AMOUNT -logfile log_m.txt &

sleep 2
input="start"

#waiting for the user to start all other programs
#read -p "type 'start' to launch all participants, or 'cancel' to abort -> " input
#while [ "$input" != "start" && "$input" != "cancel" ]; do
#    read -p "invalid input, type 'start' to launch or 'cancel' to abort -> " input
#done

#executing for the participants
if [ $input == "start" ]; then 
    ./$EXEC -popupwindow -screen-fullscreen 0 -screen-width 512 -screen-height 256 -0 -wall $WALL -sw 512 -sh 256 -r $PART_ID -x 0 -y 0 &
    ./$EXEC -popupwindow -screen-fullscreen 0 -screen-width 512 -screen-height 256 -0 -wall $WALL -sw 512 -sh 256 -r $PART_ID -x 512 -y 0 &
  
elif [ $input == "cancel" ]; then 
    exit
fi
