#!/bin/bash
EXEC="../WallBase_exe.x86_64"
WALL="WILDER"
#additional parameters
PART_ID="p"
MASTER_ID="m"
PART_AMOUNT=10

#executing for operator
echo "executing for ope"
./$EXEC -popupwindow -screen-fullscreen 0 -screen-width 1080 -screen-height 360 -wall $WALL -sw 1080 -sh 360 -r $MASTER_ID -pa $PART_AMOUNT

#waiting for the user to start all other programs
read -p "type 'start' to launch all participants, or 'cancel' to abort -> " input
while [ "$input" != "start" && "$input" != "cancel" ]; do
    read -p "invalid input, type 'start' to launch or 'cancel' to abort -> " input
done

#executing for the participants
if [ $input == "start" ]; then 
    for $col in {a,b}
    do
        for $row in {1..5}
        do  
            echo "executing for wild@$col$row"
            ./$EXEC -popupwindow -wall $WALL -r $PART_ID -x $col -y $row -fs 1 &
        done 
    done
elif [ $input == "cancel" ]; then 
    exit
fi
