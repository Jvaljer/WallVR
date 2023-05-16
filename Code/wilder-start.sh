#!/bin/bash

LOGIN="wild"
PROGHOME="/media/ssd/Demos/WallBase"
EXEC="WallBase_exe.x86_64"
LIP="145"
WALL="WILDER"
accum_rest=0
#additional parameters
PART_ID="p"
MASTER_ID="m"
PART_AMOUNT=10
MASTER_ONLY=1 

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

echo "exec as operator (master) ->"
echo "./EXEC -screen-width 1440 -screen-height 480 -cw 14400 -ch 4800 -pa $PART_AMOUNT -wall $WALL -r m &"
./$EXEC -popupwindow -screen-fullscreen 0 -screen-width 1440 -screen-height 480 -wall $WALL -sw 1440 -sh 480 -r $MASTER_ID -pa $PART_AMOUNT -mo $MASTER_ONLY -logfile log_m.txt &
	
sleep 5

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
HEIGHT=960
function setWidth {
  case "$1" in 
    "a" )  WIDTH=7680;;
    "b" )  WIDTH=6720;;
  esac
}

if [[ $MASTER_ONLY == 0 ]]; then
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
      
            echo "executing based command line"
            ssh $LOGIN@192.168.2.$startIp -o UserKnownHostsFile=/dev/null -o StrictHostKeyChecking=no "cd $PROGHOME ; DISPLAY=:0 ./$EXEC -x $X -y $Y -popupwindow -screen-fullscreen 0 -screen-width $WIDTH -screen-height $HEIGHT -sw $WIDTH -sh $HEIGHT -cw 14400 -ch 4800 -wall $WALL -r $PART_ID -logfile log.txt" -s \"192.168.2.$LIP\" $REST & 
            
            sleep 1.5
        done
    done
fi
     
