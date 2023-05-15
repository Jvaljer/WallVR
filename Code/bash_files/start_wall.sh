#!/bin/bash
EXEC="../WallBase_exe.x86_64"
WALL="WILDER"
#additional parameters
PART_ID="p"
MASTER_ID="m"
PART_AMOUNT=10

#executing for the participants
for $col in {a,b}
do
    for $row in {1..5}
    do  
        echo "executing for wild@$col$row"
        ./$EXEC -popupwindow -wall $WALL -r $PART_ID -x $col -y $row -fs 1 &
    done 
done
