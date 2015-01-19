from naoqi import ALProxy

def InitialPosition():
    print ("Initial Position\n")
    global stance
    stance = 'stance1'
    my_motion.setStiffnesses("Body", 1.0)
    postureProxy.goToPosture("StandZero", 0.5)
    my_motion.post.setAngles("LArm", [0.8160460591316223, -0.28996795415878296, -1.1858240365982056, -1.5109480619430542, -1.2533199787139893, 0.12680000066757202], 0.2)
    my_motion.setAngles("RArm", [0.8191980123519897, 0.2960200309753418, 1.3529460430145264, 1.507964015007019, 1.2670420408248901, 0.17479999363422394], 0.2)

def DefensivePosition():
    print ("Defensive Position\n")
    global stance
    stance = 'stance2'
    my_motion.setStiffnesses("Body", 1.0)
    postureProxy.goToPosture("StandZero", 0.8)
    postureProxy.stopMove()
    my_motion.setAngles("Body", [-0.00771196186542511, -0.00771196186542511, 0.5307220220565796, -0.3141592741012573, -1.1950279474258423, -1.5446163415908813, -1.1474740505218506, 0.12640000879764557, -0.035240039229393005, 0.13963596522808075, -0.4371480345726013, -0.07674196362495422, 0.34357404708862305, -0.09353204071521759, -0.035240039229393005, -0.021434038877487183, 0.021434038877487183, -0.07359004020690918, -0.1165420413017273, 0.0920819640159607, 0.5860300064086914, 0.3141592741012573, 1.2440320253372192, 1.5446163415908813, 1.2440320253372192, 0.17560000717639923], 0.2)

def RightPunch():
    print ("Right Punch\n")
    my_motion.setAngles("RArm", [0.19946196675300598, 0.15796004235744476, 1.935866117477417, 0.0368579626083374, 1.1949440240859985, 0.17520000040531158], 0.8)
    my_motion.setStiffnesses("RArm", 0.8)
    raw_input("Enter")
    my_motion.setStiffnesses("RArm", 1.0)
    my_motion.setAngles("RArm", [0.8191980123519897, 0.2960200309753418, 1.3529460430145264, 1.507964015007019, 1.2670420408248901, 0.17479999363422394], 0.8)

def LeftPunch():
    print ("Left Punch\n")
    my_motion.setAngles("LArm", [0.16102804243564606, -0.28996795415878296, -1.5800620317459106, -0.050580039620399475, -1.4573420286178589, 0.12759999930858612], 0.8)
    my_motion.setStiffnesses("LArm", 0.8)
    raw_input("Enter")
    my_motion.setStiffnesses("LArm", 1.0)
    my_motion.setAngles("LArm", [0.8160460591316223, -0.28996795415878296, -1.1858240365982056, -1.5109480619430542, -1.2533199787139893, 0.12680000066757202], 0.2)

def RightPunchCrouch():
    print ("Crouch Right Punch\n")
    my_motion.setAngles("RArm", [-0.3297680616378784, -0.006177961826324463, 0.19631004333496094, 0.046061962842941284, -0.17338396608829498, 0.14920000731945038], 0.8)
    my_motion.setStiffnesses("RArm", 0.8)
    raw_input("Enter")
    my_motion.setStiffnesses("RArm", 1.0)
    my_motion.setAngles("RArm", [0.8191980123519897, 0.2960200309753418, 1.3529460430145264, 1.507964015007019, 1.2670420408248901, 0.17479999363422394], 0.8)

def LeftPunchCrouch():
    print ("Crouch Left Punch\n")
    my_motion.setAngles("LArm", [-0.3145119547843933, -0.09361596405506134, -0.0890139639377594, -0.05825003981590271, 0.17483404278755188, 0.034800004214048386], 0.8)
    my_motion.setStiffnesses("LArm", 0.8)
    raw_input("Enter")
    my_motion.setStiffnesses("LArm", 1.0)
    my_motion.setAngles("LArm", [0.8160460591316223, -0.28996795415878296, -1.1858240365982056, -1.5109480619430542, -1.2533199787139893, 0.12680000066757202], 0.4)

def Clap():
    print ("Clap \n")
    my_motion.post.setAngles("LArm", [-0.2715599536895752, 1.1596620082855225, 0.05364803969860077, -0.0643860399723053, -1.6061400175094604, 0.12600000202655792], 0.5)
    my_motion.setAngles("RArm", [0.3206479549407959, -1.1643480062484741, 0.6043540239334106, 0.05373196303844452, 1.2670420408248901, 0.18400000035762787], 0.5)
    my_motion.setStiffnesses("RArm", 0.8)
    my_motion.setStiffnesses("LArm", 0.8)
    raw_input("Enter")
    my_motion.setStiffnesses("RArm", 1.0)
    my_motion.setStiffnesses("LArm", 1.0)
    my_motion.post.setAngles("LArm", [-0.11202395707368851, -0.2654239535331726, 0.087396040558815, -0.07052204012870789, -1.6337519884109497, 0.12600000202655792], 0.8)
    my_motion.setAngles("RArm", [-0.07972604036331177, 0.29141804575920105, 0.308292031288147, 0.05679996311664581, 0.9909220933914185, 0.18639999628067017], 0.8)

def LeftSidePunch():
    print ("LeftSidePunch \n")
    my_motion.setAngles("LArm", [-0.2715599536895752, 1.1596620082855225, 0.05364803969860077, -0.0643860399723053, -1.6061400175094604, 0.12600000202655792], 0.8)
    my_motion.setStiffnesses("LArm", 0.8)
    raw_input("Enter")
    my_motion.setStiffnesses("LArm", 1.0)
    my_motion.setAngles("LArm", [0.8160460591316223, -0.28996795415878296, -1.1858240365982056, -1.5109480619430542, -1.2533199787139893, 0.12680000066757202], 0.2)

def LeftHook():
    print ("Left Hook\n")
    my_motion.setAngles("LArm", [1.3759560585021973, 0.41874003410339355, -1.4803520441055298, -1.478734016418457, -1.2732620239257812, 0.12680000066757202], 0.8)
    raw_input("Enter")
    my_motion.setAngles("LArm", [-0.10742195695638657, -0.05833396315574646, -0.33905595541000366, -0.09199804067611694, -1.2763299942016602, 0.12680000066757202], 1.0)
    my_motion.setStiffnesses("LArm", 0.8)
    raw_input("Enter")
    my_motion.setStiffnesses("LArm", 1.0)
    my_motion.setAngles("Body", [-0.00771196186542511, -0.00771196186542511, 0.5307220220565796, -0.3141592741012573, -1.1950279474258423, -1.5446163415908813, -1.1474740505218506, 0.12640000879764557, -0.035240039229393005, 0.13963596522808075, -0.4371480345726013, -0.07674196362495422, 0.34357404708862305, -0.09353204071521759, -0.035240039229393005, -0.021434038877487183, 0.021434038877487183, -0.07359004020690918, -0.1165420413017273, 0.0920819640159607, 0.5860300064086914, 0.3141592741012573, 1.2440320253372192, 1.5446163415908813, 1.2440320253372192, 0.17560000717639923], 0.2)

def Squad():
    print ("Squad\n")

    postureProxy.goToPosture("Crouch", 0.5)
    postureProxy.stopMove()
    # my_motion.setStiffnesses("Body", 1.0)
    my_motion.setStiffnesses("LKneePitch", 0.0)
    my_motion.setStiffnesses("RKneePitch", 0.0)
    raw_input("Enter")
    my_motion.post.setAngles("LArm", [0.8160460591316223, -0.28996795415878296, -1.1858240365982056, -1.5109480619430542, -1.2533199787139893, 0.12680000066757202], 0.2)
    my_motion.setAngles("RArm", [0.8191980123519897, 0.2960200309753418, 1.3529460430145264, 1.507964015007019, 1.2670420408248901, 0.17479999363422394], 0.2)

    global stance
    stance = 'stance3'

def StepBack():
    my_motion.setWalkTargetVelocity(-0.5, 0.0, 0.0, 0.9)
    raw_input("Enter")
    my_motion.stopMove()

def StepForward():
    my_motion.setWalkTargetVelocity(0.5, 0.0, 0.0, 0.9)
    raw_input("Enter")
    my_motion.stopMove()

def StepRight():
    my_motion.setWalkTargetVelocity(0.0, -0.5, 0.0, 0.9)
    raw_input("Enter")
    my_motion.stopMove()

def StepLeft():
    my_motion.setWalkTargetVelocity(0.0, 0.5, 0.0, 0.9)
    raw_input("Enter")
    my_motion.stopMove()

def TurnLeft():
    my_motion.setWalkTargetVelocity(0.0, 0.0, 0.5, 0.9)
    raw_input("Enter")
    my_motion.stopMove()

def TurnRight():
    my_motion.setWalkTargetVelocity(0.0, 0.0, -0.5, 0.9)
    raw_input("Enter")
    my_motion.stopMove()

stance1 = {'1': 'DefensivePosition',
           '3': 'RightPunch',
           '4': 'LeftPunch',
           '5': 'LeftSidePunch',
           's': 'StepBack',
           'w': 'StepForward',
           'a': 'StepLeft',
           'd': 'StepRight',
           'q': 'TurnLeft',
           'e': 'TurnRight',
           '2': 'Squad'
}

stance2 = {'1': 'InitialPosition',
           '2': 'Squad',
           '4': 'Clap',
           '3': 'LeftHook'
}

stance3 = {'1': 'InitialPosition',
           '2': 'DefensivePosition',
           '3': 'RightPunchCrouch',
           '4': 'LeftPunchCrouch'

}

IP = raw_input("please input the IP of the robot.\njust hit enter to connect to 169.254.226.148\n")

if IP == "":
    #IP = "169.254.226.148"
    IP = "192.168.0.1"
    print ("nothing entered, setting ip to: ", IP)

tts = ALProxy("ALTextToSpeech", IP, 9559)
postureProxy = ALProxy("ALRobotPosture", IP, 9559)
my_motion = ALProxy("ALMotion", IP, 9559)

my_motion.setStiffnesses("Body", 1.0)
postureProxy.goToPosture("StandZero", 0.8)

InitialPosition()

stance = 'stance1'

print("prompt user\n")
x = raw_input()

while( x != '-1'):
    if(x != ''):
        if(stance == 'stance1'):
            command = stance1[x] + "()"
        elif(stance == 'stance2'):
            command = stance2[x] + "()"
        elif(stance == 'stance3'):
            command = stance3[x] + "()"
        exec(command)
    x = raw_input("Next Move:")

my_motion.rest()
