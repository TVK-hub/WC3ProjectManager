// cj_ifdebug.j
// v 0.1
//   by ScorpioT1000, 2009
//   powered by AdicHelper(cJass.xgm.ru)
//
//
// IFDEBUG(ARGUMENT) - with condition to all players
// IFDEBUG(ARGUMENT,RET) - with condition and return to all players
// IFDEBUGP(ARGUMENT,DEST_PLAYER) - with condition to one player
// IFDEBUGP(ARGUMENT,DEST_PLAYER,RET) - with condition and return to one player
//
// ================================================================================

#guard _IFDEBUG_J

library_once Ifdebug {

    #define private msgwinX = 1.5
    #define private msgwinY = -1.4
    #define private assert_color = "FF404040"

    nothing Ifdebug_debug_msg(string message, integer whichPlayer) {
    // -1 = for all
        integer i = 0
        if(whichPlayer >= 0 and whichPlayer <= 15) {
            DisplayTextToPlayer(Player(whichPlayer),msgwinX,msgwinY,"|c"+assert_color+"Debug: "+message+"|r")
        } elseif(whichPlayer == -1) {
            whilenot(i > 11) {
                DisplayTextToPlayer(Player(i),msgwinX,msgwinY,"|c"+assert_color+"Debug(all): "+message+"|r")
                i++
            }
        }
    }
    
    #define private DMSGFUNC = Ifdebug_debug_msg
    
    #define IFDEBUGF(ARG,DEST_PLAYER) = {
        if(ARG) {
            DMSGFUNC(`FUNCNAME` + ": " + `ARG`,DEST_PLAYER)
        }
    }
    
    #define IFDEBUGR(ARG,WHATRET,DEST_PLAYER) = {
        if(ARG) {
            DMSGFUNC(`FUNCNAME` + ": " + `ARG`,DEST_PLAYER)
            return WHATRET
        }
    }
    
    //overloading
    #define {
        IFDEBUG(ARGUMENT) = IFDEBUGF(ARGUMENT, -1)
        IFDEBUG(ARGUMENT,RET) = IFDEBUGR(ARGUMENT,RET, -1)
        IFDEBUGP(ARGUMENT,DEST_PLAYER) = IFDEBUGF(A,DEST_PLAYER)
        IFDEBUGP(ARGUMENT,DEST_PLAYER,RET) = IFDEBUGF(A,RET,DEST_PLAYER)
    }

}