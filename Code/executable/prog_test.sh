#!/bin/bash
EXEC="WallBase_exe.x86_64"
WALL="DESKTOP"
#additional parameters
PART_ID="p"
MASTER_ID="m"
PART_AMOUNT=4
#if the master must be executed alone or not
MASTER_ONLY=0

#executing for operator
echo "executing for ope"
./$EXEC -popupwindow -screen-fullscreen 0 -screen-width 1024 -screen-height 512 -wall $WALL -sw 1024 -sh 512 -r $MASTER_ID -pa $PART_AMOUNT -logfile log_m.txt -mo $MASTER_ONLY &

sleep 5
input="start"

#waiting for the user to start all other programs
#read -p "type 'start' to launch all participants, or 'cancel' to abort -> " input
#while [ "$input" != "start" && "$input" != "cancel" ]; do
#    read -p "invalid input, type 'start' to launch or 'cancel' to abort -> " input
#done

if [ $MASTER_ONLY == 0 ]; then 
	#executing for the participants
	if [ $input == "start" ]; then 
    		./$EXEC -popupwindow -screen-fullscreen 0 -screen-width 512 -screen-height 256 -wall $WALL -sw 512 -sh 256 -r $PART_ID -x 0 -y 0 -logfile log_p1.txt &
    		./$EXEC -popupwindow -screen-fullscreen 0 -screen-width 512 -screen-height 256 -wall $WALL -sw 512 -sh 256 -r $PART_ID -x 512 -y 0 -logfile log_p2.txt &
    		./$EXEC -popupwindow -screen-fullscreen 0 -screen-width 512 -screen-height 256 -wall $WALL -sw 512 -sh 256 -r $PART_ID -x 0 -y 256 -logfile log_p3.txt &
    		./$EXEC -popupwindow -screen-fullscreen 0 -screen-width 512 -screen-height 256 -wall $WALL -sw 512 -sh 256 -r $PART_ID -x 512 -y 256 -logfile log_p4.txt &
  
	elif [ $input == "cancel" ]; then 
    		exit
	fi
fi
