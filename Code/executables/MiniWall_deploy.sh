#!/bin/bash

# Define variables
PATH="/home/abel/JvaljerGit/WallVR/Code/executable/MiniWall_exe/"
EXEC="MiniWall.x86_64"
PART_NB=4

# Loop through each participant and launch the executable
for ((i=1; i<=$PART_NB; i++))
do
  "$PATH$EXEC" &
done

wait




