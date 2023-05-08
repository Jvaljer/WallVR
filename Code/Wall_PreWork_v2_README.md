In the first version I did a lot of stuff without using much of Photon's features about ownership, for a really simple reason : 
a photon object can only have 1 owner, so how could I do when it's in-between 2 screens ? 

The aim of this second version is simply to try another way of work : 
	Creating 1 program launchable from 5 different computers, with 4 computers as players and 1 as operator 
	- each computer would have its own screen, visible only by itselfs (specific scene maybe)
	- the operator would be able to see all 4 screens 
	- operator will be able to drag n drop a shape all around his visibles screens 

    -> it seems to be the same as the v1 but in this one, the screens will be much more related to the photon entities, which might be a bit
    trickier for sure, but quite interesting tho.
