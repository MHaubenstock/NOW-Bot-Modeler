import sys
from naoqi import ALProxy

ip = "169.254.226.148"
#ip = "10.224.0.80"
tts = ALProxy("ALTextToSpeech", ip , 9559)
tts.say("Hello, world.")

walker = ALProxy("ALMotion", ip, 9559)
tts.say("motion proxy set")

try:
    postureProxy = ALProxy("ALRobotPosture", ip, 9559)
except Exception, e:
    print "Could not create proxy to ALRobotPosture"
    print "Error was: ", e
else:
	tts.say("posture proxy set.")
		
tts.say("Hello, world.")

try:
	walker.moveto(0.1 , 0 , 0 )
except Exception, e:
    print "Could not walkto"
    print "Error was: ", e

raw_input('press any key to end')

