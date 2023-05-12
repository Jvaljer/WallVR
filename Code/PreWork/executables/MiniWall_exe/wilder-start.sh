#!/bin/bash

LOGIN="wild"
PROGHOME="/media/ssd/Demos/MiniWall"
EXEC="MiniWall.x86_64"
LIP="145"
WALL="WILDER"
accum_rest=0

for i in "$@"
do
case $i in
    -n=*)
      if [ $accum_rest == 1 ]; 
        then REST="$REST $i";
      else
    	 NAME="${i#*=}"
      fi
    ;;
    -e=*)
      if [ $accum_rest == 1 ]; 
        then REST="$REST $i";
      else
    	   EXEC="${i#*=}"
      fi
    ;;
    -ip=*)
      if [ $accum_rest == 1 ]; 
        then REST="$REST $i";
      else
    	   #LIP="192.168.0.${i#*=}"
        LIP="${i#*=}"
      fi
    ;;
    -l=*)
      if [ $accum_rest == 1 ]; 
        then REST="$REST $i";
      else
        LOGIN="${i#*=}"
      fi
    ;;
    --)
  		accum_rest=1
    ;;
    *)
		if [ $accum_rest == 1 ]; 
			then REST="$REST $i";
		fi
    ;;
esac
#shift
done

echo "LOGIN: " $LOGIN
echo "EXEC: " $EXEC
echo "PROGHOME: " $PROGHOME
echo "REST: " $REST
echo "LIP: " $LIP

echo "./$EXEC -screen-width 1440 -screen-height 480 -cw 14400 -ch 4800 -logfile log.txt &"
./$EXEC -screen-width 1440 -screen-height 480 -cw 14400 -ch 4800 -nc 10 -wall $WALL -logfile log.txt $REST &
sleep 1


function colNum {
  case "$1" in
          "a" ) return 0;;
          "b" ) return 2;;
  esac
}

function colIP {
  case "$1" in
          "a" ) return 0;;
          "b" ) return 1;;
  esac
}

function startX {
  case "$1" in 
      "a" ) return 0;;
      "b" ) return 1;;
  esac
}


WIDTH=7680
function setWidth {
  case "$1" in 
      "a" )  WIDTH=7680;;
      "b" )  WIDTH=6720;;
  esac
}

for col in {a..b}
do
    for row in {1..5}
    do
        ROW0=`expr $row - 1`
        Y=`expr $ROW0 \* 960`
        startX $col
        X=`expr $? \* 7680`
        colIP $col
        startIp=`expr $? + 1`
        startIp=`expr $startIp \* 10`
        startIp=`expr $startIp + $row` 
        setWidth $col
        #echo "$LOGIN@$col$row"
        #echo "start on \"192.168.2.$startIp\" "
        
        echo "ssh $LOGIN@192.168.2.$startIp cd $PROGHOME ; DISPLAY=:0 ./$EXEC -x $X -y $Y -screen-width $WIDTH -screen-height 960 -cw 14400 -ch 4800 -wall $WALL -logfile log.txt -s \"192.168.2.$LIP\" " $REST
        ssh $LOGIN@192.168.2.$startIp -o UserKnownHostsFile=/dev/null -o StrictHostKeyChecking=no "cd $PROGHOME ; DISPLAY=:0 ./$EXEC -x $X -y $Y -popupwindow -screen-fullscreen 0 -screen-width $WIDTH -screen-height 960 -cw 14400 -ch 4800 -wall $WALL -logfile log.txt" -s \"192.168.2.$LIP\" $REST &
        #exit
		#sleep 1
        
      done
done        
